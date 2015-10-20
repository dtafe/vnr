using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportHistoryProfileEntity : HRMBaseModel
    {
        public DateTime? DateHireFrom { get; set; }

        public DateTime? DateHireTo { get; set; }
        public DateTime? DateQuitFrom { get; set; }

        public DateTime? DateQuitTo { get; set; }

        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }
        public DateTime? CurrentDateHire { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public string CurrentPositionName { get; set; }

        public DateTime? DateHire { get; set; }
        public DateTime? DateQuit { get; set; }

        public string OrgStructureID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


    }
}
