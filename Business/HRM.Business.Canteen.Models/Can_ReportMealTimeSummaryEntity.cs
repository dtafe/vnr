using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Canteen.Models
{
    public class Can_ReportMealTimeSummaryEntity : HRMBaseModel
    {
        public string CateringName { get; set; }
        public string CanteenName { get; set; }
        public string LineName { get; set; }
        // Giá trị suất ăn
        public double? Price { get; set; }
        // Số lương
        public double Total { get; set; }
        // Thành Tiền
        public double? TotalAmount { get; set; }
        // Tỉ lệ (Line/Catering)
        public double? Rate { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserPrint { get; set; }
        public DateTime? DatePrint { get; set; }
        public Guid ExportID { get; set; }
        public List<Guid?> CateringIDs { get; set; }
        public List<Guid?> CanteenIDs { get; set; }
        public List<Guid?> LineIDs { get; set; }
        public List<Guid?> OrgIDs { get; set; }
    }
}
