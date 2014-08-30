namespace ClinicsProgram.Imports
{
    public partial class ImportFromMongoDB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button import;
        private System.Windows.Forms.ProgressBar importProgress;

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
            this.import = new System.Windows.Forms.Button();
            this.importProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // Import
            // 
            this.import.Location = new System.Drawing.Point(12, 12);
            this.import.Name = "Import";
            this.import.Size = new System.Drawing.Size(121, 23);
            this.import.TabIndex = 1;
            this.import.Text = "Import from Mongo";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.ImportFromMongo_Click);
            // 
            // ImportProgress
            // 
            this.importProgress.Location = new System.Drawing.Point(12, 41);
            this.importProgress.Name = "ImportProgress";
            this.importProgress.Size = new System.Drawing.Size(560, 23);
            this.importProgress.TabIndex = 2;
            this.importProgress.Visible = false;
            // 
            // ImportFromMongoDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.importProgress);
            this.Controls.Add(this.import);
            this.Name = "ImportFromMongoDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import from MongoDB";
            this.ResumeLayout(false);

        }

        #endregion
    }
}