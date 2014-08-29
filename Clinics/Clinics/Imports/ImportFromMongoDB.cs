namespace ClinicsProgram.Imports
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Clinics.Data;
    using Clinics.Models;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public partial class ImportFromMongoDB : Form
    {
        private const string MongoUri = "mongodb://telerik:teamwork2014@ds050077.mongolab.com:50077/telerik";
        private static MongoUrl mongoUrl = new MongoUrl(MongoUri);
        private static MongoClient mongoClient = new MongoClient(mongoUrl);
        private static MongoServer mongoServer = mongoClient.GetServer();
        private static MongoDatabase mongoDb = mongoServer.GetDatabase(mongoUrl.DatabaseName);
        private IClinicsData data = new ClinicsData();

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
                var titles = mongoDb.GetCollection<BsonDocument>("Titles");
                this.ImportTitles(titles);

                var procedures = mongoDb.GetCollection<BsonDocument>("Procedures");
                this.ImportProcedures(procedures);

                var specialties = mongoDb.GetCollection<BsonDocument>("Specialties");
                this.ImportSpecialties(specialties);

                var specialists = mongoDb.GetCollection<BsonDocument>("Specialists");
                this.ImportSpecialists(specialists);

                var clinics = mongoDb.GetCollection<BsonDocument>("Clinics");
                this.ImportClinics(clinics);

                this.data.SaveChanges();
                mongoServer.Disconnect();
            }
            catch (MongoConnectionException er)
            {
                MessageBox.Show(er.Message);
            }          
            catch (MongoAuthenticationException er)
            {
                MessageBox.Show(er.Message);
            }
            catch (MongoQueryException er)
            {
                MessageBox.Show(er.Message);
            }
            catch (MongoException er)
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

                var exists = this.data.Titles.All()
                    .Where(t => t.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Title newTitle = new Title
                    {
                        TitleName = mongoTitle,
                        Id = idGuid
                    };

                    this.data.Titles.Add(newTitle);                  
                }
            }
        }

        private void ImportClinics(MongoCollection<BsonDocument> clinics)
        {
            var allClinics = clinics.FindAll();

            foreach (var clinic in allClinics)
            {
                var id = clinic["Id"].ToString();
                var idGuid = new Guid(id);
                var name = clinic["ClinicName"].ToString();
                var address = clinic["ClinicAddress"].ToString();
                var phones = clinic["ClinicPhones"].ToString();
                var chieff = clinic["ClinicChiefId"].ToString();
                var chieffdGuid = new Guid(chieff);

                var exists = this.data.Clinics.All()
                    .Where(c => c.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Clinic newClinic = new Clinic
                    {
                        Id = idGuid,
                        ClinicName = name,
                        ClinicAddress = address,
                        ClinicPhones = phones,
                        ClinicChiefId = chieffdGuid
                    };

                    this.data.Clinics.Add(newClinic);
                }
            }
        }

        private void ImportProcedures(MongoCollection<BsonDocument> procedures)
        {
            var allProcedures = procedures.FindAll();

            foreach (var procedure in allProcedures)
            {
                var id = procedure["Id"].ToString();
                var idGuid = new Guid(id);
                var name = procedure["Name"].ToString();
                var price = decimal.Parse(procedure["Price"].ToString());
                var information = procedure["Information"].ToString();

                var exists = this.data.Procedures.All()
                    .Where(p => p.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Procedure newProcedure = new Procedure
                    {
                        Id = idGuid,
                        Name = name,
                        Price = price,
                        Information = information
                    };

                    this.data.Procedures.Add(newProcedure);
                }
            }
        }

        private void ImportSpecialists(MongoCollection<BsonDocument> specialists)
        {
            var allSpecialists = specialists.FindAll();

            foreach (var specialist in allSpecialists)
            {
                var mongoId = specialist["Id"].ToString();
                var idGuid = new Guid(mongoId);
                var firstName = specialist["FirstName"].ToString();
                var middleName = specialist["MiddleName"].ToString();
                var lastName = specialist["LastName"].ToString();
                var title = specialist["TitleId"].ToString();
                var titleGuid = new Guid(title);
                var specialty = specialist["SpecialtyId"].ToString();
                var specialtyGuid = new Guid(specialty);
                var uin = specialist["Uin"].ToString();

                var exists = this.data.Specialists.All()
                    .Where(s => s.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Specialist newSpecialist = new Specialist
                    {
                        Id = idGuid,
                        FirstName = firstName,
                        MiddleName = middleName,
                        LastName = lastName,
                        SpecialtyId = specialtyGuid,
                        TitleId = titleGuid,
                        Uin = uin
                    };

                    this.data.Specialists.Add(newSpecialist);
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

                var exists = this.data.Specialties.All()
                    .Where(s => s.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Specialty newSpecialty = new Specialty
                    {
                        Id = idGuid,
                        Speciality = specialtyName
                    };

                    this.data.Specialties.Add(newSpecialty);
                }
            }
        }
    }
}
