using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ReportInsCostCenterMonthlyEntity : HRMBaseModel
    {
        public Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public Guid? CostCentreID { get; set; }
        public string CostCentreName { get; set; }
        public DateTime? MonthYear { get; set; }
        public double? MoneySocialInsurance { get; set; }
        public double? MoneyUnEmpInsurance { get; set; }
        public double? MoneyHealthInsurance { get; set; }
        public double? SocialInsEmpAmount { get; set; }
        public double? SocialInsComAmount { get; set; }
        public double? HealthInsEmpAmount { get; set; }
        public double? UnemployComAmount { get; set; }
        public double? HealthInsComAmount { get; set; }
        public double? UnemployEmpAmount { get; set; }
        public double? SalaryInsurance { get; set; }
        public string ProfileIds { get; set; }
        public string OrgStructureIDs { get; set; }
        public string ProfileName { get; set; }
        public int? ProfileAmount{ get; set; }
        public Guid? WorkPlaceID { get; set; }
        public Guid? SocialInsPlaceID { get; set; }
        public string SocialInsPlaceName { get; set; }
        public bool? IsPayback { get; set; }
        public Guid? PaybackID { get; set; }
    }
}
