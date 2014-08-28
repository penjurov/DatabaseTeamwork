namespace Clinics.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Clinics.Data.Migrations;
    using Clinics.Models;

    public class ClinicsDBContext : DbContext, IClinicsDBContext
    {
        public ClinicsDBContext()
            : base("ClinicsProgramConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ClinicsDBContext, Configuration>());
        }

        public IDbSet<Clinic> Clinics { get; set; }

        public IDbSet<Manipulation> Manipulations { get; set; }

        public IDbSet<Patient> Patients { get; set; }

        public IDbSet<Procedure> Procedures { get; set; }

        public IDbSet<Specialist> Specialists { get; set; }

        public IDbSet<Specialty> Specialties { get; set; }

        public IDbSet<Title> Titles { get; set; }


        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
