namespace ClinicsProgram.Exports
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Windows.Forms;
    using System.Configuration;
    using System.Data.SQLite;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using OfficeOpenXml;
    using OfficeOpenXml.Table;
    using System.Collections.Generic;

    // You are given a SQLite database holding more information for each product.
    // Write a program to read the MySQL database of reports, read the information from SQLite
    // and generate a single Excel 2007 file holding some information by your choice.
    // Input: SQLite database; MySQL database. Output: Excel 2007 file (.xlsx).
    public partial class ExportToExcel : Form
    {
        public ExportToExcel()
        {
            this.InitializeComponent();
        }

        private void ExportToExcel_Click(object sender, EventArgs e)
        {
            DataTable specialists = ReadFromMySql();
            DataTable procedures = ReadFromSQLite();

            var proceduresDict = ConvertToDict(procedures);

            DataTable stats = JoinTables(specialists, proceduresDict);

            SaveDataToExcel(stats);
        }

        private DataTable ReadFromSQLite()
        {
            var dbCon = GetSQLiteConnection();
            dbCon.Open();
            using (dbCon)
            {
                var dataSet = new DataSet();
                var adapter = new SQLiteDataAdapter("SELECT Name, InsuranceCoverage FROM Procedures", dbCon);

                adapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
        }

        private DataTable ReadFromMySql()
        {
            var dbCon = GetMySqlConnection();
            dbCon.Open();
            using (dbCon)
            {
                var dataSet = new DataSet();
                var adapter = new MySqlDataAdapter("SELECT * FROM SpecialistStatistics", dbCon);

                adapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
        }

        private Dictionary<string, double> ConvertToDict(DataTable procedures)
        {
            var dict = new Dictionary<string, double>();
            foreach (DataRow row in procedures.Rows)
            {
                string name = row["Name"].ToString();
                double coverage = (double)row["InsuranceCoverage"];
                dict[name] = coverage;
            }

            return dict;
        }

        private DataTable JoinTables(DataTable specialists, Dictionary<string, double> procedures)
        {
            // Create new table with 4 columns
            DataTable joined = new DataTable();
            joined.Columns.Add("Specialist", typeof(string));
            joined.Columns.Add("Procedure", typeof(string));
            joined.Columns.Add("ProcedureCount", typeof(int));
            joined.Columns.Add("InsuranceCoverage", typeof(double));

            foreach (var spec in specialists.AsEnumerable())
            {
                var procedureName = spec.Field<string>("Procedure");
                joined.LoadDataRow(
                    new object[]
                    {
                        spec.Field<string>("Specialist"),
                        procedureName,
                        spec.Field<int>("ProcedureCount"),
                        procedures[procedureName]
                    }, fAcceptChanges: false);
            }

            //PrintJoined(joined);

            return joined;
        }

        private SQLiteConnection GetSQLiteConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicsSQLite"].ConnectionString;
            return new SQLiteConnection(connStr);
        }

        private MySqlConnection GetMySqlConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicsMySQL"].ConnectionString;
            return new MySqlConnection(connStr);
        }

        private OleDbConnection GetExcelConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicsExcel"].ConnectionString;
            return new OleDbConnection(connStr);
        }

        // Using http://epplus.codeplex.com/
        private void SaveDataToExcel(DataTable dataTable)
        {
            string fileName = "stats";
            string fileNameWithDate = string.Format("{0}_{1}.xlsx", fileName, DateTime.Now.ToString("dd-MM-yyyy"));
            string fileNameWithPath = @"..\..\..\Output\" + fileNameWithDate;

            // Delete existing file with same file name.
            if (File.Exists(fileNameWithPath))
                File.Delete(fileNameWithPath);

            GenerateExcel(dataTable, sheetName: "Joined", fileName: fileNameWithPath);
        }

        private void GenerateExcel(DataTable dataTable, string sheetName, string fileName)
        {
            var newFile = new FileInfo(fileName);

            //Step 1 : Create object of ExcelPackage class and pass file path to constructor.
            using (var package = new ExcelPackage(newFile))
            {
                //Step 2 : Add a new worksheet to ExcelPackage object and give a suitable name
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                //Step 3 : Start loading datatable form A1 cell of worksheet.
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true, TableStyles.None);

                //Step 5 : Save all changes to ExcelPackage object which will create Excel 2007 file.
                package.Save();

                MessageBox.Show(
                    string.Format("File name '{0}' generated successfully.", fileName),
                    "File generated successfully!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private static void PrintJoined(DataTable joined)
        {
            var result = joined
                .AsEnumerable()
                .Select(p => string.Format(
                    "{0} ({1}: {2}, {3})",
                    p.Field<string>("Specialist"),
                    p.Field<string>("Procedure"),
                    p.Field<int>("ProcedureCount"),
                    p.Field<double>("InsuranceCoverage")));

            MessageBox.Show(string.Join(Environment.NewLine, result));
        }
    }
}
