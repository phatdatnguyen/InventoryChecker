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
            this.promptLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // promptLabel
            // 
            this.promptLabel.AutoSize = true;
            this.promptLabel.ForeColor = System.Drawing.Color.White;
            this.promptLabel.Location = new System.Drawing.Point(37, 39);
            this.promptLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(158, 40);
            this.promptLabel.TabIndex = 0;
            this.promptLabel.Text = "Item Checked!\r\nScanned value: 0000";
            this.promptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ItemCheckedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(240, 120);
            this.Controls.Add(this.promptLabel);
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

        private System.Windows.Forms.Label promptLabel;
    }
}