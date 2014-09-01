namespace ClinicsProgram.Exports
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Windows.Forms;
    using System.Configuration;
    using System.Data.SQLite;
    using System.Data;
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
            DataTable specialists = ReadFromMySql();
            DataTable procedures = ReadFromSQLite();

            DataTable joined = JoinDataTables(
                specialists, procedures, "Name",
                (s, p) => s.Field<string>("Procedure") == p.Field<string>("Name"));

            //PrintJoined(joined);

            SaveDataToExcel(joined);
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

        private MySqlConnection GetMySqlConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicsMySQL"].ConnectionString;
            return new MySqlConnection(connStr);
        }

        private SQLiteConnection GetSQLiteConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicsSQLite"].ConnectionString;
            return new SQLiteConnection(connStr);
        }

        // Using http://epplus.codeplex.com/
        private void SaveDataToExcel(DataTable dataTable)
        {
            string fileName = "stats";
            string fileNameWithDate = string.Format("{0}_{1}.xlsx", fileName, DateTime.Now.ToString("dd-MM-yyyy"));
            string fileNameWithPath = "../../Reports/" + fileNameWithDate;

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

                //Step 4 : Save all changes to ExcelPackage object which will create Excel 2007 file.
                package.Save();

                MessageBox.Show(
                    string.Format("File name '{0}' generated successfully.", fileName),
                    "File generated successfully!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private DataTable JoinDataTables(DataTable t1, DataTable t2, string joinCol, params Func<DataRow, DataRow, bool>[] joinOn)
        {
            DataTable result = new DataTable();
            foreach (DataColumn col in t1.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataColumn col in t2.Columns)
            {
                if (result.Columns[col.ColumnName] == null && col.ColumnName != joinCol)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataRow row1 in t1.Rows)
            {
                var joinRows = t2.AsEnumerable().Where(row2 =>
                {
                    foreach (var parameter in joinOn)
                    {
                        if (!parameter(row1, row2)) return false;
                    }
                    return true;
                });
                foreach (DataRow fromRow in joinRows)
                {
                    DataRow insertRow = result.NewRow();
                    foreach (DataColumn col1 in t1.Columns)
                    {
                        insertRow[col1.ColumnName] = row1[col1.ColumnName];
                    }
                    foreach (DataColumn col2 in t2.Columns)
                    {
                        if (col2.ColumnName != joinCol)
                            insertRow[col2.ColumnName] = fromRow[col2.ColumnName];
                    }
                    result.Rows.Add(insertRow);
                }
            }
            return result;
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
