using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_ReportMealTimeSummaryModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Catering)]
        public string CateringName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Canteen)]
        public string CanteenName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Line)]
        public string LineName { get; set; }
        // Giá trị suất ăn
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Price)]
        public double Price { get; set; }
        // Số lương
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Total)]
        public double Total { get; set; }
        // Thành Tiền
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_TotalAmount)]
        public double TotalAmount { get; set; }
        // Tỉ lệ (Line/Catering)
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Rate)]
        public double Rate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_UserPrint)]
        public string UserPrint { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DatePrint)]
        public DateTime? DatePrint { get; set; }
        public Guid ExportID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Catering)]
        public List<Guid> CateringIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Canteen)]
        public List<Guid> CanteenIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Line)]
        public List<Guid> LineIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_OrgStructureId)]
        public List<Guid> OrgIDs { get; set; }
        public partial class FieldNames
        {
            public const string CateringName = "CateringName";
            public const string CanteenName = "CanteenName";
            public const string LineName = "LineName";
            public const string Price = "Price";
            public const string Total = "Total";
            public const string TotalAmount = "TotalAmount";
            public const string Rate = "Rate";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
            public const string OrgIDs = "OrgIDs";
        }
    }
}
