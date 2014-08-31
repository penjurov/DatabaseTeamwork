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
            var procedures = ConnectToSQLite();

            var specialists = ConnectToMySQL();

            SaveDataToExcel(procedures);
        }

        private DataTable ConnectToSQLite()
        {
            var dbSQLite = GetSQLiteConnection();
            DataTable procedures = GetProcedures(dbSQLite);

            var result = procedures
                .AsEnumerable()
                .Select(p => string.Format(
                    "{0} ({1:#0.##%})",
                    p.Field<string>("Name"),
                    p.Field<double>("InsuranceCoverage")));

            MessageBox.Show(string.Join(Environment.NewLine, result));

            return procedures;
        }

        private DataTable GetProcedures(SQLiteConnection dbCon)
        {
            dbCon.Open();
            using (dbCon)
            {
                var dataSet = new DataSet();
                var adapter = new SQLiteDataAdapter("SELECT Name, InsuranceCoverage FROM Procedures", dbCon);

                adapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
        }

        private DataTable ConnectToMySQL()
        {
            var dbMySql = GetMySqlConnection();
            DataTable specialists = GetStats(dbMySql);

            var result = specialists
                .AsEnumerable()
                .Select(p => string.Format(
                    "{0} ({1}: {2})",
                    p.Field<string>("Specialist"),
                    p.Field<string>("Procedure"),
                    p.Field<int>("ProcedureCount")));

            MessageBox.Show(string.Join(Environment.NewLine, result));

            return specialists;
        }

        private DataTable GetStats(MySqlConnection dbCon)
        {
            dbCon.Open();
            using (dbCon)
            {
                var dataSet = new DataSet();
                var adapter = new MySqlDataAdapter("SELECT * FROM SpecialistStatistics", dbCon);

                adapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
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

        private void SaveDataToExcel(DataTable dataTable)
        {
            //DataTable dataTable = GenerateDataTableWithRecords();

            string sheetName = "statsTable";
            string fileName = "stats";

            GenerateExcel(dataTable, sheetName, fileName);
        }        
        
        // Creates a DataTable with dummy data.
        private DataTable GenerateDataTableWithRecords()
        {
            var dtEmployee = new DataTable();

            dtEmployee.Columns.Add("EmployeeID", typeof(int));
            dtEmployee.Columns.Add("EmployeeName", typeof(string));
            dtEmployee.Columns.Add("Designation", typeof(string));

            dtEmployee.Rows.Add(1, "Bytes Of Code", "C# developer");
            dtEmployee.Rows.Add(2, "RAJ", "Software Engineer");
            dtEmployee.Rows.Add(3, "Vicky", "Student");
            dtEmployee.Rows.Add(4, "Me", "Programmer");

            return dtEmployee;
        }

        private void GenerateExcel(DataTable dataTable, string sheetName, string fileName)
        {
            string fileNameWithDate = string.Format("{0}_{1}", fileName, DateTime.Now.ToString("dd-MM-yyyy"));
            //string finalFileNameWithPath = string.Format("{0}\\{1}.xlsx", Environment.CurrentDirectory, fileNameWithDate);

            string finalFileNameWithPath = @"..\..\..\Output\" + fileNameWithDate + ".xlsx";

            

            //Delete existing file with same file name.
            if (File.Exists(finalFileNameWithPath))
                File.Delete(finalFileNameWithPath);

            var newFile = new FileInfo(finalFileNameWithPath);

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
    }
}
