using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_SaleBonusModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleBonus_JobTitle)]
        public Guid? JobTittleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleBonus_JobTitle)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SalesType_SalesTypeName)]
        public Guid? SalesTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SalesType_SalesTypeName)]
        public string SalesTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleBonus_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleBonus_DateOfEffect)]
        public DateTime? DateOfEffect { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleBonus_FromPercent)]
        public double? FromPercent { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleBonus_ToPercent)]
        public double? ToPercent { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_SaleBonus_Amount)]
        public double? Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_Note)]
        public string Note { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string JobTitleName = "JobTitleName";
            public const string SalesTypeName = "SalesTypeName";
            public const string Type = "Type";
            public const string DateOfEffect = "DateOfEffect";
            public const string FromPercent = "FromPercent";
            public const string ToPercent = "ToPercent";
            public const string Amount = "Amount";
            public const string Note = "Note";
        }
    }

    public class Eva_SaleBonusSearchModel
    {
       [DisplayName(ConstantDisplay.HRM_Evaluation_SalesType_SalesTypeName)]
        public string SalesTypeName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
