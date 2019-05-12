using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace InventoryChecker
{
    public partial class InventoryForm : Form
    {
        //Fields
        private ScanCodeForm scanCodeForm;
        private ItemCheckedForm ItemCheckedForm;
        private DataTable inventoryTable;
        private bool dataLoaded;
        //Contructor
        public InventoryForm()
        {
            InitializeComponent();
        }

        //Event handlers
        private void InventoryForm_Load(object sender, EventArgs e)
        {
            scanCodeForm = new ScanCodeForm();
            ItemCheckedForm = new ItemCheckedForm();
        }

        private void ButtonLoadData_Click(object sender, EventArgs e)
        {
            if (loadDataOpenFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                string filename = loadDataOpenFileDialog.FileName;
                string extension = Path.GetExtension(filename);
                if (extension == ".xls" || extension == ".xlsx")
                {
                    ExcelReader excelReader = new ExcelReader(filename, true, false);
                    TableSelectForm tableSelectForm = new TableSelectForm(excelReader.GetWorksheetList());

                    if (tableSelectForm.ShowDialog(this) == DialogResult.OK && tableSelectForm.Selection != "")
                        inventoryTable = excelReader.GetWorksheet(tableSelectForm.Selection);
                    else
                        return;
                }
                else if (extension == ".csv")
                {
                    CsvReader csvReader = new CsvReader(filename, true);
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
            inventoryDataGridView.DataSource = inventoryBindingSource;

            dataLoaded = true;
        }

        private void KeywordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataLoaded && e.KeyCode == Keys.Enter)
                SearchButton_Click(searchButton, null);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (!dataLoaded)
                return;

            if (keywordTextBox.Text.Length == 0)
                inventoryBindingSource.Filter = "";

            if (keywordTextBox.Text.Length == 1)
                return;
            
            inventoryBindingSource.Filter = "[" + searchFieldComboBox.SelectedItem.ToString() + "] LIKE '%" + keywordTextBox.Text + "%'";
        }
        
        private void ScanCodeButton_Click(object sender, EventArgs e)
        {
            if (!dataLoaded)
                return;

            inventoryBindingSource.Filter = "";

            while (true)
            {
                bool itemFound = false;
                if (scanCodeForm.ShowDialog(this) != DialogResult.OK)
                    break;

                string checkingFieldName = checkingFieldComboBox.SelectedItem.ToString();

                foreach (DataRow dataRow in inventoryTable.Rows)
                    if (dataRow[checkingFieldName].ToString() == scanCodeForm.Code)
                    {
                        itemFound = true;
                        dataRow["Checked"] = true;
                        break;
                    }

                if (itemFound)
                {
                    Task.Delay(2000).ContinueWith((t) =>
                    {
                        if (ItemCheckedForm == null || ItemCheckedForm.Disposing || ItemCheckedForm.IsDisposed)
                            return;

                        ItemCheckedForm.Invoke(new Action(() => { ItemCheckedForm.Close(); }));
                    });

                    ItemCheckedForm.ShowDialog(this);
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
            CopyAllToClipboard();

            Excel.Application application;
            Excel.Workbook workBook;
            Excel.Worksheet workSheet;
            application = new Excel.Application();
            application.Visible = true;
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

        private void CopyAllToClipboard()
        {
            inventoryDataGridView.SelectAll();
            DataObject dataObj = inventoryDataGridView.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
            inventoryDataGridView.ClearSelection();
        }

        private void ReleaseObject(object obj)
        {
            try
            {
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
