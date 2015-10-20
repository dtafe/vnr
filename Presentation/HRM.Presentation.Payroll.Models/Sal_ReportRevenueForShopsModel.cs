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
    public class Sal_ReportRevenueForShopsModel : BaseViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public string Rank { get; set; }
        public double Target { get; set; }
        public double Actual { get; set; }
        public double CompletionRate { get; set; }
        public string KPIBonusName { get; set; }
        public string OrgStructureID { get; set; }
        public Guid ExportId { get; set; }
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

        }
    }

    

}
