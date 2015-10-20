using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_DataGroupDetailModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_DataGroupDetai_DataGroupID)]
        public System.Guid DataGroupID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_DataGroupDetai_FieldName)]
        public string FieldName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_DataGroupDetai_ChildFieldName)]
        public string ChildFieldName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_DataGroupDetai_Value)]
        public string Value { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_DataGroupDetai_Notes)]
        public string Notes { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_DataGroupDetai_ObjectName)]
        public string ObjectName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_DataGroupDetai_ChildFieldName1)]
        public string ChildFieldName1 { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_DataGroupDetai_Exclusions)]
        public string Exclusions { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_DataGroupDetai_ForeignKey)]
         public string ForeignKey { get; set; }
         public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string DataGroupID = "DataGroupID";
            public const string FieldName = "FieldName";
            public const string ChildFieldName = "ChildFieldName";
            public const string Value = "Value";
            public const string Notes = "Notes";
            public const string ObjectName = "ObjectName";
            public const string ChildFieldName1 = "ChildFieldName1";
            public const string Exclusions = "Exclusions";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Cat_DataGroupDetailMultiModel
    {
        public string ForeignKey { get; set; }

    }
}
