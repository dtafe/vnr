using System;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_SaleEvaluationEntity : BaseModel.HRMBaseModel
    {
        public string SaleEvaluationName { get; set; }
        public DateTime? Year { get; set; }
        public string PerfixName { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public Nullable<double> TagetNumber { get; set; }
        public Nullable<double> ResultNumber { get; set; }
        public string ResultLevel { get; set; }
        public double? BonusLevel { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string SalesTypeCode { get; set; }
        public Guid? ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public string CodeEmp { get; set; }
        public Guid? PerfixID { get; set; }
        public double? ResultPercent { get; set; }
        public Guid? SalesTypeID { get; set; }
        public string Note { get; set; }
        public string SalesTypeName { get; set; }

        public double? TargetNumberSaleIn { get; set; }
        public double? ResultNumberSaleIn { get; set; }
        public double? ResultPercentSaleIn { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }
    }
}
