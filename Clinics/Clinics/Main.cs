namespace ClinicsProgram
{
    using System;
    using System.Windows.Forms;
    using ClinicsProgram.Exports;
    using ClinicsProgram.Imports;

    internal partial class Main : Form
    {  
        public Main()
        {
            this.InitializeComponent();
        }

        private void ImportFromZipExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var importFromZip = new ImportFromZipExcelFiles();
            importFromZip.ShowDialog();
        }

        private void ImportFromMongoDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var importFromMongo = new ImportFromMongoDB();
            importFromMongo.ShowDialog();
        }

        private void ImportFromXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var importFromXml = new ImportFromXml();
            importFromXml.ShowDialog();
        }

        private void ExportToPdfMenuItem_Click(object sender, EventArgs e)
        {
            var exportToPdf = new ExportToPDF();
            exportToPdf.ShowDialog();
        }

        private void ExportToXml_Click(object sender, EventArgs e)
        {
            var exportToXml = new ExportToXML();
            exportToXml.ShowDialog();
        }

        private void ExportToJson_Click(object sender, EventArgs e)
        {
            var exportToJson = new ExportToJSONAndMySQL();
            exportToJson.ShowDialog();
        }

        private void ExportToExcel_Click(object sender, EventArgs e)
        {
            var exportToExcel = new ExportToExcel();
            exportToExcel.ShowDialog();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
