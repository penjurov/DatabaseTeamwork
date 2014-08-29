namespace ClinicsProgram.Imports
{
    using System;
    using System.Windows.Forms;

    public partial class ImportFromZipExcelFiles : Form
    {
        public ImportFromZipExcelFiles()
        {
            this.InitializeComponent();
        }

        private void btnBrowseZipFile_Click(object sender, EventArgs e)
        {
            DialogResult result = fileDialogBrowseZipFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = fileDialogBrowseZipFile.FileName;
                try
                {
                    MessageBox.Show(file);
                    //string text = File.ReadAllText(file);
                    //size = text.Length;
                }
                catch
                {

                }
            }
        }
    }
}
