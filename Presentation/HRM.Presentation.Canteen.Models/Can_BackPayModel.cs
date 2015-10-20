using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_BackPayModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_ProfileId)]       
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_ProfileId)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_MonthYear)]
        public DateTime? MonthYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_Count)]
        public int? Count { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_Amount)]
        public double? Amount { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_CountByFomular)]
        public int? CountByFomular { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_AmountByFomular)]
        public double? AmountByFomular { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_MealAllowanceTypeSettingID)]
        public Guid? MealAllowanceTypeSettingID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_MealAllowanceTypeSettingID)]
        [MaxLength(150)]
        public string MealAllowanceTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_Note)]
        [MaxLength(1000)]
        public string Note { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string MonthYear = "MonthYear";
            public const string Count = "Count";
            public const string CountByFomular = "CountByFomular";
            public const string AmountByFomular = "AmountByFomular";
            public const string Amount = "Amount";
            public const string MealAllowanceTypeSettingID = "MealAllowanceTypeSettingID";
            public const string MealAllowanceTypeName = "MealAllowanceTypeName";
            public const string Note = "Note";           
        }
    }
    public class Can_BackPaySearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Canteen_BackPay_ProfileId)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public DateTime? DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
