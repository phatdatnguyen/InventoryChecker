using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace InventoryChecker.RegressionTests;

internal static class Program
{
    private static readonly string TestDirectory = Path.Combine(Path.GetTempPath(), "InventoryChecker-tests-" + Guid.NewGuid().ToString("N"));
    private static int fileNumber;

    [STAThread]
    private static int Main()
    {
        ApplicationConfiguration.Initialize();
        Directory.CreateDirectory(TestDirectory);
        (string Name, Action Run)[] tests =
        [
            ("CSV preserves identifiers, Unicode, quotes, line breaks, and spaces", CsvPreservesText),
            ("CSV accepts UTF-8 BOM and LF records", CsvEncodingAndLineEndings),
            ("CSV accepts Unicode BOMs and rejects damaged UTF-8 identifiers", CsvHandlesEncoding),
            ("CSV loads saved check states", CsvLoadsCheckStates),
            ("CSV rejects malformed schemas and rows", CsvRejectsMalformedInput),
            ("CSV exports round-trip every row and check state", CsvRoundTrip),
            ("Excel writes identifier text and boolean checks without formulas", ExcelPreservesCellTypes),
            ("Excel preserves escape-like text, control characters, and emoji", ExcelPreservesSpecialText),
            ("Failed exports preserve an existing destination and clean up temporary files", FailedExportPreservesDestination),
            ("Search handles single characters and literal expression syntax", SearchTreatsInputLiterally),
            ("Search escapes column names and clearing the search restores rows", SearchHandlesColumnNames),
            ("Scanning marks one item and reports repeats", ScanMarksAndDetectsRepeat),
            ("Scanning blank and unknown codes leaves inventory unchanged", ScanRejectsBlankAndUnknown),
            ("Ambiguous scans leave all duplicate rows unchanged", ScanRejectsDuplicates),
            ("Scanning honors case-sensitive matching", ScanHonorsCase),
            ("Case-insensitive scanning is ordinal under Turkish culture", ScanIsCultureIndependent),
            ("Scanner keeps focus and accepts consecutive scans without closing", ScannerFormContinuousInput),
            ("Inventory window lays out correctly and selects sorted rows", InventoryFormLayoutAndSelection),
        ];

        int failures = 0;
        try
        {
            foreach ((string name, Action run) in tests)
            {
                try
                {
                    run();
                    Console.WriteLine("PASS " + name);
                }
                catch (Exception exception)
                {
                    failures++;
                    Console.Error.WriteLine("FAIL " + name + Environment.NewLine + exception);
                }
            }
        }
        finally
        {
            Directory.Delete(TestDirectory, recursive: true);
        }

        Console.WriteLine($"{tests.Length - failures}/{tests.Length} regression checks passed.");
        return failures == 0 ? 0 : 1;
    }

    private static void CsvPreservesText()
    {
        using DataTable table = LoadCsv("Code,Description,Notes\r\n0000123,\"Cà phê, sữa\",\"She said \"\"hello\"\"\r\nnext line\"\r\n 0010 ,\" padded \",\r\n");
        Equal(2, table.Rows.Count);
        Equal("0000123", table.Rows[0]["Code"]);
        Equal("Cà phê, sữa", table.Rows[0]["Description"]);
        Equal("She said \"hello\"\r\nnext line", table.Rows[0]["Notes"]);
        Equal(" 0010 ", table.Rows[1]["Code"]);
        Equal(" padded ", table.Rows[1]["Description"]);
        Equal("", table.Rows[1]["Notes"]);
        Equal(typeof(string), table.Columns["Code"]!.DataType);
        Equal(typeof(bool), table.Columns[InventoryFile.CheckedColumnName]!.DataType);
        Assert(table.Rows.Cast<DataRow>().All(row => Equals(row[InventoryFile.CheckedColumnName], false)), "New inventories must start unchecked.");
    }

