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
    public class Rec_QuestionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_Questionaire_QuestionaireName)]
        public Nullable<System.Guid> QuestionaireID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Questionaire_QuestionaireName)]
        public string QuestionaireName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_QuestionBankName)]
        public Nullable<System.Guid> QuestionBankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_QuestionBankName)]
        public string QuestionBankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_Position)]
        public int Position { get; set; }
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_Rate)]
        public Nullable<double> Rate { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Rate = "Rate";
            public const string QuestionBankName = "QuestionBankName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Rec_QuestionSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_Questionaire_QuestionaireName)]
        public Guid? QuestionaireID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    
}
