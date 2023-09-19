namespace InventoryChecker
{
    public partial class ScanCodeForm : Form
    {
        //Properties
        public string? Code { get; set; }
        public bool IsCaseSensitive { get; set; }

        //Contructor
        public ScanCodeForm()
        {
            InitializeComponent();
        }

        //Event handlers
        private void ScanCodeForm_Load(object sender, EventArgs e)
        {
            codeTextBox.Select();
        }

        private void CodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ScanButton_Click(scanButton, EventArgs.Empty);
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            Code = codeTextBox.Text;
            IsCaseSensitive = caseSensitiveCheckBox.Checked;
            codeTextBox.Clear();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
