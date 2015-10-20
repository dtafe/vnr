using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatOvertimeTypeMultiModel 
    {
        public Guid ID { get; set; }
        public string OvertimeTypeName { get; set; }
        public string Code { get; set; }

    }
    public class CatOvertimeTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_OvertimeTypeName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_OvertimeType_OvertimeTypeName_StringLength)]
        public string OvertimeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_Code)]
        [StringLength(400, ErrorMessage = ConstantDisplay.HRM_Category_OvertimeType_Code_StringLength)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_Rate)]
        public double Rate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_TaxRate)]
        public double TaxRate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_TimeOffInLieuRate)]
        public double? TimeOffInLieuRate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_IsNightShift)]
        public bool? IsNightShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_Comment)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_OvertimeType_Comment_StringLength)]
        public string Comment { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_TypeTemp)]
        public string TypeTemp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_CodeStatistic)]
        public string CodeStatistic { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_IsWorkDay)]
        public bool? IsWorkDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_IsHoliday)]
        public bool? IsHoliday { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_IsWeeked)]
        public bool? IsWeeked { get; set; }
        // Loại thanh toán
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_PaymentType)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_OvertimeType_PaymentType_StringLength)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_Order)]
        public int? Order { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_UserCreate)]
        public string UserCreate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_DateUpdate)]
        public DateTime? DateUpdate { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string OvertimeTypeName = "OvertimeTypeName";
            public const string Rate = "Rate";
            public const string TaxRate = "TaxRate";
            public const string TimeOffInLieuRate = "TimeOffInLieuRate";
            public const string IsNightShift = "IsNightShift";
            public const string Comment = "Comment";
            public const string PaymentType = "PaymentType";
            public const string UserCreate = "UserCreate";
            public const string DateUpdate = "DateUpdate";
            public const string IsWorkDay = "IsWorkDay";

        }
    }
    public class CatOvertimeTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_OvertimeTypeName)]
        public string OvertimeTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_Rate)]
        public double? Rate { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
