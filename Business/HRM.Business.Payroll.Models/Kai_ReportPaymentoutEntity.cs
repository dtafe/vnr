using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public partial class Kai_ReportPaymentoutEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public string Note { get; set; }

        public Double? AMOUNT { get; set; } 

    }
}
