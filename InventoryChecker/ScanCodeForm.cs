using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryChecker
{
    public partial class ScanCodeForm : Form
    {
        //Properties
        public string Code { get; set; }

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

        private void codeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                scanButton_Click(scanButton, EventArgs.Empty);
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            Code = codeTextBox.Text;
            codeTextBox.Clear();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
