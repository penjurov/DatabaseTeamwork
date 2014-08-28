namespace Clinics.Data
{
    using Clinics.Data.Repositories;
    using Clinics.Models;

    public interface IClinicsData
    {
        IGenericRepository<Title> Titles { get; }

        IGenericRepository<Clinic> Clinics { get; }

        IGenericRepository<Manipulation> Manipulations { get; }

        IGenericRepository<Patient> Patients { get; }

        IGenericRepository<Procedure> Procedures { get; }

        IGenericRepository<Specialist> Specialists { get; }

        IGenericRepository<Specialty> Specialties { get; }

        void SaveChanges();

        void Dispose();

    }
}
