using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportUnusualHDTModel : BaseViewModel
    {
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Dept)]
        public string Dept { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Code)]
        public string HDTJobTypeCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_HDTJobTypeName)]
        public string HDTJobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Price)]
        public double? Price { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_TamScanLog_Hours)]
        public string TimeScan { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hre_Report_PriceRecieve)]
        public double? PriceRecieve { get; set; }
        // không đk nhưng nhận
        [DisplayName(ConstantDisplay.HRM_Hre_Report_NotRegister)]
        public bool? NotRegister { get; set; }
        // có đk nhưng nhận
        [DisplayName(ConstantDisplay.HRM_Hre_Report_HaveRegister)]
        public bool? HaveRegister { get; set; }
        // nhận sai
        [DisplayName(ConstantDisplay.HRM_Hre_Report_RevieveWrong)]
        public bool? RevieveWrong { get; set; }
        // Đk Search
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string OrgStructureIDs { get; set; }
        public Guid ExportID { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string Dept = "Dept";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string HDTJobTypeCode = "HDTJobTypeCode";
            public const string HDTJobTypeName = "HDTJobTypeName";
            public const string Price = "Price";
            public const string TimeScan = "TimeScan";
            public const string PriceRecieve = "PriceRecieve";
            public const string NotRegister = "NotRegister";
            public const string HaveRegister = "HaveRegister";
            public const string RevieveWrong = "RevieveWrong";
        }
    }
}
