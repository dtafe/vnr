using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Hr.Models
{
    public class Hre_ContractExtendEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Nullable<System.Guid> ContractID { get; set; }
        public string AnnexCode { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureName { get; set; }
        public string ContractNo { get; set; }
        public DateTime? DateStartContract { get; set; }
        public DateTime? DateEndContract { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public int? DateSign { get; set; }
        public int? MonthSign { get; set; }
        public int? YearSign { get; set; }
        public string NationalityName { get; set; }
        public int? DayOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? YearOfBirth { get; set; }
        public string PAddress { get; set; }
        public string IDNo { get; set; }
        public DateTime? IDDateOfIssue { get; set; }
        public string IDDateOfIssueFormat { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public DateTime? DateSigned { get; set; }
        public string DateStartContractFormat { get; set; }
        public string DateEndContractFormat { get; set; }
        public string DateStartFormat { get; set; }
        public string DateEndFormat { get; set; }
        public string DateSignedContractFormat { get; set; }

    }
}
