//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Presentation.Hrm7.Service.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rec_Tag
    {
        public Rec_Tag()
        {
            this.Hre_Profile = new HashSet<Hre_Profile>();
            this.Rec_Candidate = new HashSet<Rec_Candidate>();
        }
    
        public System.Guid ID { get; set; }
        public string TagName { get; set; }
        public byte[] EntityType { get; set; }
        public string UserCreate { get; set; }
        public string ServerCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public string UserUpdate { get; set; }
        public string ServerUpdate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
    
        public virtual ICollection<Hre_Profile> Hre_Profile { get; set; }
        public virtual ICollection<Rec_Candidate> Rec_Candidate { get; set; }
    }
}
