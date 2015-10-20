using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_AsynTaskModel : BaseViewModel
    {
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AsynTask_Summary)]
        public string Summary { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AsynTask_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AsynTask_PercentComplete)]
        public double PercentComplete { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AsynTask_TimeStart)]
        public System.DateTime TimeStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AsynTask_TimeEnd)]
        public Nullable<System.DateTime> TimeEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AsynTask_Description)]
        public string Description { get; set; }
        public string Dept { get; set; }
        public int TotalComputedProfileForTask { get; set; }
        public int TotalRow { get; set; }
        public List<Guid> ListIDs { get; set; }
        public Guid? CutOffDurationID { get; set; }
        public partial class FieldNames
        {
            public const string Summary = "Summary";
            public const string Status = "Status";
            public const string TimeStart = "TimeStart";
            public const string TimeEnd = "TimeEnd";
            public const string Description = "Description";
            public const string UserCreate = "UserCreate";
            public const string PercentComplete = "PercentComplete";
        }
    }

    public class Sys_AsynTaskSearchModel
    {
        public string Type { get; set; }
    }

    public class Sys_AsynTaskComputeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkPlace { get; set; }
        public string WorkPlaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_PayrollID)]
        public string SalaryJobGroup { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_CutOffDurationID)]
        public Guid CutOffDurationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDuration2ID)]
        public Guid CutOffDuration2ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public Guid? ProfileID { get; set; }
        public int? Settlement { get; set; }
        public string MethodPayroll { get; set; }
        public List<Guid> ListProfileIDs { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public string ActionStatus { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string ProfileIDs { get; set; }
        public string UserLogin { get; set; }
        public bool isIncludeWorkingEmp { get; set; }
        public bool PaymentQuit { get; set; }
        public string GradePayrollID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OnlyQuitEmp)]
        public bool OnlyQuitEmp { get; set; }
    }
    
}
