using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_CostCentreSalMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string CostCentreName { get; set; } 
    }
    public class Sal_CostCentreSalEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        //public string Code { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string CostCentreName { get; set; }
        //public string ProfileName { get; set; } 
        public Nullable<System.Guid> CostCentreID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<double> Rate { get; set; }
        public string ElementType { get; set; }
        public string ElementName { get; set; }
        public string CostCenterCode { get; set; }

    }
}
