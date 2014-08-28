namespace Clinics.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Clinics.Models;

    public interface IClinicsDBContext
    {
        IDbSet<Title> Titles { get; set; }

        IDbSet<Clinic> Clinics { get; set; }

        IDbSet<Manipulation> Manipulations { get; set; }

        IDbSet<Patient> Patients { get; set; }

        IDbSet<Procedure> Procedures { get; set; }

        IDbSet<Specialist> Specialists { get; set; }

        IDbSet<Specialty> Specialties { get; set; }

        IDbSet<T> Set<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        void SaveChanges();

        void Dispose();
    }
}
