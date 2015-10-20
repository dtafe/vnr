using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_QuestionaireEntity : HRMBaseModel
    {
        public string QuestionaireName { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Module { get; set; }
        public Guid? QuestionBankID { get; set; }
        public double? Rate { get; set; }

    }
    
}
