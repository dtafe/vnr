using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Canteen.Models
{
    public class Can_SumryMealRecordEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public Nullable<double> TotalMealAllowance { get; set; }
        public Nullable<double> SumAmount { get; set; }
        public Nullable<double> CountEatNotStandar { get; set; }
        public Nullable<double> AmountEatNotStandar { get; set; }
        public Nullable<double> CountCardMore { get; set; }
        public Nullable<double> SumAmountCardMore { get; set; }
        public Nullable<double> CountNotWorkHasEat { get; set; }
        public Nullable<double> AmountNotWorkHasEat { get; set; }
        public Nullable<double> Amount3OnMonth { get; set; }
        public Nullable<double> CountHDTJob { get; set; }
        public Nullable<double> AmountHDTJob { get; set; }
        public Nullable<double> CountNotWorkButHasHDT { get; set; }
        public Nullable<double> AmountNotWorkButHasHDT { get; set; }
        public Nullable<double> CountSubtractWorngStandarHDT { get; set; }
        public Nullable<double> AmountSubtractWorngStandarHDT { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string CodeEmp { get; set; }
    
    }
}
