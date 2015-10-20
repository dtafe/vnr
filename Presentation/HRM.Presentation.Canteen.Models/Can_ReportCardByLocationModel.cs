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
    public class Can_ReportCardByLocationModel : BaseViewModel
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

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_UserPrint)]
        public string UserPrint { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DatePrint)]
        public DateTime? DatePrint { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Catering)]
        public string CateringName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Catering)]
        public List<Guid?> CateringID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Canteen)]
        public string CanteenName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Canteen)]
        public List<Guid?> CanteenID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Line)]
        public string LineName { get; set; }
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
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string Data3 { get; set; }
        public string Data4 { get; set; }
        public string Data5 { get; set; }
        public string Data6 { get; set; }
        public string Data7 { get; set; }
        public string Data8 { get; set; }
        public string Data9 { get; set; }
        public string Data10 { get; set; }
        public string Data11 { get; set; }
        public string Data12 { get; set; }
        public string Data13 { get; set; }
        public string Data14 { get; set; }
        public string Data15 { get; set; }
        public string Data16 { get; set; }
        public string Data17 { get; set; }
        public string Data18 { get; set; }
        public string Data19 { get; set; }
        public string Data20 { get; set; }
        public string Data21 { get; set; }
        public string Data22 { get; set; }
        public string Data23 { get; set; }
        public string Data24 { get; set; }
        public string Data25 { get; set; }
        public string Data26 { get; set; }
        public string Data27 { get; set; }
        public string Data28 { get; set; }
        public string Data29 { get; set; }
        public string Data30 { get; set; }
        public string Data31 { get; set; }
        public int ExportID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_WorkLocationCode)]
        public string WorkLocationCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_WorkLocationHD)]
        public string WorkLocationHD { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_EatLoCationCode)]
        public string EatLoCationCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_EatLoCationHD)]
        public string EatLoCationHD { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Quantity)]
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
            public const string CateringName = "CateringName";
            public const string CanteenName = "CanteenName";
            public const string LineName = "LineName";
            public const string Data1 = "Data1";
            public const string Data2 = "Data2";
            public const string Data3 = "Data3";
            public const string Data4 = "Data4";
            public const string Data5 = "Data5";
            public const string Data6 = "Data6";
            public const string Data7 = "Data7";
            public const string Data8 = "Data8";
            public const string Data9 = "Data9";
            public const string Data10 = "Data10";
            public const string Data11 = "Data11";
            public const string Data12 = "Data12";
            public const string Data13 = "Data13";
            public const string Data14 = "Data14";
            public const string Data15 = "Data15";
            public const string Data16 = "Data16";
            public const string Data17 = "Data17";
            public const string Data18 = "Data18";
            public const string Data19 = "Data19";
            public const string Data20 = "Data20";
            public const string Data21 = "Data21";
            public const string Data22 = "Data22";
            public const string Data23 = "Data23";
            public const string Data24 = "Data24";
            public const string Data25 = "Data25";
            public const string Data26 = "Data26";
            public const string Data27 = "Data27";
            public const string Data28 = "Data28";
            public const string Data29 = "Data29";
            public const string Data30 = "Data30";
            public const string Data31 = "Data31";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";   
        }
    }
    public class Can_ReportCardByLocationSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public List<Guid?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Catering)]
        public List<Guid?> CateringID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Canteen)]
        public List<Guid?> CanteenID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public List<Guid?> workPlaceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_eatPlaceId)]
        public List<Guid?> eatPlaceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Line)]
        public List<Guid?> LineID { get; set; }
        public Guid ExportID { get; set; }
        public Boolean IsIncludeQuitEmp { get; set; }
    }
}
