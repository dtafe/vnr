using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportSeniorityEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }

        public string JobTitleName { get; set; }

        public string PositionName { get; set; }


        public DateTime? DateHire { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public int? YearSeniority { get; set; }
        public int? MonthSeniority { get; set; }
        public string GrossAmount { get; set; }
        public string ContractTypeID { get; set; }
        public string ContractTypeName { get; set; }
        public DateTime? DateOfEffect { get; set; }

        public string CurrencyList { get; set; }
        public string Notes { get; set; }
        public double? AnnualYearRest { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }

    }
}
