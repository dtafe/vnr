using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_ImportFromTAMEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string CodeEmp;
        public DateTime? TimeScan;
        public string Status;
    }
}
