using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SyncItemModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_Code)]
        public string Code { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_SyncID)]
        public Guid? SyncID { get; set; }
           [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_OuterField)]
        public string OuterField { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_InnerField)]
        public string InnerField { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_AllowNull)]
        public bool? AllowNull { get; set; }
          [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_AllowDuplicate)]
        public bool? AllowDuplicate { get; set; }
          [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_DuplicateGroup)]
        public int? DuplicateGroup { get; set; }
          [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_FilterValues)]
        public string FilterValues { get; set; }
           [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_IsExcluded)]
        public bool? IsExcluded { get; set; }
          [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_Description)]
        public string Description { get; set; }
    

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string SyncID = "SyncID";
            public const string OuterField = "OuterField";
            public const string InnerField = "InnerField";
            public const string AllowNull = "AllowNull";
            public const string AllowDuplicate = "AllowDuplicate";
            public const string DuplicateGroup = "DuplicateGroup";
            public const string FilterValues = "FilterValues";
            public const string IsExcluded = "IsExcluded";
            public const string Description = "Description";
        }
    }

    public class Cat_SyncItemSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Cat_SyncItem_Code)]
        public string Code { get; set; }
      
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
   
}
