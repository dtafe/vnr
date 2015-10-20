using System;


namespace HRM.Business.Category.Models
{
    public class Cat_UnusualAllowanceCfgEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string UnusualAllowanceCfgName { get; set; }
        public string EDType { get; set; }
        public string Code { get; set; }
        public double Amount { get; set; }
        public bool IsAddToHourlyRate { get; set; }
        public bool IsChargePIT { get; set; }
        public bool IsExcludePayslip { get; set; }
        public string MethodCalculation { get; set; }
        public string Formula { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public string TypeView { get; set; }
        public string EDTypeView { get; set; }
    }
    public class Cat_UnusualAllowanceCfgMultiEntity
    {
        public Guid ID { get; set; }
        public string Code { get; set; }

        public string UnusualAllowanceCfgName { get; set; }
    }

}

