using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_HoldSalaryModel : BaseViewModel
    {
        public string StatusTranslate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_ProfileName)]
        public Guid? ProfileID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_CodeEmp)]
        public string CodeEmp { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_CodeAttendance)]
        public string CodeAttendance { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_ProfileName)]
        public string ProfileName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_IDNo)]
        public string IDNo { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_WorkingPlaceName)]
         public string WorkPlaceName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_EmployeeName)]
         public string EmployeeTypeName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_MonthSalary)]
        public DateTime? MonthSalary { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_DayLeave)]
        public int? DayLeave { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_IsLeaveContinuous)]
        public bool? IsLeaveContinuous { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_Terminate)]
        public bool? Terminate { get; set; }
         [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_IsHoldSal)]
        public bool? IsHoldSal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_IsHoldSal)]
         public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID)]
        public Guid? CutOffDurationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_OrgStructure)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_ProfileName)]
        public string ProfileIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_MonthEndSalary)]
        public Nullable<System.DateTime> MonthEndSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_AmountSalary)]
        public Nullable<double> AmountSalary { get; set; }
        public DateTime? DateComeBack { get; set; }

        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_IsLeaveM)]
        public bool? IsLeaveM { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_TimeAnalyzeID)]
        public Guid? TimeAnalyzeID { get; set; }
        public string CatNameEntity { get; set; }
        public double? AmountSalaryAfterTax { get; set; }
      
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string DateComeBack = "DateComeBack";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string IDNo = "IDNo";
            public const string EmployeeTypeName = "EmployeeTypeName";
            public const string ProfileName = "ProfileName";
            public const string ProfileID = "ProfileID";
            public const string MonthSalary = "MonthSalary";
            public const string MonthEndSalary = "MonthEndSalary";
            public const string AmountSalary = "AmountSalary";
            public const string IsLeaveContinuous = "IsLeaveContinuous";
            public const string Terminate = "Terminate";
            public const string CodeEmp = "CodeEmp";
            public const string CodeAttendance = "CodeAttendance";
            public const string DayLeave = "DayLeave";
            public const string IsHoldSal = "IsHoldSal";
            public const string Status = "Status";
            public const string IsLeaveM = "IsLeaveM";
            public const string CatNameEntity = "CatNameEntity";
            public const string AmountSalaryAfterTax = "AmountSalaryAfterTax";

        }
    }


    public class Sal_HoldSalarySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_HoldSalary_IsExcludeQuitEmp)]
        public bool IsExcludeQuitEmp { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateQuit { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkingPlace { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
