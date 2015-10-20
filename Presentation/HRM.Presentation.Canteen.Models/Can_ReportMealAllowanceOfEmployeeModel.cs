using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HRM.Presentation.Canteen.Models
{
    public class Can_ReportMealAllowanceOfEmployeeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public List<Guid?> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Line)]
        public List<Guid> LineID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceTypeSettingName)]
        public List<Guid> MealAllowanceTypeSettingID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public List<Guid> WorkPlaceID { get; set; }
        public int ExportID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_WorkLocationCode)]
        public string WorkLocationCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_WorkLocationHD)]
        public string WorkLocationHD { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_EatLoCationCode)]
        public string Quantity { get; set; }

        public partial class FieldNames
        {
            public const string WorkLocationCode = "WorkLocationCode";
            public const string WorkLocationHD = "WorkLocationHD";
            public const string EatLoCationCode = "EatLoCationCode";
            public const string EatLoCationHD = "EatLoCationHD";
            public const string Quantity = "Quantity";
            public const string OrgStructureName = "OrgStructureName";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";

            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";

            public const string CodeBranch = "CodeBranch";
            public const string CodeDepartment = "CodeDepartment";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";

            public const string MealAllowanceTypeSettingName = "MealAllowanceTypeSettingName";
            public const string WorkPlaceName = "WorkPlaceName";

            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
        }
    }

    public class Can_ReportMealAllowanceOfEmployeeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public List<Guid?> WorkPlaceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Can_MealAllowanceToMoney_MealAllowanceTypeSettingName)]
        public List<Guid?> MealAllowanceTypeSettingID { get; set; }

        public Guid ExportID { get; set; }
        public Boolean IsIncludeQuitEmp { get; set; }
    }
}
