using HRM.Business.BaseModel;
using System;
namespace HRM.Business.HrmSystem.Models
{
    public class Sys_DelegateApproveEntity : HRMBaseModel
    {
        public Nullable<System.Guid> UserID { get; set; }
        public string UserName { get; set; }
        public Nullable<System.Guid> UserDelegateID { get; set; }
        public string UserDelegateName { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string DataTypeDelegate { get; set; }
        public string DataTypeDelegateView { get; set; }
        public string Status { get; set; }
    }
}
