//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Data.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cat_GradeCfg
    {
        public Cat_GradeCfg()
        {
            this.Att_Grade = new HashSet<Att_Grade>();
            this.Cat_JobTitle = new HashSet<Cat_JobTitle>();
            this.Cat_LateEarlyRule = new HashSet<Cat_LateEarlyRule>();
            this.Cat_PayrollElement = new HashSet<Cat_PayrollElement>();
            this.Sal_Grade = new HashSet<Sal_Grade>();
        }
    
        public System.Guid ID { get; set; }
        public string GradeCfgName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string WorkingTimeType { get; set; }
        public double WorkingTimeDay { get; set; }
        public string SalaryTimeType { get; set; }
        public int SalaryTimeDay { get; set; }
        public Nullable<double> SalaryMin { get; set; }
        public Nullable<double> SalaryMax { get; set; }
        public Nullable<double> StepIncrease { get; set; }
        public Nullable<double> IncomeMin { get; set; }
        public Nullable<double> IncomeMax { get; set; }
        public Nullable<double> HourOnWorkDate { get; set; }
        public Nullable<double> ExpHourPerDay { get; set; }
        public Nullable<System.DateTime> ExpHourFrom { get; set; }
        public Nullable<System.DateTime> ExpHourTo { get; set; }
        public bool IsReceiveNightShiftBonus { get; set; }
        public bool IsReceiveOvertimeBonus { get; set; }
        public bool IsReceivePerformanceBonus { get; set; }
        public double HealthInsCompRate { get; set; }
        public double HealthInsEmpRate { get; set; }
        public double SocialInsCompRate { get; set; }
        public double SocialInsEmpRate { get; set; }
        public double UnemployInsCompRate { get; set; }
        public double UnemployInsEmpRate { get; set; }
        public double PITCompRate { get; set; }
        public double PITEmpRate { get; set; }
        public bool IsDeductInLateOutEarly { get; set; }
        public bool IsDeductAbsenteesmDay { get; set; }
        public Nullable<System.Guid> NormalOTTypeID { get; set; }
        public Nullable<System.Guid> WeekendOTTypeID { get; set; }
        public Nullable<System.Guid> HolidayOTTypeID { get; set; }
        public Nullable<System.Guid> NightNormalOTTypeID { get; set; }
        public Nullable<System.Guid> NightWeekendOTTypeID { get; set; }
        public Nullable<System.Guid> NightHolidayOTTypeID { get; set; }
        public bool IsFixedOTType { get; set; }
        public Nullable<double> TotalDayAnnualLeaveOnYear { get; set; }
        public Nullable<double> Seniority { get; set; }
        public string FormulaAnnualLeave { get; set; }
        public string OptionReceive { get; set; }
        public string ServerUpdate { get; set; }
        public string ServerCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public bool IsNotScanCard { get; set; }
        public bool IsBaseOnRoster { get; set; }
        public string SalaryType { get; set; }
        public string PaidHealthIns { get; set; }
        public string PaidSocialIns { get; set; }
        public string PaidUnemploy { get; set; }
        public string PaidPIT { get; set; }
        public string MethodPIT { get; set; }
        public Nullable<System.DateTime> PITMonthApply { get; set; }
        public string RosterType { get; set; }
        public bool IsLateEarlyRounding { get; set; }
        public Nullable<int> MinWorkDay { get; set; }
        public string AttendanceMethod { get; set; }
        public Nullable<bool> IsFixedLeave { get; set; }
        public Nullable<bool> IsActualLeave { get; set; }
        public Nullable<bool> IsRosterLeave { get; set; }
        public Nullable<bool> IsFormulaLeave { get; set; }
        public Nullable<bool> IsFixedOT { get; set; }
        public Nullable<bool> IsActualOT { get; set; }
        public Nullable<bool> IsRosterOT { get; set; }
        public Nullable<bool> IsFormulaOT { get; set; }
        public Nullable<int> OTWorkdayActual { get; set; }
        public string OTWorkdayFormula { get; set; }
        public Nullable<double> OTWorkDayFix { get; set; }
        public Nullable<double> LeaveWorkDayFix { get; set; }
        public Nullable<int> LeaveWorkdayActual { get; set; }
        public string LeaveWorkdayFormula { get; set; }
        public string EDType { get; set; }
        public Nullable<System.Guid> WorkOnMondayID { get; set; }
        public Nullable<System.Guid> WorkOnTuesdayID { get; set; }
        public Nullable<System.Guid> WorkOnWednesdayID { get; set; }
        public Nullable<System.Guid> WorkOnThursdayID { get; set; }
        public Nullable<System.Guid> WorkOnFridayID { get; set; }
        public Nullable<System.Guid> WorkOnSaturdayID { get; set; }
        public Nullable<System.Guid> WorkOnSundayID { get; set; }
        public string SIContract { get; set; }
        public string HiContract { get; set; }
        public string UIContract { get; set; }
        public Nullable<bool> IsMonthlyCutOff { get; set; }
        public Nullable<bool> IsDurationCutOff { get; set; }
        public Nullable<bool> IsReceiveTerAlwBefore2008 { get; set; }
        public Nullable<bool> IsReceiveAnnualLeavePayment { get; set; }
        public Nullable<bool> IsDeductOveruseAnnualLeave { get; set; }
        public Nullable<bool> IsMonthlyMidCutOff { get; set; }
        public Nullable<int> MidCutOffDay { get; set; }
        public string ReceiveAnnualLeaveContract { get; set; }
        public string WorkHoursType { get; set; }
        public string Formula { get; set; }
        public string DurationType { get; set; }
        public Nullable<bool> NotGetPublicHoliday { get; set; }
        public string FormulaDesciption { get; set; }
        public string FormulaType { get; set; }
        public Nullable<bool> IsLateEarlyMonthlyRounding { get; set; }
        public Nullable<bool> IsSlideShift { get; set; }
        public Nullable<double> NoInsRange { get; set; }
        public Nullable<bool> IsPayInsProbation { get; set; }
        public Nullable<bool> IsLateEarlyFirstLastShiftRound { get; set; }
        public string OTCalculateStatus { get; set; }
        public Nullable<bool> IsApplySubSalaryNewQuitEmp { get; set; }
        public Nullable<double> RateUneven { get; set; }
        public Nullable<bool> IsNotNightShiftAttendance { get; set; }
        public Nullable<double> MinWorkDayNewQuitEmp { get; set; }
        public Nullable<System.Guid> GradeAttID { get; set; }
    
        public virtual ICollection<Att_Grade> Att_Grade { get; set; }
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Cat_GradeAttendance Cat_GradeAttendance { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType1 { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType2 { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType3 { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType4 { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType5 { get; set; }
        public virtual ICollection<Cat_JobTitle> Cat_JobTitle { get; set; }
        public virtual ICollection<Cat_LateEarlyRule> Cat_LateEarlyRule { get; set; }
        public virtual ICollection<Cat_PayrollElement> Cat_PayrollElement { get; set; }
        public virtual ICollection<Sal_Grade> Sal_Grade { get; set; }
    }
}
