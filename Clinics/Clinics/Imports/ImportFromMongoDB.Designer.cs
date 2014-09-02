namespace ClinicsProgram.Imports
{
    public partial class ImportFromMongoDB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button import;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportFromMongoDB));
            this.import = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // import
            // 
            this.import.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.import.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("import.BackgroundImage")));
            this.import.Location = new System.Drawing.Point(13, 13);
            this.import.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(182, 32);
            this.import.TabIndex = 1;
            this.import.Text = "Import from Mongo";
            this.import.UseVisualStyleBackColor = false;
            this.import.Click += new System.EventHandler(this.ImportFromMongo_Click);
            // 
            // ImportFromMongoDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(686, 385);
            this.Controls.Add(this.import);
            this.Font = new System.Drawing.Font("Bookman Old Style", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ImportFromMongoDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import from MongoDB";
            this.ResumeLayout(false);

        }

        #endregion
    }
}