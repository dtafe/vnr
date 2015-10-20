
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_AttConfigEntity
    {
        //HRM_ATT_OT_OTPERMIT
        #region HRM_ATT_OT_OTPERMIT
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
        /// <summary> bật cờ này lên thì mới áp dụng những bình thường vượt trần </summary>
        public bool? IsAllowOverLimit_Normal { get; set; }
        public bool? IsAllowOverLimit_Normal_Lev1 { get; set; }
        public bool? IsAllowOverLimit_Normal_Lev2 { get; set; }
        /// <summary> bật cờ này lên thì mới áp dụng những có trong ds cho phép vượt trần vượt trần  </summary>
        public bool? IsAllowOverLimit_AllowOver { get; set; }
        public bool? IsAllowOverLimit_AllowOver_Lev1 { get; set; }
        public bool? IsAllowOverLimit_AllowOver_Lev2 { get; set; }
        /// <summary> bật cờ này lên thì mới áp dụng cắt giờ làm việc theo ngày  </summary>
        public bool? IsAllowSplit { get; set; }
        /// <summary> Cho Phép Tách Tăng Ca </summary>
        public bool? IsAllowCut { get; set; }
        #endregion

        //HRM_ATT_WORKDAY_SUMMARY
        #region HRM_ATT_WORKDAY_SUMMARY
        public double? MaxHoursOneShift { get; set; }
        public double? MaxHoursNextInOut { get; set; }
        public double? MinMinutesSameAtt { get; set; }
        public string TypeLoadData { get; set; }
        public string SymbolIn { get; set; }
        public string SymbolOut { get; set; }
        public bool? DetectShift { get; set; }
        /// <summary>Kiểm Tra Ca Làm Việc liên tục dưới 12 tiếng</summary>
        public bool? IsRosterNonContinue12Hour { get; set; }
        public string ShiftCodeRosterGroup { get; set; }
        public string NameRosterGroup { get; set; }
        #endregion

        //HRM_ATT_LEAVE
        #region HRM_ATT_LEAVE


        public string AnnualLeave_FormularConfig { get; set; }
        public string AnnualLeave_FormularCompute { get; set; }
        public string AnnualSickLeave_FormularConfig { get; set; }
        public string AnnualSickLeave_FormularCompute { get; set; }
        public string AnnualInsuranceLeave_FormularConfig { get; set; }
        public string AnnualInsuranceLeave_FormularCompute { get; set; }
        /// <summary> Key Cho phép quyết toán ngày nghỉ : value33 (bool) </summary>
        public bool Leave_AllowFinalizationHolidays { get; set; }
        /// <summary> Cho Phép Phân Tích Phép Năm (Cơ Chế Mới) : value77 (bool) </summary>
        public bool? Leave_AllowAnalyzeAnnual { get; set; }
        // Phép năm chốt từ ngày 1 -> 31 hàng tháng : value46
        public bool AnnualBeginMonthToEndMonth { get; set; }
        public string AnnualMonthBeginYear { get; set; }
        public string AnnualDayBeginFullMonth { get; set; }
        public string AnnualSeniorMonth { get; set; }
        public string AnnualDayPerMonth { get; set; }
        public string AnnualRoundUp { get; set; }
        public string AnnualTypeProfileBegin { get; set; }
        public string AnnualDayMaxInMonthGetActual { get; set; }
        public string AnnualNormalCanGetFullYear { get; set; }
        public string AnnualSeniorCanGetEachLevel { get; set; }
        public string AnnualHDT4CanGetMoreNormal { get; set; }
        public string AnnualHDT5CanGetMoreNormal { get; set; }
        public string AnnualNonALNCodes { get; set; }
        public string AnnualMonthInYearToComputeSenior { get; set; }
        #endregion

        //HRM_ATT_OT
        #region HRM_ATT_OT

        /// <summary> Key Không tự động đăng ký ngày nghỉ lễ khi chấm công : value16 (bool) </summary>
        public bool NotAutoRegisterHolidayLeave { get; set; }
        /// <summary>Key Tăng ca ngày lễ tính 400% : value30 (bool)  </summary>
        public bool HolidaysCompute400 { get; set; }
        /// <summary>Key Cách tính dữ liệu quẹt thẻ 1 lần : value26 (Enum: MissTAM_LeaveType)  </summary>
        public string MisstamLeaveType { get; set; }
        /// <summary> Nếu tăng ca > 2h thì được nghỉ 30phut (vẫn tính tăng ca) : Value21 (bool:false) </summary>
        public bool IsAllowAddHourWhenOtThanTwoHour { get; set; }
        /// <summary>Cho phep người đăng kí OT có được quyền duyệt OT cho chính mình không : value45 (bool:true ) </summary>
        public bool DoNotSplitOtBreaktime { get; set; }
        /// <summary>Không tự động đăng ký ngày nghỉ lễ khi chấm công : value35  (bool:false)  </summary>
        public bool AllowRegisterOtWhenInmaternityregime { get; set; }
        /// <summary>Overtime vượt qua ngày hôm sau, không cắt, mà áp dụng như ca đêm. : value 37 (bool:true)  </summary>
        public bool NoCutOvertimePassday { get; set; }
        /// <summary>Phương thức tính ca đem theo khoản thời gian định nghỉ : value22 (Enum: ConfigNightShift : E_SHIFT)  </summary>
        public string ByperiodOfTime { get; set; }
        /// <summary> Thời gian tính ca đem từ : Value13 (TimePicker lưu xuống dạng 21:00) </summary>
        public DateTime? NightShiftFrom { get; set; }
        /// <summary> Thời gian tính ca đem đến : Value14 (TimePicker lưu xuống dạng 06:00) </summary>
        public DateTime? NightShiftTo { get; set; }
        /// <summary> Trạng Thái nào thì tình tăng ca : Value8 (lưu xuống dạng Enum) </summary>
        public string OverTimeStatus { get; set; }
        /// <summary>appConfig : HRM_ATT_ALLOWSAVEDUPLICATE :Cho phép OT nếu khác loại  </summary>
        public bool? AllowSaveDuplicate { get; set; }
        public bool? DoNotApproveOTMySeft { get; set; }
        public int? NumberLeaveApproveOverTime { get; set; }

        #endregion

        #region Tai Du Lieu Vao Ra
        public string Att_ServerName1 { get; set; }
        public string Att_UserID1 { get; set; }
        public string Att_Password1 { get; set; }
        public string Att_DBName1 { get; set; }
        public string Att_TableName1 { get; set; }
        public string Att_CardColName1 { get; set; }
        public string Att_DataColName1 { get; set; }
        public string Att_SerialNumber1 { get; set; }
        public string Att_In_Out1 { get; set; }
        public string Att_CodeAttendance1 { get; set; }
        public string Att_ServerName2 { get; set; }
        public string Att_UserID2 { get; set; }
        public string Att_Password2 { get; set; }
        public string Att_DBName2 { get; set; }
        public string Att_TableName2 { get; set; }
        public string Att_CardColName2 { get; set; }
        public string Att_DataColName2 { get; set; }
        public string Att_SerialNumber2 { get; set; }
        public string Att_In_Out2 { get; set; }
        public string Att_CodeAttendance2 { get; set; }

        public string Att_FilterData1 { get; set; }
        public string Att_FilterDataSplit1 { get; set; }
        public bool? Att_IsCodeEmp1 { get; set; }
        public bool? Att_IsActive1 { get; set; }
        public string Att_FilterData2 { get; set; }
        public string Att_FilterDataSplit2 { get; set; }
        public bool? Att_IsCodeEmp2 { get; set; }
        public bool? Att_IsActive2 { get; set; }
        #endregion

    }
}
