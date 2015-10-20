using System;

namespace HRM.Business.Category.Models
{
    public class Cat_ContractTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string Code { get; set; }
        public string ContractTypeName { get; set; }
        public string Type { get; set; }
        public string TypeView { get; set; }
        public string UnitTime { get; set; }
        public string UnitTimeView { get; set; }
        public double? ValueTime { get; set; }
        public string ContractNextID { get; set; }
        public string ContractNextName { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> ExportID { get; set; }
        public string ExportName { get; set; }
        public DateTime? DateSigned { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public Nullable<int> ExpiryContractLoop { get; set; }
        public Nullable<int> DayInMonthLoop1 { get; set; }
        public Nullable<int> DayInMonthLoop2 { get; set; }
        public Nullable<int> DayInMonthLoop3 { get; set; }
        public Nullable<int> ExpiryContractDayByDay { get; set; }
        public string EmailToList { get; set; }
        public string NextContractTypeName { get; set; }
        public string EmailOther { get; set; }
        public string Formula { get; set; }
        public Nullable<bool> NoneEndContract { get; set; }
        public Nullable<bool> NoneTypeInsuarance { get; set; }
        public Nullable<bool> ComputeEndDateByContractType { get; set; }
        public Nullable<bool> ComputeEndDateByFomular { get; set; }

        public Nullable<bool> IsSocialInsurance { get; set; }
        public Nullable<bool> IsHealthInsurance { get; set; }
        public Nullable<bool> IsUnEmployInsurance { get; set; }
    }

    public class Cat_ContractTypeMultiEntity 
    {
        public Guid ID { get; set; }
        public string ContractTypeName { get; set; }
        public string Type { get; set; }

    }
}
