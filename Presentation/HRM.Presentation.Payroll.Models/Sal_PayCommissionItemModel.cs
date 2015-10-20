using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_PayCommissionItemModel : BaseViewModel
    {

        public Nullable<System.Guid> PayCommissionID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ValueType { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string ElementType { get; set; }
        public string Value { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public string Description { get; set; }
    }
}
