using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class Cat_GradeAttendanceModel :BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Position_Description)]  
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_Code)]  
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_GradeAttendanceName)]       
        public string GradeAttendanceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_StdWorkDay)]        
        public string StdWorkDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_StdOvertimeDay)]
        public string StdOvertimeDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_HoursPerDay)]
        public Decimal? HoursPerDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_LeaveDayFrom)]
        public DateTime? LeaveDayFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_LeaveDayTo)]
        public DateTime? LeaveDayTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_OvertimeFrom)]
        public DateTime? OvertimeFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_OvertimeTo)]
        public DateTime? OvertimeTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_SalaryTimeType)]
        public string SalaryTimeType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_DurationType)]
        public string DurationType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_SalaryTimeDay)]
        public int? SalaryTimeDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_SalaryLeaveTimeDay)]

        public int? SalaryLeaveTimeDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_SalaryLeaveTimeType)]
        public string SalaryLeaveTimeType { get; set; }


        #region [Hien.Nguyen] 21/10/2014 - Thêm chức năng chỉnh sửa Chế Độ Lương
        [DisplayName(ConstantDisplay.HRM_Attendance_GradeAttendance_IsApplySubSalaryNewQuitEmp)]
        public Nullable<bool> IsApplySubSalaryNewQuitEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_GradeAttendance_MinWorkDayNewQuitEmp)]
        public Nullable<double> MinWorkDayNewQuitEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_GradeAttendance_RateUneven)]
        public Nullable<double> RateUneven { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_GradeAttendance_MinWorkDay)]
        public Nullable<int> MinWorkDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_GradeAttendance_EDType)]
        public string EDType { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_GradeAttendance_IsMonthlyMidCutOff)]
        public Nullable<bool> IsMonthlyMidCutOff { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_GradeAttendance_MidCutOffDay)]
        public Nullable<int> MidCutOffDay { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_FixedLeave)]
        [DefaultValue(false)]
        public Nullable<bool> IsFixedLeave { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_ActualLeave)]
          [DefaultValue(false)]
          public Nullable<bool> IsActualLeave { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_RosterLeave)]
          [DefaultValue(false)]
          public Nullable<bool> IsRosterLeave { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_FormulaLeave)]
          [DefaultValue(false)]
          public Nullable<bool> IsFormulaLeave { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_Day)]
          [DefaultValue(false)]
          public Nullable<double> LeaveWorkDayFix { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_FormulaLeave)]
          public string LeaveWorkdayFormula { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_Day)]
          public Nullable<int> LeaveWorkdayActual { get; set; }
          [DisplayName(ConstantDisplay.HRM_HR_Profile_AttendanceMethod)]
          [DefaultValue(false)]
          public Nullable<bool> IsLateEarlyRounding { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_ReceiveOvertimeBonus)]
          [DefaultValue(false)]
          public Nullable<bool> IsReceiveOvertimeBonus { get; set; }
          [DisplayName(ConstantDisplay.HRM_HR_Profile_AttendanceMethod)]
          [DefaultValue(false)]
          public Nullable<bool> IsLateEarlyFirstLastShiftRound { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_FixedLeave)]
          [DefaultValue(false)]
          public Nullable<bool> IsFixedOT { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_ActualLeave)]
          [DefaultValue(false)]
          public Nullable<bool> IsActualOT { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_Day)]
          public Nullable<double> OTWorkDayFix { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_RosterLeave)]
          [DefaultValue(false)]
          public bool IsRosterOT { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_FormulaLeave)]
          [DefaultValue(false)]
          public bool IsFormulaOT { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_FormulaLeave)]
          public string OTWorkdayFormula { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_Day)]
          public Nullable<int> OTWorkdayActual { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_HourOnWorkDate)]
          public int? HourOnWorkDate { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_ShiftHours)]
          [DefaultValue(false)]
          public bool ShiftHours { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_FixedLeave)]
          [DefaultValue(false)]
          public bool FixHours { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_ExpHourPerDay)]
          public int? ExpHourPerDay { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_ReportDetailOvertime_DateFrom)]
          public DateTime? ExpHourFrom { get; set; }
          [DisplayName(ConstantDisplay.HRM_Attendance_ReportDetailOvertime_DateTo)]
        public DateTime? ExpHourTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceMethod_1)]
          public string AttendanceMethod { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DeductInLateOutEarly)]  //Trừ đi trễ về sớm
        public Nullable<bool> IsDeductInLateOutEarly { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_IsOverTimeType)]  //Trừ đi trễ về sớm
        public Nullable<bool> IsOvertimeType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_LateEarly_1)]
        [DefaultValue(false)]
        public bool LateEarly { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_RoundByLateEarly)]
        [DefaultValue(false)]
        public bool RoundByLateEarly { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Round)]
        [DefaultValue(false)]
        public Nullable<bool> Round { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_RoundByMonth)]
        [DefaultValue(false)]
        public bool RoundByMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OT_Nomal)]
        public Guid? OTNomal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OT_LeaveDay)]    //thường
        public Guid? OTLeaveDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OT_Holiday)]            //nghỉ
        public Guid? OTHoliday { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OT_Nomal_Night)]                 //lễ
        public Guid? OTNomalNight { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OT_LeaveDay_Night)]                     //Đêm tường
        public Guid? OTLeaveDayNight { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OT_Holiday_Night)]                                //Đêm nghĩ
        public Guid? OTHolidayNight { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_ReceivePerformanceBonus)]                                    //Đêm lễ
        //public bool? IsReceivePerformanceBonus { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_ReceiveNightShiftBonus)]
        //public bool? IsReceiveNightShiftBonus { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_NotGetPublicHoliday)]
        //public bool? NotGetPublicHoliday { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TotalDayAnnualLeaveOnYear)]
        public int? TotalDayAnnualLeaveOnYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_FormulaAnnualLeave)]
        public string FormulaAnnualLeave { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Seniority)]
        public int? Seniority { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OptionReceive)]
        [DefaultValue("")]
        public string OptionReceive { get; set; }
        public string RosterType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_MethodPayment)]
        public string MethodPayment { get; set; }

        #endregion

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string GradeAttendanceName = "GradeAttendanceName";
            public const string StdWorkDay = "StdWorkDay";
            public const string StdOvertimeDay = "StdOvertimeDay";
            public const string HoursPerDay = "HoursPerDay";
            public const string LeaveDayFrom = "LeaveDayFrom";
            public const string LeaveDayTo = "LeaveDayTo";
            public const string OvertimeFrom = "OvertimeFrom";
            public const string OvertimeTo = "OvertimeTo";
            public const string Description = "Description";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class Cat_GradeAttendanceSearchModel 
    {
        public string GradeAttendanceName { get; set; }
        public string StdWorkDay { get; set; }        
        public string StdOvertimeDay { get; set; }        
        public double? HoursPerDay { get; set; }        
        public DateTime? LeaveDayFrom { get; set; }        
        public DateTime? LeaveDayTo { get; set; }        
        public DateTime? OvertimeFrom { get; set; }
        public DateTime? OvertimeTo { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_GradeAttendanceMultiModel
    {
        public Guid ID { get; set; }
        public string GradeAttendanceName { get; set; }
    }
}
