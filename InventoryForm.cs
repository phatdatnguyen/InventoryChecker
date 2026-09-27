using System.Data;

namespace InventoryChecker;

public partial class InventoryForm : Form
{
    private DataTable? inventoryTable;
    private string? sourceFileName;
    private bool hasUnsavedChanges;
    private bool isCaseSensitive;

    public InventoryForm()
    {
        InitializeComponent();
        UpdateStatus();
    }

    private void ButtonLoadData_Click(object sender, EventArgs e)
    {
        if (loadDataOpenFileDialog.ShowDialog(this) != DialogResult.OK)
            return;

        // Save first: the export may overwrite the same CSV the user is about to reload.
        if (!ConfirmSaveChanges())
            return;

        DataTable loadedTable;
        try
        {
            // Validate separately, preserving the current session on any import failure.
            loadedTable = InventoryFile.LoadCsv(loadDataOpenFileDialog.FileName);
        }
        catch (Exception exception)
        {
            ShowError("Could not load inventory", exception);
            return;
        }

        DataTable? previousTable = inventoryTable;
        inventoryTable = loadedTable;
        sourceFileName = loadDataOpenFileDialog.FileName;
        hasUnsavedChanges = false;
        keywordTextBox.Clear();
        inventoryBindingSource.RemoveFilter();
        inventoryBindingSource.Sort = string.Empty;
        inventoryBindingSource.DataSource = inventoryTable;
        inventoryDataGridView.DataSource = inventoryBindingSource;
        previousTable?.Dispose();

        searchFieldComboBox.Items.Clear();
        checkingFieldComboBox.Items.Clear();
        foreach (DataColumn column in inventoryTable.Columns)
        {
            if (column.ColumnName == InventoryFile.CheckedColumnName)
                continue;
            searchFieldComboBox.Items.Add(column.ColumnName);
            checkingFieldComboBox.Items.Add(column.ColumnName);
        }

        searchFieldComboBox.SelectedIndex = 0;
        checkingFieldComboBox.SelectedIndex = 0;
        foreach (string field in checkingFieldComboBox.Items)
        {
            if (string.Equals(field, "Code", StringComparison.OrdinalIgnoreCase))
            {
                checkingFieldComboBox.SelectedItem = field;
                break;
            }
        }

        searchGroupBox.Enabled = true;
        checkingGroupBox.Enabled = true;
        UpdateStatus();
    }

    private void KeywordTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;
        e.SuppressKeyPress = true;
        SearchButton_Click(searchButton, EventArgs.Empty);
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        if (inventoryTable == null || searchFieldComboBox.SelectedItem is not string field)
            return;

        inventoryBindingSource.Filter = InventoryOperations.BuildSearchFilter(field, keywordTextBox.Text);
        UpdateStatus();
    }

    private void ScanCodeButton_Click(object sender, EventArgs e)
    {
        if (inventoryTable == null || checkingFieldComboBox.SelectedItem is not string field)
            return;

        keywordTextBox.Clear();
        inventoryBindingSource.RemoveFilter();
        UpdateStatus();
        using var scanner = new ScanCodeForm { IsCaseSensitive = isCaseSensitive };
        scanner.CodeSubmitted += (_, _) =>
        {
            ScanOutcome outcome = InventoryOperations.CheckItem(inventoryTable, field, scanner.Code ?? string.Empty, scanner.IsCaseSensitive);
            string message;
            switch (outcome.Result)
            {
                case ScanResult.Checked:
                    hasUnsavedChanges = true;
                    message = $"Checked: {scanner.Code}";
                    break;
                case ScanResult.AlreadyChecked:
                    message = $"Already checked: {scanner.Code}";
                    break;
                case ScanResult.Ambiguous:
                    message = $"{outcome.MatchCount} rows match this code. Nothing changed. Close this window and select a unique checking field.";
                    break;
                case ScanResult.EmptyCode:
                    message = "Enter or scan a code.";
                    break;
                default:
                    message = $"Not found: {scanner.Code}. Ready for another scan.";
                    break;
            }

            if (outcome.Row != null)
                SelectInventoryRow(outcome.Row);
            scanner.ShowFeedback(message, outcome.Result == ScanResult.Checked);
            UpdateStatus();
        };
        scanner.ShowDialog(this);
        isCaseSensitive = scanner.IsCaseSensitive;
    }

    private void SelectInventoryRow(DataRow row)
    {
        // Binding order may differ from table order after a user sorts a column.
        for (int index = 0; index < inventoryBindingSource.Count; index++)
        {
            if (inventoryBindingSource[index] is DataRowView view && ReferenceEquals(view.Row, row))
            {
                inventoryBindingSource.Position = index;
                if (inventoryDataGridView.Columns.Count > 0)
                    inventoryDataGridView.CurrentCell = inventoryDataGridView.Rows[index].Cells[0];
                break;
            }
        }
    }

    private void ExportButton_Click(object sender, EventArgs e) => ExportInventory();

    private bool ExportInventory()
    {
        if (inventoryTable == null)
            return false;

        using var dialog = new SaveFileDialog
        {
            Title = "Export all inventory rows",
            Filter = "Excel workbook (*.xlsx)|*.xlsx|CSV inventory (*.csv)|*.csv",
            DefaultExt = "xlsx",
            AddExtension = true,
            OverwritePrompt = true,
            FileName = Path.GetFileNameWithoutExtension(sourceFileName) + "-checked"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return false;

        try
        {
            UseWaitCursor = true;
            string extension = Path.GetExtension(dialog.FileName);
            if (string.Equals(extension, ".csv", StringComparison.OrdinalIgnoreCase))
                InventoryFile.SaveCsv(inventoryTable, dialog.FileName);
            else if (string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase))
                InventoryFile.SaveExcel(inventoryTable, dialog.FileName);
            else
                throw new InvalidDataException("Choose a .csv or .xlsx filename.");

            hasUnsavedChanges = false;
            UpdateStatus();
            inventoryStatusLabel.Text += $"   Saved: {Path.GetFileName(dialog.FileName)}";
            return true;
        }
        catch (Exception exception)
        {
            ShowError("Could not export inventory", exception);
            return false;
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    private bool ConfirmSaveChanges()
    {
        if (!hasUnsavedChanges)
            return true;

        DialogResult choice = MessageBox.Show(this,
            "Export your checked results before continuing?\n\nChoose Cancel to keep working, or No to discard unsaved checks.",
            "Unsaved checks", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        return choice == DialogResult.No || (choice == DialogResult.Yes && ExportInventory());
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        if (!e.Cancel && !ConfirmSaveChanges())
            e.Cancel = true;
    }

    private void UpdateStatus()
    {
        Text = "Inventory Checker" + (sourceFileName == null ? string.Empty : $" — {Path.GetFileName(sourceFileName)}")
            + (hasUnsavedChanges ? " *" : string.Empty);
        if (inventoryTable == null)
        {
            inventoryStatusLabel.Text = "Load a CSV inventory to begin.";
            return;
        }

        int checkedCount = inventoryTable.AsEnumerable().Count(row => row.Field<bool>(InventoryFile.CheckedColumnName));
        inventoryStatusLabel.Text = $"{checkedCount} / {inventoryTable.Rows.Count} checked   |   {inventoryBindingSource.Count} shown"
            + (hasUnsavedChanges ? "   |   Unsaved checks" : string.Empty);
    }

    private void ShowError(string title, Exception exception) =>
        MessageBox.Show(this, exception.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
}
