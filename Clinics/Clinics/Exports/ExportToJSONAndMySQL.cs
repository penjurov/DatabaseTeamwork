namespace ClinicsProgram.Exports
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;
    using System.Windows.Forms;

    using Clinics.Data;
    using Clinics.MySQLModels;

    public partial class ExportToJSONAndMySQL : Form
    {
        private const string ReportFolder = "../../JsonReports/";
        private const string FileName = "{0}.json";
        private ClinicsData db = new ClinicsData();
        private ClinicsMySQLContext mySqlContext = new ClinicsMySQLContext();

        public ExportToJSONAndMySQL()
        {
            this.InitializeComponent();
        }

        ~ExportToJSONAndMySQL()
        {
            this.mySqlContext.Dispose();
        }

        public void Export()
        { 
            var year = int.Parse(this.txtBoxYear.Text);
            var month = this.cmbBoxMonth.SelectedIndex + 1;

            var groupManipulations = this.db.Manipulations.All()
                .Where(m => m.Date.Month == month && m.Date.Year == year)
                .GroupBy(m => new { m.SpecialistId, m.ProcedureId });

            foreach (var specialist in groupManipulations)
            {
                var specialistId = specialist.First().Id;
                var specialistName = string.Format("{0} {1}", specialist.First().Specialist.FirstName, specialist.First().Specialist.LastName);
                var procedureName = specialist.First().Procedure.Name;
                var procedureCount = specialist.Count();
                var totalPrice = specialist.First().Procedure.Price * specialist.Count();
                
                this.GenerateJson(year, month, specialistName, procedureName, procedureCount, totalPrice, specialistId);
                this.MySqlInsert(year, month, specialistName, procedureName, procedureCount, totalPrice);              
            }
        }

        private void MySqlInsert(int year, int month, string specialistName, string procedureName, int procedureCount, decimal totalPrice)
        {
            var specialistsStatistics = this.mySqlContext.Specialiststatistics
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

                this.mySqlContext.Add(newSpecialistStatistics);
                this.mySqlContext.SaveChanges();
            }
        }

        private void GenerateJson(int year, int month, string specialistName, string procedureName, int procedureCount, decimal totalPrice, Guid specialistId)
        {
            object dataForReport = new
            {
                Specialist = specialistName,
                Procedure = procedureName,
                ProcedureCount = procedureCount,
                TotalPrice = totalPrice,
                Month = month,
                Year = year
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string specialistAsJson = serializer.Serialize(dataForReport);

            if (!Directory.Exists(ReportFolder))
            {
                Directory.CreateDirectory(ReportFolder);
            }

            StreamWriter writer = new StreamWriter(ReportFolder + string.Format(FileName, specialistId.ToString()));
            using (writer)
            {
                writer.WriteLine(specialistAsJson);
            }
        }

        private void Export_Click(object sender, EventArgs e)
        {
            this.Export();
        }
    }
}