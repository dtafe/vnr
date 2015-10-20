using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatReligionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Religion_ReligionName)]
        public string ReligionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Religion_Code)]
        public string Code { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ReligionName = "ReligionName";
            public const string Code = "Code";
        }
    }

    public class CatReligionSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Religion_ReligionName)]
        public string ReligionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Religion_Code)]
        public string Code { get; set; }

        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }
    }

    public class Cat_ReligionMultiModel
    {
        public Guid ID { get; set; }
        public string ReligionName { get; set; }
    }
}
