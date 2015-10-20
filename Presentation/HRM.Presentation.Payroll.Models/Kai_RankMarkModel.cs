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
    public class Kai_RankMarkModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Kai_RankMark_MarkArea)]
        public string MarkArea { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_RankMark_MarkIdea)]
        public Nullable<double> MarkIdea { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_RankMark_AmountIdea)]
        public Nullable<double> AmountIdea { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_RankMark_MarkPerform)]
        public Nullable<double> MarkPerform { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_RankMark_AmountPerform)]
        public Nullable<double> AmountPerform { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_RankMark_TotalAmount)]
        public Nullable<double> TotalAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_RankMark_Accumulate)]
        public Nullable<double> Accumulate { get; set; }
        public partial class FieldNames
        {
            public const string MarkArea = "MarkArea";
            public const string MarkIdea = "MarkIdea";
            public const string AmountIdea = "AmountIdea";
            public const string MarkPerform = "MarkPerform";
            public const string AmountPerform = "AmountPerform";
            public const string TotalAmount = "TotalAmount";
            public const string Accumulate = "Accumulate";
        }
    }
    public class Kai_RankMarkSearchModel
    {
        //[DisplayName(ConstantDisplay.HRM_Kai_RankMark_MarkIdea)]
        //public Nullable<double> MarkIdea { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Kai_RankMark_MarkPerform)]
        //public Nullable<double> MarkPerform { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_RankMark_MarkArea)]
        public string MarkArea { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
