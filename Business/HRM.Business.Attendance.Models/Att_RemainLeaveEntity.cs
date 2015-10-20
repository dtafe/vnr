using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_RemainLeaveEntity : HRMBaseModel
    {
        public Guid ProfileID { get; set; }
        public double? RemainEndMonth { get; set; }
        public double? RemainEndYear { get; set; }
    }
}
