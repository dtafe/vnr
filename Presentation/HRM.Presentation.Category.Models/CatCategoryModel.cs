using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatCategoryModel:BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Category_CategoryName)]
        [StringLength(500, ErrorMessage = ConstantDisplay.HRM_Category_Category_CategoryName_StringLength)]
        public string CategoryName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Category_Code)]
        [StringLength(50, ErrorMessage = ConstantDisplay.HRM_Category_Category_Code_StringLength)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Category_Description)]
        [StringLength(500, ErrorMessage = ConstantDisplay.HRM_Category_Category_Description_StringLength)]
        public string Description { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CategoryName = "CategoryName";
            public const string Code = "Code";
            public const string Description = "Description";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class CatCategorySearchModel {
        [DisplayName(ConstantDisplay.HRM_Category_Category_CategoryName)]
        public string CategoryName { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
