using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_PayrollTableItemModel : BaseViewModel
    {
        public System.Guid PayrollTableID { get; set; }
        public Guid? CutOffDurationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_TableItem_Name)]
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
        [DisplayName(ConstantDisplay.HRM_Payroll_TableItem_Value)]
        public string Value { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_TableItem_Description)]
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Description4 { get; set; }
        public string ProfileName { get; set; }
        public int TotalRow { get; set; }
        public double Amount { get; set; }
        public Guid? JobTitleID { get; set; }
        public Guid? ProfileID { get; set; }
        public Nullable<bool> IsShow { get; set; }

        public partial class FieldNames
        {
            public const string PayrollTableID ="PayrollTableID";   
            public const string Name ="Name";
            public const string Code ="Code";
            public const string ValueType ="ValueType";
            public const string IsDecrypt ="IsDecrypt";
            public const string IsAddToHourlyRate ="IsAddToHourlyRate";
            public const string IsChargePIT ="IsChargePIT";
            public const string OrderNo ="OrderNo";
            public const string ElementType ="ElementType";
            public const string ParentID ="ParentID";
            public const string MonthYear ="MonthYear";
            public const string Value ="Value";
            public const string Description1 ="Description1";
            public const string Description2 ="Description2";
            public const string Description3 ="Description3";
            public const string Description4 = "Description4";
            public const string ProfileName = "ProfileName";
        }
    }

    public class Sal_PayrollTableItemModelSearch
    {
        public Guid ProfileID { get; set; }
        public Guid? CutOffDurationID { get; set; }
        
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
    }

    public class Sal_PayrollTableItemModelExportPDF
    {
        public Guid ProfileID { get; set; }
        public Guid? CutOffDurationID { get; set; }
        public bool? ExportPDF { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
    }

}
