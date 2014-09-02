namespace Clinics.Operations.Exports
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;

    public class JsonExport
    {
        private const string ReportFolder = "../../Reports/JsonReports/";
        private const string FileName = "{0}.json";

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
    }
}
