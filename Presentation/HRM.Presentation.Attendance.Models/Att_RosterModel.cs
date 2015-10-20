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
    public class Att_RosterModel : BaseViewModel
    {
        public string TypeTranslate { get; set; }
        public string StatusTranslate { get; set; }
        public string ProfileIds { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateStart)]
        public DateTime DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateEnd)]
        public DateTime DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_Type)]
        [MaxLength(50)]
        public string Type { get; set; }

        public List<Guid?> strOrgIds { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_MonShiftID)]
        public Guid? MonShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_MonShiftName)]
        [MaxLength(150)]
        public string MonShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_MonShift2ID)]
        public Guid? MonShift2ID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_TueShiftID)]
        public Guid? TueShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_TueShiftName)]
        [MaxLength(150)]
        public string TueShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_TueShift2ID)]
        public Guid? TueShift2ID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_WedShiftID)]
        public Guid? WedShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_WedShiftName)]
        [MaxLength(150)]
        public string WedShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_WedShift2ID)]
        public Guid? WedShift2ID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_ThuShiftID)]
        public Guid? ThuShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_ThuShiftName)]
        [MaxLength(150)]
        public string ThuShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_ThuShift2ID)]
        public Guid? ThuShift2ID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_FriShiftID)]
        public Guid? FriShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_FriShiftName)]
        [MaxLength(150)]
        public string FriShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_FriShift2ID)]
        public Guid? FriShift2ID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_SatShiftID)]
        public Guid? SatShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_SatShiftName)]
        [MaxLength(150)]
        public string SatShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_SatShift2ID)]
        public Guid? SatShift2ID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_SunShiftID)]
        public Guid? SunShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_SunShiftName)]
        [MaxLength(150)]
        public string SunShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_SunShift2ID)]
        public Guid? SunShift2ID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_OrgStructureID)]
        public Guid? OrgStructureID { get; set; }
        public string OrgStructureIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_UserApproveID)]
        public Guid? UserApproveID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_UserApproveID)]
        [MaxLength(150)]
        public string UserApproveIDName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_UserApproveID2)]
        public Guid? UserApproveID2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_UserApproveID2)]
        [MaxLength(150)]
        public string UserApprove2IDName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_Comment)]
        [MaxLength(1000)]
        public string Comment { get; set; }

        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_RosterGroupName)]
        public string RosterGroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        public Guid CutOffDurationID { get; set; }

        private bool _isChecked = false;
        public bool IsChecked { get { return false; } set { _isChecked = value; } }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_WorkPlace)]
       

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureName { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Roster_ID
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
            public const string IsChecked = "IsChecked";
            public const string Id = "Id";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string RosterGroupName = "RosterGroupName";
            public const string Type = "Type";
            public const string MonShiftName = "MonShiftName";
            public const string TueShiftName = "TueShiftName";
            public const string WedShiftName = "WedShiftName";
            public const string ThuShiftName = "ThuShiftName";
            public const string FriShiftName = "FriShiftName";
            public const string SatShiftName = "SatShiftName";
            public const string SunShiftName = "SunShiftName";
            public const string Status = "Status";
            public const string UserApproveID = "UserApproveID";
            public const string UserApproveID2 = "UserApproveID2";
            public const string UserApproveIDName = "UserApproveIDName";
            public const string UserApprove2IDName = "UserApprove2IDName";
            public const string OrgStructureName = "OrgStructureName";
          
        }
    }
    public class Att_RosterSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateStart)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateEnd)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_WorkPlace)]
        public string WorkPlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        //public List<Guid?> OrgStructureID { get; set; }
        public string OrgStructureID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Att_RosterSearchV2Model
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateStart)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_DateEnd)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_WorkPlace)]
        public string WorkPlace { get; set; }

        public Guid? SysUserID { get; set; }


        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        //public List<Guid?> OrgStructureID { get; set; }
        public string OrgStructureID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    
}
