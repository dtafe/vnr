using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Laundry.Models
{
    public class Lau_ReportEmpDetailModel : BaseViewModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? TimeLog { get; set; }
        public DateTime? Hour { get { return TimeLog; } }

        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }
        public string TeamCode { get; set; }
        public string SectionCode { get; set; }

        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }

        public Guid? OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }

        public Guid? MarkerID { get; set; }
        public string MarkerName { get; set; }
        public Guid? LockerID { get; set; }
        public string LockerName { get; set; }
        public Guid? LineID { get; set; }
        public string LineName { get; set; }

        public DateTime? Data1 { get; set; }
        public DateTime? Data2 { get; set; }
        public DateTime? Data3 { get; set; }
        public DateTime? Data4 { get; set; }
        public DateTime? Data5 { get; set; }
        public DateTime? Data6 { get; set; }
        public DateTime? Data7 { get; set; }
        public DateTime? Data8 { get; set; }
        public DateTime? Data9 { get; set; }
        public DateTime? Data10 { get; set; }
        public DateTime? Data11 { get; set; }
        public DateTime? Data12 { get; set; }
        public DateTime? Data13 { get; set; }
        public DateTime? Data14 { get; set; }
        public DateTime? Data15 { get; set; }
        public DateTime? Data16 { get; set; }
        public DateTime? Data17 { get; set; }
        public DateTime? Data18 { get; set; }
        public DateTime? Data19 { get; set; }
        public DateTime? Data20 { get; set; }
        public DateTime? Data21 { get; set; }
        public DateTime? Data22 { get; set; }
        public DateTime? Data23 { get; set; }
        public DateTime? Data24 { get; set; }
        public DateTime? Data25 { get; set; }
        public DateTime? Data26 { get; set; }
        public DateTime? Data27 { get; set; }
        public DateTime? Data28 { get; set; }
        public DateTime? Data29 { get; set; }
        public DateTime? Data30 { get; set; }
        public DateTime? Data31 { get; set; }
        public int? Total { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";

            public const string BranchCode = "BranchCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";

            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";

            public const string MarkerName = "MarkerName";
            public const string LockerName = "LockerName";
            public const string LineName = "LineName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";

            public const string TimeLog = "TimeLog";
            public const string Hour = "Hour";
            
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
            public const string Total = "Total";
        }

        
    }

    public class Lau_ReportEmpDetailSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileName)]
        public string ProfileIDs { get; set; }
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_Line_LineName)]
        public string LineID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_Marker_MarkerName)]
        public string MarkerID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_Locker_LockerName)]
        public string LockerID { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }

        public string selectedIDs { get; set; }
        public bool IsExport { get; set; }
        public int ExportID { get; set; }
    }
}
