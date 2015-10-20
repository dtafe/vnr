using System;

namespace HRM.Business.Hr.Models
{
    public class Hre_NotificationEntity
    {
        public Guid ID { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string FullName { get; set; }
        public string ProfileName { get; set; }
        public string UserInfoName { get; set; }
        public string WorkingPlace { get; set; }
        public string EmployeeTypeName { get; set; }
        public string JobTitleName { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public int? CountOvertime { get; set; }
        public int? CountLeaveday { get; set; }
        public int? CountRoster { get; set; }
        public string ActionStatus { get; set; }
    }
}
