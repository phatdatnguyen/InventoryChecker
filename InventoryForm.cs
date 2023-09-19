using System.Data;
using Accord.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace InventoryChecker
{
    public partial class InventoryForm : Form
    {
        //Fields
        private readonly ScanCodeForm scanCodeForm = new();
        private readonly ItemCheckedForm itemCheckedForm = new();
        private DataTable? inventoryTable;
        //Contructor
        public InventoryForm()
        {
            InitializeComponent();
        }

        //Event handlers
        private void ButtonLoadData_Click(object sender, EventArgs e)
        {
            if (loadDataOpenFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                string filename = loadDataOpenFileDialog.FileName;
                string extension = Path.GetExtension(filename);
                if (extension == ".csv")
                {
                    CsvReader csvReader = new(filename, true);
                    inventoryTable = csvReader.ToTable();
                }
                else
                {
                    throw new Exception("Cannot open the selected file!");
                }

                if (inventoryTable.Columns.Count == 0)
                    throw new Exception("Empty database!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            searchFieldComboBox.Items.Clear();
            foreach (DataColumn dataColumn in inventoryTable.Columns)
                searchFieldComboBox.Items.Add(dataColumn.ColumnName);
            searchFieldComboBox.SelectedIndex = 0;

            checkingFieldComboBox.Items.Clear();
            foreach (DataColumn dataColumn in inventoryTable.Columns)
                checkingFieldComboBox.Items.Add(dataColumn.ColumnName);
            foreach (string item in checkingFieldComboBox.Items)
            {
                if (item == "Code")
                {
                    checkingFieldComboBox.Text = "Code";
                    break;
                }
                checkingFieldComboBox.SelectedIndex = 0;
            }

            inventoryTable.Columns.Add("Checked", typeof(bool));

            inventoryBindingSource.DataSource = inventoryTable;
            inventoryDataGridView.DataSource = null;
            inventoryDataGridView.DataSource = inventoryBindingSource;

            searchGroupBox.Enabled = true;
            checkingGroupBox.Enabled = true;
        }

        private void KeywordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (inventoryTable != null && e.KeyCode == Keys.Enter)
                SearchButton_Click(searchButton, EventArgs.Empty);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (inventoryTable == null || keywordTextBox.Text.Length == 1)
                return;

            if (keywordTextBox.Text.Length == 0)
                inventoryBindingSource.Filter = "";

            inventoryBindingSource.Filter = "[" + searchFieldComboBox.SelectedItem.ToString() + "] LIKE '%" + keywordTextBox.Text + "%'";
        }

        private void ScanCodeButton_Click(object sender, EventArgs e)
        {
            if (inventoryTable == null)
                return;

            inventoryBindingSource.Filter = "";

            while (true)
            {
                bool itemFound = false;
                if (scanCodeForm.ShowDialog(this) != DialogResult.OK)
                    break;

                string checkingFieldName = checkingFieldComboBox.Text;

                foreach (DataRow dataRow in inventoryTable.Rows)
                    if (scanCodeForm.IsCaseSensitive)
                    {
                        if (dataRow[checkingFieldName].ToString() == scanCodeForm.Code)
                        {
                            itemFound = true;
                            dataRow["Checked"] = true;
                            break;
                        }
                    }
                    else
                    {
                        if (scanCodeForm.Code != null && ((string)dataRow[checkingFieldName]).ToLower() == scanCodeForm.Code.ToLower())
                        {
                            itemFound = true;
                            dataRow["Checked"] = true;
                            break;
                        }
                    }

                if (itemFound)
                {
                    Task.Delay(2000).ContinueWith((t) =>
                    {
                        if (itemCheckedForm == null || itemCheckedForm.Disposing || itemCheckedForm.IsDisposed)
                            return;

                        itemCheckedForm.Invoke(new Action(() => { itemCheckedForm.Close(); }));
                    });

                    itemCheckedForm.ScannedValue = scanCodeForm.Code;
                    itemCheckedForm.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show(this, "Cannot find the item in the inventory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                CopyInventoryToClipboard();

                Excel.Application application;
                Excel.Workbook workBook;
                Excel.Worksheet workSheet;
                application = new Excel.Application
                {
                    Visible = true
                };
                workBook = application.Workbooks.Add(System.Reflection.Missing.Value);
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);
                Excel.Range range = (Excel.Range)workSheet.Cells[1, 1];
                range.Select();
                workSheet.PasteSpecial(range, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(application);

                Clipboard.Clear();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
            }
        }

        private void CopyInventoryToClipboard()
        {
            inventoryDataGridView.SelectAll();
            DataObject dataObj = inventoryDataGridView.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
            inventoryDataGridView.ClearSelection();
        }

        private static void ReleaseObject(object? obj)
        {
            try
            {
                if (obj != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception occurred while releasing object: " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
