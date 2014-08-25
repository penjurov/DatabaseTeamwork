namespace Clinics
{
    using System;
    using System.Windows.Forms;
    using Clinics.Imports;
    using Clinics.Exports;

    public partial class Main : Form
    {
        
        public Main()
        {
            InitializeComponent();
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
            var importFromXml = new ImportFromXML();
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
