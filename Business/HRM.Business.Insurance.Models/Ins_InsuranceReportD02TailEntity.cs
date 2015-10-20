using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Insurance.Models
{
    /// <summary>Đuôi D02</summary>
    public class Ins_InsuranceReportD02TailEntity : HRMBaseModel
    {
        public string Name{ get; set; }
        public double PrePeriodInsSocialAmount { get; set; }
        public double PrePeriodInsHealthAmount { get; set; }
        public double PrePeriodInsUnEmpAmount { get; set; }
        
        public double InsSocialIncreaseAmount { get; set; }
        public double InsSocialDecreaseAmount { get; set; }
        
        public double InsHealthIncreaseAmount { get; set; }
        public double InsHealthDecreaseAmount { get; set; }
        
        public double InsUnEmpIncreaseAmount { get; set; }
        public double InsUnEmpDecreaseAmount { get; set; }
        public int STT { get; set; }
        
    }
}

