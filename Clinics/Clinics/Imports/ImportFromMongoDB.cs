namespace ClinicsProgram.Imports
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public partial class ImportFromMongoDB : Form
    {
        private const string MongoUri = "mongodb://telerik:teamwork2014@ds050077.mongolab.com:50077/telerik";
        private static MongoUrl mongoUrl = new MongoUrl(MongoUri);
        private static MongoClient mongoClient = new MongoClient(mongoUrl);
        private static MongoServer mongoServer = mongoClient.GetServer();
        private static MongoDatabase mongoDb = mongoServer.GetDatabase(mongoUrl.DatabaseName);
        private MsSQLServerEntities msSQLServerEntities = new MsSQLServerEntities();

        public ImportFromMongoDB()
        {
            this.InitializeComponent();
        }

        private void ImportFromMongo_Click(object sender, EventArgs e)
        {
            var titles = mongoDb.GetCollection<BsonDocument>("Titles");
            var clinics = mongoDb.GetCollection<BsonDocument>("Clinics");
            var procedures = mongoDb.GetCollection<BsonDocument>("Procedures");
            var specialists = mongoDb.GetCollection<BsonDocument>("Specialists");
            var specialistsTitles = mongoDb.GetCollection<BsonDocument>("SpecialistsTitles");
            var specialties = mongoDb.GetCollection<BsonDocument>("Specialties");

            this.ImportTitles(titles);
            this.ImportClinics(clinics);

            mongoServer.Disconnect();
        }

        private void ImportTitles(MongoCollection<BsonDocument> titles)
        {
            var allTitles = titles.FindAll();

            foreach (var title in allTitles)
            {
                var mongoId = title["TitleId"].ToString();
                var guid = new Guid(mongoId);
                var mongoTitle = title["Title"].ToString();

                var test = title.Elements.ToString();

                IQueryable<Titles> exists =
                    from t in this.msSQLServerEntities.Titles
                    where t.TitleId.Equals(guid)
                    select t;

                if (exists.Count() == 0)
                {
                    Titles newTitle = new Titles
                    {
                        Title = mongoTitle,
                        TitleId = guid
                    };

                    this.msSQLServerEntities.Titles.Add(newTitle);
                    this.msSQLServerEntities.SaveChanges();
                }
            }
        }

        private void ImportClinics(MongoCollection<BsonDocument> clinics)
        {
            var allClinics = clinics.FindAll().Fields;

            //foreach (var clinic in allClinics)
            //{
            //    var mongoTitle = clinic["Title"].ToString();
            //    var mongoId = clinic["TitleId"].ToString();

            //    var guid = new Guid(mongoId);

            //    IQueryable<Clinics> exists =
            //        from t in this.msSQLServerEntities.Clinics
            //        where t.ClinicId.Equals(guid)
            //        select t;

            //    if (exists.Count() == 0)
            //    {
            //        Clinics newClinic = new Clinics
            //        {
            //            ClinicId = guid
            //        };

            //        this.msSQLServerEntities.Clinics.Add(newClinic);
            //        this.msSQLServerEntities.SaveChanges();
            //    }
            //}
        }


        private void button3_Click(object sender, EventArgs e)
        {
            MsSQLServerEntities telerikEntities = new MsSQLServerEntities();

            IQueryable<Specialties> specialities =
            from c in telerikEntities.Specialties
            where c.Speciality == "Test"
            select c;

            foreach (var item in specialities)
            {
                textBox1.Text = item.SpecialityId.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MsSQLServerEntities telerikEntities = new MsSQLServerEntities();

            Specialties newSpecialty = new Specialties
            {
                SpecialityId = Guid.NewGuid(),
                Speciality = "Import from the program123"
            };

            telerikEntities.Specialties.Add(newSpecialty);

            telerikEntities.SaveChanges();

        }
    }
}
