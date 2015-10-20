using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Cat_InsuranceConfigEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> ContractTypeID { get; set; }
        public string ContractTypeName { get; set; }
        public Nullable<int> FromMonth { get; set; }
        public Nullable<int> ToMonth { get; set; }
        public Nullable<bool> IsSocial { get; set; }
        public Nullable<bool> IsHealth { get; set; }
        public Nullable<bool> IsUnEmploy { get; set; }
    }
}
