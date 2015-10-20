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
    public class Rec_ReportListCandidateModel : BaseViewModel
    {
        public Nullable<System.DateTime> DateInterview1 { get; set; }
        public Nullable<System.DateTime> DateInterview2 { get; set; }
        public Nullable<System.DateTime> DateInterview3 { get; set; }
        public Nullable<System.DateTime> DateInterview4 { get; set; }
        public Nullable<System.DateTime> DateInterview5 { get; set; }
        public Nullable<int> CandidateNumber { get; set; }
        public string LanguageCode1 { get; set; }
        public string LanguageCode2 { get; set; }
        public string LanguageCode3 { get; set; }
        public string LanguageCode4 { get; set; }
        public string LanguageCode5 { get; set; }
        public double? Score1_1 { get; set; }
        public double? Score1_2 { get; set; }
        public double? Score1_3 { get; set; }
        public string KQ1 { get; set; }

        public double? Score2_1 { get; set; }
        public double? Score2_2 { get; set; }
        public double? Score2_3 { get; set; }
        public string KQ2 { get; set; }

        public double? Score3_1 { get; set; }
        public double? Score3_2 { get; set; }
        public double? Score3_3 { get; set; }
        public string KQ3 { get; set; }

        public double? Score4_1 { get; set; }
        public double? Score4_2 { get; set; }
        public double? Score4_3 { get; set; }
        public string KQ4 { get; set; }

        public double? Score5_1 { get; set; }
        public double? Score5_2 { get; set; }
        public double? Score5_3 { get; set; }
        public string KQ5 { get; set; } 
        public Nullable<System.DateTime> DateInterviewLevel1 { get; set; }
        public string EducationLevelName { get; set; }
        public Nullable<System.DateTime> DateApply { get; set; }
        public Guid? PositionID { get; set; }
        public Guid? RankID { get; set; }
        public Guid? SourceAdsID { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> DateExamFrom { get; set; }
        public Nullable<System.DateTime> DateExamTo { get; set; }
        public Nullable<System.DateTime> DateInterviewFrom { get; set; }
        public Nullable<System.DateTime> DateInterviewTo { get; set; }
        public int? LevelInterview { get; set; }
        public string SourceAdsName { get; set; }
        public string Assessment { get; set; }
        public string CodeCandidate { get; set; }
        public string CandidateName { get; set; }
        public string Gender { get; set; }
        public string GenderView { get; set; }
        public Nullable<System.DateTime> IDDateOfIssue { get; set; }
        public string MarriageStatus { get; set; }
        public string MarriageStatusView { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string PAddress { get; set; }
        public string IdentifyNumber { get; set; }
        public string Mobile { get; set; }
        public string GraduateSchool { get; set; }
        public string Specialisation { get; set; }
        public Nullable<System.DateTime> YearGraduation { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DateExam { get; set; }
        public double? Score1 { get; set; }
        public double? Score2 { get; set; }
        public double? Score3 { get; set; }
        public string ResultInterview1 { get; set; }
        public string ResultInterview2 { get; set; }
        public string ResultInterview3 { get; set; }
        public Nullable<System.DateTime> DateHire { get; set; }
        public string StatusView { get; set; }
        public string Workplace { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportID { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string CandidateNumber = "CandidateNumber";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string StatusView = "StatusView";
            public const string Workplace = "Workplace";
            public const string SourceAdsName = "SourceAdsName";
            public const string Gender = "Gender";
            public const string Assessment = "Assessment";
            public const string CodeCandidate = "CodeCandidate";
            public const string CandidateName = "CandidateName";
            public const string MarriageStatus = "MarriageStatus";
            public const string MarriageStatusView = "MarriageStatusView";
            public const string DateOfBirth = "DateOfBirth";
            public const string PAddress = "PAddress";
            public const string IdentifyNumber = "IdentifyNumber";
            public const string Mobile = "Mobile";
            public const string GraduateSchool = "GraduateSchool";
            public const string Specialisation = "Specialisation";
            public const string YearGraduation = "YearGraduation";
            public const string Email = "Email";
            public const string DateExam = "DateExam";
            public const string Score1 = "Score1";
            public const string Score2 = "Score2";
            public const string Score3 = "Score3";
            public const string ResultInterview1 = "ResultInterview1";
            public const string DateInterview2 = "DateInterview2";
            public const string ResultInterview2 = "ResultInterview2";
            public const string DateInterview3 = "DateInterview3";
            public const string ResultInterview3 = "ResultInterview3";
            public const string DateHire = "DateHire";
            public const string GenderView = "GenderView";
        }
    }
}
