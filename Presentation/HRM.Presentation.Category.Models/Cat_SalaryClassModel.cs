using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SalaryClassModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_AbilityTitleVNI)]
        public string AbilityTitleVNIABILITY { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_AbilityTitleENG)]
        public string AbilityTitleEngABILITY { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_AbilityTitleID)]
        public string AbilityTitleCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_AbilityTitleID)]
        public Nullable<System.Guid> AbilityTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_SalaryClassName)]
        public string SalaryClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_SalaryClassTypeName)]
        public System.Guid? SalaryClassTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_SalaryClassTypeName)]
        public string SalaryClassTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_AbilityTitleVNI)]
        public string AbilityTitleVNI { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_AbilityTitleENG)]
        public string AbilityTitleENG { get; set; }
        public int? OrderNumber { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string SalaryClassName = "SalaryClassName";
            public const string Description = "Description";
            public const string Code = "Code";
            public const string SalaryClassTypeName = "SalaryClassTypeName";
            public const string AbilityTitleVNIABILITY = "AbilityTitleVNIABILITY";
            public const string AbilityTitleEngABILITY = "AbilityTitleEngABILITY";
        }
    }

    public class Cat_SalaryClassSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_SalaryClassName)]
        public string SalaryClassName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_SalaryClassMultiModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string SalaryClassName { get; set; }
    }
}
