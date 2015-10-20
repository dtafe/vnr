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
    public class Att_OvertimeModel : BaseViewModel
    {
        public string DeclineReason { get; set; }
        public string Host { get; set; }
        public string MethodPaymentTranslate { get; set; }
        public string StatusTranslate { get; set; }

        public string Valid { get; set; }
        public string ProfileIds { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ProfileID)]
        public Guid ProfileID { get; set; }
        public Guid CutOffID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OvertimeTypeID)]
        [UIHint("DropDownList_OvertimeTypeID")]
        public Guid OvertimeTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateStart)]
        public DateTime DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateEnd)]
        public DateTime DateEnd { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkDate)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = ConstantFormat.HRM_Format_DayMonthYear_HoursMin, ApplyFormatInEditMode = true)]      
        public DateTime WorkDate { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkDate)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = ConstantFormat.HRM_Format_DayMonthYear_HoursMin, ApplyFormatInEditMode = true)]
        public DateTime? WorkDateRoot { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_AnalyseHour)]
        public double? AnalyseHour { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ShiftID)]
        public Guid? ShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }      
      
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_MethodPayment)]
        [MaxLength(50)]
        public string MethodPayment { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_RegisterHours)]
        [UIHint("Number")]
        public double RegisterHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ConfirmAproveHours)]
        public double ConfirmAproveHours
        {
            get { return RegisterHours; }
        }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ReasonOT)]
        [MaxLength(1000)]
        public string ReasonOT { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID)]
        public Guid UserApproveID { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID2)]
        public Guid? UserApproveID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID3)]
        public Guid? UserApproveID3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID3)]

        public string UserApproveName3 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ShiftID)]
        [MaxLength(150)]
        public string ShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OvertimeTypeID)]
        [MaxLength(150)]
        public string OvertimeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OvertimeReasonID)]
        public Guid? OvertimeResonID { get; set; }
      
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID)]
        [MaxLength(150)]
        public string UserApproveName1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID2)]
        [MaxLength(150)]
        public string UserApproveName2 { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        [UIHint("Number")]
        public double? ApproveHours { get; set; }
        [DefaultValue(0)]
        [UIHint("Number")]
        public double ConfirmHours { get; set; }

        public string udLeavedayCode1 { get; set; }
        public string udLeavedayCode2 { get; set; }
        public string udLeavedayStatus1 { get; set; }
        public string udLeavedayStatus2 { get; set; }
        [UIHint("Date")]
        public DateTime? InTime { get; set; }
        [UIHint("Date")]
        public DateTime? OutTime { get; set; }
        public List<Guid?> lstSelectedType { get; set; }

        private bool _isChecked = false;
        public bool IsChecked { get { return false; } set { _isChecked = value; } }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        [UIHint("Number")]
        public double? udHourByDate { get; set; }
        [UIHint("Number")]
        public double? udHourByWeek { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Att_Overtime_udHourByMonth)]
        public double? udHourByMonth { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Att_Overtime_udHourByYear)]
        public double? udHourByYear { get; set; }
        public string udLeaveTypeCode { get; set; }
        public string udOvertimeStatus { get; set; }
        public string  Comment { get; set; }
           [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OvertimeTypeTimeOffRateLit)]
        public string OvertimeTypeTimeOffRateLit { get; set; }
        public double? TimeOffInLieuRate { get; set; }           
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_TimeStartOverTime)]
        public string TimeStartOverTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_TypeTemp)]
        public string DurationType { get; set; }

        #region [Hien.Nguyen]
        [UIHint("Number")]
         //Lưu số giờ tăng ca nghỉ bù
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OffHours)]
        public double? HourToTimeOff { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_PaymentHours)]
        //Lưu số giờ tăng trả tiền
        public double? TimeRegister { get; set; }
        [UIHint("Number")]
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSummaryOvertimeMonth_COHour)]
        public double? TimeOffReal { get; set; }
        //Ngày nghỉ bù
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSummaryOvertimeMonth_LeaveDay1)]
        public DateTime? LeaveDay1 { get; set; }
        //Ngày nghỉ bù tiếp theo
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSummaryOvertimeMonth_LeaveDay2)]
        public DateTime? LeaveDay2 { get; set; }
        public bool IsNonOvertime { get; set; }
        #region [Hien.Nguyen]
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_FirstInTime)]
        //[DisplayFormat(DataFormatString = @"{0:HH\-mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [UIHint("TimeMask")]
        public string TempTimeIn { get; set; }
        public DateTime? TempTimeIn1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LastOutTime)]
        //[DisplayFormat(DataFormatString = @"{0:HH\-mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [UIHint("TimeMask")]
        public string TempTimeOut { get; set; }
        public DateTime? TempTimeOut1 { get; set; }

        [DataType(DataType.Time)]
        public string WorkDateTime { get; set; }
        public string RegisterTimeOut { get; set; }

        #endregion
        #endregion
        private Guid _id = Guid.Empty;
        public Guid Overtime_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }

        [DisplayName(ConstantDisplay.HRM_Att_Overtime_udIsNonOvertime)]
        public string udIsNonOvertime { get; set; }

        public string UserRegister { get; set; }
        public string UserLoginID { get; set; }
        public string E_UNIT { get; set; }

        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }

        public partial class FieldNames
        {
            public const string DeclineReason = "DeclineReason";
            public const string WorkDateTime = "WorkDateTime";
            public const string RegisterTimeOut = "RegisterTimeOut";
            public const string udIsNonOvertime = "udIsNonOvertime";
            public const string udHourByDate = "udHourByDate";
            public const string udHourByWeek  = "udHourByWeek";
            public const string udHourByMonth = "udHourByMonth";
            public const string udHourByYear  = "udHourByYear";
            public const string udOvertimeStatus = "udOvertimeStatus";
            public const string udLeaveTypeCode = "udLeaveTypeCode";

            public const string IsChecked = "IsChecked";
            public const string Id = "Id";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string ShiftID = "ShiftID";
            public const string ShiftName = "ShiftName";
            public const string Status = "Status";
            public const string WorkDate = "WorkDate";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string AnalyseHour = "AnalyseHour";
            public const string RegisterHours = "RegisterHours";
            public const string ConfirmAproveHours = "ConfirmAproveHours";
            public const string MethodPayment = "MethodPayment";
            public const string ReasonOT = "ReasonOT";
            public const string StatusTranslate = "StatusTranslate";
            public const string UserApproveID = "UserApproveID";
            public const string UserApproveName1 = "UserApproveName1";
            public const string UserApproveID2 = "UserApproveID2";
            public const string UserApproveName2 = "UserApproveName2";
            public const string OvertimeTypeName = "OvertimeTypeName";
            public const string UserApproveID3 = "UserApproveID3";
            public const string OvertimeTypeID = "OvertimeTypeID";
            public const string DurationType = "DurationType";
             
            public const string ApproveHours = "ApproveHours";
            public const string ConfirmHours = "ConfirmHours";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string UserName = "UserName";

            public const string OrgStructureName = "OrgStructureName";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName"; 

            public const string Comment="Comment";

            public const string OvertimeTypeTimeOffRateLit = "OvertimeTypeTimeOffRateLit";
            public const string TimeOffInLieuRate = "TimeOffInLieuRate";
            public const string TimeStartOverTime = "TimeStartOverTime";
            public const string TempTimeIn = "TempTimeIn";
            public const string TempTimeOut = "TempTimeOut";
            public const string IsNonOvertime = "IsNonOvertime";
            public const string Valid = "Valid";

            public const string E_DIVISION = "E_DIVISION";
            public const string E_UNIT = "E_UNIT";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";

        }
    }
    public class Att_OvertimeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ProfileName)]
        public string ProfileIDs { get; set; }

        //[DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        //[MaxLength(50)]
        //public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateStart)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateEnd)]
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ShiftID)]
        public Guid? ShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OvertimeTypeID)]
        public Guid? OvertimeTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_RegisterHours)]
        public double? RegisterHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public List<Guid?> OTType { get; set; }

        public double? OTRegisterFrom { get; set; }
        public double? OTRegisterTo { get; set; }
        public double? OTApproveFrom { get; set; }
        public double? OTApproveTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkDate)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = ConstantFormat.HRM_Format_DayMonthYear_HoursMin, ApplyFormatInEditMode = true)]      
        public DateTime WorkDate { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Att_OvertimeSearchOTModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateStart)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateEnd)]
        public DateTime? DateEnd { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ShiftID)]
        //public Guid? ShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OvertimeTypeID)]
        public Guid? OvertimeTypeID { get; set; }

     //   [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_RegisterHours)]
        //public double? RegisterHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkDate)]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = ConstantFormat.HRM_Format_DayMonthYear_HoursMin, ApplyFormatInEditMode = true)]
        //public DateTime WorkDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_JobtitleID)]
        public Guid? JobtitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_SysUserID)]
        public Guid? SysUserID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkPlace)]
        public string WorkPlace { get; set; }
        public Guid ExportId { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

    }

    public class Att_OvertimeAnalysisModel
    {
        public Guid ID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ProfileName)]
        public string ProfileID { get; set; }
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkDate)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = ConstantFormat.HRM_Format_DayMonthYear_HoursMin, ApplyFormatInEditMode = true)]
        public DateTime? WorkDate { get; set; }
        public DateTime? WorkHour { get; set; }

        [DataType(DataType.Time)]
        //[UIHint("TimeMask")]
        public string WorkDateTime { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkDateEnd)]
        //public DateTime WorkDateStart { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkDateEnd)]
        //public DateTime WorkDateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ShiftID)]
        public Guid? ShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_RegisterHours)]
        public double? RegisterHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_MethodPayment)]
        [MaxLength(50)]
        public string MethodPayment { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID)]
        public Guid? UserApproveID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID3)]
        public Guid? UserApproveID3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID3)]

        public string UserApproveName3 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID2)]
        public Guid? UserApproveID2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID)]
        public string UserApproveName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID2)]
        public string UserApproveName2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ReasonOT)]
        [MaxLength(1000)]
        public string ReasonOT { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OvertimeType_TypeTemp)]
        public string DurationType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OvertimeTypeID)]
        public Guid? OvertimeTypeID { get; set; }
        public string OvertimeTypeName { get; set; }
        public string UserLoginID { get; set; }

        public string UserCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserRegister { get; set; }
        
        public bool ByShiftProfile { get; set; }
    }
    public class Att_OvertimeUpdateModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ProfileID)]
        public Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_WorkDate)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = ConstantFormat.HRM_Format_DayMonthYear_HoursMin, ApplyFormatInEditMode = true)]
        public DateTime WorkDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ShiftID)]
        public Guid? ShiftID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_RegisterHours)]
        public double RegisterHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_MethodPayment)]
        [MaxLength(50)]
        public string MethodPayment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_OvertimeTypeID)]
        public Guid OvertimeTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Status)]
        [MaxLength(50)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ReasonOT)]
        [MaxLength(1000)]
        public string ReasonOT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID)]
        public Guid UserApproveID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID2)]
        public Guid? UserApproveID2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID3)]
        public Guid? UserApproveID3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID3)]
        public string UserApproveName3 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID2)]
        public string UserApproveName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ShiftID)]
        [MaxLength(150)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_UserApproveID)]
        [MaxLength(150)]
        public string UserApproveName1 { get; set; }
        public partial class FieldNames
        {
            

           
           
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
       
            public const string ShiftID = "ShiftID";
            public const string ShiftName = "ShiftName";
            public const string Status = "Status";
            public const string WorkDate = "WorkDate";
        
            public const string RegisterHours = "RegisterHours";
            public const string MethodPayment = "MethodPayment";
            public const string ReasonOT = "ReasonOT";
            public const string UserApproveID = "UserApproveID";
            public const string UserApproveName1 = "UserApproveName1";
            public const string UserApproveID2 = "UserApproveID2";
            public const string UserApproveName2 = "UserApproveName2";
         
            public const string OvertimeTypeID = "OvertimeTypeID";

            public const string ApproveHours = "ApproveHours";
   
        }
    }

    public class Att_OvertimeAnalysisLeaveModel
    {
        public Guid ID { get; set; }
        public Guid ProfileID { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime? LeaveDay1 { get; set; }
        public DateTime? LeaveDay2 { get; set; }
        
    }
    public class Att_OvertimeValidateModel
    {
        public String selectedIds { get; set; }
        public String buttonId { get; set; }
    }

    public class Att_PersonalSubmitOvertimeSearchModel
    {
        public String UserName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateEnd)]
        public DateTime? DateEnd { get; set; }
        public Guid ExportId { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
