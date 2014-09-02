namespace Clinics.Operations.Imports
{
    using System;
    using System.Linq;
    using System.Xml;

    using Clinics.Data;
    using Clinics.Models;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class XmlImport
    {
        private const string MongoUri = "mongodb://telerik:teamwork2014@ds050077.mongolab.com:50077/telerik";
        private static readonly MongoUrl mongoUrl = new MongoUrl(MongoUri);
        private static readonly MongoClient mongoClient = new MongoClient(mongoUrl);
        private static readonly MongoServer mongoServer = mongoClient.GetServer();
        private static readonly MongoDatabase mongoDb = mongoServer.GetDatabase(mongoUrl.DatabaseName);
        private readonly XmlDocument doc = new XmlDocument();

        public void Import(IClinicsData data, string fileName)
        {
            this.doc.Load(fileName);
            XmlNode rootNode = this.doc.DocumentElement;
            
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                var name = this.GetValue(node, "name");
                var price = decimal.Parse(this.GetValue(node, "price"));
                var information = this.GetValue(node, "information");

                this.InportInMongo(name, price, information);
                this.ImportInSql(data, name, price, information);
            }

            data.SaveChanges();
        }

        private void ImportInSql(IClinicsData data, string name, decimal price, string information)
        {
            var exists = data.Procedures.All()
                .Where(p => p.Name.Equals(name))
                .FirstOrDefault();

            if (exists == null)
            {
                Procedure procedure = new Procedure
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Price = price,
                    Information = information
                };

                data.Procedures.Add(procedure);
            }
        }

        private void InportInMongo(string name, decimal price, string information)
        {
            var procedures = mongoDb.GetCollection<BsonDocument>("Procedures");

            var exists = procedures.FindAll().Where(p => p["Name"].Equals(name)).FirstOrDefault();

            if (exists == null)
            {
                Procedure procedure = new Procedure
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Price = price,
                    Information = information
                };

                procedures.Insert<Procedure>(procedure);
            }
        }

        private string GetValue(XmlNode node, string key)
        {
            string value = string.Empty;

            if (node[key] != null)
            {
                value = node[key].InnerText;
            }

            return value;
        }
    }
}
