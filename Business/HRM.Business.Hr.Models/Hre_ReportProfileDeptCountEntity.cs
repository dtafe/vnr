using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileDeptCountEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public Guid ProfileID { get; set; }
        public string OrgStructureName { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public string EpmloyeeName { get; set; }
        public string ActualTraining { get; set; }
        public string Actualcasual { get; set; }
        public DateTime? DateHire { get; set; }

        public double LastMonthCost { get; set; }
        public double LastMonthCostCurrent { get; set; }
        public double LastMonthOTCost { get; set; }
        public double LastMonthOTCostCurrent { get; set; }
        public double HeadCountPlanOfficial { get; set; }
        public double HeadCountPlanSeasonal { get; set; }
        public double TotalSalaryBudget { get; set; }
        public double TotalOvertimeBudget { get; set; }

        public string PlaceOfBirth { get; set; }

        public DateTime? DayOfBirth { get; set; }

        public string OrgStructureID { get; set; }

    }
}
