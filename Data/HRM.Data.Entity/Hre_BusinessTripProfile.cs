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
    
    public partial class Hre_BusinessTripProfile
    {
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public string BusinessTripProfileName { get; set; }
        public System.Guid BusinessTripID { get; set; }
        public System.Guid ProfileID { get; set; }
        public string JobTitleType { get; set; }
        public Nullable<System.DateTime> DateStartIntend { get; set; }
        public Nullable<System.DateTime> DateEndIntend { get; set; }
        public Nullable<System.DateTime> DateStartReal { get; set; }
        public Nullable<System.DateTime> DateEndReal { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string Description { get; set; }
        public string ServerUpdate { get; set; }
        public string ServerCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
    
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
    }
}
