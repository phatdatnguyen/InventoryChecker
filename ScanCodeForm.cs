namespace InventoryChecker
{
    public partial class ScanCodeForm : Form
    {
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string? Code { get; set; }
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool IsCaseSensitive { get; set; }
        public event EventHandler? CodeSubmitted;

        public ScanCodeForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Code = null;
            caseSensitiveCheckBox.Checked = IsCaseSensitive;
            codeTextBox.Clear();
            codeTextBox.Select();
        }

        public void ShowFeedback(string message, bool success)
        {
            feedbackLabel.Text = message;
            feedbackLabel.ForeColor = success ? Color.DarkGreen : Color.DarkRed;
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(codeTextBox.Text))
            {
                ShowFeedback("Enter or scan a code before continuing.", false);
                codeTextBox.Clear();
                codeTextBox.Select();
                return;
            }

            Code = codeTextBox.Text;
            IsCaseSensitive = caseSensitiveCheckBox.Checked;

            if (CodeSubmitted == null)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            CodeSubmitted(this, EventArgs.Empty);
            codeTextBox.Clear();
            codeTextBox.Select();
        }

        private void CaseSensitiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            IsCaseSensitive = caseSensitiveCheckBox.Checked;
            codeTextBox.Select();
        }
    }
}
