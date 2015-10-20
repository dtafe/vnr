using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_CodeObjectEntity :  HRM.Business.BaseModel.HRMBaseModel 
    {
        public string Code { get; set; }
        public string TableName { get; set; }
        public string Prefix { get; set; }
        public Nullable<int> Ordinal { get; set; }
        public Nullable<bool> IsResetByDate { get; set; }
        public Nullable<bool> IsResetByUser { get; set; }
        public string FieldName { get; set; }
        public Nullable<bool> IsResetByMonth { get; set; }
        public Nullable<bool> IsResetByYear { get; set; }
        public Nullable<bool> IsResetByObject { get; set; }
        public string ResetObjectName { get; set; }
        public string ResetFieldName { get; set; }
        public Nullable<bool> IsManual { get; set; }
        public string ResetObjectName1 { get; set; }
        public string ResetFieldName1 { get; set; }
    }
}
