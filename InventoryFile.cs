using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Xml;
using Microsoft.VisualBasic.FileIO;

namespace InventoryChecker;

internal static class InventoryFile
{
    internal const string CheckedColumnName = "Checked";

    private const string SpreadsheetNamespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    private const string RelationshipsNamespace = "http://schemas.openxmlformats.org/package/2006/relationships";
    private const string DocumentRelationshipsNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

    internal static DataTable LoadCsv(string path)
    {
        // Own the stream before constructing the parser: its constructor can read and
        // throw on invalid bytes, before a parser using-scope could dispose the file.
        using var stream = File.OpenRead(path);
        Encoding encoding = ReadEncoding(stream);
        using var parser = new TextFieldParser(stream, encoding, detectEncoding: false, leaveOpen: true)
        {
            TextFieldType = FieldType.Delimited,
            HasFieldsEnclosedInQuotes = true,
            TrimWhiteSpace = false
        };
        parser.SetDelimiters(",");

        if (parser.EndOfData)
            throw new InvalidDataException("The CSV file is empty.");

        string[] headers = ReadFields(parser);
        var headerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        int checkedIndex = -1;
        var table = new DataTable("Inventory") { Locale = CultureInfo.InvariantCulture };
        try
        {
            for (int index = 0; index < headers.Length; index++)
            {
                string header = headers[index];
                if (string.IsNullOrWhiteSpace(header))
                    throw new InvalidDataException($"Column {index + 1} has an empty name.");
                if (!headerNames.Add(header))
                    throw new InvalidDataException($"The CSV contains duplicate column name '{header}'.");

                bool isChecked = string.Equals(header, CheckedColumnName, StringComparison.OrdinalIgnoreCase);
                if (isChecked)
                    checkedIndex = index;
                table.Columns.Add(new DataColumn(isChecked ? CheckedColumnName : header, isChecked ? typeof(bool) : typeof(string))
                {
                    AllowDBNull = false,
                    DefaultValue = isChecked ? (object)false : string.Empty
                });
            }

            if (headers.Length == 0 || (headers.Length == 1 && checkedIndex == 0))
                throw new InvalidDataException("The CSV must contain at least one inventory column besides Checked.");

            if (checkedIndex < 0)
            {
                table.Columns.Add(new DataColumn(CheckedColumnName, typeof(bool))
                {
                    AllowDBNull = false,
                    DefaultValue = false
                });
            }

            int recordNumber = 1;
            while (!parser.EndOfData)
            {
                string[] fields = ReadFields(parser);
                recordNumber++;
                if (fields.Length != headers.Length)
                    throw new InvalidDataException($"CSV record {recordNumber} has {fields.Length} fields; expected {headers.Length}.");

                DataRow row = table.NewRow();
                for (int index = 0; index < fields.Length; index++)
                    row[index] = index == checkedIndex ? ParseChecked(fields[index], recordNumber) : fields[index];
                table.Rows.Add(row);
            }

            if (table.Rows.Count == 0)
                throw new InvalidDataException("The CSV contains column names but no inventory records.");

            table.AcceptChanges();
            return table;
        }
        catch
        {
            table.Dispose();
            throw;
        }
    }

    private static Encoding ReadEncoding(Stream stream)
    {
        Span<byte> prefix = stackalloc byte[4];
        int count = stream.ReadAtLeast(prefix, prefix.Length, throwOnEndOfStream: false);
        int preambleLength = 0;
        Encoding encoding = new UTF8Encoding(false, true);

        // StreamReader's automatic BOM detection uses replacement fallbacks for some
        // encodings. Select strict decoders ourselves so damaged codes never change silently.
        if (count >= 4 && prefix[0] == 0xFF && prefix[1] == 0xFE && prefix[2] == 0 && prefix[3] == 0)
        {
            encoding = new UTF32Encoding(false, false, true);
            preambleLength = 4;
        }
        else if (count >= 4 && prefix[0] == 0 && prefix[1] == 0 && prefix[2] == 0xFE && prefix[3] == 0xFF)
        {
            encoding = new UTF32Encoding(true, false, true);
            preambleLength = 4;
        }
        else if (count >= 3 && prefix[0] == 0xEF && prefix[1] == 0xBB && prefix[2] == 0xBF)
        {
            preambleLength = 3;
        }
        else if (count >= 2 && prefix[0] == 0xFF && prefix[1] == 0xFE)
        {
            encoding = new UnicodeEncoding(false, false, true);
            preambleLength = 2;
        }
        else if (count >= 2 && prefix[0] == 0xFE && prefix[1] == 0xFF)
        {
            encoding = new UnicodeEncoding(true, false, true);
            preambleLength = 2;
        }

        stream.Position = preambleLength;
        return encoding;
    }

