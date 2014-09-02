namespace ClinicsProgram
{
    internal partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem importDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromZipExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromMongoDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFromXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToPDF;
        private System.Windows.Forms.ToolStripMenuItem exportToToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportToJSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.importDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromZipExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromMongoDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToPDF = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.menuStrip1.Font = new System.Drawing.Font("Bookman Old Style", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importDataToolStripMenuItem,
            this.exportDataToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(682, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // importDataToolStripMenuItem
            // 
            this.importDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importFromZipExcelToolStripMenuItem,
            this.importFromMongoDBToolStripMenuItem,
            this.importFromXMLToolStripMenuItem});
            this.importDataToolStripMenuItem.Name = "importDataToolStripMenuItem";
            this.importDataToolStripMenuItem.Size = new System.Drawing.Size(117, 23);
            this.importDataToolStripMenuItem.Text = "Import data";
            // 
            // importFromZipExcelToolStripMenuItem
            // 
            this.importFromZipExcelToolStripMenuItem.Name = "importFromZipExcelToolStripMenuItem";
            this.importFromZipExcelToolStripMenuItem.Size = new System.Drawing.Size(269, 24);
            this.importFromZipExcelToolStripMenuItem.Text = "Import from Zip(Excel)";
            this.importFromZipExcelToolStripMenuItem.Click += new System.EventHandler(this.ImportFromZipExcelToolStripMenuItem_Click);
            // 
            // importFromMongoDBToolStripMenuItem
            // 
            this.importFromMongoDBToolStripMenuItem.Name = "importFromMongoDBToolStripMenuItem";
            this.importFromMongoDBToolStripMenuItem.Size = new System.Drawing.Size(269, 24);
            this.importFromMongoDBToolStripMenuItem.Text = "Import from Mongo DB";
            this.importFromMongoDBToolStripMenuItem.Click += new System.EventHandler(this.ImportFromMongoDBToolStripMenuItem_Click);
            // 
            // importFromXMLToolStripMenuItem
            // 
            this.importFromXMLToolStripMenuItem.Name = "importFromXMLToolStripMenuItem";
            this.importFromXMLToolStripMenuItem.Size = new System.Drawing.Size(269, 24);
            this.importFromXMLToolStripMenuItem.Text = "Import from XML";
            this.importFromXMLToolStripMenuItem.Click += new System.EventHandler(this.ImportFromXmlToolStripMenuItem_Click);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToPDF,
            this.exportToToolStripMenuItem1,
            this.exportToJSONToolStripMenuItem,
            this.exportToExcelToolStripMenuItem});
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(117, 23);
            this.exportDataToolStripMenuItem.Text = "Export data";
            // 
            // exportToPDF
            // 
            this.exportToPDF.Name = "exportToPDF";
            this.exportToPDF.Size = new System.Drawing.Size(207, 24);
            this.exportToPDF.Text = "Export to PDF";
            this.exportToPDF.Click += new System.EventHandler(this.ExportToPdfMenuItem_Click);
            // 
            // exportToToolStripMenuItem1
            // 
            this.exportToToolStripMenuItem1.Name = "exportToToolStripMenuItem1";
            this.exportToToolStripMenuItem1.Size = new System.Drawing.Size(207, 24);
            this.exportToToolStripMenuItem1.Text = "Export to XML";
            this.exportToToolStripMenuItem1.Click += new System.EventHandler(this.ExportToXml_Click);
            // 
            // exportToJSONToolStripMenuItem
            // 
            this.exportToJSONToolStripMenuItem.Name = "exportToJSONToolStripMenuItem";
            this.exportToJSONToolStripMenuItem.Size = new System.Drawing.Size(207, 24);
            this.exportToJSONToolStripMenuItem.Text = "Export to JSON";
            this.exportToJSONToolStripMenuItem.Click += new System.EventHandler(this.ExportToJson_Click);
            // 
            // exportToExcelToolStripMenuItem
            // 
            this.exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
            this.exportToExcelToolStripMenuItem.Size = new System.Drawing.Size(207, 24);
            this.exportToExcelToolStripMenuItem.Text = "Export to Excel";
            this.exportToExcelToolStripMenuItem.Click += new System.EventHandler(this.ExportToExcel_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(56, 23);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(682, 381);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Bookman Old Style", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clinics";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}