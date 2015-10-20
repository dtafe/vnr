using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportHCGenderEntity : HRMBaseModel
    {
        public string Name { get; set; }

        public DateTime DateExport { get; set; }

        public string UserExport { get; set; }

        public Guid ExportID { get; set; }

        public string PositionName { get; set; }

        public string Gender { get; set; }
        public string OrgStructureName { get; set; }

        public string JobTitleName { get; set; }
        public string Total { get; set; }

        public string TotalColumn { get; set; }

        public string HeadCount { get; set; }
        public partial class FieldNames
        {
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string HeadCount = "HeadCount";
            public const string Total = "Total";
            public const string Gender = "Gender";
            public const string TotalColumn = "TotalColumn";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
        }
    }
}
