﻿namespace Clinics.Models
{
    public class Clinic
    {
        public System.Guid Id { get; set; }

        public string ClinicName { get; set; }

        public string ClinicAddress { get; set; }

        public string ClinicPhones { get; set; }

        public System.Guid ClinicChiefId { get; set; }

        public bool Deleted { get; set; }

        public virtual Specialist ClinicChief { get; set; }
    }
}
