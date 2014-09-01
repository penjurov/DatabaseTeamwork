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
        public class DailyReport
        {
            public DateTime Date { get; set; }
            public decimal Expenses { get; set; }
            public int ManipulationCount { get; set; }
        }

        public ExportToXML()
        {
            this.InitializeComponent();
        }

        private void ExportToXML_Click(object sender, EventArgs e)
        {
            int month = this.Month.SelectedIndex + 1;
            int year = int.Parse(this.Year.Text);

            SaveReportsAsXML(month, year);

            MessageBox.Show("XML report generated in the Reports folder.");
        }

        private void SaveReportsAsXML(int month, int year)
        {
            var reportsDict = RetrieveReports(year, month);
            var clinicsReport = new XElement("reports", GetReportsXML(reportsDict));
            clinicsReport.Save("../../Reports/Specialists-Monthly-Reports.xml");
        }

        private List<XElement> GetReportsXML(Dictionary<string, Dictionary<DateTime, DailyReport>> reports)
        {
            var specialistsXML = new List<XElement>();
            foreach (var specialist in reports)
            {
                var specialistXML = new XElement("specialist", GetDailyReportsXML(specialist.Value));
                specialistXML.SetAttributeValue("name", specialist.Key);
                specialistsXML.Add(specialistXML);
            }

            return specialistsXML;
        }

        private List<XElement> GetDailyReportsXML(Dictionary<DateTime, DailyReport> dailyReports)
        {
            var dailyReportsXML = new List<XElement>();
            foreach (var daily in dailyReports)
            {
                var dateXML = new XElement("day");
                dateXML.SetAttributeValue("date", daily.Key.ToShortDateString());

                var expenseXML = new XElement("expense");
                expenseXML.Value = daily.Value.Expenses.ToString();

                var countXML = new XElement("manipulations");
                countXML.Value = daily.Value.ManipulationCount.ToString();

                dateXML.Add(expenseXML, countXML);

                dailyReportsXML.Add(dateXML);
            }

            return dailyReportsXML;
        }

        public static Dictionary<string, Dictionary<DateTime, DailyReport>> RetrieveReports(int year, int month)
        {
            var specialists = new Dictionary<string, Dictionary<DateTime, DailyReport>>();

            var db = new ClinicsData();

            var manipulations = db.Manipulations.All().Where(m => m.Date.Year == year && m.Date.Month == month);

            //    <specialists>
            //      <specialist name="Nakov">
            //        <day date="8/28/2014">
            //          <expense>12365.01</expense>
            //          <manipulations>6</manipulations>
            //        </day>
            //      </specialist>
            //    </specialists>

            foreach (var manipulation in manipulations)
            {
                Decimal sum = manipulation.Procedure.Price;
                DateTime day = manipulation.Date;

                string specName = manipulation.Specialist.LastName;

                if (!specialists.ContainsKey(specName))
                {
                    specialists.Add(specName, new Dictionary<DateTime, DailyReport>());
                }

                if (!specialists[specName].ContainsKey(day))
                {
                    specialists[specName].Add(day, new DailyReport());
                }

                specialists[specName][day].Expenses += sum;
                specialists[specName][day].ManipulationCount++;
            }

            return specialists;

        }
    }
}
