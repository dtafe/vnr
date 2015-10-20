using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

using HRM.Infrastructure.Utilities;
using System.ComponentModel;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportGeneralMonthlyAttendanceEntity : HRMBaseModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }
        public string TeamCode { get; set; }
        public string SectionCode { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string CurrentDept { get; set; }
        public string CurrentDept1 { get; set; }
        public string CurrentDept2 { get; set; }
        public string Department { get; set; }
        public string DeptCode { get; set; }
        public string OrderExcel { get; set; }
        public string SectCode { get; set; }
        public string LaborType { get; set; }
        public DateTime? DateHire { get; set; }
        public DateTime? DateQuit { get; set; }
        public string CodeOrg { get; set; }
        public DateTime? DateExport { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string JobtitleCode { get; set; }
        public string PositionCode { get; set; }
        public string EmployeeTypeName { get; set; }
        public string WorkPlaceName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public string Cat_Section { get; set; }
        public double UsableLeave { get; set; }
        public double CurrentMonth { get; set; }
        public double YearToDate { get; set; }
        public double SickLeave { get; set; }
        public double SickCurrentMonth { get; set; }
        public double SickYearToDate { get; set; }
        public double CountLateLess2H { get; set; }
        public double CountLateMore2H { get; set; }
        public double SumMuteLateEarly { get; set; }
        public double CountLateEarly { get; set; }
        public double SumCODayOT { get; set; }
        public double CountCOLeavday { get; set; }
        public double UnPaidLeave { get; set; }
        public double Allowance1 { get; set; }
        public double Allowance2 { get; set; }
        public double COBeginPeriod { get; set; }
        public double COEndPeriod { get; set; }

        
        public double StandardWorkDays { get; set; }
        public double StandardWorkDaysFormula { get; set; }
        public double RealWorkingDays { get; set; }
        public double LateEarlyHours   { get; set; }

        public string Count { get; set; }
        public string Comment { get; set; }
   
        
        public string PaidWorkDayCount { get; set; }
        public int Hour { get; set; }
        public double LateEarlyDeductionHours { get; set; }
        public double LateEarlyDeductionDays { get; set; }
        public string WorkingPlace { get; set; }
        public double AnlDayAvailable { get; set; }
        public double WorkHours { get; set; }
        public double WorkPaidHours { get; set; }
        public double LateEarlyMinutes { get; set; }
        public double LateInMinutes { get; set; }
        public double EarlyOutMinutes { get; set; }
        public double NightShiftHours { get; set; }
        public double TotalNightShiftHours { get; set; }
        public double WorkPaidHourOver8 { get; set; }
        public double NightHourOver6 { get; set; }
        public double ANL { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Cat_WorkPlace { get; set; }
        public string Cat_OrgStructure { get; set; }
        public string UserExport { get; set; }

        public string Data { get; set; }

        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string Data3 { get; set; }
        public string Data4 { get; set; }
        public string Data5 { get; set; }
        public string Data6 { get; set; }
        public string Data7 { get; set; }
        public string Data8 { get; set; }
        public string Data9 { get; set; }
        public string Data10 { get; set; }
        public string Data11 { get; set; }
        public string Data12 { get; set; }
        public string Data13 { get; set; }
        public string Data14 { get; set; }
        public string Data15 { get; set; }
        public string Data16 { get; set; }
        public string Data17 { get; set; }
        public string Data18 { get; set; }
        public string Data19 { get; set; }
        public string Data20 { get; set; }
        public string Data21 { get; set; }
        public string Data22 { get; set; }
        public string Data23 { get; set; }
        public string Data24 { get; set; }
        public string Data25 { get; set; }
        public string Data26 { get; set; }
        public string Data27{ get; set; }
        public string Data28 { get; set; }
        public string Data29 { get; set; }
        public string Data30{ get; set; }
        public string Data31 { get; set; }

        public partial class FieldNames
        {
            public const string EmployeeTypeName = "EmployeeTypeName";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string ANL = "ANL";
            public const string WorkPaidHourOver8 = "WorkPaidHourOver8";
            public const string NightHourOver6 = "NightHourOver6";
            public const string PaidWorkDayCount = "PaidWorkDayCount";
            public const string Hour = "Hour";
            public const string LateEarlyDeductionHours = "LateEarlyDeductionHours";
            public const string LateEarlyDeductionDays = "LateEarlyDeductionDays";
            public const string WorkingPlace = "WorkingPlace";
            public const string AnlDayAvailable = "AnlDayAvailable";
            public const string WorkHours = "WorkHours";
            public const string WorkPaidHours = "WorkPaidHours";
            public const string LateEarlyMinutes = "LateEarlyMinutes";
            public const string LateInMinutes = "LateInMinutes";
            public const string EarlyOutMinutes = "EarlyOutMinutes";
            public const string NightShiftHours = "NightShiftHours";
            public const string TotalNightShiftHours = "TotalNightShiftHours";
            public const string CodeOrg = "CodeOrg";
            public const string Count = "Count";
            public const string Comment = "Comment";
            public const string StandardWorkDays = "StandardWorkDays";
            public const string StandardWorkDaysFormula = "StandardWorkDaysFormula";
            public const string RealWorkingDays = "RealWorkingDays";
            public const string LateEarlyHours   = "LateEarlyHours  ";

            public const string CurrentDept = "CurrentDept";
            public const string CurrentDept1 = "CurrentDept1";
            public const string CurrentDept2 = "CurrentDept2";
            public const string Department = "Department";
            public const string DeptCode = "DeptCode";
            public const string OrderExcel = "OrderExcel";

            public const string SectCode = "SectCode";
            public const string LaborType = "LaborType";
            public const string DateHire = "DateHire";
            public const string DateQuit = "DateQuit";

            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string BranchCode = "BranchCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string JobtitleCode = "JobtitleCode";
            public const string PositionCode = "PositionCode";

            public const string CurrentMonth = "CurrentMonth";
            public const string UsableLeave = "UsableLeave";
            public const string YearToDate = "YearToDate";
            public const string SickLeave = "SickLeave";
            public const string SickYearToDate = "SickYearToDate";
            public const string CountLateLess2H = "CountLateLess2H";
            public const string CountLateMore2H = "CountLateMore2H";
            public const string SumMuteLateEarly = "SumMuteLateEarly";
            public const string CountLateEarly = "CountLateEarly";
            public const string SumCODayOT = "SumCODayOT";
            public const string CountCOLeavday = "CountCOLeavday";
            public const string SickCurrentMonth = "SickCurrentMonth";
            public const string Cat_Section = "Cat_Section";
            public const string Data = "Data";
            public const string Cat_WorkPlace = "Cat_WorkPlace";
            public const string Cat_OrgStructure = "Cat_OrgStructure";

            public const string Data1  = "Data1 ";
            public const string Data2  = "Data2 ";
            public const string Data3  = "Data3 ";
            public const string Data4  = "Data4 ";
            public const string Data5  = "Data5 ";
            public const string Data6  = "Data6 ";
            public const string Data7  = "Data7 ";
            public const string Data8  = "Data8 ";
            public const string Data9  = "Data9 ";
            public const string Data10 = "Data10";
            public const string Data11 = "Data11";
            public const string Data12 = "Data12";
            public const string Data13 = "Data13";
            public const string Data14 = "Data14";
            public const string Data15 = "Data15";
            public const string Data16 = "Data16";
            public const string Data17 = "Data17";
            public const string Data18 = "Data18";
            public const string Data19 = "Data19";
            public const string Data20 = "Data20";
            public const string Data21 = "Data21";
            public const string Data22 = "Data22";
            public const string Data23 = "Data23";
            public const string Data24 = "Data24";
            public const string Data25 = "Data25";
            public const string Data26 = "Data26";
            public const string Data27 = "Data27";
            public const string Data28 = "Data28";
            public const string Data29 = "Data29";
            public const string Data30 = "Data30";
            public const string Data31 = "Data31";
        }                         
    }

}
