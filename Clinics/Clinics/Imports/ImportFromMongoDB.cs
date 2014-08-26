namespace ClinicsProgram.Imports
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    public partial class ImportFromMongoDB : Form
    {
        public ImportFromMongoDB()
        {
            InitializeComponent();
        }

        private void ImportFromMongo_Click(object sender, EventArgs e)
        {
            telerikEntities telerikEntities = new telerikEntities();

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
            telerikEntities telerikEntities = new telerikEntities();

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
