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
    public class Att_PregnancySearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_Type)]
        [MaxLength(50)]
        public string Type { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    
    public class Att_PregnancyModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_DateStart)]
        [DataType(DataType.Date)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_DateEnd)]
        [DataType(DataType.Date)]
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_Comment)]
        [MaxLength(1000)]
        public string Comment { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_Type)]
        [MaxLength(50)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public List<Guid> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_ChildName)]
        [MaxLength(150)]
        public string ChildName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_DateOfBirth)]
        public DateTime? ChildBirthday { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Comment = "Comment";
            public const string Type = "Type";
            public const string ChildName = "ChildName";
            public const string ChildBirthday = "ChildBirthday";

            public const string CodeEmp = "CodeEmp";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureID = "OrgStructureID";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";
        }
    }
    public class Att_ReportPregnancySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_Type)]
        [MaxLength(50)]
        public string Type { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
