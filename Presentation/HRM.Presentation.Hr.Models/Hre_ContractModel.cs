using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ContractModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID7)]
        public Nullable<System.Guid> AllowanceID7 { get; set; }
        public string AllowanceID7Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID8)]
        public Nullable<System.Guid> AllowanceID8 { get; set; }
        public string AllowanceID8Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID9)]
        public Nullable<System.Guid> AllowanceID9 { get; set; }
        public string AllowanceID9Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID10)]
        public Nullable<System.Guid> AllowanceID10 { get; set; }
        public string AllowanceID10Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID11)]
        public Nullable<System.Guid> AllowanceID11 { get; set; }
        public string AllowanceID11Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID12)]
        public Nullable<System.Guid> AllowanceID12 { get; set; }
        public string AllowanceID12Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID13)]
        public Nullable<System.Guid> AllowanceID13 { get; set; }
        public string AllowanceID13Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID14)]
        public Nullable<System.Guid> AllowanceID14 { get; set; }
        public string AllowanceID14Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID15)]
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
        [DisplayName(ConstantDisplay.HRM_HR_Contract_TimesContract)]
        public Nullable<int> TimesContract { get; set; }
        public string requiredInformation { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> DateExtendFrom { get; set; }
        public Nullable<System.DateTime> DateExtendTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateExtend)]
        public Nullable<System.DateTime> DateExtend { get; set; }
        public string StatusEvaluation { get; set; }
        public string TypeHourWork { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_NoPrint)]
        public Nullable<int> NoPrint { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_TerminateDate)]
        public DateTime? TerminateDate { get; set; }
        public string selectedIds { get; set; }
        public List<Guid> selecteIds { get; set; }
        public string DayOfBirth { get; set; }
        public string DateSignedFormat { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Code)]
        [MaxLength(50)]
        public string Code { get; set; }
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        [MaxLength(50)]
        public string ContractNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractTypeID)]
        public Guid ContractTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateSigned)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateSigned { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_PositionID)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_JobTitleID)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public List<Guid> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]

        public string ProfileNameView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        [MaxLength(150)]
        public string ContractTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_Salary)]
        public double? Salary { get; set; }
        // Loại tiền tệ lương
        [DisplayName(ConstantDisplay.HRM_HR_Contract_CurrencyID)]
        public Guid? CurenncyID { get; set; }
        public string CurrencySalName { get; set; }
        // Lương đóng BHXH
        [DisplayName(ConstantDisplay.HRM_HR_Contract_InsuranceAmount)]
        public double? InsuranceAmount { get; set; }
        // Loại tiền tệ lương BHXH
        [DisplayName(ConstantDisplay.HRM_HR_Contract_CurenncyID1)]
        public Nullable<System.Guid> CurenncyID1 { get; set; }
        public string CurenncyInsName { get; set; }
        // PC1
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID1)]
        public Nullable<System.Guid> AllowanceID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID1)]
        public string AllowanceID1Name { get; set; }
        // Tiền PC1
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Allowance1)]
        public Nullable<double> Allowance1 { get; set; }
        // Loại tiền tệ PC1
        public Nullable<System.Guid> CurenncyID2 { get; set; }
        public string CurenncyAllowance1Name { get; set; }
        //PC2
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID2)]
        public Nullable<System.Guid> AllowanceID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID2)]
        public string AllowanceID2Name { get; set; }
        // Tiền PC2
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Allowance2)]
        public Nullable<double> Allowance2 { get; set; }
        // Loại tiền tệ PC2
        public Nullable<System.Guid> CurenncyID3 { get; set; }
        public string CurenncyAllowance2Name { get; set; }
        // PC3
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID3)]
        public Nullable<System.Guid> AllowanceID3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID3)]
        public string AllowanceID3Name { get; set; }
        // Tiền PC3
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Allowance3)]
        public Nullable<double> Allowance3 { get; set; }
        // Loại tiền tệ PC3
        public Nullable<System.Guid> CurenncyIDSalary { get; set; }
        public string CurenncyAllowance3Name { get; set; }
        //PC4
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID4)]
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID4)]
        public string AllowanceID4Name { get; set; }
        // Tiền PC4
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Allowance4)]
        public Nullable<double> Allowance4 { get; set; }
        // Loại tiền tệ PC4
        public Nullable<System.Guid> CurenncyID4 { get; set; }
        public string CurenncyAllowance4Name { get; set; }
        //Tiền PC khác
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Allowance)]
        public Nullable<double> Allowance { get; set; }
        // Loại tiền tệ PC khac1
        public Nullable<System.Guid> CurenncyID5 { get; set; }
        public string CurenncyOAllowanceName { get; set; }
        //Mã cấp
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ClassRateID)]
        public Nullable<System.Guid> ClassRateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ClassRateID)]
        public string ClassRateName { get; set; }
        //Bậc/ hệ số lương
        [DisplayName(ConstantDisplay.HRM_HR_Contract_RankRateID)]
        public Nullable<System.Guid> RankRateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_RankRateID_NextContract)]
        public Guid? RankDetailForNextContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_RankRateID_NextContract)]
        public string SalaryRankNameNextContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_RankRateID)]
        public string SalaryRankName { get; set; }
        //Căn cứ theo số
        [DisplayName(ConstantDisplay.HRM_HR_Contract_FollowNo)]
        public string FollowNo { get; set; }
        //Nơi làm việc
        [DisplayName(ConstantDisplay.HRM_HR_Contract_WorkPlaceID)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_WorkPlaceID)]
        public string WorkPlaceName { get; set; }
        //Mô tả cv
        [DisplayName(ConstantDisplay.HRM_HR_Contract_JobDescription)]
        public string JobDescription { get; set; }
        //Người đại diện ký
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ProfileSingID)]
        public Nullable<System.Guid> ProfileSingID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ProfileSingID)]
        public string ProfileSingName { get; set; }
        //Hệ số cá nhân
        [DisplayName(ConstantDisplay.HRM_HR_Contract_PersonalRate)]
        public Nullable<double> PersonalRate { get; set; }
        //Loại Mã lương
        [DisplayName(ConstantDisplay.HRM_HR_Contract_SalaryClassTypeID)]
        public Nullable<System.Guid> SalaryClassTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_SalaryClassTypeID)]
        public string SalaryClassTypeName { get; set; }
        //Hình thức trả lương
        [DisplayName(ConstantDisplay.HRM_HR_Contract_FormPaySalary)]
        public string FormPaySalary { get; set; }
        //Trinh do chuyen mon
        [DisplayName(ConstantDisplay.HRM_HR_Contract_QualificationID)]
        public Nullable<System.Guid> QualificationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_QualificationID)]
        public string QualificationName { get; set; }
        //h lam viec/thang
        [DisplayName(ConstantDisplay.HRM_HR_Contract_HourWorkInMonth)]
        public Nullable<double> HourWorkInMonth { get; set; }
        //Ngày Ủy Quyền
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateAuthorize)]
        public Nullable<System.DateTime> DateAuthorize { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_CreateBasicSalary)]
        public bool? CreateBasicSalary { get; set; }

        public bool BasicSalaryForPerson { get; set; }

        public string ProfileIDs { get; set; }


        public string IDNo { get; set; }
        public string PlaceOfBirth { get; set; }
        public string TAddress { get; set; }
        public DateTime? IDDateOfIssue { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string DOfBirth { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }

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
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractEvaType)]
        public string ContractEvaType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateOfContractEva)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfContractEva { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_EvaluationResult)]
        public string EvaluationResult { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractResult)]
        public string ContractResult { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_Status)]
        public string StatusView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNextID)]
        public Nullable<System.Guid> ContractNextID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNextName)]
        public string ContractNextName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DayContract)]
        public Nullable<int> DayContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DayExtend)]
        public Nullable<int> DayExtend { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_TypeOfPass)]
        public string TypeOfPass { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_TypeOfPass)]
        public string TypeOfPassView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_NextContractTypeID)]
        public Guid? NextContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_NextContractTypeID)]
        public string NextContractTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Remark)]
        public string Remark { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEndNextContract)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEndNextContract { get; set; }

        public string DateSign { get; set; }
        public string MonthSign { get; set; }
        public string YearSign { get; set; }


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
        public string DateNow { get; set; }
        public string DateNow_Day { get; set; }
        public string DateNow_Month { get; set; }
        public string DateNow_Year { get; set; }
        public string IDDateOfIssueFormat { get; set; }
        public string DateStartFormat { get; set; }
        public string DateEndFormat { get; set; }
        public DateTime? DateHire { get; set; }
        public string DateHireFormat { get; set; }
        public string DateEndProbation_Day { get; set; }
        public string DateEndProbation_Month { get; set; }
        public string DateEndProbation_Year { get; set; }
        public DateTime? DateEndProbation { get; set; }
        public string DateEndProbationFormat { get; set; }
        public string SalaryFormat { get; set; }
        public DateTime? DateOfEffect { get; set; }
        public string DateOfEffectFormat { get; set; }
        public string DateOfEffectMoreTwoMonthFormat { get; set; }
        public string GraveName { get; set; }
        public string CertificateName { get; set; }
        public DateTime? DateQuit { get; set; }
        public string DateQuitFormat { get; set; }
        public double? MonthWorking { get; set; }
        public double? YearWorking { get; set; }
        public Guid? ExportID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        [DisplayName(ConstantDisplay.HRM_Sal_Grade_OrgStructureID)]
        public string OrgStructureIDs { get; set; }
        public string RankRateName { get; set; }
        public string RankDetailForNextContractName { get; set; }

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
        public string PAddress {get; set;}
        public string PCountryName {get; set;}
        public string PProvinceName {get; set;}
        public string PDistrictName {get; set;}
        public string LastName { get; set; }
        public string FristName { get; set; }
        public string TenDem { get; set; }
        public partial class FieldNames
        {
            public const string TypeOfPassView = "TypeOfPassView";
            public const string TypeOfPass = "TypeOfPass";
            public const string TimesContract = "TimesContract";
            public const string DateExtend = "DateExtend";
            public const string RankDetailForNextContractName = "RankDetailForNextContractName";
            public const string DayExtend = "DayExtend";
            public const string DateEndNextContract = "DateEndNextContract";
            public const string NoPrint = "NoPrint";
            public const string DateSign = "DateSign";
            public const string MonthSign = "MonthSign";
            public const string YearSign = "YearSign";
            public const string Gender = "Gender";
            public const string DateOfBirth = "DateOfBirth";
            public const string IDNo = "IDNo";
            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string TAddress = "TAddress";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string ID = "ID";
            public const string CertificateName = "CertificateName";
            public const string Code = "Code";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileID = "ProfileID";
            public const string ContractNo = "ContractNo";
            public const string ContractTypeID = "ContractTypeID";
            public const string DateSigned = "DateSigned";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
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
            public const string AllowanceID1Name = "AllowanceID1Name";
            public const string Allowance1 = "Allowance1";
            public const string AllowanceID2 = "AllowanceID2";
            public const string AllowanceID2Name = "AllowanceID2Name";
            public const string Allowance2 = "Allowance2";
            public const string AllowanceID3 = "AllowanceID3";
            public const string AllowanceID3Name = "AllowanceID3Name";
            public const string Allowance3 = "Allowance3";
            public const string AllowanceID4 = "AllowanceID4";
            public const string AllowanceID4Name = "AllowanceID4Name";
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
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string ContractNextName = "ContractNextName";
        }
    }
    public class Hre_ContractSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Code)]
        [MaxLength(50)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        [MaxLength(50)]
        public string ContractNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public Guid? ContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_IsLastestContract)]
        public bool IsLastestContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_TimesContract)]
        public Nullable<int> TimesContract { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ContractSearchNewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Code)]
        [MaxLength(50)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        [MaxLength(50)]
        public string ContractNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public Guid? ContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
    }
    public class Hre_ContractWaitingSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_Code)]
        [MaxLength(50)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        [MaxLength(50)]
        public string ContractNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public Guid? ContractTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDNo)]
        public string IDNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_WorkPlaceID)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public ExportFileType ExportType { get; set; }
        public Guid ExportID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
       
    }
    public class Hre_EvaluationContractWaitingApprovedSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public Guid? ContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_EvaluationResult)]
        public string EvaluationResult { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_IsMissingInformation)]
        public bool IsMissingInformation { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        public string ContractNo { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public Guid ExportID { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ContractEndSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_Code)]
        [MaxLength(50)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        [MaxLength(50)]
        public string ContractNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        public Guid? ContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
