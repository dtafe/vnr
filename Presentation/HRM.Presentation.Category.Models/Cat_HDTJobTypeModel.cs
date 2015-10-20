using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_HDTJobTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_HDTJobTypeName)]
        public string HDTJobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Type)]
        public string TypeView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public Guid? HDTJobGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public string HDTJobGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Product_BonusPerUnit)]
        public double? Price { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_CardHistory_DateEffect)]
        public DateTime? DateEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Unit)]
        public string Unit { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Dept)]
        public string Dept { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Section)]
        public string Part { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Line)]
        public string Line { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Session)]
        public string Session { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Status)]
        public string StatusView { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_StandardElement)]
        public string StandardElement { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_EncryptJob)]
        public string EncryptJob { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_Code)]
        public string HDTJobGroupCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_HDTJobTypeNameHVN)]
        public string HDTJobTypeNameHVN { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string HDTJobTypeName = "HDTJobTypeName";
            public const string Code = "Code";
            public const string Type = "Type";
            public const string TypeView = "TypeView";
            public const string Description = "Description";
            public const string Price = "Price";
            public const string DateEffect = "DateEffect";
            public const string HDTJobGroupID = "HDTJobGroupID";
            public const string HDTJobGroupName = "HDTJobGroupName";
            public const string Dept = "Dept";
            public const string Part = "Part";
            public const string Line = "Line";
            public const string Session = "Session";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string Unit = "Unit";
            public const string HDTJobTypeNameHVN = "HDTJobTypeNameHVN";
            public const string StandardElement = "StandardElement";
            public const string EncryptJob = "EncryptJob";
            public const string HDTJobGroupCode = "HDTJobGroupCode";
        }
    }

    public class Cat_HDTJobTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_HDTJobTypeName)]
        public string HDTJobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_DayOff_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public Guid? HDTJobGroupID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_HDTJobTypeMultihModel
    {
        public Guid ID { get; set; }
        public string HDTJobTypeName { get; set; }
    }
    public class Cat_HDTJobTypeCodeMultiModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string HDTJobTypeName { get; set; }
    }

}
