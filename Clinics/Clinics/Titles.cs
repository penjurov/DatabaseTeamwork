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
    
    public partial class Titles
    {
        public Titles()
        {
            this.Specialists = new HashSet<Specialists>();
        }
    
        public System.Guid TitleId { get; set; }
        public string Title { get; set; }
        public bool Deleted { get; set; }
    
        public virtual ICollection<Specialists> Specialists { get; set; }
    }
}
