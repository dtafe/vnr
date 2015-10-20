using System;


namespace HRM.Business.Category.Models
{
    public class Cat_LateEarlyRuleEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Guid GradeCfgID { get; set; }
        public Guid? GradeAttID { get; set; }
        public string GradeCfgName { get; set; }

        public string Code { get; set; }
        public int Order { get; set; }
        public double MinValue { get; set; }
        public double? MaxValue { get; set; }
        public double RoundValue { get; set; }
        public string Comment { get; set; }
    }
    
}

