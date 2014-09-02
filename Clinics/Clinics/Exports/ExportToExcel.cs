namespace ClinicsProgram.Exports
{
    using System;
    using System.Windows.Forms;

    using Clinics.Operations.Exports;

    public partial class ExportToExcel : Form
    {
        private const string SuccessMessage = "Exporting data Excel done. The Excel file can be find in Reports folder!";
        private ExcelExport excelExport = new ExcelExport();

        public ExportToExcel()
        {
            this.InitializeComponent();
        }

        private void ExportToExcel_Click(object sender, EventArgs e)
        {
            this.excelExport.Export();
            MessageBox.Show(SuccessMessage);
        }
    }
}
