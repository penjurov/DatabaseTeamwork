namespace Clinics.Models
{
    using System.Collections.Generic;

    public class Specialist
    {
        private ICollection<Clinic> clinics;
        private ICollection<Manipulation> manupulations;
        private ICollection<Title> titles;

        public Specialist()
        {
            this.clinics = new HashSet<Clinic>();
            this.manupulations = new HashSet<Manipulation>();
            this.titles = new HashSet<Title>();
        }
    
        public System.Guid Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Duty { get; set; }

        public int Title { get; set; }

        public System.Guid Specialty { get; set; }

        public string Uin { get; set; }

        public bool Deleted { get; set; }
    
        public virtual ICollection<Clinic> Clinics 
        { 
            get
            {
                return this.clinics;
            }

            set
            {
                this.clinics = value;
            }
        }

        public virtual ICollection<Manipulation> Manupulations 
        { 
            get
            {
                return this.manupulations;
            }

            set
            {
                this.manupulations = value;
            }
        }

        public virtual Specialty Specialties { get; set; }

        public virtual ICollection<Title> Titles 
        { 
            get
            {
                return this.titles;
            }

            set
            {
                this.titles = value;
            }
        }
    }
}
