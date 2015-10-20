using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_UsualAllowanceLevelModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_UsualAllowanceName)]

        public System.Guid AllowanceID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_UsualAllowanceName)]
        public string UsualAllowanceName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_UsualAllowanceLevel_Name)]
        public string UsualAllowanceLevelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowanceLevel_Amount)]
        public double Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Currency)]
        public System.Guid CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Currency)]
        public string CurrencyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowanceLevel_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowanceLevel_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowanceLevel_MonthApply)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? MonthApply { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        private Guid _id = Guid.Empty;
        public Guid UsualAllowanceLevel_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string UsualAllowanceName = "UsualAllowanceName";
            public const string UsualAllowanceLevelName = "UsualAllowanceLevelName";
            public const string Amount = "Amount";
            public const string CurrencyName = "CurrencyName";
            public const string Comment = "Comment";
            public const string Code = "Code";
            public const string MonthApply = "MonthApply";
            public const string UserCreate = "UserCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

   

    
}
