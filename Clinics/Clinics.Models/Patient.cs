namespace Clinics.Models
{
    using System.Collections.Generic;

    public class Patient
    {
        private ICollection<Manipulation> manipulations;

        public Patient()
        {
            this.manipulations = new HashSet<Manipulation>();
        }
    
        public System.Guid Id { get; set; }

        public string PatientNumber { get; set; }

        public string Abreviature { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public bool Deleted { get; set; }
    
        public virtual ICollection<Manipulation> Manipulations 
        { 
            get
            {
                return this.manipulations;
            }

            set
            {
                this.manipulations = value;
            }
        }
    }
}
