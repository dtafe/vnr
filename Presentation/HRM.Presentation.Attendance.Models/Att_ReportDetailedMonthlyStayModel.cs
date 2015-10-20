using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_ReportDetailedMonthlyStayModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string BranchCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(50)]
        public string DepartmentCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        [MaxLength(50)]
        public string TeamCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        [MaxLength(50)]
        public string SectionCode { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DivisionCode)]
        [MaxLength(50)]
        public string DivisionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        [MaxLength(150)]
        public string ShiftName { get; set; }

        public Guid? LeadayTypeID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_LeaveDayTypeID)]
       
        public string LeaveDayTypeIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }

        public bool isNotAllowZero { get; set; }
        public Dictionary<string, Dictionary<string, double>> Leave { get; set; }

       
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportLeaveMonth_TotalDateLeave)]
        public int? TotalDateLeave { get; set; }

        public double? Paid { get; set; }
        public double? SC { get; set; }
        public double? SU { get; set; }
        public double? SD { get; set; }
        public double? SP { get; set; }
        public double? DP { get; set; }
        public double? DA { get; set; }
        public double? D { get; set; }
        public double? Bus { get; set; }
        public double? SH { get; set; }
        public double? M { get; set; }
        public double? CL { get; set; }
        public double? AL { get; set; }
        public double? CO { get; set; }
        public double? DL { get; set; }
        public double? SM { get; set; }
        public double? TSC { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Remark)]
        public string Remark { get; set; }
        public Guid ExportId { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool NoDisplay0Data { get; set; }

        public string ValueFields { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
        
        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string FromDate = "FromDate";
            public const string ToDate = "ToDate";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string BranchCode = "BranchCode";
            public const string DivisionCode = "DivisionCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";
            
            public const string ShiftName = "ShiftName";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";

            public const string Paid = "Paid";
            public const string SC = "SC";
            public const string SU = "SU";
            public const string SD = "SD";
            public const string SP = "SP";
            public const string DP = "DP";
            public const string DA = "DA";

            public const string D = "D";
            public const string Bus = "Bus";
            public const string SH = "SH";
            public const string M = "M";
            public const string CL = "CL";
            public const string AL = "AL";
            public const string CO = "CO";

            public const string DL = "DL";
            public const string SM = "SM";
            public const string TSC = "TSC";
            public const string Remark = "Remark";

            public const string OrgStructureID = "OrgStructureID";
            public const string TotalDateLeave = "TotalDateLeave";
            public const string DateExport = "DateExport";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserExport = "UserExport";
        }
       
    }
}
