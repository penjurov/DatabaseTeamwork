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
        private MsSQLServerEntities msSqlServerEntities = new MsSQLServerEntities();

        public ImportFromMongoDB()
        {
            this.InitializeComponent();
        }

        ~ImportFromMongoDB()  
        {
            msSqlServerEntities.Dispose();     
        }

        private void ImportFromMongo_Click(object sender, EventArgs e)
        {
            try
            {
                var titles = mongoDb.GetCollection<BsonDocument>("Titles");
                this.ImportTitles(titles);

                var clinics = mongoDb.GetCollection<BsonDocument>("Clinics");
                this.ImportClinics(clinics);

                var procedures = mongoDb.GetCollection<BsonDocument>("Procedures");
                this.ImportProcedures(procedures);

                var specialists = mongoDb.GetCollection<BsonDocument>("Specialists");
                this.ImportSpecialists(specialists);

                var specialties = mongoDb.GetCollection<BsonDocument>("Specialties");
                this.ImportSpecialties(specialties);

                this.msSqlServerEntities.SaveChanges();
                mongoServer.Disconnect();
            }
            catch(System.Data.Entity.Infrastructure.DbUpdateException er)
            {
                MessageBox.Show(er.Message);
            }
            catch (MongoConnectionException er)
            {
                MessageBox.Show(er.Message);
            }          
            catch(MongoAuthenticationException er)
            {
                MessageBox.Show(er.Message);
            }
            catch(MongoQueryException er)
            {
                MessageBox.Show(er.Message);
            }
            catch(MongoException er)
            {
                MessageBox.Show(er.Message);
            }          
        }

        private void ImportTitles(MongoCollection<BsonDocument> titles)
        {
            var allTitles = titles.FindAll();

            foreach (var title in allTitles)
            {
                var mongoId = title["TitleId"].ToString();
                var idGuid = new Guid(mongoId);
                var mongoTitle = title["Title"].ToString();

                var exists = this.msSqlServerEntities.Titles
                    .Where(t => t.TitleId.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Titles newTitle = new Titles
                    {
                        Title = mongoTitle,
                        TitleId = idGuid
                    };

                    this.msSqlServerEntities.Titles.Add(newTitle);                  
                }
            }
        }

        private void ImportClinics(MongoCollection<BsonDocument> clinics)
        {
            var allClinics = clinics.FindAll();

            foreach (var clinic in allClinics)
            {
                var id = clinic["ClinicId"].ToString();
                var idGuid = new Guid(id);
                var name = clinic["ClinicName"].ToString();
                var address = clinic["ClinicAddress"].ToString();
                var phones = clinic["ClinicPhone"].ToString();
                var chieff = clinic["ClinicChieff"].ToString();
                var chieffdGuid = new Guid(chieff);
             
                var exists = this.msSqlServerEntities.Clinics
                    .Where(c => c.ClinicId.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Clinics newClinic = new Clinics
                    {
                        ClinicId = idGuid,
                        ClinicName = name,
                        ClinicAddress = address,
                        ClinicPhones = phones,
                        ClinicChief = chieffdGuid
                    };

                    this.msSqlServerEntities.Clinics.Add(newClinic);
                }
            }
        }

        private void ImportProcedures(MongoCollection<BsonDocument> procedures)
        {
            var allProcedures = procedures.FindAll();

            foreach (var procedure in allProcedures)
            {
                var id = procedure["ProcedureId"].ToString();
                var idGuid = new Guid(id);
                var name = procedure["Name"].ToString();
                var iscCode= procedure["ISCCode"].ToString();
                var price = Decimal.Parse(procedure["Price"].ToString());
                var information = procedure["Information"].ToString();
                
                var exists = this.msSqlServerEntities.Procedures
                    .Where(p => p.ProcedureId.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Procedures newProcedure = new Procedures
                    {
                        ProcedureId = idGuid,
                        Name = name,
                        ISCCode = iscCode,
                        Price = price,
                        Information = information
                    };

                    this.msSqlServerEntities.Procedures.Add(newProcedure);
                }
            }
        }

        private void ImportSpecialists(MongoCollection<BsonDocument> specialists)
        {
            var allSpecialists = specialists.FindAll();

            foreach (var specialist in allSpecialists)
            {
                var mongoId = specialist["ProcedureId"].ToString();
                var idGuid = new Guid(mongoId);
                var firstName = specialist["FirstName"].ToString();
                var middleName = specialist["MiddleName"].ToString();
                var lastName = specialist["LastName"].ToString();
                var duty = specialist["Duty"].ToString();
                var title = int.Parse(specialist["Title"].ToString());
                var specialty = specialist["Specialty"].ToString();
                var specialtyGuid = new Guid(specialty);
                var uin = specialist["UIN"].ToString();
                
                var exists = this.msSqlServerEntities.Specialists
                    .Where(s => s.SpecialistId.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Specialists newSpecialist = new Specialists
                    {
                        SpecialistId = idGuid,
                        FirstName = firstName,
                        MiddleName = middleName,
                        LastName = lastName,
                        Duty = duty,
                        Title = title,
                        Specialty = specialtyGuid,
                        UIN = uin
                    };

                    this.msSqlServerEntities.Specialists.Add(newSpecialist);
                }
            }
        }

        private void ImportSpecialties(MongoCollection<BsonDocument> specialies)
        {
            var allSpecialties = specialies.FindAll();

            foreach (var specialty in allSpecialties)
            {
                var id = specialty["SpecialtyId"].ToString();
                var idGuid = new Guid(id);
                var specialtyName = specialty["Specialty"].ToString();

                
                var exists = this.msSqlServerEntities.Specialties
                    .Where(s => s.SpecialityId.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Specialties newSpecialty = new Specialties
                    {
                        SpecialityId = idGuid,
                        Speciality = specialtyName
                    };

                    this.msSqlServerEntities.Specialties.Add(newSpecialty);
                }
            }
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
        }
    }
}
