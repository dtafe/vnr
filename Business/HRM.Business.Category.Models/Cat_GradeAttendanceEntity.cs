using System;

namespace HRM.Business.Category.Models
{
    public class Cat_GradeAttendanceEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string GradeAttendanceName { get; set; }    
        public string StdWorkDay { get; set; }       
        public string StdOvertimeDay { get; set; }
        public Decimal? HoursPerDay { get; set; }
        public DateTime? LeaveDayFrom { get; set; }
        public DateTime? LeaveDayTo { get; set; }
        public DateTime? OvertimeFrom { get; set; }
        public DateTime? OvertimeTo { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public Guid? CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string SalaryTimeType { get; set; }
        public int? SalaryTimeDay { get; set; }
        public Guid? WorkOnMondayID { get; set; }
        public Guid? WorkOnTuesdayID { get; set; }
        public Guid? WorkOnWednesdayID { get; set; }
        public Guid? WorkOnThursdayID { get; set; }
        public Guid? WorkOnFridayID { get; set; }
        public Guid? WorkOnSaturdayID { get; set; }
        public Guid? WorkOnSundayID { get; set; }

        public string AttendanceMethod { get; set; }
        public bool? IsDeductInLateOutEarly { get; set; }
        public bool? IsLateEarlyRounding { get; set; }

        public Nullable<bool> IsRosterLeave { get; set; }
        public Nullable<bool> IsFormulaLeave { get; set; }
        public string LeaveWorkdayFormula { get; set; }
        public bool? IsRosterOT { get; set; }
        public bool? IsFormulaOT { get; set; }
        public string OTWorkdayFormula { get; set; }
        public int? HourOnWorkDate { get; set; }
        public bool? ShiftHours { get; set; }
        public bool? FixHours { get; set; }
        public int? ExpHourPerDay { get; set; }
        public DateTime? ExpHourFrom { get; set; }
        public DateTime? ExpHourTo { get; set; }
        public bool? LateEarly { get; set; }
        public bool? RoundByLateEarly { get; set; }
        public bool? Round { get; set; }
        public bool? RoundByMonth { get; set; }
        public Guid? OTNomal { get; set; }
        public Guid? OTLeaveDay { get; set; }
        public Guid? OTHoliday { get; set; }
        public Guid? OTNomalNight { get; set; }
        public Guid? OTLeaveDayNight { get; set; }
        public Guid? OTHolidayNight { get; set; }
        public int? TotalDayAnnualLeaveOnYear { get; set; }
        public string FormulaAnnualLeave { get; set; }
        public int? Seniority { get; set; }
        public string OptionReceive { get; set; }

        public Nullable<bool> IsApplySubSalaryNewQuitEmp { get; set; }
        public Nullable<double> MinWorkDayNewQuitEmp { get; set; }
        public Nullable<double> RateUneven { get; set; }
        public Nullable<int> MinWorkDay { get; set; }
        public string EDType { get; set; }
        public Nullable<bool> IsMonthlyMidCutOff { get; set; }
        public Nullable<int> MidCutOffDay { get; set; }



        public string RosterType { get; set; }
        public Nullable<bool> IsReceiveOvertimeBonus { get; set; }
        public Nullable<bool> IsLateEarlyFirstLastShiftRound { get; set; }
        public Nullable<bool> IsFixedOT { get; set; }
        public Nullable<bool> IsActualOT { get; set; }
        public Nullable<double> OTWorkDayFix { get; set; }
        public Nullable<int> OTWorkdayActual { get; set; }
        public Nullable<bool> IsFixedLeave { get; set; }
        public Nullable<bool> IsActualLeave { get; set; }
        public Nullable<double> LeaveWorkDayFix { get; set; }
        public Nullable<int> LeaveWorkdayActual { get; set; }
        public string DurationType { get; set; }
        public int? SalaryLeaveTimeDay { get; set; }
        public string SalaryLeaveTimeType { get; set; }
        public string MethodPayment { get; set; }
    }
    public class Cat_GradeAttendanceMultiEntity
    {
        public Guid ID { get; set; }
        public string GradeAttendanceName { get; set; }
    }
}
