using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_AnnualLeaveDetailEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<decimal> Year { get; set; }
        public Nullable<System.DateTime> MonthStart { get; set; }
        public Nullable<System.DateTime> MonthEnd { get; set; }

        public DateTime? MonthYear { get; set; }
        public DateTime? AnualLeaveAvailable { get; set; }
        public DateTime? SickLeaveAvailable { get; set; }
        public DateTime? InsuranceLeaveAvailable { get; set; }

        public Nullable<System.DateTime> DateHire { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }
        public string ProfileName { get; set; }

        public string Type { get; set; }
        public string UdLeaveType { get; set; }
        public Nullable<double> Month1 { get; set; }
        public Nullable<double> Month2 { get; set; }
        public Nullable<double> Month3 { get; set; }
        public Nullable<double> Month4 { get; set; }
        public Nullable<double> Month5 { get; set; }
        public Nullable<double> Month6 { get; set; }
        public Nullable<double> Month7 { get; set; }
        public Nullable<double> Month8 { get; set; }
        public Nullable<double> Month9 { get; set; }
        public Nullable<double> Month10 { get; set; }
        public Nullable<double> Month11 { get; set; }
        public Nullable<double> Month12 { get; set; }
        public string ProfileStatus { get; set; }

        
        public partial class FieldNames
        {
            public static string ID = "ID";
            public static string ProfileID = "ProfileID";
            public static string ProfileName = "ProfileName";
            public static string MonthYear = "MonthYear";
            public static string Year = "Year";
            public static string AnualLeaveAvailable = "AnualLeaveAvailable";
            public static string SickLeaveAvailable = "SickLeaveAvailable";
            public static string InsuranceLeaveAvailable = "InsuranceLeaveAvailable";
            public const string MonthStart = "MonthStart";
            public const string MonthEnd = "MonthEnd";
            public const string Type = "Type";
            public const string Month1 = "Month1";
            public const string Month2 = "Month2";
            public const string Month3 = "Month3";
            public const string Month4 = "Month4";
            public const string Month5 = "Month5";
            public const string Month6 = "Month6";
            public const string Month7 = "Month7";
            public const string Month8 = "Month8";
            public const string Month9 = "Month9";
            public const string Month10 = "Month10";
            public const string Month11 = "Month11";
            public const string Month12 = "Month12";
        }




    }

    public class ParamGetConfigANL
    {
        public int monthBeginYear { get; set; }
        public int dayBeginFullMonth { get; set; }
        public int seniorMonth { get; set; }
        public int dayPerMonth { get; set; }
        public double anlRoundUp { get; set; }
        public string typeProfileBegin { get; set; }
        public int maxInMonthToGetAct { get; set; }
        public double anlFullYear { get; set; }
        public double anlSeniorMoreThanNormal { get; set; }
        public double anlHDT4MoreThanNormal { get; set; }
        public double anlHDT5MoreThanNormal { get; set; }
        public List<string> lstCodeLeaveNonANL { get; set; } //Ds mã code những loại nghỉ không đuợc hưởng ngày phép
        public int monthInYearSenior { get; set; } //Ds mã code những loại nghỉ không đuợc hưởng ngày phép
        public int monthRoundUp { get; set; } //Số tháng phép làm tròn lên 
    }
}
