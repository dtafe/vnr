using System;

namespace HRM.Business.Category.Models
{
    public class Cat_OrgStructureMultiEntity
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string OrgStructureName { get; set; }
    }
    public class Cat_OrgStructureTreeViewEntity
    {
        public Guid ID{ get; set; }
        public string OrgStructureName { get; set; }
        public int OrderNumber { get; set; }
        public Guid? ParentID { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public bool? IsShow { get; set; }
        public int ProfileCount { get; set; }
        public int ProfileIsWorking { get; set; }
        public Guid COLUMN_VALUE { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


    }
    public class Cat_OrgStructureEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string RegionName { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public int TotalRow { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureID { get; set; }
        public string OrgStructureTypeName { get; set; }
        public string OrgStructureNameEN { get; set; }
        public string Code { get; set; }
        public bool IsRoot { get; set; }
        public string Description { get; set; }
        public Guid? TypeID { get; set; }
        public Guid? ParentID { get; set; }
        public int OrderNumber { get; set; }
        public string Icon { get; set; }
        public string OrgStructureParentName { get; set; }
        public int ProfileCount { get; set; }
        public int ProfileIsWorking { get; set; }
        public Guid? OrgStructureTypeID { get; set; }
        public string ShopCode { get; set; }
        public int FEMALE { get; set; }
        public int MALE { get; set; }
        public Guid? RegionID { get; set; }
        public Nullable<System.Guid> GroupCostCentreID { get; set; }
        public string CostCentreName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public string ServicesType { get; set; }
        public Nullable<System.DateTime> ContractFrom { get; set; }
        public Nullable<System.DateTime> ContractTo { get; set; }
        public string BillingCompanyName { get; set; }
        public string BillingAddress { get; set; }
        public string TaxCode { get; set; }
        public string DurationPay { get; set; }
        public string RecipientInvoice { get; set; }
        public string TelePhone { get; set; }
        public string CellPhone { get; set; }
        public string DescriptionInfo { get; set; }
        public string EmailInfo { get; set; }
        public int? HrPlanHC { get; set; }
        public string OrgFullName { get; set; }

    }

    public class OrgLevelTypeName
    {
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string BrandName { get; set; }
        public string TeamName { get; set; }

        public string DepartmentCode { get; set; }
        public string SectionCode { get; set; }
        public string BrandCode { get; set; }
        public string TeamCode { get; set; }
    }
    public class OrgTiny
    {
        public Guid ID { get; set; }
        public string OrgName { get; set; }
        public string OrgCode { get; set; }
        public Guid? ParentID { get; set; }
        public Guid? TypeID { get; set; }
    }
   
}
