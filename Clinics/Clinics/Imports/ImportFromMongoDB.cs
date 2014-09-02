namespace ClinicsProgram.Imports
{
    using System;
    using System.Windows.Forms;

    using Clinics.Data;
    using Clinics.Operations.Imports;

    public partial class ImportFromMongoDB : Form
    {
        private const string SuccessMessage = "Importing data from MongoDB to SQL server done!";
        private IClinicsData data = new ClinicsData();
        private MongoImport mongoImport = new MongoImport();

        public ImportFromMongoDB()
        {
            this.InitializeComponent();
        }

        ~ImportFromMongoDB()  
        {
            this.data.Dispose();
        }

        private void ImportFromMongo_Click(object sender, EventArgs e)
        {
            try
            {
                this.mongoImport.Import(this.data);
                MessageBox.Show(SuccessMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
