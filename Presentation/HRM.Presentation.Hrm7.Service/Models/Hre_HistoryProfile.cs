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
    
    public partial class Hre_HistoryProfile
    {
        public System.Guid ID { get; set; }
        public System.Guid ProfileId { get; set; }
        public Nullable<System.DateTime> DateHire { get; set; }
        public Nullable<System.DateTime> DateQuit { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
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
        public Nullable<System.Guid> JobTitleID { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
    
        public virtual Cat_JobTitle Cat_JobTitle { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure { get; set; }
        public virtual Cat_Position Cat_Position { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
    }
}
