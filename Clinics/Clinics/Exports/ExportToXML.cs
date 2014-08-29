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

            // Generating monthly report by clinic as XML file... 
            if (true)
            {
                CreateSampleReport();
            }
            else
            {
                var reports = RetrieveReportFromDB(year, month);
                //var reports = RetrieveReportTest(year, month);
                var clinicsReport = new XElement("reports", GetReportsXML(reports));
                clinicsReport.Save("../../Reports/Specialists-Monthly-Reports.xml");
            }

            MessageBox.Show("XML report generated in the Reports folder.");
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

        public static Dictionary<string, Dictionary<DateTime, DailyReport>> RetrieveReportFromDB(int year, int month)
        {
            var specialists = new Dictionary<string, Dictionary<DateTime, DailyReport>>();

            using (var db = new ClinicsDBContext())
            {
                var result = db.Manipulations.Where(m => m.Date.Year == year && m.Date.Month == month);

                // This is how it will look:
                //    <specialists>
                //      <specialist name="Nakov">
                //        <day date="8/28/2014">
                //          <expense>12365.01</expense>
                //          <manipulations>6</manipulations>
                //        </day>
                //        <day date="8/29/2014">
                //          <expense>265.01</expense>
                //          <manipulations>2</manipulations>
                //        </day>
                //      </specialist>
                //      <specialist name="Minkov">
                //        <day date="8/28/2014">
                //          <expense>555.01</expense>
                //          <manipulations>4</manipulations>
                //        </day>
                //        <day date="8/29/2014">
                //          <expense>6665.01</expense>
                //          <manipulations>5</manipulations>
                //        </day>
                //      </specialist>
                //    </specialists>

                foreach (var manipulation in db.Manipulations)
                {
                    Decimal sum = manipulation.Procedures.Price;
                    DateTime day = manipulation.Date;
                    string specName = manipulation.Specialists.LastName;

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

        private static void CreateSampleReport()
        {
            var day = new XElement("day");
            day.SetAttributeValue("date", "8/28/2014");
            day.Add(new XElement("expense", "12365.01"), new XElement("manipulations", "6"));

            var day2 = new XElement("day");
            day2.SetAttributeValue("date", "8/29/2014");
            day2.Add(new XElement("expense", "265.01"), new XElement("manipulations", "2"));

            var spec1 = new XElement("specialist", day, day2);
            spec1.SetAttributeValue("name", "Nakov");

            ////////////
            var day3 = new XElement("day");
            day3.SetAttributeValue("date", "8/28/2014");
            day3.Add(new XElement("expense", "555.01"), new XElement("manipulations", "4"));

            var day4 = new XElement("day");
            day4.SetAttributeValue("date", "8/29/2014");
            day4.Add(new XElement("expense", "6665.01"), new XElement("manipulations", "5"));

            var spec2 = new XElement("specialist", day3, day4);
            spec2.SetAttributeValue("name", "Minkov");


            //////
            var specialists = new XElement("specialists", spec1, spec2);

            specialists.Save("../../Reports/Clinics-Monhtly-Reports.xml");
        }
    }
}
