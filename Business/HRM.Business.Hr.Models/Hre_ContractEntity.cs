using System;
using HRM.Business.BaseModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HRM.Business.Hr.Models
{
    public class Hre_ContractEntity : HRMBaseModel
    {
        public string TypeOfPassView { get; set; }
        public Nullable<int> TimesContract { get; set; }
        public string AbilityTitleVNI { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> DateExtend { get; set; }
        public string StatusEvaluation { get; set; }
        public string RankDetailForNextContractName { get; set; }
        public string TypeHourWork { get; set; }
        public string NameByGerder { get; set; }
        public string Type { get; set; }
        public string PassportPlaceOfIssue { get; set; }
        public DateTime? PassportDateOfExpiry { get; set; }
        public string PassportDateOfExpiryFormatEN { get; set; }
        public DateTime? PassportDateOfIssue { get; set; }
        public string PassportDateOfIssueFormatEN { get; set; }
        public string PassportNo { get; set; }
        public Nullable<int> NoPrint { get; set; }
        public string TerminateDateFormatEN { get; set; }
        public DateTime? TerminateDate { get; set; }
        public bool AppendixCT { get; set; }
        public bool HasMonthOrther { get; set; }
        public bool ExcludeQuitEmp { get; set; }
        public bool ExcludeHavingNextContract { get; set; }
        public DateTime? NextContractDateStart { get; set; }
        public DateTime? NextContractDateEnd { get; set; }
        public bool IsEvaluation { get; set; }
        public string TerminateDateFormat { get; set; }
        public string NationalityNameEn { get; set; }
        public string TCountryName { get; set; }
        public string PCountryName { get; set; }
        public string TProvinceName { get; set; }
        public string PProvinceName { get; set; }
        public string TDistrictName { get; set; }
        public string PDistrictName { get; set; }
        public string TVillageName { get; set; }
        public string PVillageName { get; set; }
        public string CodeOSN { get; set; }
        public int TotalRow { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        public string OrgStructureID { get; set; }

        public Guid ProfileID { get; set; }
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        public string ProfileNameView { get; set; }
        public string NationalityName { get; set; }
        [MaxLength(50)]
        public string ContractNo { get; set; }
        public Guid ContractTypeID { get; set; }
        public string DateSignedFormatEN { get; set; }
        public DateTime? DateSigned { get; set; }
        public string DateSignedFormat { get; set; }
        public string DateStartFormatEN { get; set; }
        public DateTime DateStart { get; set; }
        public string DateStartFormat { get; set; }
        public string DateEndFormatEN { get; set; }
        public DateTime? DateEnd { get; set; }
        public string DateEndFormat { get; set; }
        public Guid? PositionID { get; set; }
        public Guid? JobTitleID { get; set; }
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        public Guid ContractNextID { get; set; }
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        public string JobTitleNameEn { get; set; }
        [MaxLength(150)]
        public string PositionName { get; set; }
        public string PositionEngName { get; set; }
        [MaxLength(150)]
        public string ContractTypeName { get; set; }

        public double? Salary { get; set; }
        public string SalaryFormat { get; set; }
        // Loại tiền tệ lương
        public Guid? CurenncyID { get; set; }
        public string CurrencySalName { get; set; }
        // Lương đóng BHXH
        public double? InsuranceAmount { get; set; }
        // Loại tiền tệ lương BHXH
        public Nullable<System.Guid> CurenncyID1 { get; set; }
        public string CurenncyInsName { get; set; }
        // PC1
        public Nullable<System.Guid> AllowanceID1 { get; set; }
        public string AllowanceID1Name { get; set; }
        // Tiền PC1
        public Nullable<double> Allowance1 { get; set; }
        // Loại tiền tệ PC1
        public Nullable<System.Guid> CurenncyID2 { get; set; }
        public string CurenncyAllowance1Name { get; set; }
        //PC2
        public Nullable<System.Guid> AllowanceID2 { get; set; }
        public string AllowanceID2Name { get; set; }
        // Tiền PC2
        public Nullable<double> Allowance2 { get; set; }
        // Loại tiền tệ PC2
        public Nullable<System.Guid> CurenncyID3 { get; set; }
        public string CurenncyAllowance2Name { get; set; }
        // PC3
        public Nullable<System.Guid> AllowanceID3 { get; set; }
        public string AllowanceID3Name { get; set; }
        // Tiền PC3
        public Nullable<double> Allowance3 { get; set; }
        // Loại tiền tệ PC3
        public Nullable<System.Guid> CurenncyIDSalary { get; set; }
        public string CurenncyAllowance3Name { get; set; }
        //PC4
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        public string AllowanceID4Name { get; set; }
        // Tiền PC4
        public Nullable<double> Allowance4 { get; set; }
        // Loại tiền tệ PC4
        public Nullable<System.Guid> CurenncyID4 { get; set; }
        public string CurenncyAllowance4Name { get; set; }
        //Tiền PC khác
        public Nullable<double> Allowance { get; set; }
        // Loại tiền tệ PC khac
        public Nullable<System.Guid> CurenncyID5 { get; set; }
        public string CurenncyOAllowanceName { get; set; }
        //Mã cấp
        public Nullable<System.Guid> ClassRateID { get; set; }

        public string ClassRateName { get; set; }
        //Bậc/ hệ số lương
        public Nullable<System.Guid> RankRateID { get; set; }

        public string RankRateName { get; set; }
        //Căn cứ theo số
        public string FollowNo { get; set; }
        //Nơi làm việc
        public Guid? WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        //Mô tả cv
        public string JobDescription { get; set; }
        //Người đại diện ký
        public Nullable<System.Guid> ProfileSingID { get; set; }
        public string ProfileSingName { get; set; }
        //Hệ số cá nhân
        public Nullable<double> PersonalRate { get; set; }
        //Loại Mã lương
        public Nullable<System.Guid> SalaryClassTypeID { get; set; }
        public string SalaryClassTypeName { get; set; }
        //Hình thức trả lương
        public string FormPaySalary { get; set; }
        //Trinh do chuyen mon
        public Nullable<System.Guid> QualificationID { get; set; }
        public string QualificationName { get; set; }
        //h lam viec/thang
        public Nullable<double> HourWorkInMonth { get; set; }
        //Ngày Ủy Quyền
        public Nullable<System.DateTime> DateAuthorize { get; set; }
        public bool? CreateBasicSalary { get; set; }
        public string IDNo { get; set; }
        public string PlaceOfBirth { get; set; }
        public string TAddress { get; set; }
        public DateTime? IDDateOfIssue { get; set; }
        public string IDDateOfIssueFormat { get; set; }
        public string DOfBirth { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string DateOfBirthFormat { get; set; }
        public string DayOfBirthString { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string PAddress { get; set; }
        //public DateTime? Birthday { get; set; }
        public string Birthday { get; set; }
        public string Cellphone { get; set; }
        public DateTime? DateHire { get; set; }
        public string DateHire_Day { get; set; }
        public string DateHire_Month { get; set; }
        public string DateHire_Year { get; set; }
        public string DateHireFormat { get; set; }
        public string DateHireFormatEN { get; set; }
        public DateTime? DateEndProbation { get; set; }
        public string DateEndProbation_Day { get; set; }
        public string DateEndProbation_Month { get; set; }
        public string DateEndProbation_Year { get; set; }
        public string DateEndProbationFormat { get; set; }
        public string DateEndProbationFormatEN { get; set; }
        public int? DayOfDateHire { get; set; }
        public int? MonthOfDateHire { get; set; }
        public int? YearOfDateHire { get; set; }
        public int? DayOfDateProbation { get; set; }
        public int? MonthOfDateProbation { get; set; }
        public int? YearOfDateProbation { get; set; }
        public string DateNow { get; set; }
        public string DateNow_Day { get; set; }
        public string DateNow_Month { get; set; }
        public string DateNow_Year { get; set; }
        public string DateNow_Hour { get; set; }
        public string CountryName { get; set; }

        public string SalaryUS { get; set; }
        public string ResignReasonName { get; set; }
        public string SalaryRankName { get; set; }
        public string SalaryRankNameNextContract { get; set; }
        public string ActionStatus { get; set; }
        public string Gender { get; set; }
        public string GenderView { get; set; }
        public string PeriodContract { get; set; }
        public string GraveName { get; set; }
        public DateTime? DateOfEffect { get; set; }
        public string DateOfEffectFormat { get; set; }
        public string DateOfEffectFormatEng { get; set; }

        public string DateOfEffectMoreTwoMonthFormat { get; set; }
        public Guid? ExportID { get; set; }
        public string NameEnglish { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Contract_ID
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

        public string ContractEvaType { get; set; }
        public DateTime? DateOfContractEva { get; set; }
        public string EvaluationResult { get; set; }
        public string ContractResult { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public Nullable<int> DayContract { get; set; }
        public Nullable<int> DayExtend { get; set; }
        public Guid? RankDetailForNextContract { get; set; }
        public string RankDetailForNextContractIds { get; set; }
        public string TypeOfPass { get; set; }
        public Guid? SalaryClassID { get; set; }
        public Guid? NextContractTypeID { get; set; }
        public string NextContractTypeName { get; set; }
        public string Remark { get; set; }
        public DateTime? DateEndNextContract { get; set; }
        public string AnnexCode { get; set; }
        public string DateSign { get; set; }
        public string MonthSign { get; set; }
        public string YearSign { get; set; }
        public string contrctextend1 { get; set; }
        public string contrctextend2 { get; set; }
        public string contrctextend3 { get; set; }
        public DateTime? DateQuit { get; set; }
        public string DateQuitFormat { get; set; }
        public int? YearOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? DayOfBirth { get; set; }

        public double? MonthWorking { get; set; }
        public double? YearWorking { get; set; }
   
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string BankNameCustomer { get; set; }
        public string BankBrandName { get; set; }
        public string Email { get; set; }
        public string ErrorMessage { get; set; }

        public string DateStart_Day { get; set; }
        public string DateStart_Month { get; set; }
        public string DateStart_Year { get; set; }
        public string DateEnd_Day { get; set; }
        public string DateEnd_Month { get; set; }
        public string DateEnd_Year { get; set; }
        public string DateSigned_Day { get; set; }
        public string DateSigned_Month { get; set; }
        public string DateSigned_Year { get; set; }
        public string DateOfBirthFormatEN { get; set; }
        public string DateOfBirth_Day { get; set; }
        public string DateOfBirth_Month { get; set; }
        public string DateOfBirth_Year { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public string EducationLevelName { get; set; }
        public string OrgStructureCode { get; set; }
        public Nullable<double> ValueTime { get; set; }
        public Nullable<int> DateEndMonth { get; set; }
        public Nullable<int> DateEndYear { get; set; }
        public DateTime? DateStartOld { get; set; }
        public string DateStartOld_Day { get; set; }
        public string DateStartOld_Month { get; set; }
        public string DateStartOld_Year { get; set; }
        public DateTime? DateEndOld { get; set; }
        public string DateEndOld_Day { get; set; }
        public string DateEndOld_Month { get; set; }
        public string DateEndOld_Year { get; set; }
        public DateTime? DateSignedOld { get; set; }
        public string DateSignedOld_Day { get; set; }
        public string DateSignedOld_Month { get; set; }
        public string DateSignedOld_Year { get; set; }
        public string LastName { get; set; }
        public string FristName { get; set; }
        public string TenDem { get; set; }
        public double? MonthSenior { get; set; }
        public string ValueFields { get; set; }
        public DateTime? DateSignedFrom { get; set; }
        public DateTime? DateSignedTo { get; set; }
        public Nullable<System.Guid> AllowanceID7 { get; set; }
        public string AllowanceID7Name { get; set; }
        public Nullable<System.Guid> AllowanceID8 { get; set; }
        public string AllowanceID8Name { get; set; }
        public Nullable<System.Guid> AllowanceID9 { get; set; }
        public string AllowanceID9Name { get; set; }
        public Nullable<System.Guid> AllowanceID10 { get; set; }
        public string AllowanceID10Name { get; set; }
        public Nullable<System.Guid> AllowanceID11 { get; set; }
        public string AllowanceID11Name { get; set; }
        public Nullable<System.Guid> AllowanceID12 { get; set; }
        public string AllowanceID12Name { get; set; }
        public Nullable<System.Guid> AllowanceID13 { get; set; }
        public string AllowanceID13Name { get; set; }
        public Nullable<System.Guid> AllowanceID14 { get; set; }
        public string AllowanceID14Name { get; set; }
        public Nullable<System.Guid> AllowanceID15 { get; set; }
        public string AllowanceID15Name { get; set; }
        public Nullable<double> Allowance7 { get; set; }
        public Nullable<double> Allowance8 { get; set; }
        public Nullable<double> Allowance9 { get; set; }
        public Nullable<double> Allowance10 { get; set; }
        public Nullable<double> Allowance11 { get; set; }
        public Nullable<double> Allowance12 { get; set; }
        public Nullable<double> Allowance13 { get; set; }
        public Nullable<double> Allowance14 { get; set; }
        public Nullable<double> Allowance15 { get; set; }
        public string ContractNextName { get; set; }
       
        public partial class FieldNames
        {
            public const string StatusEvaluation = "StatusEvaluation";
            public const string Gender = "Gender";
            public const string DateOfBirth = "DateOfBirth";
            public const string IDNo = "IDNo";
            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string TAddress = "TAddress";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string ID = "ID";
            public const string Code = "Code";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileID = "ProfileID";
            public const string ContractNo = "ContractNo";
            public const string ContractTypeID = "ContractTypeID";
            public const string DateSigned = "DateSigned";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionID = "PositionID";
            public const string JobTitleID = "JobTitleID";
            public const string ProfileName = "ProfileName";
            public const string PositionName = "PositionName";
            public const string ContractTypeName = "ContractTypeName";
            public const string JobTitleName = "JobTitleName";
            public const string Salary = "Salary";
            public const string CurrencyID = "CurrencyID";
            public const string CurrencyName = "CurrencyName";
            public const string InsuranceAmount = "InsuranceAmount";
            public const string CurenncyID1 = "CurenncyID1";
            public const string AllowanceID1 = "AllowanceID1";
            public const string Allowance1 = "Allowance1";
            public const string NextContractTypeName = "NextContractTypeName";
            public const string AllowanceID2 = "AllowanceID2";
            public const string Allowance2 = "Allowance2";
            public const string AllowanceID3 = "AllowanceID3";
            public const string Allowance3 = "Allowance3";
            public const string AllowanceID4 = "AllowanceID4";
            public const string Allowance4 = "Allowance4";
            public const string Allowance = "Allowance";
            public const string ClassRateID = "ClassRateID";
            public const string ClassRateName = "ClassRateName";
            public const string RankRateID = "RankRateID";
            public const string RankRateName = "RankRateName";
            public const string FollowNo = "FollowNo";
            public const string WorkPlaceID = "WorkPlaceID";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string JobDescription = "JobDescription";
            public const string ProfileSingID = "ProfileSingID";
            public const string ProfileSingName = "ProfileSingName";
            public const string PersonalRate = "PersonalRate";
            public const string SalaryClassTypeID = "SalaryClassTypeID";
            public const string SalaryClassTypeName = "SalaryClassTypeName";
            public const string FormPaySalary = "FormPaySalary";
            public const string QualificationID = "QualificationID";
            public const string QualificationName = "QualificationName";
            public const string HourWorkInMonth = "HourWorkInMonth";
            public const string DateAuthorize = "DateAuthorize";
            public const string ContractEvaType = "ContractEvaType";
            public const string DateOfContractEva = "DateOfContractEva";
            public const string EvaluationResult = "EvaluationResult";
            public const string ContractResult = "ContractResult";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string CertificateName = "CertificateName";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string CurenncyInsName = "CurenncyInsName";
            public const string DateSignedOld = "DateSignedOld";
            public const string DateSignedOld_Day = "DateSignedOld_Day";
            public const string DateSignedOld_Month = "DateSignedOld_Month";
            public const string DateSignedOld_Year = "DateSignedOld_Year";

            public const string DateStartOld = "DateStartOld";
            public const string DateStartOld_Day = "DateStartOld_Day";
            public const string DateStartOld_Month = "DateStartOld_Month";
            public const string DateStartOld_Year = "DateStartOld_Year";

            public const string DateEndOld = "DateEndOld";
            public const string DateEndOld_Day = "DateEndOld_Day";
            public const string DateEndOld_Month = "DateEndOld_Month";
            public const string DateEndOld_Year = "DateEndOld_Year";

            public const string PAddress = "PAddress";
            public const string PDistrictName = "PDistrictName";
            public const string PCountryName = "PCountryName";
            public const string PProvinceName = "PProvinceName";
           
            public const string LastName ="LastName";
            public const string FristName = "FristName";
            public const string TenDem = "TenDem";
            public const string DateEndNextContract="DateEndNextContract";
            public const string TypeOfPassView = "TypeOfPassView";
        }
    }

    public class Hre_ContractSendMailEntity
    {
        public Guid ContractID { get; set; }
        public Nullable<DateTime> DayDue { get; set; }
        public string ProfileName { get; set; }
        public List<string> lstEmail { get; set; }
        public string Type { get; set; }
    }

    public class Hre_ContractBodyEntity
    {
        public List<Guid> lstGContract { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
    }
    
}
