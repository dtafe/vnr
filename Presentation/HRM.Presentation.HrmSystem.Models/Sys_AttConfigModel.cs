using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_AttConfigModel : BaseViewModel
    {
        //HRM_ATT_OT_OTPERMIT
        //tab Att_OvertimePermit (Cấu hình giới hạn tăng ca)
        #region HRM_ATT_OT_OTPERMIT
        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitHour_ByDay)]
        public double? limitHour_ByDay { get; set; }
        public double? limitHour_ByDay_Lev1 { get; set; }
        public double? limitHour_ByDay_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitHour_ByWeek)]
        public double? limitHour_ByWeek { get; set; }
        public double? limitHour_ByWeek_Lev1 { get; set; }
        public double? limitHour_ByWeek_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitHour_ByMonth)]
        public double? limitHour_ByMonth { get; set; }
        public double? limitHour_ByMonth_Lev1 { get; set; }
        public double? limitHour_ByMonth_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitHour_ByYear)]
        public double? limitHour_ByYear { get; set; }
        public double? limitHour_ByYear_Lev1 { get; set; }
        public double? limitHour_ByYear_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_limitColor)]
        public string limitColor { get; set; }
        public string limitColor_Lev1 { get; set; }
        public string limitColor_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_IsAllowOverLimit_Normal)]
        public bool? IsAllowOverLimit_Normal { get; set; } //bật cờ này lên thì mới áp dụng những bình thường vượt trần 
        public bool? IsAllowOverLimit_Normal_Lev1 { get; set; }
        public bool? IsAllowOverLimit_Normal_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_OTPermit_IsAllowOverLimit_AllowOver)]
        public bool? IsAllowOverLimit_AllowOver { get; set; } //bật cờ này lên thì mới áp dụng những có trong ds cho phép vượt trần vượt trần 
        public bool? IsAllowOverLimit_AllowOver_Lev1 { get; set; }
        public bool? IsAllowOverLimit_AllowOver_Lev2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_IsAllowSplit)]
        public bool? IsAllowSplit { get; set; }//bật cờ này lên thì mới áp dụng cắt giờ làm việc theo ngày 

        [DisplayName(ConstantDisplay.HRM_Att_IsAllowCut)]
        public bool? IsAllowCut { get; set; }//Cho Phép Tách Tăng Ca
        #endregion

        //HRM_ATT_WORKDAY_SUMMARY
        //tab Att_Workday_Summary (cấu hình chấm công)
        #region HRM_ATT_WORKDAY_SUMMARY
        [DisplayName(ConstantDisplay.HRM_Att_WORKDAY_SUMMARY_MaxHoursOneShift)]
        public double? MaxHoursOneShift { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_WORKDAY_SUMMARY_MaxHoursNextInOut)]
        public double? MaxHoursNextInOut { get; set; }
        [DisplayName(ConstantDisplay.HRM_WORKDAY_SUMMARY_Att_MinMinutesSameAtt)]
        public double? MinMinutesSameAtt { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_WORKDAY_SUMMARY_TypeLoadData)]
        public string TypeLoadData { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_WORKDAY_SUMMARY_SymbolIn)]
        public string SymbolIn { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_WORKDAY_SUMMARY_SymbolOut)]
        public string SymbolOut { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_WORKDAY_SUMMARY_DetectShift)]
        public bool? DetectShift { get; set; }
        /// <summary>Kiểm Tra Ca Làm Việc liên tục dưới 12 tiếng</summary>
        [DisplayName(ConstantDisplay.HRM_Att_WORKDAY_SUMMARY_IsRosterNonContinue12Hour)]
        public bool? IsRosterNonContinue12Hour { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_WORKDAY_SUMMARY_ShiftCodeRosterGroup)]
        public string ShiftCodeRosterGroup { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_WORKDAY_SUMMARY_NameRosterGroup)]
        public string NameRosterGroup { get; set; }

        #endregion

        //tabl Att_Leave - Cấu hình nghỉ
        #region HRM_ATT_LEAVE
        [DisplayName(ConstantDisplay.HRM_Att_AnnualLeave_FormularConfig)]
        public string AnnualLeave_FormularConfig { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AnnualLeave_FormularCompute)]
        public string AnnualLeave_FormularCompute { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AnnualSickLeave_FormularConfig)]
        public string AnnualSickLeave_FormularConfig { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AnnualSickLeave_FormularCompute)]
        public string AnnualSickLeave_FormularCompute { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AnnualInsuranceLeave_FormularConfig)]
        public string AnnualInsuranceLeave_FormularConfig { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_AnnualInsuranceLeave_FormularCompute)]
        public string AnnualInsuranceLeave_FormularCompute { get; set; }
        /// <summary>Key Cho phép quyết toán ngày nghỉ : value33 (bool) </summary>
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AllowFinalizationHolidays)]
        public bool Leave_AllowFinalizationHolidays { get; set; }
        /// <summary>Cho Phép Phân Tích Phép Năm (Cơ Chế Mới) : value77 (bool) </summary>
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AllowAnalyzeAnnual)]
        public bool? Leave_AllowAnalyzeAnnual { get; set; }
        // Phép năm chốt từ ngày 1 -> 31 hàng tháng : value46
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualBeginMonthToEndMonth)]
        public bool AnnualBeginMonthToEndMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualMonthBeginYear)]
        public string AnnualMonthBeginYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualDayBeginFullMonth)]
        public string AnnualDayBeginFullMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualSeniorMonth)]
        public string AnnualSeniorMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualDayPerMonth)]
        public string AnnualDayPerMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualRoundUp)]
        public string AnnualRoundUp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualTypeProfileBegin)]
        public string AnnualTypeProfileBegin { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualDayMaxInMonthGetActual)]
        public string AnnualDayMaxInMonthGetActual { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualNormalCanGetFullYear)]
        public string AnnualNormalCanGetFullYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualSeniorCanGetEachLevel)]
        public string AnnualSeniorCanGetEachLevel { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualHDT4CanGetMoreNormal)]
        public string AnnualHDT4CanGetMoreNormal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualHDT5CanGetMoreNormal)]
        public string AnnualHDT5CanGetMoreNormal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualNonALNCodes)]
        public string AnnualNonALNCodes { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Leave_AnnualMonthInYearToComputeSenior)]
        public string AnnualMonthInYearToComputeSenior { get; set; }
        #endregion

        //HRM_ATT_OT
        //tab Att_Overtime (Tăng ca)
        #region HRM_ATT_OT
        //Key Không tự động đăng ký ngày nghỉ lễ khi chấm công : value16 (bool)
        [DisplayName(ConstantDisplay.HRM_Att_NotAutoRegisterHolidayLeave)]
        public bool NotAutoRegisterHolidayLeave { get; set; }
        //Key Tăng ca ngày lễ tính 400% : value30 (bool)
        [DisplayName(ConstantDisplay.HRM_Att_Ot_HolidaysCompute400)]
        public bool HolidaysCompute400 { get; set; }
        //Key Cách tính dữ liệu quẹt thẻ 1 lần : value26 (Enum: MissTAM_LeaveType)
        [DisplayName(ConstantDisplay.HRM_Att_Misstam_LeaveType)]
        public string MisstamLeaveType { get; set; }
        //Nếu tăng ca > 2h thì được nghỉ 30phut (vẫn tính tăng ca) : Value21 (bool:false)
        [DisplayName(ConstantDisplay.HRM_Att_Ot_IsAllowAddHourWhenOtThanTwoHour)]
        public bool IsAllowAddHourWhenOtThanTwoHour { get; set; }
        //Cho phep người đăng kí OT có được quyền duyệt OT cho chính mình không : value45 (bool:true )          
        [DisplayName(ConstantDisplay.HRM_Att_Ot_DoNotSplitOtBreaktime)]
        public bool DoNotSplitOtBreaktime { get; set; }
        //Không tự động đăng ký ngày nghỉ lễ khi chấm công : value35  (bool:false)
        [DisplayName(ConstantDisplay.HRM_Att_Ot_AllowRegisterOtWhenInmaternityregime)]
        public bool AllowRegisterOtWhenInmaternityregime { get; set; }
        //Overtime vượt qua ngày hôm sau, không cắt, mà áp dụng như ca đêm. : value 37 (bool:true)
        [DisplayName(ConstantDisplay.HRM_Att_Ot_NoCutOvertimePassday)]
        public bool NoCutOvertimePassday { get; set; }
        //Phương thức tính ca đem theo khoản thời gian định nghỉ : value22 (Enum: ConfigNightShift : E_SHIFT)
        [DisplayName(ConstantDisplay.HRM_Att_Ot_ByperiodOfTime)]
        public string ByperiodOfTime { get; set; }
        //Thời gian tính ca đem từ : Value13 (TimePicker lưu xuống dạng 21:00)
        [DisplayName(ConstantDisplay.HRM_Att_Ot_NightShiftFrom)]
        public DateTime? NightShiftFrom { get; set; }
        //Thời gian tính ca đem đến : Value14 (TimePicker lưu xuống dạng 06:00)
        [DisplayName(ConstantDisplay.HRM_Att_Ot_NightShiftTo)]
        public DateTime? NightShiftTo { get; set; }
        //Trạng Thái nào thì tình tăng ca : Value8 (lưu xuống dạng Enum)
        [DisplayName(ConstantDisplay.HRM_Att_Ot_OverTimeStatus)]
        public string OverTimeStatus { get; set; }

        //appConfig : HRM_ATT_ALLOWSAVEDUPLICATE
        [DisplayName(ConstantDisplay.HRM_Att_Ot_AllowSaveDuplicate)]
        public bool? AllowSaveDuplicate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Ot_DoNotApproveOTMySeft)]
        public bool? DoNotApproveOTMySeft { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Ot_NumberLeaveApproveOverTime)]
        public int? NumberLeaveApproveOverTime { get; set; }

        #endregion





        #region Tải DL vào ra
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        public string Att_ServerName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        public string Att_UserID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        public string Att_Password1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        public string Att_DBName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        public string Att_TableName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        public string Att_CardColName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        public string Att_DataColName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_SerialNumber)]
        public string Att_SerialNumber1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_In_Out)]
        public string Att_In_Out1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CodeAttendance)]
        public string Att_CodeAttendance1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterData)]
        public string Att_FilterData1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterDataSplit)]
        public string Att_FilterDataSplit1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsCodeEmp)]
        public bool? Att_IsCodeEmp1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsActive)]
        public bool? Att_IsActive1 { get; set; }


        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        public string Att_ServerName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        public string Att_UserID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        public string Att_Password2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        public string Att_DBName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        public string Att_TableName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        public string Att_CardColName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        public string Att_DataColName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_SerialNumber)]
        public string Att_SerialNumber2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_In_Out)]
        public string Att_In_Out2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CodeAttendance)]
        public string Att_CodeAttendance2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterData)]
        public string Att_FilterData2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterDataSplit)]
        public string Att_FilterDataSplit2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsCodeEmp)]
        public bool? Att_IsCodeEmp2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsActive)]
        public bool? Att_IsActive2 { get; set; }
        #endregion
    }

}
