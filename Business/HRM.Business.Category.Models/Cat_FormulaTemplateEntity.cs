using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Category.Models
{
    public class Cat_FormulaTemplateEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string ElementCode { get; set; }
        public string ElementName { get; set; }
        public Guid? ColumnID { get; set; }
        public string Formula { get; set; }
        public long? OrderNumber { get; set; }
        public bool? Invisible { get; set; }
        public bool? IsBold { get; set; }
        public long? DisplayIndex { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string TabType { get; set; }
        public string ColumnName { get; set; }
        public Nullable<System.Guid> GradeID { get; set; }
        public string GradeCfgName { get; set; }
        public string ElementType { get; set; }
        public string Value { get; set; }
        public double? Amount { get; set; }

        public string ElementLevel { get; set; }

        public string MethodPayroll { get; set; }
    }
}
