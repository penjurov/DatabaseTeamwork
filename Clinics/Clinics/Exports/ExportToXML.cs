namespace ClinicsProgram.Exports
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;

    using Clinics.Data;
    using Clinics.Operations.Exports;
    
    public partial class ExportToXml : Form
    {
        private const string SuccessMessage = "Exporting data to XML file done. The XML can be found in Reports folder!";
        private const string ReportFolder = "/Reports";
        private IClinicsData data = new ClinicsData();
        private XmlExport xmlExport = new XmlExport();

        public ExportToXml()
        {
            this.InitializeComponent();
        }

        private void ExportToXml_Click(object sender, EventArgs e)
        {
            int month = this.month.SelectedIndex + 1;
            int year = int.Parse(this.year.Text);

            this.xmlExport.Export(this.data, month, year);

            MessageBox.Show(SuccessMessage);
            Process.Start(Directory.GetCurrentDirectory() + ReportFolder);
        }
    }
}
