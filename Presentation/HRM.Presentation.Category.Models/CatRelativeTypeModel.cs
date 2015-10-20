using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatRelativeTypeMultiModel 
    {
        public Guid ID { get; set; }
        public string RelativeTypeName { get; set; }
    }
    public class CatRelativeTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_RelativeType_RelativeTypeName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_RelativeType_RelativeTypeName_StringLength)]
        public string RelativeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_RelativeType_RelativeTypeCode)]
        [StringLength(50, ErrorMessage = ConstantDisplay.HRM_Category_RelativeType_RelativeTypeCode_StringLength)]
        public string Code { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string RelativeTypeName = "RelativeTypeName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }


    public class CatRelativeTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_RelativeType_RelativeTypeName)]
        public string RelativeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_RelativeType_RelativeTypeCode)]
        public string Code { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
       
        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string RelativeTypeName = "RelativeTypeName";
        }

        
    }
}
