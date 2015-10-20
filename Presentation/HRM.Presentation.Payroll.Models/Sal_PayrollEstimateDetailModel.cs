using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_PayrollEstimateDetailModel : BaseViewModel
    {
        public Nullable<System.Guid> PayrollEstimateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_OrgStructureID)]

        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_QuantityEmp)]

        public Nullable<double> QuantityEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_SalaryAverage)]
        public Nullable<double> SalaryAverage { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_LeaveUnpaid)]
        public Nullable<double> LeaveUnpaid { get; set; }
        public Nullable<float> LeaveUnpaidView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_OvertimeNormal)]
        public Nullable<double> OvertimeNormal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_OvertimeHoliday)]
        public Nullable<double> OvertimeHoliday { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_OvertimeWeekend)]
        public Nullable<double> OvertimeWeekend { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_OvertimeNightNormal)]
        public Nullable<double> OvertimeNightNormal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_OvertimeNightHoliday)]
        public Nullable<double> OvertimeNightHoliday { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_OvertimeNightWeekend)]
        public Nullable<double> OvertimeNightWeekend { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_OrgStructureID)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_OrgStructureCode)]
        public string OrgStructureCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_AmountHour)]
        public Double? AmountHour { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_AmountLeaveDay)]

        public Double? AmountLeaveDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_AmountOvertime)]

        public Double? AmountOvertime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_EstimateDetail_AmountTotal)]

        public Double? AmountTotal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Estimate_RateAdjust)]

        public Nullable<double> RateAdjust { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Estimate_BounusBudget)]

        public Nullable<double> BonusBudget { get; set; }

        public partial class FieldNames
        {

            public const string QuantityEmp = "QuantityEmp";
            public const string SalaryAverage = "SalaryAverage";
            public const string LeaveUnpaid = "LeaveUnpaid";
            public const string LeaveUnpaidView = "LeaveUnpaidView";
            public const string OvertimeNormal = "OvertimeNormal";
            public const string OvertimeHoliday = "OvertimeHoliday";
            public const string OvertimeWeekend = "OvertimeWeekend";
            public const string OvertimeNightNormal = "OvertimeNightNormal";
            public const string OvertimeNightHoliday = "OvertimeNightHoliday";
            public const string OvertimeNightWeekend = "OvertimeNightWeekend";

            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureCode = "OrgStructureCode";
            public const string AmountHour = "AmountHour";
            public const string AmountLeaveDay = "AmountLeaveDay";
            public const string AmountOvertime = "AmountOvertime";
            public const string AmountTotal = "AmountTotal";
            public const string RateAdjust = "RateAdjust";
            public const string BonusBudget = "BonusBudget";
            public const string OrgStructureID = "OrgStructureID";

        }
    }
}
