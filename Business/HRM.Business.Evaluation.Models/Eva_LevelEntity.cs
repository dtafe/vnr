using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_LevelEntity : HRMBaseModel
    {
        public string LevelName { get; set; }
        public double? MinimumRating { get; set; }
        public double? MaximumRating { get; set; }
        public string Comment { get; set; }
        public Guid? PerformanceTypeID { get; set; }
        public string PerformanceTypeName { get; set; }
        public int TotalRow { get; set; }
        
    }
    public class Eva_LevelMultiEntity
    {
        public Guid ID { get; set; }
        public string LevelName { get; set; }
    }
}
