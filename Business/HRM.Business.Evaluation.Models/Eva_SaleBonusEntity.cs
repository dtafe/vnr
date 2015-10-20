using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_SaleBonusEntity : HRMBaseModel
    {
        public Guid? JobTittleID { get; set; }
        public string JobTitleName { get; set; }
        public Guid? SalesTypeID { get; set; }
        public string SalesTypeName { get; set; }
        public string Type { get; set; }
        public DateTime? DateOfEffect { get; set; }
        public double? FromPercent { get; set; }
        public double? ToPercent { get; set; }
        public double? Amount { get; set; }
        public string Note { get; set; }
    }
}
