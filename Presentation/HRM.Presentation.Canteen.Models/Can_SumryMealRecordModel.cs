using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_SumryMealRecordModel : BaseViewModel
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
        public partial class FieldNames{
        
           public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string AmountSubtractWorngStandarHDT = "AmountSubtractWorngStandarHDT";
            public const string CountSubtractWorngStandarHDT = "CountSubtractWorngStandarHDT";
            public const string AmountNotWorkButHasHDT = "AmountNotWorkButHasHDT";
            public const string AmountHDTJob = "AmountHDTJob";
            public const string CountHDTJob = "CountHDTJob";
            public const string Amount3OnMonth = "Amount3OnMonth";
            public const string AmountNotWorkHasEat = "AmountNotWorkHasEat";
            public const string CountNotWorkHasEat = "CountNotWorkHasEat";
            public const string SumAmountCardMore = "SumAmountCardMore";
            public const string CountCardMore = "CountCardMore";
            public const string AmountEatNotStandar = "AmountEatNotStandar";
            public const string CountEatNotStandar = "CountEatNotStandar";
            public const string SumAmount = "SumAmount";
            public const string TotalMealAllowance = "TotalMealAllowance";
        }
    }
    public class Can_SumryMealRecordSearchModel
    {
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public Guid? CutoffdurationID { get; set; }
        public string OrgStructure { get; set; }
    }
}
