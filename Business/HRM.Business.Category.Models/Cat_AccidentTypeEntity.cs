using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Category.Models
{
    public class Cat_AccidentTypeEntity : HRMBaseModel
    {
        public Guid ID { get; set; }
        public string AccidentTypeName { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
    }
    public class Cat_AccidentTypeMultiEntity
    {
        public Guid ID { get; set; }
        public string AccidentTypeName { get; set; }
    }
}
