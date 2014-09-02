namespace ClinicsProgram.Imports
{
    using System;
    using System.Windows.Forms;

    using Clinics.Operations.Imports;
       
    public partial class ImportFromXml : Form
    {
        private const string Filter = "xml files (*.xml)|*.xml";
        private const string SuccessMessage = "Importing data from choosed XML file to MongoDB done!";
        private const string FileNotSelectMessage = "Please choose xml file!";
        private XmlImport xmlImport = new XmlImport();

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

            ofd.Filter = Filter;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.fileName.Text = ofd.FileName;
                }     
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {   
            try
            {
                if (this.fileName.Text != string.Empty)
                {
                    this.xmlImport.Import(this.fileName.Text);
                    MessageBox.Show(SuccessMessage);
                }
                else
                {
                    MessageBox.Show(FileNotSelectMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                              
        }
    }
}
