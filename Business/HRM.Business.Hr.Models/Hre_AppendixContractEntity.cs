using System;
using HRM.Business.BaseModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_AppendixContractEntity : HRMBaseModel
    {
        public string AppendixContractTypeName { get; set; }
        public int TotalRow { get; set; }
        public Guid ContractID { get; set; }
        public Guid? ProfileID { get; set; }
        public string ContractName { get; set; }
        public string OrgStructureName { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string Gender { get; set; }
        public string ContractNo { get; set; }
        public string AppendixContractName { get; set; }
        public string JobTitleName { get; set; }
        public string WorkingPlace { get; set; }
        public DateTime? DateHire { get; set; }
        public string Code { get; set; }
        public string AppendixContractCode { get; set; }
        public DateTime? DateofEffect { get; set; }
        public DateTime? DateofEffectView { get; set; }
        public string Contents { get; set; }
        public string Attachment { get; set; }
        public string AppendixContractType { get; set; }
        public DateTime? DateSignedAppendixContract { get; set; }
        public double? Amount { get; set; }
        public DateTime? DateEndAppendixContract { get; set; }
        public Guid? JobTitleID { get; set; }
        public Guid? RankRateID { get; set; }
        public Guid? ClassRateID { get; set; }
        public Guid? PositionID { get; set; }
        public string PositionName { get; set; }
        public Guid? ContractTypeID { get; set; }
        public Guid? AllowanceID1 { get; set; }
        public double? Allowance1 { get; set; }
        public string Allowance1Format { get; set; }
        public Guid? AllowanceID2 { get; set; }
        public string IDNo { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string PlaceOfIssueName { get; set; }
        public double? Allowance2 { get; set; }
        public Guid? AllowanceID3 { get; set; }
        public double? Allowance3 { get; set; }
        public double? Allowance4 { get; set; }
        public double? Allowance { get; set; }
        public double? Salary { get; set; }
        public string PLSalary { get; set; }
        public Guid? CurenncyID { get; set; }
        public Guid? CurenncyID1 { get; set; }
        public Guid? CurenncyID2 { get; set; }
        public Guid? CurenncyID3 { get; set; }
        public Guid? AllowanceID4 { get; set; }
        public Guid? CurenncyIDSalary { get; set; }
        public Guid? CurenncyID4 { get; set; }
        public Guid? ReportMappingID { get; set; }
        public Guid? CurenncyID5 { get; set; }
        public double? InsuranceAmount { get; set; }
        public Guid? CurenncyID6 { get; set; }
        public Guid? AppendixContractTypeID { get; set; }
        public DateTime? DateAuthorize { get; set; }
        public double? HourWorkInMonth { get; set; }
        public string JobDescription { get; set; }
        public double? Bonus { get; set; }
        public string KPI { get; set; }
        public Guid? AllowanceID5 { get; set; }
        public string KPICompany { get; set; }
        public double? Tax { get; set; }
        private Guid _id = Guid.Empty;
        public Guid AppendixContract_ID
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

        public string AllowanceID1Name { get; set; }
        public string CurenncyAllowance1Name { get; set; }
        public string AllowanceID2Name { get; set; }
        public string CurenncyAllowance2Name { get; set; }
        public string AllowanceID3Name { get; set; }
        public string CurenncyAllowance3Name { get; set; }
        public string AllowanceID4Name { get; set; }
        public string CurenncyAllowance4Name { get; set; }
        public string CurenncyOAllowanceName { get; set; }
        public string ClassRateName { get; set; }
        public string RankRateName { get; set; }
        public string SalaryClassTypeName { get; set; }
        public string SalaryClassName { get; set; }
        public string ProfileSingName { get; set; }
        public string QualificationName { get; set; }
        public double? CurrentSalary { get; set; }
        public double? CurrentAllowance { get; set; }
        public double? CurrentAllowance1 { get; set; }
        public double? CurrentAllowance2 { get; set; }
        public double? CurrentAllowance3 { get; set; }
        public double? CurrentAllowance4 { get; set; }
        public double? CurrentAllowance5 { get; set; }

        public string CurrencySalName { get; set; }
        public string CurenncyInsName { get; set; }
        public Nullable<System.Guid> RankDetailID { get; set; }
        public string RankDetailName { get; set; }
        public Nullable<System.Guid> RankID { get; set; }
        public string RankName { get; set; }
        public string GenderView { get; set; }


        //Nơi làm việc
        public Guid? WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }

        public string DateofEffectFormat { get; set; }
        
        public string DateofEffect_Day { get; set; }
        public string DateofEffect_Month { get; set; }
        public string DateofEffect_Year { get; set; }
        public string PLDateStart { get; set; }


        public string DateSignedAppendixContractFormat { get; set; }
        public string DateSignedAppendixContract_Day { get; set; }
        public string DateSignedAppendixContract_Month { get; set; }
        public string DateSignedAppendixContract_Year { get; set; }
        public string PLDateSign { get; set; }
        public DateTime? DateSign { get; set; }
        public string HDDateSign { get; set; }

        public DateTime? DateStart { get; set; }
        public string HDStarDate { get; set; }

        public string DateEndAppendixContractFormat { get; set; }
        public string DateEndAppendixContract_Day { get; set; }
        public string DateEndAppendixContract_Month { get; set; }
        public string DateEndAppendixContract_Year { get; set; }
        public string PLDateEnd { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public DateTime? DayOfBirth { get; set; }
        public string DayOfBirthFormat { get; set; }
        public string DayOfBirth_Day { get; set; }
        public string DayOfBirth_Month { get; set; }
        public string DayOfBirth_Year { get; set; }

        public DateTime? IDDateOfIssue { get; set; }
        public string IDDateOfIssueFormat { get; set; }
        public string IDDateOfIssue_Day { get; set; }
        public string IDDateOfIssue_Month { get; set; }
        public string IDDateOfIssue_Year { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        
        public DateTime? DateSigned {get; set;}
        public string DateSignedFormat { get; set; }
        public string DateSigned_Day { get; set; }
        public string DateSigned_Month { get; set; }
        public string DateSigned_Year { get; set; }
        public Double? ContractSalary { get; set; }
        public string ContractSalaryFormat { get; set; }
        public string PAddress { get; set; }
        public string TAddress { get; set; }
        public string TCountryName { get; set; }
        public string TDistrictName { get; set; }
        public string TProvinceName { get; set; }
        public string CellPhone { get; set; }

    }
}
