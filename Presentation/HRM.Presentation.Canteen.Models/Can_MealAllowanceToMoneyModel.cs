using System;
using System.Collections.Generic;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_MealAllowanceToMoneyModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_Profile)]
        public Guid? ProfileID { get; set; }

        public string ProfileIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_ProfileCode)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_Profile)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_DateTo)]
        public DateTime? DateTo { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceType)]
        //public int MealAllowanceType { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceTypeName)]
        //public string MealAllowanceTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceTypeSettingID)]
        public Guid? MealAllowanceTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceTypeSettingName)]
        public string MealAllowanceTypeSettingName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_Notes)]
        public string Note { get; set; }
        public string ValueFields { get; set; }

        public string ProfileCode { get; set; }

        public string MealAllowanceTypeSettingCode { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string MealAllowanceType = "MealAllowanceType";
            public const string MealAllowanceTypeName = "MealAllowanceTypeName";
            public const string MealAllowanceTypeSettingName = "MealAllowanceTypeSettingName";
            public const string Note = "Note";
            public const string Status = "Status";

        }
    }
    public class Can_MealAllowanceToMoneySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_ProfileCode)]
        public string ProfileCodeSearch { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_ProfileName)]
        public string ProfileNameSearch { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceType)]
        public Guid? MealAllowanceTypeSettingID { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_ProfileCode)]
        //public string ProfileCodeSearch { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_Profile)]
        //public int? ProfileID { get; set; }


        //[DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceTypeSettingName)]
        //public string MealAllowanceTypeSettingName { get; set; }

        //public string ValueFields { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string ProfileCodeSearch = "ProfileCodeSearch";
            public const string ProfileName = "ProfileName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string MealAllowanceType = "MealAllowanceType";
        }
    }

    
}
