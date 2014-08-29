namespace ClinicsProgram.Exports
{
    using Clinics.Data;
    using System;
    using System.Windows.Forms;
    using System.Linq;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public partial class ExportToXML : Form
    {
        public ExportToXML()
        {
            this.InitializeComponent();
        }

        private void ExportToXML_Click(object sender, EventArgs e)
        {
            int month = this.Month.SelectedIndex + 1;
            int year = int.Parse(this.Year.Text);

            // Generating monthly report by clinic as XML file...            

            // AT THE MOMENT WORKS WITH FAKE DATA, UNCOMMENT THE METHOD BELOW TO WORK WITH THE DATABASE
            //var reports = RetrieveReportFromDB(year, month);
            var reports = RetrieveReportTest(year, month);
            var clinicsReport = new XElement("reports", GetExpensesXML(reports));
            clinicsReport.Save("../../Reports/Clinics-Monhtly-Reports.xml");

            MessageBox.Show("XML report generated in the Reports folder.");
        }

        private List<XElement> GetExpensesXML(Dictionary<string, Dictionary<DateTime, Decimal>> reports)
        {
            var sales = new List<XElement>();
            foreach (var report in reports)
            {
                var dailySummaries = GetDailySummariesXML(report.Value);
                var expense = new XElement("report", dailySummaries);
                expense.SetAttributeValue("clinic", report.Key);
                sales.Add(expense);
            }

            return sales;
        }

        private List<XElement> GetDailySummariesXML(Dictionary<DateTime, Decimal> report)
        {
            var dailySummary = new List<XElement>();
            foreach (var item in report)
            {
                var elementAsString = String.Format(
                    @"<summary date=""{0}"" expense=""{1:.00}"" />",
                    item.Key.ToShortDateString(),
                    item.Value);

                var summary = XElement.Parse(elementAsString);
                dailySummary.Add(summary);
            }

            return dailySummary;
        }

        public static Dictionary<string, Dictionary<DateTime, Decimal>> RetrieveReportFromDB(int year, int month)
        {
            var dailyByClinic = new Dictionary<string, Dictionary<DateTime, decimal>>();

            using (var db = new ClinicsDBContext())
            {
                var result = db.Manipulations.Where(m => m.Date.Year == year && m.Date.Month == month);

                // This is how it will look:

                //    <?xml version="1.0" encoding="utf-8"?>
                //    <reports>
                //      <report clinic="Clinic A">
                //        <summary date="8/29/2014" expense="12365.01" />
                //        <summary date="8/19/2014" expense="4545.20" />
                //      </report>
                //      <report clinic="Clinic B">
                //        <summary date="9/11/2014" expense="12365.01" />
                //        <summary date="8/22/2014" expense="4545.20" />
                //      </report>
                //    </reports>

                foreach (var manipulation in db.Manipulations)
                {
                    Decimal sum = manipulation.Procedures.Price;
                    DateTime day = manipulation.Date;
                    //string procedureName = manipulation.Procedures.Name;

                    string clinicName = manipulation.Specialists.Clinics.First().ClinicName;

                    if (dailyByClinic.ContainsKey(clinicName) == false)
                    {
                        dailyByClinic.Add(clinicName, new Dictionary<DateTime, decimal>());
                    }

                    if (dailyByClinic[clinicName].ContainsKey(day) == false)
                    {
                        dailyByClinic[clinicName].Add(day, 0);
                    }

                    dailyByClinic[clinicName][day] += sum;
                }

                return dailyByClinic;
            }
        }

        public static Dictionary<string, Dictionary<DateTime, Decimal>> RetrieveReportTest(int year, int month)
        {
            var dailyByClinic = new Dictionary<string, Dictionary<DateTime, decimal>>();

            dailyByClinic.Add("Clinic A", new Dictionary<DateTime, decimal>());
            dailyByClinic.Add("Clinic B", new Dictionary<DateTime, decimal>());

            dailyByClinic["Clinic A"].Add(DateTime.Now, 12365.01m);
            dailyByClinic["Clinic A"].Add(DateTime.Now.AddDays(-10), 4545.20m);

            dailyByClinic["Clinic B"].Add(DateTime.Now.AddDays(13), 12365.01m);
            dailyByClinic["Clinic B"].Add(DateTime.Now.AddDays(-7), 4545.20m);

            return dailyByClinic;
        }
    }
}
