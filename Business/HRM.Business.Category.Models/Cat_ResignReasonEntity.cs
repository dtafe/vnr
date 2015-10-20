using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Category.Models
{
    public class Cat_ResignReasonEntity : HRMBaseModel
    {
        public string ResignReasonName { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
        public string ResignReasonNameEn { get; set; }
        public string NotesEn { get; set; }
        public string UserUpdate { get; set; }
        public DateTime? DateUpdate { get; set; }
    }

    public class Cat_ResignReasonMultiEntity
    {
        public Guid ID { get; set; }
        public string ResignReasonName { get; set; }
    }
}

