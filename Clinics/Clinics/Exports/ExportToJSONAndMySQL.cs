namespace ClinicsProgram.Exports
{
    using System;
    using System.Windows.Forms;
    using System.Web.Script.Serialization;
    using Clinics.Data;
    using Clinics.Models;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using MySql.Data.MySqlClient;

    public partial class ExportToJSONAndMySQL : Form
    {
        public void ConvertToJSON()
        {
            var db = new ClinicsData();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var year = int.Parse(txtBoxYear.Text);

            var groupManipulations = db.Manipulations.All()
                .Where(m => m.Date.Month == cmbBoxMonth.SelectedIndex + 1 && m.Date.Year == year)
                .GroupBy(m => new { m.SpecialistId, m.ProcedureId });

            foreach (var specialist in groupManipulations)
            {
                object dataForReport = new
                {
                    Specialist = specialist.First().Specialist.FirstName + " " + specialist.First().Specialist.LastName,
                    Procedure = specialist.First().Procedure.Name,
                    ProcedureCount = specialist.Count(),
                    TotalPrice = specialist.First().Procedure.Price * specialist.Count(),
                    Month = specialist.First().Date.Month,
                    Year = specialist.First().Date.Year
                };

                string specialistAsJSON = serializer.Serialize(dataForReport);

                StreamWriter writer = new StreamWriter(string.Format("../../JsonReports/{0}.json", specialist.First().Id));
                using (writer)
                {
                    writer.WriteLine(specialistAsJSON);
                }

            }
        }

        public ExportToJSONAndMySQL()
        {
            this.InitializeComponent();
        }

        private void btnExportToJSON_Click_1(object sender, EventArgs e)
        {
            ConvertToJSON();
        }
    }
}
