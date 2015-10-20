using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportBirthdayModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_ProfileId)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureName { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_PositionName)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_DateHire)]
        public DateTime? DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_PlaceOfBirth)]
        public string PlaceOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_DayOfBirth)]
        public DateTime? DateOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsChecked { get; set; }

        public Guid ExportID { get; set; }
        //public bool IsCreateTemplate { get; set; }
        //public bool IsCreateTemplateForDynamicGrid { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public string WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ReportProfile_DateQuit)]
        public DateTime? DateQuit { get; set; }
        public DateTime? DateQuitFrom { get; set; }
        public DateTime? DateQuitTo { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureID = "OrgStructureID";
            public const string PositionName = "PositionName";
            public const string DateHire = "DateHire";
            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string DateOfBirth = "DateOfBirth";
            public const string CodeEmp = "CodeEmp";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
        }
    }
}
