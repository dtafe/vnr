using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Category.Models
{
    public class Cat_HDTJobGroupModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public string HDTJobGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UnitPrice_JobTypeID)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_Description)]
        public string Description { get; set; }
        public string StatusView { get; set; }
        public string Status { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string HDTJobGroupName = "HDTJobGroupName";
            public const string Type = "Type";
            public const string Description = "Description";
            public const string UserCreate = "UserCreate";
            public const string UserUpdate = "UserUpdate";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
        }
    }

    public class Cat_HDTJobGroupSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobGroup_HDTJobGroupName)]
        public string HDTJobGroupName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_HDTJobGroupMultiModel
    {
        public Guid ID { get; set; }
        public string HDTJobGroupName { get; set; }
    }
    public class Cat_HDTJobGroupCodeMultiModel
    {
        public Guid ID { get; set; }
         public string Code {get;set;}
        public string HDTJobGroupName { get; set; }
        public int TotalRow { get; set; }
    }

}
