namespace Clinics.Data
{
    using Clinics.Data.Repositories;
    using Clinics.Models;
    using System;
    using System.Collections.Generic;

    public class ClinicsData : IClinicsData
    {
        private IClinicsDBContext context;
        private IDictionary<Type, object> repositories;

        public ClinicsData(IClinicsDBContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public ClinicsData()
            : this(new ClinicsDBContext())
        {
        }

        public IGenericRepository<Title> Titles
        {
            get 
            { 
                return this.GetRepository<Title>();
            }
        }

        public IGenericRepository<Clinic> Clinics
        {
            get
            {
                return this.GetRepository<Clinic>();
            }
        }

        public IGenericRepository<Manipulation> Manipulations
        {
            get
            {
                return this.GetRepository<Manipulation>();
            }
        }

        public IGenericRepository<Patient> Patients
        {
            get
            {
                return this.GetRepository<Patient>();
            }
        }

        public IGenericRepository<Procedure> Procedures
        {
            get
            {
                return this.GetRepository<Procedure>();
            }
        }

        public IGenericRepository<Specialist> Specialists
        {
            get
            {
                return this.GetRepository<Specialist>();
            }
        }

        public IGenericRepository<Specialty> Specialties
        {
            get
            {
                return this.GetRepository<Specialty>();
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        private IGenericRepository<T> GetRepository<T>() where T : class
        {
            var typeOfModel = typeof(T);
            if (!this.repositories.ContainsKey(typeOfModel))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeOfModel, Activator.CreateInstance(type, this.context));
            }

            return (IGenericRepository<T>)this.repositories[typeOfModel];
        }
    }
}
