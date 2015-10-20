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
    public class Rec_CandidateLanguageLevelModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_HR_Language_SpecialLevelID)]
        public Nullable<System.Guid> SpecialLevelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Language_SpecialTypeID)]
        public Nullable<System.Guid> SpecialTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Language_SpecialSkillID)]
        public Nullable<System.Guid> SpecialSkillID { get; set; }
        public Nullable<System.Guid> CandidateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Language_Skill)]
        public string LanguageSkill { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Language_Type)]
        public string LanguageType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Language_Level)]
        public string LanguageLevel { get; set; }
        public partial class FieldNames
        {
            public const string LanguageSkill = "LanguageSkill";
            public const string LanguageType = "LanguageType";
            public const string LanguageLevel = "LanguageLevel";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }
 
}
