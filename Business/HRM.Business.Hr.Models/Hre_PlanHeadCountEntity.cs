using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_PlanHeadCountEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }

        public string PlanName { get; set; }

        public Guid? OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public Guid? JobTitleID { get; set; }
        public string JobTitleName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? HrPlanHC { get; set; }
        public string Note { get; set; }
        public Nullable<System.Guid> PostionID { get; set; }
        public string PositionName { get; set; }
    }
}
