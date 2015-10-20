using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using System.ComponentModel;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SalaryRankModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryClassID)]
        public Guid? SalaryClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryClassName)]
        public string SalaryClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryClassTypeID)]
        public Guid? SalaryClassTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryClassTypeName)]
        public string SalaryClassTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryRankName)]
        public string SalaryRankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_Rate)]
        public double? Rate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryStandard)]
        public double? SalaryStandard { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryMin)]
        public double? SalaryMin { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryMax)]
        public double? SalaryMax { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_DateOfEffect)]
        public DateTime? DateOfEffect { get; set; }
        public string AbilityTitleENG { get; set; }
        public string AbilityTitleVNI { get; set; }
        public string SalaryClassCode { get; set; }
        public int? OrderNumber { get; set; }
        
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string SalaryClassID = "SalaryClassName";
            public const string SalaryClassName = "SalaryClassName";
            public const string SalaryClassTypeID = "SalaryClassTypeID";
            public const string SalaryClassTypeName = "SalaryClassTypeName";
            public const string SalaryRankName = "SalaryRankName";
            public const string Rate = "Rate";
            public const string SalaryStandard = "SalaryStandard";
            public const string SalaryMin = "SalaryMin";
            public const string SalaryMax = "SalaryMax";
            public const string DateOfEffect = "DateOfEffect";
            public const string UserCreate = "UserCreate";
        }
    }
    public class Cat_SalaryRankSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryRankName)]
        public string SalaryRankName { get; set; }
        public Guid? ClassID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_SalaryRankMultiModel
    {
        public Guid ID { get; set; }
        public string SalaryRankName { get; set; }
    } 
}
