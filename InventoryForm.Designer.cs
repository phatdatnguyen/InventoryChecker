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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryForm));
            inventoryDataGridView = new DataGridView();
            searchGroupBox = new GroupBox();
            searchFieldLabel = new Label();
            searchFieldComboBox = new ComboBox();
            searchButton = new Button();
            keywordTextBox = new TextBox();
            scanCodeButton = new Button();
            loadDataOpenFileDialog = new OpenFileDialog();
            toolTip = new ToolTip(components);
            exportButton = new Button();
            controlPanel = new Panel();
            checkingGroupBox = new GroupBox();
            checkingFieldLabel = new Label();
            checkingFieldComboBox = new ComboBox();
            buttonLoadData = new Button();
            inventoryBindingSource = new BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)inventoryDataGridView).BeginInit();
            searchGroupBox.SuspendLayout();
            controlPanel.SuspendLayout();
            checkingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)inventoryBindingSource).BeginInit();
            SuspendLayout();
            // 
            // inventoryDataGridView
            // 
            inventoryDataGridView.AllowUserToAddRows = false;
            inventoryDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(224, 224, 224);
            inventoryDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            inventoryDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            inventoryDataGridView.Dock = DockStyle.Fill;
            inventoryDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            inventoryDataGridView.Location = new Point(0, 123);
            inventoryDataGridView.Margin = new Padding(4, 3, 4, 3);
            inventoryDataGridView.Name = "inventoryDataGridView";
            inventoryDataGridView.RowHeadersVisible = false;
            inventoryDataGridView.RowTemplate.Height = 30;
            inventoryDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            inventoryDataGridView.Size = new Size(984, 456);
            inventoryDataGridView.TabIndex = 1;
            // 
            // searchGroupBox
            // 
            searchGroupBox.Controls.Add(searchFieldLabel);
            searchGroupBox.Controls.Add(searchFieldComboBox);
            searchGroupBox.Controls.Add(searchButton);
            searchGroupBox.Controls.Add(keywordTextBox);
            searchGroupBox.Enabled = false;
            searchGroupBox.Location = new Point(114, 12);
            searchGroupBox.Name = "searchGroupBox";
            searchGroupBox.Size = new Size(562, 100);
            searchGroupBox.TabIndex = 1;
            searchGroupBox.TabStop = false;
            searchGroupBox.Text = "Search";
            // 
            // searchFieldLabel
            // 
            searchFieldLabel.AutoSize = true;
            searchFieldLabel.Location = new Point(6, 28);
            searchFieldLabel.Name = "searchFieldLabel";
            searchFieldLabel.Size = new Size(43, 20);
            searchFieldLabel.TabIndex = 3;
            searchFieldLabel.Text = "Field";
            // 
            // searchFieldComboBox
            // 
            searchFieldComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            searchFieldComboBox.FormattingEnabled = true;
            searchFieldComboBox.Location = new Point(55, 25);
            searchFieldComboBox.Name = "searchFieldComboBox";
            searchFieldComboBox.Size = new Size(219, 28);
            searchFieldComboBox.TabIndex = 0;
            // 
            // searchButton
            // 
            searchButton.BackgroundImageLayout = ImageLayout.Center;
            searchButton.Location = new Point(447, 61);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(100, 30);
            searchButton.TabIndex = 2;
            searchButton.Text = "&Search";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += SearchButton_Click;
            // 
            // keywordTextBox
            // 
            keywordTextBox.Location = new Point(10, 63);
            keywordTextBox.MaxLength = 50;
            keywordTextBox.Name = "keywordTextBox";
            keywordTextBox.Size = new Size(431, 26);
            keywordTextBox.TabIndex = 1;
            keywordTextBox.KeyDown += KeywordTextBox_KeyDown;
            // 
            // scanCodeButton
            // 
            scanCodeButton.Cursor = Cursors.Hand;
            scanCodeButton.Location = new Point(6, 59);
            scanCodeButton.Name = "scanCodeButton";
            scanCodeButton.Size = new Size(100, 35);
            scanCodeButton.TabIndex = 1;
            scanCodeButton.Text = "S&can Code";
            scanCodeButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip.SetToolTip(scanCodeButton, "Scan QR code for chemical");
            scanCodeButton.UseVisualStyleBackColor = true;
            scanCodeButton.Click += ScanCodeButton_Click;
            // 
            // loadDataOpenFileDialog
            // 
            loadDataOpenFileDialog.FileName = "*.csv";
            loadDataOpenFileDialog.Filter = "CSV (*.csv)|*.csv";
            // 
            // exportButton
            // 
            exportButton.Cursor = Cursors.Hand;
            exportButton.Location = new Point(112, 59);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(172, 35);
            exportButton.TabIndex = 2;
            exportButton.Text = "&Export to MS Excel";
            exportButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip.SetToolTip(exportButton, "Export checking result to Microsoft Excel");
            exportButton.UseVisualStyleBackColor = true;
            exportButton.Click += ExportButton_Click;
            // 
            // controlPanel
            // 
            controlPanel.Controls.Add(checkingGroupBox);
            controlPanel.Controls.Add(buttonLoadData);
            controlPanel.Controls.Add(searchGroupBox);
            controlPanel.Dock = DockStyle.Top;
            controlPanel.Location = new Point(0, 0);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new Size(984, 123);
            controlPanel.TabIndex = 0;
            // 
            // checkingGroupBox
            // 
            checkingGroupBox.Controls.Add(exportButton);
            checkingGroupBox.Controls.Add(checkingFieldLabel);
            checkingGroupBox.Controls.Add(scanCodeButton);
            checkingGroupBox.Controls.Add(checkingFieldComboBox);
            checkingGroupBox.Enabled = false;
            checkingGroupBox.Location = new Point(682, 12);
            checkingGroupBox.Name = "checkingGroupBox";
            checkingGroupBox.Size = new Size(290, 100);
            checkingGroupBox.TabIndex = 2;
            checkingGroupBox.TabStop = false;
            checkingGroupBox.Text = "Checking";
            // 
            // checkingFieldLabel
            // 
            checkingFieldLabel.AutoSize = true;
            checkingFieldLabel.Location = new Point(16, 28);
            checkingFieldLabel.Name = "checkingFieldLabel";
            checkingFieldLabel.Size = new Size(43, 20);
            checkingFieldLabel.TabIndex = 0;
            checkingFieldLabel.Text = "Field";
            // 
            // checkingFieldComboBox
            // 
            checkingFieldComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            checkingFieldComboBox.FormattingEnabled = true;
            checkingFieldComboBox.Location = new Point(65, 25);
            checkingFieldComboBox.Name = "checkingFieldComboBox";
            checkingFieldComboBox.Size = new Size(219, 28);
            checkingFieldComboBox.TabIndex = 0;
            // 
            // buttonLoadData
            // 
            buttonLoadData.Location = new Point(8, 12);
            buttonLoadData.Name = "buttonLoadData";
            buttonLoadData.Size = new Size(100, 100);
            buttonLoadData.TabIndex = 0;
            buttonLoadData.Text = "&Load Data";
            buttonLoadData.UseVisualStyleBackColor = true;
            buttonLoadData.Click += ButtonLoadData_Click;
            // 
            // InventoryForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 579);
            Controls.Add(inventoryDataGridView);
            Controls.Add(controlPanel);
            Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MaximumSize = new Size(1000, 618);
            MinimumSize = new Size(1000, 618);
            Name = "InventoryForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inventory Checker";
            ((System.ComponentModel.ISupportInitialize)inventoryDataGridView).EndInit();
            searchGroupBox.ResumeLayout(false);
            searchGroupBox.PerformLayout();
            controlPanel.ResumeLayout(false);
            checkingGroupBox.ResumeLayout(false);
            checkingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)inventoryBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView inventoryDataGridView;
        private GroupBox searchGroupBox;
        private Button searchButton;
        private TextBox keywordTextBox;
        private OpenFileDialog loadDataOpenFileDialog;
        private Label searchFieldLabel;
        private ComboBox searchFieldComboBox;
        private ToolTip toolTip;
        private Button scanCodeButton;
        private Panel controlPanel;
        private Button buttonLoadData;
        private BindingSource inventoryBindingSource;
        private GroupBox checkingGroupBox;
        private Label checkingFieldLabel;
        private ComboBox checkingFieldComboBox;
        private Button exportButton;
    }
}

