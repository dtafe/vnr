using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatOrgStructureMultiModel 
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string OrgStructureName { get; set; }
    }
   
    public class CatOrgStructureSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_TypeID)]
        public Guid? OrgStructureTypeID { get; set; }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class OrgStructureParentAndChild
    {
        public int ParentID { get; set; }
        public int CountChild { get; set; }
        public List<int> Childs { get; set; }
    }
    public class CatOrgStructureModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgFullName)]
        public string OrgFullName { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_Fax)]
        public string Fax { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Relatives_Phone)]
        public string Phone { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureName { get; set; }
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureNameEN)]
        public string OrgStructureNameEN { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_IsRoot)]
        public bool IsRoot { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_Description)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_TypeID)]
        public Guid? TypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_ParentID)]
        public Guid? ParentID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrderNumber)]
        public int OrderNumber { get; set; }

        public string OrgStructureTypeName { get; set; }
        public string Icon { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_OrgStructureTypeName)]
        public Guid? OrgStructureTypeID { get; set; }

        public string OrgStructureParentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_ProfileCount)]
        public int ProfileCount { get; set; }


        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_ProfileIsWorking)]
        public int ProfileIsWorking { get; set; }

        public Guid ExportID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_FEMALE)]
        public int FEMALE { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructureType_MALE)]
        public int MALE { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_CostCentreID)]
        public Nullable<System.Guid> GroupCostCentreID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_CostCentreID)]
        public string CostCentreName { get; set; }
        public bool? IsShow { get; set; }
        private Guid _id = Guid.Empty;
        public Guid OrgStructure_ID
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
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_RegionName)]
        public string RegionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_RegionID)]
        public Guid? RegionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_ServicesType)]
        public string ServicesType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_ContractFrom)]
        public Nullable<System.DateTime> ContractFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_ContractTo)]
        public Nullable<System.DateTime> ContractTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_BillingCompanyName)]
        public string BillingCompanyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_BillingAddress)]
        public string BillingAddress { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_TaxCode)]
        public string TaxCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_DurationPay)]
        public string DurationPay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_RecipientInvoice)]
        public string RecipientInvoice { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_TelePhone)]
        public string TelePhone { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_CellPhone)]
        public string CellPhone { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_Description)]
        public string DescriptionInfo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_Email)]
        public string EmailInfo { get; set; }
        public Guid? OrgMoreInforID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlanHeadCount_HrPlanHC)]
        public int? HrPlanHC { get; set; }
        public partial class FieldNames
        {
            public const string HrPlanHC = "HrPlanHC";
            public const string ID = "ID";
            public const string OrgStructureTypeName = "OrgStructureTypeName";
            public const string OrgStructureName = "OrgStructureName";
            public const string Code = "Code";
            public const string IsRoot = "IsRoot";
            public const string Description = "Description";
            public const string OrgStructureParentName = "OrgStructureParentName";
            public const string TypeID = "TypeID";
            public const string ParentID = "ParentID";
            public const string OrderNumber = "OrderNumber";
            public const string OrgStructureTypeID = "OrgStructureTypeID";
            public const string ProfileCount = "ProfileCount";
            public const string ProfileIsWorking = "ProfileIsWorking";
            public const string FEMALE = "FEMALE";
            public const string MALE = "MALE";
            public const string GroupCostCentreID = "GroupCostCentreID";
            public const string CostCentreName = "CostCentreName";
        }
    }
}
