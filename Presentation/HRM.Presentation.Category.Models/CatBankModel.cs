using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class TestAttribute : Attribute
    {
        public string Description
        {
            get;
            set;
        }

        public TestAttribute(string description)
        {
            Description = description;
        }
    }

    public class CatBankModel:BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Bank_BankName)]
        public string BankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Bank_BankCode)]
        public string BankCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Bank_Notes)]
        public string Notes { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Bank_CompBankCode)]
        public string CompBankCode { get; set; }

        [Test("test123")]
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string BankName = "BankName";
            public const string BankCode = "BankCode";
            public const string Notes = "Notes";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class CatBankSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Bank_BankName)]
        public string BankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Bank_BankCode)]
        public string BankCode { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class CatBankMultiModel
    {
        public Guid ID { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public int TotalRow { get; set; }
    }

}
