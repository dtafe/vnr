using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_RateInsuranceModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_RateInsurance_HealthInsCompRate)]
        public double HealthInsCompRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RateInsurance_HealthInsEmpRate)]
        public double HealthInsEmpRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RateInsurance_SocialInsCompRate)]
        public double SocialInsCompRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RateInsurance_SocialInsEmpRate)]
        public double SocialInsEmpRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RateInsurance_UnemployInsCompRate)]
        public double UnemployInsCompRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RateInsurance_UnemployInsEmpRate)]
        public double UnemployInsEmpRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RateInsurance_ApplyFrom)]
        public Nullable<System.DateTime> ApplyFrom { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

       

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string HealthInsCompRate = "HealthInsCompRate";
            public const string HealthInsEmpRate = "HealthInsEmpRate";
            public const string SocialInsCompRate = "SocialInsCompRate";
            public const string SocialInsEmpRate = "SocialInsEmpRate";
            public const string UnemployInsCompRate = "UnemployInsCompRate";
            public const string UnemployInsEmpRate = "UnemployInsEmpRate";
            public const string ApplyFrom = "ApplyFrom";
        }
    }
    public class Cat_RateInsuranceSearchModel
    {
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    
}
