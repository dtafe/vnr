using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Kai_RankMarkEntity : HRMBaseModel
    {
        public string MarkArea { get; set; }
        public double? MarkIdea { get; set; }
        public double? MarkPerform { get; set; }   
        public double? AmountIdea { get; set; }
        public double? AmountPerform { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public Nullable<double> Accumulate { get; set; }
      
        
    }
}      
       