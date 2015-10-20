using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Med_DiseaseEntity : HRMBaseModel
    {
        public string DiseaseName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
    public class Med_DiseaseMultiEntity
    {
        public Guid ID { get; set; }
        public string DiseaseName { get; set; }
    }
}
