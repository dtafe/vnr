using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Report.Models
{
    public class Rep_MasterEntity : HRMBaseModel
    {
        public string Description { get; set; }
        public string LinkObject { get; set; }
        public string ObjectMain { get; set; }
        public Guid? ReportMappingID { get; set; }
        public string ReportName { get; set; }
        public string ReportStoreModule { get; set; }
        public Guid? ResourceID { get; set; }
        public string ScriptExcute { get; set; }
        public string ScriptParams { get; set; }
    }
}
