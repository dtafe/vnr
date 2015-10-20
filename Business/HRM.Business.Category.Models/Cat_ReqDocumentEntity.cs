using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Category.Models
{
    public class Cat_ReqDocumentEntity : HRMBaseModel
    {
        public Guid ID { get; set; }
        public string ReqDocumentName { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
        public string TypeOfDocument { get; set; }
        public Nullable<System.Guid> ResignReasonID { get; set; }
        public string ResignReasonName { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public string JobTitleName { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public string PositionName { get; set; }
        public Nullable<System.Guid> OrgID { get; set; }
        public string OrgName { get; set; }
    }
    public class Cat_ReqDocumentMuliEntity
    {
        public Guid ID { get; set; }
        public string ReqDocumentName { get; set; }
    }
}
