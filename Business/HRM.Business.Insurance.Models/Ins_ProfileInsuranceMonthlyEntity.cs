using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ProfileInsuranceMonthlyEntity : HRMBaseModel
    {
        public Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; } 
        public DateTime? MonthYear { get; set; }
        public bool? IsSocialInsurance { get; set; }
        public bool? IsHealthInsurance { get; set; }
        public bool? IsUnEmpInsurance { get; set; }
        public bool? IsPregnant { get; set; }        
        public double? MoneySocialInsurance { get; set; }
        public double? MoneyUnEmpInsurance { get; set; }
        public double? SalaryInsurance { get; set; }
        public double? SalaryUnEmpInsurance { get; set; }
        public double? SalaryHealthInsurance { get; set; }
        public double? MoneyHealthInsurance { get; set; }
        public string ProfileName { get; set; }
        public double? Allowance1 { get; set; }
        public double? Allowance2 { get; set; }
        public double? Allowance3 { get; set; }
        public double? Allowance4 { get; set; }
        public double? AllowanceAdditional { get; set; }
        public double? AmountChargeIns { get; set; }
        public double? SocialInsEmpRate { get; set; }
        public double? HealthInsEmpRate { get; set; }
        public double? UnemployEmpRate { get; set; }
        public double? SocialInsComRate { get; set; }
        public double? HealthInsComRate { get; set; }
        public double? UnemployComRate { get; set; }
        public double? SocialInsEmpAmount { get; set; }
        public double? HealthInsEmpAmount { get; set; }
        public double? UnemployEmpAmount { get; set; }
        public double? SocialInsComAmount { get; set; }
        public double? HealthInsComAmount { get; set; }
        public double? UnemployComAmount { get; set; }
        public string JobName { get; set; }
        public bool? IsDecreaseWorkingDays { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string HDTJobGroupCode { get; set; }
        public Guid? SocialInsPlaceID { get; set; }
        public string SocialInsPlaceName { get; set; }
        public Nullable<double> AmountHDTIns { get; set; }
        public DateTime? DateHire { get; set; }
        public DateTime? MonthYearEffect { get; set; }
        public bool? IsPayback { get; set; }
        public Guid? PaybackID { get; set; }

    }
}
