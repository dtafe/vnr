using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;


namespace HRM.Presentation.Category.Models
{
    public class CatLateEarlyRuleModel : BaseViewModel
    {
      
        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_GradeCfgID)]
        public Guid GradeCfgID { get; set; }
          [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_GradeAttendanceName)] 
        public Guid? GradeAttID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_GradeName)]
        public string GradeCfgName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_Order)]
        [UIHint("Integer")]
        public int Order { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_MinValue)]
        [UIHint("Number")]
        public double MinValue { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_MaxValue)]
        [UIHint("Integer")]
        public double? MaxValue { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_RoundValue)]
        [UIHint("Number")]
        public double RoundValue { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_Comment)]
        public string Comment { get; set; }

        public string Code { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string GradeCfgID = "GradeCfgID";
            public const string GradeCfgName = "GradeCfgName";
            public const string Order = "Order";
            public const string MinValue = "MinValue";
            public const string MaxValue = "MaxValue";
            public const string RoundValue = "RoundValue";
            public const string Comment = "Comment";
            public const string Code = "Code";
        }
    }
    public class CatLateEarlyRuleSearchModel {
        [DisplayName(ConstantDisplay.HRM_Category_LateEarlyRule_GradeName)]
        public string GradeCfgName { get; set; }
    }
    public class CatLateEarlyRuleByCfgIDModelSearch
    {
        public Guid GradeAttID { get; set; }
    }
    public class CatLateEarlyRuleMultiModel
    {
        public Guid ID { get; set; }
        public string GradeCfgName { get; set; }
    }
}
