using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Sal_ReportCostCentreByOrgEntity : HRMBaseModel
    {
        public string CostCentreCode { get; set; }
        public string ElementName { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureCode { get; set; }

        public double Total { get; set; }
        public partial class FieldNames
        {
            public const string CostCentreCode = "CostCentreCode";
            public const string ElementName = "ElementName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureCode = "OrgStructureCode";
           
        }
    }
}
