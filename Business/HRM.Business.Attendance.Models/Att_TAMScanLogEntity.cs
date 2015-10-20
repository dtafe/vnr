using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class Att_TAMScanLogEntity : HRMBaseModel
    {
        public Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string CodeAttendance { get; set; }
        public string ProfileName { get; set; }
        public string CardCode { get; set; }
        public DateTime? TimeLog { get; set; }
        public string Type { get; set; }
        public string SrcType { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string MachineNo { get; set; }
        public bool Checked { get; set; }
        private Guid _id = Guid.Empty;
        public Guid TAMScanLog_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
    }
}
