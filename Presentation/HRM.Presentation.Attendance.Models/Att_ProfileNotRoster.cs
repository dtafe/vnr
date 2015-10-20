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
    public class Att_SearchProfileNotRosterModel
    {
        public string OrgStructureID { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileNotRoster_isNotRoster)]
        public bool isNotRoster { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileNotRoster_isDuplicateRoster)]
        public bool isDuplicateRoster { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileNotRoster_isConstantRoster)]
        public bool? isConstantRoster { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Att_ProfileNotRosterModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileNotRoster_isNotRoster)]
        public bool isNotRoster { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileNotRoster_isDuplicateRoster)]
        public bool isDuplicateRoster { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileNotRoster_isConstantRoster)]
        public bool? isConstantRoster { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Att_ProfileModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeAttendance)]
        [MaxLength(50)]
        public string CodeAttendance { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        [DataType(DataType.Date)]
        public DateTime? DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateEndProbation)]
        [DataType(DataType.Date)]
        public DateTime? DateEndProbation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public Nullable<System.Guid> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        [MaxLength(50)]
        public string WorkingPlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        [MaxLength(50)]
        public string Gender { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Email)]
        [MaxLength(50)]
        public string Email { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        [MaxLength(20)]
        public string Cellphone { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        [DataType(DataType.Date)]
        public DateTime? DateQuit { get; set; }
        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string ProfileName = "ProfileName";
            public const string LastName = "LastName";
            public const string CodeEmp = "CodeEmp";
            public const string CodeAttendance = "CodeAttendance";
            public const string DateHire = "DateHire";
            public const string DateEndProbation = "DateEndProbation";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionID = "PositionID";
            public const string PositionName = "PositionName";
            public const string WorkingPlace = "WorkingPlace";
            public const string Gender = "Gender";
            public const string Email = "Email";
            public const string Cellphone = "Cellphone";
            public const string JobTitleID = "JobTitleID";
            public const string JobTitleName = "JobTitleName";
            public const string DateQuit = "DateQuit";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
}
