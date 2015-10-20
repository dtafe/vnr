using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Kai_ReportKaizenDataMonthlyEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string KaizenName { get; set; }
        public Nullable<System.DateTime> Month { get; set; }
        public string CategoryName { get; set; }
        public Nullable<double> MarkIdea { get; set; }
        public Nullable<double> MarkPerform { get; set; }
        public Nullable<double> SumMark { get; set; }
        public Nullable<double> SumAmount { get; set; }
        public Nullable<double> Accumulate { get; set; }
        public string OrgStructureName { get; set; }

         public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string KaizenName = "KaizenName";
            public const string Month = "Month";
            public const string CategoryName = "CategoryName";
            public const string MarkIdea = "MarkIdea";
            public const string MarkPerform = "MarkPerform";
            public const string SumMark = "SumMark";
            public const string SumAmount = "SumAmount";
            public const string Accumulate = "Accumulate";
        }
    }
}
