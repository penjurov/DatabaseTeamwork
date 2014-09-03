namespace Clinics.Operations.Exports
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;

    public class JsonExport
    {
        private const string FileName = "{0}.json";
        private readonly string reportFolder = Directory.GetCurrentDirectory() + "/Reports/JsonReports/";
        
        public void Export(int year, int month, string specialistName, string procedureName, int procedureCount, decimal totalPrice, Guid specialistId)
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

            if (!Directory.Exists(this.reportFolder))
            {
                Directory.CreateDirectory(this.reportFolder);
            }

            StreamWriter writer = new StreamWriter(this.reportFolder + string.Format(FileName, specialistId.ToString()));
            using (writer)
            {
                writer.WriteLine(specialistAsJson);
            }
        }
    }
}
