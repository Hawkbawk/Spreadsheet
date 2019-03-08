namespace SpreadsheetGUI
{
    partial class SendHelpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendHelpForm));
            this.Help_Text = new System.Windows.Forms.Label();
            this.OK_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Help_Text
            // 
            this.Help_Text.AutoSize = true;
            this.Help_Text.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Help_Text.ForeColor = System.Drawing.Color.Black;
            this.Help_Text.Location = new System.Drawing.Point(37, 9);
            this.Help_Text.Name = "Help_Text";
            this.Help_Text.Size = new System.Drawing.Size(726, 220);
            this.Help_Text.TabIndex = 0;
            this.Help_Text.Text = resources.GetString("Help_Text.Text");
            // 
            // OK_Button
            // 
            this.OK_Button.BackColor = System.Drawing.SystemColors.GrayText;
            this.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OK_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK_Button.ForeColor = System.Drawing.Color.LightGray;
            this.OK_Button.Location = new System.Drawing.Point(314, 241);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(159, 35);
            this.OK_Button.TabIndex = 1;
            this.OK_Button.Text = "Ok, I understand.";
            this.OK_Button.UseVisualStyleBackColor = false;
            this.OK_Button.Click += new System.EventHandler(this.button1_Click);
            // 
            // SendHelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(800, 288);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.Help_Text);
            this.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendHelpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SendHelpForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Help_Text;
        private System.Windows.Forms.Button OK_Button;
    }
}