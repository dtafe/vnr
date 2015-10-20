using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_CandidateBusinessEntity : HRMBaseModel
    {
        public System.Guid CandidateID { get; set; }
        public string CandidateName { get; set; }
        public string BusinessType { get; set; }
        public string BusinessArea { get; set; }
        public string Description { get; set; }

    }
    
}