    private static void CsvEncodingAndLineEndings()
    {
        string path = NextPath(".csv");
        File.WriteAllText(path, "Code,Name\n001,Tiếng Việt\n002,\"line one\nline two\"", new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        using DataTable table = InventoryFile.LoadCsv(path);
        Equal("Code", table.Columns[0].ColumnName);
        Equal("Tiếng Việt", table.Rows[0]["Name"]);
        Equal("line one\nline two", table.Rows[1]["Name"]);
    }

    private static void CsvLoadsCheckStates()
    {
        using DataTable table = LoadCsv("Code,cHeCkEd\r\n001,true\r\n002,FALSE\r\n003,1\r\n004,0\r\n005,\r\n006, true \r\n");
        Equal(2, table.Columns.Count);
        Equal("Checked", table.Columns[1].ColumnName);
        bool[] expected = [true, false, true, false, false, true];
        for (int i = 0; i < expected.Length; i++)
            Equal(expected[i], table.Rows[i]["Checked"]);
    }

    private static void CsvHandlesEncoding()
    {
        string utf16Path = NextPath(".csv");
        File.WriteAllText(utf16Path, "Code,Name\r\n0001,Tiếng Việt\r\n", Encoding.Unicode);
        using DataTable table = InventoryFile.LoadCsv(utf16Path);
        Equal("0001", table.Rows[0]["Code"]);
        Equal("Tiếng Việt", table.Rows[0]["Name"]);

        string invalidPath = NextPath(".csv");
        File.WriteAllBytes(invalidPath, [.. Encoding.UTF8.GetBytes("Code\r\n00"), 0xc3, 0x28, 0x0d, 0x0a]);
        Throws(() => { using DataTable invalid = InventoryFile.LoadCsv(invalidPath); }, "Invalid UTF-8 bytes must not silently alter inventory identifiers.");
        File.WriteAllBytes(invalidPath, [0xef, 0xbb, 0xbf, .. Encoding.UTF8.GetBytes("Code\r\n00"), 0xc3, 0x28, 0x0d, 0x0a]);
        Throws(() => { using DataTable invalid = InventoryFile.LoadCsv(invalidPath); }, "A UTF-8 BOM must not disable invalid-byte detection.");
        File.WriteAllBytes(invalidPath, [0xff, 0xfe, .. Encoding.Unicode.GetBytes("Code\r\n00"), 0x00, 0xd8]);
        Throws(() => { using DataTable invalid = InventoryFile.LoadCsv(invalidPath); }, "Damaged UTF-16 identifiers must be rejected.");
    }

    private static void CsvRejectsMalformedInput()
    {
        string[] invalidCsv =
        [
            "",
            "Code,Name\r\n",
            "Code,code\r\n001,002\r\n",
            "Code,\r\n001,Item\r\n",
            "Code,   \r\n001,Item\r\n",
            "Code,Name\r\n001\r\n",
            "Code,Name\r\n001,Item,extra\r\n",
            "Code,Name\r\n001,\"unterminated\r\n",
            "Code,Name\r\n001,\"quoted\"suffix\r\n",
            "Code,Checked\r\n001,maybe\r\n",
            "Checked\r\ntrue\r\n",
        ];
        foreach (string csv in invalidCsv)
            Throws(() => { using DataTable table = LoadCsv(csv); }, "Malformed CSV must be rejected: " + csv.Replace("\r", "\\r").Replace("\n", "\\n"));
    }

    private static void CsvRoundTrip()
    {
        using DataTable original = LoadCsv("Code,Description\r\n00001,\"comma,quote\"\"and\r\nnewline\"\r\n00002,\"  spaces  \"\r\n00003,=1+1\r\n");
        original.Rows[0]["Checked"] = true;
        original.Rows[2]["Checked"] = true;
        original.DefaultView.RowFilter = "Code = '00001'";
        string path = NextPath(".csv");
        File.WriteAllText(path, "Existing file contents");
        InventoryFile.SaveCsv(original, path);
        using DataTable restored = InventoryFile.LoadCsv(path);
        Equal(original.Columns.Count, restored.Columns.Count);
        Equal(original.Rows.Count, restored.Rows.Count);
        for (int column = 0; column < original.Columns.Count; column++)
        {
            Equal(original.Columns[column].ColumnName, restored.Columns[column].ColumnName);
            Equal(original.Columns[column].DataType, restored.Columns[column].DataType);
        }
        for (int row = 0; row < original.Rows.Count; row++)
            for (int column = 0; column < original.Columns.Count; column++)
                Equal(original.Rows[row][column], restored.Rows[row][column]);
    }

    private static void ExcelPreservesCellTypes()
    {
        using DataTable table = LoadCsv("Code,Description\r\n000012345678901234567890,=SUM(1;2)\r\n00002,\" leading & <trailing> \"\r\n");
        table.Rows[0]["Checked"] = true;
        table.DefaultView.RowFilter = "Code = '00002'";
        string path = NextPath(".xlsx");
        InventoryFile.SaveExcel(table, path);
        using ZipArchive archive = ZipFile.OpenRead(path);
        string[] requiredEntries = ["[Content_Types].xml", "_rels/.rels", "xl/workbook.xml", "xl/_rels/workbook.xml.rels", "xl/worksheets/sheet1.xml"];
        foreach (string entry in requiredEntries)
            Assert(archive.GetEntry(entry) != null, "Missing workbook entry: " + entry);
        using Stream sheetStream = archive.GetEntry("xl/worksheets/sheet1.xml")!.Open();
        XDocument sheet = XDocument.Load(sheetStream);
        XNamespace spreadsheet = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        Equal(3, sheet.Descendants(spreadsheet + "row").Count());
        Assert(!sheet.Descendants(spreadsheet + "f").Any(), "Source values must never become spreadsheet formulas.");
        XElement Cell(string reference) => sheet.Descendants(spreadsheet + "row").Single(row => (string?)row.Attribute("r") == reference[1..]).Elements(spreadsheet + "c").ElementAt(reference[0] - 'A');
        Equal("inlineStr", (string?)Cell("A2").Attribute("t"));
        Equal("000012345678901234567890", Cell("A2").Descendants(spreadsheet + "t").Single().Value);
        Equal("inlineStr", (string?)Cell("B2").Attribute("t"));
        Equal("=SUM(1;2)", Cell("B2").Descendants(spreadsheet + "t").Single().Value);
        Equal(" leading & <trailing> ", Cell("B3").Descendants(spreadsheet + "t").Single().Value);
        Equal("preserve", (string?)Cell("B3").Descendants(spreadsheet + "t").Single().Attribute(XNamespace.Xml + "space"));
        Equal("b", (string?)Cell("C2").Attribute("t"));
        Equal("1", Cell("C2").Element(spreadsheet + "v")!.Value);
        Equal("b", (string?)Cell("C3").Attribute("t"));
        Equal("0", Cell("C3").Element(spreadsheet + "v")!.Value);
    }

    private static void FailedExportPreservesDestination()
    {
        using DataTable table = Inventory(new string('x', 32_768));
        string path = NextPath(".xlsx");
        byte[] originalContents = Encoding.UTF8.GetBytes("Do not lose this existing workbook");
        File.WriteAllBytes(path, originalContents);
        Throws(() => InventoryFile.SaveExcel(table, path), "Oversized spreadsheet cells must fail explicitly.");
        Assert(originalContents.SequenceEqual(File.ReadAllBytes(path)), "A failed export must leave the existing destination intact.");
        Assert(!Directory.EnumerateFiles(TestDirectory, ".inventory-*.tmp").Any(), "A failed export must clean up its temporary file.");

        string csvPath = NextPath(".csv");
        File.WriteAllBytes(csvPath, originalContents);
        table.Rows[0]["Code"] = "invalid UTF-16: \ud800";
        Throws(() => InventoryFile.SaveCsv(table, csvPath), "Invalid Unicode must fail explicitly rather than change the identifier.");
        Assert(originalContents.SequenceEqual(File.ReadAllBytes(csvPath)), "A failed CSV export must leave the existing destination intact.");
        Assert(!Directory.EnumerateFiles(TestDirectory, ".inventory-*.tmp").Any(), "A failed CSV export must clean up its temporary file.");
    }

    private static void ExcelPreservesSpecialText()
    {
        using DataTable table = Inventory("_x000D_", "a\u0001b", "📦 0001", "line1\r\nline2");
        string path = NextPath(".xlsx");
        InventoryFile.SaveExcel(table, path);
        using ZipArchive archive = ZipFile.OpenRead(path);
        using Stream stream = archive.GetEntry("xl/worksheets/sheet1.xml")!.Open();
        XDocument sheet = XDocument.Load(stream);
        XNamespace spreadsheet = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        string[] values = sheet.Descendants(spreadsheet + "row").Skip(1).Select(row => row.Elements(spreadsheet + "c").First().Descendants(spreadsheet + "t").Single().Value).ToArray();
        Equal("_x005F_x000D_", values[0]);
        Equal("a_x0001_b", values[1]);
        Equal("📦 0001", values[2]);
        Equal("line1\r\nline2", values[3]);
    }

    private static void SearchTreatsInputLiterally()
    {
        string[] keywords = ["x", "'", "%", "*", "[", "]", "\\", "_", "[%*]'\\", "x' OR 1=1 OR 'x' LIKE '"];
        foreach (string keyword in keywords)
        {
            using DataTable table = new();
            table.Columns.Add("Description", typeof(string));
            DataRow expected = table.Rows.Add("before " + keyword + " after");
            table.Rows.Add("nothing else");
            using DataView view = new(table) { RowFilter = InventoryOperations.BuildSearchFilter("Description", keyword) };
            Equal(1, view.Count);
            Assert(ReferenceEquals(expected, view[0].Row), "Search must match the literal keyword: " + keyword);
        }
    }

    private static void SearchHandlesColumnNames()
    {
        string[] names = ["Code", "Item name", "Quote'", "Bracket[", "Bracket]", "Back\\slash", "Mixed ]\\['%* name"];
        foreach (string name in names)
        {
            using DataTable table = new();
            table.Columns.Add(name, typeof(string));
            table.Rows.Add("0001");
            table.Rows.Add("0002");
            using DataView view = new(table) { RowFilter = InventoryOperations.BuildSearchFilter(name, "1") };
            Equal(1, view.Count);
            Equal("0001", view[0][name]);
            view.RowFilter = InventoryOperations.BuildSearchFilter(name, "");
            Equal(2, view.Count);
        }
    }

    private static void ScanMarksAndDetectsRepeat()
    {
        using DataTable table = Inventory("0001", "0002");
        ScanOutcome first = InventoryOperations.CheckItem(table, "Code", "0001", caseSensitive: false);
        Equal(ScanResult.Checked, first.Result);
        Equal(1, first.MatchCount);
        Assert(ReferenceEquals(table.Rows[0], first.Row), "Successful scan should identify the matched row.");
        Equal(true, table.Rows[0]["Checked"]);
        Equal(false, table.Rows[1]["Checked"]);
        ScanOutcome repeat = InventoryOperations.CheckItem(table, "Code", "0001", caseSensitive: false);
        Equal(ScanResult.AlreadyChecked, repeat.Result);
        Equal(1, repeat.MatchCount);
        Assert(ReferenceEquals(table.Rows[0], repeat.Row), "Repeat should identify the same row.");
        Equal(false, table.Rows[1]["Checked"]);
    }

    private static void ScanRejectsBlankAndUnknown()
    {
        using DataTable table = Inventory("", " ", "0001");
        foreach (string code in new[] { "", " ", "\t" })
        {
            ScanOutcome result = InventoryOperations.CheckItem(table, "Code", code, caseSensitive: true);
            Equal(ScanResult.EmptyCode, result.Result);
            Assert(result.Row == null, "Blank scans must not identify a row.");
        }
        ScanOutcome unknown = InventoryOperations.CheckItem(table, "Code", "missing", caseSensitive: false);
        Equal(ScanResult.NotFound, unknown.Result);
        Equal(0, unknown.MatchCount);
        Assert(unknown.Row == null, "Unknown codes must not identify a row.");
        AssertAllUnchecked(table);
    }

    private static void ScanRejectsDuplicates()
    {
        using DataTable table = Inventory("A001", "a001", "other");
        ScanOutcome duplicate = InventoryOperations.CheckItem(table, "Code", "A001", caseSensitive: false);
        Equal(ScanResult.Ambiguous, duplicate.Result);
        Equal(2, duplicate.MatchCount);
        Assert(duplicate.Row == null, "An ambiguous scan must not nominate one row.");
        AssertAllUnchecked(table);
        table.Rows[0]["Checked"] = true;
        ScanOutcome mixed = InventoryOperations.CheckItem(table, "Code", "a001", caseSensitive: false);
        Equal(ScanResult.Ambiguous, mixed.Result);
        Equal(false, table.Rows[1]["Checked"]);
        Equal(false, table.Rows[2]["Checked"]);

        using DataTable exactDuplicates = Inventory("same", "same");
        Equal(ScanResult.Ambiguous, InventoryOperations.CheckItem(exactDuplicates, "Code", "same", caseSensitive: true).Result);
        AssertAllUnchecked(exactDuplicates);
    }

    private static void ScanHonorsCase()
    {
        using DataTable table = Inventory("ABC", "abc");
        Equal(ScanResult.NotFound, InventoryOperations.CheckItem(table, "Code", "Abc", caseSensitive: true).Result);
        AssertAllUnchecked(table);
        ScanOutcome result = InventoryOperations.CheckItem(table, "Code", "abc", caseSensitive: true);
        Equal(ScanResult.Checked, result.Result);
        Equal(false, table.Rows[0]["Checked"]);
        Equal(true, table.Rows[1]["Checked"]);
    }

    private static void ScanIsCultureIndependent()
    {
        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        CultureInfo originalUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("tr-TR");
            using DataTable table = Inventory("FILE");
            Equal(ScanResult.Checked, InventoryOperations.CheckItem(table, "Code", "file", caseSensitive: false).Result);
            table.Rows[0]["Checked"] = false;
            Equal(ScanResult.NotFound, InventoryOperations.CheckItem(table, "Code", "fıle", caseSensitive: false).Result);
            AssertAllUnchecked(table);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }

    private static void ScannerFormContinuousInput()
    {
        using ScanCodeForm scanner = new() { IsCaseSensitive = true };
        Invoke(scanner, "OnShown", EventArgs.Empty);
        TextBox input = Field<TextBox>(scanner, "codeTextBox");
        CheckBox caseToggle = Field<CheckBox>(scanner, "caseSensitiveCheckBox");
        Label feedback = Field<Label>(scanner, "feedbackLabel");
        Assert(caseToggle.Checked, "The scanner must restore the case preference.");
        Assert(ReferenceEquals(scanner.ActiveControl, input), "The scanner must focus its input.");
        Assert(ReferenceEquals(scanner.AcceptButton, Field<Button>(scanner, "scanButton")), "Enter must submit a scan.");
        Assert(ReferenceEquals(scanner.CancelButton, Field<Button>(scanner, "closeButton")), "Escape must close the scanner.");
        scanner.ActiveControl = caseToggle;
        caseToggle.Checked = false;
        Assert(!scanner.IsCaseSensitive, "Changing the checkbox must retain the new preference.");
        Assert(ReferenceEquals(scanner.ActiveControl, input), "Changing case sensitivity must return focus to scanner input.");

        List<string?> submitted = [];
        scanner.CodeSubmitted += (_, _) =>
        {
            submitted.Add(scanner.Code);
            scanner.ShowFeedback("Checked " + scanner.Code, true);
        };
        input.Text = " \t ";
        Invoke(scanner, "ScanButton_Click", scanner, EventArgs.Empty);
        Equal(0, submitted.Count);
        Equal(DialogResult.None, scanner.DialogResult);
        Equal(Color.DarkRed, feedback.ForeColor);
        foreach (string code in new[] { "00123", "A&B", new string('9', 1000) })
        {
            input.Text = code;
            Invoke(scanner, "ScanButton_Click", scanner, EventArgs.Empty);
            Equal(code, submitted[^1]);
            Equal(string.Empty, input.Text);
            Assert(ReferenceEquals(scanner.ActiveControl, input), "The scanner must refocus after each code.");
            Equal(DialogResult.None, scanner.DialogResult);
            Equal("Checked " + code, feedback.Text);
            Equal(Color.DarkGreen, feedback.ForeColor);
        }
        Equal(3, submitted.Count);
        Assert(!feedback.UseMnemonic, "Ampersands in codes must display literally.");
        scanner.ShowFeedback("Unknown code. Scan again.", false);
        Equal(Color.DarkRed, feedback.ForeColor);
        Equal(DialogResult.None, scanner.DialogResult);

        using ScanCodeForm oneShot = new();
        Invoke(oneShot, "OnShown", EventArgs.Empty);
        Field<TextBox>(oneShot, "codeTextBox").Text = "007";
        Invoke(oneShot, "ScanButton_Click", oneShot, EventArgs.Empty);
        Equal("007", oneShot.Code);
        Equal(DialogResult.OK, oneShot.DialogResult);
    }

    private static void InventoryFormLayoutAndSelection()
    {
        using InventoryForm form = new() { ShowInTaskbar = false, Opacity = 0 };
        form.Show();
        Application.DoEvents();
        DataGridView grid = Field<DataGridView>(form, "inventoryDataGridView");
        Panel panel = Field<Panel>(form, "controlPanel");
        StatusStrip statusStrip = Field<StatusStrip>(form, "inventoryStatusStrip");
        ToolStripStatusLabel status = Field<ToolStripStatusLabel>(form, "inventoryStatusLabel");
        Assert((status.Text ?? string.Empty).Contains("Load a CSV", StringComparison.Ordinal), "The empty window should explain how to start.");
        Assert(!Field<GroupBox>(form, "searchGroupBox").Enabled, "Search must be disabled without inventory.");
        Assert(!Field<GroupBox>(form, "checkingGroupBox").Enabled, "Scanning must be disabled without inventory.");
        Assert(grid.ReadOnly && !grid.AllowUserToAddRows && !grid.AllowUserToDeleteRows, "The grid must protect imported inventory from edits.");
        Assert(panel.Bottom <= grid.Top && grid.Bottom <= statusStrip.Top, "Controls, inventory, and status must not overlap.");

        using DataTable table = Inventory("001", "003", "002");
        SetField(form, "inventoryTable", table);
        BindingSource source = Field<BindingSource>(form, "inventoryBindingSource");
        source.DataSource = table;
        grid.DataSource = source;
        source.Sort = "Code DESC";
        Application.DoEvents();
        Invoke(form, "SelectInventoryRow", table.Rows[0]);
        Assert(ReferenceEquals(((DataRowView)grid.CurrentRow!.DataBoundItem!).Row, table.Rows[0]), "Selecting a scanned item must respect the sorted display order.");
        source.Sort = "Checked DESC, Code DESC";
        table.Rows[0]["Checked"] = true;
        Invoke(form, "SelectInventoryRow", table.Rows[0]);
        Assert(ReferenceEquals(((DataRowView)source.Current!).Row, table.Rows[0]), "Selecting a scanned item must follow a row that moved after its check changed.");
        Assert(ReferenceEquals(((DataRowView)grid.CurrentRow!.DataBoundItem!).Row, table.Rows[0]), "Grid selection must follow the changed row's sorted position.");
        source.Filter = "Code = '001'";
        SetField(form, "hasUnsavedChanges", true);
        Invoke(form, "UpdateStatus");
        string statusText = status.Text ?? string.Empty;
        Assert(statusText.Contains("1 / 3 checked", StringComparison.Ordinal) && statusText.Contains("1 shown", StringComparison.Ordinal), "Status must distinguish total checks from filtered rows.");
        Assert(statusText.Contains("Unsaved", StringComparison.Ordinal) && form.Text.EndsWith('*'), "Changed results need a visible unsaved indicator.");
        SetField(form, "hasUnsavedChanges", false);
        form.Close();
        Assert(form.IsDisposed, "The inventory window must close cleanly.");
    }

    private static T Field<T>(object target, string name) => (T)target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(target)!;

    private static void SetField(object target, string name, object value) => target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(target, value);

    private static void Invoke(object target, string name, params object[] args) => target.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(target, args);

    private static DataTable Inventory(params string[] codes)
    {
        DataTable table = new();
        table.Columns.Add("Code", typeof(string));
        table.Columns.Add("Checked", typeof(bool)).DefaultValue = false;
        foreach (string code in codes)
            table.Rows.Add(code, false);
        return table;
    }

    private static DataTable LoadCsv(string contents)
    {
        string path = NextPath(".csv");
        File.WriteAllText(path, contents, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        return InventoryFile.LoadCsv(path);
    }

    private static string NextPath(string extension) => Path.Combine(TestDirectory, (++fileNumber).ToString(CultureInfo.InvariantCulture) + extension);

    private static void AssertAllUnchecked(DataTable table) => Assert(table.Rows.Cast<DataRow>().All(row => Equals(row["Checked"], false)), "Failed scans must leave every row unchecked.");

    private static void Equal(object? expected, object? actual)
    {
        if (!Equals(expected, actual))
            throw new InvalidOperationException($"Expected <{expected}> but found <{actual}>.");
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private static void Throws(Action action, string message)
    {
        try
        {
            action();
        }
        catch (Exception exception) when (exception is FormatException or InvalidDataException or ArgumentException)
        {
            return;
        }
        throw new InvalidOperationException(message);
    }
}
