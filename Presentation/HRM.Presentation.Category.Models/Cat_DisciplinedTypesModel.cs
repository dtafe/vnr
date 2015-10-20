using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_DisciplinedTypesMultiModel 
    {
        public Guid ID { get; set; }
        public string DisciplinedTypesName { get; set; }
    }
    public class Cat_DisciplinedTypesModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_DisciplinedTypes_DisciplinedTypesName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_RelativeType_RelativeTypeName_StringLength)]
        public string DisciplinedTypesName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DisciplinedTypes_Code)]
        [StringLength(50, ErrorMessage = ConstantDisplay.HRM_Category_RelativeType_RelativeTypeCode_StringLength)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_DisciplinedTypes_Notes)]
          public string Notes { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string DisciplinedTypesName = "DisciplinedTypesName";
            public const string Notes = "Notes";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }


    public class Cat_DisciplinedTypesSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_DisciplinedTypes_DisciplinedTypesName)]
        public string DisciplinedTypesName { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
       
        public partial class FieldNames
        {
           
            public const string DisciplinedTypesName = "DisciplinedTypesName";
        }

        
    }
}
