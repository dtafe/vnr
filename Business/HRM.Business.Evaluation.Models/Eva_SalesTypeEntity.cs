using System;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_SalesTypeEntity : BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string SalesTypeName { get; set; }
        public string SalesTypeGroup { get; set; }
        public string Note { get; set; }
    }

    public class Eva_SalesTypeMultiEntity
    {
        public Guid ID { get; set; }
        public string SalesTypeName { get; set; }
    }
}
