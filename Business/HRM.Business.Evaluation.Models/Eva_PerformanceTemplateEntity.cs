using System;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_PerformanceTemplateEntity : BaseModel.HRMBaseModel
    {
        public string TemplateName { get; set; }
        public Guid? PositionID { get; set; }
        public Guid? PerformanceTypeID { get; set; }
        public string Comment { get; set; }
        public string Code { get; set; }
        public string Formula { get; set; }
        public string Formula1 { get; set; }

        public Guid? OrgID { get; set; }
        public string PerformanceTypeName { get; set; }
        public string OrgStructureName { get; set; }
        public Guid? JobTitleID { get; set; }
        public string JobTitleName { get; set; }
    }

    public class Eva_PerformanceTemplateMultiEntity
    {
        public Guid ID { get; set; }
        public string TemplateName { get; set; }
    }
}
