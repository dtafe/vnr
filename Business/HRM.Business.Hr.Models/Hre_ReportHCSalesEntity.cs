using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportHCSalesEntity : HRMBaseModel
    {
        public string Name { get; set; }
        public string SalesTypeName { get; set; }

        public string Channel { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public DateTime DateExport { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string TargetSalesIn { get; set; }
        public string ActSalesIn { get; set; }
        public string SalesIn { get; set; }
        public string TargetSalesOut { get; set; }
        public string ActSalesOut { get; set; }
        public string SalesOut { get; set; }
        public string ProfileSupervisorName { get; set; }
        public string UserExport { get; set; }

        public Guid ExportID { get; set; }

        public string PositionName { get; set; }

        public string WorkingPlaceName { get; set; }
        public string OrgStructureTypeName { get; set; }
        public DateTime? DateHire { get; set; }

        public string Gender { get; set; }
        public string OrgStructureName { get; set; }

        public string JobTitleName { get; set; }
        public string Total { get; set; }

        public string TotalColumn { get; set; }

        public string HeadCount { get; set; }
        public string Year { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public partial class FieldNames
        {
            public const string WorkingPlaceName = "WorkingPlaceName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string SalesTypeName = "SalesTypeName";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string TargetSalesIn = "TargetSalesIn";
            public const string ActSalesIn = "ActSalesIn";
            public const string SalesIn = "SalesIn";
            public const string TargetSalesOut = "TargetSalesOut";
            public const string ActSalesOut = "ActSalesOut";
            public const string SalesOut = "SalesOut";
            public const string ProfileSupervisorName = "ProfileSupervisorName";
            public const string DateHire = "DateHire";
            public const string Area = "Area";
            public const string Channel = "Channel";
            public const string Region = "Region";
            public const string HeadCount = "HeadCount";
            public const string Total = "Total";
            public const string Gender = "Gender";
            public const string TotalColumn = "TotalColumn";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";

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
