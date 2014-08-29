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
            var reports = RetrieveReportFromDB(year, month);
            var clinicsReport = new XElement("expenses", GetExpensesXML(reports));
            clinicsReport.Save("../../Reports/Clinics-Monhtly-Reports.xml");

            MessageBox.Show("XML report generated in the Reports folder.");
        }

        private List<XElement> GetExpensesXML(Dictionary<string, Dictionary<DateTime, Decimal>> reports)
        {
            var sales = new List<XElement>();
            foreach (var report in reports)
            {
                var dailySummaries = GetDailySummariesXML(report.Value);
                var expense = new XElement("expense", dailySummaries);
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
                    @"<summary date=""{0}"" total-sum=""{1:.00}"" />",
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

                //    <?xml version="1.0" encoding="utf-8"?>
                //    <expenses>
                //      <expense clinic="Clinic A">
                //        <expenses month="Jul-2013">30.00</expenses>
                //        <expenses month="Aug-2013">40.00</expenses>
                //      </expense>
                //      <expense clinic="Clinic B">
                //        <expenses month="Jul-2013">200.00</expenses>
                //        <expenses month="Aug-2013">180.00</expenses>
                //      </expense>
                //    </expenses>

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
    }
}
