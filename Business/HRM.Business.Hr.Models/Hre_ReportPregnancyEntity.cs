using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportPregnancyEntity : HRMBaseModel
    {
        
        public string ProfileName { get; set; }

        public string CodeEmp { get; set; }

        public Guid? OrgID { get; set; }
        public string OrgStructureID { get; set; }

        public string OrgStructureName { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public Guid? PositionID { get; set; }

        public string PositionName { get; set; }

        public Guid? JobTitleID { get; set; }

        public string JobTitleName { get; set; }

        public string Type { get; set; }

        public Guid ExportID { get; set; }

        public string NameFamily { get; set; }
        public string FirstName { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Notes { get; set; }

    }
}
