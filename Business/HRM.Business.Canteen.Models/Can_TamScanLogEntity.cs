using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;
namespace HRM.Business.Canteen.Models
{
    public class Can_TamScanLogEntity : HRMBaseModel
    {
        public string CardCode { get; set; }
        public DateTime? TimeLog { get; set; }
        public string Type { get; set; }
        public string SrcType { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string MachineCode { get; set; }
        public List<Guid?> OrgStructureID { get; set; }
        public List<Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class Can_TAMEntityForDelete
    {
        public string strIDs { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
