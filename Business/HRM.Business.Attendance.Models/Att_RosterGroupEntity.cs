using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class Att_RosterGroupEntity : HRMBaseModel
    {
        //public int TotalRow { get; set; }
        public string RosterGroupName { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Status { get; set; }
        public Guid? MonShiftID { get; set; }
        public Guid? MonShift2ID { get; set; }
        public Guid? TueShiftID { get; set; }
        public Guid? TueShift2ID { get; set; }
        public Guid? WedShiftID { get; set; }
        public Guid? WedShift2ID { get; set; }
        public Guid? ThuShiftID { get; set; }
        public Guid? ThuShift2ID { get; set; }
        public Guid? FriShiftID { get; set; }
        public Guid? FriShift2ID { get; set; }
        public Guid? SatShiftID { get; set; }
        public Guid? SatShift2ID { get; set; }
        public Guid? SunShiftID { get; set; }
        public Guid? SunShift2ID { get; set; }
        public Guid? UserApproveID { get; set; }
        public Guid? UserApproceID2 { get; set; }
        public string Note { get; set; }

        public string MonShiftName { get; set; }
        public string TueShiftName { get; set; }
        public string WedShiftName { get; set; }
        public string ThuShiftName { get; set; }
        public string FriShiftName { get; set; }
        public string SatShiftName { get; set; }
        public string SunShiftName { get; set; }
        public string UserApproveName2 { get; set; }
        public string UserApproveName { get; set; }
    }
}
