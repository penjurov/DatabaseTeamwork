namespace ClinicsProgram.Imports
{
    using System;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Windows.Forms;

    using Clinics.Data;
    using Clinics.Operations.Imports;
    using MongoDB.Bson;
    using MongoDB.Driver;
    

    public partial class ImportFromMongoDB : Form
    {
        private const int NumberOfTables = 5;
        private const string MongoUri = "mongodb://telerik:teamwork2014@ds050077.mongolab.com:50077/telerik";        
        private static MongoUrl mongoUrl = new MongoUrl(MongoUri);
        private static MongoClient mongoClient = new MongoClient(mongoUrl);
        private static MongoServer mongoServer = mongoClient.GetServer();
        private static MongoDatabase mongoDb = mongoServer.GetDatabase(mongoUrl.DatabaseName);
        private IClinicsData data = new ClinicsData();
        private ImportFromMongo mongoImport = new ImportFromMongo();

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
                this.importProgress.Value = 0;
                this.importProgress.Maximum = NumberOfTables;
               
                mongoImport.ImportTitles(data);
                this.importProgress.Value++;

                mongoImport.ImportProcedures(data);
                this.importProgress.Value++;

                mongoImport.ImportSpecialties(data);
                this.importProgress.Value++;

                mongoImport.ImportSpecialists(data);
                this.importProgress.Value++;

                mongoImport.ImportClinics(data);
                this.importProgress.Value++;

                this.data.SaveChanges();
                mongoServer.Disconnect();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
