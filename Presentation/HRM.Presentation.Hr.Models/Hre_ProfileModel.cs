using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ProfileModel : BaseViewModel
    {
        public Nullable<System.Guid> AbilityTileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_IsTradeUnionist)]
        public Nullable<bool> IsTradeUnionist { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ProfilePartyUnion_TradeUnionistEnrolledDate)]
        public Nullable<System.DateTime> TradeUnionistEnrolledDate { get; set; }
        public Nullable<System.Guid> TradeUnionistPositionID { get; set; }
        public string TradeUnionistPositionName { get; set; }
        public string NameByGerder { get; set; }
        public string SalaryRankName { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<System.Guid> CandidateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsPeriodicExamination)]
        public Nullable<bool> IsPeriodicExamination { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NameContactForEmergency)]
        public string NameContactForEmergency { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhoneForEmergency)]
        public string CellPhoneForEmergency { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostSourceID)]
        public Nullable<System.Guid> CostSourceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostSourceID)]
        public string CostSourceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_VehicleID)]
        public Nullable<System.Guid> VehicleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_VehicleID)]
        public string VehicleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ShoeSize)]
        public string ShoeSize { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmpClient)]
        public string CodeEmpClient { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public Nullable<System.Guid> SocialInsPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsPlaceID)]
        public string SocialInsPlaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Village)]
        public Nullable<System.Guid> VillageID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Village)]
        public string PVillageName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Village)]
        public Nullable<System.Guid> TAVillageID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Village)]
        public string TVillageName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_RegionID)]
        public Guid? RegionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_RegionID)]
        public string RegionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CodeCandidate)]
        public string CodeCandidate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_DateExam)]
        public Nullable<System.DateTime> DateExam { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_Code)]
        public string SalaryClassCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryClass_AbilityTitleVNI)]
        public string AbilityTitleVNI { get; set; }
        public bool IsQuit { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateApplyAttendanceCode)]
        [DataType(DataType.Date)]
        public DateTime? DateApplyAttendanceCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_StatusSyn)]
        public string StatusSyn { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_StatusSyn)]
        public string StatusSynView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_LastName)]
        [MaxLength(150)]
        public string LastName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_FirstName)]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProbationDays)]
        [MaxLength(150)]
        public string ProbationDays { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CoatSize)]
        public string CoatSize { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PantSize)]
        public string PantSize { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NameEnglish)]
        [MaxLength(150)]
        public string NameEnglish { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_StoredDocuments)]
        [MaxLength(150)]
        public string StoredDocuments { get; set; }

        public string StoredDocumentCodes { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_StoredDocuments)]
        [MaxLength(150)]
        public string ReqDocumentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ImagePath)]
        [MaxLength(1000)]
        public string ImagePath { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeTax)]
        [MaxLength(50)]
        public string CodeTax { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeAttendance)]
        [MaxLength(50)]
        public string CodeAttendance { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        [DataType(DataType.Date)]
        public DateTime? DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateEndProbation)]
        [DataType(DataType.Date)]
        public DateTime? DateEndProbation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateOfEffect)]
        [DataType(DataType.Date)]
        public DateTime? DateOfEffect { get; set; }

        public DateTime? DateOfEffectOld { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostCentreID)]
        public Guid? CostCentreID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostCentreName)]
        [MaxLength(150)]
        public string CostCentreName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        [MaxLength(1000)]
        public string WorkingPlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        [MaxLength(50)]
        public string Gender { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        [MaxLength(50)]
        public string GenderView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DayOfBirth)]
        public int? DayOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_MonthOfBirth)]
        public int? MonthOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_YearOfBirth)]
        public int? YearOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlaceOfBirth)]
        [MaxLength(150)]
        public string PlaceOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlaceOfBirth)]
        [MaxLength(150)]
        public string PlaceOfBirthName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_NationalityID)]
        public Guid? NationalityID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NationalityName)]
        [MaxLength(150)]
        public string NationalityName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EthnicID)]
        public Guid? EthnicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EthnicGroupName)]
        [MaxLength(150)]
        public string EthnicGroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReligionID)]
        public Guid? ReligionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReligionName)]
        [MaxLength(150)]
        public string ReligionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_BloodType)]
        public string BloodType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Height)]
        public double? Height { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Weight)]
        public double? Weight { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        [MaxLength(50)]
        public string IDNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDDateOfIssue)]
        [DataType(DataType.Date)]
        public DateTime? IDDateOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        public string IDPlaceOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        [MaxLength(150)]
        public string PlaceOfIssueName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportNo)]
        [MaxLength(50)]
        public string PassportNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportDateOfExpiry)]
        [DataType(DataType.Date)]
        public DateTime? PassportDateOfExpiry { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportDateOfIssue)]
        [DataType(DataType.Date)]
        public DateTime? PassportDateOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportPlaceOfIssue)]
        public string PassportPlaceOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportPlaceOfIssue)]
        [MaxLength(150)]
        public string PassportPlaceOfIssueName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Email)]
        [MaxLength(50)]
        public string Email { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        [MaxLength(50)]
        public string Cellphone { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HomePhone)]
        [MaxLength(50)]
        public string HomePhone { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_BusinessPhone)]
        [MaxLength(50)]
        public string BusinessPhone { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeTypeName)]
        public Guid? EmpTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeTypeName)]
        [MaxLength(150)]
        public string EmployeeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_MarriageStatus)]
        [MaxLength(50)]
        public string MarriageStatus { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_MarriageStatus)]
        [MaxLength(50)]
        public string MarriageStatusView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SupervisiorID)]
        public Guid? SupervisorID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HighSupervisiorID)]
        public Guid? HighSupervisorID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        [DataType(DataType.Date)]
        public DateTime? DateHireFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        [DataType(DataType.Date)]
        public DateTime? DateHireTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Notes)]
        [MaxLength(1000)]
        public string Notes { get; set; }

        #region Địa chỉ tạm trú/ thường trú

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TACountry)]
        public Nullable<System.Guid> TCountryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TACountry)]
        public string TCountryName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public Nullable<System.Guid> TProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public string TProvinceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TADistrict)]
        public Nullable<System.Guid> TDistrictID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TADistrict)]
        public string TDistrictName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAddressID)]
        public string TAddress { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PACountry)]
        public Nullable<System.Guid> PCountryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PACountry)]
        public string PCountryName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAProvince)]
        public Nullable<System.Guid> PProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAProvince)]
        public string PProvinceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PADistrict)]
        public Nullable<System.Guid> PDistrictID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PADistrict)]
        public string PDistrictName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAddressID)]
        public string PAddress { get; set; }

        #endregion
      
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SupervisiorID)]
        [MaxLength(150)]
        public string SupervisorName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HighSupervisiorID)]
        [MaxLength(150)]
        public string HighSupervisorName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_RequestDate)]
        [DataType(DataType.Date)]
        public DateTime? RequestDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ResignReason)]
        public Guid? ResReasonID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ResignReason)]
        public string ResignReasonName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ResignNo)]
        public string ResignNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        [DataType(DataType.Date)]
        public DateTime? DateQuit { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsBlackList)]
        public bool? IsBlackList { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public System.Guid? EducationLevelID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public string EducationLevelName { get; set; }

         [DisplayName(ConstantDisplay.HRM_Category_Export_TemplateFile)]
        public string FileAttach { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Export_TemplateFile)]
         public string TemplateFile { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReasonBackList)]
        [MaxLength(1000)]
        public string ResonBackList { get; set; }

        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }

        public bool IsChecked { get; set; }

        public string JobtitleCode { get; set; }

        public string PositionCode { get; set; }

        public string WorkPlaceCode { get; set; }

        public string EmployeeCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkPlaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public Guid? WorkPlaceID { get; set; }

        public string CostCentreCode { get; set; }

        public string OrgStructureCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_LastName)]
        public string NameFamily { get; set; }

        public string StatusVerify { get; set; }

        // Thông tin bảo hiểm
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthInsNo)]
        public string HealthInsNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthInsIssueDate)]
        [DataType(DataType.Date)]
        public DateTime? HealthInsIssueDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthInsExpiredDate)]
        [DataType(DataType.Date)]
        public DateTime? HealthInsExpiredDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthTreatmentPlace)]
        public string HealthTreatmentPlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReceiveHealthIns)]
        public bool? ReceiveHealthIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateReceiveHealthIns)]
        [DataType(DataType.Date)]
        public DateTime? ReceiveHealthInsDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_UnEmpInsDateReg)]
        [DataType(DataType.Date)]
        public DateTime? UnEmpInsDateReg { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_LaborBookNo)]
        public string LaborBookNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_LaborBookIssueDate)]
        [DataType(DataType.Date)]
        public DateTime? LaborBookIssueDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPermitStatus)]
        public string WorkPermitStatus { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPermitInsDate)]
        [DataType(DataType.Date)]
        public DateTime? WorkPermitInsDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_UnEmpInsCountMonthOld)]
        public int? UnEmpInsCountMonthOld { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsNo)]
        public string SocialInsNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsOldNo)]
        public string SocialInsOldNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsIssueDate)]
        [DataType(DataType.Date)]
        public DateTime? SocialInsIssueDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsDateReg)]
        [DataType(DataType.Date)]
        public DateTime? SocialInsDateReg { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReceiveSocialIns)]
        public bool? ReceiveSocialIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReceiveSocialInsDate)]
        [DataType(DataType.Date)]
        public DateTime? ReceiveSocialInsDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsSubmitBookStatus)]
        public string SocialInsSubmitBookStatus { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsSubmitBookDate)]
        [DataType(DataType.Date)]
        public DateTime? SocialInsSubmitBookDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPermitNo)]
        public string WorkPermitNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPermitExpiredDate)]
        [DataType(DataType.Date)]
        public DateTime? WorkPermitExpiredDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CommentIns)]
        public string CommentIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsIssuePlace)]
        public string SocialInsIssuePlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthTreatmentPlaceCode)]
        public string HealthTreatmentPlaceCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Email2)]
        public string Email2 { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_AddressEmergency)]
        public string AddressEmergency { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsRegisterSocialIns)]
        public bool? IsRegisterSocialIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsRegisterHealthIns)]
        public bool? IsRegisterHealthIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsRegisterUnEmploymentIns)]
        public bool? IsRegisterUnEmploymentIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuitSign)]
        [DataType(DataType.Date)]
        public DateTime? DateQuitSign { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuitRequest)]
        [DataType(DataType.Date)]
        public DateTime? DateQuitRequest { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileSign)]
        public string ProfileSign { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_FileStore)]
        public string FileStore { get; set; }

        public string ScreenName { get; set; }
        //public Guid ExportID { get; set; }
        //public ExportFileType ExportType { get; set; }
        /// <summary>
        /// [Tho.Bui]: Model Hre_ProfilePartyUnion
        /// </summary>
        /// 
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Origin)]
        public string Origin { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateOfIssuedTaxCode)]
        public Nullable<System.DateTime> DateOfIssuedTaxCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsHeadDept)]
        public Nullable<bool> IsHeadDept { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_LocationCode)]
        public string LocationCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_FormType)]
        public string FormType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_LaborType)]
        public string LaborType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SikillLevel)]
        public string SikillLevel { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PayrollGroupID)]
        public Nullable<System.Guid> PayrollGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeGroup)]
        public string PayrollGroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_GraduatedLevelID)]
        public Nullable<System.Guid> GraduatedLevelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_GraduatedLevelID)]
        public string GraduatedLevelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DeptPath)]
        public string DeptPath { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_ShopName)]
        public Nullable<System.Guid> ShopID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Grade_GradeAttendanceID)]
        public string GradeAttendanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_MonthStart)]
        public Nullable<DateTime> MonthStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Sal_Grade_MonthEnd)]
        public Nullable<DateTime> MonthEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shop_ShopName)]
        public string ShopName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReasonDeny)]
        public string ReasonDeny { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_StatusHire)]
        public string StatusHire { get; set; }
        public string listId { get; set; }

         [DisplayName(ConstantDisplay.HRM_HR_Profile_Settlement)]
        public Nullable<int> Settlement { get; set; }
          [DisplayName(ConstantDisplay.HRM_HR_Profile_DateSettlement)]
         public Nullable<System.DateTime> MonnthSettlement { get; set; }
         public Nullable<bool> IsSettlement { get; set; }
        public Nullable<bool> IsHoldSal { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TypeSuspense)]
        public string TypeSuspense { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_StopWorkType_Edit)]
        public string StopWorkType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SalaryClassID)]
        public Guid? SalaryClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SalaryClassName)]
        public string SalaryClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ContractTypeID)]
        public Nullable<System.Guid> ContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ContractTypeID)]
        public string ContractTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TypeOfStop)]
        public string TypeOfStop { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public string CourseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_LevelEye)]
        public Nullable<double> LevelEye { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_LevelEyeRight)]
        public Nullable<double> LevelEyeRight { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_GenaralHealth)]
        public double? GenaralHealth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Score3)]
        public double? Score3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Score1)]
        public double? Score1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_DiseaseListIDs)]
        public string Disease { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TypeOfStop)]
        public Nullable<System.Guid> TypeOfStopID { get; set; }
        public string TypeOfStopName { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmpClient = "CodeEmpClient";
            public const string MonnthSettlement = "MonnthSettlement";
            public const string Settlement = "Settlement";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string Disease = "Disease";
            public const string Score1 = "Score1";
            public const string Score3 = "Score3";
            public const string GenaralHealth = "GenaralHealth";
            public const string LevelEyeRight = "LevelEyeRight";
            public const string LevelEye = "LevelEye";
            public const string TypeOfStop = "TypeOfStop";
            public const string CourseName = "CourseName";
            public const string ContractTypeName = "ContractTypeName";
            public const string CodeCandidate = "CodeCandidate";
            public const string DateExam = "DateExam";
            public const string AbilityTitleVNI = "AbilityTitleVNI"; 
            public const string SalaryClassCode = "SalaryClassCode";
            public const string SalaryClassName = "SalaryClassName";
            public const string StatusSyn = "StatusSyn";
            public const string StatusSynView = "StatusSynView";
            public const string IsQuit = "IsQuit";
            public const string ShopName = "ShopName";
            public const string ShopID = "ShopID";
            public const string DeptPath = "DeptPath";
            public const string DateOfIssuedTaxCode = "DateOfIssuedTaxCode";
            public const string Origin = "Origin";
            public const string Email2 = "Email2";
            public const string SocialInsIssuePlace = "SocialInsIssuePlace";
            public const string HealthInsNo = "HealthInsNo";
            public const string HealthInsIssueDate = "HealthInsIssueDate";
            public const string HealthInsExpiredDate = "HealthInsExpiredDate";
            public const string HealthTreatmentPlace = "HealthTreatmentPlace";
            public const string ReceiveHealthIns = "ReceiveHealthIns";
            public const string ReceiveHealthInsDate = "ReceiveHealthInsDate";
            public const string UnEmpInsDateReg = "UnEmpInsDateReg";
            public const string LaborBookNo = "LaborBookNo";
            public const string LaborBookIssueDate = "LaborBookIssueDate";
            public const string WorkPermitStatus = "WorkPermitStatus";
            public const string WorkPermitInsDate = "WorkPermitInsDate";
            public const string UnEmpInsCountMonthOld = "UnEmpInsCountMonthOld";
            public const string SocialInsNo = "SocialInsNo";
            public const string SocialInsOldNo = "SocialInsOldNo";
            public const string SocialInsIssueDate = "SocialInsIssueDate";
            public const string SocialInsDateReg = "SocialInsDateReg";
            public const string ReceiveSocialIns = "ReceiveSocialIns";
            public const string ReceiveSocialInsDate = "ReceiveSocialInsDate";
            public const string SocialInsSubmitBookStatus = "SocialInsSubmitBookStatus";
            public const string SocialInsSubmitBookDate = "SocialInsSubmitBookDate";
            public const string WorkPermitNo = "WorkPermitNo";
            public const string WorkPermitExpiredDate = "WorkPermitExpiredDate";
            public const string NameFamily = "NameFamily";
            public const string WorkPlaceID = "WorkPlaceID";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string RequestDate = "RequestDate";
            public const string ResignNo = "ResignNo";
            public const string ResReasonID = "ResReasonID";
            public const string ResignReasonName = "ResignReasonName";
            public const string ResonBackList = "ResonBackList";
            public const string IsChecked = "IsChecked";
            public const string ID = "ID";
            public const string TAddress = "TAddress";
            public const string TCountryID = "TCountryID";
            public const string TCountry = "TCountry";
            public const string TProvinceID = "TProvinceID";
            public const string TProvince = "TProvince";
            public const string TDistrictID = "TDistrictID";
            public const string TDistrict = "TDistrict";
            public const string PAddress = "PAddress";
            public const string PCountryID = "PCountryID";
            public const string PCountry = "PCountry";
            public const string PProvinceID = "PProvinceID";
            public const string PProvince = "PProvince";
            public const string PDistrictID = "PDistrictID";
            public const string PDistrict = "PDistrict";
            public const string ProfileName = "ProfileName";
            public const string LastName = "LastName";
            public const string FirstName = "FirstName";
            public const string NameEnglish = "NameEnglish";
            public const string ImagePath = "ImagePath";
            public const string CodeEmp = "CodeEmp";
            public const string CodeTax = "CodeTax";
            public const string CodeAttendance = "CodeAttendance";
            public const string Status = "Status";
            public const string DateHire = "DateHire";
            public const string DateEndProbation = "DateEndProbation";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionID = "PositionID";
            public const string PositionName = "PositionName";
            public const string DateOfEffect = "DateOfEffect";
            public const string CostCentreID = "CostCentreID";
            public const string CostCentreName = "CostCentreName";
            public const string WorkingPlace = "WorkingPlace";
            public const string Gender = "Gender";
            public const string GenderView = "GenderView";
            public const string DayOfBirth = "DayOfBirth";
            public const string MonthOfBirth = "MonthOfBirth";
            public const string YearOfBirth = "YearOfBirth";
            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string PlaceOfBirthName = "PlaceOfBirthName";
            public const string NationalityID = "NationalityID";
            public const string NationalityName = "NationalityName";
            public const string EthnicID = "EthnicID";
            public const string EthnicGroupName = "EthnicGroupName";
            public const string ReligionID = "ReligionID";
            public const string ReligionName = "ReligionName";
            public const string BloodType = "BloodType";
            public const string Height = "Height";
            public const string Weight = "Weight";
            public const string IDNo = "IDNo";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string PlaceOfIssueName = "PlaceOfIssueName";
            public const string PassportNo = "PassportNo";
            public const string PassportDateOfExpiry = "PassportDateOfExpiry";
            public const string PassportDateOfIssue = "PassportDateOfIssue";
            public const string PassportPlaceOfIssue = "PassportPlaceOfIssue";
            public const string PassportPlaceOfIssueName = "PassportPlaceOfIssueName";
            public const string Email = "Email";
            public const string Cellphone = "Cellphone";
            public const string HomePhone = "HomePhone";
            public const string BusinessPhone = "BusinessPhone";
            public const string PAddressID = "PAddressID";
            public const string TAddressID = "TAddressID";
            public const string JobTitleID = "JobTitleID";
            public const string JobTitleName = "JobTitleName";
            public const string EmpTypeID = "EmpTypeID";
            public const string EmployeeTypeName = "EmployeeTypeName";
            public const string MarriageStatus = "MarriageStatus";
            public const string MarriageStatusView = "MarriageStatusView";
            public const string SupervisorID = "SupervisiorID";
            public const string HighSupervisorID = "HighSupervisiorID";
            public const string IsBlackList = "IsBlackList";
            public const string DateQuit = "DateQuit";
            public const string Notes = "Notes";
            public const string DateUpdate = "DateUpdate";
            public const string ReasonDeny = "ReasonDeny";
            public const string StatusHire = "StatusHire";
            public const string Channel = "Channel";
            public const string Region = "Region";
            public const string StoredDocuments = "StoredDocuments";
            public const string ReqDocumentName = "ReqDocumentName";
            public const string PCountryName = "PCountryName";
            public const string PProvinceName = "PProvinceName";
            public const string PDistrictName = "PDistrictName";
            public const string TemplateFile = "TemplateFile";
            public const string FileAttach = "FileAttach";
        }
    }

    public class Hre_PersionalInformationModel
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
    public class Hre_PersionalInfoSearchModel
    {
        public string id { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
    }
    public class Hre_ProfilenotGradeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        public string ValueFields { get; set; }
        
    }
    public class Hre_ProfileSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        //public Nullable<DateTime> DateQuit { get; set; }

        //public Nullable<DateTime> DtToQuit { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ProfileWaitingHireSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_WorkPlaceID)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Hre_ProfileMultiModel
    {
        public Guid ID { get; set; } 
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public int TotalRow { get; set; }
        public string StatusSyn { get; set; }
    }
    public class Hre_ProfileMultiSearchModel
    {
        public string Keyword { get; set; }
        public string OrgStructureID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ProfileGeneralMultiSearchModel
    {
        public Guid? ProfileID { get; set; }
        public string Keyword { get; set; }
        public string OrgStructureID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ProfileSearchIsBackListModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        public Nullable<DateTime> DateQuit { get; set; }
        public Nullable<DateTime> DtToQuit { get; set; }
        public bool? IsExport { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportID { get; set; }
        public ExportFileType ExportType { get; set; }
        public string ValueFields { get; set; }
        //public Guid? ExportID { get; set; }
    }
    public class Hre_ProfileProbationSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateHireFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateHireTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SalaryClassID)]
        public Guid? SalaryClassID { get; set; }
        public bool? IsExCludeQuitEmp { get; set; }
        public string Status { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ProfileAllSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        public string StatusSynz { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ProfileActiveSearchModel : BaseViewSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ProfileQuitSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        public Guid? ResreasonID { get; set; }
        public Guid? TypeOfStopID { get; set; }
        public Guid? workPlaceID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_OrgStructureDetailsSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Hre_ProfileRetirementSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Hre_ReportNotFullDataSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmpTypeID)]
        public Guid? EmpTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ProfileWorkPermitExpireModel
    {
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public DateTime? WorkPermitExpiredDate { get; set; }
    }
}
