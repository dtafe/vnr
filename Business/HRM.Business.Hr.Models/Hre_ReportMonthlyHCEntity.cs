using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportMonthlyHCEntity : HRMBaseModel
    {
        public string Name { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureParentName { get; set; }

        public DateTime DateExport { get; set; }

        public string UserExport { get; set; }


        public Guid ExportID { get; set; }


        public string JobTitleName { get; set; }

        public string Months { get; set; }

        public string AppYear { get; set; }

        public string AppJan { get; set; }
        public string AppFeb { get; set; }
        public string AppMar { get; set; }
        public string AppApr { get; set; }
        public string AppMay { get; set; }
        public string AppJun { get; set; }
        public string AppJul { get; set; }
        public string AppAug { get; set; }
        public string AppSep { get; set; }
        public string AppOct { get; set; }
        public string AppNov { get; set; }
        public string AppDec { get; set; }

        public string ActJan { get; set; }
        public string ActFeb { get; set; }
        public string ActMar { get; set; }
        public string ActApr { get; set; }
        public string ActMay { get; set; }
        public string ActJun { get; set; }
        public string ActJul { get; set; }
        public string ActAug { get; set; }
        public string ActSep { get; set; }
        public string ActOct { get; set; }
        public string ActNov { get; set; }
        public string ActDec { get; set; }
        public string ActYear { get; set; }

        public string LeaJan { get; set; }
        public string LeaFeb { get; set; }
        public string LeaMar { get; set; }
        public string LeaApr { get; set; }
        public string LeaMay { get; set; }
        public string LeaJun { get; set; }
        public string LeaJul { get; set; }
        public string LeaAug { get; set; }
        public string LeaSep { get; set; }
        public string LeaOct { get; set; }
        public string LeaNov { get; set; }
        public string LeaDec { get; set; }
        public string LeaYear { get; set; }
        public partial class FieldNames
        {
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureParentName = "OrgStructureParentName";
            public const string Name = "Name";

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
           
            public const string JobTitleName = "JobTitleName";

            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
        }
    }
}
