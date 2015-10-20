using System;


namespace HRM.Business.Category.Models
{
    public class Cat_UsualAllowanceEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string UsualAllowanceName { get; set; }
        public string Code { get; set; }
        public string EDType { get; set; }
        public bool IsAddToHourlyRate { get; set; }
        public bool IsChargePIT { get; set; }
        public bool IsBaseOnWorkday { get; set; }
        public double? MinHours { get; set; }
        public bool? UseSalaryByHours { get; set; }
        public string MethodPayroll { get; set; }
        public bool? IsUseHoursSalary { get; set; }
        public string Formula { get; set; }
        public string MethodCalculation { get; set; }
        public string FormulaNoPIT { get; set; }
        public string Comment { get; set; }
    }

    public class Cat_UsualAllowanceMultiEntity
    {
        public Guid ID { get; set; }
        public string UsualAllowanceName { get; set; }
    } 
}

