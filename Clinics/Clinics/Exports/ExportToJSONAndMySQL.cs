namespace ClinicsProgram.Exports
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    using Clinics.Data;
    using Clinics.MySQLModels;
    using Clinics.Operations.Exports;

    public partial class ExportToJsonAndMySql : Form
    {
        private const string SuccessMessage = "Exporting data to MySQL Server and generating Json files done. The Json files can be find in Reports folder!";
        private IClinicsData data = new ClinicsData();
        private ClinicsMySQLContext mySqlContext = new ClinicsMySQLContext();
        private MySqlExport mySqlExport = new MySqlExport();
        private JsonExport jsonExport = new JsonExport();
        
        public ExportToJsonAndMySql()
        {
            this.InitializeComponent();
        }

        ~ExportToJsonAndMySql()
        {
            this.data.Dispose();
        }

        private void Export_Click(object sender, EventArgs e)
        {
            var year = int.Parse(this.txtBoxYear.Text);
            var month = this.cmbBoxMonth.SelectedIndex + 1;

            var groupManipulations = this.data.Manipulations.All()
                .Where(m => m.Date.Month == month && m.Date.Year == year)
                .GroupBy(m => new { m.SpecialistId, m.ProcedureId });

            foreach (var specialist in groupManipulations)
            {
                var specialistId = specialist.First().Id;
                var specialistName = string.Format("{0} {1}", specialist.First().Specialist.FirstName, specialist.First().Specialist.LastName);
                var procedureName = specialist.First().Procedure.Name;
                var procedureCount = specialist.Count();
                var totalPrice = specialist.First().Procedure.Price * specialist.Count();
                
                this.jsonExport.Export(year, month, specialistName, procedureName, procedureCount, totalPrice, specialistId);
                this.mySqlExport.Export(this.mySqlContext, year, month, specialistName, procedureName, procedureCount, totalPrice);              
            }

            MessageBox.Show(SuccessMessage);
        }
    }
}