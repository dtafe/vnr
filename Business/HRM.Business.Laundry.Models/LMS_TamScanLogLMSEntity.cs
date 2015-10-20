using System;
using System.Collections.Generic;

namespace HRM.Business.Laundry.Models
{
    public class LMS_TamScanLogLMSEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public DateTime? TimeLog { get; set; }
        public string Type { get; set; }
        public string SrcType { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string TAMStatus { get; set; }
        public List<Guid?> OrgStructureID { get; set; }
        public List<Guid> ProfileID { get; set; }
        public string MachineCode { get; set; }
    }
    public class Lau_TamScanLogEntityForDelete
    {
        public string strIDs { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
    
}
