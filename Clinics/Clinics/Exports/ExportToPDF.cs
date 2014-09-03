namespace ClinicsProgram.Exports
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;

    using Clinics.Data;
    using Clinics.Operations.Exports;

    public partial class ExportToPdf : Form
    {
        private const string SuccessMessage = "Exporting data to PDF file done. The PDF can be found in Reports folder!";
        private const string ReportFolder = "/Reports";
        private PdfExport pdfExport = new PdfExport();
        private IClinicsData data = new ClinicsData();

        public ExportToPdf()
        {
            this.InitializeComponent();
        }

        private void ExportToPdf_Click(object sender, EventArgs e)
        {
            int month = this.month.SelectedIndex + 1;
            int year = int.Parse(this.year.Text);

            this.pdfExport.Export(this.data, month, year);

            MessageBox.Show(SuccessMessage);
            Process.Start(Directory.GetCurrentDirectory() + ReportFolder);
        }
    }
}
