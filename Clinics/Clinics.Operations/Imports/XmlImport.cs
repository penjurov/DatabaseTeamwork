namespace Clinics.Operations.Imports
{
    using System;
    using System.Linq;
    using System.Xml;

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

        public void Import(string fileName)
        {
            this.doc.Load(fileName);
            XmlNode rootNode = this.doc.DocumentElement;
            var procedures = mongoDb.GetCollection<BsonDocument>("Procedures");

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                var name = this.GetValue(node, "name");
                var price = decimal.Parse(this.GetValue(node, "price"));
                var information = this.GetValue(node, "information");

                var exist = procedures.FindAll().Where(p => p["Name"].Equals(name)).FirstOrDefault();

                if (exist == null)
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
