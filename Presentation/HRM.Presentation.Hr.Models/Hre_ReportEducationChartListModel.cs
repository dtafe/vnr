using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportEducationChartListModel : BaseViewModel
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
        public string EducationLevel { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_ProfileId)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_ProfileId)]
        public Guid? EducationID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hr_ReportEducationChar_EducationIDs)]
        public List<Guid?> EducationIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureName { get; set; }
    [DisplayName(ConstantDisplay.HRM_Hr_ReportEducationChar_ProfileCount)]
        public int ProfileCount { get; set; }

        [DisplayName(ConstantDisplay.HRM_Hr_ReportEducationChar_AppliedForThisPeriod)]
        public bool AppliedForThisPeriod { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureID { get; set; }


        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsChecked { get; set; }

        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureID = "OrgStructureID";
            public const string CodeEmp = "CodeEmp";
            public const string Education = "Education";
            public const string ProfileCount = "ProfileCount";
            public const string EducationLevel = "EducationLevel";

        }
    }
}
