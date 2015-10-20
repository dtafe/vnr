using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ReportJoinInsuranceMonthlyEntity : HRMBaseModel
    {
        public int STT { get; set; }
        public Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public Guid? CostCentreID { get; set; }
        public string CostCentreName { get; set; }
        public DateTime? MonthYear { get; set; }
        public bool? IsSocialInsurance { get; set; }
        public bool? IsHealthInsurance { get; set; }
        public bool? IsUnEmpInsurance { get; set; }
        public double? MoneySocialInsurance { get; set; }
        public double? MoneyUnEmpInsurance { get; set; }
        public double? MoneySocialInsurancePreMonth { get; set; }
        public double? SalaryInsurance { get; set; }
        public double? MoneyHealthInsurance { get; set; }
        public string ProfileIds { get; set; }
        public string OrgStructureIDs { get; set; }
        public string ProfileName { get; set; }
        public string Location { get; set; }
        public string JobName { get; set; }
        public string SocialInsNo { get; set; }
        public string HealthInsNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PAddress { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureCode { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string IDNo { get; set; }
        public DateTime? IDDateOfIssue { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public double? SalaryHealthInsurance { get; set; }
        public double? SalaryUnEmpInsurance { get; set; }
        public Guid? SocialInsPlaceID { get; set; }
        public string SocialInsPlaceName { get; set; }
    }
}
