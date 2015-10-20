using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class Att_PregnancyEntity : HRMBaseModel
    {

        public Guid ProfileID { get; set; }
        public string ChildName { get; set; }
        public DateTime? ChildBirthday { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }

        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }

        public string TypePregnancyEarly { get; set; }
    }
}
