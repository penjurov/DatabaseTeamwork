namespace Clinics.Models
{
    using System.Collections.Generic;

    public class Specialty
    {
        private ICollection<Specialist> specialists;

        public Specialty()
        {
            this.specialists = new HashSet<Specialist>();
        }
    
        public System.Guid Id { get; set; }

        public string Speciality { get; set; }

        public bool Deleted { get; set; }
    
        public virtual ICollection<Specialist> Specialists 
        { 
            get
            {
                return this.specialists;
            }

            set
            {
                this.specialists = value;
            }
        }
    }
}
