using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_CostCentreMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string CostCentreName { get; set; }
        public string Code { get; set; } 

    }
    public class Cat_CostCentreEntity : HRM.Business.BaseModel.HRMBaseModel
  {
      
      public string CostCentreName { get; set; }

      public string Code { get; set; }

      public Guid? OrgStructureID { get; set; }

      public string OrgStructureName { get; set; }

      public string GroupCost { get; set; }

      public string Description { get; set; }

  }
       
   
}
