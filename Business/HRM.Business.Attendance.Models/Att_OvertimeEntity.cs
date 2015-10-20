using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_OvertimeEntity : HRMBaseModel
    {
        public string DeclineReason { get; set; }

        public string Host { get; set; }
        public string Valid { get; set; }
        public string ActionStatus { get; set; }
        public Guid ProfileID { get; set; }
        public Guid OvertimeTypeID { get; set; }
        public bool IsNightShift { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime? WorkDateEnd { get; set; }
        public DateTime? BreakStart { get; set; }
        public DateTime? BreakEnd { get; set; }
        public Guid? ShiftID { get; set; }
        public double? ConvertedHours { get; set; }
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
        public string MethodPayment { get; set; }
        public string Status { get; set; }
        public string MethodPaymentTranslate { get; set; }
        public string StatusTranslate { get; set; }
        public string DurationType { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public Guid? UserRegisterID { get; set; }
        public DateTime? DateRegister { get; set; }
        public double RegisterHours { get; set; }
        public string Comment { get; set; }
        public string ReasonOT { get; set; }
        public double? ApproveHours { get; set; }
        public DateTime? DateApprove1 { get; set; }
        public DateTime? DateApprove2 { get; set; }
        public Guid UserApproveID { get; set; }
        public string UserApproveName1 { get; set; }
        public Guid? UserApproveID3 { get; set; }
        public string UserApproveName3 { get; set; }
        public Guid? UserApproveID2 { get; set; }
        public string UserApproveName2 { get; set; }
        public string ApproveComment { get; set; }
        public Guid? UserConfirmID { get; set; }
        public string ConfirmComment { get; set; }
        public double ConfirmHours { get; set; }
        public string ProfileName { get; set; }
        public string ShiftName { get; set; }
        public string OvertimeTypeName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }

        public DateTime? WorkDateRoot { get; set; }
        public double? AnalyseHour { get; set; }
        public bool? udWorkdayNonShift { get; set; }

        public string udTypeBeginOTWithShift { get; set; }

        public string udLimitHourType { get; set; }
        public bool? udIsLimitHour { get; set; }
        public bool? udIsLimitHourLv1 { get; set; }
        public bool? udIsLimitHourLv2 { get; set; }

        public bool? udIsLimitHourLv2_Validate { get; set; }
        public bool? udIsLimitHourLv1_Validate { get; set; }
        public bool? udIsLimitHour_Validate { get; set; }

        public string udLimitHourType_Validate { get; set; }

        public string udLeaveTypeCode { get; set; }
        public string udLeaveTypeCode2 { get; set; }
        public string udLeavedayStatus1 { get; set; }
        public string udLeavedayStatus2 { get; set; }

        public Guid? udAlreadyOvertimeID { get; set; }
        public double? udRateOvertimeType { get; set; }
        public double? udHourByDate { get; set; }
        public double? udHourByWeek { get; set; }
        public double? udHourByMonth { get; set; }
        public double? udHourByYear { get; set; }
        public string udOvertimeStatus { get; set; }
        /// <summary>
        /// so gio của tổng OT trong ngày
        /// </summary>
        public double? HourByDate { get; set; }

        /// <summary>
        /// Số giờ của tổng OT trong Tuần
        /// </summary>
        public double? HourByWeek { get; set; }
        /// <summary>
        /// Số giờ của tổng OT trong Tháng
        /// </summary>
        public double? HourByMonth { get; set; }
        /// <summary>
        /// Số giờ của tổng OT trong Năm
        /// </summary>
        public double? HourByYear { get; set; }

        /// <summary>
        /// bi vượt mức Limit
        /// </summary>
        public bool? IsLimitHour { get; set; }
        /// <summary>
        /// bi vượt mức Limit lv1
        /// </summary>
        public bool? IsLimitHourLv1 { get; set; }
        /// <summary>
        /// bi vượt mức Limit lv2
        /// </summary>
        public bool? IsLimitHourLv2 { get; set; }

        /// <summary>
        /// Loại OT bi vượt limit
        /// </summary>
        public string LimitHourType { get; set; }

        public double? HourToTimeOff { get; set; }
        public double? TimeRegister { get; set; }

        public double? TimeOffInLieuRate { get; set; }
       
        public double? TimeOffReal { get; set; }
        public DateTime? LeaveDay1 { get; set; }
        public DateTime? LeaveDay2 { get; set; }
        public bool? IsNonOvertime { get; set; }

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


        public string BranchCode { get; set; }
        public string OrgStructureCode { get; set; }
        public string TeamCode { get; set; }
        public string SectionCode { get; set; }
        public string JobtitleName { get; set; }
        public string PositionName { get; set; }
        public string UserExport { get; set; }
        public string BranchName { get; set; }
        public string OrgName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public DateTime? DateOvertime { get; set; }
        public DateTime? DateExport { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? udInTime { get; set; }
        public DateTime? udOutTime { get; set; }

        public string udIsNonOvertime { get; set; }
        public string UserRegister { get; set; }

        public string E_UNIT { get; set; }

        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }

        public partial class FieldNames
        {
            public const string udIsNonOvertime = "udIsNonOvertime";
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string OvertimeTypeID = "OvertimeTypeID";
            public const string IsNightShift = "IsNightShift";
            public const string IsConfirm = "IsConfirm";
            public const string IsExcludeFromAudit = "IsExcludeFromAudit";
            public const string WorkDate = "WorkDate";
            public const string WorkDateEnd = "WorkDateEnd";
            public const string BreakStart = "BreakStart";
            public const string BreakEnd = "BreakEnd";
            public const string RegisterHours = "RegisterHours";
            public const string ConvertedHours = "ConvertedHours";
            public const string TimeFrom = "TimeFrom";
            public const string TimeTo = "TimeTo";
            public const string UserApproveID = "UserApproveID";
            public const string UserConfirmID = "UserConfirmID";
            public const string ApproveHours = "ApproveHours";
            public const string MethodPayment = "MethodPayment";
            public const string Status = "Status";
            public const string ReasonOT = "ReasonOT";
            public const string DurationType = "DurationType";
            public const string ConfirmComment = "ConfirmComment";
            public const string ApproveComment = "ApproveComment";
            public const string ConfirmHours = "ConfirmHours";
            public const string Comment = "Comment";
            public const string UserRegister = "UserRegister";
            public const string DateRegister = "DateRegister";
            public const string UserApprove = "UserApprove";
            public const string DateFirstApprove = "DateFirstApprove";
            public const string DateApprove = "DateApprove";
            public const string RegisterCode = "RegisterCode";
            public const string BasicHours = "BasicHours";
            public const string SerialCode = "SerialCode";
            public const string MenuID = "MenuID";
            public const string FoodID = "FoodID";
            public const string ShiftID = "ShiftID";
            public const string Food2ID = "Food2ID";
            public const string Menu2ID = "Menu2ID";
            public const string OvertimeResonID = "OvertimeResonID";
            public const string Label = "Label";
            public const string LeaveDayDate = "LeaveDayDate";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string UserApproveID2 = "UserApproveID2";
            public const string WorkDateRoot = "WorkDateRoot";
            public const string IsNotValidContinue = "IsNotValidContinue";
            public const string SendEmailStatus = "SendEmailStatus";
            public const string IsNotValidLimitTime = "IsNotValidLimitTime";
            public const string SpecialReasonID = "SpecialReasonID";
            public const string UserApproveName = "UserApproveName";
            public const string UserApproveName2 = "UserApproveName2";
            public const string ShiftName = "ShiftName";
            public const string OvertimeTypeName = "OvertimeTypeName";
            public const string HourByDate = "HourByDate";
            public const string HourByWeek = "HourByWeek";
            public const string HourByMonth = "HourByMonth";
            public const string HourByYear = "HourByYear";
            public const string IsLimitHour = "IsLimitHour";
            public const string IsLimitHourLv1 = "IsLimitHourLv1";
            public const string IsLimitHourLv2 = "IsLimitHourLv2";
            public const string LimitHourType = "LimitHourType";
            public const string HourToTimeOff = "HourToTimeOff";
            public const string TimeRegister = "TimeRegister";
            public const string TimeOffInLieuRate = "TimeOffInLieuRate";

            public const string CodeEmp = "CodeEmp";
            public const string BranchCode = "BranchCode";
            public const string OrgStructureCode = "OrgStructureCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";
            public const string JobtitleName = "JobtitleName";
            public const string PositionName = "PositionName";
            public const string UserExport = "UserExport";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string DateOvertime = "DateOvertime";
            public const string DateExport = "DateExport";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string udInTime = "udInTime";
            public const string udOutTime = "udOutTime";

            public const string AnalyseHour = "AnalyseHour";
            public const string udHourByDate = "udHourByDate";
            public const string udHourByWeek = "udHourByWeek";
            public const string udHourByMonth = "udHourByMonth";
            public const string udHourByYear = "udHourByYear";
            public const string udLeaveTypeCode = "udLeaveTypeCode";
            public const string udOvertimeStatus = "udOvertimeStatus";
            public const string Valid = "Valid";
            public const string TotalRow = "TotalRow";
        }



    }
    public class Att_OvertimeInfoFillterAnalyze : HRMBaseModel
    {
        /// <summary>
        /// [True]Lấy tăng ca Ngoài ca làm việc (Đầu ca, Cuối Ca)
        /// [False] lấy tăng ca nằm trong shiftitem
        /// </summary>
        public bool isAllowGetOTOutterShift { get; set; }
        /// <summary>
        /// lấy tăng ca đầu ca
        /// </summary>
        public bool isAllowGetBeforeShift { get; set; }
        /// <summary>
        /// lấy tăng ca cuối ca
        /// </summary>
        public bool isAllowGetAfterShift { get; set; }
        /// <summary>
        /// lấy tăng ca trong ca
        /// </summary>
        public bool isAllowGetInShift { get; set; }
        /// <summary>
        /// tính loại tăng ca vào đúng ngày thực tế
        /// </summary>
        public bool isAllowGetTypeBaseOnActualDate { get; set; }
        /// <summary>
        /// tính loại tăng ca và ngày bắt đầu tăng ca
        /// </summary>
        public bool isAllowGetTypeBaseOnBeginShift { get; set; }
        /// <summary>
        /// tính loai tăng ca vào ngày kết thúc tăng ca
        /// </summary>
        public bool isAllowGetTypeBaseOnEndShift { get; set; }
        /// <summary>
        /// có tính OT ca đêm không
        /// </summary>
        public bool isAllowGetNightShift { get; set; }
        /// <summary>
        /// Có cắt vào lúc 12h Khuya hay không?
        /// </summary>
        public bool isBreakMiddleNight { get; set; }
        /// <summary>
        /// số giờ tối thiểu để tính OT
        /// </summary>
        public double MininumOvertimeHour { get; set; }
        /// <summary>
        /// số giờ tối đa để tính OT
        /// </summary>
        public double MaximumOvertimeHour { get; set; }


    }
    public class Att_OvertimeLimit : HRMBaseModel
    {
        /// <summary>
        /// Giới hạn giờ limit overtime theo ngày
        /// </summary>
        public double? limitHour_ByDay { get; set; }
        /// <summary>
        /// Giới hạn giờ limit level 1 overtime theo ngày
        /// </summary>
        public double? limitHour_ByDay_Lev1 { get; set; }
        /// <summary>
        /// Giới hạn giờ limit level 2 overtime theo ngày
        /// </summary>
        public double? limitHour_ByDay_Lev2 { get; set; }

        /// <summary>
        /// Giới hạn giờ limit overtime theo tuần
        /// </summary>
        public double? limitHour_ByWeek { get; set; }
        /// <summary>
        /// Giới hạn giờ limit level 1 overtime theo tuần
        /// </summary>
        public double? limitHour_ByWeek_Lev1 { get; set; }
        /// <summary>
        /// Giới hạn giờ limit level 2 overtime theo tuần
        /// </summary>
        public double? limitHour_ByWeek_Lev2 { get; set; }

        /// <summary>
        /// Giới hạn giờ limit overtime theo Tháng
        /// </summary>
        public double? limitHour_ByMonth { get; set; }
        /// <summary>
        /// Giới hạn giờ limit  overtime theo Tháng
        /// </summary>
        public double? limitHour_ByMonth_Lev1 { get; set; }
        /// <summary>
        /// Giới hạn giờ limit overtime theo Tháng
        /// </summary>
        public double? limitHour_ByMonth_Lev2 { get; set; }

        /// <summary>
        /// Giới hạn giờ limit overtime theo Năm
        /// </summary>
        public double? limitHour_ByYear { get; set; }
        /// <summary>
        /// Giới hạn giờ limit lev 1 overtime theo Năm
        /// </summary>
        public double? limitHour_ByYear_Lev1 { get; set; }
        /// <summary>
        /// Giới hạn giờ limit lev 2 overtime theo Năm
        /// </summary>
        public double? limitHour_ByYear_Lev2 { get; set; }

        /// <summary>
        /// Màu sắc dạng #fffff để để hiện thị màu sắc 
        /// </summary>
        public string limitColor { get; set; }

        /// <summary>
        /// Màu sắc dạng #fffff để để hiện thị màu sắc level 1 
        /// </summary>
        public string limitColor_Lev1 { get; set; }

        /// <summary>
        /// Màu sắc  dạng #fffff để để hiện thị màu sắc level 2
        /// </summary>
        public string limitColor_Lev2 { get; set; }



    }
    public class Att_OvertimeTiny : HRMBaseModel
    {
        public Guid? ID { get; set; }
        public Guid? ProfileID { get; set; }
        public Guid? OvertimeTypeID { get; set; }
        public double? RegisterHours { get; set; }
        public double? ApproveHours { get; set; }
        public double? ConfirmHours { get; set; }
        public string Status { get; set; }
        public partial class FieldNames
        {
            public static string ID = "ID";
            public static string ProfileID = "ProfileID";
            public static string OvertimeTypeID = "OvertimeTypeID";
            public static string RegisterHours = "RegisterHours";
            public static string ApproveHours = "ApproveHours";
            public static string ConfirmHours = "ConfirmHours";
            public static string Status = "Status";
        }
    }

    public class OvertimePermitEntity
    {
        public double? limitHour_ByDay { get; set; }
        public double? limitHour_ByDay_Lev1 { get; set; }
        public double? limitHour_ByDay_Lev2 { get; set; }

        public double? limitHour_ByWeek { get; set; }
        public double? limitHour_ByWeek_Lev1 { get; set; }
        public double? limitHour_ByWeek_Lev2 { get; set; }

        public double? limitHour_ByMonth { get; set; }
        public double? limitHour_ByMonth_Lev1 { get; set; }
        public double? limitHour_ByMonth_Lev2 { get; set; }

        public double? limitHour_ByYear { get; set; }
        public double? limitHour_ByYear_Lev1 { get; set; }
        public double? limitHour_ByYear_Lev2 { get; set; }

        public string limitColor { get; set; }
        public string limitColor_Lev1 { get; set; }
        public string limitColor_Lev2 { get; set; }

        public bool? IsAllowOverLimit_Normal { get; set; } //bật cờ này lên thì mới áp dụng những bình thường vượt trần 
        public bool? IsAllowOverLimit_Normal_Lev1 { get; set; }
        public bool? IsAllowOverLimit_Normal_Lev2 { get; set; }

        public bool? IsAllowOverLimit_AllowOver { get; set; } //bật cờ này lên thì mới áp dụng những có trong ds cho phép vượt trần vượt trần 
        public bool? IsAllowOverLimit_AllowOver_Lev1 { get; set; }
        public bool? IsAllowOverLimit_AllowOver_Lev2 { get; set; }

        public bool? IsAllowSplit { get; set; }//bật cờ này lên thì mới áp dụng cắt giờ làm việc theo ngày 
        public bool? IsAllowCut { get; set; }//Cho Phép Tách Tăng Ca
    }
    public class Att_OvertimeEntityEdit : HRMBaseModel
    {
        public string ActionStatus { get; set; }
        public Guid ProfileID { get; set; }
        public string Status { get; set; }
        public Guid OvertimeTypeID { get; set; }
        public DateTime WorkDate { get; set; }
        public Guid? ShiftID { get; set; }
        public string MethodPayment { get; set; }
        public double RegisterHours { get; set; }
        public string ReasonOT { get; set; }
        public Guid UserApproveID { get; set; }
        //public string UserApproveName1 { get; set; }
        public Guid? UserApproveID2 { get; set; }
        //public string UserApproveName2 { get; set; }
        //public string ProfileName { get; set; }
        //public string ShiftName { get; set; }
        //public string OvertimeTypeName { get; set; }
        public partial class FieldNames
        {
            public static string ID = "ID";
            public static string ProfileID = "ProfileID";
          //  public static string ProfileName = "ProfileName";
            public static string OvertimeTypeID = "OvertimeTypeID";
            public static string WorkDate = "WorkDate";
            public static string RegisterHours = "RegisterHours";
            public static string UserApproveID = "UserApproveID";
            public static string UserApproveID2 = "UserApproveID2";
            public static string MethodPayment = "MethodPayment";
            public static string Status = "Status";
            public static string ReasonOT = "ReasonOT";
            public static string ShiftID = "ShiftID";
           // public static string UserApproveName2 = "UserApproveName2";
            //public static string ShiftName = "ShiftName";
           // public static string OvertimeTypeName = "OvertimeTypeName";
           // public static string TimeRegister = "TimeRegister";
        }
    }
}
