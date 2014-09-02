namespace Clinics.Operations.Imports
{
    using System;
    using System.Linq;

    using Clinics.Data;
    using Clinics.Models;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoImport
    {
        private const int NumberOfTables = 5;
        private const string MongoUri = "mongodb://telerik:teamwork2014@ds050077.mongolab.com:50077/telerik";        
        private static readonly MongoUrl mongoUrl = new MongoUrl(MongoUri);
        private static readonly MongoClient mongoClient = new MongoClient(mongoUrl);
        private static readonly MongoServer mongoServer = mongoClient.GetServer();
        private static readonly MongoDatabase mongoDb = mongoServer.GetDatabase(mongoUrl.DatabaseName);

        public void Import(IClinicsData data)
        {
            this.ImportTitles(data);
            this.ImportProcedures(data);
            this.ImportSpecialties(data);
            this.ImportSpecialists(data);
            this.ImportClinics(data);

            data.SaveChanges();
        }

        private void ImportTitles(IClinicsData data)
        {
            var allTitles = mongoDb.GetCollection<BsonDocument>("Titles").FindAll();;

            foreach (var title in allTitles)
            {
                var mongoId = this.GetValue(title, "TitleId");
                var idGuid = new Guid(mongoId);
                string titleName = this.GetValue(title, "Title");

                var existingRecord = data.Titles.All()
                    .Where(t => t.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (existingRecord == null)
                {
                    Title newTitle = new Title
                    {
                        Id = idGuid,
                        TitleName = titleName
                    };

                    data.Titles.Add(newTitle);                  
                }
                else
                {
                    existingRecord.TitleName = titleName;
                }
            }
        }

        private void ImportClinics(IClinicsData data)
        {
            var allClinics = mongoDb.GetCollection<BsonDocument>("Clinics").FindAll();

            foreach (var clinic in allClinics)
            {
                var id = this.GetValue(clinic, "Id");
                var idGuid = new Guid(id);

                string name = this.GetValue(clinic, "ClinicName");
                string address = this.GetValue(clinic, "ClinicAddress");
                var phones = this.GetValue(clinic, "ClinicPhones");

                var chieff = this.GetValue(clinic, "ClinicChiefId");
                var chieffdGuid = new Guid(chieff);

                var existingRecord = data.Clinics.All()
                    .Where(c => c.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (existingRecord == null)
                {
                    Clinic newClinic = new Clinic
                    {
                        Id = idGuid,
                        ClinicName = name,
                        ClinicAddress = address,
                        ClinicPhones = phones,
                        ClinicChiefId = chieffdGuid
                    };

                    data.Clinics.Add(newClinic);
                }
                else
                {
                    existingRecord.ClinicName = name;
                    existingRecord.ClinicAddress = address;
                    existingRecord.ClinicPhones = phones;
                    existingRecord.ClinicChiefId = chieffdGuid;
                }
            }
        }

        private void ImportProcedures(IClinicsData data)
        {
            var allProcedures = mongoDb.GetCollection<BsonDocument>("Procedures").FindAll();

            foreach (var procedure in allProcedures)
            {
                var idGuid = procedure["_id"].AsGuid;
                var name = this.GetValue(procedure, "Name");
                var price = decimal.Parse(this.GetValue(procedure, "Price"));
                var information = this.GetValue(procedure, "Information");

                var existingRecord = data.Procedures.All()
                    .Where(p => p.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (existingRecord == null)
                {
                    Procedure newProcedure = new Procedure
                    {
                        Id = idGuid,
                        Name = name,
                        Price = price,
                        Information = information
                    };

                    data.Procedures.Add(newProcedure);
                }
                else
                {
                    existingRecord.Name = name;
                    existingRecord.Price = price;
                    existingRecord.Information = information;
                }
            }
        }

        private void ImportSpecialists(IClinicsData data)
        {
            var allSpecialists = mongoDb.GetCollection<BsonDocument>("Specialists").FindAll();

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

                var existingRecord = data.Specialists.All()
                    .Where(s => s.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (existingRecord == null)
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

                    data.Specialists.Add(newSpecialist);
                }
                else
                {
                    existingRecord.FirstName = firstName;
                    existingRecord.MiddleName = middleName;
                    existingRecord.LastName = lastName;
                    existingRecord.SpecialtyId = specialtyGuid;
                    existingRecord.TitleId = titleGuid;
                    existingRecord.Uin = uin;
                }
            }
        }

        private void ImportSpecialties(IClinicsData data)
        {
            var allSpecialties = mongoDb.GetCollection<BsonDocument>("Specialties").FindAll();

            foreach (var specialty in allSpecialties)
            {
                var id = this.GetValue(specialty, "SpecialtyId");
                var idGuid = new Guid(id);
                var specialtyName = this.GetValue(specialty, "Specialty");

                var existingRecord = data.Specialties.All()
                    .Where(s => s.Id.Equals(idGuid))
                    .FirstOrDefault();

                if (existingRecord == null)
                {
                    Specialty newSpecialty = new Specialty
                    {
                        Id = idGuid,
                        Speciality = specialtyName
                    };

                    data.Specialties.Add(newSpecialty);
                }
                else
                {
                    existingRecord.Speciality = specialtyName;
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
