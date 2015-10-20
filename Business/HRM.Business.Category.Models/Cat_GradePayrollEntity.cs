using System;

namespace HRM.Business.Category.Models
{
    public class Cat_GradePayrollEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string GradeCfgName { get; set; }

        public bool? IsProductSalary { get; set; }
        public bool? IsMoneySalary { get; set; }
        public bool? IsFormulaSalary { get; set; }

        public DateTime? SalaryDateClose { get; set; }
        public Nullable<int> SalaryDayClose { get; set; }
        public Nullable<bool> IsDeptSalary { get; set; }
        public Nullable<bool> IsCommissionSalary { get; set; }
        public string SalaryTimeTypeClose { get; set; }
        public string FormulaSalaryIns { get; set; }
        public string FormulaJobNameIns { get; set; }


    }

    public class Cat_GradePayrollMultiEntity
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string GradeCfgName { get; set; }
    }
  
}
