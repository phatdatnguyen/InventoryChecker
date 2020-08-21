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
    public partial class ItemCheckedForm : Form
    {
        public string ScannedValue { get; set; }
        public ItemCheckedForm()
        {
            InitializeComponent();
        }

        private void ItemCheckedForm_Load(object sender, EventArgs e)
        {
            promptScannedValue.Text = ScannedValue;
        }
    }
}
