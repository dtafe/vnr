using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_WorkdayEntity : HRMBaseModel
    {
        public Guid ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string CodeAttendance { get; set; }
        public string udLeavedayCode1 { get; set; }
        public string udLeavedayCode2 { get; set; }
        public string udLeavedayStatus1 { get; set; }
        public string udLeavedayStatus2 { get; set; }
        public string ShiftCode { get; set; }
        public DateTime WorkDate { get; set; } 
        public double? LateEarlyRoot { get; set; }
        public DateTime? InTimeRoot { get; set; }
        public DateTime? OutTimeRoot { get; set; }
        public DateTime? FirstInTime { get; set; }
        public DateTime? FirstOutTime { get; set; }
        public DateTime? LastInTime { get; set; }
        public DateTime? LastOutTime { get; set; }
        public Guid? ShiftID { get; set; }
        public double WorkDuration { get; set; }
        public int? LateEarlyAdjust { get; set; }
        public string LateEarlyReason { get; set; }
        public string LateEarlyError { get; set; }
        public double? LateDuration { get; set; }
        public double? EarlyDuration { get; set; }
        public string ProfileName { get; set; }
        public string ShiftName { get; set; }
        public DateTime? InTime1 { get; set; }
        public DateTime? OutTime1 { get; set; }
        public DateTime? InTime2 { get; set; }
        public DateTime? OutTime2 { get; set; }
        public DateTime? InTime3 { get; set; }
        public DateTime? OutTime3 { get; set; }
        public DateTime? InTime4 { get; set; }
        public DateTime? OutTime4 { get; set; }
        public string SrcType { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string TAMScanReasonMissName { get; set; }
        public string ShiftApproveName { get; set; }
        public string ShiftActualName { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public DateTime? ShiftOutTime { get; set; }
        public double? CoOut { get; set; }

        public string Note { get; set; }
        public Nullable<double> LateDuration1 { get; set; }
        public Nullable<double> EarlyDuration1 { get; set; }
        public Nullable<double> LateDuration2 { get; set; }
        public Nullable<double> EarlyDuration2 { get; set; }
        public Nullable<double> LateDuration3 { get; set; }
        public Nullable<double> EarlyDuration3 { get; set; }
        public Nullable<double> LateDuration4 { get; set; }
        public Nullable<double> EarlyDuration4 { get; set; }
        public Nullable<double> LateEarlyDuration { get; set; }
        public Nullable<double> NightShiftDuration { get; set; }
        public string LateEarlyStatus { get; set; }
        public Nullable<System.Guid> ShiftActual { get; set; }
        public Nullable<System.Guid> ShiftApprove { get; set; }
        public Nullable<System.Guid> MissInOutReason { get; set; }
        public string WrongShiftStatus { get; set; }
        public string MissInOutStatus { get; set; }
        public Nullable<System.Guid> LeaveDayID1 { get; set; }
        public Nullable<System.Guid> LeaveDayID2 { get; set; }
        public string OrgStructureName { get; set; }

        public string TempTimeIn { get; set; }
        public string TempTimeOut { get; set; }
        public string TempInTimeRoot { get; set; }
        public string TempOutTimeRoot { get; set; }
    }

    public class Att_WorkDaySearchEntity
    {
        public string ProfileName { get; set; }

        public string CodeEmp { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public Guid? ShiftID { get; set; }

        public string OrgStructureID { get; set; }

        public string SrcType { get; set; }

        public string Type { get; set; }

        public int? LateDuration { get; set; }
        public int? EarlyDuration { get; set; }

    }

    public class WorkdayCustom
    {
        public Guid ID { get; set; }
        public Guid ProfileID { get; set; }
        public Guid? ShiftID { get; set; }
        public Guid? ShiftApprove { get; set; }
        public DateTime WorkDate { get; set; }
        public string Status { get; set; }
        public DateTime? InTime1 { get; set; }
        public DateTime? InTime2 { get; set; }
        public DateTime? InTime3 { get; set; }
        public DateTime? InTime4 { get; set; }
        public DateTime? OutTime1 { get; set; }
        public DateTime? OutTime2 { get; set; }
        public DateTime? OutTime3 { get; set; }
        public DateTime? OutTime4 { get; set; }
    }
}
