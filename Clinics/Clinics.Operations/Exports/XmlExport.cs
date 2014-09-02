namespace Clinics.Operations.Exports
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Clinics.Data;

    public class XmlExport
    {
        private const string FileName = "../../Reports/Specialists-Monthly-Reports.xml";

        public void Export(IClinicsData data, int month, int year)
        {
            var reportXml = this.GenerateReportXml(data, year, month);
            reportXml.Save(FileName);
        }

        private XElement GenerateReportXml(IClinicsData data, int year, int month)
        {
            string monthStr = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            var root = new XElement("reports", new XAttribute("period", string.Format("{0}-{1}", monthStr, year)));

            foreach (var specialist in data.Specialists.All().OrderBy(s => s.LastName).ThenBy(s => s.FirstName))
            {
                var days = specialist
                    .Manupulations
                    .Where(m => m.Date.Year == year && m.Date.Month == month)
                    .GroupBy(m => m.Date).ToDictionary(gr => gr.Key,
                        gr => new
                        {
                            manipulations = gr.Count(),
                            expense = gr.Sum(m => m.Procedure.Price)
                        })
                    .OrderBy(date => date.Key)
                    .Select(date => new XElement(
                        "day",
                        new XAttribute("date", date.Key.ToShortDateString()),
                        new XElement("expense", date.Value.expense),
                        new XElement("manipulations", date.Value.manipulations)));

                var specialistXml = new XElement(
                    "specialist",
                    new XAttribute("firstName", specialist.FirstName),
                    new XAttribute("lastName", specialist.LastName),
                    days);

                root.Add(specialistXml);
            }

            return root;
        }
    }
}