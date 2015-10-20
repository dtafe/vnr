using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HRM.Business.Canteen.Models
{
    public class Can_ReportMealAllowanceOfEmployeeEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public List<Guid?> WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public List<Guid?> MealAllowanceTypeSettingID { get; set; }
        public string MealAllowanceTypeSettingName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public List<Guid?> OrgStructureID { get; set; }
        public List<Guid?> LocationID { get; set; }
        public string OrgStructureName { get; set; }

        public partial class FieldNames
        {
            public const string WorkLocationCode = "WorkLocationCode";
            public const string WorkLocationHD = "WorkLocationHD";
            public const string Quantity = "Quantity";
            public const string Amount = "Amount";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string MealAllowanceTypeSettingName = "MealAllowanceTypeSettingName";
            public const string Division = "Division";
            public const string Department = "Department";
            public const string Section = "Section";
            public const string CateringName = "CateringName";
            public const string CanteenName = "CanteenName";
            public const string LineName = "LineName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";

            public const string MealAllowance = "MealAllowance";
            public const string ShiftActual = "ShiftActual";
            public const string ShiftApprove = "ShiftApprove";
            public const string Date = "Date";
            public const string Status = "Status";
            public const string DateExport = "DateExport";
            


            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";

            public const string CodeBranch = "CodeBranch";
            public const string CodeDepartment = "CodeDepartment";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";
        }
    }
}
