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
    public class Rec_QuestionaireModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_Questionaire_QuestionaireName)]
        public string QuestionaireName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Certificate_Code)]
        public string Code { get; set; }
        public string Module { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_QuestionBank_QuestionBankName)]
        public Guid? QuestionBankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_Rate)]
        public double? Rate { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string QuestionaireName = "QuestionaireName";
            public const string Status = "Status";
            public const string Code = "Code";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Rec_QuestionaireSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_Questionaire_QuestionaireName)]
        public string QuestionaireName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    
}
