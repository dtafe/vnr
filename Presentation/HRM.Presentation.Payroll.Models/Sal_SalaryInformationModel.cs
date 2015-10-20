using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_SalaryInformationModel : BaseViewModel
    {
        
        public System.Guid ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public System.Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public System.DateTime? DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public System.Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountNo)]
        public string AccountNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_Bank1)]
        public Nullable<System.Guid> BankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_Bank1)]
        public string BankName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountName2)]
        public string AccountName2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountNo2)]
        public string AccountNo2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_SwiftCode2)]
        public string SwiftCode2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_SortCode2)]
        public string SortCode2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AmountTransfer2)]
        public string AmountTransfer2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_Bank2)]
        public Nullable<System.Guid> BankID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_Bank2)]
        public string BankName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountName)]
        public string AccountName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_SwiftCode)]
        public string SwiftCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_SortCode)]
        public string SortCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AmountTransfer)]
        public string AmountTransfer { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_Currency)]
        public Nullable<System.Guid> CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_Currency)]
        public string CurrencyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_Currency2)]

        public Nullable<System.Guid> CurrencyID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_Currency2)]
        public string CurrencyName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_BankNameCustomer)]
        public string BankNameCustomer { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_BankNameCustomer2)]
        public string BankNameCustomer2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountCurrency)]
        public Nullable<System.Guid> AccountCurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountCurrency2)]
        public Nullable<System.Guid> AccountCurrencyID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_BankBrandName)]
        public string BankBrandName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_BankBrandName2)]
        public string BankBrandName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_InCash)]
        public Nullable<bool> IsCash { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_InCash2)]
        public Nullable<bool> IsCash2 { get; set; }
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }
          [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_GroupBank)]
        public string GroupBank { get; set; }
        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }

        private Guid _id = Guid.Empty;
        public Guid UnusualED_ID
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
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string IDNo = "IDNo";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string DateHire = "DateHire";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string SectionName = "SectionName";
            public const string DepartmentName = "DepartmentName";
            public const string GroupBank = "GroupBank";
            public const string AccountNo = "AccountNo";
            public const string BankID = "BankID";
            public const string BankName = "BankName";
            public const string AccountName2 = "AccountName2";
            public const string AccountNo2 = "AccountNo2";
            public const string SwiftCode2 = "SwiftCode2";
            public const string SortCode2 = "SortCode2";
            public const string AmountTransfer2 = "AmountTransfer2";
            public const string BankID2 = "BankID2";
            public const string BankName2 = "BankName2";
            public const string AccountName = "AccountName";
            public const string SwiftCode = "SwiftCode";
            public const string SortCode = "SortCode";
            public const string AmountTransfer = "AmountTransfer";
            public const string CurrencyID = "CurrencyID";
            public const string CurrencyID2 = "CurrencyID2";
            public const string CurrencyName = "CurrencyName";
            public const string CurrencyNam2 = "CurrencyNam2";
            public const string BankNameCustomer = "BankNameCustomer";
            public const string BankNameCustomer2 = "BankNameCustomer2";
            public const string AccountCurrencyID2 = "AccountCurrencyID2";
            public const string AccountCurrencyID = "AccountCurrencyID";
            public const string BankBrandName = "AccountCurrencyID";
            public const string BankBrandName2 = "AccountCurrencyID";
            public const string IsCash = "AccountCurrencyID";
            public const string IsCash2 = "AccountCurrencyID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";


        }
    }

    public class Sal_SalaryInformationSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountName2)]
        public string AccountName2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountName)]
        public string AccountName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountNo)]
        public string AccountNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_Sal_SalaryInformation_AccountNo2)]
        public string AccountNo2 { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public class Sal_SalaryInformationMutiModel
        {
            public Guid ID { get; set; }
            //public string ProfileName { get; set; }
        }
    }
}
