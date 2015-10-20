using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_ReportMealTimeDetailNoEatModel :BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_OrgCode)]
        [MaxLength(50)]
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string BranchCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(50)]
        public string DepartmentCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(50)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        [MaxLength(50)]
        public string TeamCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        [MaxLength(50)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        [MaxLength(50)]
        public string SectionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        [MaxLength(50)]
        public string SectionName { get; set; }     
        //[DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]     
        public DateTime? Date { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Notes)]
        public string Notes { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public string CateringName { get; set; }
        public Guid? CateringID { get; set; }
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public string CanteenName { get; set; }
        public Guid? CanteenID { get; set; }
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public string LineName { get; set; }
        public Guid? LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_UserPrint)]
        public string UserPrint { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DatePrint)]
        public DateTime? DatePrint { get; set; }
        public Guid ExportID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public List<Guid> workPlaceID { get; set; }
        public Boolean IsIncludeQuitEmp { get; set; }
        public String Status { get; set; }
        public partial class FieldNames
        {
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string BranchCode = "BranchCode";
            public const string BranchName = "BranchName";
            public const string DepartmentCode = "DepartmentCode";
            public const string DepartmentName = "DepartmentName";
            public const string TeamCode = "TeamCode";
            public const string TeamName = "TeamName";
            public const string SectionCode = "SectionCode";
            public const string SectionName = "SectionName";           
            public const string Date = "Date";
            public const string Notes = "Notes";
            public const string CateringID = "CateringID";
            public const string CanteenID = "CanteenID";
            public const string LineID = "LineID";
            public const string CateringName = "CateringName";
            public const string CanteenName = "CanteenName";
            public const string LineName = "LineName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";     
        }
    }
    public class Can_ReportMealTimeDetailNoEatSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public String CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public Guid? CateringID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public List<Guid?> CateringIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public Guid? CanteenID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public List<Guid?> CanteenIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public Guid? LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public List<Guid?> LineIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_OrgStructureId)]
        public List<Guid?> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public List<Guid?> WorkPlaceID { get; set; }
        public Boolean IsIncludeQuitEmp { get; set; }
        public String Status { get; set; }
        public Guid ExportID { get; set; }
    }  
}
