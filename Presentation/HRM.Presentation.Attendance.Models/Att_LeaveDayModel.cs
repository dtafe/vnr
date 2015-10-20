using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_LeavedaySearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_Status)]
        [MaxLength(50)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_LeaveDayTypeID)]
        public Guid? LeaveDayTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateEnd)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        public Guid? SysUserID { get; set; }
        public Guid ExportId { get; set; }
        //public string UserLoginName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Att_ComputeLeaveDayModel
    {
        public DateTime HoursFrom { get; set; }
        public DateTime HoursTo { get; set; }
        public double LeaveHours { get; set; }
        public Nullable<double> LeaveDays { get; set; }
        public string Messages { get; set; }
    }
    public class Att_LeaveDayModel : BaseViewModel
    {
        public string Host { get; set; }
        public string StatusTranslate { get; set; }
        public string ProfileIds { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_LeaveDayTypeID)]
        public Guid LeaveDayTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateStart)]
        public DateTime DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateEnd)]
        public DateTime DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_TotalLeave)]
        public int? TotalLeave { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_TotalLeave)]
        public double? TotalDuration { get; set; }
        public bool IsPortal { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_Comment)]
        [MaxLength(1000)]
        public string Comment { get; set; }

        public Guid CutOffDurationID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_UserRegister)]
        [MaxLength(150)]
        public string UserRegister { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateRegister)]
        public DateTime? DateRegister { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_UserApprove1ID)]
        public Guid? UserApproveID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_UserApprove1ID)]
        [MaxLength(150)]
        public string UserApprove { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateApprove)]
        public DateTime? DateApprove { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_UserRejectID)]
        public Guid? UserRejectID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateReject)]
        public DateTime? DateReject { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_Duration)]
        public double Duration { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_LeaveDayTypeName)]
        [MaxLength(50)]
        public string LeaveDayTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        public DateTime? HoursFrom { get; set; }
        public DateTime? HoursTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }
        public Nullable<System.DateTime> DateOvertimeOff { get; set; }




        private bool _isChecked = false;
        public bool IsChecked { get { return false; } set { _isChecked = value; } }


        public List<Guid> lstLeaveIDs { get; set; }
        /// <summary>
        /// [Tam.Le] - 9.8.2014 - Thêm field BC Nghỉ Chờ Duyệt
        /// </summary>
        public string CodeBranch { get; set; }
        public string CodeOrg { get; set; }
        public string CodeTeam { get; set; }
        public string CodeSection { get; set; }
        public string CodeJobtitle { get; set; }
        public string CodePosition { get; set; }
        public string BranchName { get; set; }
        public string OrgName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string Paid { get; set; }
        public DateTime Date { get; set; }
        public string udInTime { get; set; }
        public string udOutTime { get; set; }
        public string Cat_Shift { get; set; }

        public DateTime DateExport { get; set; }
        public string UserExport { get; set; }


        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }



        #region [Hien.Nguyen]
        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_DurationType)]
        public string DurationType { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_InsuranceRecord)]
        public Guid? InsuranceRecordID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_UserApprove1ID)]
         public string UserApproveName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_UserApprove2ID)]
         public string UserApproveName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_UserApprove2ID)]
        public Guid? UserApproveID2 { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_CommentApprove)]
        public string CommentApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DeclineReason)]
         public string DeclineReason { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_TotalLeave)]
        public double? LeaveDays { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_Duration)]
        public double? LeaveHours { get; set; }

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
        public string strOrgStructureIDs { get; set; }
        public string ProfileIDsExclude { get; set; }

        public partial class FieldNames
        {
            public const string IsChecked = "IsChecked";
            public const string Id = "Id";
            public const string ProfileID = "ProfileID";
            public const string OrgStructureName = "OrgStructureName";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string LeaveDayTypeID = "LeaveDayTypeID";
            public const string LeaveDayTypeName = "LeaveDayTypeName";
            public const string UserApprove = "UserApprove";
            public const string UserApproveID = "UserApproveID";
            public const string UserApproveName = "UserApproveName";
            public const string UserApproveID2 = "UserApproveID2";
            public const string UserApproveName2 = "UserApproveName2";

            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string TotalLeave = "TotalLeave";
            public const string Status = "Status";
            public const string Comment = "Comment";
            public const string DeclineReason = "DeclineReason";

            //public const string UserRegister = "UserRegister";
            //public const string DateRegister = "DateRegister";
            //public const string UserApprove = "UserApprove";
            //public const string DateApprove = "DateApprove";
            //public const string UserRejectID = "UserRejectID";
            //public const string DateReject = "DateReject";
            public const string Duration = "Duration";
            public const string DurationType = "DurationType";
            public const string TotalDuration = "TotalDuration";
            public const string LeaveDays = "LeaveDays";
            public const string LeaveHours = "LeaveHours";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";

        }
    }

    public class Att_ChangeStatusLeavedayModel
    {
        public string SelectedIds { get; set; }
        public string Type { get; set; }
    }

    public class Att_PersonalSubmitLeavedaySearchModel
    {
        public string UserCreateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DateEnd)]
        public DateTime? DateEnd { get; set; }
        public Guid ExportId { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
