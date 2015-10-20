using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SkillModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_Skill_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Skill_SkillName)]
        public string SkillName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Skill_Note)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Skill_ID
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
            public const string Code = "Code";
            public const string SkillName = "SkillName";
            public const string Note = "Note";
        }
    }

    public class Cat_SkillSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_Skill_SkillName)]
        public string SkillName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_SkillMultiModel
    {
        public Guid ID { get; set; }
        public string SkillName { get; set; }
        public int TotalRow { get; set; }
    }

}
