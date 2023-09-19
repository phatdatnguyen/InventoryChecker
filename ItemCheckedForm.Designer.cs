namespace InventoryChecker
{
    partial class ItemCheckedForm
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
            promptScannedValue = new Label();
            promptItemChecked = new Label();
            SuspendLayout();
            // 
            // promptScannedValue
            // 
            promptScannedValue.Dock = DockStyle.Bottom;
            promptScannedValue.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Regular, GraphicsUnit.Point);
            promptScannedValue.ForeColor = Color.White;
            promptScannedValue.Location = new Point(0, 66);
            promptScannedValue.Margin = new Padding(4, 0, 4, 0);
            promptScannedValue.Name = "promptScannedValue";
            promptScannedValue.Size = new Size(300, 112);
            promptScannedValue.TabIndex = 0;
            promptScannedValue.Text = "0000";
            promptScannedValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // promptItemChecked
            // 
            promptItemChecked.AutoSize = true;
            promptItemChecked.ForeColor = Color.White;
            promptItemChecked.Location = new Point(94, 30);
            promptItemChecked.Name = "promptItemChecked";
            promptItemChecked.Size = new Size(109, 20);
            promptItemChecked.TabIndex = 0;
            promptItemChecked.Text = "Item checked!\r\n";
            promptItemChecked.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ItemCheckedForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(300, 178);
            Controls.Add(promptItemChecked);
            Controls.Add(promptScannedValue);
            Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ItemCheckedForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ItemCheckedForm";
            Load += ItemCheckedForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label promptScannedValue;
        private Label promptItemChecked;
    }
}