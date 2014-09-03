namespace ClinicsProgram.Exports
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
 
    using Clinics.MySQLModels;
    using Clinics.Operations.Exports;

    public partial class ExportToExcel : Form
    {
        private const string SuccessMessage = "Exporting data to Excel done. The Excel file can be found in Reports folder!";
        private const string ReportFolder = "/Reports";
        private ExcelExport excelExport = new ExcelExport();
        private ClinicsMySQLContext mySqlContext = new ClinicsMySQLContext();

        public ExportToExcel()
        {
            this.InitializeComponent();
        }

        private void ExportToExcel_Click(object sender, EventArgs e)
        {
            this.excelExport.Export(this.mySqlContext);
            MessageBox.Show(SuccessMessage);
            Process.Start(Directory.GetCurrentDirectory() + ReportFolder);
        }
    }
}
