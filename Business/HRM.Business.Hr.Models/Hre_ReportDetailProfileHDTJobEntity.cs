using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportDetailProfileHDTJobEntity : HRMBaseModel
    {
        public DateTime? DateSearch { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string OrgStructureID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string Dept { get; set; }
        public string HDTJobTypeCode { get; set; }
        public string HDTJobTypeName { get; set; }
        public double? Price { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string HDTJobTypeCodeOld { get; set; }
        public string HDTJobTypeNameOld { get; set; }
        public double? PriceOld { get; set; }

    }
}
