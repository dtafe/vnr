using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_GradeEntity : HRMBaseModel
    {
        public System.Guid ProfileID { get; set; }
        public string ProfileName { get; set; }

        public Guid? GradePayrollID { get; set; }
        public string GradeCfgName { get; set; }

        public DateTime? MonthStart { get; set; }
        public DateTime? MonthEnd { get; set; }
        public Nullable<int> SalaryDayClose { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
     
            public const string Note = "Note";
       
        }
    }
}
