using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_InsuranceConfigModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractTypeID)]
        public Nullable<System.Guid> ContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractTypeID)]
        public string ContractTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_FromMonth)]
        public Nullable<int> FromMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_ToMonth)]
        public Nullable<int> ToMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_IsSocialInsurance)]
        public Nullable<bool> IsSocial { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_IsHealthInsurance)]
        public Nullable<bool> IsHealth { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_IsUnEmployInsurance)]
        public Nullable<bool> IsUnEmploy { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ContractTypeName = "ContractTypeName";
            public const string FromMonth = "FromMonth";
            public const string ToMonth = "ToMonth";
            public const string IsSocial = "IsSocial";
            public const string IsHealth = "IsHealth";
            public const string IsUnEmploy = "IsUnEmploy";
        }
    }

}
