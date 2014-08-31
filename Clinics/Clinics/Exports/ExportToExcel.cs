namespace ClinicsProgram.Exports
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Windows.Forms;
    using System.Configuration;
    using System.Data.SQLite;
    using System.Data;

    public partial class ExportToExcel : Form
    {
        public ExportToExcel()
        {
            this.InitializeComponent();
        }

        private void ExportToExcel_Click(object sender, EventArgs e)
        {
            ConnectToSQLite();

            // Connect to MySQL
        }

        private static void ConnectToSQLite()
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
        }

        static DataTable GetProcedures(SQLiteConnection dbCon)
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

        private static SQLiteConnection GetSQLiteConnection()
        {
            return new SQLiteConnection("Data Source=" + @"..\..\..\Database\ClinicsLite.sqlite" + ";Version=3;");
        }

        private static MySqlConnection GetMySqlConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ClinicsMySQL"].ConnectionString;
            return new MySqlConnection(connStr);
        }
    }
}
