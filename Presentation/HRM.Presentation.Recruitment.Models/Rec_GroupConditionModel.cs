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
    public class Rec_GroupConditionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_GroupCondition_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_GroupCondition_GroupName)]
        public string GroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_GroupCondition_LevelInterview)]
        public Nullable<int> LevelInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_GroupCondition_JobConditionIDs)]
        public string JobConditionIDs { get; set; }

  
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        private Guid _id = Guid.Empty;
        public Guid GroupCondition_ID
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
            public const string Code = "Code";
            public const string GroupName = "GroupName";
            public const string LevelInterview = "LevelInterview";
            public const string JobConditionIDs = "JobConditionIDs";
        }
    }
    public class Rec_GroupConditionSearchModel
    {
        public string GroupName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Rec_GroupConditionMultiModel
    {
        public Guid ID { get; set; }
        public string GroupName { get; set; }
        public int TotalRow { get; set; }
    }

    public class Rec_JobConditionByGroupConditionSearchModel
    {
        public string JobConditionIDs { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
