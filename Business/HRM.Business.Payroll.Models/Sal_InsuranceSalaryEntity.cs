using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
  
    public class Sal_InsuranceSalaryEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ProfileName { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.DateTime> DateEffect { get; set; }
        public Nullable<double> InsuranceAmount { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<bool> IsSocialIns { get; set; }
        public Nullable<bool> IsMedicalIns { get; set; }
        public Nullable<bool> IsUnimploymentIns { get; set; }
        public string DepartmentName { get; set; }
        public string DecisionNo { get; set; }
        public string Note { get; set; }
        public string CurrencyName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


    }
}
