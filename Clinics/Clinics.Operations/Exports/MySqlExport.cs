namespace Clinics.Operations.Exports
{
    using System.Linq;
    using Clinics.MySQLModels;

    public class MySqlExport
    {
        public void Export(ClinicsMySQLContext mySqlContext, int year, int month, string specialistName, string procedureName, int procedureCount, decimal totalPrice)
        {
            var specialistsStatistics = mySqlContext.Specialiststatistics
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

                mySqlContext.Add(newSpecialistStatistics);
                mySqlContext.SaveChanges();
            }
        }
    }
}
