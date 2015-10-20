using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportSummaryDependantDeductionEntity : HRMBaseModel
    {
        public Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string CodeTax { get; set; }
        public string DependantName { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public DateTime? MonthOfEffect { get; set; }
        public DateTime? MonthOfExpiry { get; set; }
        public DateTime? MonthFrom { get; set; }
        public DateTime? MonthTo { get; set; }

        public partial class FieldNames
        {
            public const string CompleteDate = "CompleteDate";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string DependantName = "DependantName";
            public const string MonthOfEffect = "MonthOfEffect";
            public const string MonthOfExpiry = "MonthOfExpiry";
            public const string CodeTax = "CodeTax";
            public const string CodeEmp = "CodeEmp";
            public const string TotalDependant = "TotalDependant";
            public const string MonthFrom = "MonthFrom";
            public const string MonthTo = "MonthTo";
        }
    }
}
