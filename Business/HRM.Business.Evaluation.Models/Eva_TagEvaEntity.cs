using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_TagEvaEntity : HRMBaseModel
    {
        public string TagEvaName { get; set; }
        public byte[] EntityType { get; set; }
        
    }
    public class Eva_TagEvaMultiEntity
    {
        public Guid ID { get; set; }
        public string TagEvaName { get; set; }
    }
}
