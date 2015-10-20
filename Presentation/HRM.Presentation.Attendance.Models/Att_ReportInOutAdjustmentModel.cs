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
    public class Att_ReportInOutAdjustmentModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftID)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftActualName)]
        public string ShiftNameActual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftApproveName)]
        public string ShiftNameApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_InTime1)]
        public DateTime? InTime1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_OutTime1)]
        public DateTime? OutTime1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_InTimeRoot)]
        public DateTime? InTimeRoot { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_OutTimeRoot)]
        public DateTime? OutTimeRoot { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_WorkDate)]
        public DateTime WorkDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_Type)]
        public String Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_SrcType)]
        public String SrcType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_Status)]
        public String Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_Note)]
        public String Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LateDuration1)]
        public Int32 LateDuration1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_EarlyDuration1)]
        public Int32 EarlyDuration1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_EarlyDuration)]
        public Int32 LateEarlyDuration { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LateEarlyRoot)]
        public Int32 LateEarlyRoot { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LateEarlyReason)]
        public String LateEarlyReason { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public String DepartmentCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public String SectionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        public String BrandCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public String TeamCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportInOutAdjustment_ProfileOrgCode)]
        public String ProfileOrgCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportInOutAdjustment_ProfileOrgName)]
        public String ProfileOrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public String UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportInOutAdjustment_EarlyLateLess2h)]
        public String EarlyLateLess2h { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportInOutAdjustment_EarlyLateOver2h)]
        public String EarlyLateOver2h { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportInOutAdjustment_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportInOutAdjustment_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_MissInOutReason)]
        public string MissInOutReason { get; set; }
        public String PositionCode { get; set; }
        public String PositionName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string ShiftName = "ShiftName";
            public const string ShiftNameActual = "ShiftNameActual";
            public const string ShiftNameApprove = "ShiftNameApprove";
            public const string InTime1 = "InTime1";
            public const string OutTime1 = "OutTime1";
            public const string InTimeRoot = "InTimeRoot";
            public const string OutTimeRoot = "OutTimeRoot";
            public const string WorkDate = "WorkDate";
            public const string Type = "Type";
            public const string SrcType = "SrcType";
            public const string Status = "Status";
            public const string Note = "Note";
            public const string LateDuration1 = "LateDuration1";
            public const string EarlyDuration1 = "EarlyDuration1";
            public const string LateEarlyDuration = "LateEarlyDuration";
            public const string LateEarlyRoot = "LateEarlyRoot";
            public const string LateEarlyReason = "LateEarlyReason";
            public const string DepartmentCode = "DepartmentCode";
            public const string SectionCode = "SectionCode";
            public const string BrandCode = "BrandCode";
            public const string TeamCode = "TeamCode";
            public const string ProfileOrgCode = "ProfileOrgCode";
            public const string ProfileOrgName = "ProfileOrgName";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
            public const string EarlyLateLess2h = "EarlyLateLess2h";
            public const string EarlyLateOver2h = "EarlyLateOver2h";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string MissInOutReason = "MissInOutReason";
            public const string PositionCode = "PositionCode";
            public const string PositionName = "PositionName";
        }
        
    }

    public class Att_RptInOutAdjustmentSearchModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TAMScanReasonMiss)]
        public String TAMScanReasonMissIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public String OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public String CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PayrollGroupID)]
        public String PayrollGroupIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportInOutAdjustment_UserPrint)]
        public String UserExport { get; set; }
       
        public bool IsExport { get; set; }
        public String ValueFields { get; set; }
        public Guid ExportId { get; set; }

    }
}
