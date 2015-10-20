using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_YouthUnionPositionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_YouthUnionPosition_YouthUnionPositionName)]
        public string YouthUnionPositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_YouthUnionPosition_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_YouthUnionPosition_Notes)]
        public string Notes { get; set; }

        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string YouthUnionPositionName = "YouthUnionPositionName";
            public const string Code = "Code";
            public const string Notes = "Notes";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_YouthUnionPositionSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_YouthUnionPosition_YouthUnionPositionName)]
        public string YouthUnionPositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_YouthUnionPosition_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_YouthUnionPositionMultiModel
    {
        public Guid ID { get; set; }
        public string YouthUnionPositionName { get; set; }
        public int TotalRow { get; set; }
    }

}
