namespace InventoryChecker
{
    partial class ScanCodeForm
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
            this.scanButton = new System.Windows.Forms.Button();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.closeButton = new System.Windows.Forms.Button();
            this.guideLabel = new System.Windows.Forms.Label();
            this.caseSensitiveCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // scanButton
            // 
            this.scanButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.scanButton.Location = new System.Drawing.Point(197, 154);
            this.scanButton.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(100, 30);
            this.scanButton.TabIndex = 13;
            this.scanButton.Text = "Scan";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(13, 115);
            this.codeTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(396, 26);
            this.codeTextBox.TabIndex = 10;
            this.toolTip.SetToolTip(this.codeTextBox, "Place the cursor here before scanning");
            this.codeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.codeTextBox_KeyDown);
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(309, 154);
            this.closeButton.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(100, 30);
            this.closeButton.TabIndex = 14;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // guideLabel
            // 
            this.guideLabel.AutoSize = true;
            this.guideLabel.ForeColor = System.Drawing.Color.Red;
            this.guideLabel.Location = new System.Drawing.Point(12, 9);
            this.guideLabel.Name = "guideLabel";
            this.guideLabel.Size = new System.Drawing.Size(392, 60);
            this.guideLabel.TabIndex = 0;
            this.guideLabel.Text = "● Place the cursor in the textbox before scanning\r\n● Scan the code by using a sca" +
    "nner or typing manually\r\n● Code scanning is case-insensitive by default";
            // 
            // caseSensitiveCheckBox
            // 
            this.caseSensitiveCheckBox.AutoSize = true;
            this.caseSensitiveCheckBox.Location = new System.Drawing.Point(13, 158);
            this.caseSensitiveCheckBox.Name = "caseSensitiveCheckBox";
            this.caseSensitiveCheckBox.Size = new System.Drawing.Size(131, 24);
            this.caseSensitiveCheckBox.TabIndex = 15;
            this.caseSensitiveCheckBox.Text = "Case-sensitive";
            this.caseSensitiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // ScanCodeForm
            // 
            this.AcceptButton = this.scanButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(424, 201);
            this.Controls.Add(this.caseSensitiveCheckBox);
            this.Controls.Add(this.guideLabel);
            this.Controls.Add(this.scanButton);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.closeButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScanCodeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scan Code";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label guideLabel;
        private System.Windows.Forms.CheckBox caseSensitiveCheckBox;
    }
}