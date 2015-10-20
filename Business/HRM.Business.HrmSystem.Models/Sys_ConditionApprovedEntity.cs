
using HRM.Business.BaseModel;
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_ConditionApprovedEntity : HRMBaseModel
    {
        public string ConditionName { get; set; }
        public string Description { get; set; }
        public string ApprovedType { get; set; }
        public string ExpensesType { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public Nullable<System.Guid> ProcessApprovedID { get; set; }
        public Nullable<System.Guid> OrgID1 { get; set; }
        public Nullable<System.Guid> OrgID2 { get; set; }
        public Nullable<System.Guid> OrgID3 { get; set; }
        public Nullable<System.Guid> OrgID4 { get; set; }
        public Nullable<System.Guid> OrgID5 { get; set; }
        public Nullable<System.Guid> OrgID6 { get; set; }

        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public string WorkPlaceName { get; set; }
        public string ProcessName { get; set; }
        public string OrgCode1 { get; set; }
        public string OrgCode2 { get; set; }
        public string OrgCode3 { get; set; }
        public string OrgCode4 { get; set; }
        public string OrgCode5 { get; set; }
        public string OrgCode6 { get; set; }
    }
}
