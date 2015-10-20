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
    public class Rec_QuestionBankModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionType_QuestionTypeName)]
        public System.Guid QuestionTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionType_QuestionTypeName)]
        public string QuestionTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_QuestionBankName)]
        public string QuestionBankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_Position)]
        public int Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_Length)]
        public int? Length { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_AnswerViewCol)]
        public int? AnswerViewCol { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_IsPublic)]
        public Nullable<bool> IsPublic { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_PointMin)]
        public Nullable<double> PointMin { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_PointMax)]
        public Nullable<double> PointMax { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_Rate)]
        public Nullable<double> Rate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_Module)]
        public string Module { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string QuestionBankName = "QuestionBankName";
            public const string QuestionTypeName = "QuestionTypeName";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string Length = "Length";
            public const string AnswerViewCol = "AnswerViewCol";
            public const string PointMin = "PointMin";
            public const string PointMax = "PointMax";
            
        }
    }

    public class Rec_QuestionBankSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_QuestionBankName)]
        public string QuestionBankName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Rec_QuestionBankMultiModel
    {
        public Guid ID { get; set; }
        public string QuestionBankName { get; set; }
    }
}
