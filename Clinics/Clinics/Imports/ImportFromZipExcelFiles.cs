namespace ClinicsProgram.Imports
{
    using System;
    using System.Windows.Forms;

    using Clinics.Data;
    using Clinics.Operations.Imports;

    public partial class ImportFromZipExcelFiles : Form
    {
        private const string Filter = "zip files (*.zip)|*.zip";
        private IClinicsData data = new ClinicsData();
        private ExcelImport excelImport = new ExcelImport();

        public ImportFromZipExcelFiles()
        {
            this.InitializeComponent();
        }

        ~ImportFromZipExcelFiles()  
        {
            this.data.Dispose();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            this.FileSelect();
        }

        private void FileSelect()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = Filter;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.fileName.Text = ofd.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            try
            {
                this.excelImport.Import(this.data, this.fileName.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}