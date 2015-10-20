using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_CandidateGeneralEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public Nullable<System.Guid> RankRateID { get; set; }
        public string RankRateName { get; set; }
        public Nullable<System.Guid> SalaryClassID { get; set; }
        public string SalaryClassName { get; set; }
        public Nullable<double> BasicSalary { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public Nullable<System.Guid> ContractTypeID { get; set; }
        public string ContractTypeName { get; set; }
        public Nullable<System.DateTime> EnteringDate { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public Nullable<System.Guid> GradeAttendanceID { get; set; }
        public string GradeAttendanceName { get; set; }
        public Nullable<System.Guid> GradePayrollID { get; set; }
        public string GradePayrollName { get; set; }
        public string IDNo { get; set; }
        public string Gender { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public Nullable<System.Guid> JobTitleID { get; set; }
        public string JobTitleName { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public string PositionName { get; set; }
        public Nullable<System.Guid> AllowanceID1 { get; set; }
        public string AllowanceID1Name { get; set; }
        public Nullable<System.Guid> AllowanceID2 { get; set; }
        public string AllowanceID2Name { get; set; }
        public Nullable<System.Guid> AllowanceID3 { get; set; }
        public string AllowanceID3Name { get; set; }
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        public string AllowanceID4Name { get; set; }
        public Nullable<System.Guid> AllowanceID5 { get; set; }
        public string AllowanceID5Name { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public Nullable<double> Allowance5 { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CodeEmp { get; set; }
        public string AbilityTitleVNI { get; set; }

    }
}
