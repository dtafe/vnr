using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_PerformanceTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_PerformanceType_Name)]
        public string PerformanceTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PerformanceType_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PerformanceType_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string PerformanceTypeName = "PerformanceTypeName";
            public const string Code = "Code";
            public const string Description = "Description";
            public const string UserUpdate = "UserUpdate";
            public const string UserCreate = "UserCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_PerformanceTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_PerformanceType_Name)]
        public string PerformanceTypeName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

   
}
