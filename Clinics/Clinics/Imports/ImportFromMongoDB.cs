namespace ClinicsProgram.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Windows.Forms;

    using Clinics.Data;
    using Clinics.Models;
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

                var titles = mongoDb.GetCollection<BsonDocument>("Titles");
                this.ImportTitles(titles);
                this.importProgress.Value++;

                var procedures = mongoDb.GetCollection<BsonDocument>("Procedures");
                this.ImportProcedures(procedures);
                this.importProgress.Value++;

                var specialties = mongoDb.GetCollection<BsonDocument>("Specialties");
                this.ImportSpecialties(specialties);
                this.importProgress.Value++;

                var specialists = mongoDb.GetCollection<BsonDocument>("Specialists");
                this.ImportSpecialists(specialists);
                this.importProgress.Value++;

                var clinics = mongoDb.GetCollection<BsonDocument>("Clinics");
                this.ImportClinics(clinics);
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
                var mongoId = this.GetValue(title, "TitleId");
                var idGuid = new Guid(mongoId);

                string titleName = this.GetValue(title, "Title");

                var exists = this.data.Titles.All()
                    .Where(t => t.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (exists == null)
                {
                    Title newTitle = new Title
                    {
                        Id = idGuid,
                        TitleName = titleName
                    };

                    this.data.Titles.Add(newTitle);                  
                }
                else
                {
                    var existingTitles = this.data.Titles.SearchFor(t => t.Id.Equals(idGuid)).First();
                    existingTitles.TitleName = titleName;
                }
            }
        }

        private void ImportClinics(MongoCollection<BsonDocument> clinics)
        {
            var allClinics = clinics.FindAll();

            foreach (var clinic in allClinics)
            {
                var id = this.GetValue(clinic, "Id");
                var idGuid = new Guid(id);

                string name = this.GetValue(clinic, "ClinicName");
                string address = this.GetValue(clinic, "ClinicAddress");
                var phones = this.GetValue(clinic, "ClinicPhones");

                var chieff = this.GetValue(clinic, "ClinicChiefId");
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
                else
                {
                    var existingClinic = this.data.Clinics.SearchFor(p => p.Id.Equals(idGuid)).First();
                    existingClinic.ClinicName = name;
                    existingClinic.ClinicAddress = address;
                    existingClinic.ClinicPhones = phones;
                    existingClinic.ClinicChiefId = chieffdGuid;
                }
            }
        }

        private void ImportProcedures(MongoCollection<BsonDocument> procedures)
        {
            var allProcedures = procedures.FindAll();

            foreach (var procedure in allProcedures)
            {
                var idGuid = procedure["_id"].AsGuid;
                var name = this.GetValue(procedure, "Name");
                var price = decimal.Parse(this.GetValue(procedure, "Price"));
                var information = this.GetValue(procedure, "Information");

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
                else
                {
                    var existingProcedure = this.data.Procedures.SearchFor(p => p.Id.Equals(idGuid)).First();
                    existingProcedure.Name = name;
                    existingProcedure.Price = price;
                    existingProcedure.Information = information;
                }
            }
        }

        private void ImportSpecialists(MongoCollection<BsonDocument> specialists)
        {
            var allSpecialists = specialists.FindAll();

            foreach (var specialist in allSpecialists)
            {
                var mongoId = this.GetValue(specialist, "Id");
                var idGuid = new Guid(mongoId);

                var firstName = this.GetValue(specialist, "FirstName");
                var middleName = this.GetValue(specialist, "MiddleName");
                var lastName = this.GetValue(specialist, "LastName");

                var title = this.GetValue(specialist, "TitleId");
                var titleGuid = new Guid(title);

                var specialty = this.GetValue(specialist, "SpecialtyId");
                var specialtyGuid = new Guid(specialty);

                var uin = this.GetValue(specialist, "Uin");

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
                else
                {
                    var existingSpecialist = this.data.Specialists.SearchFor(s => s.Id.Equals(idGuid)).First();
                    existingSpecialist.FirstName = firstName;
                    existingSpecialist.MiddleName = middleName;
                    existingSpecialist.LastName = lastName;
                    existingSpecialist.SpecialtyId = specialtyGuid;
                    existingSpecialist.TitleId = titleGuid;
                    existingSpecialist.Uin = uin;
                }
            }
        }

        private void ImportSpecialties(MongoCollection<BsonDocument> specialies)
        {
            var allSpecialties = specialies.FindAll();

            foreach (var specialty in allSpecialties)
            {
                var id = this.GetValue(specialty, "SpecialtyId");
                var idGuid = new Guid(id);
                var specialtyName = this.GetValue(specialty, "Specialty");

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
                else
                {
                    var existingSpecialty = this.data.Specialties.SearchFor(s => s.Id.Equals(idGuid)).First();
                    existingSpecialty.Speciality = specialtyName;
                }
            }
        }

        private string GetValue(BsonDocument document, string key)
        {
            string value = string.Empty;
            if (document.Contains(key))
            {
                value = document[key].AsString;
            }

            return value;
        }
    }
}
