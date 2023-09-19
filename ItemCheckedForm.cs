namespace InventoryChecker
{
    public partial class ItemCheckedForm : Form
    {
        public string? ScannedValue { get; set; }
        public ItemCheckedForm()
        {
            InitializeComponent();
        }

        private void ItemCheckedForm_Load(object sender, EventArgs e)
        {
            if (ScannedValue != null)
                promptScannedValue.Text = ScannedValue;
            else
                promptScannedValue.Text = "";
        }
    }
}
