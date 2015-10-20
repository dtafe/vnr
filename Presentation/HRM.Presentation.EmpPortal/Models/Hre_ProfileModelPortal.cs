using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRM.Presentation.EmpPortal.Models
{
    public class Hre_ProfileModelPortal : BaseModelPortal
    {
        #region [Hien.Nguyen]
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_LateEarlyDeductionHours)]
        public double LateEarlyDeductionHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_Note)]
        [MaxLength(1000)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_StdWorkDayCount)]
        public double StdWorkDayCount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_RealWorkDayCount)]
        public double RealWorkDayCount { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_PaidWorkDayCount)]
        public double PaidWorkDayCount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_AnlDayAvailable)]
        public double AnlDayAvailable { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_AnlDayTaken)]
        public double AnlDayTaken { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_NightShiftHours)]
        public double NightShiftHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsRegisterSocialIns)]
        public bool? IsRegisterSocialIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsNo)]
        public string SocialInsNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsOldNo)]
        public string SocialInsOldNo { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsIssueDate)]
         [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> SocialInsIssueDate { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsDateReg)]
         [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> SocialInsDateReg { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsIssuePlace)]
        public string SocialInsIssuePlace { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReceiveSocialIns)]
        public Nullable<bool> ReceiveSocialIns { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_ReceiveSocialInsDate)]
         [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ReceiveSocialInsDate { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsSubmitBookStatus)]
        public string SocialInsSubmitBookStatus { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsSubmitBookDate)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> SocialInsSubmitBookDate { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_IsRegisterHealthIns)]
        public Nullable<bool> IsRegisterHealthIns { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthInsNo)]
        public string HealthInsNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthInsIssueDate)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> HealthInsIssueDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthInsExpiredDate)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> HealthInsExpiredDate { get; set; }
          [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthTreatmentPlace)]
        public string HealthTreatmentPlace { get; set; }
            [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthTreatmentPlaceCode)]
        public string HealthTreatmentPlaceCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReceiveHealthIns)]
        public Nullable<bool> ReceiveHealthIns { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateReceiveHealthIns)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ReceiveHealthInsDate { get; set; }
          [DisplayName(ConstantDisplay.HRM_HR_Profile_DateReceiveHealthIns)]
        public Nullable<bool> IsRegisterUnEmploymentIns { get; set; }
           [DisplayName(ConstantDisplay.HRM_HR_Profile_UnEmpInsDateReg)]
           [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> UnEmpInsDateReg { get; set; }
               [DisplayName(ConstantDisplay.HRM_HR_Profile_UnEmpInsCountMonthOld)]
        public Nullable<int> UnEmpInsCountMonthOld { get; set; }
        #endregion
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        //public string NameFamily { get; set; }
        //public string FirstName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NameEnglish)]
        public string NameEnglish { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ImagePath)]
        public string ImagePath { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeTax)]
        public string CodeTax { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeAttendance)]
        public string CodeAttendance { get; set; }
        //public string StatusSyn { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public Nullable<System.DateTime> DateHire { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateEndProbation)]
        public Nullable<System.DateTime> DateEndProbation { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        public Nullable<System.DateTime> DateQuit { get; set; }
        //public string ResignDescription { get; set; }
        //public Nullable<System.Guid> CandidateID { get; set; }
        //public Nullable<System.Guid> WorkGroupID { get; set; }
        //public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        //public Nullable<System.Guid> PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateOfEffect)]
        public Nullable<System.DateTime> DateOfEffect { get; set; }
        //public Nullable<System.Guid> CostCentreID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_CostCentreName)]
        public string CostCentreName { get; set; }
          [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
         public string WorkPlaceName { get; set; }
          public Guid? WorkPlaceID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_LaborType)]
        public string LaborType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkingPlace { get; set; }
        //public string Supervisor { get; set; }
       
       
        //
        //
        //
        //
        //
        //
        //public string HealthInsIssuePlace { get; set; }
        //
        //public string HealthTreatmentProvince { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string GenderView { get; set; }
        
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlaceOfBirth)]
        public string PlaceOfBirth { get; set; }
        //public Nullable<System.Guid> NationalityID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_NationalityName)]
        public string NationalityName { get; set; }
        //public Nullable<System.Guid> EthnicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EthnicGroupName)]
        public string EthnicGroupName { get; set; }
        //public Nullable<System.Guid> ReligionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_BloodType)]
        public string BloodType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Height)]
        public Nullable<double> Height { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Weight)]
        public Nullable<double> Weight { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDDateOfIssue)]

        public Nullable<System.DateTime> IDDateOfIssue { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]

        public string IDPlaceOfIssue { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportNo)]

        public string PassportNo { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportDateOfExpiry)]
        public Nullable<System.DateTime> PassportDateOfExpiry { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportDateOfIssue)]
        public Nullable<System.DateTime> PassportDateOfIssue { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportPlaceOfIssue)]

        public string PassportPlaceOfIssue { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string Cellphone { get; set; }
        public string HomePhone { get; set; }
        //public string BusinessPhone { get; set; }
        //public string PAStreet { get; set; }
        //public Nullable<System.Guid> PADistrictID { get; set; }
        //public Nullable<System.Guid> PACityID { get; set; }
        //public string TAStreet { get; set; }
        //public Nullable<System.Guid> TADistrictID { get; set; }
        //public Nullable<System.Guid> TACityID { get; set; }
        //public string StatusVerify { get; set; }
        //public Nullable<System.Guid> JobTitleID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]

        public string JobTitleName { get; set; }

         [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
         public Guid? JobTitleID { get; set; }
        //public Nullable<System.Guid> TagID { get; set; }
        //public string LabourNo { get; set; }
        //public Nullable<System.DateTime> LabourIssueDate { get; set; }
        //public Nullable<System.Guid> PayrollGroupID { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_PayrollGroup_PayrollGroupName)]
        public string PayrollGroupName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_PayrollGroupID)]
         public Nullable<System.Guid> PayrollGroupID { get; set; }
        //public string LaborBookNo { get; set; }
        //public Nullable<System.DateTime> LaborBookIssueDate { get; set; }
        //public Nullable<System.DateTime> RequestDate { get; set; }
        //public Nullable<System.Guid> EmpTypeID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeTypeName)]

        public string EmployeeTypeName { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeTypeName)]
         public Guid? EmpTypeID { get; set; }

        //public Nullable<System.Guid> EducationLevelID { get; set; }

         [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
         public string EducationLevelName { get; set; }
        //public Nullable<System.Guid> ResReasonID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_MarriageStatus)]

        public string MarriageStatus { get; set; }
        public string MarriageStatusView { get; set; }
         //public string ResignNo { get; set; }
        //public string Forte { get; set; }
        //public string TrainingSkill { get; set; }

        private double _promotionCondition = 0;
         [DisplayName(ConstantDisplay.HRM_REC_Candidate_ProbationDay)]
        public double? PromotionCondition
        {
            get
            {
                if (DateHire != null && DateEndProbation != null)
                {
                    _promotionCondition = DateEndProbation.Value.Subtract(DateHire.Value).TotalDays;
                }

                return _promotionCondition;
            }
            set
            {
                _promotionCondition = 0;
            }
        }
        //public string PromotionRequest { get; set; }
        //public Nullable<System.DateTime> DeadLineApprove { get; set; }
        //public string Notes2 { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_Origin)]
         public string Origin { get; set; }
        //public string ContactName1 { get; set; }
        //public string ContactName2 { get; set; }
        //public string ContactRelation1 { get; set; }
        //public string ContactRelation2 { get; set; }
        //public string ContactPhone1 { get; set; }
        //public string ContactPhone2 { get; set; }
        //public string ContactAddress1 { get; set; }
        //public string ContactAddress2 { get; set; }
        //public string ServerUpdate { get; set; }
        //public string ServerCreate { get; set; }
        //public string UserUpdate { get; set; }
        //public string UserCreate { get; set; }
        //public Nullable<System.DateTime> DateCreate { get; set; }
        //public Nullable<System.DateTime> DateUpdate { get; set; }
        //public Nullable<System.Guid> UserLockID { get; set; }
        //public Nullable<System.DateTime> DateLock { get; set; }
        //public Nullable<bool> IsDelete { get; set; }
        //public string IPCreate { get; set; }
        //public string IPUpdate { get; set; }
        //public string biography { get; set; }
        public string Notes { get; set; }
        //public Nullable<System.Guid> JobTitlePotentinalID { get; set; }
        //public Nullable<System.DateTime> DateFrom { get; set; }
        //public Nullable<System.DateTime> DateTo { get; set; }
        //public string LabourBookPlaceOfIssue { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Smoke)]
        public Nullable<bool> Smoke { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TypeOfVehicle)]
        public string TypeOfVehicle { get; set; }
        //public string DrivingLisenceNo { get; set; }
        //public string StoredDocuments { get; set; }
        //public Nullable<System.Guid> LockerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_LockerName)]
        public string LockerName { get; set; }
        //public string UnEmpInsNo { get; set; }
        //
        //public string UnEmpInsPlace { get; set; }
        //public Nullable<bool> ReceiveUnEmp { get; set; }
        //
        //
        //public string FileStore { get; set; }
        //public string FileAttach { get; set; }
        //
        //public Nullable<int> DayOfBirth { get; set; }
        //public Nullable<int> MonthOfBirth { get; set; }
        //public Nullable<int> YearOfBirth { get; set; }
        //public Nullable<bool> IsBlackList { get; set; }
        //public string HighSupervisor { get; set; }
        //public string Notes3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsDrivingLisence)]
        public Nullable<bool> IsDrivingLisence { get; set; }
        //public Nullable<int> ProbExtendDays { get; set; }
        //public Nullable<bool> IsHeadDept { get; set; }
        //public Nullable<bool> IsInsFollowUp { get; set; }
        //public string CommentIns { get; set; }
        //
        //
        //
        //public string Specialization { get; set; }
        //public Nullable<System.DateTime> ProbExtendDate { get; set; }
        //
        //
        //public string PasswordPaySlip { get; set; }
        //public string WorkPermitStatus { get; set; }
        //public string WorkPermitNo { get; set; }
        //public Nullable<System.DateTime> WorkPermitInsDate { get; set; }
        //public Nullable<System.DateTime> WorkPermitExpiredDate { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_DateOfIssuedTaxCode)]
        public Nullable<System.DateTime> DateOfIssuedTaxCode { get; set; }
        //public Nullable<System.DateTime> DateQuitSign { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_LocationCode)]
        public string LocationCode { get; set; }
        //public Nullable<System.Guid> SupervisorID { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_SupervisiorID)]

        public string SupervisorName { get; set; }
        //public Nullable<System.Guid> HighSupervisorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HighSupervisiorID)]
        public string HighSupervisorName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SupervisiorID)]
        public Guid? SupervisorID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HighSupervisiorID)]
        public Guid? HighSupervisorID { get; set; }
        //public Nullable<System.Guid> WorkPlaceID { get; set; }
        //public string PasswordPaySliptDefault { get; set; }
        //public string SikillLevel { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Profile_DateApplyAttendanceCode)]
        public Nullable<System.DateTime> DateApplyAttendanceCode { get; set; }
        //public string FormType { get; set; }
        //public Nullable<System.Guid> PlaceOfIssueID { get; set; }
        //public string ResonBackList { get; set; }
        //public Nullable<System.DateTime> DatePrepare { get; set; }
        //public Nullable<System.Guid> PlaceOfBirthID { get; set; }
        //public Nullable<System.Guid> OrgLineDefaultID { get; set; }
        //public Nullable<System.Guid> ProducteLineDefaultID { get; set; }
        //public Nullable<System.Guid> ProvinceInsuranceID { get; set; }
        //public Nullable<System.DateTime> DateQuitRequest { get; set; }
        //public string ProfileSign { get; set; }
        //public string AddressEmergency { get; set; }
        //public Nullable<System.Guid> PassportPlaceID { get; set; }
        //public Nullable<System.Guid> VillageID { get; set; }
        //public Nullable<System.Guid> TAVillageID { get; set; }
        //public Nullable<int> Order { get; set; }
        //public string MarkColor { get; set; }
        //public Nullable<bool> IsExistentConcurrentPosition { get; set; }
        //public Nullable<double> PositionRate { get; set; }
        //public Nullable<System.Guid> PAddressID { get; set; }
        //public Nullable<System.Guid> TAddressID { get; set; }
        //public Nullable<bool> IsRegisterSocialIns { get; set; }
        //
        //

        public string PProvince { get; set; }
        public string PDistrict { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAddressID)]
        public string PAddress { get; set; }

        public string TProvince { get; set; }
        public string TDistrict { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAddressID)]
        public string TAddress { get; set; }





        [DisplayName(ConstantDisplay.HRM_HR_Profile_DayOfBirth)]
        public int? DayOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_MonthOfBirth)]
        public int? MonthOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_YearOfBirth)]
        public int? YearOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_NationalityID)]
        public Guid? NationalityID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public System.Guid? EducationLevelID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EthnicID)]
        public Guid? EthnicID { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_LastName)]
        public string NameFamily { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_FirstName)]
        [MaxLength(150)]
        public string FirstName { get; set; }


          [DisplayName(ConstantDisplay.HRM_HR_Profile_TACountry)]
        public Nullable<System.Guid> TCountryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TACountry)]
        public string TCountryName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PACountry)]
        public Nullable<System.Guid> PCountryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PACountry)]
        public string PCountryName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public Nullable<System.Guid> TProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public string TProvinceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TADistrict)]
        public Nullable<System.Guid> TDistrictID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TADistrict)]
        public string TDistrictName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Village)]
        public Nullable<System.Guid> TAVillageID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Village)]
        public string TVillageName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAProvince)]
        public Nullable<System.Guid> PProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAProvince)]
        public string PProvinceName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PADistrict)]
        public Nullable<System.Guid> PDistrictID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PADistrict)]
        public string PDistrictName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Village)]
        public Nullable<System.Guid> VillageID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Village)]
        public string PVillageName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_AddressEmergency)]
        public string AddressEmergency { get; set; }

    }
}