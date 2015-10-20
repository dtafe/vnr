using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_PayrollTableItemEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public System.Guid PayrollTableID { get; set; }
        public Guid? CutOffDurationID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ValueType { get; set; }
        public bool IsDecrypt { get; set; }
        public bool IsAddToHourlyRate { get; set; }
        public bool IsChargePIT { get; set; }
        public int OrderNo { get; set; }
        public string ElementType { get; set; }
        public Nullable<System.Guid> ParentID { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Description4 { get; set; }
        public string ProfileName { get; set; }
        public Guid? JobTitleID { get; set; }
        public Guid? ProfileID { get; set; }
        public Nullable<bool> IsShow { get; set; }

        public string CutOffDurationName { get; set; }

   
    }
}
