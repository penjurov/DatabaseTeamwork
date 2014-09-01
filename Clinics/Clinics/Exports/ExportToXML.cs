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
            //    <specialists>
            //      <specialist name="Nakov">
            //        <day date="8/28/2014">
            //          <expense>12365.01</expense>
            //          <manipulations>6</manipulations>
            //        </day>
            //      </specialist>
            //    </specialists>            

            var root = new XElement("reports");

            var db = new ClinicsData();
            foreach (var specialist in db.Specialists.All())
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
                    .Select(date => new XElement(
                        "day",
                        new XAttribute("date", date.Key.ToShortDateString()),
                        new XElement("expense", date.Value.expense),
                        new XElement("manipulations", date.Value.manipulations)
                        ));

                var specialistXML = new XElement(
                    "specialist",
                    new XAttribute("name", specialist.LastName),
                    days);

                root.Add(specialistXML);
            }

            return root;
        }
    }
}
