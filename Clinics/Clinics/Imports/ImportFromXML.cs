namespace ClinicsProgram.Imports
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using System.Xml;

    using Clinics.Models;
    using MongoDB.Bson;
    using MongoDB.Driver;
       
    public partial class ImportFromXml : Form
    {
        private const string MongoUri = "mongodb://telerik:teamwork2014@ds050077.mongolab.com:50077/telerik";
        private static MongoUrl mongoUrl = new MongoUrl(MongoUri);
        private static MongoClient mongoClient = new MongoClient(mongoUrl);
        private static MongoServer mongoServer = mongoClient.GetServer();
        private static MongoDatabase mongoDb = mongoServer.GetDatabase(mongoUrl.DatabaseName);
        private XmlDocument doc = new XmlDocument();

        public ImportFromXml()
        {
            this.InitializeComponent();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            this.FileSelect();
        }

        private void FileSelect()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "xml files (*.xml)|*.xml";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.fileName.Text = ofd.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {           
            this.Import();           
        }

        private void Import()
        {
            if (this.fileName.Text != string.Empty)
            {
                this.doc.Load(this.fileName.Text);
                XmlNode rootNode = this.doc.DocumentElement;
                var procedures = mongoDb.GetCollection<BsonDocument>("Procedures");

                this.importProgress.Visible = true;
                this.importProgress.Value = 0;
                this.importProgress.Maximum = rootNode.ChildNodes.Count - 1;

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

                    if (this.importProgress.Value < this.importProgress.Maximum)
                    {
                        this.importProgress.Value++;
                    }                   
                }              
            }
            else
            {
                MessageBox.Show("Please choose xml file!");
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
