namespace Clinics.Models
{
    using System.Collections.Generic;

    public class Title
    {
        private ICollection<Specialist> specialists;

        public Title()
        {
            this.specialists = new HashSet<Specialist>();
        }
    
        public System.Guid Id { get; set; }

        public string TitleName { get; set; }

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
