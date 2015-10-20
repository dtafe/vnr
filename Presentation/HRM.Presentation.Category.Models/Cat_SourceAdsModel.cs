using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SourceAdsModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Recruitment_SourceAdsName)]
        public string SourceAdsName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_Notes)]
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string SourceAdsName = "SourceAdsName";
            public const string Description = "Description";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Cat_SourceAdsSearchModel
    {
        public string SourceAdsName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_SourceAdsMultiModel
    {
        public Guid ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_SourceAdsName)]
        public string SourceAdsName { get; set; }
    }

}
