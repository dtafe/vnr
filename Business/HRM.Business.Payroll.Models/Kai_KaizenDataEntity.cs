using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Kai_KaizenDataEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string KaizenName { get; set; }
        public Nullable<System.DateTime> Month { get; set; }
        public Nullable<System.Guid> CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Nullable<double> MarkIdea { get; set; }
        public Nullable<double> AmountIdea { get; set; }
        public Nullable<double> MarkPerform { get; set; }
        public Nullable<double> AmountPerform { get; set; }
        public Nullable<double> SumMark { get; set; }
        public Nullable<double> SumAmount { get; set; }
        public Nullable<double> Accumulate { get; set; }
        public string CategoryList { get; set; }
        public Nullable<double> AmountTransfered { get; set; }
        public Nullable<System.DateTime> DateTransferPayment { get; set; }
        public string Status { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public string KaiOrgStructureName { get; set; }
        public string   StatusSyn { get; set; }
        public string StatusView { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public Nullable<bool> IsPaymentOut { get; set; }
        public double? AccumulateRevice { get; set; }

    }
}
