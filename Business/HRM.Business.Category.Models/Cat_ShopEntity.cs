using System;

namespace HRM.Business.Category.Models
{
    public class Cat_ShopEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ShopName { get; set; }
        public string Code { get; set; }
        public string Rank { get; set; }
        public Nullable<double> SaleProduct { get; set; }
        public Nullable<double> NoCustomer { get; set; }
        public Nullable<double> MoneyCollectService { get; set; }
        public Nullable<double> NoCustomerService { get; set; }
        public Nullable<double> MainLineProduct { get; set; }
        public Nullable<double> PromoteProduct { get; set; }
        public string Note { get; set; }
        public Nullable<System.Guid> ShopLeaderID { get; set; }
        public string ShopLeaderName { get; set; }
        public Nullable<System.Guid> ShiftLeaderID { get; set; }
        public string ShiftLeaderName { get; set; }
        public string Formular { get; set; }
        public string Formular1 { get; set; }
        public Nullable<int> NoShopLeader { get; set; }
        public Nullable<int> NoShiftLeader { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public int TotalProfile { get; set; }

        public Nullable<System.Guid> ShopGroupID { get; set; }
        public string ShopGroupName { get; set; }

    }
    public class Cat_ShopMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string ShopName { get; set; }
    }


}

