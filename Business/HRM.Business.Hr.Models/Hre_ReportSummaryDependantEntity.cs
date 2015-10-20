using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{

    public class Hre_ReportSummaryDependantEntity : HRMBaseModel
    {
        public Guid ProfileID { get; set; }

        [MaxLength(150)]
        public string DependantName { get; set; }

        public DateTime? MonthOfEffect { get; set; }

        public DateTime? MonthOfExpiry { get; set; }

        [MaxLength(50)]
        public string CodeTax { get; set; }

        [MaxLength(150)]
        public string ProfileName { get; set; }

        [MaxLength(50)]
        public string CodeEmp { get; set; }

        public Nullable<System.DateTime> CompleteDate { get; set; }

        public int TotalDependant { get; set; }
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
        }
    }
}
