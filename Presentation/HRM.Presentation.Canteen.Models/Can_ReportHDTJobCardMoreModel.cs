﻿using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_ReportHDTJobCardMoreModel : BaseViewModel
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
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
        }
    }

    public class Can_ReportHDTJobCardMoreSearchModel
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
