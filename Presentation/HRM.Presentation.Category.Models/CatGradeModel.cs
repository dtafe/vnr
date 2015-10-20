using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatGradeMultiModel 
    {
        public Guid ID { get; set; }
        public string GradeCfgName { get; set; }
    }
    public class CatGradeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Grade_GradeCfgName)]
        public string GradeCfgName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Grade_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Grade_Description)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public Guid? CurrencyID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public string CurrencyName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Grade_HourOnWorkDate)]
        public double? HourOnWorkDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Grade_SalaryTimeType)]
        public string SalaryTimeType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Grade_SalaryTimeDay)]
        public int SalaryTimeDay { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string GradeCfgName = "GradeCfgName";
            public const string Code = "Code";
            public const string Description = "Description";
            public const string CurrencyID = "CurrencyID";
            public const string CurrencyName = "CurrencyName";
            public const string HourOnWorkDate = "HourOnWorkDate";
            public const string SalaryTimeType = "SalaryTimeType";
            public const string SalaryTimeDay = "SalaryTimeDay";
        }
    }
    public class CatGradeSearchModel {
        [DisplayName(ConstantDisplay.HRM_Category_Grade_GradeCfgName)]
        public string GradeCfgName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Grade_Code)]
        public string Code { get; set; }
    }
}
