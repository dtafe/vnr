using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportHCSeniorityEntity : HRMBaseModel
    {
        public string Type { get; set; }

        public DateTime DateExport { get; set; }

        public string UserExport { get; set; }

        public Guid ExportID { get; set; }

        public string OrgStructureName { get; set; }

        public string Total { get; set; }

        public string HeadCount { get; set; }

        public partial class FieldNames
        {
            public const string HeadCount = "HeadCount";
            public const string Type = "Type";
            public const string Total = "Total";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
        }
    }
}
