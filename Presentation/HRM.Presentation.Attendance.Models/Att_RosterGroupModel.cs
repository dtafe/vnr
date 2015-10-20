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
    public class Att_RosterGroupSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_RosterGroupName)]
        [MaxLength(150)]
        public string RosterGroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_DateStart)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_DateEnd)]
        public DateTime? DateEnd { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Att_RosterGroupModel : BaseViewModel
    {
        public string ProfileIds { get; set; }


        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_RosterGroupName)]
        [MaxLength(150)]
        public string RosterGroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_DateStart)]
        [DataType(DataType.Date)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_DateEnd)]
        [DataType(DataType.Date)]
        public DateTime? DateEnd { get; set; }

        public DateTime? Month { get; set; }


        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_MonShiftID)]
        public Guid? MonShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_TueShiftID)]
        public Guid? TueShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_WedShiftID)]
        public Guid? WedShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_ThuShiftID)]
        public Guid? ThuShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_FriShiftID)]
        public Guid? FriShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_SatShiftID)]
        public Guid? SatShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_SunShiftID)]
        public Guid? SunShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_UserApprove1ID)]
        public Guid? UserApproveID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_UserApprove2ID)]
        public Guid? UserApproceID2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_RosterGroup_Comment)]
        [MaxLength(1000)]
        public string Note { get; set; }

        [MaxLength(150)]
        public string MonShiftName { get; set; }
        [MaxLength(150)]
        public string TueShiftName { get; set; }
        [MaxLength(150)]
        public string WedShiftName { get; set; }
        [MaxLength(150)]
        public string ThuShiftName { get; set; }
        [MaxLength(150)]
        public string FriShiftName { get; set; }
        [MaxLength(150)]
        public string SatShiftName { get; set; }
        [MaxLength(150)]
        public string SunShiftName { get; set; }
        [MaxLength(150)]
        public string UserApproveName2 { get; set; }
        [MaxLength(150)]
        public string UserApproveName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string RosterGroupName = "RosterGroupName";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Type = "Type";
            public const string Status = "Status";
            public const string MonShiftID = "MonShiftID";
            public const string TueShiftID = "TueShiftID";
            public const string WedShiftID = "WedShiftID";
            public const string ThuShiftID = "ThuShiftID";
            public const string FriShiftID = "FriShiftID";
            public const string SatShiftID = "SatShiftID";
            public const string SunShiftID = "SunShiftID";
            public const string UserApproveID = "UserApproveID";
            public const string UserApproceID2 = "UserApproceID2";
            public const string Note = "Note";

            public const string MonShiftName = "MonShiftName";
            public const string TueShiftName = "TueShiftName";
            public const string WedShiftName = "WedShiftName";
            public const string ThuShiftName = "ThuShiftName";
            public const string FriShiftName = "FriShiftName";
            public const string SatShiftName = "SatShiftName";
            public const string SunShiftName = "SunShiftName";
            public const string UserApproveName2 = "UserApproveName2";
            public const string UserApproveName = "UserApproveName";
            public const string Month = "Month";

        }
    }
}
