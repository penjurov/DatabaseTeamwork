namespace ClinicsProgram.Imports
{
    public partial class ImportFromXml
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Button import;
        private System.Windows.Forms.TextBox fileName;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportFromXml));
            this.browse = new System.Windows.Forms.Button();
            this.import = new System.Windows.Forms.Button();
            this.fileName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // browse
            // 
            this.browse.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("browse.BackgroundImage")));
            this.browse.Font = new System.Drawing.Font("Bookman Old Style", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.browse.Location = new System.Drawing.Point(12, 12);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(121, 23);
            this.browse.TabIndex = 1;
            this.browse.Text = "Browse";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // import
            // 
            this.import.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("import.BackgroundImage")));
            this.import.Font = new System.Drawing.Font("Bookman Old Style", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.import.Location = new System.Drawing.Point(12, 41);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(121, 23);
            this.import.TabIndex = 3;
            this.import.Text = "Import";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.Import_Click);
            // 
            // fileName
            // 
            this.fileName.Location = new System.Drawing.Point(139, 15);
            this.fileName.Name = "fileName";
            this.fileName.ReadOnly = true;
            this.fileName.Size = new System.Drawing.Size(433, 20);
            this.fileName.TabIndex = 5;
            // 
            // ImportFromXml
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(686, 385);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.import);
            this.Controls.Add(this.browse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImportFromXml";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import from XML";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}