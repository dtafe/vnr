//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Data.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rec_SupplerRelation
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CandidateId { get; set; }
        public string SupplierRelationName { get; set; }
        public string CompanyName { get; set; }
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
    
        public virtual Rec_Candidate Rec_Candidate { get; set; }
    }
}
