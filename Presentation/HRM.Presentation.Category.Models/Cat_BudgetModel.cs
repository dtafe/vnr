using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_BudgetModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Budget_BudgetName)]
        public string BudgetName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Budget_Description)]
        public string Description { get; set; }
        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string BudgetName = "BudgetName";
            public const string Description = "Description";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_BudgetSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Bank_BankName)]
        public string BudgetName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_BudgetMultiModel
    {
        public Guid ID { get; set; }
        public string BudgetName { get; set; }
        public int TotalRow { get; set; }
    }

}
