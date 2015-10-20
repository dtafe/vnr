using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Kai_ReportKaizenDataMonthlyModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_KaizenName)]
        public string KaizenName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_Month)]
        public Nullable<System.DateTime> Month { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public string CategoryName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_MarkIdea)]
        public Nullable<double> MarkIdea { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_AmountIdea)]
        public Nullable<double> AmountIdea { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_MarkPerform)]
        public Nullable<double> MarkPerform { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_SumMark)]
        public Nullable<double> SumMark { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_SumAmount)]
        public Nullable<double> SumAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_Accumulate)]
        public Nullable<double> Accumulate { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public Guid ExportId { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string KaizenName = "KaizenName";
            public const string Month = "Month";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CategoryName = "CategoryName";
            public const string MarkIdea = "MarkIdea";
            public const string MarkPerform = "MarkPerform";
            public const string SumMark = "SumMark";
            public const string SumAmount = "SumAmount";
            public const string Accumulate = "Accumulate";
        }
    }
}
