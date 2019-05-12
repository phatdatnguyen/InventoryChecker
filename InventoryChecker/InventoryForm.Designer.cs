namespace InventoryChecker
{
    partial class InventoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryForm));
            this.inventoryDataGridView = new System.Windows.Forms.DataGridView();
            this.searchGroupBox = new System.Windows.Forms.GroupBox();
            this.searchFieldLabel = new System.Windows.Forms.Label();
            this.searchFieldComboBox = new System.Windows.Forms.ComboBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.keywordTextBox = new System.Windows.Forms.TextBox();
            this.scanCodeButton = new System.Windows.Forms.Button();
            this.loadDataOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.exportButton = new System.Windows.Forms.Button();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.checkingGroupBox = new System.Windows.Forms.GroupBox();
            this.checkingFieldLabel = new System.Windows.Forms.Label();
            this.checkingFieldComboBox = new System.Windows.Forms.ComboBox();
            this.buttonLoadData = new System.Windows.Forms.Button();
            this.inventoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDataGridView)).BeginInit();
            this.searchGroupBox.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.checkingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // inventoryDataGridView
            // 
            this.inventoryDataGridView.AllowUserToAddRows = false;
            this.inventoryDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.inventoryDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.inventoryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inventoryDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inventoryDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.inventoryDataGridView.Location = new System.Drawing.Point(0, 123);
            this.inventoryDataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.inventoryDataGridView.Name = "inventoryDataGridView";
            this.inventoryDataGridView.RowTemplate.Height = 30;
            this.inventoryDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.inventoryDataGridView.Size = new System.Drawing.Size(982, 448);
            this.inventoryDataGridView.TabIndex = 1;
            // 
            // searchGroupBox
            // 
            this.searchGroupBox.Controls.Add(this.searchFieldLabel);
            this.searchGroupBox.Controls.Add(this.searchFieldComboBox);
            this.searchGroupBox.Controls.Add(this.searchButton);
            this.searchGroupBox.Controls.Add(this.keywordTextBox);
            this.searchGroupBox.Location = new System.Drawing.Point(114, 12);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(562, 100);
            this.searchGroupBox.TabIndex = 1;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "Search";
            // 
            // searchFieldLabel
            // 
            this.searchFieldLabel.AutoSize = true;
            this.searchFieldLabel.Location = new System.Drawing.Point(6, 28);
            this.searchFieldLabel.Name = "searchFieldLabel";
            this.searchFieldLabel.Size = new System.Drawing.Size(54, 25);
            this.searchFieldLabel.TabIndex = 3;
            this.searchFieldLabel.Text = "Field";
            // 
            // searchFieldComboBox
            // 
            this.searchFieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.searchFieldComboBox.FormattingEnabled = true;
            this.searchFieldComboBox.Location = new System.Drawing.Point(55, 25);
            this.searchFieldComboBox.Name = "searchFieldComboBox";
            this.searchFieldComboBox.Size = new System.Drawing.Size(219, 33);
            this.searchFieldComboBox.TabIndex = 0;
            // 
            // searchButton
            // 
            this.searchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.searchButton.Location = new System.Drawing.Point(447, 61);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(100, 30);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = "&Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // keywordTextBox
            // 
            this.keywordTextBox.Location = new System.Drawing.Point(10, 63);
            this.keywordTextBox.MaxLength = 50;
            this.keywordTextBox.Name = "keywordTextBox";
            this.keywordTextBox.Size = new System.Drawing.Size(431, 30);
            this.keywordTextBox.TabIndex = 1;
            this.keywordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeywordTextBox_KeyDown);
            // 
            // scanCodeButton
            // 
            this.scanCodeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.scanCodeButton.Location = new System.Drawing.Point(6, 59);
            this.scanCodeButton.Name = "scanCodeButton";
            this.scanCodeButton.Size = new System.Drawing.Size(100, 35);
            this.scanCodeButton.TabIndex = 1;
            this.scanCodeButton.Text = "S&can Code";
            this.scanCodeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.scanCodeButton, "Scan QR code for chemical");
            this.scanCodeButton.UseVisualStyleBackColor = true;
            this.scanCodeButton.Click += new System.EventHandler(this.ScanCodeButton_Click);
            // 
            // loadDataOpenFileDialog
            // 
            this.loadDataOpenFileDialog.FileName = "*.xlsx";
            this.loadDataOpenFileDialog.Filter = "Excel (*.xls, *.xlsx)|*.xls;*.xlsx|CSV (*.csv)|*.csv";
            // 
            // exportButton
            // 
            this.exportButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exportButton.Location = new System.Drawing.Point(112, 59);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(172, 35);
            this.exportButton.TabIndex = 2;
            this.exportButton.Text = "&Export to MS Excel";
            this.exportButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.exportButton, "Export checking result to Microsoft Excel");
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.checkingGroupBox);
            this.controlPanel.Controls.Add(this.buttonLoadData);
            this.controlPanel.Controls.Add(this.searchGroupBox);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(982, 123);
            this.controlPanel.TabIndex = 0;
            // 
            // checkingGroupBox
            // 
            this.checkingGroupBox.Controls.Add(this.exportButton);
            this.checkingGroupBox.Controls.Add(this.checkingFieldLabel);
            this.checkingGroupBox.Controls.Add(this.scanCodeButton);
            this.checkingGroupBox.Controls.Add(this.checkingFieldComboBox);
            this.checkingGroupBox.Location = new System.Drawing.Point(682, 12);
            this.checkingGroupBox.Name = "checkingGroupBox";
            this.checkingGroupBox.Size = new System.Drawing.Size(290, 100);
            this.checkingGroupBox.TabIndex = 2;
            this.checkingGroupBox.TabStop = false;
            this.checkingGroupBox.Text = "Checking";
            // 
            // checkingFieldLabel
            // 
            this.checkingFieldLabel.AutoSize = true;
            this.checkingFieldLabel.Location = new System.Drawing.Point(16, 28);
            this.checkingFieldLabel.Name = "checkingFieldLabel";
            this.checkingFieldLabel.Size = new System.Drawing.Size(54, 25);
            this.checkingFieldLabel.TabIndex = 0;
            this.checkingFieldLabel.Text = "Field";
            // 
            // checkingFieldComboBox
            // 
            this.checkingFieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.checkingFieldComboBox.FormattingEnabled = true;
            this.checkingFieldComboBox.Location = new System.Drawing.Point(65, 25);
            this.checkingFieldComboBox.Name = "checkingFieldComboBox";
            this.checkingFieldComboBox.Size = new System.Drawing.Size(219, 33);
            this.checkingFieldComboBox.TabIndex = 0;
            // 
            // buttonLoadData
            // 
            this.buttonLoadData.Location = new System.Drawing.Point(8, 12);
            this.buttonLoadData.Name = "buttonLoadData";
            this.buttonLoadData.Size = new System.Drawing.Size(100, 100);
            this.buttonLoadData.TabIndex = 0;
            this.buttonLoadData.Text = "&Load Data";
            this.buttonLoadData.UseVisualStyleBackColor = true;
            this.buttonLoadData.Click += new System.EventHandler(this.ButtonLoadData_Click);
            // 
            // InventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 571);
            this.Controls.Add(this.inventoryDataGridView);
            this.Controls.Add(this.controlPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(1000, 618);
            this.MinimumSize = new System.Drawing.Size(1000, 618);
            this.Name = "InventoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventory Checker";
            this.Load += new System.EventHandler(this.InventoryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDataGridView)).EndInit();
            this.searchGroupBox.ResumeLayout(false);
            this.searchGroupBox.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.checkingGroupBox.ResumeLayout(false);
            this.checkingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView inventoryDataGridView;
        private System.Windows.Forms.GroupBox searchGroupBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox keywordTextBox;
        private System.Windows.Forms.OpenFileDialog loadDataOpenFileDialog;
        private System.Windows.Forms.Label searchFieldLabel;
        private System.Windows.Forms.ComboBox searchFieldComboBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button scanCodeButton;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button buttonLoadData;
        private System.Windows.Forms.BindingSource inventoryBindingSource;
        private System.Windows.Forms.GroupBox checkingGroupBox;
        private System.Windows.Forms.Label checkingFieldLabel;
        private System.Windows.Forms.ComboBox checkingFieldComboBox;
        private System.Windows.Forms.Button exportButton;
    }
}

