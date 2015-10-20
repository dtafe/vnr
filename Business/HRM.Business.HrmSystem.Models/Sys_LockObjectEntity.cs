using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_LockObjectEntity : HRMBaseModel
    {
        public System.DateTime DateStart { get; set; }
        public System.DateTime DateEnd { get; set; }
        public string OrgStructureName { get; set; }
        public string Type { get; set; }
        public Nullable<System.Guid> UserApprove1ID { get; set; }
        public Nullable<System.Guid> UserApprove2ID { get; set; }
        public string Status { get; set; }
        public byte[] OrgStructures { get; set; }
        public List<int> OrgStructureOrderNumber { get; set; }
        public byte[] PayrollGroups { get; set; }
        public string Commend { get; set; }
        public string UserApprove1Name { get; set; }
        public string UserApprove2Name { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public string CutOffDurationName { get; set; }
        public string StatusView { get; set; }
    }
}