    private static string[] ReadFields(TextFieldParser parser)
    {
        try
        {
            return parser.ReadFields() ?? throw new InvalidDataException("The CSV is missing a record.");
        }
        catch (MalformedLineException exception)
        {
            throw new InvalidDataException($"Invalid CSV quoting near line {exception.LineNumber}.", exception);
        }
    }

    private static bool ParseChecked(string value, int recordNumber)
    {
        string flag = value.Trim();
        if (flag.Length == 0 || flag == "0")
            return false;
        if (flag == "1")
            return true;
        if (bool.TryParse(flag, out bool result))
            return result;

        throw new InvalidDataException($"CSV record {recordNumber} has invalid Checked value '{value}'. Use true, false, 1, 0, or an empty field.");
    }

    internal static void SaveCsv(DataTable table, string path)
    {
        ArgumentNullException.ThrowIfNull(table);
        if (table.Columns.Count == 0)
            throw new InvalidDataException("There are no inventory columns to export.");

        WriteAtomically(path, stream =>
        {
            using var writer = new StreamWriter(stream, new UTF8Encoding(true, true), leaveOpen: true);
            WriteCsvRecord(writer, table.Columns.Cast<DataColumn>().Select(column => column.ColumnName));
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                    WriteCsvRecord(writer, table.Columns.Cast<DataColumn>().Select(column => CellText(row[column])));
            }
        });
    }

    private static void WriteCsvRecord(TextWriter writer, IEnumerable<string> values)
    {
        bool first = true;
        foreach (string value in values)
        {
            if (!first)
                writer.Write(',');
            // Quoting every field preserves whitespace, commas, quotes, and line breaks.
            writer.Write('"');
            writer.Write(value.Replace("\"", "\"\"", StringComparison.Ordinal));
            writer.Write('"');
            first = false;
        }
        writer.WriteLine();
    }

    internal static void SaveExcel(DataTable table, string path)
    {
        ArgumentNullException.ThrowIfNull(table);
        DataRow[] rows = table.Rows.Cast<DataRow>().Where(row => row.RowState != DataRowState.Deleted).ToArray();
        if (table.Columns.Count == 0 || table.Columns.Count > 16_384 || rows.Length > 1_048_575)
            throw new InvalidDataException("An Excel export needs 1 to 16,384 columns and can contain at most 1,048,575 inventory records.");

        WriteAtomically(path, stream =>
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
            WritePackageParts(archive);
            WriteXmlEntry(archive, "xl/worksheets/sheet1.xml", writer =>
            {
                writer.WriteStartElement("worksheet", SpreadsheetNamespace);
                writer.WriteStartElement("sheetData", SpreadsheetNamespace);
                writer.WriteStartElement("row", SpreadsheetNamespace);
                writer.WriteAttributeString("r", "1");
                foreach (DataColumn column in table.Columns)
                    WriteStringCell(writer, column.ColumnName);
                writer.WriteEndElement();

                for (int index = 0; index < rows.Length; index++)
                {
                    writer.WriteStartElement("row", SpreadsheetNamespace);
                    writer.WriteAttributeString("r", (index + 2).ToString(CultureInfo.InvariantCulture));
                    foreach (DataColumn column in table.Columns)
                    {
                        object value = rows[index][column];
                        if (column.DataType == typeof(bool) && string.Equals(column.ColumnName, CheckedColumnName, StringComparison.OrdinalIgnoreCase))
                        {
                            writer.WriteStartElement("c", SpreadsheetNamespace);
                            writer.WriteAttributeString("t", "b");
                            writer.WriteElementString("v", SpreadsheetNamespace, value is true ? "1" : "0");
                            writer.WriteEndElement();
                        }
                        else
                        {
                            // Inline strings keep leading zeroes and formula-like text literal.
                            WriteStringCell(writer, CellText(value));
                        }
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            });
        });
    }

    private static void WritePackageParts(ZipArchive archive)
    {
        WriteXmlEntry(archive, "[Content_Types].xml", writer =>
        {
            const string ns = "http://schemas.openxmlformats.org/package/2006/content-types";
            writer.WriteStartElement("Types", ns);
            WriteAttributesElement(writer, "Default", ns, ("Extension", "rels"), ("ContentType", "application/vnd.openxmlformats-package.relationships+xml"));
            WriteAttributesElement(writer, "Default", ns, ("Extension", "xml"), ("ContentType", "application/xml"));
            WriteAttributesElement(writer, "Override", ns, ("PartName", "/xl/workbook.xml"), ("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"));
            WriteAttributesElement(writer, "Override", ns, ("PartName", "/xl/worksheets/sheet1.xml"), ("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"));
            writer.WriteEndElement();
        });
        WriteXmlEntry(archive, "_rels/.rels", writer =>
        {
            writer.WriteStartElement("Relationships", RelationshipsNamespace);
            WriteAttributesElement(writer, "Relationship", RelationshipsNamespace, ("Id", "rId1"), ("Type", DocumentRelationshipsNamespace + "/officeDocument"), ("Target", "xl/workbook.xml"));
            writer.WriteEndElement();
        });
        WriteXmlEntry(archive, "xl/workbook.xml", writer =>
        {
            writer.WriteStartElement("workbook", SpreadsheetNamespace);
            writer.WriteStartElement("sheets", SpreadsheetNamespace);
            writer.WriteStartElement("sheet", SpreadsheetNamespace);
            writer.WriteAttributeString("name", "Inventory");
            writer.WriteAttributeString("sheetId", "1");
            writer.WriteAttributeString("r", "id", DocumentRelationshipsNamespace, "rId1");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        });
        WriteXmlEntry(archive, "xl/_rels/workbook.xml.rels", writer =>
        {
            writer.WriteStartElement("Relationships", RelationshipsNamespace);
            WriteAttributesElement(writer, "Relationship", RelationshipsNamespace, ("Id", "rId1"), ("Type", DocumentRelationshipsNamespace + "/worksheet"), ("Target", "worksheets/sheet1.xml"));
            writer.WriteEndElement();
        });
    }

    private static void WriteAttributesElement(XmlWriter writer, string name, string ns, params (string Name, string Value)[] attributes)
    {
        writer.WriteStartElement(name, ns);
        foreach ((string attributeName, string value) in attributes)
            writer.WriteAttributeString(attributeName, value);
        writer.WriteEndElement();
    }

    private static void WriteXmlEntry(ZipArchive archive, string name, Action<XmlWriter> write)
    {
        using Stream stream = archive.CreateEntry(name, CompressionLevel.Optimal).Open();
        using var writer = XmlWriter.Create(stream, new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false, true),
            NewLineHandling = NewLineHandling.Entitize
        });
        writer.WriteStartDocument();
        write(writer);
        writer.WriteEndDocument();
    }

    private static void WriteStringCell(XmlWriter writer, string value)
    {
        if (value.Length > 32_767)
            throw new InvalidDataException("An inventory value exceeds Excel's limit of 32,767 characters per cell. Save as CSV to preserve it.");
        writer.WriteStartElement("c", SpreadsheetNamespace);
        writer.WriteAttributeString("t", "inlineStr");
        writer.WriteStartElement("is", SpreadsheetNamespace);
        writer.WriteStartElement("t", SpreadsheetNamespace);
        writer.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");
        writer.WriteString(EscapeSpreadsheetText(value));
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
    }

    private static string EscapeSpreadsheetText(string value)
    {
        // SpreadsheetML interprets _xHHHH_ sequences as escaped UTF-16 code units.
        // Escape literal occurrences and XML-invalid characters without dropping data.
        var result = new StringBuilder(value.Length);
        for (int index = 0; index < value.Length; index++)
        {
            char character = value[index];
            if (character == '_' && index + 6 < value.Length && value[index + 1] == 'x' && value[index + 6] == '_'
                && value.AsSpan(index + 2, 4).IndexOfAnyExcept("0123456789abcdefABCDEF") < 0)
            {
                result.Append("_x005F_");
            }
            else if (char.IsHighSurrogate(character) && index + 1 < value.Length && char.IsLowSurrogate(value[index + 1]))
            {
                result.Append(character).Append(value[++index]);
            }
            else if (!XmlConvert.IsXmlChar(character))
            {
                result.Append("_x").Append(((int)character).ToString("X4", CultureInfo.InvariantCulture)).Append('_');
            }
            else
            {
                result.Append(character);
            }
        }
        return result.ToString();
    }

    private static string CellText(object value) => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;

    private static void WriteAtomically(string path, Action<Stream> write)
    {
        string fullPath = Path.GetFullPath(path);
        string directory = Path.GetDirectoryName(fullPath)!;
        string temporaryPath = Path.Combine(directory, ".inventory-" + Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                write(stream);
                stream.Flush(flushToDisk: true);
            }
            File.Move(temporaryPath, fullPath, overwrite: true);
        }
        finally
        {
            try
            {
                File.Delete(temporaryPath);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
}
