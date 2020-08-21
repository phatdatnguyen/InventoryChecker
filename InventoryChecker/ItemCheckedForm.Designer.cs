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
            this.promptScannedValue = new System.Windows.Forms.Label();
            this.promptItemChecked = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // promptScannedValue
            // 
            this.promptScannedValue.AutoSize = true;
            this.promptScannedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.promptScannedValue.ForeColor = System.Drawing.Color.White;
            this.promptScannedValue.Location = new System.Drawing.Point(109, 99);
            this.promptScannedValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.promptScannedValue.Name = "promptScannedValue";
            this.promptScannedValue.Size = new System.Drawing.Size(89, 37);
            this.promptScannedValue.TabIndex = 0;
            this.promptScannedValue.Text = "0000";
            this.promptScannedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // promptItemChecked
            // 
            this.promptItemChecked.AutoSize = true;
            this.promptItemChecked.ForeColor = System.Drawing.Color.White;
            this.promptItemChecked.Location = new System.Drawing.Point(94, 30);
            this.promptItemChecked.Name = "promptItemChecked";
            this.promptItemChecked.Size = new System.Drawing.Size(118, 40);
            this.promptItemChecked.TabIndex = 0;
            this.promptItemChecked.Text = "Item checked!\r\nScanned value:";
            this.promptItemChecked.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ItemCheckedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(300, 178);
            this.Controls.Add(this.promptItemChecked);
            this.Controls.Add(this.promptScannedValue);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemCheckedForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ItemCheckedForm";
            this.Load += new System.EventHandler(this.ItemCheckedForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label promptScannedValue;
        private System.Windows.Forms.Label promptItemChecked;
    }
}