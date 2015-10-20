using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_VeteranUnionPositionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_VeteranUnionPosition_VeteranUnionPositionName)]
        public string VeteranUnionPositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_VeteranUnionPosition_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_VeteranUnionPosition_Notes)]
        public string Notes { get; set; }

        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string VeteranUnionPositionName = "VeteranUnionPositionName";
            public const string Code = "Code";
            public const string Notes = "Notes";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_VeteranUnionPositionSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_VeteranUnionPosition_VeteranUnionPositionName)]
        public string VeteranUnionPositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_VeteranUnionPosition_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_VeteranUnionPositionMultiModel
    {
        public Guid ID { get; set; }
        public string VeteranUnionPositionName { get; set; }
        public int TotalRow { get; set; }
    }

}
