using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_CateCodeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_CateCode_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_CateCode_CateCodeType)]
        public string CateCodeType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_CateCode_Name)]
        public string Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_CateCode_Note)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string CateCodeType = "CateCodeType";
            public const string Name = "Name";
            public const string Note = "Note";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Cat_CateCodeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_CateCode_Name)]
        public string Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_CateCode_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_CateCodeMultiModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }
}
