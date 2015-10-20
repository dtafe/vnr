namespace HRM.Infrastructure.Utilities
{
    /// <summary>
    /// Sử dụng cho mục đích tạo ra biến không thay đổi cho thông báo
    /// vd: 
    /// </summary>
    public class ConstantAction
    {

        #region General profile
        /// <summary>
        /// Sử dụng trường hợp thông báo sử dụng chung
        /// vd: Lưu thành công.....
        /// </summary>
        ///  
        public const string HRM_Common_RegisterOut = "HRM_Common_RegisterOut";
        public const string HRM_Common_Create_UpdateEvaContract = "HRM_Common_Create_UpdateEvaContract"; 
        public const string HRM_Common_ExtendContract = "HRM_Common_ExtendContract"; 
        public const string HRM_Common_OutOfBlackList = "HRM_Common_OutOfBlackList"; 
        public const string HRM_Common_ComebackToCandidate = "HRM_Common_ComebackToCandidate";
        public const string HRM_Common_CreateContract = "HRM_Common_CreateContract"; 
        public const string HRM_Common_Submit = "HRM_Common_Submit"; 
        public const string HRM_Common_UpdateFileCandidate = "HRM_Common_UpdateFileCandidate";
        public const string HRM_Common_UpdateBasicSalary = "HRM_Common_UpdateBasicSalary";
        public const string HRM_Common_UpdateRank = "HRM_Common_UpdateRank";
        public const string HRM_Common_UpdateRankDetail = "HRM_Common_UpdateRankDetail";
        public const string HRM_Common_Call = "HRM_Common_Call";
        public const string HRM_Common_WaitingGetWork = "HRM_Common_WaitingGetWork";
        public const string HRM_Common_Active = "HRM_Common_Active";
        public const string HRM_Common_Cancel = "HRM_Common_Cancel";
        public const string HRM_Common_ViewDetail = "HRM_Common_ViewDetail";
        public const string HRM_Common_Calculate = "HRM_Common_Calculate";
        public const string HRM_Common_Add = "HRM_Common_Add";
        public const string HRM_Common_Add_Topic = "HRM_Common_Add_Topic";
        public const string HRM_Common_Add_ScoreType = "HRM_Common_Add_ScoreType";
        public const string HRM_Common_Save = "HRM_Common_Save";
        public const string HRM_Common_SaveDetail = "HRM_Common_SaveDetail";
        public const string HRM_Common_ImportAndSave = "HRM_Common_ImportAndSave";
        public const string HRM_Common_SaveConfig = "HRM_Common_SaveConfig";
        public const string HRM_Common_Edit = "HRM_Common_Edit";
        public const string HRM_Common_Search = "HRM_Common_Search";
        public const string HRM_Common_check = "HRM_Common_check";
        public const string HRM_Common_Commander = "HRM_Common_Commander";
        public const string HRM_Common_ComputeBonus = "HRM_Common_ComputeBonus";
        public const string HRM_Common_MenuOption = "HRM_Common_MenuOption";
        
        public const string HRM_Common_CreateRequestTrain = "HRM_Common_CreateRequestTrain";
        public const string HRM_Common_Payment = "HRM_Common_Payment";
        public const string HRM_Common_Run_ScheduleTask = "HRM_Common_Run_ScheduleTask";
        public const string HRM_Common_Start_ScheduleTask = "HRM_Common_Start_ScheduleTask";
        public const string HRM_Common_AddNewRoster = "HRM_Common_AddNewRoster";
        public const string HRM_Common_Confirm = "HRM_Common_Confirm";
        public const string HRM_Common_ComputeAndConfirm = "HRM_Common_ComputeAndConfirm";
        public const string HRM_Common_Compute = "HRM_Common_Compute";
        public const string HRM_Common_Compute_HoldSalary = "HRM_Common_Compute_HoldSalary";
        public const string HRM_Common_HoldSalary = "HRM_Common_HoldSalary";
        public const string HRM_Common_LoadData = "HRM_Common_LoadData";
        public const string HRM_Common_Create = "HRM_Common_Create";
        public const string HRM_Common_Create_LockObject = "HRM_Common_Create_LockObject";
        public const string HRM_Common_ConfigIns = "HRM_Common_ConfigIns";
        public const string HRM_Common_AddListTrainee = "HRM_Common_AddListTrainee";
        public const string HRM_Common_TranferTrainee = "HRM_Common_TranferTrainee";
        public const string HRM_Common_ComputePoint = "HRM_Common_ComputePoint";
        public const string HRM_Common_Create_Certificate = "HRM_Common_Create_Certificate";
        public const string HRM_Common_Edit_Certificate = "HRM_Common_Edit_Certificate";
        public const string HRM_Common_Remove_Certificate = "HRM_Common_Remove_Certificate";
        public const string HRM_Common_Create_Form = "HRM_Common_Create_Form";
        public const string HRM_Common_Analysis = "HRM_Common_Analysis";
        public const string HRM_Common_Analysis_Estimate = "HRM_Common_Analysis_Estimate";

        public const string HRM_Common_CreateAppendixContract = "HRM_Common_CreateAppendixContract";
        public const string HRM_Common_Export = "HRM_Common_Export";
        public const string HRM_Common_ExportWord = "HRM_Common_ExportWord";
        public const string HRM_Common_ExportAll = "HRM_Common_ExportAll";
        public const string HRM_Common_ExportWordAll = "HRM_Common_ExportWordAll";
        public const string HRM_Common_ExportSelected = "HRM_Common_ExportSelected";
        public const string HRM_Common_FilterCandidate = "HRM_Common_FilterCandidate";
        public const string HRM_Common_ExportByTemplate = "HRM_Common_ExportByTemplate";
        public const string HRM_Common_PDF = "HRM_Common_PDF";
        public const string HRM_Common_ExportByTemplateID = "HRM_Common_ExportByTemplateID";
        public const string HRM_Common_SettlementNumer = "HRM_Common_SettlementNumer";
        public const string HRM_Common_ExportAllByTemplate = "HRM_Common_ExportAllByTemplate";
        public const string HRM_Common_Create_ContractAndNextBasicSalary = "HRM_Common_Create_ContractAndNextBasicSalary";
        public const string HRM_Common_Create_EvaContract = "HRM_Common_Create_EvaContract";
        public const string HRM_Common_Import = "HRM_Common_Import";
        public const string HRM_Common_Pivot = "HRM_Common_Pivot";
        public const string HRM_Common_Delete = "HRM_Common_Delete";
        public const string HRM_Common_Refesh = "HRM_Common_Refesh";
        public const string HRM_Common_SaveContinue = "HRM_Common_SaveContinue";
        public const string HRM_Common_SaveClose = "HRM_Common_SaveClose";
        public const string HRM_Common_Close = "HRM_Common_Close";
        public const string HRM_Common_SaveAndApply = "HRM_Common_SaveAndApply";
        public const string HRM_Common_SaveNew = "HRM_Common_SaveNew";
        public const string HRM_Common_SaveAndForward = "HRM_Common_SaveAndForward";
        public const string HRM_Common_Info_Persional = "HRM_Common_Info_Persional";
        public const string HRM_Common_Info_Attendance = "HRM_Common_Info_Attendance";
        public const string HRM_Common_Info_Payroll = "HRM_Common_Info_Payroll";
        public const string HRM_Common_ComputeAttendance = "HRM_Common_ComputeAttendance";
        public const string HRM_Common_DownLoad = "HRM_Common_DownLoad";
        public const string HRM_Common_ExportReport = "HRM_Common_ExportReport";
        public const string HRM_Common_Approve = "HRM_Common_Approve";
        public const string HRM_Common_CheckData = "HRM_Common_CheckData";

        public const string HRM_Common_Sal_ProfileNotApplyGrade_Apply = "HRM_Common_Sal_ProfileNotApplyGrade_Apply";
        public const string HRM_Common_Apply = "HRM_Common_Apply";
        public const string HRM_Common_ApplyPerformance = "HRM_Common_ApplyPerformance";
        public const string HRM_Common_ApproveAll = "HRM_Common_ApproveAll";
        public const string HRM_ComputeResult = "HRM_ComputeResult";
        public const string HRM_Common_UpdateWorkingPosition = "HRM_Common_UpdateWorkingPosition";
        public const string HRM_Common_UpdateAppendixContract = "HRM_Common_UpdateAppendixContract";
        public const string HRM_Common_DefaultConfig = "HRM_Common_DefaultConfig";
        public const string HRM_Common_ImportData = "HRM_Common_ImportData";

        public const string HRM_Common_Approve_Profile = "HRM_Common_Approve_Profile";
        public const string HRM_Common_Appointment = "HRM_Common_Appointment";
        public const string HRM_Common_AddToBlackList = "HRM_Common_AddToBlackList";
        public const string HRM_Common_ChangeMethodOvertime = "HRM_Common_ChangeMethodOvertime";
        public const string HRM_Common_ChangeMethodOvertimeLeave = "HRM_Common_ChangeMethodOvertimeLeave";
        public const string HRM_Common_Reject = "HRM_Common_Reject";
        public const string HRM_Common_Approved = "HRM_Common_Approved";
        public const string HRM_Common_Waitting = "HRM_Common_Waitting";
        public const string HRM_Common_Rejected = "HRM_Common_Rejected";
        public const string HRM_Common_SendRequest = "HRM_Common_SendRequest";
        public const string HRM_Common_SendRequest_Excel = "HRM_Common_SendRequest_Excel";
        public const string HRM_Common_SendRequest_PDF = "HRM_Common_SendRequest_PDF"; 
        public const string HRM_Common_Copy = "HRM_Common_Copy";
        public const string HRM_Common_AllowOvertime = "HRM_Common_AllowOvertime";
        public const string HRM_Common_DonotOvertime = "HRM_Common_DonotOvertime";
        public const string HRM_Common_NoCalculateOvertime = "HRM_Common_NoCalculateOvertime";
        public const string HRM_Common_AllowCalculateOvertime = "HRM_Common_AllowCalculateOvertime";
        
        public const string HRM_Common_CollectData = "HRM_Common_CollectData";
        public const string HRM_Common_ChangeMethodPayment_Money = "HRM_Common_ChangeMethodPayment_Money";
        public const string HRM_Common_ChangeMethodPayment_Off = "HRM_Common_ChangeMethodPayment_Off";
        public const string HRM_Common_Compute_LaundryRecord = "HRM_Common_Compute_LaundryRecord";
        public const string HRM_Common_UpdateSumLeaveday = "HRM_Common_UpdateSumLeaveday";

        public const string HRM_Common_Filter_All = "HRM_Common_Filter_All";
        public const string HRM_Common_Filter_DataLimit = "HRM_Common_Filter_DataLimit";
        public const string HRM_Common_Filter_DataNotLimit = "HRM_Common_Filter_DataNotLimit";
        public const string HRM_Common_Excel_PersonalInformation = "HRM_Common_Excel_PersonalInformation";
        //Nút tạo kế hoạch phỏng vấn
        public const string HRM_Common_CreateInterviewCampaign = "HRM_Common_CreateInterviewCampaign";
        // Nút Nhận việc
        public const string HRM_Common_Hire = "HRM_Common_Hire";
        // Nút Từ Chối Nhận Việc 
        public const string HRM_Common_UnHire = "HRM_Common_UnHire";

        public const string HRM_Common_Create_Contract = "HRM_Common_Create_Contract";
        public const string HRM_Common_Create_NextContract = "HRM_Common_Create_NextContract";
        //bảo hiểm

        public const string HRM_Compute_Comission = "HRM_Compute_Comission";
        public const string HRM_ComputeAnnualLeaveDetail = "HRM_ComputeAnnualLeaveDetail";

        public const string HRM_Common_CancelLateEarly = "HRM_Common_CancelLateEarly";
        public const string HRM_Common_UpdateInOut = "HRM_Common_UpdateInOut";
        public const string HRM_Common_ChangeInOut = "HRM_Common_ChangeInOut";
        public const string HRM_Common_ComputeAnnualLeave = "HRM_Common_ComputeAnnualLeave";
        public const string HRM_Common_ComputeCompensation = "HRM_Common_ComputeCompensation";
        public const string HRM_Common_ComputeInsural = "HRM_Common_ComputeInsural";
        public const string HRM_Common_SaveChanges = "HRM_Common_SaveChanges";
        public const string HRM_Common_SaveChanges_Estimate = "HRM_Common_SaveChanges_Estimate";

        public const string HRM_Common_CancelChanges = "HRM_Common_CancelChanges";
        public const string HRM_Common_Cumulative = "HRM_Common_Cumulative";
        public const string HRM_Common_CreateTemplate = "HRM_Common_CreateTemplate";
        public const string HRM_Common_CreateTemplateFormula = "HRM_Common_CreateTemplateFormula";
        public const string HRM_Common_UpdateOrg_Profile = "HRM_Common_UpdateOrg_Profile";

        public const string HRM_Common_Payment_Salary = "HRM_Common_Payment_Salary";
        public const string HRM_Common_Result_Jobcandidate = "HRM_Common_Result_Jobcandidate";
        public const string HRM_Common_Add_Point = "HRM_Common_Add_Point";

        public const string HRM_Common_Eva_Summary = "HRM_Common_Eva_Summary";
        public const string HRM_Common_Eva_LoadData = "HRM_Common_Eva_LoadData";
        public const string HRM_Common_Can_SumryMealRecord = "HRM_Common_Can_SumryMealRecord";
        public const string HRM_Common_BackToWaitInterview = "HRM_Common_BackToWaitInterview";
        public const string HRM_Common_BackToWaitApproved = "HRM_Common_BackToWaitApproved";
        #endregion

    }
}
