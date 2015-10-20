using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class DashboardInformationModel
    {
        public Guid ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_User_LoginName)]
        public string UserInfoName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_EmployeeType_EmployeeTypeName)]
        public string EmployeeTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkingPlace { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_User_LastLogin)]
        public DateTime? TimeEnd { get; set; }

        public bool? IsDelete { get; set; }
    }

    public class DashboardAlertsModel : BaseViewModel
    {
        //cau hinh hien thi nguoi chờ đánh giá
        public bool IsShowEvalutionWaiting { get; set; }
        //hien thi canh bao hop dong cho duyet
        public bool IsShowContractWaitingAprove { get; set; }
        //hien canh bao trang chu hien luong co bang cho duyet hay khong
        public bool IsShowBasicSalaryWaitingAprove { get; set; }

        //hien canh bao trang chu hd den han hay khong
        public bool ShowValue1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Dashboard_SummaryContract)]
        public int sumContract { get; set; }
        // hien canh bao trang chu Hết Hạn Thử Việc Trong Tháng hay khong
        public bool ShowValue2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Dashboard_SummaryProbation)]
        public int sumProbation { get; set; }
        //cau hinh hien thi sinh nhat trong thang
        public bool ShowValue5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Dashboard_SummaryBirthDay)]
        public int sumBirthDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Dashboard_SummaryWaitEva)]
        public int sumWaitEva { get; set; }
        [DisplayName(ConstantDisplay.HRM_Dashboard_sumContractWaiting)]
        public int sumContractWaiting { get; set; }
        //cau hinh hien thi canh bao nv lam HDT cho duyet
        public bool IsShowHDTJobWaitingApproved { get; set; }
        [DisplayName(ConstantDisplay.HRM_Dashboard_sumHDTJobWaitingApproved)]
        public int sumHDTJobWaitingApproved { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalaryPending)]
        public int BasicSalaryPending { get; set; }
        //hien thi canh bao phu luc hop dong den han
        public bool IsShowContractAppendixExpriday { get; set; }
        public int sumAppendixExpiryContract { get; set; }
        //hien thi cau hinh nhan vien sap den ngay tam hoan
        public bool IsShowDaySuspenseExpiry { get; set; }
        [DisplayName(ConstantDisplay.HRM_Dashboard_sumDaySuspenseExpiry)]
        public int sumDaySuspenseExpiry { get; set; }
        //cau hinh hien thi nv sap den ngay nghi viec
        public bool IsShowDayStopWorking { get; set; }
        public int sumDayStopWorking { get; set; }
        //cau hinh hien thi nv sap vao lam lai
        public bool IsShowDayComeBackExpiry { get; set; }
        public int? sumDayComeBackExpiry { get; set; }
        // Giấy Phép LĐ Đến Hạn
        public bool IsShowExpiryWorkPermit { get; set; }
        public int sumExpiryWorkPermit { get; set; }
        public bool IsShowSumProfileQuitNotSettlement { get; set; }
        public int SumProfileQuitNotSettlement { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class DashboardLeavesSummaryModel
    {
        public Guid ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_LeaveDayTypeName)]
        public string LeavesType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Dashboard_Entitled)]
        public int Entitled { get; set; }
        [DisplayName(ConstantDisplay.HRM_Dashboard_Taken)]
        public int Taken { get; set; }
        [DisplayName(ConstantDisplay.HRM_Dashboard_Balance)]
        public int Balance { get; set; }

        public partial class FieldNames
        {
            public const string LeavesType = "LeavesType";
            public const string Entitled = "Entitled";
            public const string Taken = "Taken";
            public const string Balance = "Balance";
        }
    }
    public class DashboardLeavesSummarySearchModel
    {

    }
    public class DashboardApproveModel
    {
        public Guid ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Dashboard_ApproveOverTime)]
        public int ApproveOverTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Dashboard_WaitingApproveOverTime)]
        public int WaitingApproveOverTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Dashboard_ApproveLeavesDay)]
        public int ApproveLeavesDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Dashboard_WaitingApproveLeavesDay)]
        public int WaitingApproveLeavesDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Dashboard_ApproveRoster)]
        public int ApproveRoster { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Dashboard_WaitingApproveRoster)]
        public int WaitingApproveRoster { get; set; }
    }
    public class DashboardApproveSearchModel
    {
        public Guid ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
