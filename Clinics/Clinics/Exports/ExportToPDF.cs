namespace ClinicsProgram.Exports
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using Clinics.Data;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System.Text;

    public partial class ExportToPDF : Form
    {
        public ExportToPDF()
        {
            this.InitializeComponent();
        }

        private void ExportToPDF_click(object sender, EventArgs e)
        {
            int month = this.Month.SelectedIndex + 1;
            int year = int.Parse(this.Year.Text);

            IClinicsData db = new ClinicsData();
            var dbManipulations = db.Manipulations.All().Where(m => m.Date.Year == year && m.Date.Month == month);
            var dbProcedures = db.Procedures.All().ToList();
            var dbPatients = db.Patients.All().ToList();
            var dbSpecialists = db.Specialists.All().ToList();
            var dbTitles = db.Titles.All().ToList();

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("../../Reports/test.pdf", FileMode.Create));
            doc.Open();

            //Create title header
            PdfPTable titleHeader = new PdfPTable(1);

            
            PdfPCell cellHeader = new PdfPCell(MakeBoldPhrase("Aggregated Procedures Report"));
            cellHeader.HorizontalAlignment = 1;

            titleHeader.AddCell(cellHeader);

            //Create table header
            PdfPTable tableHeader = new PdfPTable(5);
            PdfPCell cellHeaderDate = new PdfPCell(new Phrase("Date: " + month + "-" + year));
            cellHeaderDate.BackgroundColor = new BaseColor(217, 217, 217);
            cellHeaderDate.Colspan = 5;

            PdfPCell procedure = new PdfPCell(MakeBoldPhrase("Procedure"));
            procedure.BackgroundColor = new BaseColor(217, 217, 217);

            PdfPCell manipulation = new PdfPCell(MakeBoldPhrase("Manipulation"));
            manipulation.BackgroundColor = new BaseColor(217, 217, 217);

            PdfPCell date = new PdfPCell(MakeBoldPhrase("Date"));
            date.BackgroundColor = new BaseColor(217, 217, 217);

            PdfPCell patient = new PdfPCell(MakeBoldPhrase("Patient"));
            patient.BackgroundColor = new BaseColor(217, 217, 217);

            PdfPCell specialist = new PdfPCell(MakeBoldPhrase("Specialist"));
            specialist.BackgroundColor = new BaseColor(217, 217, 217);

            tableHeader.AddCell(cellHeaderDate);
            tableHeader.AddCell(procedure);
            tableHeader.AddCell(manipulation);
            tableHeader.AddCell(date);
            tableHeader.AddCell(patient);
            tableHeader.AddCell(specialist);

            //Create table body
            PdfPTable tableBody = new PdfPTable(5);

            foreach (var man in dbManipulations)
            {
                //"Procedure"
                var currentProcedure = dbProcedures.Where(p => p.Id == man.ProcedureId).FirstOrDefault();
                tableBody.AddCell(currentProcedure.Name);

                //"Manipulation"
                tableBody.AddCell(man.Information);

                tableBody.AddCell(man.Date.Day + "-" + man.Date.Month + "-" + man.Date.Year);

                //"Patient"
                var currentPatient = dbPatients.Where(p => p.Id == man.PatientId)
                                                .FirstOrDefault();
                tableBody.AddCell(currentPatient.Abreviature + " " + currentPatient.Age + " yrs");

                //"Specialist"
                var currentSpecialist = dbSpecialists.Where(s => s.Id == man.SpecialistId).FirstOrDefault();
                var currentSpecialistTitles = dbTitles.Where(t => t.Id == currentSpecialist.TitleId).FirstOrDefault();

                tableBody.AddCell(currentSpecialistTitles.TitleName + " " + currentSpecialist.FirstName + " " +
                    currentSpecialist.MiddleName + " " + currentSpecialist.LastName);
            }

            //Create footer
            PdfPTable footer = new PdfPTable(5);

            PdfPCell totalManipulationsTextCell = new PdfPCell(new Phrase("Total manipulations: "));
            totalManipulationsTextCell.Colspan = 4;
            totalManipulationsTextCell.HorizontalAlignment = 2;
            footer.AddCell(totalManipulationsTextCell);

            PdfPCell TotalManipulationsCell = new PdfPCell(new Phrase(dbManipulations.ToList().Count.ToString()));
            TotalManipulationsCell.HorizontalAlignment = 1;
            footer.AddCell(TotalManipulationsCell);

            //Add to document
            doc.Add(titleHeader);
            doc.Add(tableHeader);
            doc.Add(tableBody);
            doc.Add(footer);

            doc.Close();

            MessageBox.Show("PDF report generated in the Reports folder.");
        }

        private Phrase MakeBoldPhrase(string text) 
        {
            var phrase = new Phrase();
            phrase.Add(new Chunk(text, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));

            return phrase;
        }
    }
}
