using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Recruitment.Models
{
    public class Rec_ReportInterviewModel : BaseViewModel
    {
        public string CodeCandidate { get; set; }
        public string CandidateName { get; set; }
        public string Gender { get; set; }
        public string GenderView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IdentifyNumber { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TAddress { get; set; }
        public Nullable<System.DateTime> YearGraduation { get; set; }
        public string GraduateSchool { get; set; }
        public string Specialisation { get; set; }
        public Nullable<System.DateTime> DateExam { get; set; }
        public string Description { get; set; }
        public Nullable<double> Score1 { get; set; }
        public Nullable<double> Score3 { get; set; }
        public string PositionName { get; set; }
        public partial class FieldNames
        {
            public const string PositionName = "PositionName";
            public const string CodeCandidate = "CodeCandidate";
            public const string CandidateName = "CandidateName";
            public const string Gender = "Gender";
            public const string IdentifyNumber = "IdentifyNumber";
            public const string GenderView = "GenderView";
            public const string DateOfBirth = "DateOfBirth";
            public const string Mobile = "Mobile";
            public const string Email = "Email";
            public const string Phone = "Phone";
            public const string TAddress = "TAddress";
            public const string YearGraduation = "YearGraduation";
            public const string GraduateSchool = "GraduateSchool";
            public const string Specialisation = "Specialisation";
            public const string DateExam = "DateExam";
            public const string Description = "Description";
            public const string Score1 = "Score1";
            public const string Score3 = "Score3";
        }
    }
    public class Rec_ReportInterviewSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CodeCandidate)]
        public string CodeCandidate { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Guid? JobVacancyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_LevelInterview)]
        public Nullable<int> LevelInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateOfContractEva)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }

        public DateTime? DateExamFrom { get; set; }
        public DateTime? DateExamTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
