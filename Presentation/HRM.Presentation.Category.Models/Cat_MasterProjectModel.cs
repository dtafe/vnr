using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_MasterProjectModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_MasterProject_ProjectCode)]
        public string ProjectCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_MasterProject_ProjectName)]
        public string ProjectName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_MasterProject_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_MasterProject_Note)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ProjectCode = "ProjectCode";
            public const string ProjectName = "ProjectName";
            public const string Type = "Type";
            public const string Note = "Note";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
        }
    }

    public class Cat_MasterProjectSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_MasterProject_ProjectName)]
        public string ProjectName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    //public class Cat_SourceAdsMultiModel
    //{
    //    public Guid ID { get; set; }
    //    [DisplayName(ConstantDisplay.HRM_Recruitment_SourceAdsName)]
    //    public string SourceAdsName { get; set; }
    //}

}
