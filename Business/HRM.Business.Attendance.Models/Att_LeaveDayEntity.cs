using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_LeaveDayEntity : HRMBaseModel
    {
        public string UserRegister { get; set; }
        public string Host { get; set; }
        public int TotalRow { get; set; }
        public Guid ProfileID { get; set; }
        public string OrgStructureName { get; set; }
        public Guid LeaveDayTypeID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int? TotalLeave { get; set; }
        public string Status { get; set; }
        public string StatusTranslate { get; set; }
        public string Comment { get; set; }
        public DateTime? DateRegister { get; set; }
        public Guid? UserApproveID { get; set; }
        public string UserApprove { get; set; }
        public DateTime? DateApprove { get; set; }
        public Guid? UserRejectID { get; set; }
        public DateTime? DateReject { get; set; }
        public double Duration { get; set; }
        public string ProfileName { get; set; }
        public string LeaveDayTypeName { get; set; }
        public string CodeEmp { get; set; }
        public string DurationType { get; set; }
        public double? DurationHour { get; set; }
        public double? TotalDuration { get; set; }
        //public string StatusLeaveDay { get; set; }
        public Nullable<System.DateTime> DateOvertimeOff { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }
        public string LeaveDayTypeCode { get; set; }


        #region [Hien.Nguyen]
       
        public Guid? InsuranceRecordID { get; set; }
    
        public string UserApproveName { get; set; }

        public string UserApproveName2 { get; set; }
      
        public Guid? UserApproveID2 { get; set; }

        public string CommentApprove { get; set; }
    
        public string DeclineReason { get; set; }


        #region Hien.Nguyen
        public double? LeaveDays { get; set; }
        public double? LeaveHours { get; set; } 
        #endregion


        #endregion

        private Guid _id = Guid.Empty;
        public Guid LeaveDay_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string CodeBranch = "CodeBranch";
            public const string CodeOrg = "CodeOrg";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";
            public const string CodeJobtitle = "CodeJobtitle";
            public const string CodePosition = "CodePosition";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string Paid = "Paid";
            public const string Date = "Date";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string udInTime = "udInTime";
            public const string udOutTime = "udOutTime";
            public const string Cat_Shift = "Cat_Shift";
            public const string Status = "Status";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";

            public const string LeaveDays = "LeaveDays";
            public const string LeaveHours = "LeaveHours";
            public const string DateOvertimeOff = "DateOvertimeOff";
            
           
        }
    }
    public class Att_LeaveDayInfo
    {
        public Guid ID { get; set; }
        public Guid ProfileID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Guid LeaveDayTypeID{ get; set; }
        public double? TotalDuration { get; set; }
        public double Duration { get; set; }
        public double? LeaveHours { get; set; }
        public string DurationType { get; set; }
        public double? LeaveDays { get; set; }
        
            
        
    }
}
