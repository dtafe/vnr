using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ProfileQualificationModel : BaseViewModel 
    {
        [DisplayName(ConstantDisplay.HRM_HR_Qualification_QualificationTypeID)]
        public Guid? QualificationTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_QualifiTypeID)]
        public Guid? QualifiTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_FieldOfTraining)]
        public string FieldOfTraining { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_CertificateName)]
        public string CertificateName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_GraduationDate)]
        public DateTime? GraduationDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_QualificationName)]
        public string QualificationName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_TrainingPlace)]
        public string TrainingPlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_TrainingAddress)]
        public string TrainingAddress { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_DateStart)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_DateFinish)]
        public DateTime? DateFinish { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_Rank)]
        public string Rank { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_SpecialLevelID)]
        public Guid? SpecialLevelID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_Notes)]
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_ProfileID)]
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_QualificationLevel)]
        public string NameEntityName { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Qualification_ID
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

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string QualificationTypeID = "QualificationTypeID";
            public const string QualifiTypeID = "QualifiTypeID";
            public const string FieldOfTraining = "FieldOfTraining";
            public const string CertificateName = "CertificateName";
            public const string GraduationDate = "GraduationDate";
            public const string QualificationName = "QualificationName";
            public const string TrainingPlace = "TrainingPlace";
            public const string TrainingAddress = "TrainingAddress";
            public const string DateStart = "DateStart";
            public const string DateFinish = "DateFinish";
            public const string Rank = "Rank";
            public const string SpecialLevelID = "SpecialLevelID";
            public const string Comment = "Comment";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string PositionName = "PositionName";
            public const string PositionID = "PositionID";
            public const string JobTitleName = "JobTitleName";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureID = "OrgStructureID";
            public const string NameEntityName = "NameEntityName";
            public const string Qualification_ID = "Qualification_ID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
    public class Hre_ProfileQualificationModelSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Qualification_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_QualificationName)]
        public string QualificationName { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";
            public const string OrgStructureID = "OrgStructureID";
            public const string QualificationName = "QualificationName";
        }
    }
}
