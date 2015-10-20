using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_ConditionalColorModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ConditionalColor_ConditionalColorName)]
        public string ConditionalColorName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ConditionalColor_ConditionalCode)]
        public string ConditionalCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ConditionalColor_PropertyName)]
        public string PropertyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ConditionalColor_Condition)]
        public string Condition { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ConditionalColor_Value)]
        public string Value { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ConditionalColor_ColorCode)]
        public string ColorCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ConditionalColor_BGColorCode)]
        public string BGColorCode { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ConditionalColorName = "ConditionalColorName";
            public const string ConditionalCode = "ConditionalCode";
            public const string PropertyName = "PropertyName";
            public const string Condition = "Condition";
            public const string Value = "Value";
            public const string ColorCode = "ColorCode";
            public const string BGColorCode = "BGColorCode";
        }
    }

    public class CatConditionalColorSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ConditionalColor_ConditionalColorName)]
        public string ConditionalColorName { get; set; }
       
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }


}
