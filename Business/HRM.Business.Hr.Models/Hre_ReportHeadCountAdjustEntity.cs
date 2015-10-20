using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportHeadCountAdjustEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public Guid ProfileID { get; set; }
        public string EducationLevel { get; set; }
        public int ProfileCountQuit { get; set; }
        public int ProfileCountHire { get; set; }
        public string OrgStructureID { get; set; }

    }
}
