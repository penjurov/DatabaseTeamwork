namespace ClinicsProgram.Exports
{
    using System;
    using System.Windows.Forms;
    using System.Web.Script.Serialization;
    using Clinics.Data;
    using Clinics.MySQLModels;
    using System.IO;
    using System.Linq;

    public partial class ExportToJSONAndMySQL : Form
    {
        private ClinicsData db = new ClinicsData();
        private ClinicsMySQLContext mySqlContext = new ClinicsMySQLContext();

        public void ConvertToJSON()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var year = int.Parse(txtBoxYear.Text);

            var groupManipulations = db.Manipulations.All()
                .Where(m => m.Date.Month == cmbBoxMonth.SelectedIndex + 1 && m.Date.Year == year)
                .GroupBy(m => new { m.SpecialistId, m.ProcedureId });

            foreach (var specialist in groupManipulations)
            {
                var specialistName = specialist.First().Specialist.FirstName + " " + specialist.First().Specialist.LastName;
                var procedureName = specialist.First().Procedure.Name;
                var procedureCount = specialist.Count();
                var totalPrice = specialist.First().Procedure.Price * specialist.Count();
                var month = specialist.First().Date.Month;

                object dataForReport = new
                {
                    Specialist = specialistName,
                    Procedure = procedureName,
                    ProcedureCount = procedureCount,
                    TotalPrice = totalPrice,
                    Month = month,
                    Year = year
                };

                string specialistAsJSON = serializer.Serialize(dataForReport);

                StreamWriter writer = new StreamWriter(string.Format("../../JsonReports/{0}.json", specialist.First().Id));
                using (writer)
                {
                    writer.WriteLine(specialistAsJSON);
                }    

                var specialistsStatistics = mySqlContext.Specialiststatistics
                    .Where(ss => ss.Specialist.Equals(specialistName) && ss.Procedure.Equals(procedureName) && ss.Month.Equals(month) && ss.Year == year)
                    .FirstOrDefault();

                if (specialistsStatistics == null)
                {
                    Specialiststatistic newSpecialistStatistics = new Specialiststatistic
                    {
                        Specialist = specialistName,
                        Procedure = procedureName,
                        ProcedureCount = procedureCount,
                        TotalPrice = totalPrice.ToString(),
                        Month = month,
                        Year = year
                    };

                    mySqlContext.Add(newSpecialistStatistics);
                    mySqlContext.SaveChanges();
                }               
            }

            
        }

        public ExportToJSONAndMySQL()
        {
            this.InitializeComponent();
        }

        ~ExportToJSONAndMySQL()
        {
            this.mySqlContext.Dispose();
        }

        private void btnExportToJSON_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConvertToJSON();
        }
    }
}
