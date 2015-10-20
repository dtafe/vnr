using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_RevenueForProfileEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public Nullable<double> Target { get; set; }
        public Nullable<double> Actual { get; set; }
        public Nullable<System.DateTime> Month { get; set; }
        public string Type { get; set; }
        public string TypeTranslate { get; set; }
        public string Note { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
