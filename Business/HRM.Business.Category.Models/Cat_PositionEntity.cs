using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_PositionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        
        public string PositionName { get; set; }

        public string Description { get; set; }
      
        public string Code { get; set; }

        public Guid? JobtitleID { get; set; }

        public string JobTitleName { get; set; }
      
        public string PositionEngName { get; set; }
        public Guid? OrgStructureID { get; set; }

        public string OrgStructureName { get; set; }

        public Guid? CostCentreID { get; set; }

        public string CostCentreName { get; set; }
    }
    public class Cat_PositionMultiEntity
    {
        public Guid ID { get; set; }
        public string PositionName { get; set; }
    } 
}
