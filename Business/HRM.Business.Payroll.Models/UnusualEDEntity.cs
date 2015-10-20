using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class UnusualEDEntity : HRM.Business.BaseModel.HRMBaseModel
    {
     
        public System.Guid ProfileID { get; set; }
       
        public string ProfileName { get; set; }
     
        public System.Guid UnusualEDTypeID { get; set; }
     
        public double Amount { get; set; }
       
        public bool ChargePIT { get; set; }
     
        public System.DateTime MonthStart { get; set; }
     
        public System.DateTime MonthEnd { get; set; }
     
        public string UserUpdate { get; set; }
       
        public Nullable<System.DateTime> DateUpdate { get; set; }
    }
}
