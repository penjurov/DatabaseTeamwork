namespace Clinics.Operations.Exports
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SQLite;
    using System.IO;
    using MySql.Data.MySqlClient;
    using OfficeOpenXml;
    using OfficeOpenXml.Table;

    public class ExcelExport
    { 
        public void Export()
        {
            DataTable specialists = this.ReadFromMySql();
            DataTable procedures = this.ReadFromSQLite();

            DataTable joined = this.JoinDataTables(specialists, procedures, "Name", (s, p) => s.Field<string>("Procedure") == p.Field<string>("Name"));

            this.SaveDataToExcel(joined);
        }

        private DataTable ReadFromMySql()
        {
            var dbCon = this.GetMySqlConnection();
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
            var dbCon = this.GetSQLiteConnection();
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
            string fileNameWithPath = string.Format("../../Reports/{0}", fileNameWithDate);

            // Delete existing file with same file name.
            if (File.Exists(fileNameWithPath))
            {
                File.Delete(fileNameWithPath);
            }

            this.GenerateExcel(dataTable, sheetName: "Joined", fileName: fileNameWithPath);
        }

        private void GenerateExcel(DataTable dataTable, string sheetName, string fileName)
        {
            var newFile = new FileInfo(fileName);

            // Step 1 : Create object of ExcelPackage class and pass file path to constructor.
            using (var package = new ExcelPackage(newFile))
            {
                // Step 2 : Add a new worksheet to ExcelPackage object and give a suitable name
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                // Step 3 : Start loading datatable form A1 cell of worksheet.
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true, TableStyles.None);

                // Step 4 : Save all changes to ExcelPackage object which will create Excel 2007 file.
                package.Save();
            }
        }

        private DataTable JoinDataTables(DataTable t1, DataTable t2, string joinCol, params Func<DataRow, DataRow, bool>[] joinOn)
        {
            DataTable result = new DataTable();
            foreach (DataColumn col in t1.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                {
                    result.Columns.Add(col.ColumnName, col.DataType);
                }
            }

            foreach (DataColumn col in t2.Columns)
            {
                if (result.Columns[col.ColumnName] == null && col.ColumnName != joinCol)
                {
                    result.Columns.Add(col.ColumnName, col.DataType);
                }
            }

            foreach (DataRow row1 in t1.Rows)
            {
                var joinRows = t2.AsEnumerable().Where(row2 =>
                {
                    foreach (var parameter in joinOn)
                    {
                        if (!parameter(row1, row2))
                        {
                            return false;
                        }
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
                        {
                            insertRow[col2.ColumnName] = fromRow[col2.ColumnName];
                        }
                    }

                    result.Rows.Add(insertRow);
                }
            }

            return result;
        }
    }
}