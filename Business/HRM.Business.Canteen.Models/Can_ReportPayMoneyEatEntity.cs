using System;

namespace HRM.Business.Canteen.Models
{
    public class Can_ReportPayMoneyEatEntity : HRM.Business.BaseModel.HRMBaseModel
    {      
        public string CodeEmp { get; set; }       
        public string ProfileName { get; set; }
        public string BranchName { get; set; }     
        public string DepartmentName { get; set; }    
        public string TeamName { get; set; }      
        public string SectionName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserPrint { get; set; }
        public DateTime? DatePrint { get; set; }
        public string CateringName { get; set; }
        public int? CountEat { get; set; }
        public double? SubtractSalary { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string CateringName = "CateringName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CountEat = "CountEat";
            public const string SubtractSalary = "SubtractSalary";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
        }
    }
}
