using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_RelativeEntity : HRMBaseModel
    {
        public string RelativeName { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public Nullable<System.Guid> CandidateID { get; set; }
        public string CandidateName { get; set; }
        public Nullable<System.Guid> RelationshipID { get; set; }
        public string RelationshipName { get; set; }
        public string BusinessRelation { get; set; }
        public string Type { get; set; }
        public string BussinessType { get; set; }
        public string Bussiness { get; set; }
        public string Phone { get; set; }
        public string DateOrBirth { get; set; }

    }
    
}
