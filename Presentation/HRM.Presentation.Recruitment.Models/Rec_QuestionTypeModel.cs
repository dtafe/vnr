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
    public class Rec_QuestionTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionType_QuestionTypeName)]
        public string QuestionTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionType_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionType_IsChoice)]
        public bool IsChoice { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionType_Type)]
        public Nullable<int> Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionType_ResponseTable)]
        public string ResponseTable { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionType_Code)]
        public string Code { get; set; }
        public partial class FieldNames
        {
            public const string QuestionTypeName = "QuestionTypeName";
            public const string Description = "Description";
            public const string IsChoice = "IsChoice";
            public const string Type = "Type";
            public const string ResponseTable = "ResponseTable";
            public const string Code = "Code";
        }
    }

    public class Rec_QuestionTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionType_QuestionTypeName)]
        public string QuestionTypeName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Rec_QuestionTypeMultiModel
    {
        public Guid ID { get; set; }
        public string QuestionTypeName { get; set; }
        public int TotalRow { get; set; }
    }
}
