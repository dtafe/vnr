using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Report.Models
{
    public class Rep_ConditionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ConditionGroupName { get; set; }
        public Nullable<System.Guid> MasterID { get; set; }
        public string Notes { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public string WhereType { get; set; }
    }
}
