using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SalaryClassTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClassType_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClassType_Name)]
        public string SalaryClassTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClassType_Description)]
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string SalaryClassTypeName = "SalaryClassTypeName";
            public const string Description = "Description";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_SalaryClassTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClassType_Name)]
        public string SalaryClassTypeName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_SalaryClassTypeMultiModel
    {
        public Guid ID { get; set; }
        public string SalaryClassTypeName { get; set; }
    }
}
