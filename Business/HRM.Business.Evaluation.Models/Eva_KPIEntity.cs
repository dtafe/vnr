using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_KPIEntity : HRMBaseModel
    {
        public System.Guid? NameEntityID { get; set; }
        public string NameEntityObject { get; set; }
        public double MinimumRating { get; set; }
        public double MaximumRating { get; set; }
        public double Rate { get; set; }
        public string Level { get; set; }
        public string Comment { get; set; }
        public string Code { get; set; }
        public string KPIName { get; set; }
        public string MeasuredSource { get; set; }
        public string Scale { get; set; }
        public int? OrderNumber { get; set; }
        public int? Stt { get; set; }
        public string NameEntityName { get; set; }
        public double? Mark { get; set; }
        public string Evaluation { get; set; }
        public string DescriptionKPIFix { get; set; }
        public string DescriptionKP { get; set; }
        public Guid? PerformanceForDetailID { get; set; }
        public bool? IsKPIFix { get; set; }
        public string KPIColor { get; set; }
    }
    public class Eva_KPIMultiEntity
    {
        public Guid ID { get; set; }
        public string KPIName { get; set; }
    } 
   
}
