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
    
    public partial class Hre_AppendixContract
    {
        public System.Guid ID { get; set; }
        public System.Guid ContractID { get; set; }
        public string AppendixContractName { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> DateofEffect { get; set; }
        public string Contents { get; set; }
        public string Attachment { get; set; }
        public Nullable<System.Guid> ServerID { get; set; }
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
        public string AppendixContractType { get; set; }
        public Nullable<System.DateTime> DateSignedAppendixContract { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<System.DateTime> DateEndAppendixContract { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public Nullable<System.Guid> RankRateID { get; set; }
        public Nullable<System.Guid> ClassRateID { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public Nullable<System.Guid> ContractTypeID { get; set; }
        public Nullable<System.Guid> AllowanceID1 { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<System.Guid> AllowanceID2 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<System.Guid> AllowanceID3 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public Nullable<double> Allowance { get; set; }
        public Nullable<double> Salary { get; set; }
        public Nullable<System.Guid> CurenncyID { get; set; }
        public Nullable<System.Guid> CurenncyID1 { get; set; }
        public Nullable<System.Guid> CurenncyID2 { get; set; }
        public Nullable<System.Guid> CurenncyID3 { get; set; }
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        public Nullable<System.Guid> CurenncyIDSalary { get; set; }
        public Nullable<System.Guid> CurenncyID4 { get; set; }
        public Nullable<System.Guid> ReportMappingID { get; set; }
        public Nullable<System.Guid> CurenncyID5 { get; set; }
        public Nullable<double> InsuranceAmount { get; set; }
        public Nullable<System.Guid> CurenncyID6 { get; set; }
        public Nullable<System.Guid> AppendixContractTypeID { get; set; }
        public Nullable<System.DateTime> DateAuthorize { get; set; }
        public Nullable<double> HourWorkInMonth { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public string JobDescription { get; set; }
        public Nullable<double> Bonus { get; set; }
        public string KPI { get; set; }
        public Nullable<System.Guid> AllowanceID5 { get; set; }
        public string KPICompany { get; set; }
        public Nullable<double> Tax { get; set; }
    
        public virtual Cat_AppendixContractType Cat_AppendixContractType { get; set; }
        public virtual Cat_ContractType Cat_ContractType { get; set; }
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Cat_Currency Cat_Currency1 { get; set; }
        public virtual Cat_Currency Cat_Currency2 { get; set; }
        public virtual Cat_Currency Cat_Currency3 { get; set; }
        public virtual Cat_Currency Cat_Currency4 { get; set; }
        public virtual Cat_Currency Cat_Currency5 { get; set; }
        public virtual Cat_Currency Cat_Currency6 { get; set; }
        public virtual Cat_JobTitle Cat_JobTitle { get; set; }
        public virtual Cat_Position Cat_Position { get; set; }
        public virtual Cat_ReportMapping Cat_ReportMapping { get; set; }
        public virtual Cat_SalaryClass Cat_SalaryClass { get; set; }
        public virtual Cat_SalaryRank Cat_SalaryRank { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance1 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance2 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance3 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance4 { get; set; }
        public virtual Cat_WorkPlace Cat_WorkPlace { get; set; }
        public virtual Hre_Contract Hre_Contract { get; set; }
    }
}
