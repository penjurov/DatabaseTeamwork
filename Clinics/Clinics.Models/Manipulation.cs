namespace Clinics.Models
{
    public class Manipulation
    {
        public System.Guid Id { get; set; }

        public System.Guid PatientId { get; set; }

        public System.Guid SpecialistId { get; set; }

        public System.Guid ProcedureId { get; set; }

        public System.DateTime Date { get; set; }

        public string Information { get; set; }

        public bool Deleted { get; set; }
    
        public virtual Patient Patient { get; set; }

        public virtual Procedure Procedure { get; set; }

        public virtual Specialist Specialist { get; set; }
    }
}
