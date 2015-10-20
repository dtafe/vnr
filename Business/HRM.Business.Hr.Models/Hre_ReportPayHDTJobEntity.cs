using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportPayHDTJobEntity : HRMBaseModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string OrgStructureIDs { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }
        public int? CountType4 { get; set; }
        public int? CountType5 { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string Dept { get; set; }
        public string Unit { get; set; }
        public string Part { get; set; }
        public string Line { get; set; }
        public string Session { get; set; }

    }
}
