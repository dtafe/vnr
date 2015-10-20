using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_ReportPayMoneyEatModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string BranchName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(50)]
        public string DepartmentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        [MaxLength(50)]
        public string TeamName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        [MaxLength(50)]
        public string SectionName { get; set; }  
   
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public string CateringName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public List<Guid?> CateringIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_EmpTypes)]
        public string EmpTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_EmpTypes)]
        public List<int?> EmpTypes { get; set; }

        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public string CanteenName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public List<Guid?> CanteenIDs { get; set; }

        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public string LineName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public List<Guid?> LineIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_UserPrint)]
        public string UserPrint { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DatePrint)]
        public DateTime? DatePrint { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountEat)]
        public int? CountEat { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_SubtractSalary)]
        public Double? SubtractSalary { get; set; }
        public Boolean IsIncludeQuitEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Province_CountryId)]
        public List<Guid?> CountryID { get; set; }


        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";   
            public const string CateringName = "CateringName";
            public const string CateringID = "CateringID";
            public const string CanteenName = "CanteenName";
            public const string CanteenID = "CanteenID";
            public const string LineID = "LineID";
            public const string LineName = "LineName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CountEat = "CountEat";
            public const string SubtractSalary = "SubtractSalary";
            public const string EmpTypeName = "EmpTypeName";
            public const string EmpTypes = "EmpTypes";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
        }
    }

    public class Can_ReportPayMoneyEatSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public List<Guid?> ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public List<Guid?> CateringID { get; set; }    

        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public List<Guid?> CanteenID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public List<Guid?> LineID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_OrgStructureId)]
        public List<Guid?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_EmpTypes)]
        public List<Guid?> EmpTypes { get; set; }
        public Boolean IsIncludeQuitEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Province_CountryId)]
        public List<Guid?> CountryID { get; set; }
        public Guid ExportID { get; set; }
    }
}
