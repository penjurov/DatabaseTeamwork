namespace Clinics.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Clinics.Data.Migrations;
    using Clinics.Models;

    public class ClinicsDBContex : DbContext
    {
        public ClinicsDBContex()
            : base("ClinicsProgramConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ClinicsDBContex, Configuration>());
        }

        public IDbSet<Clinic> Clinics { get; set; }

        public IDbSet<Manipulation> Manipulations { get; set; }

        public IDbSet<Patient> Patients { get; set; }

        public IDbSet<Procedure> Procedures { get; set; }

        public IDbSet<Specialist> Specialists { get; set; }

        public IDbSet<Specialty> Specialties { get; set; }

        public IDbSet<Title> Titles { get; set; }
    }
}
