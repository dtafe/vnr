using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_ValueEntityModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_ValueEntityName)]
        public string ValueEntityName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_Value)]
        public Nullable<double> Value { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_DateOfEffect)]
        public Nullable<System.DateTime> DateOfEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_Value2)]
        public Nullable<double> Value2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_Value3)]
        public Nullable<double> Value3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_ValueString)]
        public string ValueString { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_ValueString2)]
        public string ValueString2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_ValueString3)]
        public string ValueString3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ValueEntity_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Currency)]
        public Nullable<System.Guid> CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Currency)]
        public string CurrencyName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

       

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ValueEntityName = "ValueEntityName";
            public const string Value = "Value";
            public const string DateOfEffect = "DateOfEffect";
        }
    }
    public class Cat_ValueEntitySearchModel
    {
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    
}
