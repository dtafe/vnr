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
    
    public partial class Can_MealRecordMissing
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.DateTime> WorkDate { get; set; }
        public string Type { get; set; }
        public Nullable<System.Guid> TamScanReasonMissID { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
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
        public Nullable<bool> IsFullPay { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<System.Guid> MealAllowanceTypeSettingID { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
    
        public virtual Can_MealAllowanceTypeSetting Can_MealAllowanceTypeSetting { get; set; }
        public virtual Cat_TAMScanReasonMiss Cat_TAMScanReasonMiss { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure { get; set; }
    }
}
