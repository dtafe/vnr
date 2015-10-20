using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_QuestionEntity : HRMBaseModel
    {
        public Nullable<System.Guid> QuestionaireID { get; set; }
        public string QuestionaireName { get; set; }
        public Nullable<System.Guid> QuestionBankID { get; set; }
        public string QuestionBankName { get; set; }
        public int Position { get; set; }
        public string Code { get; set; }
        public Nullable<double> Rate { get; set; }

    }
    
}
