namespace ClinicsProgram.Exports
{
    using Clinics.Data;
    using System;
    using System.Windows.Forms;
    using System.Linq;
    using System.Xml.Linq;
    using System.Globalization;

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

            SaveReportsAsXML(month, year);

            MessageBox.Show("XML report generated in the Reports folder.");
        }

        private void SaveReportsAsXML(int month, int year)
        {
            var reportXML = GenerateReportXML(year, month);
            reportXML.Save("../../Reports/Specialists-Monthly-Reports.xml");
        }

        private XElement GenerateReportXML(int year, int month)
        {
            //  <reports period="August-2014">
            //    <specialist firstName="Ivan" lastName="Petrov">
            //      <day date="8/25/2014">
            //        <expense>180.00</expense>
            //        <manipulations>3</manipulations>
            //      </day>
            //    </specialist>
            //  </reports>       

            string monthStr = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            var root = new XElement("reports", new XAttribute("period", monthStr + "-" + year));

            var db = new ClinicsData();
            foreach (var specialist in db.Specialists.All().OrderBy(s => s.LastName).ThenBy(s => s.FirstName))
            {
                var days = specialist
                    .Manupulations
                    .Where(m => m.Date.Year == year && m.Date.Month == month)
                    .GroupBy(m => m.Date)
                    .ToDictionary(gr => gr.Key, gr => new
                    {
                        manipulations = gr.Count(),
                        expense = gr.Sum(m => m.Procedure.Price)
                    })
                    .OrderBy(date => date.Key)
                    .Select(date => new XElement(
                        "day",
                        new XAttribute("date", date.Key.ToShortDateString()),
                        new XElement("expense", date.Value.expense),
                        new XElement("manipulations", date.Value.manipulations)
                        ));

                var specialistXML = new XElement(
                    "specialist",
                    new XAttribute("firstName", specialist.FirstName),
                    new XAttribute("lastName", specialist.LastName),
                    days);

                root.Add(specialistXML);
            }

            return root;
        }
    }
}
