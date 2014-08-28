namespace Clinics.Models
{
    using System.Collections.Generic;

    public class Procedure
    {
        private ICollection<Manipulation> manipulations;

        public Procedure()
        {
            this.manipulations = new HashSet<Manipulation>();
        }
    
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public string IscCode { get; set; }

        public decimal Price { get; set; }

        public string Information { get; set; }

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
