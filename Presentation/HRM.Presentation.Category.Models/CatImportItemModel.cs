using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatImportItemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_Code)]
        [StringLength(32, ErrorMessage = ConstantDisplay.HRM_Category_CatImportItem_Code_StringLength)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_ImportID)]
        public Guid? ImportID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_ChildFieldLevel1)]
        [StringLength(32, ErrorMessage = ConstantDisplay.HRM_Category_CatImportItem_ChildFieldLevel1_StringLength)]
     
        public string ChildFieldLevel1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_ChildFieldLevel2)]
        [StringLength(32, ErrorMessage = ConstantDisplay.HRM_Category_CatImportItem_ChildFieldLevel2_StringLength)]
        public string ChildFieldLevel2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_ExcelField)]
        [StringLength(32, ErrorMessage = ConstantDisplay.HRM_Category_CatImportItem_ExcelField_StringLength)]
        public string ExcelField { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_AllowNull)]
        public bool AllowNull { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_AllowDuplicate)]
        public bool AllowDuplicate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_DuplicateGroup)]
        [UIHint("Number")]
        public long? DuplicateGroup { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_Description)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_CatImportItem_Description_StringLength)]
        public string Description { get; set; }

        public string ImportName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_IsDefaultValue)]
        public Nullable<bool> IsDefaultValue { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Cat_ImportItem_DefaultValue)]
        public string DefaultValue { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string ImportID = "ImportID";
            public const string ImportName = "ImportName";
            public const string ChildFieldLevel1 = "ChildFieldLevel1";
            public const string ChildFieldLevel2 = "ChildFieldLevel2";
            public const string ExcelField = "ExcelField";
            public const string AllowNull = "AllowNull";
            public const string AllowDuplicate = "AllowDuplicate";
            public const string DuplicateGroup = "DuplicateGroup";
            public const string Description = "Description";
            public const string IsDefaultValue = "IsDefaultValue";
            public const string DefaultValue = "DefaultValue";
        }
    }
}
