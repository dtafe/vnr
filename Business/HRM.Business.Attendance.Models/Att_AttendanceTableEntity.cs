using HRM.Business.BaseModel;
using HRM.Business.Category.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class Att_AttendanceTableEntity : HRMBaseModel
    {
        public Nullable<double> PaidLeaveDays { get; set; }

        public Nullable<System.DateTime> MonthYear { get; set; }
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        public System.Guid ProfileID { get; set; }
        public string Status { get; set; }
        public double StdWorkDayCount { get; set; }
        public double RealWorkDayCount { get; set; }
        public double PaidWorkDayCount { get; set; }
        public double HourPerDay { get; set; }
        public double NightShiftHours { get; set; }
        public double AnlDayTaken { get; set; }
        public double AnlDayAvailable { get; set; }

        public string LeaveDay1Name { get; set; }
        public string LeaveDay2Name { get; set; }
        public string LeaveDay3Name { get; set; }
        public string LeaveDay4Name { get; set; }
        public string Overtime1Name { get; set; }
        public string Overtime2Name { get; set; }
        public string Overtime3Name { get; set; }
        public string Overtime4Name { get; set; }
        public string Overtime5Name { get; set; }
        public string Overtime6Name { get; set; }

        public Nullable<System.Guid> LeaveDay1Type { get; set; }
        public double LeaveDay1Hours { get; set; }
        public Nullable<System.Guid> LeaveDay2Type { get; set; }
        public double LeaveDay2Hours { get; set; }
        public Nullable<System.Guid> LeaveDay3Type { get; set; }
        public double LeaveDay3Hours { get; set; }
        public Nullable<System.Guid> LeaveDay4Type { get; set; }
        public double LeaveDay4Hours { get; set; }
        public Nullable<System.Guid> Overtime1Type { get; set; }
        public double Overtime1Hours { get; set; }
        public Nullable<System.Guid> Overtime2Type { get; set; }
        public double Overtime2Hours { get; set; }
        public Nullable<System.Guid> Overtime3Type { get; set; }
        public double Overtime3Hours { get; set; }
        public Nullable<System.Guid> Overtime4Type { get; set; }
        public double Overtime4Hours { get; set; }
        public Nullable<System.Guid> Overtime5Type { get; set; }
        public double Overtime5Hours { get; set; }
        public Nullable<System.Guid> Overtime6Type { get; set; }
        public double Overtime6Hours { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public double LateEarlyDeductionHours { get; set; }
        public string UserRegister { get; set; }
        public Nullable<System.DateTime> DateRegister { get; set; }
        public string UserApprove { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
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
        public Nullable<double> StdWorkDayOTCount { get; set; }
        public Nullable<double> TotalRealWorkDayCount { get; set; }
        public Nullable<double> TotalPaidWorkDayCount { get; set; }
        public Nullable<System.Guid> LeaveWorkDay1Type { get; set; }
        public Nullable<double> LeaveWorkDay1Hour { get; set; }
        public Nullable<System.Guid> LeaveWorkDay2Type { get; set; }
        public Nullable<double> LeaveWorkDay2Hour { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public Nullable<System.Guid> EmployeeTypeID { get; set; }
        public Nullable<System.Guid> PayrollGroupID { get; set; }
        public Nullable<System.Guid> CostCentreID { get; set; }
        public Nullable<double> AnlDayReset { get; set; }
        public string Note { get; set; }
        public Nullable<double> OTNightShiftHours { get; set; }
        public Nullable<double> TotalAnlDayAvailable { get; set; }
        public Nullable<double> TotalSickDayAvailable { get; set; }
        public Nullable<double> SickDayTaken { get; set; }
        public Nullable<double> SickDayAvailable { get; set; }
        public Nullable<double> SickDayAdjacent { get; set; }
        public Nullable<double> AnlDayAdjacent { get; set; }
        public Nullable<double> UnPaidLeave { get; set; }
        public Nullable<double> LateEarlyLeastCount { get; set; }
        public Nullable<double> LateEarlyGreaterCount { get; set; }
        public Nullable<double> LateEarlyTotal { get; set; }
        public Nullable<double> CardMissingCount { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> COBeginPeriod { get; set; }
        public Nullable<double> COEndPeriod { get; set; }
        public Nullable<double> LeaveDay1Days { get; set; }
        public Nullable<double> LeaveDay2Days { get; set; }
        public Nullable<double> LeaveDay3Days { get; set; }
        public Nullable<double> LeaveDay4Days { get; set; }
        public Nullable<double> LeaveWorkDay1Days { get; set; }
        public Nullable<double> LeaveWorkDay2Days { get; set; }

        public string HDTJobType1 { get; set; }
        public string HDTJobType2 { get; set; }
        public string HDTJobType3 { get; set; }
        public Nullable<int> HDTJobDayCount1 { get; set; }
        public Nullable<int> HDTJobDayCount2 { get; set; }
        public Nullable<int> HDTJobDayCount3 { get; set; }

        public string CutOffDurationName { get; set; }
        public string ProfileName { get; set; }

        public virtual ICollection<Att_AttendanceTableItemEntity> Att_AttendanceTableItem { get; set; }
        public virtual Att_CutOffDurationEntity Att_CutOffDuration { get; set; }
        public virtual Cat_LeaveDayTypeEntity Cat_LeaveDayType { get; set; }
        public virtual Cat_LeaveDayTypeEntity Cat_LeaveDayType1 { get; set; }
        public virtual Cat_LeaveDayTypeEntity Cat_LeaveDayType2 { get; set; }
        public virtual Cat_LeaveDayTypeEntity Cat_LeaveDayType3 { get; set; }
        public virtual Cat_LeaveDayTypeEntity Cat_LeaveDayType4 { get; set; }
        public virtual Cat_LeaveDayTypeEntity Cat_LeaveDayType5 { get; set; }
        public virtual Cat_OvertimeTypeEntity Cat_OvertimeType { get; set; }
        public virtual Cat_OvertimeTypeEntity Cat_OvertimeType1 { get; set; }
        public virtual Cat_OvertimeTypeEntity Cat_OvertimeType2 { get; set; }
        public virtual Cat_OvertimeTypeEntity Cat_OvertimeType3 { get; set; }
        public virtual Cat_OvertimeTypeEntity Cat_OvertimeType4 { get; set; }
        public virtual Cat_OvertimeTypeEntity Cat_OvertimeType5 { get; set; }

        public partial class FieldNames
        {
            public const string UnPaidLeave = "UnPaidLeave";
            public const string Allowance1 = "Allowance1";
            public const string Allowance2 = "Allowance2";
            public const string COBeginPeriod = "COBeginPeriod";
            public const string COEndPeriod = "COEndPeriod";
        }
    }
}
