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
    
    public partial class Rec_CandidateHistory
    {
        public System.Guid ID { get; set; }
        public System.Guid CandidateID { get; set; }
        public string WordHistoryName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Position { get; set; }
        public string PositionLast { get; set; }
        public string SalaryLast { get; set; }
        public string Tasks { get; set; }
        public string JobDescription { get; set; }
        public string Phone { get; set; }
        public string BussinessType { get; set; }
        public string Salary { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateFinish { get; set; }
        public string Comment { get; set; }
        public string Code { get; set; }
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
        public string SupPosition { get; set; }
        public string SupName { get; set; }
        public string SupMobile { get; set; }
        public string SupEmail { get; set; }
        public string SupCompany { get; set; }
        public string SupComment { get; set; }
        public string SupRelation { get; set; }
        public string ResignReason { get; set; }
    
        public virtual Rec_Candidate Rec_Candidate { get; set; }
    }
}
