using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_ProfileEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string CodeAttendance { get; set; }
        public Guid? OrgStructureID { get; set; }
        public Guid? JobTitleID { get; set; }
        public Guid? PositionID { get; set; }
        public DateTime? DateHire { get; set; }
        public DateTime? DateEndProbation { get; set; }
        public string Gender { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public string OrgStructureName { get; set; }
        public string WorkingPlace { get; set; }
        public string Status { get; set; }
        public string StatusSyn { get; set; }
        public DateTime? DateQuit { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


    
    }
}
