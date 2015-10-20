using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_QuestionBankEntity : HRMBaseModel
    {
        public System.Guid QuestionTypeID { get; set; }
        public string QuestionTypeName { get; set; }
        public string QuestionBankName { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public int AnswerViewCol { get; set; }
        public Nullable<bool> IsPublic { get; set; }
        public string Code { get; set; }
        public Nullable<double> PointMin { get; set; }
        public Nullable<double> PointMax { get; set; }
        public Nullable<double> Rate { get; set; }
        public string Module { get; set; }
    }
    public class Rec_QuestionBankMultiEntity
    {
        public Guid ID { get; set; }
        public string QuestionBankName { get; set; }
    }
}
