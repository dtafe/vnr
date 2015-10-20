using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel;
using HRM.Presentation.Service;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_InsuranceLeaveDetailModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public DateTime? MonthYear { get; set; }

        public DateTime? MonthStart { get; set; }

        public DateTime? MonthEnd { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public double Year { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public DateTime? AnualLeaveAvailable { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public DateTime? SickLeaveAvailable { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public DateTime? InsuranceLeaveAvailable { get; set; }

        [MaxLength(150)]
        public string ProfileName { get; set; }

        [MaxLength(50)]
        public string LeaveType { get; set; }
        public string UdLeaveType { get; set; }
        public double Month1 { get; set; }
        public double Month2 { get; set; }
        public double Month3 { get; set; }
        public double Month4 { get; set; }
        public double Month5 { get; set; }
        public double Month6 { get; set; }
        public double Month7 { get; set; }
        public double Month8 { get; set; }
        public double Month9 { get; set; }
        public double Month10 { get; set; }
        public double Month11 { get; set; }
        public double Month12 { get; set; }

        public string ProfileStatus { get; set; }
        
        public partial class FieldNames
        {
            public static string ID = "ID";
            public static string ProfileName = "ProfileName";
            public static string ProfileID = "ProfileID";
            public static string MonthYear = "MonthYear";
            public const string MonthStart = "MonthStart";
            public const string MonthEnd = "MonthEnd";
            public static string Year = "Year";
            public const string LeaveType = "LeaveType";
            public static string AnualLeaveAvailable = "AnualLeaveAvailable";
            public static string SickLeaveAvailable = "SickLeaveAvailable";
            public static string InsuranceLeaveAvailable = "InsuranceLeaveAvailable";
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
            public const string UdLeaveType = "UdLeaveType";
        }
    }


    public class Att_InsuranceLeaveDetailSearchModel
    {
        public double Year { get; set; }
        public string OrgStructureID { get; set; }
        public string ProfileStatus { get; set; }
        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string MonthYear = "MonthYear";
            public const string OrgStructureID = "OrgStructureID";
           
        }
    }
}
