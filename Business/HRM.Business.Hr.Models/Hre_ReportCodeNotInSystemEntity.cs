using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportCodeNotInSystemEntity : HRMBaseModel
    {
        public string Code { get; set; }
        public DateTime? Time { get; set; }
        public string Type { get; set; }
        public string Machine { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid ExportID { get; set; }
    }
}
