using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
namespace HRM.Presentation.Canteen.Models
{
    public class Can_MealAllowanceTypeSettingModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Canteen_MealAllowanceTypeSetting_MealAllowanceTypeSettingName)]
        public string MealAllowanceTypeSettingName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealAllowanceTypeSetting_MealAllowanceTypeSettingCode)]
        public string MealAllowanceTypeSettingCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealAllowanceTypeSetting_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealAllowanceTypeSetting_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealAllowanceTypeSetting_Amount)]
        public double? Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_Standard)]
        public bool? Standard { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealAllowanceTypeSetting_Notes)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealAllowanceTypeSetting_IsMealAllowanceToMoney)]
        public bool? IsMealAllowanceToMoney { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string MealAllowanceTypeSettingName = "MealAllowanceTypeSettingName";
            public const string MealAllowanceTypeSettingCode = "MealAllowanceTypeSettingCode";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string Amount = "Amount";
            public const string IsMealAllowanceToMoney = "IsMealAllowanceToMoney";
            public const string Note = "Note";
            public const string DateCreate = "DateCreate";
            public const string UserCreate = "UserCreate";
        }
    }
    public class Can_MealAllowanceTypeSettingSearchModel
    {
        
        [DisplayName(ConstantDisplay.HRM_Canteen_MealAllowanceTypeSetting_MealAllowanceTypeSettingCode)]
        public string MealAllowanceTypeSettingCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealAllowanceTypeSetting_MealAllowanceTypeSettingName)]
        public string MealAllowanceTypeSettingName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        
    }

    public class Can_MealAllowanceTypeSettingMultiModel
    {

        public Guid ID { get; set; }
        public string MealAllowanceTypeSettingName { get; set; }
    }
}
