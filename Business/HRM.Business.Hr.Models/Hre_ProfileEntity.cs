using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HRM.Business.Hr.Models
{
    public class Hre_SupervisorEntity 
    {
        public Nullable<System.Guid> SupervisorID { get; set; }
        public Nullable<System.Guid> HighSupervisorID { get; set; }
        public string SupervisorName { get; set; }
        public string HighSupervisorName { get; set; }

    }
    public class Hre_ProfileEntity : HRMBaseModel
    {
        public Nullable<System.Guid> AbilityTileID { get; set; }
        public double? PCConNho { get; set; }
        public double? PCArea { get; set; }
        public int? ScoreIQ { get; set; }
        public int? ScoreEngJap { get; set; }
        public Nullable<System.DateTime> YearGraduation { get; set; }
        public string  YearGraduationFormat { get; set; }
        public string GraduateSchool { get; set; }
        public Nullable<bool> IsTradeUnionist { get; set; }
        public Nullable<System.DateTime> TradeUnionistEnrolledDate { get; set; }
        public Nullable<System.Guid> TradeUnionistPositionID { get; set; }
        public string TradeUnionistPositionName { get; set; }
        public string SalaryRankName { get; set; }
        public string SalaryFormat { get; set; }
       // public string Allowance1Format { get; set; }
        public string DateEndFortmat { get; set; }
        public string IDDateOfIssueFormat { get; set; }
        public string DateOfBirthFormat { get; set; }
        public string DateNow_Day { get; set; }
        public string DateNow_Month { get; set; }
        public string DateNow_Year { get; set; }
        public string DateNow_Hour { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        public string DateEndProbationFormat { get; set; }
        public Nullable<System.DateTime> DateSigned { get; set; }
        public string DateSignedFormat { get; set; }
        public string ContractNo { get; set; }
        public Nullable<System.Guid> CandidateID { get; set; }
        public string E_COMPANY { get; set; }
        public string E_BRANCH { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public string E_COMPANY_CODE { get; set; }
        public string E_BRANCH_CODE { get; set; }
        public string E_UNIT_CODE { get; set; }
        public string E_DIVISION_CODE { get; set; }
        public string E_DEPARTMENT_CODE { get; set; }
        public string E_TEAM_CODE { get; set; }
        public string E_SECTION_CODE { get; set; }

        public Nullable<DateTime> DateHireFrom { get; set; }
        public Nullable<DateTime> DateHireTo { get; set; }
        public string JobTitleCode { get; set; }
        public string HighSupervisor { get; set; }
        public string Supervisor { get; set; }
        public int? OrderNumber { get; set; }
        public Nullable<System.Guid> SocialInsPlaceID { get; set; }
        public string SocialInsPlaceName { get; set; }
        public Nullable<System.Guid> VillageID { get; set; }
        public string PVillageName { get; set; }
        public Nullable<System.Guid> TAVillageID { get; set; }
        public string TVillageName { get; set; }
        public Nullable<double> LevelEye { get; set; }
        public Nullable<double> LevelEyeRight { get; set; }
        public double? GenaralHealth { get; set; }
        public double? Score3 { get; set; }
        public double? Score1 { get; set; }
        public string Disease { get; set; }
        public string ProfileNameView { get; set; }
        public string CodeCandidate { get; set; }
        public Nullable<System.DateTime> DateExam { get; set; }
        public string StatusSynView { get; set; }
        public string SalaryClassCode { get; set; } 
        public string AbilityTitleVNI { get; set; }
        public string StoredDocuments { get; set; }
        public string ReqDocumentName { get; set; }
        public string ProbationDays { get; set; }
        public string CoatSize { get; set; }
        public string PantSize { get; set; }
        //Bậc Lương
        public Nullable<System.Guid> RankID { get; set; }
        //Đã nghỉ việc chưa
        public bool IsQuit { get; set; }
        //public System.Guid ID { get; set; }
        public DateTime? DateApplyAttendanceCode { get; set; }
        [MaxLength(50)]
        public string StatusSyn { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        [MaxLength(150)]
        public string LastName { get; set; }
        [MaxLength(150)]
        public string FirstName { get; set; }
        [MaxLength(150)]
        public string NameEnglish { get; set; }
        [MaxLength(1000)]
        public string ImagePath { get; set; }
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [MaxLength(50)]
        public string CodeTax { get; set; }
        [MaxLength(50)]
        public string CodeAttendance { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        public string DateNow { get; set; }
        public DateTime? DateHire { get; set; }
        public string DateHireFormat { get; set; }
        public string DateHireString { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string DateStartString { get; set; }
        public string DateStartFormat { get; set; }
        public DateTime? DateEnd { get; set; }
        public string DateEndString { get; set; }
        public string DateEndFormat { get; set; }
        public DateTime? DateEndProbation { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public Guid? PositionID { get; set; }
        public Nullable<int> Settlement { get; set; }
        public Nullable<System.DateTime> MonnthSettlement { get; set; }
        public Nullable<bool> IsSettlement { get; set; }
        public Guid? ProvinceInsuranceID { get; set; }
        [MaxLength(150)]
        public string PositionName { get; set; }
        public string PositionEngName { get; set; }
        public DateTime? DateOfEffect { get; set; }
        public Guid? CostCentreID { get; set; }
        [MaxLength(150)]
        public string CostCentreName { get; set; }
        [MaxLength(1000)]
        public string LaborType { get; set; }
        public string WorkingPlace { get; set; }
        [MaxLength(50)]
        public string Gender { get; set; }
        [MaxLength(50)]
        public string NameByGerder { get; set; }
        public string GraveName { get; set; }
        [MaxLength(50)]
        public string GenderView { get; set; }
        public int? DayOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? YearOfBirth { get; set; }
        [MaxLength(150)]
        public string PlaceOfBirth { get; set; }
        [MaxLength(150)]
        public string PlaceOfBirthName { get; set; }
        public Guid? NationalityID { get; set; }
        public Guid? SalaryClassID { get; set; }
        public string SalaryClassName { get; set; }
        [MaxLength(50)]
        public string NationalityName { get; set; }
        public Guid? EthnicID { get; set; }
        public string EthnicGroupName { get; set; }
        public Guid? ReligionID { get; set; }
        [MaxLength(50)]
        public string ReligionName { get; set; }
        [MaxLength(50)]
        public string BloodType { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        [MaxLength(50)]
        public string IDNo { get; set; }
        public DateTime? IDDateOfIssue { get; set; }
        [MaxLength(150)]
        public string IDPlaceOfIssue { get; set; }
        [MaxLength(150)]
        public string PlaceOfIssueName { get; set; }
        [MaxLength(50)]
        public string PassportNo { get; set; }
        public DateTime? PassportDateOfExpiry { get; set; }
        public DateTime? PassportDateOfIssue { get; set; }
        [MaxLength(150)]
        public string PassportPlaceOfIssue { get; set; }
        [MaxLength(150)]
        public string PassportPlaceOfIssueName { get; set; }
        [MaxLength(150)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Cellphone { get; set; }
        [MaxLength(50)]
        public string HomePhone { get; set; }
        [MaxLength(50)]
        public string BusinessPhone { get; set; }
        public Guid? PAddressID { get; set; }
        public Guid? TAddressID { get; set; }
        public Guid? JobTitleID { get; set; }
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        public string JobTitleNameEn { get; set; }
        public Guid? EmpTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        [MaxLength(150)]
        public string MarriageStatus { get; set; }
        [MaxLength(150)]
        public string MarriageStatusView { get; set; }
        public Guid? SupervisorID { get; set; }
        public Guid? HighSupervisorID { get; set; }
        public DateTime? DateQuit { get; set; }
        public string DateQuitFormat { get; set; }
        public Guid? RegionID { get; set; }
        public string RegionName { get; set; }
        public string OrgStructureCode { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }
        #region Địa chỉ tạm trú/ thường trú

        public Nullable<System.Guid> TCountryID { get; set; }
        public string TCountryName { get; set; }
        public Nullable<System.Guid> TProvinceID { get; set; }
        public string TProvinceName { get; set; }
        public Nullable<System.Guid> TDistrictID { get; set; }
        public string TDistrictName { get; set; }
        public string TAddress { get; set; }
        public Nullable<System.Guid> PCountryID { get; set; }
        public string PCountryName { get; set; }
        public Nullable<System.Guid> PProvinceID { get; set; }
        public string PProvinceName { get; set; }
        public Nullable<System.Guid> PDistrictID { get; set; }
        public string PDistrictName { get; set; }
        public string PAddress { get; set; }

        #endregion
      
        [MaxLength(150)]
        public string SupervisorName { get; set; }
        [MaxLength(150)]
        public string HighSupervisorName { get; set; }
        public string WorkPlaceName { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string NameFamily { get; set; }
        public string Email2 { get; set; }
        public string AddressEmergency { get; set; }

        //public int TotalRow { get; set; }
        // Thông tin bảo hiểm
        public string HealthInsNo { get; set; }
        public DateTime? HealthInsIssueDate { get; set; }
        public DateTime? HealthInsExpiredDate { get; set; }
        public string HealthTreatmentPlace { get; set; }
        public bool? ReceiveHealthIns { get; set; }
        public DateTime? ReceiveHealthInsDate { get; set; }
        public DateTime? UnEmpInsDateReg { get; set; }
        public string LaborBookNo { get; set; }
        public DateTime? LaborBookIssueDate { get; set; }
        public string WorkPermitStatus { get; set; }
        public DateTime? WorkPermitInsDate { get; set; }
        public int? UnEmpInsCountMonthOld { get; set; }
        public string SocialInsNo { get; set; }
        public string SocialInsOldNo { get; set; }
        public DateTime? SocialInsIssueDate { get; set; }
        public string SocialInsIssueDateFormat { get; set; }
        public DateTime? SocialInsDateReg { get; set; }
        public bool? ReceiveSocialIns { get; set; }
        public DateTime? ReceiveSocialInsDate { get; set; }
        public string SocialInsSubmitBookStatus { get; set; }
        public DateTime? SocialInsSubmitBookDate { get; set; }
        public string WorkPermitNo { get; set; }
        public DateTime? WorkPermitExpiredDate { get; set; }
        public string CommentIns { get; set; }
        public string SocialInsIssuePlace { get; set; }
        public string HealthTreatmentPlaceCode { get; set; }
        public System.Guid? EducationLevelID { get; set; }
        public string EducationLevelName { get; set; }

        public bool? IsRegisterSocialIns { get; set; }
        public bool? IsRegisterHealthIns { get; set; }
        public bool? IsRegisterUnEmploymentIns { get; set; }
        public string FileStore { get; set; }
        public Guid? BankID { get; set; }
        public string AccountNo { get; set; }

        // Thông tin nghỉ việc
        public DateTime? RequestDate { get; set; }
        public string RequestDateFormat { get; set; }
        public Guid? ResReasonID { get; set; }
        public Guid? PlaceOfIssueID { get; set; }
        public string ResignReasonName { get; set; }
        [MaxLength(1000)]
        public string ResonBackList { get; set; }
        public bool? IsBlackList { get; set; }
        public string ResignNo { get; set; }
        public DateTime? DateQuitSign { get; set; }
        public string DateQuitSignFormat { get; set; }
        public DateTime? DateQuitRequest { get; set; }
        public string ProfileSign { get; set; }

        public Guid? PayrollGroupID { get; set; }
        public string PayrollGroupName { get; set; }

        public string Birthday { get; set; }
        public string Origin { get; set; }
        public Nullable<System.DateTime> DateOfIssuedTaxCode { get; set; }
        public Nullable<bool> IsHeadDept { get; set; }
        public string LocationCode { get; set; }
        public string FormType { get; set; }
        public string SikillLevel { get; set; }
        public Nullable<System.Guid> GraduatedLevelID { get; set; }
        public string GraduatedLevelName { get; set; }
        public string DeptPath { get; set; }
        public Nullable<System.Guid> ShopID { get; set; }
        public string ShopName { get; set; }
        public string PositionCode { get; set; }


        #region Bảo Hiểm
        public bool? IsNonHaveGradeIns { get; set; }
        public DateTime? MonthBeginInsSocial { get; set; }
        public DateTime? MonthBeginInsHealth { get; set; }
        public DateTime? MonthBeginInsUnEmp { get; set; }
        public bool? IsHaveInsSocial { get; set; }
        public bool? IsHaveInsHealth { get; set; }
        public bool? IsHaveInsUnEmp { get; set; }
        /// <summary>Có Sinh Con Không?</summary>
        public bool? IsPregnant{ get; set; }

        /// <summary>Tiền Lương BHXH</summary>
        public double? MoneyInsuranceTotal { get; set; }
        /// <summary>Tiền Lương BHYT</summary>
        public double? MoneyInsuranceHealthTotal { get; set; }
        /// <summary>Tiền Lương BHTN</summary>
        public double? MoneyInsuranceUnEmpTotal { get; set; }
         /// <summary>Tiền HDTJob</summary>
        public double? AmountHDTIns { get; set; }
        

        public double? MoneyInsuranceSocial { get; set; }
        public double? MoneyInsuranceHealth { get; set; }
        public double? MoneyInsuranceUnEmp { get; set; }
        public string Allowance1Format { get; set; }
        public string Allowance2Format { get; set; }
        public string Allowance3Format { get; set; }
        public string Allowance4Format { get; set; }
        public double? Allowance1 { get; set; }
        public double? Allowance2 { get; set; }
        public double? Allowance3 { get; set; }
        public double? Allowance4 { get; set; }
        public double? AllowanceAdditional { get; set; }
        public double? AmountChargeIns { get; set; }
        public double? SocialInsEmpRate { get; set; }
        public double? HealthInsEmpRate { get; set; }
        public double? UnemployEmpRate { get; set; }
        public double? SocialInsComRate { get; set; }
        public double? HealthInsComRate { get; set; }
        public double? UnemployComRate { get; set; }
        public double? SocialInsEmpAmount { get; set; }
        public double? HealthInsEmpAmount { get; set; }
        public double? UnemployEmpAmount { get; set; }
        public double? SocialInsComAmount { get; set; }
        public double? HealthInsComAmount { get; set; }
        public double? UnemployComAmount { get; set; }
        public string JobName { get; set; }
        /// <summary> Số ngày không làm việc HDTJob </summary>
        public int? NumdayNonHDTJob { get; set; }
        /// <summary> Số ngày làm việc HDTJob thuộc loại 4</summary>
        public int? NumdayHDTJobTypeIV { get; set; }
        /// <summary> Số ngày làm việc HDTJob thuộc loại 5</summary>
        public int? NumdayHDTJobTypeV { get; set; }
        /// <summary> Có nghỉ 14 ngày không lương ? </summary>
        public bool? IsDecreaseWorkingDays { get; set; }
        /// <summary> Mã HDTJobGroupCode </summary>
        public string HDTJobGroupCode { get; set; }

        #endregion

        public string ReasonDeny { get; set; }
        public string StatusHire { get; set; }
        public string listId { get; set; }

        public Nullable<bool> IsHoldSal { get; set; }

        public int? DayOfDateHire { get; set; }
        public int? MonthOfDateHire { get; set; }
        public int? YearOfDateHire { get; set; }
        public string DateHireFormatEN { get; set; }
        public int? DayOfDateProbation { get; set; }
        public int? MonthOfDateProbation { get; set; }
        public int? YearOfDateProbation { get; set; }
        public string DateEndProbationFormatEN { get; set; }
        public double? Salary { get; set; }

        public string TypeSuspense { get; set; }
        public string StopWorkType { get; set; }
        public Nullable<System.Guid> ContractTypeID { get; set; }
        public string ContractTypeName { get; set; }
        public string TypeOfStop { get; set; }

        public double? BasicSalary { get; set; }
        public bool? IsCash { get; set; }

        public Nullable<bool> IsUnEmpInsurance { get; set; }
        public Nullable<bool> IsSocialInsurance { get; set; }
        public Nullable<bool> IsHealthInsurance { get; set; }
        public Nullable<System.DateTime> DateStop { get; set; }

         public string UnitNameOrg { get; set; }
         public string DivisionNameOrg { get; set; }
         public string DepartmentNameOrg { get; set; }
         public string SectionNameOrg { get; set; }
        public string TeamNameOrg { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public string CostCentreCode { get;set; }
        public string TypeOfStopName { get; set; }
        public Nullable<System.Guid> TypeOfStopID { get; set; }
        public string ShopGroupName { get; set; }
        public string CodeEmpClient { get; set; }

        // Lấy thông tin phụ cấp
        public string AllowanceID1Name { get; set; }
        public string AllowanceID2Name { get; set; }
        public string AllowanceID3Name { get; set; }
        public string AllowanceID4Name { get; set; }
        public string AllowanceID5Name { get; set; }

        public Nullable<System.Guid> CostSourceID { get; set; }
        public string CostSourceName { get; set; }
        public Nullable<System.Guid> VehicleID { get; set; }
        public string VehicleName { get; set; }
        public string ShoeSize { get; set; }
        public string NameContactForEmergency { get; set; }
        public string CellPhoneForEmergency { get; set; }
        public Nullable<bool> IsPeriodicExamination { get; set; }
        public string FileAttach { get; set; }

    }
    public class Hre_ProfileMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string StatusSyn { get; set; }
    }

    public class Hre_ProfileMultiSearchEntity
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
    }

    public class Hre_ProfileIdEntity
    {
        public Guid ID { get; set; }
        public string ProfileName { get; set; }
    }

    public class Hre_ProfileCodeEntity
    {
        public Guid ID { get; set; }
        public string CodeEmp { get; set; }
        public string CodeAttendance { get; set; }
    }

    public class ProfileDateTime
    {
        public Guid ProfileID { get; set; }
        public DateTime WorkDate { get; set; }
        public Guid OvertimeID { get; set; }
    }

    public class Hre_PersionalInformationEntity
    {
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public DateTime? DateHire { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Gender { get; set; }
        public string IDNo { get; set; }
        public string IDDateOfIssue { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string PassportNo { get; set; }
        public string PassportDateOfExpiry { get; set; }
        public string PassportDateOfIssue { get; set; }
        public string PassportPlaceOfIssue { get; set; }
        public string PAddressID { get; set; }
        public string TAddressID { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public DateTime? DateQuit { get; set; }
        public string ImagePath { get; set; }
        public DateTime? DateEffective { get; set; }
        public string OrgStructureNew { get; set; }
        public string OrgStructureOld { get; set; }
        public string PositionNew { get; set; }
        public string PositionOld { get; set; }
        public string JobTitleNew { get; set; }
        public string JobTitleOld { get; set; }
        public string Note { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string ContractTypeID { get; set; }
        public string Salary { get; set; }
        public string DateOfViolation { get; set; }
        public string ContentOfDiscipline { get; set; }
        public string ViolationRule { get; set; }
        public DateTime? DateOfEffective { get; set; }
     
      
    }

    public class Hre_ProfileMultiField
    {
        public Guid ID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public Guid? OrgStructureID { get; set; }
        public Guid? PayrollGroupID { get; set; }
        public DateTime? DateHire { get; set; }
        public string OrgStructureName { get; set; }
        public Guid? PositionID { get; set; }
        public DateTime? DateQuit { get; set; }
        public DateTime? DateEndProbation { get; set; }
        public Guid? JobTitleID { get; set; }
        
        
        
    }

}
