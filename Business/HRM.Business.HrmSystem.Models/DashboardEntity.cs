
using System;
namespace HRM.Business.HrmSystem.Models
{
    public class DashboardInformationEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Guid ID { get; set; } 
        public string ProfileName { get; set; }
        public string UserInfoName { get; set; }
        public string EmployeeTypeName { get; set; }
        public string JobTitleName { get; set; }
        public string OrgStructureName { get; set; }
        public string WorkingPlace { get; set; }
        public DateTime? TimeEnd { get; set; }

        public bool? IsDelete { get; set; }
    }

    public class DashboardAlertsEntity
    {
        public int sumContract { get; set; }
        public int sumProbation { get; set; }
        public int sumBirthDay { get; set; }
    }

    public class DashboardLeavesSummaryEntity
    {
        public Guid ID { get; set; }
        public string LeavesType { get; set; }
        public int Entitled { get; set; }
        public int Taken { get; set; }
        public int Balance { get; set; }
    }

    public class DashboardApproveEntity
    {
        public Guid ID { get; set; }
        public int ApproveOverTime { get; set; }
        public int WaitingApproveOverTime { get; set; }
        public int ApproveLeavesDay { get; set; }
        public int WaitingApproveLeavesDay { get; set; }
        public int ApproveRoster { get; set; }
        public int WaitingApproveRoster { get; set; }
    }
}
