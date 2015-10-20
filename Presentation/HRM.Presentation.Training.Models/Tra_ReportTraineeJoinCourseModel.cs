using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_ReportTraineeJoinCourseModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public string CourseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ClassName)]
        public string ClassName { get; set; }
        public string TopicName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public string TraineeName { get; set; }
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SalaryClassID)]
        public string RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public string CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_DateInterview)]
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string IDNo { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public string Note { get; set; }
        public Guid ExportId { get; set; }
        public bool IsAllPage { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Certificate_AllCourse)]
        public Nullable<bool> IsAllCourse { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Certificate_DateSeniory)]
        public DateTime? DateSeniory { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Certificate_Seniory)]

        public double? Seniority { get; set; }
        [DisplayName(ConstantDisplay.HRM_Training_TrainingRequirements)]
        public string RequirementTrainID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_RequirementTrainNames)]
        public string RequirementTrainName { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string TraineeName = "TraineeName";
            public const string OrgStructureName = "OrgStructureName";
            public const string CourseName = "CourseName";
            public const string IDNo = "IDNo";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string ClassName = "ClassName";
            public const string Note = "Note";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string Seniority = "Seniority";
            public const string TraineeCertificateID = "TraineeCertificateID";
            public const string RequirementTrainName = "RequirementTrainName";
        }
    }

 

}
