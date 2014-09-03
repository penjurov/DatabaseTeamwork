namespace Clinics.Operations.Imports
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Clinics.Data;
    using Clinics.Models;

    public class ExcelImport
    {
        private const string TempFileName = "clinicImport.xlsx";
        private const string TempFolderName = @"\extracted\";
        private const string ExcelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source = {0}; Extended Properties=\"Excel 12.0;HDR=YES\"";

        public void Import(IClinicsData data, string fileName)
        {
            string zipPath = fileName;
            string tempFolder = string.Format("{0}{1}", Directory.GetCurrentDirectory(), TempFolderName);
            string currentReportDate = string.Empty;
            ZipArchive archive = ZipFile.OpenRead(zipPath);

            using (archive)
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith("/"))
                    {
                        currentReportDate = entry.FullName.TrimEnd('/');
                    }
                    else
                    {
                        if (!Directory.Exists(tempFolder))
                        {
                            Directory.CreateDirectory(tempFolder);
                        }
                            
                        entry.ExtractToFile(Path.Combine(tempFolder, TempFileName), true);

                        DataTable excelData = this.ReadExcelData(string.Format("{0}{1}", tempFolder, TempFileName));

                        foreach (DataRow row in excelData.Rows)
                        {
                            var patientNumber = row["PatientNumber"].ToString();

                            if (patientNumber != string.Empty)
                            {
                                var patient = data
                                    .Patients.All()
                                    .Where(p => p.PatientNumber == patientNumber)
                                    .FirstOrDefault();

                                if (patient == null)
                                {
                                    patient = this.CreateNewPatient(row);
                                    data.Patients.Add(patient);
                                }

                                Manipulation currentManipulation = this.CreateNewManipulation(data, currentReportDate, row, patient);

                                patient.Manipulations.Add(currentManipulation);  
                            }                          
                        }
                    }
                }
            }

            data.SaveChanges();       
        }

        private Manipulation CreateNewManipulation(IClinicsData data, string currentReportDate, DataRow row, Patient currentPatient)
        {
            var procedureName = row["Procedure"].ToString();
            var specialistUin = row["SpecialistUIN"].ToString();
            var information = row["Information"].ToString();

            var specialist = data
                .Specialists.All()
                .Where(s => s.Uin == specialistUin)
                .FirstOrDefault();

            var procedure = data
                .Procedures.All()
                .Where(pr => pr.Name == procedureName)
                .FirstOrDefault();

            Manipulation currentManipulation = new Manipulation()
            {
                Id = Guid.NewGuid(),
                PatientId = currentPatient.Id,
                SpecialistId = specialist.Id,
                ProcedureId = procedure.Id,
                Information = information,
                Date = DateTime.ParseExact(currentReportDate, "dd-MM-yyyy",  CultureInfo.InvariantCulture)
            };
            return currentManipulation;
        }

        private Patient CreateNewPatient(DataRow row)
        {
            var patientNumber = row["PatientNumber"].ToString();
            var abreviature = row["Abreviature"].ToString();
            var age = row["Age"].ToString();
            var gender = row["Gender"].ToString();        

            Patient currentPatient = new Patient()
            {
                Id = Guid.NewGuid(),
                PatientNumber = patientNumber,
                Abreviature = abreviature,
                Age = int.Parse(age),
                Gender = gender
            };
            return currentPatient;
        }

        private DataTable ReadExcelData(string filePath)
        {
            OleDbConnection excelConnection = new OleDbConnection(string.Format(ExcelConnectionString, filePath));
            DataTable dt = new DataTable();
            
            excelConnection.Open();
            OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter("select * from [SHEET1$]", excelConnection);
            da.Fill(dt);
            excelConnection.Close();

            return dt;
        }
    }
}
