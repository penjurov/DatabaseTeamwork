namespace ClinicsProgram.Imports
{
    using Clinics.Models;
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.IO.Compression;
    using System.Windows.Forms;
    using Clinics.Data;
    using System.Linq;

    public partial class ImportFromZipExcelFiles : Form
    {
        public ImportFromZipExcelFiles()
        {
            this.InitializeComponent();
        }

        private void BtnBrowseZipFile_Click(object sender, EventArgs e)
        {
            DialogResult result = fileDialogBrowseZipFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filePath = fileDialogBrowseZipFile.FileName;

                if (IsFileInCorrectFormat("zip", filePath))
                {
                    txtBoxImportFilePath.Text = filePath;
                }
                else
                {
                    MessageBox.Show(string.Format("Incorrect file format! Please select {0} file.", "zip"));
                }
            }
        }

        private bool IsFileInCorrectFormat(string fileFormat, string filePath)
        {
            if (filePath.EndsWith(fileFormat, true, System.Globalization.CultureInfo.InvariantCulture))
            {
                return true;
            }

            return false;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            string zipPath = @"C:\Users\Tihomir\Desktop\test.zip";
            string extractPath = @"C:\Users\Tihomir\Desktop\extracted\";
            string tempExtractedFile = "temp.xlsx";
            string currentReportDate = string.Empty;

            ClinicsData clinicsData = new ClinicsData();

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith("/"))
                    {
                        currentReportDate = entry.FullName.TrimEnd('/');
                    }
                    else
                    {
                        entry.ExtractToFile(Path.Combine(extractPath, tempExtractedFile), true);

                        DataTable excelData = ReadExcelData(extractPath + tempExtractedFile);

                        foreach (DataRow row in excelData.Rows)
                        {
                            var patientNumber = row["PatientNumber"];
                            var abreviature = row["Abreviature"].ToString();
                            var age = row["Age"];
                            var gender = row["Gender"].ToString();

                            var procedureName = row["Procedure"].ToString();
                            // var specialistFirstName = row["SpecialistFirstName"];
                            // var specialistLastName = row["SpecialistLastName"];
                            var specialistUIN = row["SpecialistUIN"];

                            Patient currentPatient = new Patient()
                            {
                                Id = Guid.NewGuid(),
                                PatientNumber = new Guid(patientNumber.ToString()),
                                Abreviature = abreviature,
                                Age = (int)age,
                                Gender = gender
                            };

                            var specialistID = clinicsData
                                .Specialists
                                .SearchFor(s => s.Uin == specialistUIN);

                            var procedureID = clinicsData
                                .Procedures
                                .SearchFor(pr => pr.Name == procedureName);

                            Manipulation currentManipulation = new Manipulation()
                            {
                                Id = Guid.NewGuid(),
                                PatientId = currentPatient.Id,
                                SpecialistId = new Guid(specialistID.ToString()),
                                ProcedureId = new Guid(procedureID.ToString()),
                                Date = DateTime.Parse(currentReportDate)
                            };

                            clinicsData.Patients.Add(currentPatient);

                            clinicsData.SaveChanges();

                            clinicsData.Patients
                                .SearchFor(p => p.Id == currentPatient.Id)
                                .First()
                                .Manipulations
                                .Add(currentManipulation);

                            clinicsData.SaveChanges();
                        }
                    }
                }
            }
        }

        private DataTable ReadExcelData(string filePath)
        {
            OleDbConnection dbConn = new OleDbConnection();
            DataTable dt = new DataTable();

            try
            {
                dbConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " +
                                                                    filePath + "; Extended Properties=\"Excel 12.0;HDR=YES\"";
                dbConn.Open();

                OleDbCommand dbCommand = new OleDbCommand("SELECT * FROM [Sheet1$]", dbConn);

                dbConn.Close();

                OleDbDataAdapter dbAdapter = new OleDbDataAdapter();
                dbAdapter.SelectCommand = dbCommand;
                dbAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dt;
        }
    }
}
