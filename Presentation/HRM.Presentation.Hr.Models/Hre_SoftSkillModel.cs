using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_SoftSkillModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_ProfileID)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_TrainingPlace)]
        [MaxLength(1000)]
        public string TrainingPlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_Certificate)]
        [MaxLength(150)]
        public string Certificate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_SoftSkillName)]
        [MaxLength(150)]
        public string SoftSkillName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_SoftSkillType)]
        [MaxLength(150)]
        public string SoftSkillType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_SoftSkillLevel)]
        [MaxLength(150)]
        public string SoftSkillLevel { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_TrainingFrom)]
        public DateTime? TrainingFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_TrainingTo)]
        public DateTime? TrainingTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string TrainingPlace = "TrainingPlace";
            public const string Certificate = "Certificate";
            public const string SoftSkillName = "SoftSkillName";
            public const string SoftSkillType = "SoftSkillType";
            public const string SoftSkillLevel = "SoftSkillLevel";
            public const string TrainingFrom = "TrainingFrom";
            public const string TrainingTo = "TrainingTo";
            public const string CodeEmp = "CodeEmp";
            public const string PositionName = "PositionName";
            public const string PositionID = "PositionID";
            public const string JobTitleName = "JobTitleName";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureID = "OrgStructureID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }

    public class Hre_SoftSkillSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_SoftSkill_ProfileID)]
        public string ProfileName { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
     
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureID = "OrgStructureID";
        }
    }
}
