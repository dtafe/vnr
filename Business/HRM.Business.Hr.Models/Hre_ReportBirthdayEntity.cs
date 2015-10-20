using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportBirthdayEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public Guid ProfileID { get; set; }
        public string OrgStructureName { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public DateTime? DateHire { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string OrgStructureID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
    }
}
