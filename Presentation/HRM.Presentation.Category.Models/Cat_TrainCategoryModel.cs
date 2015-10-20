using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_TrainCategoryModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Cat_TrainCategory_TrainCategoryName)]
        public string TrainCategoryName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_TrainCategory_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_TrainCategory_Description)]
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {

            public const string TrainCategoryName = "TrainCategoryName";
            public const string Type = "Type";
            public const string Description = "Description";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_TrainCategorySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_TrainCategory_TrainCategoryName)]
        public string TrainCategoryName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_TrainCategoryMultiModel
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string TrainCategoryName { get; set; }
    }
}
