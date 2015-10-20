using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_CodeConfigEntity : HRM.Business.BaseModel.HRMBaseModel 
    {
        public string ObjectName { get; set; }
        public string Code { get; set; }
        public string FieldName { get; set; }
        public string Formula { get; set; }
        public int? Ordinal { get; set; }
        public bool? IsOnInitObject { get; set; }
        public bool? IsResetByDay { get; set; }
        public bool? IsResetByMonth { get; set; }
        public bool? IsResetByYear { get; set; }
        public bool? IsResetByUser { get; set; }
        public bool? IsResetByField { get; set; }
        public string ResetByField1 { get; set; }
        public string ResetByField2 { get; set; }
        public string Description { get; set; }
    }
}
