using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_KPIBonusModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_KPIBonus_KPIBonusName)]
        public string KPIBonusName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_KPIBonus_Code)]
        public string Code { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Category_KPIBonus_Value)]
        
        //public double? Value { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_Category_KPIBonus_Note)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }


        [DisplayName(ConstantDisplay.HRM_Category_KPIBonus_IsTotalRevenue)]
        public bool? IsTotalRevenue { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_KPIBonus_Unit)]
        public Nullable<System.Guid> UnitID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_KPIBonus_Unit)]
        public string UnitName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string KPIBonusName = "KPIBonusName";
            public const string Code = "Code";
            //public const string Value = "Value";
            public const string Note = "Note";
            public const string UnitID = "UnitID";
            public const string UnitName = "UnitName";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string IsTotalRevenue = "IsTotalRevenue";
        }
    }

    public class Cat_KPIBonusSearchMoel
    {
        public string Code { get; set; }
        public string KPIBonusName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_KPIBonusMultiModel
    {
        public Guid ID { get; set; }
        public string KPIBonusName { get; set; }
        public int TotalRow { get; set; }
    }
}
