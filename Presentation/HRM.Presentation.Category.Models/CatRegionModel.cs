using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatRegionMultiModel
    {
        public Guid ID { get; set; }
        public string RegionName { get; set; }
    }
    public class CatRegionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Region_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Region_RegionName)]
        public string RegionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Region_Notes)]
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Region_MinSalary)]
        public Nullable<double> MinSalary { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Region_MaxSalary)]
        public Nullable<double> MaxSalary { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string RegionName = "RegionName";
            public const string Notes = "Notes";
            public const string MinSalary = "MinSalary";
            public const string MaxSalary = "MaxSalary";
        }
    }
    public class CatRegionSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Region_RegionName)]
        public string RegionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Region_Code)]
        public string Code { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
