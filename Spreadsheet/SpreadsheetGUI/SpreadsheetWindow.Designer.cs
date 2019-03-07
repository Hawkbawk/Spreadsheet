namespace SpreadsheetGUI
{
    partial class spreadsheetWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private CloseDialog cd;

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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.file_NewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.file_SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.file_OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.file_CloseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topPanel = new System.Windows.Forms.Panel();
            this.currentCell = new System.Windows.Forms.Label();
            this.textBoxOne = new System.Windows.Forms.TextBox();
            this.spreadsheetPanel_Panel = new System.Windows.Forms.Panel();
            this.spreadsheetPanelOne = new SSGui.SpreadsheetPanel();
            this.openSpreadsheetDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveSpreadsheetDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.spreadsheetPanel_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.helpMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1272, 28);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file_NewMenuItem,
            this.file_SaveMenuItem,
            this.file_OpenMenuItem,
            this.file_CloseMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileMenuItem.Text = "File";
            // 
            // file_NewMenuItem
            // 
            this.file_NewMenuItem.Name = "file_NewMenuItem";
            this.file_NewMenuItem.Size = new System.Drawing.Size(216, 26);
            this.file_NewMenuItem.Text = "New";
            this.file_NewMenuItem.Click += new System.EventHandler(this.New_Clicked);
            // 
            // file_SaveMenuItem
            // 
            this.file_SaveMenuItem.Name = "file_SaveMenuItem";
            this.file_SaveMenuItem.Size = new System.Drawing.Size(216, 26);
            this.file_SaveMenuItem.Text = "Save";
            this.file_SaveMenuItem.Click += new System.EventHandler(this.Save_Clicked);
            // 
            // file_OpenMenuItem
            // 
            this.file_OpenMenuItem.Name = "file_OpenMenuItem";
            this.file_OpenMenuItem.Size = new System.Drawing.Size(216, 26);
            this.file_OpenMenuItem.Text = "Open";
            this.file_OpenMenuItem.Click += new System.EventHandler(this.Open_Click);
            // 
            // file_CloseMenuItem
            // 
            this.file_CloseMenuItem.Name = "file_CloseMenuItem";
            this.file_CloseMenuItem.Size = new System.Drawing.Size(216, 26);
            this.file_CloseMenuItem.Text = "Close";
            this.file_CloseMenuItem.Click += new System.EventHandler(this.Close_Clicked);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpMenuItem.Text = "Help";
            this.helpMenuItem.Click += new System.EventHandler(this.Help_Clicked);
            // 
            // topPanel
            // 
            this.topPanel.AutoSize = true;
            this.topPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.topPanel.Controls.Add(this.currentCell);
            this.topPanel.Controls.Add(this.textBoxOne);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 28);
            this.topPanel.Margin = new System.Windows.Forms.Padding(4);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1272, 540);
            this.topPanel.TabIndex = 4;
            // 
            // currentCell
            // 
            this.currentCell.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.currentCell.AutoSize = true;
            this.currentCell.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentCell.Location = new System.Drawing.Point(16, 15);
            this.currentCell.Margin = new System.Windows.Forms.Padding(4);
            this.currentCell.Name = "currentCell";
            this.currentCell.Size = new System.Drawing.Size(171, 20);
            this.currentCell.TabIndex = 3;
            this.currentCell.Text = "Current Cell Contents";
            this.currentCell.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxOne
            // 
            this.textBoxOne.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxOne.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxOne.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.textBoxOne.Location = new System.Drawing.Point(216, 14);
            this.textBoxOne.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOne.Name = "textBoxOne";
            this.textBoxOne.Size = new System.Drawing.Size(413, 22);
            this.textBoxOne.TabIndex = 2;
            this.textBoxOne.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // spreadsheetPanel_Panel
            // 
            this.spreadsheetPanel_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel_Panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanel_Panel.Controls.Add(this.spreadsheetPanelOne);
            this.spreadsheetPanel_Panel.Location = new System.Drawing.Point(0, 82);
            this.spreadsheetPanel_Panel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spreadsheetPanel_Panel.Name = "spreadsheetPanel_Panel";
            this.spreadsheetPanel_Panel.Size = new System.Drawing.Size(1272, 485);
            this.spreadsheetPanel_Panel.TabIndex = 5;
            this.spreadsheetPanel_Panel.MouseEnter += new System.EventHandler(this.New_Clicked);
            // 
            // spreadsheetPanelOne
            // 
            this.spreadsheetPanelOne.AutoScroll = true;
            this.spreadsheetPanelOne.AutoSize = true;
            this.spreadsheetPanelOne.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanelOne.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.spreadsheetPanelOne.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanelOne.ForeColor = System.Drawing.SystemColors.Control;
            this.spreadsheetPanelOne.Location = new System.Drawing.Point(0, 0);
            this.spreadsheetPanelOne.Margin = new System.Windows.Forms.Padding(5);
            this.spreadsheetPanelOne.Name = "spreadsheetPanelOne";
            this.spreadsheetPanelOne.Size = new System.Drawing.Size(1272, 485);
            this.spreadsheetPanelOne.TabIndex = 0;
            this.spreadsheetPanelOne.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpreadsheetPanel_KeyDown);
            this.spreadsheetPanelOne.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpreadsheetPanel_KeyPressed);
            // 
            // openSpreadsheetDialog
            // 
            this.openSpreadsheetDialog.FileName = "openFileDialog1";
            // 
            // spreadsheetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 569);
            this.Controls.Add(this.spreadsheetPanel_Panel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "spreadsheetWindow";
            this.Text = "Spreadsheet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetForm_Closing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.spreadsheetPanel_Panel.ResumeLayout(false);
            this.spreadsheetPanel_Panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem file_NewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem file_OpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem file_SaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem file_CloseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label currentCell;
        private System.Windows.Forms.TextBox textBoxOne;
        public SSGui.SpreadsheetPanel spreadsheetPanelOne;
        private System.Windows.Forms.Panel spreadsheetPanel_Panel;
        private System.Windows.Forms.OpenFileDialog openSpreadsheetDialog;
        private System.Windows.Forms.SaveFileDialog saveSpreadsheetDialog;
    }
}

