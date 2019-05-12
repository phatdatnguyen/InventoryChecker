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
    public partial class TableSelectForm : Form
    {
        //Property
        public string Selection
        {
            get
            {
                if (tablesListBox.SelectedIndex == -1)
                    return "";
                else
                    return tablesListBox.SelectedItem as string;
            }
        }

        //Contructor
        public TableSelectForm(string[] tables)
        {
            InitializeComponent();

            tablesListBox.DataSource = tables;
        }

        //Method
        private void tablesListBox_DoubleClick(object sender, EventArgs e)
        {
            if (tablesListBox.SelectedIndex != -1)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
