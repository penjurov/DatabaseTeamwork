﻿namespace ClinicsProgram.Imports
{
    public partial class ImportFromZipExcelFiles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.OpenFileDialog fileDialogBrowseZipFile;
        private System.Windows.Forms.Button btnBrowseZipFile;
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
            this.btnImport = new System.Windows.Forms.Button();
            this.fileDialogBrowseZipFile = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowseZipFile = new System.Windows.Forms.Button();
            this.fileName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(13, 41);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(121, 23);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.Import_Click);
            // 
            // fileDialogBrowseZipFile
            // 
            this.fileDialogBrowseZipFile.FileName = "openFileDialog1";
            // 
            // btnBrowseZipFile
            // 
            this.btnBrowseZipFile.Location = new System.Drawing.Point(12, 12);
            this.btnBrowseZipFile.Name = "btnBrowseZipFile";
            this.btnBrowseZipFile.Size = new System.Drawing.Size(121, 23);
            this.btnBrowseZipFile.TabIndex = 1;
            this.btnBrowseZipFile.Text = "Browse";
            this.btnBrowseZipFile.UseVisualStyleBackColor = true;
            this.btnBrowseZipFile.Click += new System.EventHandler(this.Browse_Click);
            // 
            // fileName
            // 
            this.fileName.Location = new System.Drawing.Point(139, 14);
            this.fileName.Name = "fileName";
            this.fileName.ReadOnly = true;
            this.fileName.Size = new System.Drawing.Size(433, 20);
            this.fileName.TabIndex = 2;
            // 
            // ImportFromZipExcelFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.btnBrowseZipFile);
            this.Controls.Add(this.btnImport);
            this.Name = "ImportFromZipExcelFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import from ZIP(Excel)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}