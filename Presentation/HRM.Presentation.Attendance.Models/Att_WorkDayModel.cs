using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_WorkdayModel : BaseViewModel
    {
        public string ProfileIds { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ProfileID)]
        public Guid ProfileID { get; set; }
        public Guid CutOffDurationID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_WorkDate)]
        [DataType(DataType.Date)]
        public DateTime WorkDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_FirstInTime)]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        //[DisplayFormat(DataFormatString = @"{0:dd\-MM\-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FirstInTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_FirstOutTime)]
        [DataType(DataType.DateTime)]
       // [UIHint("DateTime")]
        public DateTime? FirstOutTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LastInTime)]
        [DataType(DataType.DateTime)]
       // [UIHint("DateTime")]
        public DateTime? LastInTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LastOutTime)]
        [DataType(DataType.Date)]
        [UIHint("Date")]
        public DateTime? LastOutTime { get; set; }

        //[UIHint("MultiSelect")]
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftID)]
        public Guid? ShiftID { get; set; }

        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_WorkDuration)]
        public double WorkDuration { get; set; }

        [UIHint("Integer")]
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LateEarlyDuration)]
        public double? LateEarlyDuration { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        //[UIHint("MultiSelect")]
        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        [MaxLength(150)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        [MaxLength(150)]
        public string ShiftCode { get; set; }

        [MaxLength(50)]
        public string CodeAttendance { get; set; }
        [DataType(DataType.DateTime)]
        [UIHint("Date")]
        public DateTime? InTime1 { get; set; }
        [DataType(DataType.DateTime)]
        [UIHint("Date")]
        public DateTime? OutTime1 { get; set; }
        public DateTime? InTime2 { get; set; }
        public DateTime? OutTime2 { get; set; }
        public DateTime? InTime3 { get; set; }
        public DateTime? OutTime3 { get; set; }
        public DateTime? InTime4 { get; set; }
        public DateTime? OutTime4 { get; set; }
        public DateTime? InTimeRoot { get; set; }
        public DateTime? OutTimeRoot { get; set; }
        [UIHint("Integer")]
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LateDuration)]
        public Nullable<double> LateDuration1 { get; set; }
        [UIHint("Integer")]
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_EarlyDuration)]
        public Nullable<double> EarlyDuration1 { get; set; }
        public Nullable<double> LateDuration2 { get; set; }
        public Nullable<double> EarlyDuration2 { get; set; }
        public Nullable<double> LateDuration3 { get; set; }
        public Nullable<double> EarlyDuration3 { get; set; }
        public Nullable<double> LateDuration4 { get; set; }
        public Nullable<double> EarlyDuration4 { get; set; }
        [MaxLength(50)]
        public string SrcType { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        //[UIHint("MultiSelect")]
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWrongShift_ApprovedShift)]
        [MaxLength(150)]
        public string ShiftApproveName { get; set; }
        //[UIHint("MultiSelect")]
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWrongShift_ActualShift)]
        [MaxLength(150)]
        public string ShiftActualName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        //[UIHint("DropDownList_Status")]
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_Status)]
        [MaxLength(50)]
        public string Status { get; set; }
        public string StatusTranslate
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Status))
                    return this.Status.TranslateString();
                return string.Empty;
            }
        }
        //[UIHint("MultiSelect")]
        public Guid? ShiftApprove { get; set; }
        //[UIHint("MultiSelect")]
        public Guid? ShiftActual { get; set; }
        public double? LateEarlyRoot { get; set; }
        [MaxLength(1000)]
        public string LateEarlyReason { get; set; }
        [MaxLength(1000)]
        public string LateEarlyError { get; set; }
        public string udLeavedayCode1 { get; set; }
        public string udLeavedayCode2 { get; set; }
        public string udLeavedayStatus1 { get; set; }
        public string udLeavedayStatus2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_TamScanReasonMissName)]
        public Guid? MissInOutReason { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_TamScanReasonMissName)]
        public string TAMScanReasonMissName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftInTime)]
        public DateTime? ShiftInTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftOutTime)]
        public DateTime? ShiftOutTime { get; set; }
        public double? CoOut { get; set; }

        private bool _isChecked = false;
        public bool IsChecked { get { return false; } set { _isChecked = value; } }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public string ProfileCode { get; set; }
        public string OrgStructureName { get; set; }
        #region [Hien.Nguyen]
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_FirstInTime)]
        [DataType(DataType.Time)]
        [UIHint("TimeMask")]
        public string TempTimeIn { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LastOutTime)]
        [DataType(DataType.Time)]
        [UIHint("TimeMask")]
        public string TempTimeOut { get; set; }
        public string TempInTimeRoot { get; set; }
        public string TempOutTimeRoot { get; set; }
        #endregion

        public partial class FieldNames
        {
            public const string IsChecked = "IsChecked";
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string CodeAttendance = "CodeAttendance";
            public const string WorkDate = "WorkDate";
            public const string FirstInTime = "FirstInTime";
            public const string FirstOutTime = "FirstOutTime";
            public const string LastInTime = "LastInTime";
            public const string LastOutTime = "LastOutTime";
            public const string ShiftID = "ShiftID";
            public const string ShiftName = "ShiftName";
            public const string ShiftApprove = "ShiftApprove";
            public const string ShiftActual = "ShiftActual";
            public const string ShiftApproveName = "ShiftApproveName";
            public const string ShiftActualName = "ShiftActualName";
            public const string WorkDuration = "WorkDuration";
            public const string LateEarlyDuration = "LateEarlyDuration";
            public const string LateDuration = "LateDuration";
            public const string EarlyDuration = "EarlyDuration";
            public const string InTime1 = "InTime1";
            public const string OutTime1 = "OutTime1";
            public const string InTime2 = "InTime2";
            public const string OutTime2 = "OutTime2";
            public const string InTime3 = "OutTime3";
            public const string InTime4 = "OutTime4";
            public const string LateDuration1 = "LateDuration1";
            public const string EarlyDuration1 = "EarlyDuration1";
            public const string LateDuration2 = "LateDuration2";
            public const string EarlyDuration2 = "EarlyDuration2";
            public const string LateDuration3 = "LateDuration3";
            public const string EarlyDuration3 = "EarlyDuration3";
            public const string LateDuration4 = "LateDuration4";
            public const string EarlyDuration4 = "EarlyDuration4";
            public const string SrcType = "SrcType";
            public const string Type = "Type";
            public const string Status = "Status";
            public const string StatusTranslate = "StatusTranslate";
            public const string LateEarlyReason = "LateEarlyReason";
            public const string LateEarlyError = "LateEarlyError";
            public const string udLeavedayCode1 = "udLeavedayCode1";
            public const string udLeavedayCode2 = "udLeavedayCode2";
            public const string udLeavedayStatus1 = "udLeavedayStatus1";
            public const string udLeavedayStatus2 = "udLeavedayStatus2";
            public const string MissInOutReason = "MissInOutReason";
            public const string TAMScanReasonMissName = "TAMScanReasonMissName";
            public const string ShiftInTime = "ShiftInTime";
            public const string ShiftOutTime = "ShiftOutTime";
            public const string CoOut = "CoOut";
            public const string LateEarlyRoot = "LateEarlyRoot";
            public const string ShiftCode = "ShiftCode";

            public const string TempTimeIn = "TempTimeIn";
            public const string TempTimeOut = "TempTimeOut";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string InTimeRoot = "InTimeRoot";
            public const string OutTimeRoot = "OutTimeRoot";
            public const string TempInTimeRoot = "TempInTimeRoot";
            public const string TempOutTimeRoot = "TempOutTimeRoot";
        }
    }

    public class Att_WorkDaySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftID)]
        public string ShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_SrcType)]
        public string SrcType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_Type)]
        public string Type { get; set; }

        public int? LateDuration { get; set; }
        public int? EarlyDuration { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string ShiftID = "ShiftID";
            public const string OrgStructureID = "OrgStructureID";
        }
    }

    public class Att_WorkDayExportTemplateModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftID)]
        public string ShiftID { get; set; }
       

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_SrcType)]
        public string SrcType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_Type)]
        public string Type { get; set; }

        public int? LateDuration { get; set; }
        public int? EarlyDuration { get; set; }

        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string ShiftID = "ShiftID";
            public const string OrgStructureID = "OrgStructureID";
            public const string ShiftName = "ShiftName";
        }
    }


    public class Att_LateEarlySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftID)]
        public Guid? ShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_SrcType)]
        public string SrcType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_Type)]
        public string Type { get; set; }

        public int? LateDuration { get; set; }
        public int? EarlyDuration { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        //#region [Hien.Nguyen] Thêm trường để thêm form
        //[DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_LeaveDayTypeID)]
        //public string LeaveDayTypeCode { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_UserApprove)]
        //public Guid UserApproved { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_Comment)]
        //public string Comment { get; set; }
        //#endregion

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string ShiftID = "ShiftID";
            public const string OrgStructureID = "OrgStructureID";
        }
    }

    public class Att_ChangeStatusWorkdayModel
    {
        public string SelectedIds { get; set; }
        public string Type { get; set; }
    }

    public class LeaveDayValidate
    {
        private Guid _ProfileID;
        public Guid ProfileID
        {
            get
            {
                return _ProfileID;
            }
            set
            {
                _ProfileID = value;
            }
        }
        private DateTime _DateStart;
        public DateTime DateStart
        {
            get
            {
                return _DateStart;
            }
            set
            {
                _DateStart = value;
            }

        }
        private DateTime _DateEnd;
        public DateTime DateEnd
        {
            get
            {
                return _DateEnd;
            }
            set
            {
                _DateEnd = value;
            }

        }

        private double _Duration;
        public double Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = value;
            }

        }
        private double _TotalDuration;
        public double TotalDuration
        {
            get
            {
                return _TotalDuration;
            }
            set
            {
                _TotalDuration = value;
            }

        }

        public class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Duration = "Duration";
            public const string TotalDuration = "TotalDuration";
        }

    }
}
