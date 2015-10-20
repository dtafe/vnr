using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_HoldSalaryEntity : HRMBaseModel
    {
        public string StatusTranslate { get; set; }
        public System.Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string CodeAttendance { get; set; }
        public string ProfileName { get; set; }
        public string IDNo { get; set; }
        public string WorkPlaceName { get; set; }
        public string EmployeeTypeName { get; set; }
        public DateTime? MonthSalary { get; set; }
        public int? DayLeave { get; set; }
        public bool? IsLeaveContinuous { get; set; }
        public bool? Terminate { get; set; }
        public bool? IsHoldSal { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> MonthEndSalary { get; set; }
        public Nullable<double> AmountSalary { get; set; }
        public DateTime? DateComeBack { get; set; }
        public bool? IsLeaveM { get; set; }
        public Guid? TimeAnalyzeID { get; set; }
        public double? AmountSalaryAfterTax { get; set; }
        public string CatNameEntity { get; set; }
       
    }
}
