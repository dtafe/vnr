using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_ShopModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Shop_ShopName)]
        public string ShopName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_Rank)]
        public string Rank { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_SaleProduct)]
        public Nullable<double> SaleProduct { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_NoCustomer)]
        public Nullable<double> NoCustomer { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_MoneyCollectService)]
        public Nullable<double> MoneyCollectService { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_NoCustomerService)]
        public Nullable<double> NoCustomerService { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_MainLineProduct)]
        public Nullable<double> MainLineProduct { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_PromoteProduct)]
        public Nullable<double> PromoteProduct { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_ShopLeaderID)]
        public Nullable<System.Guid> ShopLeaderID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_ShopLeaderID)]
        public string ShopLeaderName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_ShiftLeaderID)]
        public Nullable<System.Guid> ShiftLeaderID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_ShiftLeaderID)]
        public string ShiftLeaderName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_Formular)]
        public string Formular { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_Formular1)]
        public string Formular1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_NoShopLeader)]
        public Nullable<int> NoShopLeader { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_NoShiftLeader)]
        public Nullable<int> NoShiftLeader { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Shop_OrgStructureID)]
        public Nullable<System.Guid> OrgStructureID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Shop_OrgStructureID)]
        public string OrgStructureName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public int TotalProfile { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ShopGroup_ShopGroupName)]
        public Nullable<System.Guid> ShopGroupID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_ShopGroup_ShopGroupName)]
        public string ShopGroupName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ShopName = "ShopName";
            public const string Code = "Code";
            public const string Rank = "Rank";
            public const string SaleProduct = "SaleProduct";
            public const string NoCustomer = "NoCustomer";
            public const string MoneyCollectService = "MoneyCollectService";
            public const string NoCustomerService = "NoCustomerService";
            public const string MainLineProduct = "MainLineProduct";
            public const string PromoteProduct = "PromoteProduct";
            public const string Note = "Note";
            public const string ShopLeaderName = "ShopLeaderName";
            public const string ShiftLeaderName = "ShiftLeaderName";
            public const string Formular = "Formular";
            public const string Formular1 = "Formular1";
            public const string NoShopLeader = "NoShopLeader";
            public const string NoShiftLeader = "NoShiftLeader";
            public const string OrgStructureName = "OrgStructureName";
        }
    }

    public class Cat_ShopSearchModel
    {

        public Guid? ShopGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_ShopName)]
        public string ShopName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_ShopMultiModel
    {
        public Guid ID { get; set; }
        public string ShopName { get; set; }
        public int TotalRow { get; set; }
    }

}
