namespace ClinicsProgram.Imports
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Windows.Forms;
    using Clinics.Data;
    using Clinics.Models;
    using System.Globalization;

    public partial class ImportFromZipExcelFiles : Form
    {
        private const string Filter = "zip files (*.zip)|*.zip";
        private const string TempFileName = "clinicImport.xlsx";
        private const string TempFolderName = @"\extracted\";
        private const string ExcelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source = {0}; Extended Properties=\"Excel 12.0;HDR=YES\"";
        private IClinicsData data = new ClinicsData();

        public ImportFromZipExcelFiles()
        {
            this.InitializeComponent();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            this.FileSelect();
        }

        private void FileSelect()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = Filter;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.fileName.Text = ofd.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            this.Import();
        }

        private void Import()
        {
            try
            {
                string zipPath = this.fileName.Text;
                string tempFolder = Directory.GetCurrentDirectory() + TempFolderName;
                string currentReportDate = string.Empty;
                ZipArchive archive = ZipFile.OpenRead(zipPath);

                this.importProgress.Value = 0;
                this.importProgress.Maximum = archive.Entries.Count;

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

                            DataTable excelData = this.ReadExcelData(tempFolder + TempFileName);

                            foreach (DataRow row in excelData.Rows)
                            {
                                var patientNumber = row["PatientNumber"].ToString();

                                if (patientNumber != string.Empty)
                                {
                                    var patient = this.data
                                        .Patients.All()
                                        .Where(p => p.PatientNumber == patientNumber)
                                        .FirstOrDefault();

                                    if (patient == null)
                                    {
                                        patient = this.CreateNewPatient(row);
                                        this.data.Patients.Add(patient);
                                    }

                                    Manipulation currentManipulation = this.CreateNewManipulation(currentReportDate, row, patient);

                                    patient.Manipulations.Add(currentManipulation);  
                                }                          
                            }
                        }

                        if (this.importProgress.Value < this.importProgress.Maximum)
                        {
                            this.importProgress.Value++;
                        }
                    }
                }

                this.data.SaveChanges();

                MessageBox.Show("Data was successfully imported from Zip(Excel) to SQL Server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }          
        }

        private Manipulation CreateNewManipulation(string currentReportDate, DataRow row, Patient currentPatient)
        {
            var procedureName = row["Procedure"].ToString();
            var specialistUin = row["SpecialistUIN"].ToString();

            var specialist = this.data
                .Specialists.All()
                .Where(s => s.Uin == specialistUin)
                .FirstOrDefault();

            var procedure = this.data
                .Procedures.All()
                .Where(pr => pr.Name == procedureName)
                .FirstOrDefault();

            Manipulation currentManipulation = new Manipulation()
            {
                Id = Guid.NewGuid(),
                PatientId = currentPatient.Id,
                SpecialistId = specialist.Id,
                ProcedureId = procedure.Id,
                Date = DateTime.ParseExact(currentReportDate, "dd-MM-yyyy",  CultureInfo.InvariantCulture)
                //Date = DateTime.Parse(currentReportDate)
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