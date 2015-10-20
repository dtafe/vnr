using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatContractTypeMultiModel 
    {
        public Guid ID { get; set; }
        public string ContractTypeName { get; set; }
        public string Type { get; set; }
    }
    public class CatContractTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_Type)]
        public string TypeView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_Code)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_ContractType_Code_StringLength)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        [StringLength(200, ErrorMessage = ConstantDisplay.HRM_Category_ContractType_ContractTypeName_StringLength)]
        public string ContractTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_Type)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_ContractType_Type_StringLength)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_UnitTime)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_ContractType_UnitTime_StringLength)]
        public string UnitTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_UnitTime)]
        //[StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_ContractType_UnitTime_StringLength)]
        public string UnitTimeView { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ValueTime)]
        public double? ValueTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractNextID)]
        public string ContractNextID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractNextID)]
        public string ContractNextName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_Description)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_ContractType_Description_StringLength)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ExpiryContractLoop)]
        public Nullable<int> ExpiryContractLoop { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_DayInMonthLoop1)]
        public Nullable<int> DayInMonthLoop1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_DayInMonthLoop2)]
        public Nullable<int> DayInMonthLoop2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_DayInMonthLoop3)]
        public Nullable<int> DayInMonthLoop3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ExpiryContractDayByDay)]
        public Nullable<int> ExpiryContractDayByDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_EmailToList)]
        public string EmailToList { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_EmailToList)]
        public string EmailOther { get; set; }


        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_ExportID)]
        public Nullable<System.Guid> ExportID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ExportItem_ExportID)]
        public string ExportName { get; set; }
        public Guid? ORGSTRUCTURETYPEID { get; set; }
        public string OrgStructureTypeName { get; set; }
        public string OrgStructureTypeCode { get; set; }
        public string OrgStructureTypeIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_Formula)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ComputeEndDateByContractType)]
        public Nullable<bool> ComputeEndDateByContractType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ComputeEndDateByFomular)]
        public Nullable<bool> ComputeEndDateByFomular { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ComputeEndDateByContractType)]
        public Nullable<bool> NoneEndContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_NoneTypeInsuarance)]
        public Nullable<bool> NoneTypeInsuarance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_NoneTypeInsuaranceAdvance)]
        public Nullable<bool> NoneTypeInsuaranceAdvance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_IsSocialInsurance)]
        public Nullable<bool> IsSocialInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_IsHealthInsurance)]
        public Nullable<bool> IsHealthInsurance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_IsUnEmployInsurance)]
        public Nullable<bool> IsUnEmployInsurance { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string ContractTypeName = "ContractTypeName";
            public const string Type = "Type";
            public const string UnitTime = "UnitTime";
            public const string UnitTimeView = "UnitTimeView";
            public const string ValueTime = "ValueTime";
            public const string ContractNextID = "ContractNextID";
            public const string ContractNextName = "ContractNextName";
            public const string Description = "Description";
            public const string ExportID = "ExportID";
            public const string ExportName = "ExportName";
            public const string TypeView = "TypeView";
        }
    }

    public class CatContractTypeSearchModel : BaseViewSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public string ContractTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_Code)]
        public string ContractTypeCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_Type)]
        public string Type { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string ContractTypeName = "ContractTypeName";
            public const string Type = "Type";
        }
    }
}
