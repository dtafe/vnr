using System;


namespace HRM.Business.Payroll.Models
{
    public class Sal_SalaryInformationEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public System.Guid ID { get; set; }
        public System.Guid ProfileID { get; set; }
        public string AccountNo { get; set; }
        public Nullable<System.Guid> BankID { get; set; }
        public string ProfileName { get; set; }

        public string CodeEmp { get; set; }

        public System.DateTime? DateHire { get; set; }
        public string BankName { get; set; }
        public string BankCode1 { get; set; }
        public string BankName2 { get; set; }
        public string BankCode2 { get; set; }
        public System.Guid? OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public string AccountName2 { get; set; }
        public string AccountNo2 { get; set; }
        public string SwiftCode2 { get; set; }
        public string SortCode2 { get; set; }
        public string AmountTransfer2 { get; set; }
        public Nullable<System.Guid> BankID2 { get; set; }
        public string AccountName { get; set; }
        public string SwiftCode { get; set; }
        public string SortCode { get; set; }
        public string AmountTransfer { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<System.Guid> CurrencyID2 { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<bool> IsDefault2 { get; set; }
        public string BankNameCustomer { get; set; }
        public string BankNameCustomer2 { get; set; }
        public Nullable<System.Guid> AccountCurrencyID { get; set; }
        public Nullable<System.Guid> AccountCurrencyID2 { get; set; }
        public Nullable<bool> IsCash { get; set; }
        public Nullable<bool> IsCash2 { get; set; }
        public string BankBrandName { get; set; }
        public string BankBrandName2 { get; set; }
        public string DepartmentName { get; set; }

        public string CurrencyName { get; set; }
        public string CurrencyName2 { get; set; }
        public string IDNo { get; set; }
        public string GroupBank { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }

    public class Sal_SalaryInformationMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        //public string UnusualAllowanceCfgName { get; set; }
    }
}

