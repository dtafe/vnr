using System;

namespace HRM.Business.Canteen.Models
{
    public class Can_ReportMealTimeDetailNoEatEntity : HRM.Business.BaseModel.HRMBaseModel
    {      
        public string CodeEmp { get; set; }       
        public string ProfileName { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }   
        public string BranchCode { get; set; }
        public string BranchName { get; set; }     
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }    
        public string TeamCode { get; set; }
        public string TeamName { get; set; }      
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserPrint { get; set; }
        public DateTime? DatePrint { get; set; }
        public string Notes { get; set; }
        public string CateringName { get; set; }
        public string CanteenName { get; set; }
        public string LineName { get; set; }

         public partial class FieldNames
        {
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string CodeBranch = "CodeBranch";
            public const string CodeDepartment = "CodeDepartment";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";
            public const string TimeIn = "TimeIn";
            public const string TimeOut = "TimeOut";
            public const string MealAllowanceName = "MealAllowanceName";
            public const string DateExport = "DateExport";
        }

    }
   
}
