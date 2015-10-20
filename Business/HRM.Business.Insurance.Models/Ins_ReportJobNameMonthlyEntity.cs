using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ReportJobNameMonthlyEntity : HRMBaseModel
    {
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
        public string Rank { get; set; }
        public DateTime? DateHire { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public Guid? SocialInsPlaceID { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public bool? IsDecreaseWorkingDays { get; set; }
        public bool? IsPregnant { get; set; }

    }
}
