//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Clinics
{
    using System;
    using System.Collections.Generic;
    
    public partial class Patients
    {
        public Patients()
        {
            this.Manupulations = new HashSet<Manupulations>();
        }
    
        public System.Guid PatientId { get; set; }
        public System.Guid PatientNumber { get; set; }
        public string Abreviature { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public bool Deleted { get; set; }
    
        public virtual ICollection<Manupulations> Manupulations { get; set; }
    }
}