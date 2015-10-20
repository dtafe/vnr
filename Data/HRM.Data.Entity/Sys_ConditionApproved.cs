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
    
    public partial class Sys_ConditionApproved
    {
        public System.Guid ID { get; set; }
        public string ConditionName { get; set; }
        public string Description { get; set; }
        public string ApprovedType { get; set; }
        public string ExpensesType { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public Nullable<System.Guid> ProcessApprovedID { get; set; }
        public Nullable<System.Guid> OrgID1 { get; set; }
        public Nullable<System.Guid> OrgID2 { get; set; }
        public Nullable<System.Guid> OrgID3 { get; set; }
        public Nullable<System.Guid> OrgID4 { get; set; }
        public Nullable<System.Guid> OrgID5 { get; set; }
        public Nullable<System.Guid> OrgID6 { get; set; }
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
    
        public virtual Cat_JobTitle Cat_JobTitle { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure1 { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure2 { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure3 { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure4 { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure5 { get; set; }
        public virtual Cat_Position Cat_Position { get; set; }
        public virtual Cat_WorkPlace Cat_WorkPlace { get; set; }
        public virtual Sys_ProcessApproved Sys_ProcessApproved { get; set; }
    }
}