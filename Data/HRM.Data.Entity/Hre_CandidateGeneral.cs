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
    
    public partial class Hre_CandidateGeneral
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> RankRateID { get; set; }
        public Nullable<System.Guid> SalaryClassID { get; set; }
        public Nullable<double> BasicSalary { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public Nullable<System.Guid> ContractTypeID { get; set; }
        public Nullable<System.DateTime> EnteringDate { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public Nullable<System.Guid> GradeAttendanceID { get; set; }
        public Nullable<System.Guid> GradePayrollID { get; set; }
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
        public Nullable<System.Guid> AllowanceID1 { get; set; }
        public Nullable<System.Guid> AllowanceID2 { get; set; }
        public Nullable<System.Guid> AllowanceID3 { get; set; }
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        public Nullable<System.Guid> AllowanceID5 { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public Nullable<double> Allowance5 { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CodeEmp { get; set; }
    
        public virtual Cat_ContractType Cat_ContractType { get; set; }
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Cat_GradeAttendance Cat_GradeAttendance { get; set; }
        public virtual Cat_GradePayroll Cat_GradePayroll { get; set; }
        public virtual Cat_JobTitle Cat_JobTitle { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure { get; set; }
        public virtual Cat_Position Cat_Position { get; set; }
        public virtual Cat_SalaryClass Cat_SalaryClass { get; set; }
        public virtual Cat_SalaryRank Cat_SalaryRank { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance1 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance2 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance3 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance4 { get; set; }
        public virtual Cat_WorkPlace Cat_WorkPlace { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
    }
}
