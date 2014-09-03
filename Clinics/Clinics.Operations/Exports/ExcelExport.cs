namespace Clinics.Operations.Exports
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SQLite;
    using System.IO;
    using Clinics.MySQLModels;
    using OfficeOpenXml;
    using OfficeOpenXml.Table;

    public class ExcelExport
    {
        public void Export(ClinicsMySQLContext mySqlContext)
        {
            DataTable procedures = this.ReadFromSQLite();
            DataTable joined = this.JoinData(mySqlContext, procedures, "Procedure", "Name");

            this.SaveDataToExcel(joined);
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
            string fileNameWithPath = string.Format("{1}/Reports/{0}", fileNameWithDate, Directory.GetCurrentDirectory());

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

        private DataTable JoinData(ClinicsMySQLContext mySqlContext, DataTable procedures, string joinProp, string joinCol)
        {
            DataTable result = this.GenerateExcelColumns(procedures, joinCol);

            // For each item in the collection fill a row in the excel table
            foreach (var row in mySqlContext.Specialiststatistics)
            {
                DataRow insertRow = result.NewRow();

                // Fill the properties of the object
                foreach (var prop in typeof(Specialiststatistic).GetProperties())
                {
                    insertRow[prop.Name] = row.GetType().GetProperty(prop.Name).GetValue(row, null);
                }

                // Find the matching row from the table
                string procName = row.GetType().GetProperty(joinProp).GetValue(row, null).ToString();
                foreach (DataRow proc in procedures.Rows)
                {
                    if ((string)proc[joinCol] == procName)
                    {
                        foreach (DataColumn procCol in procedures.Columns)
                        {
                            if (procCol.ColumnName != joinCol)
                            {
                                insertRow[procCol.ColumnName] = proc[procCol.ColumnName];
                            }
                        }

                        break;
                    }
                }

                result.Rows.Add(insertRow);
            }

            return result;
        }

        private DataTable GenerateExcelColumns(DataTable procedures, string joinCol)
        {
            DataTable result = new DataTable();

            // Add excel colums corresponding to the properties of the object
            foreach (var prop in typeof(Specialiststatistic).GetProperties())
            {
                result.Columns.Add(prop.Name, typeof(string));
            }

            // Add excel columns from the datatable colums
            foreach (DataColumn col in procedures.Columns)
            {
                if (col.ColumnName != joinCol)
                {
                    result.Columns.Add(col.ColumnName, col.DataType);
                }
            }

            return result;
        }
    }
}