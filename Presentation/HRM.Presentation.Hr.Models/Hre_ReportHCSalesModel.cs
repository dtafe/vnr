using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportHCSalesModel : BaseViewModel
    {
        public string WorkingPalceName { get; set; }
        public DateTime? DateHire { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string SalesTypeName { get; set; }
        public string ProfileSuperviorName { get; set; }
        public string JobTitleName { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_DateExport)]
        public DateTime DateExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public Guid OrgStructureID { get; set; }

        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_TypeID)]
        public Guid? OrgStructureTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_JobtitleID)]
        public List<Guid?> JobtitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_DateSearch)]
        public DateTime dateSearch { get; set; }

        public Guid ExportID { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Jan)]
        public string Jan { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Feb)]
        public string Feb { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Mar)]
        public string Mar { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Apr)]
        public string Apr { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_May)]
        public string tMay { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Jun)]
        public string Jun { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Jul)]
        public string Jul { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Aug)]
        public string Aug { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Sep)]
        public string Sep { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Oct)]
        public string Oct { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Nov)]
        public string Nov { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Dec)]
        public string Dec { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Year)]
        public string Year { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_DateSearch)]
        public int YearSearch { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
      

        public partial class FieldNames
        {
            public const string SalesTypeName = "SalesTypeName";
            public const string OrgStructureName = "OrgStructureName";
            public const string JobTitleName = "JobTitleName";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
            public const string MonthSearch = "MonthSearch";

            public const string Year = "Year";
            public const string Jan = "Jan";
            public const string Feb = "Feb";
            public const string Mar = "Mar";
            public const string Apr = "Apr";
            public const string May = "May";
            public const string Jun = "Jun";
            public const string Jul = "Jul";
            public const string Aug = "Aug";
            public const string Sep = "Sep";
            public const string Oct = "Oct";
            public const string Nov = "Nov";
            public const string Dec = "Dec";

           
        }
    }
}
