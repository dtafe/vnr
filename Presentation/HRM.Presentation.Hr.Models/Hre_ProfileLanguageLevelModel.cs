using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ProfileLanguageLevelModel : BaseViewModel 
    {


        [DisplayName(ConstantDisplay.HRM_HR_Language_SpecialLevelID)]
        public Guid? SpecialLevelID { get; set; }

      
        [DisplayName(ConstantDisplay.HRM_HR_Qualification_ProfileID)]
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Qualification_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Language_SpecialTypeID)]
        public Guid? SpecialTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Language_SpecialSkillID)]
        public Guid? SpecialSkillID { get; set; }

         [DisplayName(ConstantDisplay.HRM_HR_Language_Level)]
        public string LanguageLevel { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Language_Skill)]
        public string LanguageSkill { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Language_Type)]

        public string LanguageType { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Language_ID
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
            public const string ID = "ID";

            public const string LanguageSkill = "LanguageSkill";
            public const string LanguageLevel = "LanguageLevel";
            public const string ProfileID = "ProfileID";
            public const string SpecialLevelID = "SpecialLevelID";
            public const string SpecialTypeID = "ProfileID";
            public const string LanguageType = "LanguageType";
            public const string ProfileName = "ProfileName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string SpecialSkillID = "SpecialSkillID";
            public const string Language_ID = "Language_ID";
           
         
        }
    }
   
}
