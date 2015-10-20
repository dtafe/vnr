using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_ReleaseNoteModel : BaseViewModel
    {
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_ReleaseNote_Code)]
        public string Code { get; set; }

        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_System_ReleaseNote_ReleaseNoteName)]
        public string ReleaseNoteName { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_ReleaseNote_DateRelease)]
        public DateTime DateRelease { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_ReleaseNote_FeaturesNew)]
        public string FeaturesNew { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_ReleaseNote_Enhancements)]
        public string Enhancements { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_ReleaseNote_Fixes)]
        public string Fixes { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_ReleaseNote_KnownIssuesandProblems)]
        public string KnownIssuesandProblems { get; set; }

        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string ReleaseNoteName = "ReleaseNoteName";
            public const string DateRelease = "DateRelease";
            public const string FeaturesNew = "FeaturesNew";
            public const string Enhancements = "Enhancements";
            public const string Fixes = "Fixes";
            public const string KnownIssuesandProblems = "KnownIssuesandProblems";
        }
    }

    public class Sys_ReleaseNoteSearchModel
    {
        public string Code { get; set; }
        public string ReleaseNoteName { get; set; }
        //public DateTime DateRelease { get; set; }
    }

    public class Sys_ReleaseNoteModelMultiModel
    {
        public Guid ID { get; set; }
        public string ReleaseNoteName { get; set; }
    }

}