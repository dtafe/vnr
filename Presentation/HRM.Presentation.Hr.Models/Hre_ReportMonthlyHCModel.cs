using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportMonthlyHCModel : BaseViewModel
    {
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_DateExport)]
        public DateTime DateExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public Guid OrgStructureID { get; set; }
        public string OrgStructureParentName { get; set; }
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_TypeID)]
        public Guid? OrgStructureTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_JobtitleID)]
        public List<Guid?> JobtitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_DateSearch)]
        public DateTime dateSearch { get; set; }

        public Guid ExportID { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Jan)]
        public int? ActJan { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Feb)]
        public int? ActFeb { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Mar)]
        public int? ActMar { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Apr)]
        public int? ActApr { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_May)]
        public int? ActMay { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Jun)]
        public int? ActJun { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Jul)]
        public int? ActJul { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Aug)]
        public int? ActAug { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Sep)]
        public int? ActSep { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Oct)]
        public int? ActOct { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Nov)]
        public int? ActNov { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Dec)]
        public int? ActDec { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Act_Year)]
        public int? ActYear { get; set; }


       [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Year)]
        public int? AppYear { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Jan)]
       public int? AppJan { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Feb)]
         public int? AppFeb { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Mar)]
        public int? AppMar { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Apr)]
        public int? AppApr { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_May)]
        public int? AppMay { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Jun)]
        public int? AppJun { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Jul)]
        public int? AppJul { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Aug)]
        public int? AppAug { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Sep)]
        public int? AppSep { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Oct)]
        public int? AppOct { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Nov)]
        public int? AppNov { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_App_Dec)]
        public int? AppDec { get; set; }
        

        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Jan)]
        public int? LeaJan { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Feb)]
        public int? LeaFeb { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Mar)]
        public int? LeaMar { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Apr)]
        public int? LeaApr { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_May)]
        public int? LeaMay { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Jun)]
        public int? LeaJun { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Jul)]
        public int? LeaJul { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Aug)]
        public int? LeaAug { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Sep)]
        public int? LeaSep { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Oct)]
        public int? LeaOct { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Nov)]
        public int? LeaNov { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Dec)]
        public int? LeaDec { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportMonthlyHC_Lea_Year)]
        public int? LeaYear { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public partial class FieldNames
        {
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureParentName = "OrgStructureParentName";
            public const string JobTitleName = "JobTitleName";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
            public const string MonthSearch = "MonthSearch";

            public const string ActYear = "ActYear";
            public const string ActJan = "ActJan";
            public const string ActFeb = "ActFeb";
            public const string ActMar = "ActMar";
            public const string ActApr = "ActApr";
            public const string ActMay = "ActMay";
            public const string ActJun = "ActJun";
            public const string ActJul = "ActJul";
            public const string ActAug = "ActAug";
            public const string ActSep = "ActSep";
            public const string ActOct = "ActOct";
            public const string ActNov = "ActNov";
            public const string ActDec = "ActDec";

            public const string AppYear = "AppYear";
            public const string AppJan = "AppJan";
            public const string AppFeb = "AppFeb";
            public const string AppMar = "AppMar";
            public const string AppApr = "AppApr";
            public const string AppMay = "AppMay";
            public const string AppJun = "AppJun";
            public const string AppJul = "AppJul";
            public const string AppAug = "AppAug";
            public const string AppSep = "AppSep";
            public const string AppOct = "AppOct";
            public const string AppNov = "AppNov";
            public const string AppDec = "AppDec";

            public const string LeaYear ="LeaYear";
            public const string LeaJan = "LeaJan";
            public const string LeaFeb = "LeaFeb";
            public const string LeaMar = "LeaMar";
            public const string LeaApr = "LeaApr";
            public const string LeaMay = "LeaMay";
            public const string LeaJun = "LeaJun";
            public const string LeaJul = "LeaJul";
            public const string LeaAug = "LeaAug";
            public const string LeaSep = "LeaSep";
            public const string LeaOct = "LeaOct";
            public const string LeaNov = "LeaNov";
            public const string LeaDec = "LeaDec";
        }
    }
}
