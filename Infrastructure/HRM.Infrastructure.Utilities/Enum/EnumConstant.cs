using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace HRM.Infrastructure.Utilities
{
    public enum UsingUserStatus
    {
        #region UsingUserStatus
        E_INACTIVE = 0, E_ACTIVE
        #endregion
    }
    /// <summary>
    /// Các resource phân quyền trong hệ thống
    /// Format: [Module]__[ResourceName]
    /// </summary>
    public enum OtherResource
    {
        #region System
        //System__Abc,//bat dau la System
        System__Sys_User,
        System__Sys_AttendanceConfig,
        System__Sys_UserSetting,
        System__Sys_ConfigDB,
        System__Sys_ConfigDBCan,
        System__Sys_ConfigDBLau,
        System__Sys_User_Permission,// phân quyền
        System__Sys_ConfigGeneral,//Cấu hình chung
        /// <summary> Nút cập nhật cấu hình mặc định của màn hình "Tất cả cài đặt" </summary>
        System__Sys_AllSetting_ConfigDefault,
        System__Sys_HreConfig,
        System__Sys_SalConfig,
        System__Sys_CustomReport,
        System__Sys_ItemTracking,
        System__Sys_LogTracking,
        System__Sys_SQLCommander,
        /// <summary> Cấu hình đánh giá </summary>
        System__Sys_EvaConfig,
        System__Sys_TraConfig,
        System__Sys_DelegateApprove,
        System__Sys_ApprovedLockObject,
        System__Sys_ConditionApproved,
        System__Sys_ProcessApproved,
        System__Sys_LockObjectConfig,
        System__Sys_TemplateSendMail,
        System__Sys_InsConfig,
        System__Sys_UserChangePassword,
        #endregion

        #region Category
        //Category__Abc,//bat dau la Category
        Category__Cat_OrgStructureType,
        Category__Cat_OrgStructure,
        Category__Cat_JobTile,
        Category__Cat_Position,
        Category__Cat_ContactType,
        Category__Cat_ResignReason,
        Category__Cat_RelativeType,
        Category__Cat_EmployeeType,
        Category__Cat_Shift,
        Category__Cat_OvertimeType,
        Category__Cat_LateEarlyRule,
        Category__Cat_LeaveDayType,
        Category__Cat_DayOff,
        // Category__Cat_TAMScanReasonMiss,
        Category__Cat_TAMScanReasonMissForAtt,//Danh sách ly do không quẹt thẻ
        Category__Cat_EducationLevel,
        Category__Cat_ComputingType,
        Category__Cat_ComputingLevel,
        Category__Cat_LanguageSkill,
        Category__Cat_LockObject,
        Category__Cat_LanguageType,
        Category__Cat_LanguageLevel,
        Category__Cat_GraduatedLevel,
        Category__Pur_MCAM,
        Category__Cat_ExportWord,
        Category__Cat_AdvancePayment,
        Category__Cat_ReasonDeny,
        Category__Cat_JobVancancyReson,
        Category__Cat_Sick,
        Category__Cat_Rank,
        Category__Cat_DisciplineReason,
        Category__Cat_TypeOfTransfer,
        Category__Cat_CutOffDurationType,
        Category__Cat_InsuranceElement,//phần tử tính bảo hiểm
        Category__Cat_BlackListReson,
        Category__Cat_TypeOfStop,
        Category__Cat_Pivot,
        Category__Cat_MasterDataGroup,
        Category__Cat_AreaPostJob,
        Categoty__Cat_ReceivePlace,
        Categoty__Cat_ReceivePlace_Create,
        Categoty__Cat_ReceivePlace_Edit,
        Categoty__Cat_ReceivePlace_Delete,
        Categoty__Cat_ReceivePlace_Export,
        Category__Cat_Vehicle,
        Category__Cat_Vehicle_Create,
        Category__Cat_Vehicle_Delete,
        Category__Cat_Vehicle_Edit,
        Category__Cat_Vehicle_Export,

        Category__Cat_CostSource,
        Category__Cat_CostSource_Create,
        Category__Cat_CostSource_Delete,
        Category__Cat_CostSource_Edit,
        Category__Cat_CostSource_Export,
        Category__Cat_TimeEvalutionData,

        Category__Cat_Model,
        Category__Cat_Model_Create,
        Category__Cat_Model_Edit,
        Category__Cat_Model_Delete,
        Category__Cat_Model_Export,

        Category__Pur_Color,
        Category__Pur_Color_Create,
        Category__Pur_Color_Edit,
        Category__Pur_Color_Delete,
        Category__Pur_Color_Export,
        #endregion

        #region Attendance
        //Attendance__Abc,//bat dau la Attendance
        Attendance__Att_TAM,
        Attendance__Att_ComputeOvertime,
        Attendance__Att_ComputeWorkdayAdjust,
        Attendance__Att_ComputeLeaveLateEarly,
        Attendance__Att_ComputeAttendance,
        Attendance__Att_ProfileNotRoster,
        Attendance__Att_ExceptionOvertime,
        Attendance__Att_AnnualLeaveDetail,
        Attendance__Att_AnnualInsuranceLeaveDetail,
        Attendance__Att_AnalysysLeaveAndLateEarly,
        Attendance__Att_ReportDetailShift,
        Attendance__Att_ReportProfileAllowLimitOvertime,
        Attendance__Att_ReportProfileByShift,
        Attendance__Att_ReportDetailForgetCard,
        Attendance__Att_ReportExceptionData,
        Attendance__Att_ReportSummaryInOut,
        Attendance__Att_ReportWrongShift,
        Attendance__Att_ReportDailyAttendance,
        Attendance__Att_ComputeWorkDay,
        Attendance__Att_ReportSummaryOvertimeMonth,
        Attendance__Att_ReportDetailOvertime,
        Attendance__Att_ReportStatisticsOvertimeByYear,
        Attendance__Att_ReportSumaryLateInOut,
        Attendance__Att_ReportSummaryLateInOut,
        Attendance__Att_ReportLeaveday,
        Attendance__Att_ReportLeaveMonth,
        Attendance__Att_ReportDetailedMonthlyStay,
        Attendance__Att_ReportLeaveYear,
        Attendance__Att_ReportSummarytLeaveYearSick,
        Attendance__Att_ReportDetailLeaveSick,
        Attendance__Att_ReportSummaryLeaveYearSick,
        Attendance__Att_ReportSickLeave,
        Attendance__Att_ReportMonthlyTimeSheet,
        Attendance__Att_ReportMonthly,
        Attendance__Att_ReportDetailSwingCard,
        Attendance__Att_ReportMonthlyRoster,
        Attendance__Att_ReportInOutAdjustment,
        Attendance__Att_ReportDiligenceYear,
        Attendance__Att_ReportMonthlyHourFlightLocal,
        Attendance__Att_CutOffDuration,
        Attendance__Att_Element,
        Attendance__Att_OvertimeConfirm,
        Attendance__Att_ReportCountLateInOut,
        Attendance__Att_ReportLateEarlyTotal,
        Attendance__Att_ReportMonthlyOvertime,
        Attendance__Att_AnalyzeAnnual,
        Attendance__Att_ChangeRosterGroup,


        Attendance__Att_ProfileNotGrade_Appy,
        Attendance__Att_ProfileNotGrade,
        Attendance__Att_ProfileNotGrade_Export,

        Attendance__Att_ApprovedLeaveday,
        Attendance__Att_ApprovedOvertime,
        Attendance__Att_ApprovedRoster,

        Attendance__Att_Roster_Approve,
        Attendance__Att_Roster_Rejected,
        Attendance__Att_Roster_Wait_Approved,
        Attendance__Att_Roster_Submit,

        Attendance__Att_ComputeWorkdayAdjust_Approved,
        Attendance__Att_ComputeWorkdayAdjust_Wait_Approved,
        Attendance__Att_ComputeWorkdayAdjust_CancelLateEarly,
        Attendance__Att_ComputeWorkdayAdjust_UpdateInOut,
        Attendance__Att_ComputeWorkdayAdjust_ChangeInOut,
        Attendance__Att_ComputeWorkdayAdjust_LoadData,


        Attendance__Att_Workday_Rejected,

        Attendance__Att_AnnualLeaveDetail_Compute,//tính phép năm
        Attendance__Att_AnnualSickLeaveDetail,//Phép năm chi tiết hàng năm

        Attendance__Att_RosterGroup_Approve,
        Attendance__Att_RosterGroup_Rejected,


        Attendance__Att_Overtime_Approve,
        Attendance__Att_Overtime_Reject,
        Attendance__Att_Overtime_Payment,
        Attendance__Att_Overtime_Leave,
        Attendance__Att_Overtime_ActionSubmit,
        Attendance__Att_Overtime_ChangeMethodPayment,

        Attendance__Att_ExceptionOvertime_Reject,

        Attendance__Att_LeaveDay_WaitApprove,
        Attendance__Att_LeaveDay_Approved,
        Attendance__Att_LeaveDay_Rejected,
        Attendance__HomeAttendance,
        Attendance__Att_ReportSumaryInOut,
        Attendance__Att_ReportWarringOvertime,
        Attendance__Att_TAM_Config,
        Attendance__Att_OvertimeConfirm_SaveChange,
        Attendance__Att_ReportMonthlyTimeSheetV2,
        Attendance__Att_ReportGeneralMonthlyAttendance,
        Attendance__Att_AnnualSickLeaveDetail_Compute,
        Attendance__Att_ComputeAttendance_Compute,
        Attendance__Att_OvertimeConfirm_Confirm,
        Attendance__Att_ComputeOvertime_Compute,
        Attendance__Att_LeaveDay_Copy,
        Attendance__Att_LeaveDay_UpdateSum,
        Attendance__Att_ComputeLeaveLateEarly_Save,
        Attendance__Att_ComputeLeaveLateEarly_Compute,
        Attendance__Att_ReportExceptionDataAdv,
        Attendance__Att_RptExceptionData,
        Attendance__Att_ReportSumaryShiftMonth,
        Attendance__Att_PersonalSubmitOvertime,
        Attendance__Att_PersonalSubmitLeaveDay,
        Attendance__Att_ReportAnnualDetail,
        Attendance__Att_ConfigFirstYear,
        Attendance__Att_ConfigFirstYearOff,
        Attendance__Att_CompensationDetail,
        Attendance__Att_CompensationDetail_Compute,
        Attendance__Att_EditFutureRoster,
        Attendance__Att_EditPastRoster,
        Attendance__Att_ImportRoster,
        Attendance__Att_ImportLeaveday,
        Attendance__Att_CompensationDetailView,
        Attendance__Att_AnnualDetailView,

        #region Portal
        Attendance__Att_Leaveday_Approve_Portal,
        Attendance__Att_Leaveday_Approve_Approve_Portal,
        Attendance__Att_Leaveday_Approve_Reject_Portal,
        Attendance__Att_Overtime_Approve_Portal,
        Attendance__Att_Overtime_Approve_Approve_Portal,
        Attendance__Att_Overtime_Approve_Reject_Portal,
        Attendance__Att_Leaveday_Submit_Portal,
        Attendance__Att_Overtime_Submit_Portal,
        Attendance__Att_Leaveday_LeavedayList_Portal,
        Attendance__Att_Overtime_OvertimeList_Portal,
        Attendance__Portal_Home_Portal,
        Attendance__Personal_GeneralProfileDetail_Portal,
        Attendance__Portal_ChangePassword_Portal,
        Attendance__Portal_Index_Portal,
        Attendance__Menu_Approve_Portal,
        Attendance__Menu_Request_Portal,

        #endregion

        #endregion

        #region Canteen
        //Canteen__Abc,//bat dau la Canteen
        Canteen__Can_RecordMissing,
        Canteen__Can_MealPriceTypeSetting,
        Canteen__Can_MealRecordSummary,
        Canteen__Can_ComputeBackPay,
        Canteen__Can_ReportAdjustmentMealAllowancePayment,
        Canteen__Can_ReportMealTimeSummary,
        Canteen__Can_ReportMealTimeDetailNoEat,
        Canteen__Can_ReportMealTimeDetail,
        Canteen__Can_ReportCardNotStand,
        Canteen__Can_ReportMultiSlideCard,
        Canteen__Can_ReportDetailCard,
        Canteen__Can_ReportSumaryReturnMoneyEat,
        Canteen__Can_ReportPayMoneyEat,
        Canteen__Can_ReportCardByLocation,
        Canteen__Can_TamScanLogCMS_Download,
        Canteen__Can_TamScanLogCMS_Config,
        Canteen__Can_MealAllowanceToMoneySubmit,
        Canteen__Can_MealAllowanceToMoneyApproved,
        Canteen__Can_RecordMissingSubmit,
        Canteen__Can_RecordMissingApproved,
        Canteen__Can_ReportMealAllowanceOfEmployee,
        Canteen__Can_ReportHDTJobCardMore,
        #endregion

        #region HR
        //HR__Abc,//bat dau la HR
        HR__Hre_OrgStructureDetails,
        HR__Hre_ProfileProbation,
        HR__Hre_ProfileWatingHire,
        HR__Hre_SoftSkill,
        HR__Hre_ReportHDTJob,
        HR__Hre_ReportCodeNotInSystem,
        HR__Hre_ProfileQuit,
        HR__Hre_ProfileWaitingHire,
        HR__GeneralProfile,
        HR__GeneralProfile_ComputeWorkday,
        HR__Hre_ProfileAll,
        HR__Hre_ProfileRetirement,
        HR__Hre_ProfileAttendance,
        //[Tho.Bui-29/10/2014]: thêm quyền
        HR__CatEmployeeType,
        HR__CatJobTitle,
        HR__CatPosition,
        HR__Home,
        HR__Hre_Accident,
        HR__Hre_Address,
        HR__Hre_AppendixContract,
        HR__Hre_CardHistory,
        HR__Hre_Computing,
        HR__Hre_Contract,
        HR__Hre_Dependant,
        HR__Hre_Discipline,
        HR__Hre_ExpiryContract,
        HR__Hre_HDTJob,
        HR__Hre_Language,
        HR__Hre_PlanHeadCount,
        HR__Hre_Profile,
        HR__Hre_ProfilePartyUnion,
        HR__Hre_ProfileProcessWork,
        HR__Hre_Qualification,
        HR__Hre_Relatives,
        HR__Hre_ReportAccident,
        HR__Hre_ReportBirthday,
        HR__Hre_ReportEducationCharList,
        HR__Hre_ReportExpiryContract,
        HR__Hre_ReportHCGender,
        HR__Hre_ReportHCSeniority,
        HR__Hre_ReportHCSales,
        HR__Hre_ReportHistoryProfile,
        HR__Hre_ReportMonthlyHC,
        HR__Hre_ReportPregnancy,
        HR__Hre_ReportProfileDiscipline,
        HR__Hre_ReportProfileIsBackList,
        HR__Hre_ReportProfileNew,
        HR__Hre_ReportProfileNotContract,
        HR__Hre_ReportProfileOrgStructure,
        HR__Hre_ReportProfileProbation,
        HR__Hre_ReportProfileQuit,
        HR__Hre_ReportProfileWorking,
        HR__Hre_ReportReward,
        HR__Hre_ReportSeniority,
        HR__Hre_ReportVisaInfoMoreDateEnd,
        HR__Hre_ReportWorkHistoryDept,
        HR__Hre_Reward,
        HR__Hre_Uniform,
        HR__Hre_WorkHistory,
        HR__Session,
        HR__Hre_StopWorking,
        HR__Hre_ProfileKeepingSalary,
        HR__Hre_ReportNotFullData,
        HR__Hre_ProfileunHire,
        HR__Hre_RegisterSuspense,
        HR__Hre_RegisterComeBack,
        HR__Hre_CandidateGeneral,
        HR__Hre_HDTJobWaitingApproved,
        HR__Hre_ContractWaiting,
        HR__Hre_ReportSummaryDiscipline,
        HR__Hre_EditHDTDataApproved,
        HR__Hre_ProposalSignExpireContract,
        HR__Hre_ReportRelatives,
        HR__Hre_ReportPayHDTJob,
        HR__Hre_ReportProfileWaitingStopWorking,
        HR__Hre_ReportProfileComeBack,
        HR__Hre_ReportProfileHDTInMonth,
        HR__Hre_ReportProfileHDTNotWork,
        HR__Hre_WorkHistoryWaitingApprove,
        HR__Hre_ContractEnd,
        HR__Hre_ReportProfileInformation,
        HR__Hre_ReportContractDetail,
        HR__Hre_ReportContractExpired,
        HR__Hre_ReportProfileInformationMoment,
        HR__Hre_ReportProfileQuitV2,
        HR__Hre_AppendixExpiryContract,
        HR__Hre_ReportSummaryDependant,
        HR__Hre_ReportSumarySeniorHDTProfile,
        HR__Hre_ApprovedWorkingHistory,
        HR__Hre_CancelWorkingHistory,
        HR__Hre_ReportRecieveObjectByTime,
        HR__Hre_ReportHDTJobNotDateEnd,
        HR__Hre_ReportSumaryHDTProfile,
        HR__Hre_ReportUnusualHDT,
        HR__Hre_ProfileSuspense,
        HR__Hre_ReportSummaryDependantDeduction,
        HR__Hre_CancelApproveHDTJob,
        HR__Hre_ReportHCByMonth,
        HR__Hre_AddToBlackListProfile,
        HR__Hre_ReportDetailProfileHDTJob,
        HR__Hre_ReportDependantProfileQuit,
        HR__Hre_EvaluationContractWaitingApproved,
        HR__Hre_ProfileCandidateHistory,
        HR__Hre_RecCandidateHistory,
        HR__Hre_HDTJobOut,
        HR__Hre_ReportHDTJobOut,
        HR__Hre_ReportHDTJobIn,
        HR__Hre_ReportHDTJobDecisionAssignWork,
        #endregion

        #region Laundry
        //Laundry__Abc,//bat dau la Laundry
        Laundry__Lau_Laundry_Record,
        Laundry__Lau_TamScanLog,
        Laundry__LaundryRecord,
        Laundry__Lau_LaundryRecordSummary,
        Laundry__Lau_MachinceOfLine,
        Laundry__Lau_ReportEmpDetail,
        Laundry__Lau_ReportSummayLaundryRecord,
        Laundry__Lau_ReportSummaryClothing,
        //  Laundry__Lau_ReportExceptCothing,
        //Laundry__Lau_ReportSumaryExceptClothing,
        Laundry__Lau_Reader,
        Laundry__Lau_ReportSummaryLaundry,
        Laundry__Lau_ReportClothing,
        Laundry__Lau_ReportExceptClothing,
        Laundry__Lau_ReportSummaryExceptClothing,
        Laundry__Lau_Marker,
        Laundry__Lau_Locker,
        Laundry__Lau_Line,
        Laundry__Lau_TamScanLog_Config,
        Laundry__Lau_TamScanLog_Download,
        Laundry__Lau_Location,
        #endregion

        #region Medical
        //Medical__Abc,//bat dau la Medical

        #endregion

        #region Training
        //Training__Abc,//bat dau la Training
        Training__Tra_Trainee,
        Training__Tra_TraineeFinish,
        Training__Tra_TraineePass,
        Training__Tra_TraineeProgress,
        Training__Tra_TraineeFail,
        Training__Tra_TraineeWithOutProcess,
        Training__Tra_TraineeCertificateExpire,
        Training__Tra_RequirementTrainInSide,
        Training__Tra_ClassFinishInside,
        Training__Tra_ClassFinish,
        Training__Tra_ClassProcessInSide,
        Training__Tra_ClassProcess,
        Training__Tra_PlanInside,
        Training__Tra_TraineeFinishInside,
        Training__Tra_PlanOutside,
        Training__Tra_ReportTraineeJoinCourse,
        Training__Tra_ReportTraineeByMonth,
        Training__Tra_ReportTraineeNotInRequirementDetail,
        Training__Tra_AnalysisTraining,
        Training__Tra_TraineeScoreImport,
        Training__Tra_TraineeImport,
        Training__Tra_ReportClass,
        Training__Tra_ReportTraineeClass,
        Training__Tra_ReportTransferTrainee,
        Training__Tra_ReportTraineeUnAttend,
        Training__Tra_TraineeRegister,
        Training__Tra_ReportTraineeResult,
        Training__Tra_ChangeClass,
        #endregion

        #region Salary
        //Salary__Abc,//bat dau la Salary
        Salary__Sal_ProfileNotGrade_Appy,
        Salary__Sal_ProfileNotGrade,
        Salary__Sal_ProfileNotGrade_Export,
        Salary__Sal_CountAnalyzeHoldSalary_Delele,
        Salary__Sal_CountAnalyzeHoldSalary,
        Salary__Sal_CountAnalyzeHoldSalary_Create,
        Salary__Sal_CountAnalyzeHoldSalary_Edit,
        Salary__Sal_CountAnalyzeHoldSalary_Export,
        Salary__Sal_ComputePayroll,
        Salary__Sal_UnusualED,
        Salary__Sal_ReportSalaryTableMonth,
        Salary__Sal_ReportSalaryTableMonth_Payment,
        Salary__Sal_ReportSalaryMonthly,
        Salary__Sal_ReportUnusualPayItem,
        Salary__Sal_ReportGroupPayrollCostCentre,
        Salary__Sal_CostCentre,
        Salary__Sal_RevenueRecord,
        Salary__Sal_ComputePayroll_Compute,
        Salary__Sal_ComputePayroll_ComputeAdvance,
        Salary__Sal_RevenueRecord_Compute,
        Salary__Sal_UnusualEDChildCare_Approve,
        Salary__Sal_UnusualEDChildCare,
        Salary__Sal_ReportRevenueForShop,
        Salary__Sal_ReportTransferViaBank,
        Salary__Sal_ReportCash,
        Salary__Sal_ReportStaffNumber,
        Salary__Sal_ReportSalaryElement,
        Salary__ReportChildCare,
        Salary__ReportProfileEntitledAllowance,
        Salary__ReportTotalProfileUnusualAllowance,
        Salary__Sal_ComparePayroll,
        Salary__Sal_ReportPIT,
        Salary__Sal_ReportCostCentreByOrg,
        Salary__Sal_ReportHaveChildUnusual,
        Salary__Sal_ReportTotalIncomeByProfile,
        Salary__Sal_ReportTotalAnnualTaxableIncome,
        Salary__Sal_ReportPlanningPayroll,
        Salary__Sal_ReportAllowanceStopWorking,
        Salary__Sal_ReportAmountOTandUnpadLeave,
        Salary__Sal_ReportNotPayAllowancesByHoldSalary,
        Salary__Sal_ReportFilialWedding,
        Salary__Sal_ReportTotalAnnualSick,
        Salary__Sal_CompareUnusualAllowance,
        Salary__Sal_RemittanceAllowSick,
        Salary__Sal_ReportTransferViaBank_ED,
        Salary__Sal_ReportCash_ED,
        Salary__Sal_ReportTotalPITMonthly,
        Salary__Sal_ReportProfile,
        Salary__Sal_AnalyzeHoldSalary,
        Salary__Sal_ReportAllowanceDetail,
        Salary__Sal_ReportTransferUnions,
        Salary__Sal_ReportTransferBonusHold,
        Salary__Sal_ReportBonusEvaDetail,
        Salary__Sal_ReportSalaryAllowanceQuit,
        Salary__Sal_ReportBonusHoliday,
        Salary__Sal_ReportAccountingOfClearing,
        Salary__Sal_ReportProfileNotDeductMoney,
        Salary__Sal_ReportTotalUnusualAllowance,
        Salary__Sal_ReportAllowsAccountingOfSick,
        Salary__Sal_ReportGeneralInsuranceInYear,
        Salary__Sal_ReportChangingTheBasicSalary,
        Salary__Sal_AdjustmentSuggestion,
        Salary__Sal_AllowanceEvaluationYear_Approve,
        Salary__Sal_AllowanceEvaluationYear_Reject,
        Salary__Sal_AllowanceEvaluationYear_Delete,
        Salary__Sal_AllowanceEvaluationYear_Export,
        Salary__Sal_AllowanceEvaluationYear,
        Salary__Sal_HealthInsuranceAndOrther,
        Salary__Sal_ReportGeneralAnnualWage,
        Salary__Sal_ReportKaizenMonthlyCash,
        Salary__Sal_ReportKaizenGeneral,
        Salary__Sal_BonusEvalutionYearInfo,
        Salary__Sal_ComputePayrollSettlement,
        Salary__Sal_ReportPITStopWorking,
        Salary__Sal_ReportVariableSalary,
        Salary__Sal_ReportHistoryPayrollElement,
        Salary__Sal_Kai_KaizenData_Approve,
        Salary__Sal_Kai_KaizenData_WaitApprove,
        Salary__Sal_Kai_KaizenData_MoveForSalary,
        Salary__Sal_Kai_KaizenData_Edit,
        Salary__Sal_ReportPensions,
        Salary__Sal_ReportPayrollEstimate,
        Salary__Sal_ReportHoldSalary,
        Salary__Sal_ReportProfileSalaryNegative,
        Salary__Sal_ReportComparePayroll,



        #endregion

        #region Recruitment
        //Recruitment__Abc,//bat dau la Recuitment
        Recruitment__Rec_Candidate_Approve,
        Recruitment__Rec_Candidate_Reject,
        Recruitment__Rec_Candidate_Call,
        Recruitment__Rec_Candidate_Cancel,
        Recruitment__Rec_Candidate_Appointment,
        Recruitment__Rec_Candidate_Filter,
        Recruitment__GeneralCandidate,
        Recruitment__Rec_WaitingInterviewList,
        Recruitment__Rec_CandidateFailToGetTheJob,
        Recruitment__Rec_EnrolledCandidate,
        Recruitment__Rec_SourceAds,
        Recruitment__Rec_FailCandidate,
        Recruitment__Rec_RecruitmentCampaign_Approved,
        Recruitment__Rec_RecruitmentCampaign_Reject,
        Recruitment__Rec_RecruitmentCampaign_Cancel,
        Recruitment__Rec_RecruitmentCampaign_Active,
        Recruitment__Cat_Sick,
        Recruitment__Rec_GroupCondition,
        Recruitment__Hre_ReportJobVacancy,
        Recruitment__Rec_CandidateInBlackList,
        Recruitment__Rec_ReportJobVacancy,
        Recruitment__Rec_ReportInterview,
        Recruitment__Rec_ReportListCandidate,
        Recruitment__Rec_ImportResultInterview,
        Recruitment__Rec_RecCandidateHistory,
        #endregion

        #region Insurance
        //Insurance__Abc,//bat dau la Insurance
        Insurance__Ins_AnalyzeInsurance,
        Insurance__Ins_ConfigInsurance,
        Insurance__Ins_ReportD02TS,
        Insurance__Ins_ReportC70A,
        Insurance__Ins_ReportNotHaveSocial,
        Insurance__Ins_ReportInsuranceTrackingMonthly,
        Insurance__Ins_ReportJoinInsuranceMonthly,
        Insurance__Ins_ReportJobnameMonthly,
        Insurance__Ins_ReportInsuranceTotal,
        Insurance__Ins_ReportEmpHardJob,
        Insurance__Ins_ReportEmpHardJob2nd,
        Insurance__Ins_ReportD02TSTail,
        Insurance__Ins_ReportInsCostCenterMonthly,
        Insurance__Ins_ReportD03TS,
        Insurance__Ins_InsuranceSettlement,
        #endregion

        #region Evaluation
        //Evaluation__Abc,//bat dau la Evaluation
        Evaluation__Eva_PerformanceProfile,
        Evaluation__Eva_PerformanceEvaProfile,
        Evaluation__Eva_PerformanceEvaWaitingEva,
        Evaluation__Eva_PerformanceEvaActive,
        Evaluation__Eva_ReportEvaluationResult,
        Evaluation__Eva_ReportEvalutionData,
        Evaluation__Eva_KPIBuilding,
        Evaluation__Eva_PerformanceGeneral,
        Evaluation__Eva_KPIBuilding360,
        Evaluation__Eva_PerformanceEvaWaitingEva360,
        Evaluation__Eva_PerformanceEvaDetail360,
        Evaluation__Eva_PerformanceEvaDetail360Approve,
        Evaluation__Eva_Evaluator360,
        Evaluation__Eva_PerformanceGeneral_Approve,
        #endregion

        #region Kaizen
        //Kaizen__Abc,//bat dau la Kaizen
        Kaizen__Kai_KaiZenDataDXCT,
        Kaizen__Kai_ReportKaizenDetail,
        Kaizen__Kai_ReportKaizenDataMonthly,
        Kaizen__Kai_ReportCTKaizenDataMonthly,
        Kaizen__Kai_ReportAccumulate,
        Kaizen__Kai_ReportPaymentout,
        #endregion

        #region Permission In Profile Detail

        #region Button
        HRDetail__InfoPersionalDetail,
        HRDetail__InfoContactDetail,
        HRDetail__WorkHistoryDetail,
        HRDetail__AttendenceDetail,
        HRDetail__SalaryDetail,
        HRDetail__EvaluationDetail,
        HRDetail__TraAndEvaDetail,
        #endregion

        #region HR

        HRDetail__HR_ProfileInformationDetail,
        HRDetail__HR_ContactInformationDetail,
        HRDetail__HR_ProfileLaborForeignDetail,
        HRDetail__HR_RelativesDetail,
        HRDetail__HR_QualificationDetail,
        HRDetail__HR_PartyAndUnionDetail,
        HRDetail__HR_ContractDetail,
        HRDetail__HR_InsuranceDetail,
        HRDetail__HR_WorkHistoryDetail,
        HRDetail__HR_HDTJobDetail,
        HRDetail__HR_RewardDetail,
        HRDetail__HR_DisciplineDetail,
        HRDetail__HR_AccidentDetail,
        HRDetail__HR_CandidateHistoryDetail,
        HRDetail__HR_CardHistoryDetail,
        HRDetail__HR_AppendixContractDetail,
        HRDetail__HR_CongressDeputyDetail,
        #endregion

        #region Att
        HRDetail__Att_TAMScanDetail,
        HRDetail__Att_WorkDayDetail,
        HRDetail__Att_RosterDetail,
        HRDetail__Att_LeaveDayDetail,
        HRDetail__Att_OvertimeDetail,
        HRDetail__AttendanceTableDetail,
        HRDetail__Att_GradeDetail,
        #endregion

        #region Payroll
        HRDetail__Sal_SalaryInfoDetail,
        HRDetail__Sal_BasicSalaryTabDetail,
        HRDetail__Sal_DependantDetail,
        HRDetail__Sal_GradeAndAllowanceDetail,
        HRDetail__Sal_UnsualAllowanceTXDetail,
        HRDetail__Sal_UnsualAllowanceBTDetail,
        HRDetail__Sal_UnusualEDChildCareDetail,
        HRDetail__Sal_UnusualAllowanceDetail,
        HRDetail__Sal_UnusualPayDetail,
        HRDetail__Sal_PayslipsDetail,
        HRDetail__Sal_PaysCommission,
        #endregion

        #region Eva
        HRDetail__Eva_Sale4SDetail,
        HRDetail__Eva_SaleOutDetail,
        HRDetail__Eva_TrainningResultDetail,
        HRDetail__Eva_EvaResultDetail,
        #endregion

        #endregion

        #region Finance
        //Finance__Abc,//bat dau la Finance
        Finance__Fin_ApprovedPurchaseRequest,
        Finance__Fin_ApprovedTravelRequest,
        Finance__Fin_ApprovedClaim,
        Finance__Fin_ApprovedClaim_Delete,
        Finance__Fin_ApprovedClaimV2,
        Finance__FIN_ApproveCashAdvance,
        Finance__FIN_ApproveTravelCost,
        Finance__FIN_ClaimCostPaymentApprove,
        Finance__FIN_HistoryApproveECLAIM,

        #endregion
    }

    public enum SyncDataType
    {
        /// <summary>
        /// Đồng bộ mã chấm công
        /// </summary>
        Sync_Card_History,
        /// <summary>
        /// Đồng bộ ngày nghỉ việc
        /// </summary>
        Sync_Profile_DateQuit
    }

    public enum DashboardOptionEnum
    {
        TopFunction,
        Information,
        AlertsView,
        LeavesSummaryView,
        Chart1,
        Chart2,
        Chart3,
    }

    /// <summary>
    /// Loại database sử dụng
    /// </summary>
    public enum DATABASETYPE
    {
        SQLSERVER, ORACLE
    }

    /// <summary>
    /// Các loại control sử dụng cho CustomReport
    /// </summary>
    public enum CustomReportControl
    {
        /// <summary>
        /// Testbox
        /// </summary>
        Text,
        /// <summary>
        /// Combobox
        /// </summary>
        Combobox,
        /// <summary>
        /// Numberic text box
        /// </summary>
        Numberic,
        /// <summary>
        /// Câu phòng ban chọn 1
        /// </summary>
        Org,
        /// <summary>
        /// Control Datetime
        /// </summary>
        Datetime,
        /// <summary>
        /// Từ ngày đến ngày
        /// </summary>
        DateFromDateTo,
        /// <summary>
        /// True False
        /// </summary>
        CheckBox,


    }

    /// <summary>
    /// [Hien.Nguyen]
    /// </summary>
    public enum WhereType
    {
        [Description("Không Chọn")]
        None,
        [Description("Và")]
        And,
        [Description("Hoặc")]
        Or,
    }
    /// <summary>
    /// [Hien.Nguyen]
    /// </summary>
    public enum Operator
    {
        Equals,
        NotEquals,
        StartsWith,
        EndsWith,
        Contains,
        NotContains,
        Less,
        Greater,
        LessOrEquals,
        GreaterOrEquals,
        Between,

    }

    /// <summary>
    /// [Hien.Nguyen] 
    /// </summary>
    public enum SysLockObjectType
    {
        E_APPROVED_PAYROLL,
        E_LOCKOBJECT
    }

    /// <summary>
    /// [Hien.Nguyen] Các loại phần tử dùng để tính lương
    /// </summary>
    public enum PayrollElement
    {
        #region Nhân Sự

        /// <summary>
        /// Ngày Vào Làm
        /// </summary>
        [Description("Ngày Vào Làm")]
        HR_WORKINGDAY,

        /// <summary>
        /// "Ngày Nghỉ Việc
        /// </summary>
        [Description("Ngày Nghỉ Việc")]
        HR_LEAVEDAY,

        /// <summary>
        /// Nhân viên có ngày vào làm trong tháng tính lương
        /// </summary>
        [Description("Nhân viên có ngày vào làm trong tháng tính lương")]
        HR_IS_WORKINGDAY,

        /// <summary>
        /// Nhân viên có ngày nghỉ việc trong tháng tính lương
        /// </summary>
        [Description("Nhân viên có ngày nghỉ việc trong tháng tính lương")]
        HR_IS_LEAVEDAY,

        /// <summary>
        /// Bậc / Hệ số lương (Code)
        /// </summary>
        [Description("Bậc / Hệ số lương (Code)")]
        HR_SALARYCLASSNAME,

        /// <summary>
        /// Nhân viên có trong  doanh sách kỷ luật trong tháng tính lương
        /// </summary>
        /// 
        [Description("Nhân viên có trong  doanh sách kỷ luật trong tháng tính lương")]
        HR_IS_DISCIPLINE,

        /// <summary>
        /// Tổng số ngày tạm hoãn công việc tính tới cuối kỳ tính lương
        /// </summary>
        /// 
        [Description("Tổng số ngày tạm hoãn công việc tính tới cuối kỳ tính lương")]
        HR_TOTAL_DAY_STOP_WORKING,

        /// <summary>
        /// Nhân viên có được tính trợ cấp hay không (Có ngày vào làm từ 1996<=N<=31/12/2008)
        /// </summary>
        /// 
        [Description("Nhân viên có được tính trợ cấp hay không (Có ngày vào làm từ 1996<=N<=31/12/2008)")]
        HR_IS_COMPUTE_SUBSIDIZE,

        /// <summary>
        /// Ngày vào làm HDT Job trong kỳ tính lương
        /// </summary>
        /// 
        [Description("Ngày vào làm HDT Job trong kỳ tính lương")]
        HR_START_DATE_HDTJOB,

        /// <summary>
        /// Ngày vào làm HDT Job trong kỳ tính lương N-1
        /// </summary>
        /// 
        [Description("Ngày vào làm HDT Job trong kỳ tính lương N-1")]
        HR_START_DATE_HDTJOB_PREV,

        /// <summary>
        /// Ngày kết thúc HDT Job trong kỳ tính lương
        /// </summary>
        /// 
        [Description("Ngày kết thúc HDT Job trong kỳ tính lương")]
        HR_END_DATE_HDTJOB,

        /// <summary>
        /// Ngày kết thúc HDT Job trong kỳ tính lương N-1
        /// </summary>
        /// 
        [Description("Ngày kết thúc HDT Job trong kỳ tính lương N-1")]
        HR_END_DATE_HDTJOB_PREV,

        /// <summary>
        /// Nhân viên có tham gia công đoàn
        /// </summary>
        /// 
        [Description("Nhân viên có tham gia công đoàn")]
        IS_HRE_TRADEUNION,

        /// <summary>
        /// Số ngày trước khi vào làm (tính từ đầu tháng)
        /// </summary>
        /// 
        [Description("Số ngày trước khi vào làm (tính từ đầu tháng)")]
        HR_NUMBER_DAY_BEFORE_WORK,

        /// <summary>
        /// Số ngày trước khi vào làm tháng N-1 (tính từ đầu tháng)
        /// </summary>
        /// 
        [Description("Số ngày trước khi vào làm tháng N-1 (tính từ đầu tháng)")]
        HR_NUMBER_DAY_BEFORE_WORK_PREV,

        /// <summary>
        /// Nhân viên có ngày vào làm hoặc ngày đi làm lại trong khoảng từ ngày 1 đến ngày chốt lương trong tháng tính lương thì trả về 1, nếu không trả về 0
        /// </summary>
        /// 
        [Description("Nhân viên có ngày vào làm hoặc vào làm lại từ ngày 1 tới ngày chốt lương trong tháng tính lương")]
        HR_IS_BACK_TO_WORK,

        /// <summary>
        /// số ngày dayoff từ ngày 1 tháng tính lương đến ngày vào làm (VD: vào làm ngày 05/01, 01/01, 02/01 là ngày dayoff => trả về 2)
        /// </summary>
        /// 
        [Description("Số ngày dayoff từ ngày 1 tháng tính lương đến ngày vào làm")]
        HR_COUNT_DAYOFF_TO_DATEHIRE,

        /// <summary>
        /// Số ngày trong tháng tính lương - tổng số ngày dayoff trong tháng tính lương (VD: tháng 1 có 31 ngày - 8 ngày dayoff = 23)
        /// </summary>
        /// 
        [Description("Số ngày trong tháng tính lương - tổng số ngày dayoff trong tháng tính lương")]
        HR_COUNT_DAY_IN_CUTOFF,

        /// <summary>
        /// Số ngày từ ngày vào đến cuối tháng, không tính những ngày dayoff từ ngày vào đến cuối tháng (VD: vào ngày 05/01, số ngày dayoff từ mùng 5 đến 31 = 6 => trả về: 31 (số ngày trong tháng) - 5 (ngày vào) + 1 (từ 5 đến 31 là 27 ngày nên phải + thêm 1) - 6 (số ngày dayoff từ ngày 05/01 đến 31/01) = 21)
        /// </summary>
        /// 
        [Description("Số ngày từ ngày vào đến cuối tháng, không tính những ngày dayoff từ ngày vào đến cuối tháng")]
        HR_COUNT_DAY_TO_DATEEND_CUTOFF,

        /// <summary>
        /// Loại hợp đồng
        /// </summary>
        /// 
        [Description("Loại hợp đồng")]
        HR_CONSTRACT_TYPE,
        #endregion

        #region Công
        /// <summary>
        /// Số Giờ chuẩn của 1 ngày làm việc (Done)
        /// </summary>
        /// 
        [Description("Số Giờ chuẩn của 1 ngày làm việc")]
        ATT_HOURS_PER_DAY,

        ///// <summary>
        ///// Giờ công chuẩn làm việc trên ngày
        ///// </summary>
        //ATT_HOURS_PER_DAY,
        /// <summary>
        /// Ngày công đi làm thực tế (Done)
        /// </summary>
        /// 
        [Description(" Ngày công đi làm thực tế")]
        ATT_WORKING_DAY,

        /// <summary>
        /// Ngày công đi làm thực tế sau khi thay đổi lương
        /// </summary>
        /// 
        [Description(" Ngày công đi làm thực tế sau khi thay đổi lương")]
        ATT_WORKING_DAY_AFTER,

        /// <summary>
        /// Ngày công đi làm thực tế tháng N-1
        /// </summary>
        /// 
        [Description("Ngày công đi làm thực tế tháng N-1")]
        ATT_WORKING_DAY_PREV,

        /// <summary>
        /// Ngày công chuẩn từng tháng (Done)
        /// </summary>
        /// 
        [Description("Ngày công chuẩn từng tháng")]
        ATT_STD_DAY,

        /// <summary>
        /// Ngày công chuẩn từng tháng N-1 (Done)
        /// </summary>
        /// 
        [Description("Ngày công chuẩn từng tháng N-1")]
        ATT_STD_DAY_PREV,

        /// <summary>
        /// Ngày công đi tính lương ( load từ bảng công ) (Done)
        /// </summary>
        /// 
        [Description("Ngày công đi tính lương ( load từ bảng công )")]
        ATT_WORKING_PAIDLEAVE_DAY,

        /// <summary>
        /// Ngày công đi tính lương sau khi thay đổi lương ( load từ bảng công )
        /// </summary>
        /// 
        [Description("Ngày công đi tính lương sau khi thay đổi lương ( load từ bảng công )")]
        ATT_WORKING_PAIDLEAVE_DAY_AFTER,

        /// <summary>
        /// Tổng Số gio tăng ca trong tháng - theo loai tăng ca (Done)
        /// </summary>
        /// 
        [Description("Tổng Số gio tăng ca trong tháng - theo loai tăng ca")]
        ATT_OVERTIME_HOURS,

        /// <summary>
        /// Số gio tăng ca chiu thuế (Done)
        /// </summary>
        /// 
        [Description("Số gio tăng ca chiu thuế ")]
        ATT_OVERTIME_PIT_HOURS,

        /// <summary>
        /// Tổng số giờ ca đêm
        /// </summary>
        /// 
        [Description("Tổng số giờ ca đêm")]
        ATT_WORKING_NIGHTSHIFT,

        /// <summary>
        /// Tổng Số giờ nghỉ trong tháng
        /// </summary>
        /// 
        [Description("Tổng Số giờ nghỉ trong tháng")]
        ATT_LEAVE_HOURS,

        /// <summary>
        /// Tổng Số giờ nghỉ trong tháng N-1
        /// </summary>
        /// 
        [Description("Tổng Số giờ nghỉ trong tháng N-1")]
        ATT_LEAVE_HOURS_PREV,

        /// <summary>
        /// Số giờ nghỉ có trả lương
        /// </summary>
        /// 
        [Description("Số giờ nghỉ có trả lương")]
        ATT_TOTAL_PAID_LEAVEDAY_HOURS,

        /// <summary>
        /// Số ngày nghỉ có trả lương
        /// </summary>
        /// 
        [Description("Số ngày nghỉ có trả lương")]
        ATT_TOTAL_PAID_LEAVEDAY_DAY,

        /// <summary>
        /// Số ngày nghỉ không trả lương
        /// </summary>
        /// 
        [Description("Số ngày nghỉ không trả lương")]
        ATT_TOTAL_PAID_LEAVEDAY_DAY_NOT_PAY,

        /// <summary>
        /// Số giờ nghỉ có trả lương tháng N-1 (Done)
        /// </summary>
        /// 
        [Description("Số giờ nghỉ có trả lương tháng N-1")]
        ATT_TOTAL_PAID_LEAVEDAY_HOURS_PREV,

        /// <summary>
        /// Tổng số phép năm của nhân viên
        /// </summary>
        /// 
        [Description("Tổng số phép năm của nhân viên")]
        ATT_TOTAL_ANNUALLEAVE_AVAILABLE,

        /// <summary>
        /// Số ngày đã nghỉ tính tới đầu tháng đang tính công
        /// </summary>
        /// 
        [Description("Số ngày đã nghỉ tính tới đầu tháng đang tính công")]
        ATT_ANNUALLEAVE_ADJACENT,

        /// <summary>
        /// Tổng số phép ốm của nhân viên
        /// </summary>
        /// 
        [Description("Tổng số phép ốm của nhân viên")]
        ATT_TOTAL_SICK_AVAILABLE,

        /// <summary>
        /// Số ngày phép năm trong tháng
        /// </summary>
        /// 
        [Description("Số ngày phép năm trong tháng")]
        ATT_ANNUALLEAVE,

        /// <summary>
        /// Số ngày phép ốn trong tháng
        /// </summary>
        /// 
        [Description("Số ngày phép ốm trong tháng")]
        ATT_SICKLEAVE,

        /// <summary>
        /// Số ngày đã nghỉ tính tới đầu tháng đang tính công
        /// </summary>
        /// 
        [Description("Số ngày đã nghỉ tính tới đầu tháng đang tính công")]
        ATT_SICK_ADJACENT,

        /// <summary>
        /// Tháng tính lương
        /// </summary>
        /// 
        [Description("Tháng tính lương")]
        ATT_CUTOFFDURATION_MONTH,

        /// <summary>
        /// Số ngày công làm HDT Job Loại 5 (tháng N)
        /// </summary>
        /// 
        [Description("Số ngày công làm HDT Job Loại 5 (tháng N)")]
        ATT_WORKDAY_HDTJOB_5,
        /// <summary>
        /// Số ngày công làm HDT Job Loại 4 (tháng N)
        /// </summary>
        /// 
        [Description("Số ngày công làm HDT Job Loại 4 (tháng N)")]
        ATT_WORKDAY_HDTJOB_4,
        /// <summary>
        /// Số ngày công làm HDT Job Loại 5 (tháng N-1)
        /// </summary>
        /// 
        [Description("Số ngày công làm HDT Job Loại 5 (tháng N-1)")]
        ATT_WORKDAY_HDTJOB_PREV_5,
        /// <summary>
        /// Số ngày công làm HDT Job Loại 4 (tháng N-1)
        /// </summary>
        /// 
        [Description("Số ngày công làm HDT Job Loại 4 (tháng N-1)")]
        ATT_WORKDAY_HDTJOB_PREV_4,

        /// <summary>
        /// Tổng số ngày công trong năm
        /// </summary>
        /// 
        [Description("Tổng số ngày công trong năm")]
        ATT_TOTAL_WORKDAY_IN_YEAR,

        /// <summary>
        /// Tổng số ngày công thực tế trong năm
        /// </summary>
        /// 
        [Description("Tổng số ngày công thực tế trong năm")]
        ATT_TOTAL_REALITYWORKDAY_IN_YEAR,

        /// <summary>
        /// Tổng số ngày làm việc trong năm (365-dayoff)
        /// </summary>
        /// 
        [Description("Tổng số ngày làm việc trong năm (365-dayoff)")]
        ATT_TOTAL_DAY_NOT_DAYOFF_IN_YEAR,

        /// <summary>
        /// Số ngày phép năm cộng dồn
        /// </summary>
        /// 
        [Description("Số ngày phép năm cộng dồn")]
        ATT_ANNUAL_INCREMENTAL,

        /// <summary>
        /// Số ngày phép ốm cộng dồn
        /// </summary>
        /// 
        [Description("Số ngày phép ốm cộng dồn")]
        ATT_SICK_INCREMENTAL,

        /// <summary>
        /// trả về số ngày làm hdt loại 4 nếu tháng đó có đăng ký làm hdt, tính từ ngày vào đến ngày ra, trừ đi ngày dayoff
        /// </summary>
        /// 
        [Description("Ngày làm HDT loại 4 (không tính DayOff)")]
        ATT_WORKDAY_HDT_NOT_DAYOFF_4,
        /// <summary>
        /// trả về số ngày làm hdt loại 5 nếu tháng đó có đăng ký làm hdt, tính từ ngày vào đến ngày ra, trừ đi ngày dayoff
        /// </summary>
        /// 
        [Description("Ngày làm HDT loại 5 (không tính DayOff)")]
        ATT_WORKDAY_HDT_NOT_DAYOFF_5,

        /// <summary>
        /// trả về số ngày làm hdt loại 4 nếu tháng đó có đăng ký làm hdt, tính từ ngày vào đến ngày ra, trừ đi ngày dayoff
        /// </summary>
        /// 
        [Description("Ngày làm HDT loại 4 tháng N-1 (không tính DayOff)")]
        ATT_WORKDAY_HDT_NOT_DAYOFF_4_N_1,
        /// <summary>
        /// trả về số ngày làm hdt loại 5 nếu tháng đó có đăng ký làm hdt, tính từ ngày vào đến ngày ra, trừ đi ngày dayoff
        /// </summary>
        /// 
        [Description("Ngày làm HDT loại 5 th (không tính DayOff)")]
        ATT_WORKDAY_HDT_NOT_DAYOFF_5_N_1,

        /// <summary>
        /// số ngày dayoff từ đầu tháng đến ngày vào hdt
        /// </summary>
        /// 
        [Description("Số ngày Day Off từ đầu tháng đến ngày vào HDT")]
        ATT_DAYOFF_STARTMONTH_STARTHDT,

        /// <summary>
        /// số ngày dayoff từ đầu tháng đến ngày vào hdt  tháng N-1"
        /// </summary>
        /// 
        [Description("Số ngày Day Off từ đầu tháng đến ngày vào HDT tháng N-1")]
        ATT_DAYOFF_STARTMONTH_STARTHDT_N_1,

        /// <summary>
        /// số ngày từ ngày vào hdt đến cuối tháng trừ số ngày dayoff từ ngày vào đến cuối tháng
        /// </summary>
        /// 
        [Description("Số ngày từ ngày vào HDT tới cuối tháng, trừ ngày day off từ ngày vào HDT tới cuối tháng")]
        ATT_WORKDAY_STARTHDT_MONTHEND,

        /// <summary>
        /// số ngày từ ngày vào hdt đến cuối tháng trừ số ngày dayoff từ ngày vào đến cuối tháng  (tháng N-1)
        /// </summary>
        /// 
        [Description("Số ngày từ ngày vào HDT tới cuối tháng, trừ ngày day off từ ngày vào HDT tới cuối tháng (tháng N-1)")]
        ATT_WORKDAY_STARTHDT_MONTHEND_N_1,

        /// <summary>
        /// số ngày từ ngày vào hdt tháng N-1 đến ngày ra hdt tháng N-1 trừ ngày dayoff tháng N-1
        /// </summary>
        /// 
        [Description("Số ngày làm từ ngày vào HDT tới ngày ra HDT tháng N-1 (trừ Day Off tháng N-1)")]
        ATT_WORKDAY_STARTHDT_ENDHDT_N_1,

        /// <summary>
        /// số ngày từ ngày vào hdt tháng N-2 đến ngày ra hdt tháng N-1 trừ ngày dayoff tháng N-2
        /// </summary>
        /// 
        [Description("Số ngày làm từ ngày vào HDT tới ngày ra HDT tháng N-2 (trừ Day Off tháng N-2)")]
        ATT_WORKDAY_STARTHDT_ENDHDT_N_2,

        /// <summary>
        /// Ngày vào HDT Job Tháng N-1
        /// </summary>
        /// 
        [Description("Ngày vào HDT Job tháng N-1")]
        ATT_STARTDATE_HDT_N_1,
        /// <summary>
        /// Ngày vào HDT Job Tháng N-2
        /// </summary>
        /// 
        [Description("Ngày vào HDT Job tháng N-2")]
        ATT_STARTDATE_HDT_N_2,

        /// <summary>
        /// HVN Kiểm tra ngày null (=1 nếu không có ngày ra)
        /// </summary>
        /// 
        [Description("Không tồn tại ngày ra HDT (=1 nếu không có ngày ra)")]
        ATT_HDT_IS_DATE_END,

        /// <summary>
        /// HVN Kiểm tra ngày null tháng N-1 (=1 nếu không có ngày ra)
        /// </summary>
        /// 
        [Description("Không tồn tại ngày ra HDT tháng N-1  (=1 nếu không có ngày ra)")]
        ATT_HDT_IS_DATE_END_N_1,

        #endregion

        #region Lương

        /// <summary>
        /// Lương căn bản
        /// </summary>
        /// 
        [Description(" Lương cơ bản")]
        SAL_BASIC_SALARY,

        [Description(" Lương cơ bản T3")]
        SAL_BASIC_SALARY_T3,

        [Description(" Lương cơ bản tháng N-1")]
        SAL_BASIC_SALARY_N_1,

        [Description(" Lương cơ bản tháng N-2")]
        SAL_BASIC_SALARY_N_2,

        [Description(" Lương cơ bản tháng N-3")]
        SAL_BASIC_SALARY_N_3,

        [Description(" Lương cơ bản tháng N-4")]
        SAL_BASIC_SALARY_N_4,

        [Description(" Lương cơ bản tháng N-5")]
        SAL_BASIC_SALARY_N_5,

        [Description(" Lương cơ bản tháng N-6")]
        SAL_BASIC_SALARY_N_6,

        /// <summary>
        /// Lương cơ bản theo ngày chốt lương tháng N
        /// </summary>
        /// 
        [Description("Lương cơ bản theo ngày chốt lương tháng N")]
        SAL_BASIC_SALARY_DATECLOSE,
        /// <summary>
        /// Lương cơ bản theo ngày chốt lương tháng N-1
        /// </summary>
        /// 
        [Description("Lương cơ bản theo ngày chốt lương tháng N-1")]
        SAL_BASIC_SALARY_DATECLOSE_N_1,


        /// <summary>
        /// Lương cơ bản trước khi thay đổi lương
        /// </summary>
        [Description("Lương cơ bản trước khi thay đổi lương")]
        SAL_BASIC_SALARY1,

        /// <summary>
        /// Lương cơ bản sau khi thay đổi lương
        /// </summary>
        [Description("Lương cơ bản sau khi thay đổi lương")]
        SAL_BASIC_SALARY2,

        /// <summary>
        /// số ngày thay đổi lương trong tháng
        /// </summary>
        /// 
        [Description("Số ngày thay đổi lương trong tháng")]
        SAL_INCENTIVE,

        /// <summary>DisplayTypeBasicSalary
        /// Số người phụ thuộc
        /// </summary>
        /// 
        [Description("Số người phụ thuộc")]
        SAL_DEPENDENT,

        /// <summary>
        /// Tháng bắt đầu hưởng PC
        /// </summary>
        /// 
        [Description("Tháng bắt đầu hưởng PC")]
        SAL_UNUSUALALLOWANCE_MONTHSTART,

        /// <summary>
        /// Năm bắt đầu hưởng PC
        /// </summary>
        /// 
        [Description("Năm bắt đầu hưởng PC")]
        SAL_UNUSUALALLOWANCE_YEARSTART,

        /// <summary>
        /// Tháng kết thúc hưởng PC
        /// </summary>
        /// 
        [Description("Tháng kết thúc hưởng PC")]
        SAL_UNUSUALALLOWANCE_MONTHEND,

        /// <summary>
        /// Năm kết thúc hưởng PC
        /// </summary>
        /// 
        [Description("Năm kết thúc hưởng PC")]
        SAL_UNUSUALALLOWANCE_YEAREND,

        /// <summary>
        /// Số tháng bù
        /// </summary>
        /// 
        [Description("Số tháng bù")]
        SAL_UNUSUALALLOWANCE_NOCOMPENSATION,

        /// <summary>
        /// Lương Bị Giữ
        /// </summary>
        /// 
        [Description("Lương Bị Giữ Trước Thuế")]
        SAL_HOLD_SALARY,

        /// <summary>
        /// Lương Bị Giữ
        /// </summary>
        /// 
        [Description("Lương Bị Giữ Sau Thuế")]
        SAL_HOLD_SALARY_AFTERTAX,

        /// <summary>
        /// Hệ số lương nhân viên
        /// </summary>
        /// 
        [Description("Hệ số lương nhân viên")]
        SAL_BASIC_PERSONALRATE,

        /// <summary>
        /// Tổng lương bộ phận của nhân viên
        /// </summary>
        /// 
        [Description("Tổng lương bộ phận của nhân viên")]
        SAL_SALARY_DEPARTMENT,

        /// <summary>
        /// Ngày chốt lương
        /// </summary>
        /// 
        [Description("Ngày chốt lương")]
        SAL_SALARY_DATE_CLOSE,

        /// <summary>
        /// Bậc Lương
        /// </summary>
        /// 
        [Description("Bậc Lương (RANK)")]
        SAL_SALARY_RANK_NAME,

        /// <summary>
        /// Bậc Lương
        /// </summary>
        /// 
        [Description("Bậc Lương (CLASS)")]
        SAL_SALARY_CLASS_NAME,

        /// <summary>
        /// Mức lương HDT
        /// </summary>
        /// 
        [Description("Mức lương HDT")]
        SAL_SALARY_HDT,
        /// <summary>
        /// Mức lương HDT tháng N-1
        /// </summary>
        /// 
        [Description("Mức lương HDT tháng N-1")]
        SAL_SALARY_HDT_N_1,
        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        /// 
        [Description("Số tài khoản ngân hàng")]
        SAL_SALARY_ACCOUNT_NO,
        /// <summary>
        /// Nhóm ngân hàng
        /// </summary>
        /// 
        [Description("Nhóm ngân hàng")]
        SAL_SALARY_GROUP_BANK,
        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        /// 
        [Description("Tên ngân hàng")]
        SAL_SALARY_BANK_NAME,

        ///// <summary>
        ///// trả về mức tiền làm hdt loại 4 nếu tháng đó có đăng ký làm hdt, căn cứ theo mức thưởng phụ cấp (Cat_UnAllowCfgAmount), nếu không trả về 0
        ///// </summary>
        ///// 
        //[Description("Mức tiền làm HDT loại 4")]
        //SAL_UNALLOW_CFG_AMOUNT_4,

        // /// <summary>
        ///// trả về mức tiền làm hdt loại 5 nếu tháng đó có đăng ký làm hdt, căn cứ theo mức thưởng phụ cấp (Cat_UnAllowCfgAmount), nếu không trả về 0
        ///// </summary>
        ///// 
        //[Description("Mức tiền làm HDT loại 5")]
        //SAL_UNALLOW_CFG_AMOUNT_5,
        #endregion

        #region Bảo Hiểm
        /// <summary>
        /// BH Xả hội
        /// </summary>
        /// 
        [Description("BH Xả hội (Công ty + Nhân viên)")]
        INS_SOCIAL_INSURANCE,
        /// <summary>
        /// BH thất nghiệp
        /// </summary>
        /// 
        [Description("BH thất nghiệp (Công ty + Nhân viên)")]
        INS_UNEMP_INSURANCE,
        /// <summary>
        /// Bảo Hiểm Y Tế
        /// </summary>
        /// 
        [Description("Bảo Hiểm Y Tế (Công ty + Nhân viên)")]
        INS_HEALTH_INSURANCE,
        /// <summary>
        /// Lương đóng bảo hiểm
        /// </summary>
        /// 
        [Description("Lương đóng bảo hiểm")]
        INS_SALARY_INSURANCE,

        /// <summary>
        /// Lương đóng bảo hiểm gốc
        /// </summary>
        /// 
        [Description("Lương đóng bảo hiểm gốc")]
        INS_SALARY_INSURANCE_ROOT,

        /// <summary>
        /// BH xã hội nhân viên đóng
        /// </summary>
        /// 
        [Description("BH xã hội nhân viên đóng")]
        INS_SOCIAL_INSURANCE_PROFILE,
        /// <summary>
        /// BH xã hội công ty đóng
        /// </summary>
        /// 
        [Description("BH xã hội công ty đóng")]
        INS_SOCIAL_INSURANCE_COMPANY,
        /// <summary>
        /// BH thất nghiệp nhân viên đóng
        /// </summary>
        /// 
        [Description("BH thất nghiệp nhân viên đóng")]
        INS_UNEMP_INSURANCE_PROFILE,
        /// <summary>
        /// BH thất nghiệp công ty đóng
        /// </summary>
        /// 
        [Description("BH thất nghiệp công ty đóng")]
        INS_UNEMP_INSURANCE_COMPANY,
        /// <summary>
        /// Bảo Hiểm Y Tế nhân viên đóng
        /// </summary>
        /// 
        [Description("Bảo hiểm y tế nhân viên đóng")]
        INS_HEALTH_INSURANCE_PROFILE,
        /// <summary>
        /// Bảo Hiểm Y Tế công ty đóng
        /// </summary>
        /// 
        [Description("Bảo hiểm y tế công ty đóng")]
        INS_HEALTH_INSURANCE_COMPANY,

        #endregion

        #region Lương hoa hồng

        /// <summary>
        /// Doanh thu 6 dòng sản phẩm chủ đạo của cửa hàng
        /// </summary>
        /// 
        [Description("Doanh thu 6 dòng sản phẩm chủ đạo của cửa hàng")]
        SAL_COM_PERCENT_SHOP_LINEITEM,
        /// <summary>
        /// Doanh thu các sản phẩm đẩy mạnh của cửa hàng
        /// </summary>
        /// 
        [Description("Doanh thu các sản phẩm đẩy mạnh của cửa hàng")]
        SAL_COM_PERCENT_SHOP_ITEM,

        /// <summary>
        /// Doanh thu thực đạt của cửa hàng
        /// </summary>
        /// 
        [Description("Doanh thu thực đạt của cửa hàng")]
        SAL_COM_ACTUAL_SHOP,
        /// <summary>
        /// Mục tiêu của cửa hàng
        /// </summary>
        /// 
        [Description("Mục tiêu của cửa hàng")]
        SAL_COM_TAGET_SHOP,

        /// <summary>
        /// Doanh thu thực đạt của nhân viên
        /// </summary>
        /// 
        [Description("Doanh thu thực đạt của nhân viên")]
        SAL_COM_ACTUAL_CUSTOMER,
        /// <summary>
        /// Mục tiêu của nhân viên
        /// </summary>
        /// 
        [Description("Mục tiêu của nhân viên")]
        SAL_COM_TAGET_CUSTOMER,

        /// <summary>
        /// Số tháng làm việc
        /// </summary>
        /// 
        [Description("Số tháng làm việc")]
        SAL_COM_WORKINGMONTH,

        /// <summary>
        /// Số nhân viên của cửa hàng
        /// </summary>
        /// 
        [Description("Số nhân viên của cửa hàng")]
        SAL_COM_COUNT_SHOPMEMBER,

        /// <summary>
        /// Chức Danh
        /// </summary>
        /// 
        [Description("Chức Danh")]
        SAL_COM_JOBTITLE,

        /// <summary>
        /// Số ca trưởng của cửa hàng
        /// </summary>
        /// 
        [Description("Số ca trưởng của cửa hàng")]
        SAL_COM_COUNT_SL,

        /// <summary>
        /// Loại cửa hàng
        /// </summary>
        /// 
        [Description("Cấp Bậc Của Cửa Hàng")]
        SAL_COM_RANK,

        /// <summary>
        /// Phần trăm doanh thu của cửa hàng
        /// </summary>
        /// 
        [Description("Phần trăm doanh thu của cửa hàng")]
        SAL_COM_PRECENT_REVENUE,

        /// <summary>
        /// Thưởng thêm khi có nhân viên không đủ thâm niên
        /// </summary>
        /// 
        [Description("Thưởng thêm khi có nhân viên không đủ thâm niên")]
        SAL_COM_BONUS_SCV,

        /// <summary>
        /// Tên của shop
        /// </summary>
        /// 
        [Description("Tên cửa hàng")]
        SAL_COM_SHOPNAME,

        /// <summary>
        /// Tổng doanh thu của tất cả nhân viên trong shop
        /// </summary>
        /// 
        [Description("Tổng doanh thu của tất cả nhân viên trong shop")]
        SAL_TOTAL_ACTUAL_PROFILE_SHOP,
        #endregion

        #region Đánh Giá
        /// <summary>
        /// Loại đánh giá
        /// </summary>
        /// 
        [Description("Loại đánh giá")]
        EVA_PERFORMANCE_TYPE_CODE,

        /// <summary>
        /// Cấp độ đánh giá
        /// </summary>
        /// 
        [Description("Cấp độ đánh giá")]
        EVA_PERFORMANCE_LEVEL_NAME,


        #endregion

        #region Canteen

        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_SUMAMOUNT")]
        CAN_SUMAMOUNT,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTEATNOTSTANDAR")]
        CAN_AMOUNTEATNOTSTANDAR,
        /// <summary>
        /// /
        /// </summary>
        /// 
        [Description("CAN_AMOUNTCARDMORE")]
        CAN_AMOUNTCARDMORE,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTNOTWORKHASEAT")]
        CAN_AMOUNTNOTWORKHASEAT,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTHDTJOB")]
        CAN_AMOUNTHDTJOB,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTNOTWORKBUTHASHDT")]
        CAN_AMOUNTNOTWORKBUTHASHDT,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTSUBTRACTWRONGSTANDARHDT")]
        CAN_AMOUNTSUBTRACTWRONGSTANDARHDT,

        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_SUMAMOUNT N-1")]
        CAN_SUMAMOUNT_N_1,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTEATNOTSTANDAR N-1")]
        CAN_AMOUNTEATNOTSTANDAR_N_1,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTCARDMORE N-1")]
        CAN_AMOUNTCARDMORE_N_1,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTNOTWORKHASEAT N-1")]
        CAN_AMOUNTNOTWORKHASEAT_N_1,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTHDTJOB N-1")]
        CAN_AMOUNTHDTJOB_N_1,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTNOTWORKBUTHASHDT N-1")]
        CAN_AMOUNTNOTWORKBUTHASHDT_N_1,
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("CAN_AMOUNTSUBTRACTWRONGSTANDARHDT N-1")]
        CAN_AMOUNTSUBTRACTWRONGSTANDARHDT_N_1,


        #endregion

        #region Phần tử động
        /// <summary>
        /// Phần tử động, đếm số ngày ot trong tháng
        /// </summary>
        /// 
        [Description("(Dynamic) Đếm số ngày OT trong tháng theo tham số (_OTType_Number)")]
        DYN_COUNTDAYOVERTIMEBYTYPE_,
        #endregion

    }

    /// <summary>
    /// [Hien.Nguyen] Các loại phần tử trong bảng cat_FormulaTemplate
    /// </summary>
    public enum FormulaTemplateType
    {
        [Description("Lương Thời Gian")]
        E_TIMESALARY,
        [Description("Lương Tăng Ca")]
        E_OVERTIMESALARY,
        [Description("Lương Phụ Cấp")]
        E_ALLOWANCESALARY,
        [Description("Lương Ngày Nghỉ")]
        E_LEAVEDAYSALARY,
        [Description("Lương Sản Phẩm")]
        E_PRODUCTSALARY,
    }

    /// <summary> [Tung.Ly] các loại phần tử dùng để tính bảo hiểm </summary>
    public enum InsuranceElement
    {
        #region Tinh Tien HDTJob
        /// <summary> Số ngày công làm HDT Job Loại 4 (tháng N) </summary> 
        [Description("Số ngày công làm HDT Job Loại 4 (tháng N)")]
        ATT_WORKDAY_HDTJOB_4,
        /// <summary> Số ngày công làm HDT Job Loại 5 (tháng N) </summary> 
        [Description("Số ngày công làm HDT Job Loại 5 (tháng N)")]
        ATT_WORKDAY_HDTJOB_5,
        /// <summary> Ngày vào làm HDT Job trong kỳ tính lương </summary> 
        [Description("Ngày vào làm HDT Job trong kỳ tính lương")]
        HR_START_DATE_HDTJOB,
        /// <summary> Ngày kết thúc HDT Job trong kỳ tính lương </summary> 
        [Description("Ngày kết thúc HDT Job trong kỳ tính lương")]
        HR_END_DATE_HDTJOB,
        /// <summary> Tháng tính lương </summary> 
        [Description("Tháng tính lương")]
        ATT_CUTOFFDURATION_MONTH,
        /// <summary> Lương đóng bảo hiểm gốc </summary> 
        [Description("Lương đóng bảo hiểm gốc")]
        INS_SALARY_INSURANCE_ROOT,

        #endregion

        #region Sinh tên công việc cho Phân tích bảo hiểm
        /// <summary> Tiền công việc nặng nhọc </summary>
        [Description("Tiền công việc nặng nhọc")]
        INS_JOBNAME_HDTJOBMONEY,

        /// <summary> Name Of Rank </summary>
        [Description("Name Of Rank")]
        INS_JOBNAME_NAMEOFRANK,

        /// <summary> Tên Nhóm Công Việc Nặng Nhọc </summary>
        [Description("Tên Nhóm Công Việc Nặng Nhọc")]
        INS_JOBNAME_HDTGROUPNAME,

        /// <summary> Tên Phòng Ban Cha </summary>
        [Description("Tên Phòng Ban Cha")]
        INS_JOBNAME_ORGSTRUCTURENAME,

        /// <summary> Chức Danh </summary>
        [Description("Chức Danh")]
        INS_JOBNAME_JOBTITLE,
        /// <summary> Số ngày không làm việc HDTJob </summary>
        [Description("Số ngày không làm việc HDTJob")]
        INS_JOBNAME_NUMDAYNONHDTJOB,

        /// <summary> Số tiền HDT4 theo timeline </summary>
        [Description("Số tiền HDT4 theo timeline")]
        INS_HDT4_TIMELINE,

        /// <summary> Số tiền HDT5 theo timeline </summary>
        [Description("Số tiền HDT5 theo timeline")]
        INS_HDT5_TIMELINE,

        #endregion
    }

    public enum SalaryTimeTypeClose
    {
        [Description("Cùng tháng")]
        E_CURRENTMONTH,
        [Description("Tháng Trước")]
        E_LASTMONTH,
    }
    public enum OvertimeReasonCode
    {
        OT_ELSE,
        OT_BU,
        OT_PROVIDER,
        OT_EARLY,
        OT_HOME
    }
    public enum PayrollShopElement
    {
        /// <summary>
        /// % Doanh thu cửa hàng (Actual/Taget)
        /// </summary>
        RevenueForShop,
        /// <summary>
        /// % Doanh thu bán hàng được hưởng ()
        /// </summary>
        SHOP_ACTUAL_1,
        /// <summary>
        /// % Doanh thu số khách được hưởng ()
        /// </summary>
        SHOP_ACTUAL_2,
        /// <summary>
        /// % Doanh thu dịch vụ thu tiền được hưởng ()
        /// </summary>
        SHOP_ACTUAL_3,
        /// <summary>
        /// % Doanh thu dịch vụ số khách được hưởng ()
        /// </summary>
        SHOP_ACTUAL_4,
        /// <summary>
        /// % 6 dòng sản phẩm chủ đạo ()
        /// </summary>
        SHOP_ACTUAL_5,
        /// <summary>
        /// % các sản phẩm đẩy mạnh ()
        /// </summary>
        SHOP_ACTUAL_6,
    }



    /// <summary> Loại xuất báo cáo </summary>
    public enum ExportFileType
    {
        Excel, PDF
    }

    public enum ShiftBreakType
    {
        E_FIX, E_FLEXIBLE
    }

    public enum ConfigElementPayroll
    {
        HRM_SAL_PAYROLL_CONFIGELEMENT,
        HRM_SAL_PAYROLL_CONFIGELEMENT_REPORTTALANNUALTAXABLEINCOME,
        HRM_SAL_PAYROLL_CONFIGELEMENT_REPORTPLANNINGPAYROLL,
        HRM_SAL_PAYROLL_CONFIGELEMENT_REPORTAMOUNTOTANDUNPAIDLEAVE,
        HRM_SAL_PAYROLL_CONFIGELEMENT_RemittanceAllowSick,
        HRM_SAL_PAYROLL_CONFIGELEMENT_REPORTTOTALANNUALSICK,
        HRM_SAL_PAYROLL_CONFIGELEMENT_REPORTTOTALPITMONTHLY,
        HRM_SAL_PAYROLL_CONFIGELEMENT_REPORTTRANSFERVIABANK_ED,
        HRM_SAL_PAYROLL_CONFIGELEMENT_REPORTCASH_ED,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTALLOWANCESTOPWORKING,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTTRANSFERUNIONS,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTTRANSFERBONUSHOLD,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTTOTALANNUALSICK,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTBONUSEVADETAIL,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTSALARYALLOWANCEQUIT,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTBONUSHOLIDAY,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTPITSTOPWORKING,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTGENERALINSURANCE,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTKAIZENMONTHLYCASH,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTVARIABLESALARY,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTHISTORYPAYROLLELEMENT,
        HRM_SAL_PAYROLL_CONFIGELEMENT_ED_SAL_REPORTTRANSFERVIABANK,
    }

    public enum ConfigObject
    {
        HRM_SYS_LOCKOBJECT_CONFIGOBJECT,


    }

    public enum MethodPayroll
    {
        [Description("Hình Thức Tính Lương")]
        E_NORMAL,
        [Description("Hình Thức Ứng Lương")]
        E_ADNVANCE_PAYMENT,
        [Description("Hình Thức Tính Lương Hoa Hồng")]
        E_COMMISSION_PAYMENT,
    }
    public enum MissTAM_LeaveType
    {
        E_OFF_NOT_SALARY = 1, E_OFF_NOT_ALLOW = 2, E_OFF_LESS = 3
    }
    public enum LaundryRecordType
    {
        E_AUTO = 0, E_MANUAL
    }
    public enum LaundryRecordStatus
    {
        E_SUBMIT = 0, E_WAIT_APPROVED, E_APPROVED,
    }
    /// <summary>
    /// Phương thức lấy dữ liệu
    /// </summary>
    public enum HttpMethod
    {
        POST, GET
    }
    /// <summary>
    /// Phương thức lấy dữ liệu
    /// </summary>
    public enum Gender
    {
        Nam, Nữ
    }

    /// <summary>
    /// Loại phần tử tính lương
    /// </summary>
    public enum CatElementType
    {
        [Description("Lương")]
        Payroll,
        [Description("Công")]
        Attendance,
        [Description("Bảo Hiểm")]
        Insurance,
        [Description("Phụ Cấp")]
        Allowances,
        [Description("Phụ Cấp Bất Thường (Cộng)")]
        AllowancesOut,
        [Description("Phụ Cấp Bất Thường (Trừ)")]
        AllowancesOut_Minus,
        [Description("Hoa Hồng")]
        Comission,
        [Description("Đánh Giá")]
        Evaluation,
        [Description("Ứng Lương")]
        AdvancePayment,
        [Description("Chuyến Bay")]
        FLIGHT,
        [Description("Tiền Tệ")]
        ExChangeRate,
    }

    /// <summary>
    /// Phương Thức Tính Lương
    /// </summary>
    public enum PayrollComputeMethod
    {
        #region PayrollComputeMethod
        [Description("Cộng")]
        E_SUM,
        [Description("Trừ")]
        E_SUBTRACT,
        [Description("Công Thức")]
        E_FORMULA
        #endregion
    }

    /// <summary>
    /// Phuong thuc tinh luong theo quy luong
    /// </summary>
    public enum MethodComputeGroupSalary
    {
        // Tinh luong theo phong ban
        E_COMPUTE_DERPARMENT,

        // Tinh luong theo Line
        E_COMPUTE_LINE
    }

    /// <summary>
    /// Đăng ký roster theo các loại (thời gian, sản phẩm, bộ phận)
    /// </summary>
    public enum SalaryRosterType
    {
        E_TIMESAL, E_PRODUCTSAL, E_DEPTSAL
    }

    public enum Education
    {
        E_EDUCATION_LEVEL
    }

    public enum ExceptionType
    {
        NULL = 0, NORMAL, DATA_TABLE_DESIGN, SQL_EXCEPTION, DAL, DATACONTEXT, FRAMEWORK, QUERY_STRING, DATA_ERROR
    }

    public enum ResourceType
    {
        Metadata, Module, Screen, Other
    }

    /// <summary>
    /// Key phân biệt module khi tạo resource.
    /// </summary>
    public enum ModuleKey
    {
        Sys, Cat, Att, Can, Hre, Lau, Med, Tra, Sal, Rec, Ins, Eva, Kai, Per, Hrd, Fin, Pur, PUR
    }

    /// <summary>
    /// Tên những module có trong chương trình.
    /// </summary>
    public enum ModuleName
    {
        [Description("Hệ Thống")]
        System,
        [Description("Danh Mục")]
        Category,
        [Description("Chấm Công")]
        Attendance,
        [Description("Nhà Ăn")]
        Canteen,
        [Description("Nhân Sự")]
        HR,
        [Description("Giặc Là")]
        Laundry,
        [Description("Y Tế")]
        Medical,
        [Description("Đào Tạo")]
        Training,
        [Description("Lương")]
        Salary,
        [Description("Tuyển Dụng")]
        Recruitment,
        [Description("Bảo Hiểm")]
        Insurance,
        [Description("Đánh Giá")]
        Evaluation,
        [Description("Sáng Kiến")]
        Kaizen,
        [Description("Tài Chính")]
        Finance,
        [Description("Đăng Ký")]
        PersonalRegister,
        [Description("Chi Tiết Nhân Viên")]
        HRDetail,
        [Description("Mua Xe Trả Góp")]
        Purchase,
    }

    public enum TAMScanType
    {
        E_IN,
        E_OUT
    }

    public enum WorkdaySrcType
    {
        [Description("Tự Động")]
        E_NORMAL,
        [Description("Bẳng Tay")]
        E_MANUAL
    }

    public enum WorkHistoryStatus
    {
        //[Description("Triển Vọng")]
        //E_PROSPECT,

        [Description("Chờ Duyệt")]
        E_WAITAPPROVE,

        [Description("Duyệt")]
        E_APPROVED,

        [Description("Hủy")]
        E_CANCEL,

        [Description("Từ Chối")]
        E_REJECT,

        //[Description("Nghỉ Việc")]
        //E_PROFILEQUIT,

        //[Description("Làm Việc Lại")]
        //E_WORKAGAIN,


    }

    public enum ReqDocumentType
    {
        [Description("Nhận Việc")]
        E_GETJOB,
        [Description("Nghỉ Việc")]
        E_QUIT,
    }

    public enum UnusualAllowanceStatus
    {
        [Description("Triển Vọng")]
        E_PROSPECT,

        [Description("Chờ Duyệt")]
        E_WAITAPPROVE,

        [Description("Duyệt")]
        E_APPROVED,

        [Description("Từ Chối")]
        E_REJECT,

        [Description("Nghỉ Việc")]
        E_PROFILEQUIT,

        [Description("Làm Việc Lại")]
        E_WORKAGAIN,

        [Description("Hủy")]
        E_CANCEL,
    }

    public enum ConfigNightShift
    {
        #region OverTimeType
        //Lay ca dem trong ca
        E_SHIFT,
        //Lay ca dem trong config
        E_CONFIG
        #endregion
    }
    public enum TypeLoadData
    {
        /// <summary>
        /// Lay du lieu in/out theo bien do max min
        /// </summary>
        E_MAXMIN,
        /// <summary>
        /// Lay theo mac dinh theo thu tu in - out
        /// </summary>
        E_DEFAULT,
        /// <summary>
        /// Lay du lieu dau cuoi in - out trong ca ngay
        /// </summary>
        E_CHECKDAYSHIFT,
        /// <summary>
        /// Lay du lieu da co san type in - out theo tung cap
        /// </summary>
        E_TYPEINOUT,
        /// <summary>
        /// Lay du lieu da co san type in - out chi lay du lieu dau va du lieu cuoi
        /// </summary>
        E_TYPEINOUT_MAXMIN,
        /// <summary>
        /// Lấy ca tự động theo In đầu Out cuối
        /// </summary>
        E_AUTOSHIFT_MAXMIN
    }

    /// <summary>
    /// Loai Nghi thai san
    /// </summary>
    public enum PregnancyType
    {
        #region PregnancyType
        E_LEAVE_EARLY
      , E_NEW_BORN_CHILD
        #endregion
    }

    public enum ScheduleType
    {
        #region ScheduleType
        E_HR_CONTRACT,
        E_OTHER
        #endregion
    }

    public enum DataErrorCode
    {
        Success = 0, Unknown, OverFlow, NullData, NoEntryForUpdate, FieldNotAcceptNull, Dupplicate, Locked, ErrorFormat, BINARYDATA, Error,
        // không có quyền
        NoPermission,
        // Duyệt và chuyển lên cấp trên
        Success_Forward,
        // Bản ghi đã từ chối
        Error_Reject,
        // Bản ghi đã gữi yêu cầu duyệt
        Error_SendRequest,
        // Lỗi chưa chọn người duyệt
        Error_NoUserApproved,
        // Lỗi chưa cấu hình gửi mail
        Error_NoConfigMail,
        // Lỗi chưa cấu hình gửi mail
        Error_NoTemplateMail
    }

    public enum LockObjectStatus
    {
        E_WAIT_APPROVED, E_FIRST_APPROVED, E_APPROVED, E_CANCEL
    }

    public enum CheckLockType
    {
        E_BETWEEN, E_FROMTOINTERSECTION
    }

    public enum WorkdayType
    {
        /// <summary>
        /// bình thường
        /// </summary>
        [Description("Hợp Lệ")]
        E_NORMAL,
        /// <summary>
        /// Thiếu in
        /// </summary>
        [Description("Thiếu In")]
        E_MISS_IN,
        /// <summary>
        /// Thiếu out
        /// </summary>
        [Description("Thiếu Out")]
        E_MISS_OUT,
        /// <summary>
        /// Thiếu cả in out
        /// </summary>
        [Description("Thiếu Cả In Out")]
        E_MISS_IN_OUT,
        /// <summary>
        /// Thời gian in out không thuộc ca
        /// </summary>
        [Description("Sai Ca")]
        E_WRONG_SHIFT,
        /// <summary>
        /// Tự detect shift khi không có shift
        /// </summary>
        [Description("Tự Nhận Ca")]
        E_DETECTED_SHIFT,
        [Description("Ca Ngắn")]
        E_SHORTIN_SHIFT,
        [Description("Ca Dài")]
        E_LONGIN_SHIFT,
        /// <summary>
        /// Đi làm ngày lễ
        /// </summary>
        [Description("Ngày Lễ")]
        E_HOLIDAY,
        [Description("Muộn Sớm")]
        E_LATE_EARLY
    }

    /// <summary>
    /// Loai ap dung thai san duoc tre som 1 h
    /// </summary>
    public enum PregnancyLeaveEarlyType
    {

        /// <summary>
        /// Di tre 1 h
        /// </summary>
        E_FIRST,
        /// <summary>
        /// ve som giua ca 1 h
        /// </summary>
        E_FIRST_OUT_BEARK,
        /// <summary>
        /// Vao tre giua ca 1 h
        /// </summary>
        E_LAST_IN_BEARK,
        /// <summary>
        /// Ve som 1 h
        /// </summary>
        E_LAST

    }
    /// <summary>
    /// loai phep chuyen
    /// </summary>
    public enum TypeAnnual
    {
        [Description("Phép Bệnh")]
        E_SICK,
        [Description("Phép Năm")]
        E_ANNUAL
    }

    public enum CheckHDTJobType
    {
        [Description("Dữ Liệu Hợp Lệ")]
        E_SUCCESS_DATA,
        [Description("Dữ Liệu Lỗi")]
        E_WRONG_DATA
    }
    public enum AttendanceMethod
    {
        [Description("Có Kiểm Tra Vào Ra")]
        E_TAM,
        [Description("Lịch Làm Việc")]
        E_TIMESHEET,
        [Description("Không Kiểm Tra Vào Ra")]
        E_FULL
    }
    public enum GradeRosterType
    {
        #region GradeRosterType
        /// <summary>
        /// Gradecfg IsRoster
        /// </summary>
        E_ISROSTER,
        /// <summary>
        /// Lay ca lam viec theo phong ban.
        /// </summary>
        E_ISROSTER_ORG,
        E_ISHOLIDAY,
        E_ISDEFAULT
        #endregion
    }
    public enum StatusEmployee
    {
        #region Tat ca nhan vien
        /// <summary>Tat ca nhan vien</summary>

        E_ALL,
        #endregion
        #region Nhan vien nghi viec
        /// <summary> Nhan vien nghi viec</summary>

        E_STOPWORKING,
        #endregion
        #region Nhan vien moi vao
        /// <summary> Nhan vien moi vao</summary>

        E_NEWEMPLOYEE,
        #endregion
        #region Nhan vien dang lam viec
        /// <summary> Nhan vien dang lam viec</summary>

        E_WORKING,
        #endregion
        #region Nhan vien moi vao va dang lam viec
        /// <summary> Nhan vien moi vao va dang lam viec</summary>

        E_WORKINGANDNEW
        #endregion
    }
    public enum PregnancyStatus
    {
        #region PregnancyStatus
        E_LEAVE_EARLY, E_NEW_BORN_CHILD
        #endregion
    }
    public enum ComputeLeavedayType
    {
        E_DATA_NON_LEAVE, E_DATA_LEAVE
    }
    public enum ComputeOvertimeType
    {
        E_DATA_OT, E_DATA_NON_OT
    }
    public enum RosterType
    {
        /// <summary>
        /// TKB Hằng Ngày
        /// </summary>
        [Description("TKB Hằng Ngày")]
        E_DEFAULT,
        /// <summary>
        /// Nghỉ Theo Phân Công
        /// </summary>
        [Description("Nghỉ Theo Phân Công")]
        E_TIME_OFF,
        /// <summary>
        /// Đổi TKB
        /// </summary>
        [Description("Đổi TKB")]
        E_CHANGE_SHIFT,
        /// <summary>
        /// Bất Thường
        /// </summary>
        [Description("Bất Thường")]
        E_ABNORMAL,
        /// <summary>
        /// Nhóm Ca Làm Việc
        /// </summary>
        [Description("Nhóm Ca Làm Việc")]
        E_ROSTERGROUP
    }

    public enum DisplayTypeBasicSalary
    {

        // E_BOLD,
        [Description("Level 1")]
        E_LEVEL_0,
        [Description("Level 2")]
        E_LEVEL_1,
        [Description("Level 3")]
        E_LEVEL_2,
        [Description("Level 4")]
        E_LEVEL_3,
        [Description("Level 5")]
        E_LEVEL_4
    }

    public enum ScheduleTaskType
    {
        [Description("Tháng")]
        ByMonths = 8,
        [Description("Ngày")]
        ByDays = 7,
        [Description("Giờ")]
        ByHours = 5,
        [Description("Phút")]
        ByMinutes = 6,
    }


    public enum ProfileStatusSyn
    {
        [Description("Chờ Nhận Việc")]
        E_WAITING,
        [Description("Đang Làm Việc")]
        E_HIRE,
        [Description("Từ Chối")]
        E_UNHIRE,
        [Description("Chờ Duyệt")]
        E_WAITING_APPROVE,
        [Description("Quyết Toán Nghỉ Việc")]
        E_PAYMENT_QUIT,
        [Description("Thôi Việc")]
        E_STOP,
        [Description("Tạm Hoãn")]
        E_SUSPENSE
    }

    public enum ImportRosterType
    {
        /// <summary>
        /// Ghi đè dữ liệu cả tháng
        /// </summary>
        [Description("Ghi đè dữ liệu cả tháng")]
        OverrideMonth,
        /// <summary>
        /// Ghi đè vùng có dữ liệu
        /// </summary>
        [Description("Ghi đè vùng có dữ liệu")]
        OverrideHasValue,
    }

    public enum RosterStatus
    {
        #region LeaveDayStatus
        E_SUBMIT = 0, E_WAIT_APPROVED, E_FIRST_APPROVED, E_APPROVED, E_REJECTED, E_CANCEL, E_SUBMIT_TEMP
        #endregion
    }

    public enum WorkdayStatus
    {
        #region WorkdayStatus
        [Description("Yêu Cầu")]
        E_SUBMIT,
        [Description("Xác Nhận")]
        E_CONFIRM,
        [Description("Từ Chối")]
        E_REJECTED,
        [Description("Hủy")]
        E_CANCEL,
        [Description("Người Duyệt Đầu")]
        E_FIRST_APPROVED,
        [Description("Duyệt")]
        E_APPROVED,
        [Description("Chờ Duyệt")]
        E_WAIT_APPROVED,
        [Description("Lưu Tạm")]
        E_SUBMIT_TEMP
        #endregion
    }


    public enum AppConfig
    {
        //Tác vụ sau import nhân viên    
        E_TASK_AFTER_IMPORT_PROFILE,
        //Gui Mail
        E_SERVER_MAIL,
        // DateSuckle: ngay sinh con, bo frame se co cau hinh phu hop voi FGL value3 boolean
        E_COLLECT_SOCIAL_INSURANCE_CONFIGFGL,

        E_CONFIG_INSRECORD_WORKDAYBYINOUT,
        E_CONFIG_INSRECORD_WORKDAYBYINOUTMORETHAN2HOUR,
        E_STANDARD_WORKDAY,
        HRM_ATT_OT_,

        //Key Cho phép mặc định trạng thái là Duyệt đối với các dự án ko sd tính năng Phê Duyệt (Task DoanHuynh yêu cầu 0045883) : (bool:false)
        HRM_CONFIG_DEFAULTAPPROVED,

        //Key Cho phép quyết toán ngày nghỉ : value33 (bool)
        HRM_ATT_LEAVEDAY_ALLOWFINALIZATIONHOLIDAYS,
        //Key Không tự động đăng ký ngày nghỉ lễ khi chấm công : value16 (bool)
        HRM_ATT_NOTAUTOREGISTERHOLIDAYLEAVE,
        //Key Tăng ca ngày lễ tính 400% : value30 (bool)
        HRM_ATT_OT_HOLIDAYSCOMPUTE400,
        //Key Cách tính dữ liệu quẹt thẻ 1 lần : value26 (Enum: MissTAM_LeaveType)
        HRM_ATT_MISSTAM_LEAVETYPE,
        //Nếu tăng ca > 2h thì được nghỉ 30phut (vẫn tính tăng ca) : Value21 (bool:false)
        HRM_ATT_OT_ISALLOWADDHOURWHENOTTHANTWOHOUR,
        // Không được phép duyệt OT cho chính bản thân : value45 (bool:true ) 
        HRM_ATT_OT_DONOTAPPROVEOTMYSELF,
        // Không được phép cắt OT giờ nghỉ : value44 (bool:true ) 
        HRM_ATT_OT_DONOTSPLITOTBREAKTIME,

        //Cho Phép Đăng Ký Tăng Ca Khi Đang Hưởng Chế Độ Thai Sản : value35  (bool:false) 
        HRM_ATT_OT_ALLOWREGISTEROTWHENINMATERNITYREGIME,
        //Overtime vượt qua ngày hôm sau, không cắt, mà áp dụng như ca đêm. : value 37 (bool:true)
        HRM_ATT_OT_NOCUTOVERTIMEPASSDAY,
        //Phương thức tính ca đem theo khoản thời gian định nghỉ : value22 (Enum: ConfigNightShift : E_SHIFT)
        HRM_ATT_OT_BYPERIODOFTIME,
        //Thời gian tính ca đem từ : Value13 (TimePicker lưu xuống dạng 21:00)
        HRM_ATT_OT_NIGHTSHIFTFROM,
        //Thời gian tính ca đem đến : Value14 (TimePicker lưu xuống dạng 06:00)
        HRM_ATT_OT_NIGHTSHIFTTO,
        //Trạng Thái nào thì tình tăng ca : Value8 (lưu xuống dạng Enum)
        HRM_ATT_OT_OVERTIMESTATUS,
        // Cho Phép Tách Tăng Ca
        HRM_ATT_OT_ISALLOWCUT,
        // Phép năm chốt từ ngày 1 -> 31 hàng tháng : value46
        HRM_ATT_ANNUALLEAVE_ANNUALBEGINMONTHTOENDMONTH,

        //Loại ngày nghỉ mà nhân viên thử việc không được nghỉ : value97
        HRM_ATT_STAFF_PROBATION,

        HRM_ATT_ANNUALLEAVE_,
        // Cấu Hình Tham Số Tính Phép Năm : value74 (HonDa)
        HRM_ATT_ANNUALLEAVE_FORMULARCONFIG,
        // Cấu Hình Công Thức Tính Phép Năm : value 75 (HonDa)
        HRM_ATT_ANNUALLEAVE_FORMULARCOMPUTE,

        HRM_ATT_ANNUALSICKLEAVE_,
        // Cấu Hình Tham Số Tính Phép Bệnh : value76 (HonDa)
        HRM_ATT_ANNUALSICKLEAVE_FORMULARCONFIG,
        // Cấu Hình Công Thức Tính Phép Bệnh : value 77 (HonDa)
        HRM_ATT_ANNUALSICKLEAVE_FORMULARCOMPUTE,

        HRM_ATT_ANNUALINSURANCELEAVE_,
        // Cấu Hình Tham Số Tính Chi Tiết BHXH : value78 (HonDa)
        HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCONFIG,
        // Cấu Hình Công Thức Tính Chi Tiết BHXH : value 79 (HonDa)
        HRM_ATT_ANNUALINSURANCELEAVE_FORMULARCOMPUTE,
        //Cho phép quyết toán ngày nghỉ : value33 (bool)
        HRM_ATT_ANNUALLEAVE_AllowFinalizationHolidays,

        HRM_ATT_ANNUALDETAIL_,
        // Mã Ngày Nghỉ Không Tính Phép Năm Cho Phần Tử Tính : value66 
        HRM_ATT_ANNUALDETAIL_LEAVECODENOTCOMPUTE,
        // Tháng Bắt Đầu Theo Chu Kỳ Tính Phép Năm (Mặc định là Tháng 1) : value76 
        HRM_ATT_ANNUALDETAIL_MONTHBEGININYEAR,
        // Cho Phép Phân Tích Phép Năm (Cơ Chế Mới) : value77 (bool)
        HRM_ATT_ANNUALDETAIL_ALLOWANALYZEANNUAL,


        // cấu hình giờ kết thúc ăn
        HRM_CAN_MEALRECORD_EATEND_CONFIG,
        // cấu hình số phút xử lý trùng quẹt thẻ ở canteen
        HRM_CAN_MEALRECORD_SCANMULTI_CONFIG,
        // cấu hình số phút xử lý trùng quẹt thẻ ở laundry
        HRM_LAU_LAUNDRYRECORD_SCANMULTI_CONFIG,
        // lưu thời điểm tổng hợp gần đây nhất
        HRM_CAN_MEALRECORD_SUMMARY,
        // lưu thời điểm tổng hợp gần đây nhất
        HRM_LAU_LAUNDRYRECORD_SUMMARY,
        // lưu thời điểm tổng hợp gần đây nhất
        HRM_ATT_WORKDAY_SUMMARY,
        // lưu thời điểm tải dữ liệu chấm công gần đây nhất
        HRM_ATT_TAM_LOADDATA,

        // Key Quy định Có Làm tròn thời gian hay ko?
        HRM_ATT_ROUNDOT,
        // Key Quy định Làm tròn thời gian trong khoản bao nhiêu
        HRM_ATT_ROUNDOT_LINEROUND,

        // Key Quy định giới hạn ràng buộc OT
        HRM_ATT_OT_OTPERMIT_,
        //  Số giờ làm việc tối đa	Theo ngày: valu54
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR,
        HRM_ATT_OT_OTPERMIT_LIMITCOLOR,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV1,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV1,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV1,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV1,
        HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV1,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV2,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV2,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV2,
        HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV2,
        HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV2,
        HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL,
        HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV1,
        HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV2,
        HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER,
        HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV1,
        HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV2,
        // cấu hỉnh màu chữ nhân viên nghỉ việc
        HRM_HRE_CONFIG_PROFILEQUITCOLOR,

        // cấu hỉnh màu nền nhân viên nghỉ việc
        HRM_HRE_CONFIG_PROFILEQUITBACKGROUNDCOLOR,
        // Áp dụng cắt theo giờ làm việc tối đa : value90 (bool)
        HRM_ATT_OT_OTPERMIT_ISALLOWSPLIT,

        //Cho phép OT nếu khác loại
        HRM_ATT_ALLOWSAVEDUPLICATE,

        //Validate roster không được liên tục dưới 12h 
        HRM_ATT_VALIDATE_ROSTER_NON_CONTINUE_12HOUR,

        HRM_SYS_TAMSCANLOG_1_SERVERNAME,
        HRM_ATT_OT_NIGHTSHIFTOFDAYNOTSHIFT_START,
        HRM_ATT_OT_NIGHTSHIFTOFDAYNOTSHIFT_END,
        HRM_ATT_WORKDAY_SUMMARY_MAXHOURSONESHIFT,
        HRM_ATT_WORKDAY_SUMMARY_MAXHOURSNEXTINOUT,
        HRM_ATT_WORKDAY_SUMMARY_MINMINUTESSAMEATT,
        HRM_ATT_WORKDAY_SUMMARY_TYPELOADDATA,
        HRM_ATT_WORKDAY_SUMMARY_SYMBOL,
        HRM_ATT_WORKDAY_SUMMARY_DETECTSHIFT,

        E_SERVER_TAM,
        // html menu main
        E_MAIN_MENU,

        //Thông số thiết lập ngôn ngữ của người dùng
        HRM_SYS_USERSETTING_LANGUAGE,
        //Thông số thiết lập giao diện của người dùng
        HRM_SYS_USERSETTING_THEME,
        //Chuỗi html của SubMenu
        HRM_SYS_USERSETTING_SUBMENU,
        //Giá trị mặc định tổng hợp hợp đồng đến hạn
        HRM_HRE_CONTRACT_ALERT_EXPRIDAY,
        HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEBEFOR,
        HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEAFTE,

        //Giá trị mặc định tổng hợp hợp đồng hết hạn thử việc
        HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY,
        HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY_VALUEBEFOR,
        HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY_VALUEAFTER,
        //Giá trị mặc định tổng hợp sinh nhật gần dến
        HRM_HRE_PROFILE_ALERT_BIRTHDAY_EXPRIDAY,
        //Giá trị mặc định tổng hợp sinh nhật hôm nay
        HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY,
        HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY_VALUEBEFOR,
        HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY_VALUEAFTER,

        // Cấu hình Ngày Nghỉ Chi Tiết Hàng Năm
        HRE_ATT_ANNUALLEAVEDETAIL,
        HRE_ATT_ANNUALLEAVEDETAIL_FORMULARCONFIG,
        HRE_ATT_ANNUALLEAVEDETAIL_FORMULARCOMPUTE,
        //Cau hinh hien thi luong co ban cho duyet
        HRM_SAL_BASICSALARY_ALERT_WAITINGAPROVE,
        //Cau hinh hien thi luong co ban cho duyet (-Ngay so voi hien tai)
        HRM_SAL_BASICSALARY_ALERT_DAYWAITINGAPROVEFROM,
        //Cau hinh hien thi luong co ban cho duyet (+Ngay so voi hien tai)
        HRM_SAL_BASICSALARY_ALERT_DAYWAITINGAPROVETO,

        //Cau hinh hien thi hop dong cho duyet
        HRM_HRE_CONTRACT_ALERT_WAITINGAPROVE,
        //Cau hinh hien thi hop dong cho duyet (-Ngay so voi hien tai)
        HRM_HRE_CONTRACT_ALERT_DAYWAITINGAPROVEFROM,
        ////Cau hinh hien thi hop dong cho duyet (+Ngay so voi hien tai)
        HRM_HRE_CONTRACT_ALERT_DAYWAITINGAPROVETO,

        //Cau hinh hien thi phu luc hd den han
        HRM_HRE_CONTRACT_ALERT_APPENDDIXEXPRIDAY,
        //Cau hinh hien thi phu luc hd den han(-Ngay so voi hien tai)
        HRM_HRE_CONTRACT_ALERT_DAYAPPENDDIXEXPRIDAYFROM,
        //Cau hinh hien thi phu luc hd den han(+Ngay so voi hien tai)
        HRM_HRE_CONTRACT_ALERT_DAYAPPENDDIXEXPRIDAYTO,

        //Cau hinh hien thi Nv vao HDT cho duyet
        HRM_HRE_PROFILE_ALERT_HDTJOBWAINTINGAPPROVED,
        //Cau hinh hien thi Nv vao HDT cho duyet(-Ngay so voi hien tai)
        HRM_HRE_PROFILE_ALERT_DAYHDTJOBWAINTINGAPPROVEDFROM,
        //Cau hinh hien thi Nv vao HDT cho duyet(+Ngay so voi hien tai)
        HRM_HRE_PROFILE_ALERT_DAYHDTJOBWAINTINGAPPROVEDTO,

        //cau hinh hien thi Nv sap den ngay tam hoan
        HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRY,
        //cau hinh hien thi Nv sap den ngay tam hoan(-Ngay so voi hien tai)
        HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRYFROM,
        //cau hinh hien thi Nv sap den ngay tam hoan(+Ngay so voi hien tai)
        HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRYTO,


        //cau hinh hien thi Nv sap den ngày nghỉ việc
        HRM_HRE_PROFILE_ALERT_DAYSTOPWORKING,
        //cau hinh hien thi Nv sap den ngày nghỉ việc(-Ngay so voi hien tai)
        HRM_HRE_PROFILE_ALERT_DAYSTOPWORKINGFROM,
        //cau hinh hien thi Nv sap den ngày nghỉ việc(+Ngay so voi hien tai)
        HRM_HRE_PROFILE_ALERT_DAYSTOPWORKINGTO,


        //cau hinh hien thi Nv sap vào làm lại
        HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRY,
        //cau hinh hien thi Nv sap vào làm lại(-Ngay so voi hien tai)
        HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRYFROM,
        //cau hinh hien thi Nv sap vào làm lại(+Ngay so voi hien tai)
        HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRYTO,

        //cau hinh hien thi người chờ đánh giá
        HRM_EVA_PROFILE_ALERT_EVALUTIONWAITING,
        //cau hinh hien thi người chờ đánh giá(-Ngay so voi hien tai)
        HRM_EVA_PROFILE_ALERT_DAYEVALUTIONWAITINGFROM,
        //cau hinh hien thi người chờ đánh giá(+Ngay so voi hien tai)
        HRM_EVA_PROFILE_ALERT_DAYEVALUTIONWAITINGTO,


        //Lấy dữ liệu chấm công từ server
        HRM_SYS_TAMSCANLOG_1_,
        HRM_SYS_TAMSCANLOG_2_,
        HRM_SYS_CAN_TAMSCANLOG_1_,
        HRM_SYS_CAN_TAMSCANLOG_2_,
        HRM_SYS_LAU_TAMSCANLOG_1_,
        HRM_SYS_LAU_TAMSCANLOG_2_,
        FILTERDATASPLIT,
        FILTERDATA,
        ISCODEEMP,
        DELAY_DAYS,
        SYNC_CARD,
        ISACTIVATED,
        SERVERNAME,
        DBNAME,
        USERID,
        PASSWORD,
        TABLENAME,
        TYPE,
        MACHINECODE,
        CODEFIELDNAME,
        DATACOLNAME,
        CARDCOLNAME,
        COLUMNFILTER,
        DATAFILTER,
        COMPLEXDATE,
        COMPLEXTIME,
        ISCOMPLEXTIME,
        TIMECONVERTSQLFORMATNUM,
        TYPECOLNAME,

        //hỢP ĐỒNG LAO ĐỘNG ĐẾN HẠN
        USOKPERMITED_BOOL,
        USOKPERMITED_VALUE,
        //hỢP ĐỒNG LAO ĐỘNG ĐẾN HẠN CỦA HẠN CỦA NHÂN VIÊN ĐẢNG ĐOÀN
        // USOKPERMITED_EMP_WORTH,
        USOKPERMITED_EMP_WORTH,
        USOKPERMITED_EMP_WORTH_VALUEBEFOR,
        USOKPERMITED_EMP_WORTH_VALUEAFTER,
        //NHân Viên Chưa Lợp Tài Liệu
        // EMP_LETTERED,
        EMP_LETTERED,



        EMP_LETTERED_VALUEBEFOR,
        EMP_LETTERED_VALUEBEFOR_VALUEBEFOR,
        EMP_LETTERED_VALUEBEFOR_VALUEAFTER,
        EMP_LETTERED_VALUEAFTER,
        EMP_LETTERED_VALUEAFTER_VALUEBEFOR,
        EMP_LETTERED_VALUEAFTER_VALUEAFTER,

        E_NEWEMPLOYEE,
        E_NEWEMPLOYEE_VALUEBEFOR,
        E_NEWEMPLOYEE_VALUEAFTER,

        E_STOPWORKING,
        E_STOPWORKING_VALUEBEFOR,
        E_STOPWORKING_VALUEAFTER,

        VISAED,
        VISAED_VALUEBEFOR,
        VISAED_VALUEAFTER,
        //Giấy Phép Lao Động Đến Hạn
        PAPRlABORPHEPSLED,
        PAPRlABORPHEPSLED_VALUEBEFOR,
        PAPRlABORPHEPSLED_VALUEAFTER,

        //Yêu Cầu Tuyển Dụng
        E_REQUEST,
        E_REQUESt_VALUEBEFOR,
        E_REQUEST_VALUEAFTER,
        //Chứng Chỉ Học Viên Sắp Hết Hạng
        TRA_CERTIFICATED,
        TRA_CERTIFICATED_VALUEBEFOR,
        TRA_CERTIFICATED_VALUEAFTER,
        //E_COLLECT_SOCIAL_INSURANCE,

        #region [AppConfig] System - Hệ Thống
        //HRM_SYS_CONFIG_ABC,//bat dau la HRM_SYS_CONFIG_ [ABC là tên tuỳ chọn]

        #region Cấu hình Mail
        HRM_SYS_CONFIG_MAIL_MAILSERVER,
        HRM_SYS_CONFIG_MAIL_MAILUSERNAME,
        HRM_SYS_CONFIG_MAIL_MAILPASSWORD,
        HRM_SYS_CONFIG_MAIL_IMAP4,
        HRM_SYS_CONFIG_MAIL_IMAP2,
        HRM_SYS_CONFIG_MAIL_NOTLS,
        HRM_SYS_CONFIG_MAIL_TLS,
        HRM_SYS_CONFIG_MAIL_SSL,
        #endregion

        #region Tải Dữ Liệu Vào Ra
        //HRM_SYS_TAMSCANLOG_1_USERID,
        //HRM_SYS_TAMSCANLOG_1_PASSWORD,
        //HRM_SYS_TAMSCANLOG_1_DBNAME,
        //HRM_SYS_TAMSCANLOG_1_TABLENAME,
        //HRM_SYS_TAMSCANLOG_1_CARDCOLNAME,
        //HRM_SYS_TAMSCANLOG_1_DATACOLNAME,
        //HRM_SYS_TAMSCANLOG_1_MACHINECODE,
        //HRM_SYS_TAMSCANLOG_1_TYPE,
        //HRM_SYS_TAMSCANLOG_1_CODEFIELDNAME,
        //HRM_SYS_TAMSCANLOG_2_SERVERNAME,
        //HRM_SYS_TAMSCANLOG_2_USERID,
        //HRM_SYS_TAMSCANLOG_2_PASSWORD,
        //HRM_SYS_TAMSCANLOG_2_DBNAME,
        //HRM_SYS_TAMSCANLOG_2_TABLENAME,
        //HRM_SYS_TAMSCANLOG_2_CARDCOLNAME,
        //HRM_SYS_TAMSCANLOG_2_MACHINECODE,
        //HRM_SYS_TAMSCANLOG_2_TYPE,
        //HRM_SYS_TAMSCANLOG_2_CODEFIELDNAME,
        //HRM_SYS_TAMSCANLOG_2_DATACOLNAME,

        //// Kích Hoạt
        //HRM_SYS_TAMSCANLOG_1_ISACTIVATED,
        //HRM_SYS_TAMSCANLOG_2_ISACTIVATED,
        //// Tải DL theo Mã NV
        //HRM_SYS_TAMSCANLOG_1_ISCODEEMP,
        //HRM_SYS_TAMSCANLOG_2_ISCODEEMP,
        //// Dữ liệu lọc (Phân cách: ',' hoặc ';')
        //HRM_SYS_TAMSCANLOG_1_FILTERDATASPLIT,
        //HRM_SYS_TAMSCANLOG_2_FILTERDATASPLIT,
        //// Tên Cột Để Lọc Dữ Liệu
        //HRM_SYS_TAMSCANLOG_1_FILTERDATA,
        //HRM_SYS_TAMSCANLOG_2_FILTERDATA,

        #endregion

        #endregion

        #region [AppConfig] Category - Danh Mục
        //HRM_CAT_CONFIG_ABC,//bat dau la HRM_CAT_CONFIG_ [ABC là tên tuỳ chọn]

        #region Cat_Export
        /// <summary> Password chỉ đọc </summary>
        HRM_CAT_CONFIG_EXPORT_EXCELPASSREADONLY,
        /// <summary> Password mở file </summary>
        HRM_CAT_CONFIG_EXPORT_EXCELPASSOPENFILE,
        #endregion

        #endregion

        #region [AppConfig] Attendance - Chấm Công
        //HRM_ATT_CONFIG_ABC,//bat dau la HRM_ATT_CONFIG_ [ABC là tên tuỳ chọn]
        /// <summary> validate vấn đề roster không được liên tục dưới 12 tiếng </summary>
        HRM_ATT_CONFIG_VALIDATE_ROSTER_NON_CONTINUE_12HOUR,
        HRM_ATT_CONFIG_ANL_MONTHBEGINYEAR,
        HRM_ATT_CONFIG_ANL_DAYBEGIN_FULLMONTH,
        HRM_ATT_CONFIG_ANL_SENIOR_MONTH,
        HRM_ATT_CONFIG_ANL_DAY_PER_MONTH,
        HRM_ATT_CONFIG_ANL_ROUND_UP,
        HRM_ATT_CONFIG_ANL_TYPE_PROFILE_BEGIN,
        HRM_ATT_CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL,
        HRM_ATT_CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR,
        HRM_ATT_CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL,
        HRM_ATT_CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL,
        HRM_ATT_CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL,
        HRM_ATT_CONFIG_LEAVE_NON_ALN_CODES,
        HRM_ATT_CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR,
        /// <summary>
        /// dung để cho phép đăng ký rostergroup theo mã của ca làm việc
        /// </summary>
        HRM_ATT_CONFIG_SHIFT_CODE_ROSTERGROUP,
        HRM_ATT_CONFIG_NAME_ROSTERGROUP,
        HRM_ATT_CONFIG_NUMBER_LEAVE_APPROVE_OVERTIME,

        #endregion

        #region [AppConfig] Canteen - Nhà Ăn
        //HRM_CAN_CONFIG_ABC,//bat dau la HRM_CAN_CONFIG_ [ABC là tên tuỳ chọn]

        #endregion

        #region [AppConfig] HRE - Nhân Sự
        //HRM_HRE_CONFIG_ABC,//bat dau la HRM_HRE_CONFIG_ [ABC là tên tuỳ chọn]

        HRM_HRE_CONFIG,
        HRM_HRE_CONFIG_GROUPBYORGPROFILEQUIT,
        HRM_HRE_CONFIG_CHECKDUPLICATEPROFILENAME,
        HRM_HRE_CONFIG_CHECKDUPLICATECODEEMP,
        HRM_HRE_CONFIG_CHECKDUPLICATECODEATT,
        HRM_HRE_CONFIG_CHECKDUPLICATERELATIVES,
        HRM_HRE_CONFIG_CHECKDUPLICATEPROFILENAMEANDBIRTHDAY,
        HRM_HRE_CONFIG_CHECKDUPLICATEIDNO,
        HRM_HRE_CONFIG_DEFAULTCURRENCY,
        /// <summary> Key tạo mã hợp đồng </summary>
        HRM_HRE_GENERATE_CODE_CONTRACT,
        HRM_HRE_CONFIG_SHOWEXCONBYCONRESULT,
        HRM_HRE_CONFIG_DAYSUSPENSEEXPIRY,
        HRM_HRE_CONFIG_DAYSTOPWORKINGEXPIRY,
        HRM_HRE_CONFIG_DAYCOMEBACKEXPIRY,
        HRM_HRE_CONFIG_INPUTGENERALCANDIDATEMANUAL,
        HRM_HRE_CONFIG_ISUSECODEEMPOFCUSTOMER,
        HRM_HRE_CONFIG_ISALERTIFNUMBEROFEMPEXCEEDPLAN,
        HRM_HRE_CONFIG_FIELDVALIDATEHIREPROFILE,
        HRM_HRE_CONFIG_ISNOTUSEEXPIRYCONTRACTLOOP,
        #endregion

        #region [AppConfig] Laundry - Giặt Là
        //HRM_LAU_CONFIG_ABC,//bat dau la HRM_LAU_CONFIG_ [ABC là tên tuỳ chọn]

        #endregion

        #region [AppConfig] Medical - Y Tế
        //HRM_MED_CONFIG_ABC,//bat dau la HRM_MED_CONFIG_ [ABC là tên tuỳ chọn]

        #endregion

        #region [AppConfig] Training - Đào Tạo
        //HRM_TRA_CONFIG_ABC,//bat dau la HRM_TRA_CONFIG_ [ABC là tên tuỳ chọn]
        HRM_TRA_CONFIG,
        // Cấu hình số ngày thông báo hết hạn chứng chỉ
        // Số ngày trước ngày hiện tại
        HRM_TRA_CONFIG_TRAIN_EXPIR_DAY_WARRING_FROM,
        // Số ngày sau ngày hiện tại
        HRM_TRA_CONFIG_TRAIN_EXPIR_DAY_WARRING_TO,
        //Add Học Viên Vào Lớp Mà Không Có Trong Yêu Cầu Đào Tạo 
        HRM_TRA_CONFIG_TRAINEE_IN_REQUIREMENTDETAIL,
        #endregion

        #region [AppConfig] Recruitment - Tuyển Dụng
        //HRM_REC_CONFIG_ABC,//bat dau la /HRM_REC_CONFIG_ [ABC là tên tuỳ chọn]

        #endregion

        #region [AppConfig] Insurance - Bảo Hiểm
        //HRM_INS_CONFIG_ABC,//bat dau la HRM_INS_CONFIG_ [ABC là tên tuỳ chọn]
        /// <summary> Tổng hợp dữ liệu BH tạm đến ngày [int] </summary>
        HRM_INS_CONFIG_SUMDATATEMPT,
        /// <summary> Tổng hợp dữ liệu BH chính thức đến ngày [int] </summary>
        HRM_INS_CONFIG_SUMDATAMAIN,
        /// <summary> Ngày Chốt BH (ngày 18 đối với honda) [int] </summary>
        HRM_INS_CONFIG_PERIODINSURANCEYDAY,
        /// <summary> Ngày Bắt Đầu Chu Kỳ Báo Giảm LĐ (ngày 15 tháng N-1 đối với honda) [int] </summary>
        HRM_INS_CONFIG_PERIODINSURANCEDAYDESCREASEFROM,
        /// <summary> Ngày Chốt Chu Kỳ Báo Giảm LĐ (ngày 14 tháng N đối với honda) [int] </summary>
        HRM_INS_CONFIG_PERIODINSURANCEDAYDESCREASETO,
        /// <summary> Ngày Bắt Đầu Chu Kỳ BH  (ngày 15 tháng [N-1] đối với honda) [int] </summary>
        HRM_INS_CONFIG_PERIODINSURANCEDAYDEFAULTFROM,
        /// <summary> Ngày Chốt Chu Kỳ BH (ngày 14 tháng N đối với honda) [int] </summary>
        HRM_INS_CONFIG_PERIODINSURANCEDAYDEFAULTTO,
        /// <summary> Ngày bắt đầu số ngày nghỉ >=14 ngày  (ngày 1 tháng N đối với honda) [int] </summary>
        HRM_INS_CONFIG_PERIODINSURANCE14DAYFROM,
        /// <summary> Ngày kết thúc số ngày nghỉ >=14 ngày  (ngày 31 tháng N đối với honda) [int] </summary>
        HRM_INS_CONFIG_PERIODINSURANCE14DAYTO,
        /// <summary> Ngày bắt đầu Thay đổi lương  (ngày 15 tháng [N-1] đối với honda) [int] </summary>
        HRM_INS_CONFIG_PERIODINSURANCEDAYCHANGESALFROM,
        /// <summary> Ngày kết thúc Thay đổi lương  (ngày 14 tháng [N] đối với honda) [int] </summary>
        HRM_INS_CONFIG_PERIODINSURANCEDAYCHANGESALTO,
  
        #endregion

        #region [AppConfig] Evaluation - Đánh Giá
        //HRM_EVA_CONFIG_ABC,//bat dau la HRM_EVA_CONFIG_ [ABC là tên tuỳ chọn]

        /// <summary> Thời gian bắt đầu cho phép nhập điểm </summary>
        HRM_EVA_CONFIG_DATESTARTMARK,
        /// <summary> Thời gian kết thúc cho phép nhập điểm </summary>
        HRM_EVA_CONFIG_DATEENDMARK,
        /// <summary> Thời Gian Bắt Đầu Đánh Giá Lần 1 [DateTime] </summary>
        HRM_EVA_CONFIG_DATESTARTNODETIME1,
        /// <summary> Thời Gian Kết Thúc Đánh Giá Lần 1 [DateTime] </summary>
        HRM_EVA_CONFIG_DATEENDNODETIME1,
        /// <summary> Thời Gian Bắt Đầu Đánh Giá Lần 2 [DateTime] </summary>
        HRM_EVA_CONFIG_DATESTARTNODETIME2,
        /// <summary> Thời Gian Kết Thúc Đánh Giá Lần 3 [DateTime] </summary>
        HRM_EVA_CONFIG_DATEENDNODETIME2,
        /// <summary>
        /// Cấu hình Ngày Kết Thúc Năm Tài Chính
        /// </summary>
        HRM_EVA_CONFIG_DAYENDYEARFINANCE,

        #region Cấu Hình Lưu Đánh Giá
        //Cấu hình Lưu cột C1
        HRM_EVA_CONFIG_SAVE_C1,
        //Cấu hình Lưu cột C2
        HRM_EVA_CONFIG_SAVE_C2,
        //Cấu hình Lưu cột C3
        HRM_EVA_CONFIG_SAVE_C3,
        //Cấu hình Lưu cột C4
        HRM_EVA_CONFIG_SAVE_C4,
        //Cấu hình Lưu cột C5
        HRM_EVA_CONFIG_SAVE_C5,
        //Cấu hình Lưu cột C6
        HRM_EVA_CONFIG_SAVE_C6,
        //Cấu hình Lưu cột C7
        HRM_EVA_CONFIG_SAVE_C7,
        //Cấu hình Lưu cột C8
        HRM_EVA_CONFIG_SAVE_C8,
        //Cấu hình Lưu cột C9
        HRM_EVA_CONFIG_SAVE_C9,
        //Cấu hình Lưu cột C10
        HRM_EVA_CONFIG_SAVE_C10,
        //Cấu hình Lưu cột C11
        HRM_EVA_CONFIG_SAVE_C11,
        //Cấu hình Lưu cột C12
        HRM_EVA_CONFIG_SAVE_C12,
        //Cấu hình Lưu cột C13
        HRM_EVA_CONFIG_SAVE_C13,
        //Cấu hình Lưu cột C14
        HRM_EVA_CONFIG_SAVE_C14,
        //Cấu hình Lưu cột C15
        HRM_EVA_CONFIG_SAVE_C15,
        //Cấu hình Lưu cột C16
        HRM_EVA_CONFIG_SAVE_C16,
        #endregion


        #region cau hinh nhac nho mail
        /// <summary> Nhac nho nhan vien danh gia </summary>
        HRM_EVA_CONFIG_EMAIL_ISEMPLOYEEEVALUATION,
        /// <summary> Nội dung Nhac nho nhan vien danh gia</summary>
        //HRM_EVA_CONFIG_EMAIL_EMPLOYEEEVALUATION_CONTENT,
        /// <summary> Nhac nho nguoi duyet danh gia 1 </summary>
        HRM_EVA_CONFIG_EMAIL_ISUSERAPPROVEVALUATION1,
        /// <summary> Noi dung Nhac nho nguoi duyet danh gia 1</summary>
        //HRM_EVA_CONFIG_EMAIL_USERAPPROVEVALUATION_CONTENT1,
        /// <summary> Nhac nho nguoi duyet danh gia 2</summary>
        HRM_EVA_CONFIG_EMAIL_ISUSERAPPROVEVALUATION2,
        /// <summary> Noi dung Nhac nho nguoi duyet danh gia 2</summary>
        //HRM_EVA_CONFIG_EMAIL_USERAPPROVEVALUATION_CONTENT2,
        /// <summary> Theo chu ky</summary>
        HRM_EVA_CONFIG_EMAIL_ISCYCLE,
        /// <summary> Theo chu ky Ngay 1</summary>
        HRM_EVA_CONFIG_EMAIL_CYCLE_DAY1,
        /// <summary> Theo chu ky Ngay 2</summary>
        HRM_EVA_CONFIG_EMAIL_CYCLE_DAY2,
        /// <summary> Theo chu ky Ngay 3</summary>
        HRM_EVA_CONFIG_EMAIL_BYCYCLE_DAY3,
        /// <summary> Theo thoi gian</summary>
        HRM_EVA_CONFIG_EMAIL_ISTIME,
        /// <summary> Theo thoi gian Tu Ngay</summary>
        HRM_EVA_CONFIG_EMAIL_TIME_DATEFROM,
        /// <summary> Theo thoi gian den ngay</summary>
        HRM_EVA_CONFIG_EMAIL_TIME_DATETO,
        #endregion


        #endregion

        #region [AppConfig] Salary - Lương
        //HRM_SAL_CONFIG_ABC,//bat dau la HRM_SAL_CONFIG_ [ABC là tên tuỳ chọn]

        #region Sys_AppConfig Lương
        HRM_SAL_CONFIG_VALUE8,
        HRM_SAL_CONFIG_VALUE52,
        HRM_SAL_CONFIG_VALUE53,
        HRM_SAL_CONFIG_VALUE54,
        HRM_SAL_CONFIG_VALUE55,
        HRM_SAL_CONFIG_VALUE56,
        HRM_SAL_CONFIG_VALUE57,
        HRM_SAL_CONFIG_VALUE58,
        HRM_SAL_CONFIG_VALUE59,
        #endregion

        #region Hold Salary
        HRM_SAL_HOLDSALARY_ELEMENT,
        HRM_SAL_HOLDSALARY_ELEMENT_AFTERTAX,
        #endregion

        #region Ngày chốt tính phụ cấp
        HRM_SAL_DATECLOSE_ALLOWANCE,
        #endregion

        #region Ngày chốt lương
        HRM_SAL_DATECLOSE_SALARY,
        #endregion

        #region Phần tử thực nhận
        HRM_SAL_ELEMENT_REALWAGES,
        #endregion

        #region Tính công thức theo số thứ tự
        HRM_SAL_COMPUTEPAYROLL_ORDERNUMBER,
        #endregion

        #region Ngày nghỉ việc hiển thị cảnh báo trang chủ
        SAL_SETTLEMENT,
        SAL_SETTLEMENT_VALUEAFTER,
        SAL_SETTLEMENT_VALUEBEFOR,
        #endregion

        #region Sal_Config
        /// <summary>
        /// Hệ Số Lương Sản Phẩm
        /// Lấy Lương Cơ Bản
        /// Value 52
        /// </summary>
        E_RATE_BY_BASICSALARY,
        /// <summary>
        /// Hệ Số Lương Sản Phẩm
        /// Nhập Theo Nhu Cầu Phát Sinh Thực Tế
        /// Value 52
        /// </summary>
        E_RATE_BY_ATUALARISING,

        /// <summary>
        /// Các Trường Hợp Tính OT
        /// Tính OT Đã Quy Đổi
        /// Value 54
        /// </summary>
        E_OT_CHANGE_COMPUTE,
        /// <summary>
        /// Các Trường Hợp Tính OT
        /// Tính OT Chưa Quy Đổi
        /// Value 54
        /// </summary>
        E_OT_NO_CHANGE_COMPUTE,
        /// <summary>
        /// Các Trường Hợp Tính OT
        /// Không Tính OT
        /// Value 54
        /// </summary>
        E_NOT_OT_COMPUTE,

        #endregion

        #region payroll
        HRM_SAL_PAYROLL_CONFIGELEMENT,
        #endregion

        #endregion

        #region  #region [AppConfig] Cấu hình chung
        HRM_SYS_SETTING_LANGUAGE_DEFAULT,
        #endregion

    }//end AppConfig

    public enum ModifyType
    {
        E_EDIT,
        E_CREATE
    }

    public enum ShiftItemType
    {
        E_OVERTIME, E_SHIFTBREAK
    }
    public enum GradeHoursType
    {
        #region Work Hours
        /// <summary>
        /// Gradecfg Hours work
        /// </summary>
        E_FIX_HOURS,
        E_SHIFT_HOURS
        #endregion
    }
    public enum LeaveDayStatus
    {
        #region LeaveDayStatus
        E_SUBMIT, E_FIRST_APPROVED, E_APPROVED, E_WAIT_APPROVED, E_REJECTED, E_CANCEL
        #endregion
    }
    public enum DutyCode
    {
        E_ON,
        E_OFF,
        E_NOT_HIRED,
        E_TERMINATED
    }
    public enum AnnualLeaveDetailType
    {
        [Description("Nghỉ Ốm")]
        E_SICK_LEAVE,
        [Description("Nghỉ Phép")]
        E_ANNUAL_LEAVE,
        [Description("Nghỉ Bảo Hiểm")]
        E_INSURANCE_LEAVE
    }
    public enum LeaveDayDurationType
    {
        #region LeaveDayDurationType
        [Description("Toàn Ca")]
        E_FULLSHIFT,
        [Description("Nữa Ca Trước")]
        E_FIRSTHALFSHIFT,
        [Description("Nữa Ca Sau")]
        E_LASTHALFSHIFT,
        [Description("Giữa Ca")]
        E_MIDDLEOFSHIFT
        #endregion
    }
    public enum AttendanceTableStatus
    {
        #region AttendanceTableStatus
        E_UNPREPARE, E_WAITING, E_APPROVE, E_LOCK
        #endregion
    }
    public enum LeavedayTypeCode
    {
        #region LeavedayTypeCode_have to exist
        /// <summary>
        /// Absenteeism leave - Nghỉ việc riêng
        /// </summary>
        PER,
        /// <summary>
        /// Absenteeism leave - Mã hệ thống
        /// </summary>
        ABS,
        /// <summary>
        /// Annual Leave - Mã hệ thống
        /// </summary>
        ANL,
        /// <summary>
        /// Holiday Leave - Mã hệ thống
        /// </summary>
        HLD,
        #endregion
        #region LeavedayTypeCode


        /// <summary>
        /// Holiday Leave - No Pay (due to Overtime)
        /// </summary>
        HLD_NP,
        /// <summary>
        /// NO PAY
        /// </summary>
        NOPAY,
        /// <summary>
        /// Có phép hưởng lương
        /// </summary>
        PAY,
        /// <summary>
        /// Biz Trip
        /// </summary>
        BIZ,
        /// <summary>
        /// Cong ty cho nghi COMPANYLEAVE_TAISUN
        /// </summary>
        CPL,
        /// <summary>
        /// Nghi dai han co phep khong luong - CPG 
        /// </summary>
        NPS,
        /// <summary>
        /// Nghỉ ốm để dành
        /// </summary>
        SAVESICK,
        /// <summary>
        /// Nghỉ bù
        /// </summary>
        CPS,
        /// <summary>
        /// Nghỉ bù
        /// </summary>
        OCPS,
        /// <summary>
        /// nghi cham con nho duoi 1 tuoi
        /// </summary>
        PSN,
        /// <summary>
        /// Nghi di party
        /// </summary>
        PARTY,
        /// <summary>
        /// Unpaid (không duyệt phép)
        /// </summary>
        KD,
        ///// <summary>
        ///// Unpaid (không phép không báo)
        ///// </summary>
        //ABS,
        /// <summary>
        /// Unpaid (có phép)
        /// </summary>
        AWS,




        #endregion
        #region Insurance
        /// <summary>
        /// Nghi hau san - Mã hệ thống
        /// </summary>
        PRG,
        /// <summary>
        /// Nghỉ ốm
        /// </summary>
        SICK,

        /// <summary>
        /// Con Om
        /// </summary>

        BBS,
        /// <summary>
        /// Nghỉ dưỡng sức
        /// </summary>
        HEA,
        /// <summary>
        /// Nghỉ khám thai
        /// </summary>
        CHK,
        /// <summary>
        /// Nghỉ được hưởng lương theo ti le
        /// </summary>
        PRO,
        #endregion
    }
    public enum OverTimeBeginType
    {
        #region OverTimeBeginType
        E_BEFORE_SHIFT, E_AFTER_SHIFT
        #endregion
    }
    public enum OverTimeStatus
    {
        #region OverTimeStatus
        E_SUBMIT, E_CONFIRM, E_APPROVED, E_APPROVED1, E_REJECTED, E_CANCEL, E_FIRST_APPROVED, E_SUBMIT_TEMP, E_WAIT_APPROVED,
        /// <summary>
        /// Luu xuong database nhung khong view len
        /// </summary>
        E_TEMP
        #endregion
    }
    public enum AttendanceDataStatus
    {
        [Description("Yêu Cầu")]
        E_SUBMIT,
        [Description("Xác Nhận")]
        E_CONFIRM,
        [Description("Từ Chối")]
        E_REJECTED,
        [Description("Hủy")]
        E_CANCEL,
        [Description("Người Duyệt Đầu")]
        E_FIRST_APPROVED,
        [Description("Duyệt")]
        E_APPROVED,
        [Description("Chờ Duyệt")]
        E_WAIT_APPROVED,
        [Description("Lưu Tạm")]
        E_SUBMIT_TEMP
    }

    public enum Att_ComputeOvertimeStatus
    {
        [Description("Yêu Cầu")]
        E_SUBMIT,
        [Description("Chờ Duyệt")]
        E_WAIT_APPROVED,
    }
    public enum OverTimeType
    {
        #region OverTimeType
        E_WORKDAY, E_WORKDAY_NIGHTSHIFT, E_WEEKEND, E_WEEKEND_NIGHTSHIFT, E_HOLIDAY, E_HOLIDAY_NIGHTSHIFT,
        /// <summary>
        /// Lam bu 100%
        /// </summary>
        E_COMPENSATION,
        E_COMPENSATION_NIGHTSHIFT
            // Xử lý trường hợp Thời vụ làm ca đêm và được nhận PC ca đêm 30%
            ,
        E_COMPENSATION_30
            // Xử lý trường hợp Thời vụ OT ca đêm và được nhận PC ca đêm 130%
            , E_COMPENSATION_130

        #endregion
    }
    public enum OverTimeOverLimitType
    {
        #region OverTimeOverLimitType
        E_NONE,
        E_DAY, E_DAY_LV1, E_DAY_LV2,
        E_WEEK, E_WEEK_LV1, E_WEEK_LV2,
        E_MONTH, E_MONTH_LV1, E_MONTH_LV2,
        E_YEAR, E_YEAR_LV1, E_YEAR_LV2,
        #endregion
    }
    public enum MethodOption
    {
        #region MethodOption
        [Description("Trả Tiền")]
        E_CASHOUT,
        [Description("Nghỉ Bù")]
        E_TIMEOFF
        #endregion
    }
    public enum HolidayType
    {
        #region HolidayType
        E_HOLIDAY_HLD, E_WEEKEND_HLD, E_OTHER
        #endregion
    }

    public enum AnlProfileTypeBegin
    {
        E_DATE_HIRE, E_DATE_ENDPROBATION
    }

    public enum FormulaAnual
    {
        #region Cấu Hình Công Thức Phép Năm

        /// <summary>
        /// Tháng bắt đầu tính phép nắm Mặc định là tháng 1
        /// </summary>
        CONFIG_ANL_MONTHBEGINYEAR,

        /// <summary>
        /// Ngày bắt đầu để tính tháng Full
        /// Sau ngày này sẽ chuyển tháng bắt đầu qua tháng sau
        /// lấy ngày này để tính phép thâm niên
        /// </summary>
        CONFIG_ANL_DAYBEGIN_FULLMONTH,

        /// <summary>
        /// Số tháng để bắt đầu tính thâm niên
        /// </summary>
        CONFIG_ANL_SENIOR_MONTH,

        /// <summary>
        /// Số ngày trung bình cho 1 tháng mặc đinh 30
        /// </summary>
        CONFIG_ANL_DAY_PER_MONTH,

        /// <summary>
        /// Hệ số để làm tròn lên và làm tròn xuống mặc định 0.5
        /// </summary>
        CONFIG_ANL_ROUND_UP,

        /// <summary>
        /// Loai để bắt đầu tính phép cho 1 nhân viên vd. Ngày DateHire, ngày Hến Hạn thử việc v.v
        /// Value của phần tử này là Enum AnlProfileTypeBegin
        /// </summary>
        CONFIG_ANL_TYPE_PROFILE_BEGIN,

        /// <summary>
        /// Số Ngày để bắt đầu tính Nghỉ phép và tính quy tắc HDT4 HDT5
        /// </summary>
        CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL,


        /// <summary>
        /// số phép năm bình thường có thể nhận 1 năm
        /// </summary>
        CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR,
        /// <summary>
        /// số phép năm Có có thể nhận cho 1 bậc . Mặc định là 1
        /// </summary>
        CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL,
        /// <summary>
        /// số phép năm Có có thể nhận nhiều hơn NORMAL . Mặc định là 2
        /// </summary>
        CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL,
        /// <summary>
        /// số phép năm Có có thể nhận nhiều hơn NORMAL . Mặc định là 4
        /// </summary>
        CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL,


        #endregion

        #region Công Thức Phép Năm

        /// <summary>
        /// Số tháng đc tính phép năm bình thường
        /// </summary>
        ANL_NORMAL,

        /// <summary>
        /// Số tháng nghỉ ko được hưởng phép
        /// </summary>
        ANL_LEAVE_NON_HAVEANL,

        /// <summary>
        /// Lây ra tháng Hiện Tại
        /// </summary>
        ANL_CURRENTMONTH,

        /// <summary>
        /// Tính ra số tháng được hưởng thâm niên
        /// </summary>
        ANL_SENIOR,

        /// <summary>
        /// Tính ra nhân viên đó tháng đó có làm công việc nặng nhọc loại 4 hay không ?? Trả về 1 hoặc 0
        /// </summary>
        ANL_WORK_HDT4,


        /// <summary>
        /// Tính ra nhân viên đó tháng đó có làm công việc nặng nhọc loại 5 hay không ?? Trả về 1 hoặc 0
        /// </summary>
        ANL_WORK_HDT5,


        #endregion
    }

    public enum AttendanceTableItemType
    {
        E_NORMAL,
        E_TOTAL_ROW,
    }

    public enum FormulaConstant
    {
        #region Function
        //Function:ham Or nhu Excel
        //Created By: Tran.Pham
        //Created Date: 07/04/2011
        OR,
        //Function:ham Round (Value,number of round)
        //Created By: Tran.Pham
        //Created Date: 07/04/2011
        ROUND,
        //Function:ham ROUNDCUSTOMIZE(Double _hour, double upToHours, double minuteRound, Boolean IsUpTo)
        //Created By: LamLe ()
        //Created Date: 26/11/2012
        ROUNDCUSTOMIZE,
        MFLOOR,
        //Function:ham Roundup nhu Excel
        //Created By: Vi.nguyen
        //Created Date: 08/04/2012
        ROUNDUP,
        //Function:ham isnull nhu sql
        //Created By: Vi.nguyen
        //Created Date: 08/04/2012
        ISNULL,
        //Function:ham int nhu Excel
        //Created By: Hien.Pham
        //Created Date: 20121026
        INT,

        #endregion

        #region Công Thức Phụ Cấp
        //Function:tiền theo level
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        LEVEL,
        //Function:Ngày công chuẩn
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        STDDAY,
        //Function:Giờ công chuẩn
        //Created By: Hien.Pham
        //Created Date: 08/03/2011
        STD_HOUR_PER_DAY,
        //Function:Ngày công thực tế 
        //Created By: LamLe
        //Created Date: 08/03/2011
        REALDAY,
        //Function:Dem so Ngày công thực tế (co quet the thi tinh 1 ngay)
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        REALDAY_COUNT,
        //Function: Dem so Ngày công thực tế di lam duoc huong thai san 1h(co quet the thi tinh 1 ngay)
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        REALDAY_PREGNANCY_COUNT,
        //Function:Ngày công thực tế và ngày nghỉ có lương
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        REALDAY_AND_PAIDLEAVE_COUNT,
        //Function:Ngày công tính lương đã trừ trễ sớm
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        WORKING_PAIDLEAVE_DEDUCTION_LATE_EARLY_DAY,

        //Function: Ngày công tính lương đã trừ trễ sớm sau khi kết thúc thử việc trong tháng
        //Created By: SonNgo
        //Created Date: 20120627
        PAID_DAYS_DEDUCTION_LATE_EARLY_DAY_END_PROBATION,

        //Function: số ngày công tính lương sau khi kết thúc thử việc trong tháng
        //Created By: SonNgo
        //Created Date: 20120713
        PAID_DAYS_END_PROBATION,

        //Function: số ngày nghỉ không tính lương sau khi kết thúc thử việc trong tháng
        //Created By: SonNgo
        //Created Date: 20120731
        UNPAID_DAYS_END_PROBATION,

        //Function: số ngày hieu luc tinh phu cap phat sinh
        //Created By: hien.pham
        //Created Date: 20130207
        DAYS_OFFEFFECT_COUNT,

        //Function:So ngay nghi phep nam
        //Created By: Lam.Le
        //Created Date: 23/05/2011
        ANNUALLEAVE_COUNT,
        //Function:Phep nam con lai cho den thang
        //Created By: Lam.Le
        //Created Date: 17/09/2012
        ANNUAL_AVAILABLE_COUNT,
        //Function:So ngay nghi khong luong 
        //Created By: Lam.Le
        //Created Date: 23/05/2011
        UNPAIDLEAVE_COUNT,
        //Function: Ngay di lam duoc tra luong
        //Created By: LamLe 
        //Created Date: 28/11/2011 
        WORKINGPAIDDAY,
        //Function:So ngay nghi có luong
        //Created By: Tran.Pham
        //Created Date: 3/06/2011
        PAIDLEAVE_COUNT,
        //Function:Ngày làm ca đêm
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        NIGHTDAY_COUNT,
        //Function: Số ngày đi trễ
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        //LATE_COUNT, // SonNgo unuse

        //Function: So ngay nghi khong luong va tre som
        //Created By: Lam.Le
        //Created Date: 23/05/2011
        UNPAIDLEAVE_LATEEARLY_DAY,

        //Function: Số ngày đi trễ || về sớm
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        LATE_EARLY_COUNT,
        //Function: Số ngày đi trễ || về sớm
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        LATE_EARLY_HOURS,
        //Function: Lay tat ca nhung ngay khong quet the.
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        LEAVEDAY_COUNT,
        //Function: Vao trong thang
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        NEWCOME,
        //Function: Nghi trong thang
        //Created By: Tran.Pham
        //Created Date: 08/03/2011            
        TERMINATION,

        //Function: Ma phong ban
        //Created By: LamLe 
        //Created Date: 28/11/2011 
        CODE_BRANCH,
        //Function: Dang thu viec
        //Created By: LamLe 
        //Created Date: 28/11/2011 
        PROBATION,
        //Function: Dang thu viec
        //Created By: LamLe 
        //Created Date: 28/11/2011 
        IS_DATEHIRE_BF_15,
        //Function: Kiểm tra tháng tính lương có hết hạn thử việc hay không
        //Created By: SonNgo 
        //Created Date: 28/08/2012 
        E_END_PROBATION_SALMONTH,

        //Function: Thông tin Mã chi phí
        //Created By: SơnNgo 
        //Created Date: 23/08/2012 
        COSTCENTER_CODE,

        //Function: Kiểm tra thời gian kết thúc thử việc của NV vào ngày mấy của tháng tính lương, vd: END_PROBATION_ON_20
        //Created By: SonNgo 
        //Created Date: 28/11/2011 
        END_PROBATION_ON_,

        //Function: Nghi viec tinh giong BHXH. Dua vao ngay 15
        //Created By: LamLe 
        //Created Date: 28/11/2011             
        INS_TERMINATION,
        //Function: Thu viec tinh giong BHXH. Dua vao ngay 15
        //Created By: LamLe 
        //Created Date: 28/11/2011 
        INS_PROBATION,
        //Function: Dung cho XP, ngay ket thuc thu viec + them 1 roi so sanh lon hon or nho ngay 15 cua thang = > tra ve true or false
        //Created By: LamLe 
        //Created Date: 03/07/2011 
        INS_PROBATION_ADD_1,
        //Function: số lần tăng ca đầu giờ (được approve) và >=1.5 giờ
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        FGL_EARLY_OVERTIME_COUNT,
        //Function: số ngày thường làm việc sau 19h30
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        WEEKDAY_WORK_LATE_AFTER_19h30_COUNT,

        //Function: số ngày nghỉ full-shift bị tính trong trường hợp tính pc chuyên cần của FGL
        //- khám thai: CHK
        //- phép năm: ANL
        //- duong suc: HEA
        //- nghi con nho:NBC
        //- cong ty cho nghi: 
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        FGL_DILIGENT_LEAVEDAY_COUNT,

        //Function: số ngày đi làm > 4hrs trong ngày chủ nhật và ngày lễ
        //- Chủ nhật: 
        //- Ngày lễ: 
        //Created By: Lam.LE
        //Created Date: 08/03/2011
        FGL_SUNDAY_HOLIDAY_WORK_GREATER_4HOURS_COUNT,

        //Function: Lay gio tang ca ngay cuoi tuan dayoff va Holiday
        //- Chủ nhật: 
        //- Ngày lễ: 
        //Created By: Lam.LE
        //Created Date: 08/03/2011
        OT_DAYOFF_HOLIDAY_WORK_GREATER_6HOURS_COUNT,

        //Function: số giờ đi làm trong ngày chủ nhật và ngày lễ
        //- Chủ nhật: 
        //- Ngày lễ: 
        //Created By: Tran.Pham
        //Created Date: 23/06/2011
        FGL_SUNDAY_HOLIDAY_WORK_HOURS,

        //Function:Ngày công tinh luong cua TAISUN = PaidWorkDayCount(ngay cua ctrinh) + ngay nghi phep tinh luong
        //Created By: Lam.Le
        //Created Date: 23/05/2011
        TAISUN_PAIDWORKDAY_COUNT,

        //Function: Tính số giờ OT quá 8h ngày chủ nhật
        //Create by: Khang. Huynh
        OT_SUNDAY_HOLIDAY_GREATER_8H,
        //Số giờ làm thêm ca đem
        //Create by: Hien.Pham
        OT_NIGHTSHIFT_HOURS,
        //Function: Tính số ngày OT > 4h/ ngày
        //Create by: SonNgo
        OT_UPPER_4H,

        //Function: Tính số ngày OT > 4h/ ngày
        //Create by: SonNgo
        OT_UPPER_8H,
        //Function: Tính số giờ OT quá 8h ngày thứ 7 nghỉ bù
        //Create by: Khang. Huynh
        OT_SATURDAY_GREATER_8H,
        //Function: Tính số lượng tuần đc tính phụ cấp chuyên cần
        //Create by: Khang. Huynh
        WEEKLY_ATTENDANCE_ALLOWANCE,
        //Function: Tính tháng thâm niên
        //Created By: Son.Ngo
        //Created Date: 23/05/2011
        SENIORITY_MONTHS,

        //Function: Tính năm thâm niên từ ngày 01/01 hàng năm
        // ex: Vao cong ty thang 10/2011 den 01/01/2012 tham nien = 0 nam
        //                               den 01/01/2013 tham nien = 1 nam
        //Created By: Khang.Huynh
        //Created Date: 
        SENIORITY_YEAR_FUJIKURA,

        //Function: Ngay vao lam
        //Created Date: 
        SENIORITY_YEAR_FUJIKURA_BY_DATEHIRE,
        //Function:So ngay có In/Out
        //Created By: Trân.Pham
        //Created Date: 26/05/2011
        //SENIORITY_YEAR, // SonNgo unuse
        //Function:So ngay có In/Out
        //Created By: Trân.Pham
        //Created Date: 26/05/2011
        INOUTDAY_COUNT,

        //Function:So ngay có In/Out loai tru ngay nghi le va cuoi tuan
        //Created By: Lam.Le
        //Created Date: 02/06/2011
        INOUTDAY_GREATER_4HOURS_EXCLUDE_HOLIDAYANDWEEKEND_COUNT,
        //Function:So ngay có In/Out loai tru ngay nghi le va cuoi tuan, lon hon or bang 4h. Pataya - DucHo
        //Created By: Lam.Le
        //Created Date: 10/06/2013
        INOUTDAY_GREATER_EQUAL_4HOURS_EXCLUDE_HOLIDAYANDWEEKEND_COUNT,
        //Function: So ngay có In/Out >=8h loai tru ngay nghi le va cuoi tuan
        //Created By: SonNgo
        //Created Date: 10/12/2011
        INOUTDAY_UPPER_8HOURS_EXCLUDE_HOLIDAYANDWEEKEND_COUNT,


        //Function: Đếm số ngày quét thẻ >= 8h/ngày
        //Created By: SonNgo
        //Created Date: 15/07/2012
        WORKHOURS_UPPER_8HOURS,

        //Function: So ngay có In/Out >=8h loai tru ngay nghi le va cuoi tuan (D/v Cong nhan co tham nien >= 2 nam)
        //Created By: SonNgo
        //Created Date: 10/12/2011
        INOUTDAY_UPPER_8HOURS_AND_SENIORITY_UPPER_2YEARS_EXCLUDE_HOLIDAYANDWEEKEND_COUNT,

        //Function:số lượng con nhỏ valid trong tháng tính lương
        //Created By: Son.Ngo
        //Created Date: 15/06/2011
        NEW_BORN_CHILD_COUNT,

        //Function: Tinh so tien BHXH
        //Created By: Lam.Le
        //Created Date: 12/08/2011
        AMOUNT_INSURANCE,

        //Function: Dem so ngay lam ca có mã số phía sau dấu "_"
        //Created By: Tran.Pham
        //Created Date: 12/08/2011
        SHIFT_COUNT_,//Code1_Code2_

        //Function: Dem so ngay lam ca tang ca có mã số phía sau dấu "_" >= 8
        //Created By: Hien.Pham
        //Created Date: 2013/10/21 
        OVERTIME_SHIFT_HOURS_,

        //Function: Dem so ngay lam ca có mã số phía sau dấu "_" >= 4
        //Created By: Hien.Pham
        //Created Date: 2013/10/21 great
        SHIFT4HOURS_COUNT_,//Code1_Code2_

        //Function: Dem so ngay lam ca tang ca có mã số phía sau dấu "_" >= 8
        //Created By: Hien.Pham
        //Created Date: 2013/10/21 
        OVERTIME_SHIFT_COUNT_,

        //Function: Dem so ngay lam ca tang ca có mã số phía sau dấu "_" >= 8
        //Created By: Hien.Pham
        //Created Date: 2013/10/21 
        OVERTIME4HOURS_SHIFT_COUNT_,

        //Function: Dem so ngay lam ca tang ca có mã số phía sau dấu "_" >= 8
        //Created By: Hien.Pham
        //Created Date: 2013/10/21 
        OVERTIME8HOURS_SHIFT_COUNT_,

        //Function: Dem so lan nghi trong thang có mã Code phía sau dấu "_"
        //Created By: SonNgo //LamLe - 20121008 - Chi dem so lan nghi 
        //Created Date: 03/01/2012
        LEAVEDAY_COUNT_,//Code1_Code2_

        //Function: Dem so ngay nghi trong thang có mã Code phía sau dấu "_"
        //Created By: LamLe
        //Created Date: 03/01/2012
        LEAVE_DAY_,//Code1_Code2_

        //Function: Dem so ngay nghi trong thang có mã Code phía sau dấu "_", trong khoản thời gian từ ngày kết thúc thử việc -> cuối tháng
        //Created By: SonNgo
        //Created Date: 20120627
        LEAVEDAY_END_PROBATION_COUNT_,//Code1_Code2_

        //Function: Tính Phụ cấp trừ cho tháng này từ Ngày nghỉ không lương của tháng trước
        //Created By: Son.Ngo
        //Created Date: 13/08/2011
        //UNPAID_LEAVEDAY_PREVMONTH, // SonNgo unuse

        //Function: Tính Phụ cấp cộng cho tháng này từ OT và nghỉ hưởng lương của tháng trước
        //Created By: Son.Ngo
        //Created Date: 20/08/2011
        //PAID_OT_PREVMONTH, // SonNgo unuse

        //Function: Tính lương thời gian từ ngày nghỉ đến cuối tháng để trừ lại Tổng lương - Áp dụng cho CPG
        //Created By: Son.Ngo
        //Created Date: 20/08/2011
        //SUBTRACT_ALLOWANCE_QUIT_EMPLOYEE_CPG, // SonNgo don't use

        //Function: Lương giờ
        //Created By: Son.Ngo
        //Created Date: 22/08/2011
        HOURLY_RATE,
        //Function: Lương giờ 1
        //Created By: Son.Ngo
        //Created Date: 22/08/2011
        HOURLY_RATE1,
        //Function: Có hưởng được phụ cấp tuần hay ko?
        //Created By: Hien.Pham
        //Created Date: 22/08/2011
        RECEIVE_WEEK_ATTENDANCE_FJK,
        //Function: Có hưởng được phụ cấp Tháng hay ko?
        //Created By: Hien.Pham
        //Created Date: 22/08/2011
        RECEIVE_MONTH_ATTENDANCE_FJK,
        //Function: Lương giờ sau dieu chinh
        //Created By: Son.Ngo
        //Created Date: 22/08/2011
        HOURLY_RATE2,
        //Function: số ca 3 làm trong tháng ban ngay
        //Created By: hien.pham 
        //Created Date: 10/01/2012
        COUNT_SHIFTA_N,
        //Function: số ca 3 làm trong tháng ban dem
        //Created By: hien.pham 
        //Created Date: 10/01/2012
        COUNT_SHIFTA_D,
        //Function: số ca 3 làm trong tháng ban dem
        //Created By: hien.pham 
        //Created Date: 10/01/2012
        COUNT_SHIFT_3,
        //Function: số tuần trong tháng
        //Created By: hien.pham 
        //Created Date: 10/01/2012
        COUNT_WEEK_IN_MONTH,

        //Function: Phụ cấp tiếng nhật. 
        //Created By: lam.le    
        //Created Date: 11/07/2012
        JAPANESE_COUNT,

        //Function: Kiem tra co ma so thue hay ko. 
        //Created By: lam.le    
        //Created Date: 11/07/2012
        CHECK_CODETAX,

        //Function: Kiem tra loại nhân viên. 
        //Created By: hien.pham   EmployeeType
        //Created Date: 07/03/2013
        EMPLOYEE_TYPE,

        //Function: Tổng số ngày nghỉ việc trong tháng tính lương
        //Created By: son.ngo    
        //Created Date: 28/07/2012
        TOTAL_DAYS_QUIT_COMPANY,

        //Function: Số ngày chưa vào công ty trong tháng tính lương
        //Created By: son.ngo    
        //Created Date: 28/07/2012
        TOTAL_DAYS_NOT_HIRE_COMPANY,

        //Function: Ngày nghỉ việc <= ngày kết thúc thử việc
        //Created By: son.ngo    
        //Created Date: 02/08/2012
        E_UNPASS_PROBATION,

        //Function: Nghỉ việc nhưng không kịp trả thẻ BHYT trong tháng
        //Created By: son.ngo    
        //Created Date: 31/08/2012
        E_NOT_RECIVIED_INSUBOOK_IN_TERMINATE_MONTH,

        //Function: Sinh nhật trong tháng
        //Created By: son.ngo    
        //Created Date: 04/09/2012
        E_BIRTHDAY,

        //Function: Ngày quốc tế phụ nữ (chỉ áp dụng cho Nữ)
        //Created By: son.ngo    
        //Created Date: 04/09/2012
        E_International_Women_Day,

        //Function: Kiểm tra trong tháng có bị kỷ luật
        //Created By: son.ngo    
        //Created Date: 08/09/2012
        E_DISCIPLINE,

        #endregion

        #region Salary
        /// <summary>
        /// Ngay vao cong ty
        /// </summary>
        DAY_HIREDATE,
        /// <summary>
        /// Luong dong BHXH
        /// </summary>
        INSURANCE_SALARY,
        /// <summary>
        /// Luong dong BHXH
        /// </summary>
        INSURANCE_SALARY_VND,
        /// <summary>
        /// Luong co ban
        /// </summary>
        BASIC_SALARY,
        /// <summary>
        /// Luong co ban
        /// </summary>
        BASIC_SALARY_VND,
        /// <summary>
        /// Luong tinh PIT
        /// </summary>
        SALARY_WITH_PIT,
        /// <summary>
        /// luong tinh PIT cua Thoi vu khong tru giam tru gia canh
        /// </summary>
        SALARY_PIT_CASUAL,
        /// <summary>
        /// money giam tru gia canh + Nguoi phu thuoc
        /// </summary>
        DEPENDANT_AMOUNT,
        #endregion

        #region Công Thức Phép Năm
        //Phep nam
        //Function:Tổng số ngày phép năm/phep benh/phep om
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        TOTAL,
        //Ngay phep duoc cong them trong nam. Cau hinh trong Att_AnnualLeave
        //Function: Att_AnnualLeave.AdditionalValue
        //Created By: Lam.Le
        //Created Date: 10/06/2011
        ADDITIONAL_ANNUAL,
        //Function: ngày kế thúc thử việc
        //Created By: Lam.Le
        //Created Date: 08/03/2011
        MONTHENDPRO,
        //Function:Tháng bắt dầu vào làm
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        MONTHSTART,

        //Function: Tháng bắt đầu vào làm 
        //          Ngay 1-5 => 1     6-25 => 0.5   26-31=>0
        //Created By: Trung.Le
        //Created Date: 08/05/2011
        MONTHSTART2,
        //Function: Tháng kết thúc thử việc
        //          Ngay 1-5 => 1     6-25 => 0.5   26-31=>0
        //Created By: Trung.Le
        //Created Date: 26/05/2012
        MONTHSTART_PROB,
        MONTHEND,
        //Function: Tháng nghỉ việc
        //Created By: Tran.Pham
        //Created Date: 08/03/2011
        CURRENTMONTH,
        //Function: Số ngày phép được hương dựa trên thăm niên làm việc
        //Created By: Hien.Pham
        //Created Date: 28/06/2011 
        //Function:Tháng vào làm
        //Created By: hien.pham
        //Created Date: 15/01/2013
        MONTHHIRE,

        //Function:Năm import
        //Created By: hien.pham
        //Created Date: 15/01/2013
        CURRENTYEAR,



        SENIOR_BONUS_LEAVE,

        //Function: Số ngày phép được hương dựa trên thâm niên làm việc (với tuỳ biến tháng áp dụng quy tắc tính phép năm)
        //Created By: SơnNgô
        //Created Date: 07/05/2012
        SENIOR_BONUS_LEAVE_FROM_,
        #endregion

        #region Ngày công chuẩn
        //Function: Số ngày của tháng trừ ngày CN
        //Created By: Trân.Pham
        //Created Date: 20/07/2011
        DAYS_IN_MONTH_EXCLUDE_SUNDAY,

        //Function: Số ngày của tháng trừ ngày CN VÀ ngày nghỉ
        //Created By: Yen.Ho
        //Created Date: 19/08/2011
        DAYS_IN_MONTH_EXCLUDE_SUNDAY_HOLIDAY,

        //Function: Số lần được nhân phụ cấp breakfast
        //Created By: hien.pham
        //Created Date: 19/02/2012
        OT_BREAKFAST_COUNT,
        //Function: Số ngày lễ trong tháng
        //Created By: tran.pham
        //Created Date: 20/12/2011
        H,
        //Function: Số ngày lễ trong tháng
        //Created By: vi.nguyen
        //Created Date: 20/12/2011
        SAT,

        //Function: Số ngày CN trong tháng
        //Created By: tran.pham
        //Created Date: 20/12/2011
        S,

        /// <summary>
        /// LamLe - 20120611 - #0014210 Số ngày roster trong thang
        /// </summary>
        R,

        /// <summary>
        /// LamLe - 20120611 - #0014210 Số ngày roster trong thang, khong tinh ngay nghi le
        /// </summary>
        R_NOT_H,
        //Function: Số ngày của tháng tính công
        //Created By: tran.pham
        //Created Date: 20/12/2011
        D,
        /// <summary>
        /// Ngày công chuẩn tính theo quy tắt Fujikura
        /// </summary>
        WORKDAY_STANDARD_PER_MONTH,

        /// <summary>
        /// Tổng số ngaày cuủa năm trừ cn & ngay nghỉ
        /// </summary>
        WORKDAY_AVERAGE_PER_MONTH,

        //Function: Dem so ngay lam ca có mã số phía sau dấu "_"
        //Created By: SonNgo
        //Created Date: 14/03/2012
        SHIFT_PRE_MONTH_COUNT_,//Code1_Code2_

        #endregion

        #region Phụ cấp bất thường
        //Phep nam
        //Function:Phụ cấp hoàn thành công việc
        //Created By: hien.pham
        //Created Date: 17/11/2011
        PERFORMANCE_ALL,
        #endregion

        #region Ngày Nghỉ
        /// <summary>
        /// Ngày nghỉ trong tháng
        /// </summary>
        VALUE,

        #endregion

        #region Cấu Hình Công Thức Phép Năm

        /// <summary>
        /// Tháng bắt đầu tính phép nắm Mặc định là tháng 1
        /// </summary>
        CONFIG_ANL_MONTHBEGINYEAR,

        /// <summary>
        /// Ngày bắt đầu để tính tháng Full
        /// Sau ngày này sẽ chuyển tháng bắt đầu qua tháng sau
        /// lấy ngày này để tính phép thâm niên
        /// </summary>
        CONFIG_ANL_DAYBEGIN_FULLMONTH,

        /// <summary>
        /// Số tháng để bắt đầu tính thâm niên
        /// </summary>
        CONFIG_ANL_SENIOR_MONTH,

        /// <summary>
        /// Số ngày trung bình cho 1 tháng mặc đinh 30
        /// </summary>
        CONFIG_ANL_DAY_PER_MONTH,

        /// <summary>
        /// Hệ số để làm tròn lên và làm tròn xuống mặc định 0.5
        /// </summary>
        CONFIG_ANL_ROUND_UP,

        /// <summary>
        /// Loai để bắt đầu tính phép cho 1 nhân viên vd. Ngày DateHire, ngày Hến Hạn thử việc v.v
        /// Value của phần tử này là Enum AnlProfileTypeBegin
        /// </summary>
        CONFIG_ANL_TYPE_PROFILE_BEGIN,

        /// <summary>
        /// Số Ngày để bắt đầu tính Nghỉ phép và tính quy tắc HDT4 HDT5
        /// </summary>
        CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL,


        /// <summary>
        /// số phép năm bình thường có thể nhận 1 năm
        /// </summary>
        CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR,
        /// <summary>
        /// số phép năm Có có thể nhận cho 1 bậc . Mặc định là 1
        /// </summary>
        CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL,
        /// <summary>
        /// số phép năm Có có thể nhận nhiều hơn NORMAL . Mặc định là 2
        /// </summary>
        CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL,
        /// <summary>
        /// số phép năm Có có thể nhận nhiều hơn NORMAL . Mặc định là 4
        /// </summary>
        CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL,
        /// <summary>
        /// Các loại mã ngày nghỉ không được hưởng phép năm
        /// </summary>
        CONFIG_LEAVE_NON_ALN_CODES,
        /// <summary>
        /// thang trong nam de tinh phem nam honda la thang 10 hang nam
        /// </summary>
        CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR,
        /// <summary>
        /// Số tháng được phép làm tròn lên
        /// </summary>
        CONFIG_MONTH_ROUND_UP,

        #endregion

        #region Công Thức Phép Năm 2

        /// <summary>
        /// Số tháng đc tính phép năm bình thường
        /// </summary>
        ANL_NORMAL,

        /// <summary>
        /// Số tháng nghỉ ko được hưởng phép
        /// </summary>
        ANL_LEAVE_NON_HAVEANL,

        /// <summary>
        /// Lây ra tháng Hiện Tại
        /// </summary>
        ANL_CURRENTMONTH,

        /// <summary>
        /// Tính ra số tháng được hưởng thâm niên
        /// </summary>
        ANL_SENIOR,

        /// <summary>
        /// Tính ra nhân viên đó tháng đó có làm công việc nặng nhọc loại 4 hay không ?? Trả về 1 hoặc 0
        /// </summary>
        ANL_WORK_HDT4,


        /// <summary>
        /// Tính ra nhân viên đó tháng đó có làm công việc nặng nhọc loại 5 hay không ?? Trả về 1 hoặc 0
        /// </summary>
        ANL_WORK_HDT5,


        #endregion
    }
    public enum OrgUnit
    {
        #region OrganizationStructureStatus
        /// <summary>Chi nhánh</summary>
        E_BRANCH,
        /// <summary>phòng ban </summary>
        E_DEPARTMENT,
        /// <summary>Tổ</summary>
        E_SECTION,
        /// <summary>Nhóm</summary>
        E_TEAM,

        E_COMPANY,
        E_UNIT,
        E_DIVISION,
        E_GROUP
        #endregion
    }

    public enum MealRecord_Status
    {
        /// <summary>
        /// Duyệt
        /// </summary>
        [Description("Duyệt")]
        E_APPROVED,
        /// <summary>
        /// Yêu Cầu
        /// </summary>
        [Description("Yêu Cầu")]
        E_SUBMIT,
        /// <summary>
        /// CHỜ DUYỆT
        /// </summary>
        [Description("Chờ Duyệt")]
        E_WAITING,
        /// <summary>
        /// TỪ CHỐI
        /// </summary>
        [Description("Từ Chối")]
        E_IGNORE
    }



    public enum TAMScanStatus
    {
        #region FieldInfoSearchType
        E_LOADED   //Load tu may cham cng
      ,
        E_MANUAL //them vao bang tay

            ,
        E_MODIFY //Sua du lieu tu may cham cong (In-Out)

            , E_APPROVE
        #endregion
    }
    public static class EnumDropDown
    {
        public enum Sal_BasicSalaryStatus
        {
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Chờ Duyệt")]
            E_WATTING_APPROVED,
            [Description("Từ Chối")]
            E_REJECTED,
        }
        public enum Kai_KaizenDataStatus
        {
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Chờ Duyệt")]
            E_WATTING_APPROVED,
            [Description("Hủy")]
            E_REJECTED,
        }

        public enum TraineeStatus
        {
            #region TraineeStatus
            [Description("Đang Học")]
            E_STUDYING,
            [Description("Đạt")]
            E_PASSED,
            [Description("Không Đạt")]
            E_FAILED,
            [Description("Tham Gia")]
            E_ATTENDED,
            [Description("Hoàn Thành")]
            E_COMPLETED,
            [Description("Vắng Mặt")]
            E_UN_ATTENDED,
            [Description("Bảo Lưu")]
            E_RESERVES
            #endregion

        }

        public enum TraineeRegisterStatus
        {
            #region TraineeRegisterStatus
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Xác Nhận")]
            E_SUBMIT,
            [Description("Yêu Cầu")]
            E_REQUEST,
            [Description("Từ Chối")]
            E_REJECTED,
            [Description("Hủy")]
            E_CANCEL,
            #endregion
        }

        public enum CourseStatus
        {
            #region CourseStatus
            [Description("Đã Áp Dụng")]
            E_APPLIED,
            [Description("Đang Áp Dụng")]
            E_HAVE_APPLIED,
            [Description("Sẽ Áp Dụng")]
            E_WILL_APPLY,

            #endregion
        }

        // HIeuVan: Dịch giá trị "Description" trong enum
        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.
                       GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((attributes != null) && (attributes.Length > 0))
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch (Exception)
            {
                return value.ToString();
            }


        }

        public enum InsuranceRecordType
        {
            /// <summary>
            /// Bản Thân Ốm Ngắn Ngày
            /// </summary>
            [Description("Bản Thân Ốm Ngắn Ngày")]
            E_SICK_SHORT,

            /// <summary>
            /// Bản Thân Ốm Dài Ngày
            /// </summary>
            [Description("Bản Thân Ốm Dài Ngày")]
            E_SICK_LONG,

            /// <summary>
            /// Con Ốm
            /// </summary>

            [Description("Con Ốm")]
            E_SICK_CHILD,

            /// <summary>
            /// Kham thai
            /// </summary>
            [Description("Khám Thai")]
            E_PREGNANCY_EXAMINE,

            /// <summary>
            /// Sẩy thai, nạo hút thai, thai chết lưu
            /// </summary>
            [Description("Sẩy Thai, Nạo Hút Thai, Thai Chết Lưu")]
            E_PREGNANCY_LOST,

            /// <summary>
            /// Sinh con, nuôi con nuôi
            /// </summary>
            [Description("Sinh Con, Nuôi Con Nuôi")]
            E_PREGNANCY_SUCKLE,

            /// <summary>
            /// Thực hiện các biện pháp tránh thai
            /// </summary>
            [Description("Thực Hiện Các Biện Pháp Tránh Thai")]
            E_PREGNANCY_PREVENTION,

            /// <summary>
            /// bao cao duong suc thai san Restoration
            /// </summary>
            [Description("Báo Cáo Dưỡng Sức Thai Sản Restoration")]
            E_RESTORATION_PREGNANCY,
            /// <summary>
            /// Bao cao duong suc om dau
            /// </summary>
            [Description("Báo Cáo Dưỡng Sức Ốm Đau")]
            E_RESTORATION_SICK,
            /// <summary>
            /// Dưỡng sức sau tai nạn lao động. Bệnh Nghê nghiệp
            /// </summary>
            [Description("Dưỡng Sức Sau Tai Nạn Lao Động. Bệnh Nghề Nghiệp")]
            E_RESTORATION_TNLD

        }

        public enum EmailType
        {
            [Description("Duyệt Tăng Ca Gửi Tới Người Duyệt")]
            E_APPROVED_OVERTIME,
            [Description("Duyệt Tăng Ca Gửi Tới Người Dăng Ký")]
            E_APPROVED_OVERTIME_RETURN,
            [Description("Duyệt Ca Làm Việc Tới Người Duyệt")]
            E_APPROVED_ROSTER,
            [Description("Duyệt Ca Làm Việc Tới Người Dăng Ký")]
            E_APPROVED_ROSTER_RETURN,
            [Description("Duyệt Ngày Nghỉ Tới Người Duyệt")]
            E_APPROVED_LEAVEDAY,
            [Description("Duyệt Ngày Nghỉ Tới Người Dăng Ký")]
            E_APPROVED_LEAVEDAY_RETURN,

            [Description("Duyệt Đơn Hàng")]
            E_APPROVED_PURCHASEREQUEST,
            [Description("Duyệt Điều Động")]
            E_APPROVED_TRAVELREQUEST,
            [Description("Duyệt Tạm Ứng")]
            E_APPROVED_CASHADVANCE,
            [Description("Duyệt Thanh Toán Công Tác")]
            E_APPROVED_CLAIM,

            [Description("Yêu Cầu Điều Động")]
            E_REQUEST_TRAVELREQUEST,
            [Description("Yêu Cầu Tạm Ứng")]
            E_REQUEST_CASHADVANCE,
            [Description("Yêu Cầu Thanh Toán Công Tác")]
            E_REQUEST_CLAIM,
            [Description("Cảnh Báo Hợp Đồng Đến Hạn")]
            E_WARNINGCONTRACT,
            [Description("Phiếu Lương")]
            E_PAYSLIP,
            [Description("Nhắc Nhở Đánh Giá")]
            E_REMIND_EVALUTION,
            [Description("Thông Báo Reset Mật Khẩu")]
            E_NOTIFYRESETPASSWORD,
            [Description("Gửi Email Thiếu Quẹt Thẻ")]
            E_SENDMAILFORGETTAMSCANLOG,
            [Description("Thông Báo Ứng Viên Trúng Tuyển")]
            E_NOTIFYCANDIDATEPASS,
            [Description("Thông Báo Ứng Viên Không Đạt")]
            E_NOTIFYCANDIDATEFAIL,
            [Description("Từ Chối Điều Động")]
            E_REJECTED_TRAVELREQUEST,
            [Description("Từ Chối Tạm Ứng")]
            E_REJECTED_CASHADVANCE,
            [Description("Từ Chối Thanh Toán Công Tác")]
            E_REJECTED_CLAIM,


        }

        public enum EmailTypeDetails
        {
            E_UserName,
            E_ProfileName,
            E_CodeEmp,
            E_DateStart,
            E_DateEnd,
            E_Status,
            E_UserInfoName,
            E_UserLogin,
            E_Password,
            E_TableContent,
            E_Date,
        }

        public enum TravelRequestType
        {
            [Description("Công Tác Trong Nước")]
            FIN_TravelRequest_Domestic,
            [Description("Công Tác Nước Ngoài")]
            FIN_TravelRequest_External,

        }
        public enum ClaimType
        {
            [Description("Thanh Toán Cho Nhân Viên")]
            FIN_Claim_Profile,
            [Description("Thanh Toán Cho Nhà Cung Cấp")]
            FIN_Claim_Manufacture,

        }

        /// <summary>Tham Số sendMail Ưng Viên không đạt</summary>
        public enum EmailCandidateParam
        {
            E_CandidateName,
            E_CodeCandidate,
            E_ReasonFailFilter,
            E_WorkplaceSuggestion,
            E_JobVacancyID,
            E_NoEmailFail,
            E_NoEmailPass
        }

        public enum EmailType_APPROVED_OVERTIME
        {
            E_UserName,
            E_ProfileName,
            E_CodeEmp,
            E_OrgStructureName,
            E_WorkDate,
            E_RegisterHours,
            E_ReasonOT,
            E_Status,
            E_DurationType,
            E_OvertimeTypeName,
            E_ApprovedHours,
            E_ApprovedBy1,
            E_ApprovedBy2,
            E_Payment,
            E_Comment,
            E_IsNightShift,
            E_LinkContent,
        }

        public enum EmailType_APPROVED_ROSTER
        {
            E_UserName,
            E_ProfileName,
            E_CodeEmp,
            E_DateStart,
            E_DateEnd,
            E_Status,
            E_Type,
            E_LinkContent,
        }

        public enum EmailType_APPROVED_LEAVEDAY
        {
            E_UserName,
            E_ProfileName,
            E_CodeEmp,
            E_DateStart,
            E_DateEnd,
            E_Status,
            E_LeaveDayTypeName,
            E_LeaveHours,
            E_LeaveDays,
            E_LinkContent,
        }



        public enum EmailType_WARNINGCONTRACT
        {
            E_PROFILENAME,
            E_CODEEMP,
            E_DATEEXPIRED,
            E_TABLECONTENT,
        }

        public enum EmailType_APPROVED_PURCHASEREQUEST
        {
            E_PROFILENAME,
            E_USERINFONAME,
            E_FUNCTIONNAME,
            E_BUDGETOWNERNAME,
            E_SUPPLIERNAME,
            E_BUDGETCHARGEDIN,
            E_FROMTO,
            E_DESCRIPTION,
            E_TOTAL,
            E_STATUS,
            E_LINKCONTENT,
        }

        public enum EmailType_APPROVED_TRAVELREQUEST
        {
            E_PROFILENAME,
            E_USERINFONAME,

            E_TRAVELREQUESTNAME,
            E_TRAVELREQUESTCODE,
            E_ISBOOKINGHOTEL,
            E_JOBTITLE,
            E_AREA,
            E_SUBJECT,
            E_PAIDAMOUNT,
            E_ACCOUNTCODE,
            E_BALANCE,
            E_CASHADVANCE,
            E_DESCRIPTION,
            E_TOTAL,
            E_STATUS,
            E_DENIES,
            E_LINKCONTENT,
            E_REJECTLINK,
        }

        public enum EmailType_APPROVED_CASHADVANCE
        {
            E_PROFILENAME,
            E_USERINFONAME,
            E_JOBTITLE,
            E_AREA,

            E_CASHADVANCENAME,
            E_CASHADVANCECODE,
            E_TOTALADVANCE,

            E_SUBJECT,
            E_STATUS,
            E_DENIES,
            E_REJECTLINK,
            E_LINKCONTENT,
        }


        public enum EmailType_APPROVED_CLAIM
        {
            E_PROFILENAME,
            E_USERINFONAME,
            E_JOBTITLE,
            E_AREA,

            E_CLAIMNAME,
            E_CLAIMCODE,
            E_TRAVELREQUESTNAME,
            E_ACCOUNTCODE,
            E_TOTAL,
            E_STATUS,
            E_TOTALCLAIM,

            E_REJECTLINK,
            E_DENIES,
            E_LINKCONTENT,
        }

        public enum Sexual
        {
            #region Sexual
            [Description("Nam")]
            E_MALE,
            [Description("Nữ")]
            E_FEMALE
            #endregion
        }

        public enum ExpiredContractType
        {
            [Description("Hằng Ngày")]
            E_DayByDay,
            [Description("Theo Chu Kỳ")]
            E_Loop
        }

        public enum ValueEntityType
        {
            /// <summary>
            /// Muc tran dong BHXH
            /// </summary>
            E_INSURANCE_CAPE_AMOUNT,
            /// <summary>
            /// Ty gia dong BHXH
            /// </summary>
            INSURANCE_USD_TO_VND,
            /// <summary>
            /// Luong toi thieu
            /// </summary>
            E_MINIMUM_SALARY,
        }

        public enum Rank
        {
            /// <summary>
            /// ≥ 1.5 tỷ
            /// </summary>
            [Description("Loại A")]
            A,
            /// <summary>
            /// ≥1 tỷ đến < 1,5 tỷ
            /// </summary>
            [Description("Loại B")]
            B,
            /// <summary>
            /// ≥ 0,5 tỷ đến < 1 tỷ
            /// </summary>
            [Description("Loại C")]
            C,
            /// <summary>
            /// ≤ 0,5 tỷ
            /// </summary>
            [Description("Loại D")]
            D,
            /// <summary>
            /// ≤ 0,5 tỷ
            /// </summary>
            [Description("Loại E")]
            E

        }

        public enum OvertimeDurationType
        {
            #region OvertimeDurationType

            [Description("TC Đầu Giờ")]
            E_OT_EARLY,
            [Description("TC Cuối Giờ")]
            E_OT_LATE,
            [Description("TC Trong Ca")]
            E_OT_SHIFT,
            [Description("Không Giới Hạn")]
            E_OT_UNLIMIT,
            [Description("TC Giờ Nghỉ")]
            E_OT_BREAK

            #endregion
        }
        public enum OverTimeStatus
        {
            [Description("Yêu Cầu")]
            E_SUBMIT,
            [Description("Xác Nhận")]
            E_CONFIRM,
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Từ Chối")]
            E_REJECTED,
            [Description("Hủy")]
            E_CANCEL,
            [Description("Người Duyệt Đầu")]
            E_FIRST_APPROVED,
            [Description("Lưu Tạm")]
            E_SUBMIT_TEMP,
            [Description("Chờ Duyệt")]
            E_WAIT_APPROVED,
            [Description("Tạm")]
            E_TEMP
        }



        public enum StatusApproved
        {
            [Description("Yêu Cầu")]
            E_SUBMIT,
            [Description("Xác Nhận")]
            E_CONFIRM,
            [Description("Duyệt")]
            E_APPROVED
        }

        public enum HourMonthlyWorking
        {
            [Description("Xuất Giờ Ngày Nghỉ")]
            HourLeaveday,
            [Description("Xuất Giờ Làm Việc")]
            HourWokingday,
        }

        public enum GroupByType
        {
            [Description("Theo Giờ Làm Việc")]
            ByWorkHours,
            [Description("Theo Phòng Ban")]
            ByOrgStructure,
        }

        public enum ProjectType
        {

            BODY,
            DEO_MEN,
            DEO_WOMEN,
            NFM,
            VISAGE,
            SHAMPOO,
            SHOWER_GEL
        }

        public enum CateCodeType
        {

            BODY,
            CREAM,
            DEO_MEN,
            DEO_WOMEN,
            NFM,
            LIP,
            SUN,
            VISAGE,
            SHAMPOO,
            SHOWER_GEL,
            EUCERIN
        }
        public enum OwnerType
        {

            FE,
            BO,
            CHANNEL
        }

        public enum KaiZenCategoryType
        {
            [Description("Cải Tiến")]
            E_Kaizen,
            [Description("Phát Hiện")]
            E_Dawn
        }

        public enum StatusEmployee
        {
            #region Tat ca nhan vien
            /// <summary>Tat ca nhan vien</summary>
            [Description("Tất Cả")]
            E_ALL,
            #endregion
            #region Nhan vien nghi viec
            /// <summary> Nhan vien nghi viec</summary>
            [Description("Nhân Viên Nghỉ Việc")]
            E_STOPWORKING,
            #endregion
            #region Nhan vien moi vao
            /// <summary> Nhan vien moi vao</summary>
            [Description("Nhân Viên Mới Vào")]
            E_NEWEMPLOYEE,
            #endregion
            #region Nhan vien dang lam viec
            /// <summary> Nhan vien dang lam viec</summary>
            [Description("Nhân Viên Đang Làm Việc")]
            E_WORKING,
            #endregion
            #region Nhan vien moi vao va dang lam viec
            /// <summary> Nhan vien moi vao va dang lam viec</summary>
            [Description("Nhân Viên Mới Vào Và Đang Làm Việc")]
            E_WORKINGANDNEW
            #endregion
        }

        public enum TypeLoadData
        {
            /// <summary>
            /// Lay theo mac dinh theo thu tu in - out
            /// </summary>
            [Description("Mặc định")]
            E_DEFAULT,
            /// <summary>
            /// Lay du lieu in/out theo bien do max min
            /// </summary>
            [Description("Giờ Đầu In Giờ Cuối Out (Áp Dụng với Ca Ngày)")]
            E_MAXMIN,
            /// <summary>
            /// Lay du lieu da co san type in - out theo tung cap
            /// </summary>
            [Description("Lấy In/Out Đã Có gán Theo Từng cặp")]
            E_TYPEINOUT,
            /// <summary>
            /// Lay du lieu da co san type in - out chi lay du lieu dau va du lieu cuoi
            /// </summary>
            [Description("Lấy In/Out Đã Có Theo In Đầu Out Cuối")]
            E_TYPEINOUT_MAXMIN,
            /// <summary>
            /// Lay du lieu dau cuoi in - out trong ca ngay
            /// </summary>
            [Description("Lấy In/Out Theo Biên Độ Max-Min (Dựa Vào Ca Làm Việc)")]
            E_CHECKDAYSHIFT,
            /// <summary>
            /// Lấy ca tự động theo In đầu Out cuối
            /// </summary>
            [Description("Lấy ca tự động theo In đầu Out cuối")]
            E_AUTOSHIFT_MAXMIN
        }
        public enum MissTAM_LeaveType
        {
            [Description("Nghỉ Không Lương")]
            E_OFF_NOT_SALARY = 1,
            [Description("Nghỉ Không Phép")]
            E_OFF_NOT_ALLOW = 2,
            [Description("Trừ Muộn Sớm")]
            E_OFF_LESS = 3
        }
        public enum ConfigNightShift
        {
            //Lay ca dem trong ca
            [Description("Lấy Từ Ca")]
            E_SHIFT,
            //Lay ca dem trong config
            [Description("Lấy Từ Config")]
            E_CONFIG
        }
        public enum Languague
        {
            [Description("Tiếng Việt")]
            VN,
            [Description("English")]
            EN
        }

        public enum ETheme
        {
            [Description("/Content/kendo/2014.1.415/kendo.metrovnr.min.css")]
            VNR_Metro,

            [Description("/Content/kendo/2014.1.415/kendo.metrovnr2.min.css")]
            VNR_Metro2,

            [Description("/Content/kendo/2014.1.415/kendo.bootstrapvnr.min.css")]
            VNR_Bootstrap,

            [Description("/Content/kendo/2014.1.415/kendo.bootstrapvnr2.min.css")]
            VNR_Bootstrap2,

            [Description("/Content/kendo/2014.1.415/kendo.defaultvnr.min.css")]
            VNR_Default,

            [Description("/Content/kendo/2014.1.415/kendo.defaultvnr2.min.css")]
            VNR_Default2,

            [Description("/Content/kendo/2014.1.415/kendo.defaultvnr3.min.css")]
            VNR_Default3,

            [Description("/Content/kendo/2014.1.415/kendo.metro.min.css")]
            Metro,

            [Description("/Content/kendo/2014.1.415/kendo.bootstrap.min.css")]
            Bootstrap,

            [Description("/Content/kendo/2014.1.415/kendo.default.min.css")]
            DefaultKendo,

            [Description("/Content/kendo/2014.1.415/kendo.moonlight.min.css")]
            Moonlight,


            [Description("/Content/kendo/2014.1.415/kendo.blueopal.min.css")]
            Blueopal,
            [Description("/Content/kendo/2014.1.415/kendo.silver.min.css")]
            Silver,
            [Description("/Content/kendo/2014.1.415/kendo.uniform.min.css")]
            Uniform,
            [Description("/Content/kendo/2014.1.415/kendo.metroblack.min.css")]
            Metroblack,
            [Description("/Content/kendo/2014.1.415/kendo.flat.min.css")]
            Highcontrast
        }

        public enum PregnancyType
        {
            [Description("Đi Trể / Về Sớm")]
            E_LEAVE_EARLY,
            [Description("Hưởng Trợ Cấp Con Nhỏ")]
            E_NEW_BORN_CHILD
        }
        public enum LaundryRecord_Status
        {
            /// <summary>
            /// Yêu Cầu
            /// </summary>
            [Description("Yêu Cầu")]
            E_SUBMIT,
            /// <summary>
            /// Duyệt
            /// </summary>
            [Description("Duyệt")]
            E_APPROVED,
            /// <summary>
            /// Chờ Duyệt
            /// </summary>
            [Description("Chờ Duyệt")]
            E_WAIT_APPROVED
        }

        public enum EntityType
        {
            #region EntityNameType
            [Description("Trình Độ Chuyên Môn")]
            E_QUALIFICATION,
            [Description("Hôn Nhân")]
            E_MARRIAGE_CONDITION,
            [Description("Trình Độ Ngôn Ngữ")]
            E_LANGUAGE_TYPE,
            [Description("Kỹ Năng Ngoại Ngữ")]
            E_LANGUAGE_SKILL,
            [Description("Cấp Bậc Ngoại Ngữ")]
            E_LANGUAGE_LEVEL,
            [Description("Loại Tin Học")]
            E_COMPUTING_TYPE,
            [Description("Kỹ Năng Tin Học")]
            E_COMPUTING_SKILL,
            [Description("Cấp Bậc Tin Học")]
            E_COMPUTING_LEVEL,
            [Description("E_ORGANIZATIONSTRUCTURE_TYPE")]
            E_ORGANIZATIONSTRUCTURE_TYPE,
            [Description("Loại Hợp Đồng")]
            E_CONTRACT_TYPE,
            [Description("Loại Người Phụ Thuộc")]
            E_DEPENDANT_TYPE,
            [Description("E_BANK")]
            E_BANK,
            [Description("Loại Chuyên Ngành")]
            E_QUALIFICATION_TYPE,
            [Description("Cấp Độ Chuyên Ngành")]
            E_QUALIFICATION_LEVEL,
            [Description("Lý Do Thay Thế")]
            E_REASONSFORREPLACEMENT,
            [Description("Loại Thay Thế")]
            E_RELATIVE_TYPE,
            [Description("Danh Mục Đánh Giá")]
            E_CATEGORY_KPI,
            //, E_RESIGNATION_REASON_TYPE
            [Description("Nguồn Tuyển")]
            E_SOURCE_ADS_TYPE,
            [Description("E_RESIGN_REASON")]
            E_RESIGN_REASON,
            [Description("E_EMPLOYEE_TYPE")]
            E_EMPLOYEE_TYPE,
            [Description("Trình Độ Học Vấn")]
            E_EDUCATION_LEVEL,
            [Description("Loại Hình Công Việc")]
            E_FORMJOB,
            [Description("Kinh Nghiệm")]
            E_EXPRIENCE,
            [Description("Lý Do Từ Chối Ứng Viên")]
            E_REASONDENYCV,
            [Description("Bệnh Lý")]
            E_SICK_REC,
            [Description("Loại Kỳ Công")]
            E_CUTOFFDURATION_TYPE,
            [Description("Rank")]
            E_RANK,
            [Description("Lý Do Kỷ Luật")]
            E_DISCIPLINE_REASON,
            [Description("Loại Điều Chuyển")]
            E_TYPEOFTRANSFER,
            [Description("Phương Tiện Đi Làm")]
            E_VEHICLE,
            [Description("nguồn Chí Phí")]
            E_COSTSOURCE,
            [Description("Lần Phân Tích Giữ Lương")]
            E_CountAnalyzeHoldSalary,
            #endregion
        }

        public enum Status
        {
            [Description("Yêu Cầu")]
            E_SUBMIT,
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Chờ Phê Duyệt")]
            E_WAITING,
            [Description("Từ Chối")]
            E_REJECTED
        }
        public enum SalaryType
        {
            [Description("Lương Cơ Bản")]
            E_BASICSALARY,
            [Description("LCB Không Thấp Hơn Lương Tối Thiểu")]
            E_BASICSALARY_NO_LESS_MINIMUM_SALARY,
            [Description("Lương Đóng BHXH")]
            E_SALARY_INSURANCE,
            [Description("Lương Có Phụ Cấp")]
            E_GROSSSALARY,
            [Description("Lương Tăng Ca")]
            E_BASICSALARY_OVERTIME,
            [Description("Lương Ngày Nghỉ")]
            E_BASICSALARY_LEAVEDAY,
            [Description("Lương Thời Gian")]
            E_BASICSALARY_TIME,

        }
        public enum InsuranceType
        {
            [Description("Bản Thân Ốm Ngắn Ngày")]
            E_SICK_SHORT,
            [Description("Bản Thân Ốm Dài Ngày")]
            E_SICK_LONG,
            [Description("Con Ốm")]
            E_SICK_CHILD,
            [Description("Khám Thai")]
            E_PREGNANCY_EXAMINE,
            [Description("Sẩy Thai, Nạo Hút Thai, Thai Chết Lưu")]
            E_PREGNANCY_LOST,
            [Description("Sinh Con, Nuôi Con Nuôi")]
            E_PREGNANCY_SUCKLE,
            [Description("Thực Hiện Các Biện Pháp Tránh Thai")]
            E_PREGNANCY_PREVENTION,
            [Description("Dưỡng Sức Thai Sản")]
            E_RESTORATION_PREGNANCY,
            [Description("Dưỡng Sức Ốm Đau")]
            E_RESTORATION_SICK,
            [Description("Tai nạn lao động")]
            E_ACCIDENT_LEAVE,
            [Description("Nghỉ tạm hoãn (Đi bộ đội…)")]
            E_POSTPONEMENT
        }
        /// <summary>
        /// [Tam.Le] - 15.8.2014 - enum cho man hinh Sys_AllSetting
        /// </summary>
        public enum Module
        {
            [Description("Công")]
            E_ATT,
            [Description("Nhân Sự")]
            E_HR,
            [Description("Danh Mục")]
            E_CAT,
            [Description("Hệ Thống")]
            E_SYS,
            [Description("Nhà Ăn")]
            E_CAN,
            [Description("Giặt Là")]
            E_LAU
        }


        public enum TamScanLogSrcType
        {
            [Description("Từ Máy")]
            E_LOADED,
            [Description("Bằng Tay")]
            E_MANUAL,
            //[Description("Sửa Dữ Liệu Từ Máy")]
            //E_MODIFY
        }

        public enum ShiftBreakType
        {
            [Description("Linh Hoạt")]
            E_FLEXIBLE,
            [Description("Cố Định")]
            E_FIX
        }

        // loại giờ công cho bàng công tháng
        public enum WorkHourType
        {
            [Description("Giờ Công Tính Lương")]
            E_PAIDHOUR,
            [Description("Giờ Công Thực Tế")]
            E_REALHOUR
        }

        public enum ShiftItemDurationType
        {
            [Description("Sớm")]
            E_EARLY,
            [Description("Muộn")]
            E_LATE
        }
        public enum TamScanLogType
        {
            [Description("Vào")]
            E_IN,
            [Description("Ra")]
            E_OUT
        }

        public enum Type
        {
            [Description("TKB Hằng Ngày")]
            E_EVERYDAY,
            [Description("Nghỉ Theo Phân Công")]
            E_DIVISION,
            [Description("Đổi TKB")]
            E_CHANGE,
            [Description("Bất Thường")]
            E_ABNORMAL,
            [Description("Nhóm Ca Làm Việc")]
            E_GROUPROSTER
        }

        public enum RewardType
        {
            [Description("Loại dành cho Nữ")]
            E_TYPEFORFEMALE,
            [Description("Loại dành cho Nam")]
            E_TYPEFORMALE
        }
        public enum WorkPermitStatus
        {
            [Description("Không Cần")]
            E_NOTREQUIRED,
            [Description("Đã Cấp")]
            E_GRANTED,
            [Description("Chờ")]
            E_WAIT
        }
        public enum Gender
        {
            [Description("Nữ")]
            E_FEMALE,
            [Description("Nam")]
            E_MALE,
            [Description("Khác")]
            E_OTHER
        }

        public enum TravelRequestMean
        {
            [Description("Xe")]
            E_CAR,
            [Description("Máy Bay")]
            E_PLAN,
            [Description("Khác")]
            E_OTHER
        }
        public enum TravelRequestPurpose
        {
            [Description("Meeting")]
            E_MEETING,
            [Description("Travelling")]
            E_TRAVELLY,
            [Description("Others (Please Specific)")]
            E_OTHER
        }
        public enum Transfer
        {
            [Description("Chuyển Khoản")]
            E_TRANSFER,
            [Description("Tiền Mặt")]
            E_CASH,
            [Description("Tất Cả")]
            E_OTHER,
        }

        public enum StopWorkType
        {
            [Description("Thôi Việc")]
            E_STOP,
            [Description("Tạm Hoãn")]
            E_SUSPENSE
        }

        public enum TypeOfStop
        {
            [Description("Nghỉ Việc")]
            E_STOPWORK,
            [Description("Bỏ Việc")]
            E_QUITWORK,
            [Description("Hết Hạn HĐ")]
            E_EXPIRECONTRACT,
            [Description("Sa Thải")]
            E_LAYOFF,
            [Description("Chấm Dứt HĐ")]
            E_STOPCONTRACT,
            [Description("Tạm Hoãn Thực Hiện HĐ")]
            E_SUSPENSECONTRACT,
            [Description("Chết")]
            E_DEAD,
            [Description("Về Hưu")]
            E_RETIRE,
            [Description("Khác")]
            E_OTHER,
        }

        public enum TypeSuspense
        {
            [Description("Đi Bộ Đội")]
            E_MILITARY,
            [Description("Tạm Giam")]
            E_DETENTION,
            [Description("Thỏa Thuận")]
            E_AGREEMENT,
            [Description("Khác")]
            E_OTHER
        }

        public enum StopWorkStatus
        {
            [Description("Chờ Duyệt")]
            E_WAITAPPROVE,
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Hủy")]
            E_CANCEL,
        }

        public enum ContractEvaType
        {
            [Description("Đánh Giá Hợp Đồng Đến Hạn")]
            E_EXPIRED_APPRENTICE,
            [Description("Đánh Giá Hàng Năm")]
            E_ANNUAL_EVALUATION,

        }

        public enum EvaResultContract
        {
            [Description("Loại A")]
            A,
            [Description("Loại B")]
            B,
            [Description("Loại C")]
            C,
            [Description("Loại D")]
            D

        }
        public enum ResultContract
        {
            [Description("CUT")]
            CUT,
            [Description("OUT")]
            OUT,
            [Description("PASS")]
            PASS

        }

        public enum TypeOfPass
        {
            [Description("Ký Tiếp Hợp Đồng")]
            E_SIGNED_NEXTCONTRACT,
            [Description("Ký Tiếp Phụ Lục")]
            E_SIGNED_APPENDIXCONTRACT


        }

        public enum FormType
        {
            [Description("Nhân Viên/Công Nhân Chính Thức")]
            Official,
            [Description("Thời Vụ")]
            Seasonal,
        }

        public enum JRType
        {
            JR1,
            JR2,
            JR3,
            JR4,
        }

        public enum LaborType
        {
            #region LaborTypeStatus
            [Description("Trực Tiếp")]
            E_DIRECT,
            [Description("Gián Tiếp")]
            E_INDIRECT
            #endregion
        }

        public enum PoliticalTheory
        {
            [Description("Sơ Cấp")]
            E_PRIMARY,
            [Description("Trung Cấp")]
            E_INTERMEDIATE,
            [Description("Cao Cấp")]
            E_SENIOR,
            [Description("None")]
            E_NONE,
        }

        public enum MPRank
        {
            [Description("Quốc Hội")]
            E_CONGRESS,
            [Description("Tỉnh Thành")]
            E_PROVINCE,
            [Description("Quận Huyện")]
            E_DISTRICT,
            [Description("Phường Xã")]
            E_WARD,
        }

        public enum LoopType
        {
            #region LoopType
            [Description("Ngày")]
            E_DAY,
            [Description("Tuần")]
            E_WEEK,
            [Description("Tháng")]
            E_MONTH,
            [Description("Năm")]
            E_YEAR
            #endregion
        }
        public enum StatusHealth
        {
            [Description("Đạt")]
            E_ACCEPT = 1,
            [Description("Không Đạt")]
            E_NOTACCEPT
        }

        public enum DaysOFWeeks
        {
            #region WeekDays

            [Description("Thứ 2")]
            E_MONDAY,
            [Description("Thứ 3")]
            E_TUESDAY,
            [Description("Thứ 4")]
            E_WEDNESDAY,
            [Description("Thứ 5")]
            E_THURSDAY,
            [Description("Thứ 6")]
            E_FRIDAY,
            [Description("Thứ 7")]
            E_SATURDAY,
            [Description("Chủ Nhật")]
            E_SUNDAY

            #endregion
        }

        public enum GenderType
        {
            [Description("Nữ")]
            E_FEMALE,
            [Description("Nam")]
            E_MALE

        }

        public enum CompareType
        {
            [Description("Nhân Viên")]
            E_PROFILE,
            [Description("Phòng Ban")]
            E_ORGSTRUCTURE
        }
        public enum ShowDataType
        {
            [Description("Dạng Cột")]
            E_COLUMN,
            [Description("Dạng Dòng")]
            E_ROW
        }
        public enum AccidentType
        {
            [Description("Tai Nạn Lao Động")]
            E_LABOR,
            [Description("Tai Nạn Khác")]
            E_NONLABOR

        }
        public enum Condition
        {
            [Description("<")]
            E_LESSTHAN,
            [Description("=")]
            E_EQUALS,
            [Description(">")]
            E_LARGERTHAN

        }
        public enum JobCondition
        {
            [Description("")]
            E_HEIGHT,
            [Description("")]
            E_WEIGHT,
            [Description("")]
            E_LEVELEYES,
            [Description("")]
            E_LEVELEYERIGHT,
            [Description("")]
            E_MUSCULOSKELETAL,
            [Description("")]
            E_BLOODPRESSURE,
            [Description("")]
            E_HEARTBEAT,
            [Description("")]
            E_GENARALHEALTH,
            [Description("")]
            E_DISEASELISTIDS,
            [Description("")]
            E_WRITETEST,
            [Description("")]
            E_INTERVIEW,
            [Description("")]
            E_LEVERIGHTLEYES
        }

        public enum EDType
        {
            #region Earning Deduction Type
            [Description("Khoản Nhận")]
            E_EARNING,
            [Description("Khoản Trừ")]
            E_DEDUCTION
            #endregion
        }
        public enum Day
        {
            [Description("01")]
            E_01,
            [Description("02")]
            E_02,
            [Description("03")]
            E_03,
            [Description("04")]
            E_04,
            [Description("05")]
            E_05,
            [Description("06")]
            E_06,
            [Description("07")]
            E_07,
            [Description("08")]
            E_08,
            [Description("09")]
            E_09,
            [Description("10")]
            E_10,
            [Description("11")]
            E_11,
            [Description("12")]
            E_12,
            [Description("13")]
            E_13,
            [Description("14")]
            E_14,
            [Description("15")]
            E_15,
            [Description("16")]
            E_16,
            [Description("17")]
            E_17,
            [Description("18")]
            E_18,
            [Description("19")]
            E_19,
            [Description("20")]
            E_20,
            [Description("21")]
            E_21,
            [Description("22")]
            E_22,
            [Description("23")]
            E_23,
            [Description("24")]
            E_24,
            [Description("25")]
            E_25,
            [Description("26")]
            E_26,
            [Description("27")]
            E_27,
            [Description("28")]
            E_28,
            [Description("29")]
            E_29,
            [Description("30")]
            E_30,
            [Description("31")]
            E_31

        }

        public enum Month
        {
            [Description("01")]
            E_01,
            [Description("02")]
            E_02,
            [Description("03")]
            E_03,
            [Description("04")]
            E_04,
            [Description("05")]
            E_05,
            [Description("06")]
            E_06,
            [Description("07")]
            E_07,
            [Description("08")]
            E_08,
            [Description("09")]
            E_09,
            [Description("10")]
            E_10,
            [Description("11")]
            E_11,
            [Description("12")]
            E_12
        }

        public enum OptionReceive
        {
            [Description("Đủ Tháng")]
            E_FULLMONTH,
            [Description("Đủ Năm")]
            E_FULLYEAR,
            [Description("Năm Tới")]
            E_NEXTYEAR,
            [Description("Đánh Giá Tháng")]
            E_RATEBYMONTH
        }
        public enum DisciplineType
        {
            [Description("Buộc thôi việc")]
            E_SEVERANCE,
            [Description("Đình chỉ công tác")]
            E_CESSATION,
            [Description("Nghỉ việc")]
            E_LAYOFF
        }
        /// <summary>
        /// Loại trong màn hình loại hợp đồng
        /// </summary>
        public enum TypeContract
        {
            [Description("Thời Vụ")]
            E_CASUAL,
            [Description("Thử Việc")]
            E_PROBATION,
            [Description("Chính Thức")]
            E_OFFICIAL,
            [Description("Phụ Lục")]
            E_APPENDIX,
            [Description("Học Việc")]
            E_APPRENTICESHIP,
            [Description("Xác định thời hạn")]
            E_DURATION,
            [Description("Không xác định thời hạn")]
            E_NODURATION

        }
        /// <summary>
        /// Xử Lý Dữ Liệu Trùng
        /// </summary>
        public enum DuplicateData
        {
            [Description("Bỏ Qua")]
            E_SKIP,
            [Description("Tạo mới")]
            E_INSERT,
            [Description("Cập Nhật")]
            E_UPDATE
        }
        /// <summary>
        /// Loại thời gian trong màn hình loại hợp đồng
        /// </summary>
        public enum UnitType
        {
            [Description("Tháng")]
            E_MONTH,
            [Description("Năm")]
            E_YEAR,
            [Description("Vô Thời Hạn")]
            E_INDEFINITE
        }
        public enum MethodPayment
        {
            [Description("Trả Tiền")]
            E_CASHOUT,
            [Description("Nghỉ Bù")]
            E_TIMEOFF
        }
        public enum DurationType
        {
            [Description("Hàng Tuần")]
            E_WEEKLY,
            [Description("Hàng Tháng")]
            E_MOTHLY,
            [Description("Nữa Tháng")]
            E_HALFOFMONTH
        }
        public enum SalaryTimeType
        {
            [Description("Cùng Tháng")]
            E_SAMEMONTH,
            [Description("Tháng Trước")]
            E_LASTMONTH
        }
        public enum StatusOT
        {
            [Description("Chờ Duyệt")]
            E_WATTING_APPROVED,
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Hủy")]
            E_CANCEL,
            [Description("Xác Nhận")]
            E_CONFIRM,
            [Description("Người Duyệt Đầu")]
            E_FIRST_APPROVED,
            [Description("Từ Chối")]
            E_REJECTED,
            [Description("Yêu Cầu")]
            E_REQUEST,
            [Description("Yêu Cầu Tạm")]
            E_REQUESTTEMP,
            [Description("Lưu Tạm")]
            E_SAVETEMP

        }


        public enum DisciplineLevel
        {
            [Description("Công Ty")]
            E_LEVELCOMPANY,
            [Description("Phòng Ban")]
            E_LEVELDEPT
        }

        public enum StatusDiscipline
        {
            [Description("Đã Xử Lý")]
            E_PROCESSED,
            [Description("Hết Hạn Kỷ Luật")]
            E_EXPIRE
        }


        public enum WorkdayStatus
        {
            #region WorkdayStatus
            [Description("Yêu Cầu")]
            E_SUBMIT,
            [Description("Xác Nhận")]
            E_CONFIRM,
            [Description("Từ Chối")]
            E_REJECTED,
            [Description("Hủy")]
            E_CANCEL,
            [Description("Người Duyệt Đầu")]
            E_FIRST_APPROVED,
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Chờ Duyệt")]
            E_WAIT_APPROVED,
            [Description("Lưu Tạm")]
            E_SUBMIT_TEMP
            #endregion
        }

        public enum HoldSalaryStatus
        {
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Chờ Duyệt")]
            E_WAIT_APPROVED,
            [Description("Từ Chối")]
            E_REJECTED,
            [Description("Hủy")]
            E_CANCEL,
        }

        public enum MarriageStatus
        {
            [Description("Độc Thân")]
            E_SINGLE,
            [Description("Đã Kết Hôn")]
            E_MARRIED,
            [Description("Ly Hôn")]
            E_DIVORCE,
            [Description("Quả Phụ")]
            E_WIDOW,
            [Description("Ly Thân")]
            E_SEPARAYTION,
            [Description("Khác")]
            E_OTHER,
        }

        public enum BloodType
        {
            [Description("A")]
            E_A,
            [Description("B")]
            E_B,
            [Description("AB")]
            E_AB,
            [Description("O")]
            E_O,
            [Description("Khác")]
            E_OTHER,
        }
        public enum StatusRoster
        {
            [Description("Duyệt")]
            E_APPROVE,
            [Description("Yêu Cầu")]
            E_REQUEST

        }
        public enum DayOffType
        {
            [Description("Ngày Nghỉ Lễ")]
            E_HOLIDAY_HLD,
            [Description("Ngày Nghỉ Lễ Các Năm")]
            E_HOLIDAY,
            [Description("Cuối Tuần")]
            E_WEEKEND_HLD,
            [Description("Khác")]
            E_OTHER

        }
        public enum HDTJobType
        {
            [Description("Loại 1")]
            E_TYPE1,
            [Description("Loại 2")]
            E_TYPE2,
            [Description("Loại 3")]
            E_TYPE3,
            [Description("Loại 4")]
            E_TYPE4,
            [Description("Loại 5")]
            E_TYPE5,
            [Description("Loại 6")]
            E_TYPE6
        }

        public enum TypeTable
        {
            [Description("Bảng")]
            FOREIGN,
            [Description("Cột")]
            COLUMN,
        }
        public enum HireStatus
        {
            [Description("Chưa Tuyển")]
            E_UNHIRE,
            [Description("Tuyển")]
            E_HIRE,
        }

        public enum CandidateStatus
        {
            #region CandidateStatus
            [Description("Mới Tạo")]
            E_NEW,
            [Description("Chờ Đánh Giá")]
            E_WAITINTERVIEW,
            [Description("Duyệt")]
            E_APPROVE,
            [Description("Chờ Nhận Việc")]
            E_HIRE,
            [Description("Đỗ")]
            E_PASS,
            [Description("Trượt")]
            E_FAIL,
            [Description("Trượt Lọc Hồ Sơ")]
            E_FAILFILTERRESUME,
            [Description("Hủy")]
            E_CANCEL,
            [Description("Từ Chối")]
            E_REJECT
            #endregion
        }

        public enum UnusualEDType
        {
            [Description("Bất Thường")]
            E_UNUSUALALLOWANCE,
            [Description("Thường Xuyên")]
            E_ALLOWANCE
        }

        public enum TypeDynamicColumn
        {
            [Description("Loại 1...")]
            E_Type_1,
            [Description("Loại 2...")]
            E_Type_2,
        }
        public enum StatusDynamicColumn
        {
            [Description("Kích Hoạt")]
            E_ACTIVE,
            [Description("Vô Hiệu")]
            E_NO_ACTIVE,
        }

        public enum UserType
        {
            [Description("Người dùng 1")]
            E_USERTYPE_1,
            [Description("Người dùng 2")]
            E_USERTYPE_2,
            [Description("Người dùng 3")]
            E_USERTYPE_3
        }
        public enum SalaryRate
        {
            [Description("Học Việc")]
            E_STUDY,
            [Description("Thử Việc")]
            E_TRY,
            [Description("Chế Độ Chung")]
            E_GENERAL,
            [Description("Nhân Viên")]
            E_Employee

        }
        public enum PermissionOvertimeType
        {
            [Description("OT 1.5")]
            E_ONE,
            [Description("OT 3.0")]
            E_THREE,
            [Description("null")]
            E_NULL

        }
        public enum TypeElement
        {

            [Description("Lương")]
            Sal_PayrollTableItem,
            [Description("Công")]
            Att_AttendanceTableItem
        }
        public enum DataType
        {
            [Description("Char")]
            Char,
            [Description("Datetime")]
            Datetime,
            [Description("Decimal")]
            Decimal,
            [Description("Float")]
            Float,
            [Description("Int")]
            Int,
            [Description("Nvarchar")]
            Nvarchar,
            [Description("Varchar")]
            Varchar
        }

        public enum ElementDataType
        {

            [Description("Ngày Tháng")]
            Datetime,
            [Description("Kiểu Số Nguyên")]
            Double,
            [Description("Kiểu Chuỗi")]
            Nvarchar

        }

        public enum LockObjectType
        {
            [Description("Bảng Công Và Lương")]
            E_ATT_SAL,
            [Description("Bảng Công")]
            E_ATT,
            [Description("Bảng Lương")]
            E_SAL
        }

        public enum LockObjectStatus
        {
            #region OverTimeStatus
            //[Description("Khóa")]
            //E_LOCKED,
            [Description("Mở Khóa")]
            E_UNLOCKED,
            [Description("Chỉnh Sửa")]
            E_MODIFYING,
            [Description("Người Duyệt Đầu")]
            E_FIRST_APPROVED,
            [Description("Duyệt")]
            E_APPROVED,
            [Description("Chờ Duyệt")]
            E_WAITING_APPROVED,
            [Description("Hủy")]
            E_CANCEL
            #endregion
        }

        public enum PaymentType
        {
            [Description("Trả tiền")]
            E_CASHOUT,
            [Description("Nghỉ bù")]
            E_TIMEOFF,
            [Description("Trả tiền và nghỉ bù")]
            E_CASHOUT_TIMEOFF
        }
        public enum AnnualLeaveDetailType
        {
            E_SICK_LEAVE, E_ANNUAL_LEAVE, E_INSURANCE_LEAVE
        }

        public enum WorkdaySrcType
        {
            [Description("Tự Động")]
            E_NORMAL,
            [Description("Bẳng Tay")]
            E_MANUAL
        }

        public enum WorkdayType
        {
            /// <summary>
            /// bình thường
            /// </summary>
            [Description("Hợp Lệ")]
            E_NORMAL,
            /// <summary>
            /// Thiếu in
            /// </summary>
            [Description("Thiếu In")]
            E_MISS_IN,
            /// <summary>
            /// Thiếu out
            /// </summary>
            [Description("Thiếu Out")]
            E_MISS_OUT,
            /// <summary>
            /// Thiếu cả in out
            /// </summary>
            [Description("Thiếu Cả In Out")]
            E_MISS_IN_OUT,
            /// <summary>
            /// Thời gian in out không thuộc ca
            /// </summary>
            [Description("Sai Ca")]
            E_WRONG_SHIFT,
            /// <summary>
            /// Tự detect shift khi không có shift
            /// </summary>
            [Description("Tự Nhận Ca")]
            E_DETECTED_SHIFT,
            [Description("Ca Ngắn")]
            E_SHORTIN_SHIFT,
            [Description("Ca Dài")]
            E_LONGIN_SHIFT,
            /// <summary>
            /// Đi làm ngày lễ
            /// </summary>
            [Description("Ngày Lễ")]
            E_HOLIDAY,
            [Description("Muộn Sớm")]
            E_LATE_EARLY
        }

        public enum ComputeLeavedayType
        {
            [Description("Dữ Liệu Chưa Đăng Ký Nghỉ")]
            E_DATA_NON_LEAVE,
            [Description("Dữ Liệu Đã Đăng Ký Nghỉ")]
            E_DATA_LEAVE
        }

        public enum ComputeOvertimeType
        {
            [Description("Dữ Liệu Đã Đăng Ký Tăng Ca")]
            E_DATA_OT,
            [Description("Dữ Liệu Chưa Đăng Ký Tăng Ca")]
            E_DATA_NON_OT
        }
        public enum OverTimeType
        {
            #region OverTimeType
            E_WORKDAY, E_WORKDAY_NIGHTSHIFT, E_WEEKEND, E_WEEKEND_NIGHTSHIFT, E_HOLIDAY, E_HOLIDAY_NIGHTSHIFT,
            #endregion
        }

        public enum SalesType
        {
            /// <summary>
            /// Các dòng sản phẩm chủ đạo
            /// </summary>
            /// 
            [Description("Các dòng sản phẩm chủ đạo")]
            E_LINEITEM_MAJOR,

            /// <summary>
            /// Sản phẩm chủ lực
            /// </summary>
            /// 
            [Description("Sản phẩm chủ lực")]
            E_ITEM_MAJOR,
            /// <summary>
            /// Doanh Số
            /// </summary>
            [Description("Doanh Số")]
            E_SALE,
            ////[Description("Khách Hàng Cũ")]
            ////E_OLDCUSTOMER,
            ////[Description("Khách Hàng Mới")]
            ////E_NEWCUSTOMER,
            ////[Description("Khách Hàng Trung Thành")] 
            ////E_LOYALCUSTOMER,
            ///// <summary>
            ///// Tổng Khách Hàng
            ///// </summary>
            //[Description("Tổng Khách Hàng")]
            //E_TOTALCUSTOMER,
            ///// <summary>
            ///// Số Lượng KHDV
            ///// </summary>
            //[Description("Số Lượng KHDV")]
            //E_NOOFSERVICECUSTOMER,
            ///// <summary>
            ///// Tiền DV"
            ///// </summary>
            //[Description("Tiền DV")]
            //E_MONEYSERVICE,
        }
        public enum PurColor
        {
            E_MOTOR,
            E_CAR,
        }

        public enum StatusCheck
        {
            E_PASS,
            E_FAIL,
        }

        public enum ClassStatus
        {
            [Description("Chưa Học")]
            E_CLOSE,
            [Description("Đang Học")]
            E_MASS_TRAINING,
            [Description("Đã Học")]
            E_PASSED_CERTIFICATE
        }
        public enum EvaExpiryContract
        {
            [Description("Đã Đánh Giá")]
            E_EVA_CONTRACT,
            [Description("Chưa Đánh Giá")]
            E_NONEEVA_CONTRACT,

        }
        public enum FormulaType
        {
            [Description("Nhập Công Thức Tính Điểm Bằng Tay")]
            E_TYPEHANDLEFORMULA_NORMAL,
            [Description("Tự Động Tạo Công Thức Tính Điểm Trung Bình")]
            E_TYPEHANDLEFORMULA_AVERAGE,

        }
        public enum StatusLoadEnumSalDepartItem
        {
            [Description("Mới Tạo")]
            E_NEW,
            [Description("Loại Trừ")]
            E_REMOVE,
            [Description("Chỉnh Sửa")]
            E_EDIT,
            [Description("Cố Định")]
            E_FIX,
            [Description("Chỉnh Sửa Và Cố Định")]
            E_EDIT_FIX
        }



        public enum SalesTypeRevenue
        {
            /// <summary>
            /// Doanh Số
            /// </summary>
            [Description("Doanh Số")]
            E_SALE,
            /// <summary>
            /// Tổng Khách Hàng
            /// </summary>
            [Description("Tổng Khách Hàng")]
            E_TOTALCUSTOMER,
            /// <summary>
            /// Số Lượng KHDV
            /// </summary>
            [Description("Số Lượng KHDV")]
            E_NOOFSERVICECUSTOMER,
            /// <summary>
            /// Tiền DV"
            /// </summary>
            [Description("Tiền DV")]
            E_MONEYSERVICE,

            /// <summary>
            /// Các dòng sản phẩm chủ đạo
            /// </summary>
            /// 
            [Description("Các dòng sản phẩm chủ đạo")]
            E_LINEITEM_MAJOR,

            /// <summary>
            /// Sản phẩm chủ lực
            /// </summary>
            /// 
            [Description("Sản phẩm chủ lực")]
            E_ITEM_MAJOR,
        }
    }
    public enum AsynTaskStatus
    {
        Queue,
        Doing,
        Done,
        Error,
        Error_Excel,
        Locked
    }
    /// <summary>
    /// Tac vu xu ly bất đồng bộ
    /// </summary>
    public enum AsynTask
    {
        Load_Attendance,
        Download_TimeLog,
        Download_TimeLog_CMS,
        Download_TimeLog_LMS,
        Attendance_Computing,
        Attendance_Compute_Workday,
        Attendance_By_Duration_Computing,
        Payroll_By_Duration_Computing,
        Payroll_Different_By_Duration_Generating,
        Payroll_Computing,
        Payroll_Computing_Commission,
        Payroll_Computing_HoldSalary,
        Payroll_Compute_BonusUnusualAllowance,
        Payroll_Compute_AnnualLeaveAllowance,
        Export_Payroll,
        Export_Attendance,
        Export_NonAttendance,
        Payroll_Different_Generating,
        Run_Audit,
        Convert_OT,
        Change_Password,
        Login_Failed,
        Lock_User,
        ImportFromTAM,
    }

    /// <summary>
    /// Trạng Thái nhân viên các báo cáo  Phân Tích Chi Tiết Phép Năm, phép ốm, phép BHXH
    /// </summary>
    public enum StatusEmpLeaveDetail
    {
        /// <summary>
        /// NHÂN VIÊN MỚI TRONG NĂM
        /// </summary>
        [Description("Nhân Viên Mới Trong Năm")]
        E_NEWEMPINYEAR,
        /// <summary>
        /// NHÂN VIÊN NGHỈ VIỆC TRONG NĂM
        /// </summary>
        [Description("Nhân Viên Nghỉ Việc Trong Năm")]
        E_RESIGNEMPINYEAR,
        /// <summary>
        /// NHÂN VIÊN THUỘC HDT4
        /// </summary>
        [Description("Nhân Viên Thuộc HDT4")]
        E_EMPOFHDT4,
        /// <summary>
        /// NHÂN VIÊN THUỘC HDT5
        /// </summary>
        [Description("Nhân Viên Thuộc HDT5")]
        E_EMPOFHDT5,


    }


    public enum StatusMealAllowanceType
    {
        [Description("Loai")]
        E_MealAllowanceType1 = 1,

        [Description("Loại 2")]
        E_MealAllowanceType2,

    }

    /// <summary>
    /// Kiểu xóa dữ liệu: Delete or Remove
    /// </summary>
    public enum DeleteType
    {
        Remove,
        Delete,
    }

    /// <summary>
    /// Loại thông báo cho 
    /// </summary>
    public enum NotificationType
    {
        Info,
        Success,
        Locked,
        Error,
        Duplicate,
        NonDuplicate,
        Warning,
        Change,
        E_NotNull,
        E_Mismatch,
        E_Incorrect,
        E_Messages,
        E_NotFound,
        E_ChangePass_Success
    }

    public enum StatusMealAllowanceToMoney
    {
        [Description("Chờ Duyệt")]
        E_WAIT_APPROVED,
        [Description("Duyệt")]
        E_APPROVED,
        [Description("Từ Chối")]
        E_CANCEL,
        [Description("Yêu Cầu")]
        E_REQUEST,
    }

    public enum TypeHDTJOBOfCanLine
    {

        IV = 0,
        V = 1
    }

    public enum HDTJobType
    {
        E_One = 1, E_Tow, E_Three, E_Four, E_Five, E_Six
    }

    public enum HDTJobStatus
    {
        [Description("Yêu Cầu")]
        E_SUBMIT,
        [Description("Chờ Duyệt")]
        E_WAITING,
        [Description("Duyệt")]
        E_APPROVE,
        [Description("Từ Chối")]
        E_REFUSE,
        [Description("Hủy")]
        E_CANCEL
    }

    public enum EDType
    {
        #region Earning Deduction Type
        [Description("Khoản Nhận")]
        E_EARNING,
        [Description("Khoản Trừ")]
        E_DEDUCTION
        #endregion
    }

    public enum EFormula
    {
        [Description("Theo Nguyên Tắc")]
        E_RULES,
        [Description("Nhận Đủ")]
        E_FULLPAYMENT,
        [Description("Theo Công Thức")]
        E_FORMULA

    }

    #region Recruitment(tuyển dụng)

    public enum RelativeType
    {
        E_BRANCH, E_OPPONENT, E_SUPPLIER, E_OWNBUSINESS
    }
    public enum ConditionName
    {
        [Description("Chiều Cao")]
        E_HEIGHT,
        [Description("Cân Nặng")]
        E_WEIGHT,
        [Description("Mắt Trái")]
        E_LEVELEYES,
        [Description("Mắt Phải")]
        E_LEVERIGHTLEYES,
        [Description("Thời Gian Trượt Kỳ Trước")]
        E_DURATIONFAILPREVIOUS,
        [Description("Đã nghỉ Việc Tại Công Ty/ Đang Làm Việc")]
        E_ISTERMINATEINCOMPANY,
        [Description("Tuổi")]
        E_AGE,
        [Description("Giới Tính")]
        E_GENDER,
        [Description("Bệnh Loại Trừ")]
        E_DISEASEIDS,
        [Description("Tổng Sức Khỏe")]
        E_GENERALHEALTH,
        [Description("Điểm Thi Viết")]
        E_WRITETEST,
        [Description("Tổng Điểm Phỏng Vấn")]
        E_INTERVIEW,
        [Description("Điểm Thi 1")]
        E_SCORE1,
        [Description("Điểm Thi 2")]
        E_SCORE2,
        [Description("Điểm Thi 3")]
        E_SCORE3,
        [Description("Hệ Vận Động")]
        E_MUSCULOSKELETAL,
        [Description("Huyết Áp")]
        E_BLOODPRESSURE,
        [Description("Nhịp Tim")]
        E_HEARTBEAT,
        [Description("Trình Độ Học Vấn")]
        E_EDUCATIONLEVEL,
        [Description("Số Năm Kinh Nghiệm")]
        E_YEAROFEXPERIENCE,
        [Description("Trường Tốt Nghiệp")]
        E_GRADUATESCHOOL,
        [Description("Chuyên Nghành")]
        E_SPECIALISATION,
        [Description("Tỉnh Thường Trú")]
        E_CANDIDATEP_PPROVINCE,
    }
    public enum ConditionType
    {
        [Description("Số")]
        E_INT,
        [Description("Đối Tượng")]
        E_OBJECT,
        [Description("Đúng/Sai")]
        E_BOOL,
        [Description("Ngày Tháng")]
        E_DATETIME
    }
    public enum ConditionBrand
    {
        [Description("Điều Kiện Thỏa")]
        E_AGREEMENT,
        [Description("Điều Kiện Không Thỏa")]
        E_NOTAGREEMENT
    }
    public enum ValueType
    {
        [Description("Lớn Hơn")]
        E_THAN,
        [Description("Nhỏ Hơn")]
        E_LESS,
        [Description("Bằng")]
        E_EQUAL,
        // [Description("Trong Khoảng")]
        //E_BETWEEN,
        //      [Description("Nội Dung")]
        //     E_CONTENT,
        //   [Description("Và")]
        //   E_AND,
        //   [Description("Hoặc")]
        //    E_OR,

    }
    public enum StatusQuestionaire
    {
        [Description("Kích Hoạt")]
        E_ACTIVE,
        [Description("Chưa Kích Hoạt")]
        E_INACTIVE,
    }
    public enum RecruitmentCampaignStatus
    {
        #region RecruitmentCampaignStatus
        [Description("Mới Tạo")]
        E_OPEN,
        [Description("Chờ Duyệt")]
        E_WAITAPPROVE,
        [Description("Yêu Cầu")]
        E_SUBMIT,
        [Description("Duyệt")]
        E_APPROVED,
        [Description("Đóng")]
        E_CLOSE,
        [Description("Từ Chối")]
        E_REJECT,
        [Description("Hủy")]
        E_CANCEL
        #endregion
    }
    public enum JobVacancyType
    {
        [Description("Thay Thế")]
        E_VACANCY_REPLACE,
        [Description("Thêm Mới")]
        E_VACANCY_NEW,
        [Description("Tuyển Thêm")]
        E_VACANCY_ADD
    }
    public enum Interview
    {
        [Description("Đỗ")]
        E_PASS,
        [Description("Trượt")]
        E_FAIL,
    }
    public enum JobVacancyStatus
    {
        #region JobVacancyStatus
        [Description("Yêu Cầu")]
        E_SUBMIT,
        //[Description("Chờ Duyệt")]
        //E_WAITAPPROVE,
        //[Description("Duyệt Cấp 1")]
        //E_FIRST_APPROVED,
        [Description("Duyệt")]
        E_APPROVE,
        [Description("Từ Chối")]
        E_REFUSE,
        [Description("Hủy")]
        E_CANCEL,
        [Description("Hoàn Thành")]
        E_FINISH,
        [Description("Duyệt Và Gửi Đến")]
        E_ASSIGN,
        [Description("Đóng")]
        E_CLOSE
        #endregion
    }
    #endregion
    #region Đánh Giá

    /// <summary> Trạng Thái Đánh Giá NV </summary>
    public enum PerformaceEvaStatus
    {
        [Description("Tự Đánh Giá")]
        E_SEFT,
        [Description("Leader Đánh Giá")]
        E_SUBMIT,
        [Description("Chờ Đánh Giá")]
        E_WAITING,
        [Description("Đánh Giá 1")]
        E_APPROVE_1,
        [Description("Đánh Giá 2")]
        E_APPROVE_2,
        [Description("Đánh Giá 3")]
        E_APPROVE_3,
        [Description("Đánh Giá 4")]
        E_APPROVE_4,
        [Description("Đánh Giá 5")]
        E_APPROVE_5,
        [Description("Đánh Giá 6")]
        E_APPROVE_6,
        [Description("Đánh Giá 7")]
        E_APPROVE_7,
        [Description("Đánh Giá 8")]
        E_APPROVE_8,
        [Description("Đánh Giá 9")]
        E_APPROVE_9,
        [Description("Đánh Giá 10")]
        E_APPROVE_10,
        [Description("Đánh Giá Cuối")]
        E_APPROVE_END,
        /// <summary>Sử dụng đặc thù cho HonDa</summary>
        [Description("Duyệt")]
        E_APPROVED,
    }

    public enum EvaBonusType
    {
        [Description("Tháng")]
        E_Month,
        [Description("Quí")]
        E_Quarter
    }

    public enum Quarter
    {
        [Description("Quí 1")]
        E_Quarter1 = 1,
        [Description("Quí 2")]
        E_Quarter2 = 2,
        [Description("Quí 3")]
        E_Quarter3 = 3,
        [Description("Quí 4")]
        E_Quarter4 = 4,

    }
    public enum Mobility
    {
        [Description("Currently Not Mobile")]
        E_CURRENTLYNOTMOBILE,
        [Description("Current Country")]
        E_CURRENTCOUNTRY,
        [Description("Current Region")]
        E_CURRENTREGION,
        [Description("Other Region")]
        E_OTHERREGION,
        [Description("Global")]
        E_GLOBAL,
        [Description("Current location")]
        E_CURRENTLOCATION,

    }
    public enum Proficiency
    {
        [Description("Native")]
        E_NATIVE,
        [Description("Fluent")]
        E_FLUENT,
        [Description("Advanced")]
        E_ADVANCED,
        [Description("Intermediate")]
        E_INTERMEDIATE,
        [Description("Basic")]
        E_BASIC,
    }
    public enum ThisYear
    {
        [Description("Next Level")]
        E_NEXTLEVEL,
        [Description("Current Level")]
        E_CURRENTLEVEL,
        [Description("Well Placed")]
        E_WELLPLACED,
        [Description("New On Job")]
        E_NEWONJOB,
    }
    public enum Option
    {
        [Description("Business Management")]
        E_BUSINESSMANAGEMENT,
        [Description("Engineering & Technical")]
        E_ENGINEERINGTECHNICAL,
        [Description("Finance & Controlling")]
        E_FINANCECONTROLLING,
        [Description("General Functions")]
        E_GENERALFUNCTIONS,
        [Description("Human Resources")]
        E_HUMANRESOURCES,
        [Description("Information Technology")]
        E_INFORMATIONTEACHNOLOGY,
        [Description("Legal")]
        E_LEGAL,
        [Description("Marketing")]
        E_MARKETING,
        [Description("Sales")]
        E_SALES,
        [Description("Operations")]
        E_OPERATIONS,
        [Description("Supply & Deliver")]
        E_SUPPLYDELIVER,
        [Description("Scientific & Product Development")]
        E_SCIENTIFICPRODUCTDEVELOPMENT,

    }

    public enum RiskToLose
    {
        [Description("No foreseeable risk to lose")]
        E_RISTTOLOSE,
        [Description("Risk to lose within next 2 years")]
        E_TWOYEAR,
        [Description("Change scheduled")]
        E_CHANGESCHEDULED,
    }

    public enum UrgencyToMove
    {
        [Description("Within 12 months")]
        E_TWELVEMONTH,
        [Description("In 1-2 years")]
        E_ONETWOYEAR,
        [Description("No need for action")]
        E_NEEDACTION,
    }

    public enum MeasureType
    {
        [Description("New Assignment")]
        E_NEWASSIGNMENT,
        [Description("International Assignment (<3 Month)")]
        E_INTERNATIONALASSIGNMENT,
        [Description("Project Assignment")]
        E_PROJECTASSIGNMENT,
        [Description("Challenging Task")]
        E_CHALLENGINTASK,
        [Description("Guidance & Advice")]
        E_GUIDANCEADVICE,
        [Description("Development Feedback")]
        E_DEVELOPMENTFEEDBACK,
        [Description("Teaching Others")]
        E_TEACHINGOTHERS,
        [Description("On The Job Traning")]
        E_ONTHEJOBTRANING,
        [Description("Learning From Success")]
        E_LEARNINGFROMSUCCESS,
        [Description("Learning From Others")]
        E_LEARNINGFROMOTHERS,
        [Description("Coaching")]
        E_COACHING,
        [Description("Mentoring")]
        E_MENTORING,
        [Description("360 Feedback")]
        E_360FEEDBACK,
        [Description("Training")]
        E_TRAINING,
        [Description("Other")]
        E_OTHER,
    }


    #endregion

    #region Bảo Hiểm

    public enum CurrencyCode
    {
        VND,//Vietnam dong
        USD,
        SGN // Singapore
    }

    public enum ExchangeRateType
    {
        #region ExchangeRateType
        E_RATE_PAYROLL, E_RATE_SOCIALINSURANCE
        #endregion
    }

    public enum ValueEntityType
    {
        /// <summary>
        /// Muc tran dong BHXH
        /// </summary>
        [Description("Mức Trần Đóng BHXH")]
        E_INSURANCE_CAPE_AMOUNT,
        /// <summary>
        /// Ty gia dong BHXH
        /// </summary>
        [Description("Tỷ Giá Đóng BHXH")]
        INSURANCE_USD_TO_VND,
        /// <summary>
        /// Luong toi thieu
        /// </summary>
        [Description("Lương Tối Thiểu")]
        E_MINIMUM_SALARY,
    }

    public enum InsuranceRecordType
    {
        /// <summary>
        /// Bản Thân Ốm Ngắn Ngày
        /// </summary>
        [Description("Bản Thân Ốm Ngắn Ngày")]
        E_SICK_SHORT,

        /// <summary>
        /// Bản Thân Ốm Dài Ngày
        /// </summary>
        [Description("Bản Thân Ốm Dài Ngày")]
        E_SICK_LONG,

        /// <summary>
        /// Con Ốm
        /// </summary>

        [Description("Con Ốm")]
        E_SICK_CHILD,

        /// <summary>
        /// Kham thai
        /// </summary>
        [Description("Khám Thai")]
        E_PREGNANCY_EXAMINE,

        /// <summary>
        /// Sẩy thai, nạo hút thai, thai chết lưu
        /// </summary>
        [Description("Sẩy Thai, Nạo Hút Thai, Thai Chết Lưu")]
        E_PREGNANCY_LOST,

        /// <summary>
        /// Sinh con, nuôi con nuôi
        /// </summary>
        [Description("Sinh Con, Nuôi Con Nuôi")]
        E_PREGNANCY_SUCKLE,

        /// <summary>
        /// Thực hiện các biện pháp tránh thai
        /// </summary>
        [Description("Thực Hiện Các Biện Pháp Tránh Thai")]
        E_PREGNANCY_PREVENTION,

        /// <summary>
        /// bao cao duong suc thai san Restoration
        /// </summary>
        [Description("Dưỡng Sức Thai Sản Restoration")]
        E_RESTORATION_PREGNANCY,
        /// <summary>
        /// Bao cao duong suc om dau
        /// </summary>
        [Description("Dưỡng Sức Ốm Đau")]
        E_RESTORATION_SICK,
        /// <summary>
        /// Dưỡng sức sau tai nạn lao động. Bệnh Nghê nghiệp
        /// </summary>
        [Description("Dưỡng Sức Sau Tai Nạn Lao Động. Bệnh Nghề Nghiệp")]
        E_RESTORATION_TNLD,
        /// <summary>
        /// Nghỉ Tại Gia Đình
        /// </summary>
        [Description("Nghỉ Tại Gia Đình")]
        E_LEAVE_AT_HOME,
        /// <summary>
        /// Nghỉ Tập Trung
        /// </summary>
        [Description("Nghỉ Tập Trung")]
        E_LEAVE_AT_SAME_PLACE,

        /// <summary>
        /// Nhóm 
        /// </summary>
        [Description("Chế Độ Ốm Đau")]
        A_SICK_GROUP,
        [Description("Chế Độ Thai Sản")]
        B_PREGNANCY_GROUP,
        [Description("Dưỡng Sức Phục Hồi Sức Khỏe Sau Ốm Đau")]
        C_RESTORATION_SICK_GROUP,
        [Description("Dưỡng Sức Phục Hồi Sức Khỏe Sau Sinh")]
        D_RESTORATION_PREGNANCY_GROUP,
        [Description("Dưỡng Sức Phục Hồi Sức Khỏe Sau TNLD - Bệnh Nghề Nghiệp")]
        E_RESTORATION_TNLD_GROUP,

    }


    public enum InsuranceRecordTypeData
    {
        [Description("Trống")]
        E_EMPTY,
        /// <summary>
        /// Nghỉ Tại Gia Đình
        /// </summary>
        [Description("Nghỉ Tại Gia Đình")]
        E_LEAVE_AT_HOME,
        /// <summary>
        /// Nghỉ Tập Trung
        /// </summary>
        [Description("Nghỉ Tập Trung")]
        E_LEAVE_AT_SAME_PLACE,
    }

    public enum TypeSuckle
    {
        /// <summary>
        /// Sinh thuong
        /// </summary>
        [Description("Sinh Thường")]
        E_SUCKLE_USUALLY,
        /// <summary>
        /// Sinh mo
        /// </summary>
        [Description("Sinh Mổ")]
        E_SUCKLE_SURGERY,
        /// <summary>
        /// Sinh doi
        /// </summary>
        [Description("Sinh Đôi")]
        E_SUCKLE_TWINS,
    }

    public enum TypeSick
    {
        /// <summary>
        /// Phau thuat
        /// </summary>
        [Description("Phẫu Thuật")]
        E_SICK_SURGERY,
        /// <summary>
        /// Dieu tri
        /// </summary>
        [Description("Điểu Trị")]
        E_SICK_TREATMENT,
        /// <summary>
        /// Other
        /// </summary>
        [Description("Khác")]
        E_SICK_OTHER,

    }

    public enum TypeInsuranceStatus
    {
        /// <summary>
        /// Dang cho
        /// </summary>
        [Description("Đang Chờ")]
        E_INS_WAITING,
        /// <summary>
        /// Xac nhan
        /// </summary>
        [Description("Xác Nhận")]
        E_INS_CONFIRM,
    }

    #endregion

    #region Hệ Thống (System)

    public enum ApproveType
    {
        #region ApproveType
        //E_LEAVE_DAY,
        //E_OVERTIME,
        //E_TRANININGPLAN,
        //E_CANDIDATE,
        //E_JOBVACANCY,
        //E_REQUIREMENTTRAIN,
        //E_REWARD,
        //E_DISCIPLINE,
        //E_BUSINESSTRIP,
        //E_ROSTER,
        //E_LOCKOBJECT,
        //E_SPEND,
        //E_CONTRACTAPPROVE
        #endregion
        #region ApproveType
        [Description("Duyệt Lương Cơ Bản")]
        E_BASICSALARY,
        [Description("Duyệt Ng.Nghỉ")]
        E_LEAVE_DAY,
        [Description("Tăng Ca")]
        E_OVERTIME,
        [Description("Chờ Duyệt Trợ Cấp Căng Tin")]
        E_WAITALLOWANCECANTEEN,
        [Description("Duyệt Trợ Cấp Căng Tin")]
        E_ALLOWANCECANTEEN,
        [Description("Chờ Duyệt Dữ Liệu Quên Quẹt Thẻ")]
        E_MealRecordMissingWaitApprove,
        [Description("Duyệt Dữ Liệu Quên Quẹt Thẻ")]
        E_MealRecordMissingApprove,
        [Description("Duyệt Giặt Là")]
        E_LAUNDRYRECORDAPPROVE,
        [Description("Duyệt Kế Hoạch Đào Tạo")]
        E_TRANININGPLAN,
        [Description("Duyệt Ứng Viên")]
        E_CANDIDATE,
        [Description("Duyệt Yêu Cầu Tuyển")]
        E_JOBVACANCY,
        [Description("Duyệt Yêu Cầu Đào Tạo")]
        E_REQUIREMENTTRAIN,
        [Description("Duyệt Khen Thưởng")]
        E_REWARD,
        [Description("Duyệt Kỷ Luật")]
        E_DISCIPLINE,
        [Description("Duyệt Công Tác Nước Ngoài")]
        E_BUSINESSTRIP,
        [Description("Duyệt Ca Làm Việc")]
        E_ROSTER,
        [Description("Duyệt Khóa Đối Tượng")]
        E_LOCKOBJECT,
        [Description("Duyệt Chi Phí")]
        E_SPEND,
        [Description("Duyệt Ngày Công")]
        E_WORKDAY_APPROVE,
        [Description("Duyệt Hợp Đồng")]
        E_CONTRACTAPPROVE,
        [Description("Duyệt Đơn Hàng")]
        E_FIN_PurchaseRequest,
        [Description("Duyệt Chi Phí Công Tác")]
        E_FIN_TravelRequest,
        [Description("Duyệt Thanh Toán Chi Phí Công Tác")]
        E_FIN_ClaimRequest,
        [Description("Duyệt Thanh Tạm Ứng")]
        E_FIN_CashAdvance,
        [Description("Duyệt Yêu Cầu Đào Tạo")]
        E_TRA_REQUIREMENTTRAIN,

        [Description("Duyệt HDTJob Trong Quá Khứ")]
        E_HDTJOB_PAST,
        [Description("Duyệt Học Viên Đăng Ký Lớp Học")]
        E_TRA_TRAINEEREGISTER,
        [Description("Duyệt Kết Quả Đánh Giá Và Tăng Lương")]
        E_EVA_PERFORMANCEGENERAL,
        #endregion
    }

    public enum ExpensesType
    {
        [Description("Công Tác Trong Nước")]
        E_DomesticTravel,
        [Description("Công Tác Nước Ngoài")]
        E_OverseaTravel,
        [Description("Giao Tế")]
        E_Entertaiment,
        [Description("Khác")]
        E_Other,
    }

    #endregion

    public enum TypeInsuranceD02TS
    {
        /// <summary>
        /// LOAI TRUY TANG
        /// </summary>
        [Description("Tăng")]
        E_TANG,
        /// <summary>
        /// Tang lao dong
        /// </summary>
        [Description("Tăng Lao Động")]
        E_TANG_LD,
        /// <summary>
        /// Tang sau thai san
        /// </summary>
        [Description("Nghỉ Sinh Con Đi Làm Lại")]
        E_TANG_TS,
        /// <summary>
        /// Tang sau benh
        /// </summary>
        [Description("Tăng Sau Bệnh")]
        E_TANG_BENH,
        /// <summary>
        /// Tang luong
        /// </summary>
        [Description("Tăng Lương")]
        E_TANG_LUONG,
        /// <summary>
        /// Tang BHYT
        /// </summary>
        [Description("Tăng BHYT")]
        E_TANG_BHYT,
        /// <summary>
        /// Tang BHTN
        /// </summary>
        [Description("Tăng Đóng BHTN")]
        E_TANG_BHTN,
        /// <summary> Tang do nghỉ hơn 14 ngày </summary>
        [Description("Nghỉ >= 14 Ngày Đi Làm Lại")]
        E_TANG_LEAVE_14WORKINGDAYS,
        /// <summary>
        /// Truy tang khac (do chua nop, di nuoc ngoai, om dau thai san lay the...)
        /// </summary>
        [Description("Truy Tăng Khác")]
        E_TANG_OTHER,
        /// <summary>
        /// Truy thu BHYT khac (fgl)
        /// </summary>
        [Description("Truy Thu BHYT Khác")]
        E_TANG_TRUYTHU_BHYT,
        /// <summary> Tăng Lương và thay đổi chức danh nghề [Honda] </summary>
        [Description("Tăng Lương và thay đổi chức danh nghề")]
        E_TANG_LUONG_CHANGEJOBNAME,
        /// <summary>
        /// LOAI TRUY GIAM
        /// </summary>
        [Description("Giảm")]
        E_GIAM,
        /// <summary>
        /// Giam lao dong
        /// </summary>
        [Description("Giảm Lao Động")]
        E_GIAM_LD,
        /// <summary>
        /// Giam lao dong tra the BHYT kip thoi
        /// </summary>
        [Description("Nghỉ Việc")]
        E_GIAM_LD_BHYT,
        /// <summary>
        /// Giam lao dong tra the BHYT không kip thoi
        /// </summary>
        [Description("Giảm Lao Động Trả Thẻ")]
        E_GIAM_LD_BHYT_KOKIPTHOI,
        /// <summary>
        /// Giam lao dong khong tra the BHYT 
        /// </summary>
        [Description("Giảm Lao Động Không Trả Thẻ")]
        E_GIAM_LD_NOT_BHYT,
        /// <summary>
        /// Giam thai san
        /// </summary>
        [Description("Nghỉ Sinh Con")]
        E_GIAM_TS,
        /// <summary>
        /// Nghỉ Việc Sau Khi Nghỉ Sinh Con
        /// </summary>
        [Description("Nghỉ Việc Sau Khi Nghỉ Sinh Con")]
        E_GIAM_TS_QUIT,
        /// <summary>
        /// Giam lao dong khong tra the BHYT 
        /// </summary>
        [Description("Giảm Bệnh")]
        E_GIAM_BENH,
        /// <summary>
        /// Giam BHYT
        /// </summary>
        [Description("Giảm BHYT")]
        E_GIAM_BHYT,
        /// <summary>
        /// Giam BHTN
        /// </summary>
        [Description("Giảm BHTN")]
        E_GIAM_BHTN,
        /// <summary>
        /// Giam muc dong (luong)
        /// </summary>
        [Description("Giảm Lương")]
        E_GIAM_LUONG,
        /// <summary>
        /// Truy giam khac (do chua nop, di nuoc ngoai, om dau thai san lay the...)
        /// </summary>
        [Description("Truy Giảm Khác")]
        E_GIAM_OTHER,
        /// <summary>
        /// Truy giam Thai san
        /// </summary>
        [Description("Truy Giảm Thai Sản")]
        E_GIAM_TRUYGIAM_TS,
        /// <summary> Giảm do nghỉ hơn 14 ngày </summary>
        [Description("Nghỉ >= 14 Ngày")]
        E_GIAM_LEAVE_14WORKINGDAYS,

        /// <summary> Nghỉ Việc Mà Trước Đó Nghỉ >= 14 Ngày </summary>
        [Description("Nghỉ Việc Mà Trước Đó Nghỉ >= 14 Ngày")]
        E_GIAM_LEAVE_PREMONTH_14WORKINGDAYS,

        /// <summary> Giảm Do Nghỉ 14 Ngày Sau Thai Sản </summary>
        [Description("Nghỉ >= 14 Ngày Sau Khi Sinh Con")]
        E_GIAM_PREGNANT_14WORKINGDAYS,

        /// <summary> Giảm Lương và thay đổi chức danh nghề [Honda] </summary>
        [Description("Giảm Lương Và Thay Đổi Chức Danh Nghề")]
        E_GIAM_LUONG_CHANGEJOBNAME,
        /// <summary> Thay đổi chức danh nghề [Honda] </summary>
        [Description("Thay Đổi Chức Danh Nghề")]
        E_CHANGEJOBNAME,
        /// <summary>Giảm do tạm hoãn</summary>
        [Description("Tạm Hoãn Hợp Đồng")]
        E_GIAM_QUIT_SUSPENSE,
        /// <summary>Giảm do tạm hoãn</summary>
        [Description("Điều Chỉnh")]
        E_Dieu_Chinh,
        [Description("Điều Chỉnh Tăng")]
        E_DieuChinhTang,
        [Description("Điều Chỉnh Giảm")]
        E_DieuChinhGiam,
        [Description("Tăng Lao Động Do Chuyển Nơi Đóng BH")]
        E_TANG_LD_CHANGE_INSPLACE,
        [Description("Giảm Lao Động Do Chuyển Nơi Đóng BH")]
        E_GIAM_LD_CHANGE_INSPLACE,
    }
    public enum InsuranceSettlement
    {
        /// <summary>Tất Cả</summary>
        [Description("Tất Cả")]
        E_INS_SETTLEMENT_ALL,

        /// <summary>Đã Quyết Toán</summary>
        [Description("Đã Quyết Toán")]
        E_INS_SETTLEMENT_SUCCESS,

        /// <summary>Chưa Quyết Toán</summary>
        [Description("Chưa Quyết Toán")]
        E_INS_SETTLEMENT_YET,
    }

    public enum ReceiveSocialInsType
    {
        /// <summary>Tất Cả</summary>
        [Description("Tất Cả")]
        E_INS_RECEIVESOCIAL_ALL,

        /// <summary>Đã Trả Thẻ</summary>
        [Description("Đã Trả Thẻ")]
        E_INS_RECEIVESOCIAL_SUCCESS,

        /// <summary>Chưa Trả Thẻ</summary>
        [Description("Chưa Trả Thẻ")]
        E_INS_RECEIVESOCIAL_YET,
    }

    /// <summary>
    /// Loại Duyệt
    /// </summary>
    public enum FinApproverType
    {
        /// <summary> Tạm Ứng </summary>
        [Description("Công Tác Trong Nước")]
        FIN_TravelRequest_Domestic,
        [Description("Công Tác Nước Ngoài")]
        FIN_TravelRequest_External,
        [Description("Tạm Ứng")]
        Fin_CashAdvance,
        [Description("Thanh Toán Cho Nhân Viên")]
        FIN_Claim_Profile,
        [Description("Thanh Toán Cho Nhà Cung Cấp")]
        FIN_Claim_Manufacture,

    }


    /// <summary> Kỳ Bảo Hiểm ( chia làm 2 kỳ [tạm thời] , [Chính Thức]) {Sử dụng khi phân tích bảo hiểm} </summary>
    public enum PeriodInsurance
    {
        /// <summary> Kỳ Tạm thời (thời gian dựa vào cấu hình) </summary>
        [Description("Tạm Thời")]
        GETTEMP,
        /// <summary> Kỳ Chính Thức (thời gian dựa vào cấu hình ) </summary>
        [Description("Chính Thức")]
        GETFINAL
    }

    public enum StateTracking
    {
        Modified, Added, Deleted, Logout, Login
    }
    public enum Modules
    {
        Hr,
        Rec,
        Att,
        Can,
        Cat,
        Eva,
        Fin,
        Ins,
        Lau,
        Sal,
        Sys,
    }
    public enum RequirementTrainStatus
    {
        #region RequirementTrainDetailStatus
        [Description("Yêu Cầu")]
        E_SUBMIT,
        [Description("Đang Được Phê Duyệt")]
        E_BEINGAPPROVED,
        [Description("Duyệt")]
        E_APPROVED,
        [Description("Từ Chối")]
        E_REJECTED
        #endregion
    }

    public enum PlanStatus
    {
        #region PlanStatus
        [Description("Chờ Duyệt")]
        E_WAITING,
        [Description("Chờ Xử Lý")]
        E_PENDING,
        [Description("Đang Thực Hiện")]
        E_DOING,
        [Description("Hoàn Thành")]
        E_DONE
        #endregion
    }
    public enum SalesTypeGroup
    {
        [Description("SaleIn")]
        E_SaleIn,
        [Description("SaleOut")]
        E_SaleOut,
        [Description("4S")]
        E_4S
    }

    // Luong Cu
    public enum PayrollElemType
    {
        #region Thong tin nghi bu
        /// <summary>
        /// Nghi bu
        /// </summary>
        TIMEOFF,
        /// <summary>
        /// Số ngày nghỉ bù đầu tháng
        /// </summary>
        TIMEOFF_BEGIN_MONTH,
        /// <summary>
        /// Số ngày nghỉ bù phat sinh trong thang
        /// </summary>
        TIMEOFF_REG_INMONTH,
        /// <summary>
        /// Số ngày nghỉ bù su dung trong thang
        /// </summary>
        TIMEOFF_USE_INMONTH,
        /// <summary>
        /// Số ngày nghỉ bù con lai trong thang
        /// </summary>
        TIMEOFF_FINISH_MONTH,
        #endregion

        #region Thông tin lương

        /// <summary>
        /// Tổng nhận
        /// </summary>
        TOTAL_EARNING,
        /// <summary>
        /// Tổng nhận đã trừ trễ sớm
        /// </summary>
        TOTAL_EARNING_DEDUCT_LATE_EARLY,
        /// <summary>
        /// Ghi chú
        /// </summary>
        NONE,
        /// <summary>
        /// Phi công đoàn công ty trả
        /// </summary>
        UNION_COM,
        /// <summary>
        /// Bảo hiểm y tế người nước ngoài nộp
        /// </summary>
        INSURANCE_MEDICAL_EMP_FOREIGNER,
        /// </summary>
        /// Thuế TNCN được miễn giảm
        /// </summary>
        PIT_EXE_EMP,
        /// </summary>
        /// Chi phí lương
        /// </summary>
        COST_SALARY,
        /// Chi phí lương người NN
        /// </summary>
        COST_SALARY_FOREIGNER,
        /// Chi phí lương người VN
        /// </summary>
        COST_SALARY_VN,
        /// </summary>
        /// Thuế TNCN người VN đóng
        /// </summary>
        PIT_EMP_VN,
        /// BHYT người VN đóng
        /// </summary>
        INSURANCE_MEDICAL_EMP_VN,
        /// Thuế TNCN người VN được giảm
        /// </summary>
        PIT_EXE_EMP_VN,
        /// Thuế TNCN người NN được giảm
        /// </summary>
        PIT_EXE_EMP_FOREIGNER,
        /// </summary>
        /// Thuế TNCN người NN đóng
        /// </summary>
        PIT_EMP_FOREIGNER,
        /// <summary>
        /// Lương cơ bản
        /// </summary>
        BASIC_SALARY,
        /// <summary>
        /// Ngày hiệu lực 
        /// </summary>
        BASIC_SALARY_DATE,
        /// <summary>
        /// Lương cơ bản sau khi điều chỉnh
        /// </summary>
        BASIC_SALARY2,
        /// <summary>
        /// Ty gia tinh luong
        /// </summary>
        EXCHANGE_PAYROLL_,
        /// <summary>
        /// Ty gia tinh BHXH
        /// </summary>
        EXCHANGE_INS_,

        /// <summary>
        /// Ty gia ngoai te 1
        /// </summary>
        EXCHANGE_RATE_BASIC1,
        /// <summary>
        /// Ty gia ngoai te 2
        /// </summary>
        EXCHANGE_RATE_BASIC2,
        /// <summary>
        /// Ngày hiệu lực mức lương sau khi điều chỉnh
        /// </summary>
        BASIC_SALARY_DATE2,
        /// <summary>
        /// Lương Gross
        /// </summary>
        GROSS_SALARY,
        /// <summary>
        /// Lương Gross sau khi điều chỉnh
        /// </summary>
        GROSS_SALARY1,
        /// <summary>
        /// Luong thoi gian lam viec sau khi da tru tre som
        /// </summary>
        SALARY_TIME_DEDUCTION_LATE_EARLY,
        /// <summary>
        /// Lấy PC trả pay back do tăng lương
        /// </summary>
        BACKPAY_SALARY,
        /// <summary>
        /// Tong tru
        /// </summary>
        TOTAL_DEDUCTION,
        /// <summary>
        /// Tong nhan
        /// </summary>
        TOTAL_ALLOWANCE,
        /// <summary>
        /// Tong tru ko tính trễ smớ
        /// </summary>
        TOTAL_DEDUCTION_EXCLUDE_LATE_EARLY_DEDUCTION,
        /// <summary>
        /// Loai tien
        /// </summary>
        CURRENCY,
        /// <summary>
        /// Lay PC tru tu thang trc
        /// </summary>
        PREVIOUSMONTH_SUBTRACTALLOWANCE,
        /// <summary>
        /// He số lương cá nhân
        /// </summary>
        PERSONAL_RATE,
        /// <summary>
        /// lương nhân viên theo hệ số & ngày công
        /// </summary>
        FUND_SALARY,
        /// <summary>
        /// lương nhân viên theo hệ số & ngày công 2
        /// </summary>
        FUND_SALARY2,
        /// <summary>
        /// số tiền làm trên 1 h của quỹ lương 1
        /// </summary>
        FUND_SALARY_RATE1,
        /// <summary>
        /// số tiền làm trên 1 h của quỹ lương 2
        /// </summary>
        FUND_SALARY_RATE2,
        /// <summary>
        /// tổng cộng lương nhân viên theo hệ số & ngày công 
        /// </summary>
        TOTAL_FUND_SALARY,
        /// <summary>
        /// Muc luong binh quan 6 thang truoc khi nghi
        /// </summary>
        AVERAGE_SALARY_6MONTH,
        /// <summary>
        /// Thoi gian lam viec tai cong ty (Ngay vao lam - ngay nghi viec)
        /// </summary>
        WORKING_TIME_COMPANY,
        /// <summary>
        /// Tong thoi gian lam viec (?nam ? thang)
        /// </summary>
        TOTAL_WORKING_TIME_COMPANY,
        /// <summary>
        /// Số ngày đã nghỉ việc trong tháng tính lương
        /// </summary>
        TOTAL_DAYS_QUIT_COMPANY,
        /// <summary>
        /// Số ngày chưa vào công ty trong tháng tính lương
        /// </summary>
        TOTAL_DAYS_NOT_HIRE_COMPANY,
        #endregion

        #region Thông tin công
        /// <summary>
        /// Ngày công chuẩn
        /// </summary>
        STANDARD_WORKDAY,
        /// <summary>
        /// Ngày công chuẩn trước điều chỉnh (Không trừ trễ sớm & ngày nghỉ)
        /// </summary>
        /// 
        STANDARD_WORKDAY1,

        /// <summary>
        /// Ngày công chuẩn sau điều chỉnh (Không trừ trễ sớm & ngày nghỉ)
        /// </summary>
        STANDARD_WORKDAY2,

        /// <summary>
        /// Ngày công chuẩn tính tăng ca
        /// </summary>
        STANDARD_WORKDAY_OT,
        /// <summary>
        /// Ngày công chuẩn tính theo quy tắt Fujikura
        /// </summary>
        WORKDAY_AVERAGE_PER_MONTH,
        /// <summary>
        /// Giờ công chuẩn
        /// </summary>
        STANDARD_HOURS,

        /// <summary>
        /// Giờ công thực tế
        /// </summary>
        REAL_WORKINGHOURS,
        /// <summary>
        /// Giờ công trong 1 ngày
        /// </summary>
        HOUR_PER_DAY,
        /// <summary>
        /// Ngày công thực tế
        /// </summary>
        REAL_WORKDAY,
        /// <summary>
        /// Số giờ trừ trễ sớm thuc te. Truong hop Grade khong tinh tre som nhung van lay ra so gio tre som.
        /// </summary>
        REAL_LATE_EARLY_HOURS,
        /// <summary>
        /// Ngày công thực tế (count )
        /// </summary>
        REALDAY_COUNT,
        /// <summary>
        /// Ngày công thực tế sau khi điều chỉnh lương
        /// </summary>
        REAL_WORKDAY2,
        /// <summary>
        /// Ngày công thực tế trước khi trừ trễ sớm
        /// </summary>
        REAL_WORKDAY_SUBTRACT_LATE_EARLY_HOURS,
        /// <summary>
        /// Ngày công thực tế sau khi trừ trễ sớm
        /// </summary>
        REAL_WORKDAY_SUBTRACT_LATE_EARLY_HOURS2,
        /// <summary>
        /// Lương ngày
        /// </summary>
        DAILY_RATE,
        /// <summary>
        /// Lương ngày trước điều chỉnh hiển thị trong payslip
        /// </summary>
        DAILY_RATE1,
        /// <summary>
        /// Lương ngày sau điều chỉnh hiển thị trong payslip
        /// </summary>
        DAILY_RATE2,
        /// <summary>
        /// Lương giờ
        /// </summary>
        HOURLY_RATE,
        /// <summary>
        /// Lương giờ trc khi điều chỉnh hiển thị trong payslip
        /// </summary>
        HOURLY_RATE1,
        /// <summary>
        /// Lương giờ sau điều chỉnh hiển thị trong payslip
        /// </summary>
        HOURLY_RATE2,
        /// <summary>
        /// Tổng phép năm hiện tại
        /// </summary>
        TOTAL_ANNUAL_LEAVE,
        /// <summary>
        /// Tổng Số ngày nghi phép năm trong tháng
        /// </summary>
        TOTAL_ANNUALLEAVE_DAYS,
        /// <summary>
        /// Tổng Số ngày nghi phép năm cho đến tháng tính lương
        /// </summary>
        TOTAL_LEAVE_TO_COM_PAYROLL,
        /// <summary>
        /// Tổng ngày phép năm còn lại cho đến tháng 31/12
        /// </summary>
        TOTAL_ANNUAL_TO_END_YEAR,
        /// <summary>
        /// Tổng ngày phép năm còn lại cho đến tháng tinh luong
        /// </summary>
        TOTAL_ANNUAL_TO_END_MONTH,
        /// <summary>
        /// Số ngày nghi phép năm trong tháng trước khi điều chỉnh lương
        /// </summary>
        ANNUALLEAVE_DAY,

        /// <summary>
        /// Số ngày nghỉ phép năm trong tháng sau khi điều chỉnh lương
        /// </summary>
        ANNUALLEAVE_DAY2,

        /// <summary>
        /// Phép năm còn lại -> unchecked 4 CPG
        /// </summary>
        BALANCE_LEAVE,

        /// <summary>
        /// Số ngày nghi Holiday trong tháng
        /// </summary>
        HOLIDAYLEAVE_DAY,
        /// <summary>
        /// Số ngày nghi Holiday trong tháng 1
        /// </summary>
        HOLIDAYLEAVE_DAY1,

        /// <summary>
        /// Số ngày nghi Holiday trong tháng 2
        /// </summary>
        HOLIDAYLEAVE_DAY2,

        /// <summary>
        /// Ngày công tính lương trước khi điều chỉnh lương
        /// </summary>
        WORKING_PAIDLEAVE_DAY,

        /// <summary>
        /// Ngày công tính lương sau khi điều chỉnh lương
        /// </summary>
        WORKING_PAIDLEAVE_DAY2,


        /// <summary>
        /// Ngày công tính lương đã trừ trễ sớm trước khi điều chỉnh lương
        /// </summary>
        WORKING_PAIDLEAVE_DEDUCTION_LATE_EARLY_DAY,
        /// <summary>
        /// Ngày công tính lương đã trừ trễ sớm truoc khi điều chỉnh lương
        /// </summary>
        WORKING_PAIDLEAVE_DEDUCTION_LATE_EARLY_DAY1,
        /// <summary>
        /// Ngày công tính lương đã trừ trễ sớm sau khi điều chỉnh lương
        /// </summary>
        WORKING_PAIDLEAVE_DEDUCTION_LATE_EARLY_DAY2,
        /// <summary>
        /// Giờ công tính lương đã trừ trễ sớm trước khi điều chỉnh lương
        /// </summary>
        WORKING_PAIDLEAVE_DEDUCTION_LATE_EARLY_HOURS,
        /// <summary>
        /// Giờ công tính lương đã trừ trễ sớm trước khi điều chỉnh lương
        /// </summary>
        WORKING_PAIDLEAVE_DEDUCTION_LATE_EARLY_HOURS1,

        /// <summary>
        ///  Lương ngày công thực tế
        /// </summary>       
        WORKING_SALARY,
        /// <summary>
        ///  Lương sản phẩmproductionSalary
        /// </summary>       
        PRODUCTION_SALARY,
        /// <summary>
        /// Lương thời gian
        /// </summary>           
        WORKING_PAIDLEAVE_SALARY,

        /// <summary>
        /// Tổng Số ngày nghỉ hưởng lương
        /// </summary>
        TOTAL_PAID_LEAVE_DAY,

        /// <summary>
        /// Tổng Số giờ nghỉ hưởng lương
        /// </summary>
        TOTAL_PAID_LEAVE_HOUR,

        /// <summary>
        /// Số ngày nghỉ hưởng lương trước khi điều chỉnh
        /// </summary>
        PAID_LEAVE_DAY,

        /// <summary>
        /// Số ngày nghỉ hưởng lương sau khi điều chỉnh lương
        /// </summary>
        PAID_LEAVE_DAY2,

        /// <summary>
        /// Số ngày nghỉ không phép không báo
        /// </summary>
        ABS_LEAVE_DAY,

        /// <summary>
        /// Số ngày nghỉ không phép không báo sau khi điều chỉnh lương
        /// </summary>
        ABS_LEAVE_DAY2,

        /// <summary>
        /// Số ngày nghỉ hưởng lương (Trừ ngày phép năm)
        /// </summary>
        PAID_LEAVE_DAY_SUBTRACT_ANNUAL_LEAVE,

        /// <summary>
        /// Số ngày nghỉ hưởng lương (Trừ ngày phép năm) sau khi điều chỉnh lương
        /// </summary>
        PAID_LEAVE_DAY_SUBTRACT_ANNUAL_LEAVE2,

        /// <summary>
        /// Lương ngày nghỉ
        /// </summary>
        LEAVE_SALARY,

        /// <summary>
        /// Số ngày nghỉ không lương -> unchecked 4 CPG
        /// </summary>
        UNPAID_LEAVE_DAYS,

        /// <summary>
        /// Số ngày nghỉ không lương do không làm ot đủ
        /// </summary>
        UNPAID_LEAVE_DAYS_BY_COMPENSATION,

        /// <summary>
        /// Số ngày nghỉ không lương OCPS
        /// </summary>
        LEAVE_DAYS_BY_COMPENSATION_OCPS,
        /// <summary>
        /// Số ngày nghỉ không lương PARTY
        /// </summary>
        LEAVE_DAYS_BY_COMPENSATION_PARTY,

        /// <summary>
        /// Số ngày nghỉ không lương do lien hoang nhung phai tang ca lam bu
        /// </summary>
        UNPAID_LEAVE_DAYS_BY_PARTY_HAS_COMPENSATION,
        /// <summary>
        /// Tổng ngày nghỉ bù do tăng ca
        /// </summary>
        TOTAL_LEAVE_COMPENSATION_BY_OT,

        /// <summary>
        /// Số giờ nghỉ không lương 
        /// </summary>
        UNPAID_LEAVE_HOUR,

        /// <summary>
        /// hien.pham 20120621
        /// Số giờ nghỉ không lương trước điều chỉnh lương
        /// </summary>
        UNPAID_LEAVE_HOUR1,
        /// <summary>
        /// hien.pham 20120621
        /// Số giờ nghỉ không lương sau điều chỉnh lương
        /// </summary>
        UNPAID_LEAVE_HOUR2,
        /// <summary>
        /// hien.pham 20120621
        /// Trừ lương ngày nghỉ -> unchecked 4 CPG
        /// </summary>
        UNPAID_LEAVE_SALARY,
        /// <summary>
        /// hien.pham 20120621
        /// Trừ lương ngày nghỉ trước điều chỉnh
        /// </summary>
        UNPAID_LEAVE_SALARY1,
        /// <summary>
        /// hien.pham 20120621
        /// Trừ lương ngày nghỉ sau điều chỉnh
        /// </summary>
        UNPAID_LEAVE_SALARY2,

        /// <summary>
        /// Ngày nghỉ được hưởng lương theo ti lệ
        /// </summary>
        PAID_LEAVE_DAY_BY_RATE,
        /// <summary>
        /// Thiết lập tham số tính so ngay di lam co' gio quet the Out - In >= 8h/ngay
        /// </summary>
        INOUTDAY_UPPER_8HOURS_EXCLUDE_HOLIDAYANDWEEKEND_COUNT,
        /// <summary>
        /// Prefix, for future used
        /// </summary>
        LEAVEDAY,
        /// <summary>
        /// Prefix, for future used
        /// </summary>
        OVERTIME,
        /// <summary>
        /// Phụ cấp ca đêm
        /// </summary>
        NIGHT_SHIFT_ALLOWANCE,
        /// <summary>
        /// Giờ làm ca đêm
        /// </summary>
        NIGHT_SHIFT_HOURS,
        /// <summary>
        /// Giờ làm ca đêm thuc te trong ca
        /// </summary>
        NIGHT_SHIFT_INSHIFT_HOURS,
        /// <summary>
        /// Tiền tăng ca
        /// </summary>
        OVERTIME_SALARY,

        /// <summary>
        /// Tiền tăng ca cua quy luong 2
        /// </summary>
        OVERTIME_SALARY2,
        /// <summary>
        /// Giờ tăng ca ngày thường
        /// </summary>
        OVERTIME_WORKDAY_HOURS,
        /// <summary>
        /// Tiền tăng ca ngày thường
        /// </summary>
        OVERTIME_WORKDAY_AMOUNT,
        /// <summary>
        /// Giờ tăng ca ngày thường ca đêm
        /// </summary>
        OVERTIME_WORKDAY_NIGHTSHIFT_HOURS,
        /// <summary>
        /// Tiền tăng ca ngày thường ca đêm
        /// </summary>
        OVERTIME_WORKDAY_NIGHTSHIFT_AMOUNT,
        /// <summary>
        /// Giờ tăng ca ngày nghỉ
        /// </summary>
        OVERTIME_WEEKEND_HOURS,
        /// <summary>
        /// Tiền tăng ca ngày nghỉ
        /// </summary>
        OVERTIME_WEEKEND_AMOUNT,
        /// <summary>
        /// Giờ tăng ca ngày nghỉ ca đêm
        /// </summary>
        OVERTIME_WEEKEND_NIGHTSHIFT_HOURS,
        /// <summary>
        /// Giờ tăng ca ngày nghỉ ca đêm
        /// </summary>
        OVERTIME_WEEKEND_NIGHTSHIFT_AMOUNT,
        /// <summary>
        /// Giờ tăng ca ngày lễ
        /// </summary>
        OVERTIME_HOLIDAY_HOURS,
        /// <summary>
        /// Tiền tăng ca ngày lễ
        /// </summary>
        OVERTIME_HOLIDAY_AMOUNT,
        /// <summary>
        /// Giờ tăng ca ngày lễ ca đêm
        /// </summary>
        OVERTIME_HOLIDAY_NIGHTSHIFT_HOURS,
        /// <summary>
        /// Tiền tăng ca ngày lễ ca đêm
        /// </summary>
        OVERTIME_HOLIDAY_NIGHTSHIFT_AMOUNT,
        /// <summary>
        /// Số giờ tăng ca ngày nghỉ vượt số giờ công chuẩn
        /// </summary>
        OVERTIME_DAYOFF_OVER_STANDARDHOURS_HOURS,
        /// <summary>
        /// Số giờ làm bù
        /// </summary>
        COMPENSATION_HOURS,
        /// <summary>
        /// Tiền làm bù
        /// </summary>
        COMPENSATION_AMOUNT,
        /// <summary>
        /// Khoản tăng ca kô tính thuế
        /// </summary>
        OTSALARY_NON_TAX,
        /// <summary>
        /// Khoản tăng ca kô tính thuế quy luong 2
        /// </summary>
        OTSALARY_NON_TAX2,
        /// <summary>
        /// Tong cac khoan khong tinh thue bao gom OT
        /// </summary>
        TOTAL_NON_TAX,
        /// <summary>
        /// Số giờ làm thêm ngày chủ nhật quá 8 giờ
        /// </summary>
        OT_SUNDAY_HOLIDAY_GREATER_8H_HOURS,
        /// <summary>
        /// Số giờ làm thêm ngày thứ 7 không làm việc quá 8 giờ
        /// </summary>
        OT_SATURDAY_GREATER_8H_HOURS,
        /// <summary>
        /// Prefix cho phụ cấp No-Gross monthly hay unusual: [ALLOWANCE]_[Allowance Code]
        /// </summary>
        ALLOWANCE,
        /// <summary>
        /// Prefix cho khoản trừ monthly hay unusual: [DEDUCTION]_[Allowance Code]
        /// </summary>
        DEDUCTION,
        /// <summary>
        /// Trừ trễ sớm trước khi điều chỉnh lương
        /// </summary>
        DEDUCTION_LATE_EARLY_AMOUNT,
        /// <summary>
        /// Số giờ trừ trễ sớm trước khi điều chỉnh lương
        /// </summary>
        DEDUCTION_LATE_EARLY_HOURS,
        /// <summary>
        /// Trừ trễ sớm sau khi điều chỉnh lương
        /// </summary>
        DEDUCTION_LATE_EARLY_AMOUNT2,
        /// <summary>
        /// Số giờ trừ trễ sớm sau khi điều chỉnh lương
        /// </summary>
        DEDUCTION_LATE_EARLY_HOURS2,
        /// <summary>
        /// Phạt ngày nghỉ
        /// </summary>
        DEDUCTION_LEAVE_PENALTY,
        /// <summary>
        /// Số giờ OT > 8H trên ngày
        /// </summary>
        WORKING_OT_HOURS_UPPER_8H,
        /// <summary>
        /// Số giờ OT ngay nghi > 6H 
        /// </summary>
        OT_DAYOFF_HOLIDAY_WORK_GREATER_6HOURS_COUNT,
        #endregion

        #region Thông tin bảo hiển
        /// <summary>
        /// Prefix of Insurance for future used
        /// </summary>
        INSURANCE,
        /// <summary>
        /// Lương đóng BHXH
        /// </summary>
        INSURANCE_SALARY,
        /// <summary>
        /// Lương đóng BHXH trước điều chỉnh
        /// </summary>
        INSURANCE_SALARY1,
        /// <summary>
        /// Lương đóng BHXH sau điều chỉnh
        /// </summary>
        INSURANCE_SALARY2,
        /// <summary>
        /// Ty gia BHXH 1
        /// </summary>
        EXCHANGE_RATE_INSURANCE1,
        /// <summary>
        /// Ty gia BHXH 2
        /// </summary>
        EXCHANGE_RATE_INSURANCE2,



        /// <summary>
        /// Lương Theo sản phẩm
        /// </summary>
        PRODUCTS_SALARY,
        /// <summary>
        /// Tổng BH nhân viên đóng
        /// </summary>
        TOTAL_INSURANCE_EMP,
        /// <summary>
        /// Tổng BH công ty đóng
        /// </summary>
        TOTAL_INSURANCE_COM,
        /// <summary>
        /// BHXH nhân viên đóng
        /// </summary>
        INSURANCE_SOCIAL_EMP,
        /// <summary>
        /// BHXH công ty đóng
        /// </summary>
        INSURANCE_SOCIAL_COM,
        /// <summary>
        /// BHYT nhân viên đóng
        /// </summary>
        INSURANCE_MEDICAL_EMP,
        /// <summary>
        /// BHYT công ty đóng
        /// </summary>
        INSURANCE_MEDICAL_COM,
        /// <summary>
        /// BH thất nghiệp nhân viên đóng
        /// </summary>
        INSURANCE_UNEMPLOY_EMP,
        /// <summary>
        /// BH thất nghiệp nhân viên đóng
        /// </summary>
        INSURANCE_UNEMPLOY_COM,
        /// <summary>
        /// BHYT Nguoi nuoc ngoai
        /// </summary>
        INSURANCE_HEALTH_ROREIGNERS_EMP,
        /// <summary>
        /// BHYT Nguoi nuoc ngoai cty dong
        /// </summary>
        INSURANCE_HEALTH_ROREIGNERS_COM,
        /// <summary>
        /// Ngày đóng bảo hiểm tai nạn
        /// </summary>
        INSURANCE_ACCIDENT_DATE,
        /// <summary>
        /// Thu nhập trước thuế
        /// </summary>
        INCOME_BEFORE_TAX,
        /// <summary>
        /// Tổng Thu nhập truoc thue = thu nhap tinh thue + bhxh + giam tru (ap dung  cach tinh nguoc lai - Massan)
        /// </summary>
        INCOME_BEFORE_TAX_FROMINCOMETAX,
        /// <summary>
        /// Tổng Thu nhập tính thuế trước giảm trừ gia cảnhAmountDependant
        /// </summary>
        INCOME_BEFORE_TAX_DEPENDANT,
        /// <summary>
        /// Số người phụ thuộc
        /// </summary>
        DEPENDANT_NO,
        /// <summary>
        /// Giảm trừ người phụ thuộc (ko bao gồm bản thân)
        /// </summary>
        DEPENDANT_AMOUNT_NON_PERSONEL,
        /// <summary>
        /// Giảm trừ bản thân & gia cảnh
        /// </summary>
        DEPENDANT_AMOUNT,

        /// <summary>
        /// Giảm trừ bản thân
        /// </summary>
        PERSONEL_AMOUNT,
        /// <summary>
        /// Thu nhập chịu thuế
        /// </summary>
        INCOME_TAXABLE,
        /// <summary>
        /// Thu nhập chịu thuế co so am
        /// </summary>
        INCOME_TAXABLE_LESS_ZERO,
        /// <summary>
        /// Thuế P.I.T nhân viên đóng
        /// </summary>
        PIT_EMP,
        /// <summary>
        /// Thực nhận
        /// </summary>
        NET_TAKE_HOME,
        /// <summary>
        /// Thuế P.I.T cty đóng
        /// </summary>
        PIT_COM,
        /// <summary>
        /// Thu nhập NET
        /// </summary>
        INCOME_NET,
        /// <summary>
        /// Thu nhập NET la tron 3 so
        /// </summary>
        INCOME_NET_ROUND,
        /// <summary>
        /// Ứng lương
        /// </summary>
        ADVANCE_PAY,
        /// <summary>
        /// Thu nhập không chịu thuế
        /// </summary>
        AdditionNoTax,
        /// <summary>
        /// Khoản trừ không chịu thuế
        /// </summary>
        DeductNoTax,
        /// <summary>
        /// Tiền thưởng (ko chịu thuế)
        /// </summary>
        BONUS,
        /// <summary>
        /// Cách tính lương (cộng/trừ)
        /// </summary>
        COMPUTING_METHOD,
        /// <summary>
        /// Khoản chênh lệch giữa Phụ cấp trừ của tháng trước và lương đc nhận
        /// Khoản này sẽ được lưu lại và trừ tiếp vào tháng sau
        /// </summary>
        GAP_SUBTRACT_PRE_ALLOWANCE_SALARY,
        #endregion
    }

    //Loại dữ liệu backup
    public enum TypeDataBKInScheduleTask
    {
        //Email
        E_EMAIL_BK,
        //Lương
        E_PAYROLL_BK,
        //Công
        E_ATTENDANCE_BK
    }

    //Loại dữ liệu backup
    public enum DelegateApproveType
    {
        //Tăng ca
        E_OVERTIME,
        //Ngày Nghỉ
        E_LEAVE_DAY,
        //Ca Làm Việc
        E_ROSTER
    }

    public enum CompanyName
    {
        E_BDF,
        E_FASHIONGARMENT,
    }

    //Loại này để tính lương bộ phận cho kiểu nhân viên hoặc kiểu phòng ban
    public enum TypeEmployeeOrTeamInSalDepartmentItem
    {
        [Description("Là Nhân Viên")]
        E_IS_EMPLOYEE,
        [Description("Là Nhóm/Phòng Ban")]
        E_IS_TEAM
    }

    public enum MealRecordMissingStatus
    {
        E_APPROVED,
        E_WAIT_APPROVED
    }
}
