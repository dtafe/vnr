using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_EmailRequireEntity : HRMBaseModel
    {
        public Guid IdObject { get; set; }
        public Guid IdUserApprove { get; set; }
        public string EmailUserApprove { get; set; }
        public bool? IsRegister { get; set; }
    }
}
