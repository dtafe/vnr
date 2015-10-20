using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_HDTJobTypePriceModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public Guid? HDTJobGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public string HDTJobGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_HDTJobTypeID)]
        public Guid? HDTJobTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_HDTJobTypeID)]
        public string HDTJobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Code)]
        public string HDTJobTypeCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Price)]
        public double? Price { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_DateOfEffect)]
        public DateTime? DateEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_DateOfEffect)]
        public DateTime? DateOfEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Status)]
        public string StatusView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_JobTypeID)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_JobTypeID)]
        public string TypeView { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_StandardElement)]
        public string StandardElement { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_EncryptJob)]
        public string EncryptJob { get; set; }
        public string HDTJobGroupCode { get; set; }
        public partial class FieldNames
        {
            public const string HDTJobTypeCode = "HDTJobTypeCode";
            public const string DateOfEffect = "DateOfEffect";
            public const string ID = "ID";
            public const string HDTJobTypeID = "HDTJobTypeID";
            public const string HDTJobTypeName = "HDTJobTypeName";
            public const string Price = "Price";
            public const string DateEffect = "DateEffect";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string Description = "Description";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserCreate = "UserCreate";
            public const string HDTJobGroupName = "HDTJobGroupName";
            public const string Type = "Type";
            public const string TypeView = "TypeView";
            public const string StandardElement = "StandardElement";
            public const string EncryptJob = "EncryptJob";
            public const string HDTJobGroupCode = "HDTJobGroupCode";
        }
    }

    public class Cat_HDTJobTypePriceSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_HDTJobTypeID)]
        public Guid? HDTJobTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_HDTJobTypePrice_DateOfEffect)]
        public DateTime? DateOfEffect { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
