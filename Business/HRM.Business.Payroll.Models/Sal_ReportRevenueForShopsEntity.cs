using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Sal_ReportRevenueForShopsEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string Rank { get; set; }
        public double Target { get; set; }
        public double Actual { get; set; }
        public double CompletionRate { get; set; }
        public string KPIBonusName { get; set; }
        public double Commission { get; set; }

        public double TopLine5Forcus { get; set; }
        public double CommissionTopLine5Forcus { get; set; }

        public double ProductForcus { get; set; }
        public double CommissionProductForcus { get; set; }
        public double Incentive { get; set; }
        public double Shiftleader { get; set; }
        public double Total { get; set; }
        public partial class FieldNames
        {

            public const string Date = "Date";
            public const string ShopCode = "ShopCode";
            public const string ShopName = "ShopName";
            public const string Rank = "Rank";
            public const string Target = "Target";
            public const string Actual = "Actual";
            public const string CompletionRate = "CompletionRate";
            public const string KPIBonusName = "KPIBonusName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string Commission = "Commission";
            public const string TopLine5Forcus = "TopLine5Forcus";
            public const string CommissionTopLine5Forcus = "CommissionTopLine5Forcus";
            public const string ProductForcus = "ProductForcus";
            public const string CommissionProductForcus = "CommissionProductForcus";
            public const string Incentive = "Incentive";
            public const string Shiftleader = "Shiftleader";
            public const string Total = "Total";
        }
    }
}
