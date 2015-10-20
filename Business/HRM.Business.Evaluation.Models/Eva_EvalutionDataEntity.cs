using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_EvalutionDataEntity : HRMBaseModel
    {
        //public Nullable<int> TotalC1C2 { get; set; }
        //public Nullable<int> TotalC3C4C5C6C7 { get; set; }
        //public Nullable<int> TotalC1C2C3C4C5C6C7 { get; set; }
        //public Nullable<int> TotalC8C9 { get; set; }
        public string TimeEvalutionData { get; set; }
        public Nullable<Guid> TimesGetDataID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public Nullable<System.DateTime> Year { get; set; }
        public Nullable<int> Time { get; set; }
        public string Type { get; set; }
        public Nullable<int> C1 { get; set; }
        public Nullable<int> C2 { get; set; }
        public Nullable<int> C3 { get; set; }
        public Nullable<int> C4 { get; set; }
        public Nullable<int> C5 { get; set; }
        public Nullable<int> C6 { get; set; }
        public Nullable<int> C7 { get; set; }
        public Nullable<int> C8 { get; set; }
        public Nullable<int> C9 { get; set; }
        public Nullable<int> C10 { get; set; }
        public Nullable<int> C11 { get; set; }
        public Nullable<int> C12 { get; set; }
        public Nullable<int> C13 { get; set; }
        public Nullable<int> C14 { get; set; }
        public Nullable<int> C15 { get; set; }
        public Nullable<int> C16 { get; set; }

        public string C17 { get; set; }
        public string C18 { get; set; }
        public double? C19 { get; set; }
        public double? C20 { get; set; }
        public double? C21 { get; set; }
        public string C22 { get; set; }
        public double? C23 { get; set; }
        public double? C24 { get; set; }
        public double? C25 { get; set; }
        public double? C26 { get; set; }
        public double? C27 { get; set; }
        public double? C28 { get; set; }
        public double? C29 { get; set; }
        public double? C30 { get; set; }
        public string C31 { get; set; }
        public string C32 { get; set; }

        public string CostCentreCode { get; set; }
        public string UnitNameOrg { get; set; }
        public string DivisionNameOrg { get; set; }
        public string DepartmentNameOrg { get; set; }
        public string SectionNameOrg { get; set; }
        public string TeamNameOrg { get; set; }
        public string WorkPlaceName { get; set; }
        public string WorkPlaceCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateHire { get; set; }
        public Guid? SalaryClassID { get; set; }
        public string SalaryClassName { get; set; }
        public string AbilityTitleVNI { get; set; }
        public string JobTitleName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string TimeEvalutionDataCode { get; set; }
    }
   
}
