using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_BonusSalaryEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public Guid? ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string Type { get; set; }
        public DateTime? Month { get; set; }
        public double? Bonus { get; set; }
        public Guid? SalesTypeID { get; set; }
        public string SalesTypeName { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureID { get; set; }
        public string Quarter { get; set; }
        public string JobTitleName { get; set; }
        public DateTime? DateMonth { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }

    }
}
