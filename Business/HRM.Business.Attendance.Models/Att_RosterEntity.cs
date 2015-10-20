using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class Att_RosterEntity : HRMBaseModel
    {
        public string TypeTranslate { get; set; }
        public string StatusTranslate { get; set; }
        public Guid ProfileID { get; set; }

        public string ProfileName { get; set; }

        public string RosterGroupName { get; set; }

        public string CodeEmp { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string Type { get; set; }

        public Guid? MonShiftID { get; set; }

        public string MonShiftName { get; set; }

        public Guid? MonShift2ID { get; set; }

        public Guid? TueShiftID { get; set; }

        public string TueShiftName { get; set; }

        public Guid? TueShift2ID { get; set; }

        public Guid? WedShiftID { get; set; }

        public string WedShiftName { get; set; }

        public Guid? WedShift2ID { get; set; }

        public Guid? ThuShiftID { get; set; }

        public string ThuShiftName { get; set; }

        public Guid? ThuShift2ID { get; set; }

        public Guid? FriShiftID { get; set; }

        public string FriShiftName { get; set; }

        public Guid? FriShift2ID { get; set; }

        public Guid? SatShiftID { get; set; }

        public string SatShiftName { get; set; }

        public Guid? SatShift2ID { get; set; }

        public Guid? SunShiftID { get; set; }

        public string SunShiftName { get; set; }

        public Guid? SunShift2ID { get; set; }

        public Guid? OrgStructureID { get; set; }

        public string OrgStructureName { get; set; }

        public Guid? UserApproveID { get; set; }
        public string UserApproveIDName { get; set; }

        public Guid? UserApproveID2 { get; set; }
        public string UserApprove2IDName { get; set; }
        public string Status { get; set; }

        public string StatusRoster { get; set; }

        public string Comment { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Roster_ID
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

    public class Att_RosterInfo
    {
        public Guid ID { get; set; }
        public Guid ProfileID { get; set; }
        public Guid? MonShiftID { get; set; }
        public Guid? TueShiftID { get; set; }
        public Guid? WedShiftID { get; set; }
        public Guid? ThuShiftID { get; set; }
        public Guid? FriShiftID { get; set; }
        public Guid? SatShiftID { get; set; }
        public Guid? SunShiftID { get; set; }
        public string Type{ get; set; }
    }
}
