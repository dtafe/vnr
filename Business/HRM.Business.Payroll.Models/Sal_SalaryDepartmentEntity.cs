using System;


namespace HRM.Business.Payroll.Models
{
    public class Sal_SalaryDepartmentEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        public System.DateTime MonthYear { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string TypeCompute { get; set; }
        public Nullable<System.Guid> ProductionLineID { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public double Amount { get; set; }
        public Nullable<double> TotalGroupHours { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<bool> IsAll { get; set; }
        public string Note { get; set; }
        public string CutOffDurationName { get; set; }
        public string ProductionLineName { get; set; }
        public string OrgStructureName { get; set; }
        public string CurrencyName { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductItemID { get; set; }
        public double? QuantityCalculate { get; set; }
        public double? RateForPO { get; set; }
        public double? AmountAdjust { get; set; }
        public double? AmountAfterAdjust { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }

    }

    public class Sal_SalaryDepartmentMultiEntity
    {
        public Guid ID { get; set; }
        public string DepartmentName { get; set; }
    }
    
}

