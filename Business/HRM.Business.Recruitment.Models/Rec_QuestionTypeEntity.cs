using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_QuestionTypeEntity : HRMBaseModel
    {
        public string QuestionTypeName { get; set; }
        public string Description { get; set; }
        public bool IsChoice { get; set; }
        public Nullable<int> Type { get; set; }
        public string ResponseTable { get; set; }
        public string Code { get; set; }
    }

    public class Rec_QuestionTypeMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string QuestionTypeName { get; set; }
    }
}
