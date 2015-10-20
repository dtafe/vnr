using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Canteen.Models
{
    public class Can_ReportHDTJobCardMoreEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public List<Guid?> CateringID { get; set; }
        public string CateringName { get; set; }
        public List<Guid?> CanteenID { get; set; }
        public string CanteenName { get; set; }
        public List<Guid?> LineID { get; set; }
        public string LineName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public List<Guid?> OrgStructureID { get; set; }
        public List<Guid?> LocationID { get; set; }
        public string OrgStructureName { get; set; }

        public partial class FieldNames
        {
            public const string WorkLocationCode = "WorkLocationCode";
            public const string WorkLocationHD = "WorkLocationHD";
            public const string EatLoCationCode = "EatLoCationCode";
            public const string EatLoCationHD = "EatLoCationHD";

            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Division = "Division";
            public const string Department = "Department";
            public const string Section = "Section";
            public const string CateringName = "CateringName";
            public const string CanteenName = "CanteenName";
            public const string LineName = "LineName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";

            public const string DateCard = "DateCard";
            public const string Amount = "Amount";
            public const string Hour = "Hour";
            public const string SumCardMore = "SumCardMore";
            public const string PriceStardand = "PriceStardand";
            public const string Total = "Total";
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
