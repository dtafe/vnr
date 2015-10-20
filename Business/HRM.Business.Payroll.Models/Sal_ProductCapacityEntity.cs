using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_ProductCapacityEntity : HRMBaseModel
    {
        public string ProductCapacityName { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<System.Guid> ProductItemID { get; set; }
        public string ProductItemName { get; set; }
        public Nullable<System.Guid> DepartmentID { get; set; }
        public string OrgStructureName { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public Nullable<double> MinCapacity { get; set; }
        public Nullable<double> MaxCapacity { get; set; }
        public Nullable<double> Rate { get; set; }
        public string Formula { get; set; }
        public string Notes { get; set; }
        public string Code { get; set; }
    }
}
