﻿namespace Clinics.Operations.Exports
{
    using System;
    using System.IO;
    using System.Linq;

    using Clinics.Data;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class PdfExport
    {
        private const string FileName = "../../Reports/Report.pdf";
        private const string FileHeader = "Aggregated Procedures Report";
        private const string FileFooter = "Total manipulations: ";

        public void Export(int month, int year)
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(FileName, FileMode.Create));
            
            doc.Open();

            this.CreateTitleHeader(doc);
            this.CreateTableHeader(doc, month, year);
            this.CreateTable(doc, month, year);

            doc.Close();
        }

        private void CreateTitleHeader(Document doc)
        {
            PdfPTable titleHeader = new PdfPTable(1);
           
            PdfPCell cellHeader = new PdfPCell(this.MakeBoldPhrase(FileHeader));
            cellHeader.HorizontalAlignment = 1;

            titleHeader.AddCell(cellHeader);

            doc.Add(titleHeader);
        }

        private void CreateTableHeader(Document doc, int month, int year)
        {
            PdfPTable tableHeader = new PdfPTable(5);
            PdfPCell cellHeaderDate = new PdfPCell(new Phrase("Period: " + month + "-" + year));
            cellHeaderDate.BackgroundColor = new BaseColor(217, 217, 217);
            cellHeaderDate.Colspan = 5;

            PdfPCell procedure = new PdfPCell(this.MakeBoldPhrase("Procedure"));
            procedure.BackgroundColor = new BaseColor(217, 217, 217);

            PdfPCell manipulation = new PdfPCell(this.MakeBoldPhrase("Manipulation"));
            manipulation.BackgroundColor = new BaseColor(217, 217, 217);

            PdfPCell date = new PdfPCell(this.MakeBoldPhrase("Date"));
            date.BackgroundColor = new BaseColor(217, 217, 217);

            PdfPCell patient = new PdfPCell(this.MakeBoldPhrase("Patient"));
            patient.BackgroundColor = new BaseColor(217, 217, 217);

            PdfPCell specialist = new PdfPCell(this.MakeBoldPhrase("Specialist"));
            specialist.BackgroundColor = new BaseColor(217, 217, 217);

            tableHeader.AddCell(cellHeaderDate);
            tableHeader.AddCell(procedure);
            tableHeader.AddCell(manipulation);
            tableHeader.AddCell(date);
            tableHeader.AddCell(patient);
            tableHeader.AddCell(specialist);

            doc.Add(tableHeader);
        }

        private void CreateTable(Document doc, int month, int year)
        {
            IClinicsData db = new ClinicsData();
            var dbManipulations = db.Manipulations.All()
                .Where(m => m.Date.Year == year && m.Date.Month == month)
                .OrderBy(m => m.Date)
                .ThenBy(m => m.Specialist.FirstName);

            var dbProcedures = db.Procedures.All().ToList();
            var dbPatients = db.Patients.All().ToList();
            var dbSpecialists = db.Specialists.All().ToList();
            var dbTitles = db.Titles.All().ToList();

            PdfPTable tableBody = new PdfPTable(5);

            foreach (var man in dbManipulations)
            {
                // "Procedure"
                var currentProcedure = dbProcedures.Where(p => p.Id == man.ProcedureId).FirstOrDefault();
                tableBody.AddCell(currentProcedure.Name);

                // "Manipulation"
                tableBody.AddCell(man.Information);

                tableBody.AddCell(man.Date.Day + "-" + man.Date.Month + "-" + man.Date.Year);

                // "Patient"
                var currentPatient = dbPatients.Where(p => p.Id == man.PatientId)
                                                .FirstOrDefault();
                tableBody.AddCell(currentPatient.Abreviature + " " + currentPatient.Age + " yrs");

                // "Specialist"
                var currentSpecialist = dbSpecialists.Where(s => s.Id == man.SpecialistId).FirstOrDefault();
                var currentSpecialistTitles = dbTitles.Where(t => t.Id == currentSpecialist.TitleId).FirstOrDefault();

                tableBody.AddCell(currentSpecialistTitles.TitleName + " " + currentSpecialist.FirstName + " " +
                    currentSpecialist.MiddleName + " " + currentSpecialist.LastName);
            }

            // Create footer
            PdfPTable footer = new PdfPTable(5);

            PdfPCell totalManipulationsTextCell = new PdfPCell(new Phrase(FileFooter));
            totalManipulationsTextCell.Colspan = 4;
            totalManipulationsTextCell.HorizontalAlignment = 2;
            footer.AddCell(totalManipulationsTextCell);

            PdfPCell totalManipulationsCell = new PdfPCell(new Phrase(dbManipulations.ToList().Count.ToString()));
            totalManipulationsCell.HorizontalAlignment = 1;
            footer.AddCell(totalManipulationsCell);

            doc.Add(tableBody);
            doc.Add(footer);
        }

        private Phrase MakeBoldPhrase(string text) 
        {
            var phrase = new Phrase();
            phrase.Add(new Chunk(text, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));

            return phrase;
        }
    }
}