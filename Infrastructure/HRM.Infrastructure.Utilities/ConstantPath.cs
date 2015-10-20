

namespace HRM.Infrastructure.Utilities
{
    public static class ConstantPathWeb
    {
        public static string Hrm_Sys_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        public static string Hrm_Sys_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Sys_Service"];

        public static string HrWebUrl = System.Configuration.ConfigurationManager.AppSettings["eWebUrl"];
        public static string Hrm_Hre_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];
        public static string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];       

        public static string Hrm_Cat_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        public static string Hrm_Cat_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];

        public static string Hrm_Hre_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];

        public static string Hrm_Att_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        public static string Hrm_Att_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];

        public static string Hrm_Can_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        public static string Hrm_Can_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];


        public static string Hrm_Lau_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        public static string Hrm_Lau_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];

        public static string Hrm_Sal_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        public static string Hrm_Sal_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];

        public static string Hrm_Eva_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];
        public static string Hrm_Eva_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];

        public static string Hrm_Hr7_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];

        public static string Hrm_Att_Service_Path_Report = System.Configuration.ConfigurationManager.AppSettings["Hrm_Att_Service_Path_Report"];

        public static string Hrm_StaffPortal_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_StaffPortal_Web"];
        public static string Hrm_EmpPortal_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_EmpPortal_Web"];

        public static string Hrm_Ins_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];
        public static string Hrm_Ins_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];

        public static string Hrm_Rec_Service = System.Configuration.ConfigurationManager.AppSettings["Hrm_Hre_Service"];
        public static string Hrm_Rec_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];

        //do chưa có thời gian để chuyển vào bảng setting nên lấy dữ liệu tại webconfig tạm, sau này sẻ xóa
        public static string Hrm_Sal_ElementName_Comission = System.Configuration.ConfigurationManager.AppSettings["Hrm_Sal_ElementName_Comission"];
        public static string Hrm_Portal_Permission = System.Configuration.ConfigurationManager.AppSettings["Portal_Permission"];

    }

    public class ConstantPath
    {
        public const string IconPath = "/Content/images/icons/";

        #region General
        public const string IconEva = "../Content/images/icons/submenu/menu/Att_Report.png";
        public const string IconCall = "../Content/images/icons/phone.png";
        public const string IconFilter = "../Content/images/icons/HRM_Filter.png";
        public const string IconVi = "../Content/images/vi.gif";
        public const string IconEn = "../Content/images/en.gif";
        public const string IconWaitApprovePath = "../Content/images/icons/wait_approve.png";
        public const string IconExportPath = "../Content/images/icons/page_white_excel.png";
        public const string IconExportPathPDF = "../Content/images/icons/page_white_excel_pdf.png";
        public const string IconSavePath = "/Content/images/icons/save.png";
        public const string IconConfirmPath = "../Content/images/icons/accept.png";
        public const string IconComputePath = "../Content/images/icons/compute.png";
        public const string IconLoadDataPath = "../Content/images/icons/LoadData.png";
        public const string IconDeletePath = "../Content/images/icons/delete-file-icon.png";
        public const string IconAddNewPath = "../Content/images/icons/add-new-icon.png";
        public const string IconSearchPath = "../Content/images/icons/search.png";
        public const string IconRunPath = "../Content/images/icons/run.png";
        public const string IconStartPath = "../Content/images/icons/clock_start.png";
        public const string IconBtnCompute = "../Content/images/icons/submenu/menu/Btn_Att_CalculateAttendance.png";
        public const string IconBookmark = "../Content/images/icons/submenu/menu/bookmark.png";
        public const string ShowBookmark = "../Content/images/icons/submenu/menu/showBookmark.png";
        public const string IconEditPath = "../Content/images/icons/edit_icon.png";
        public const string IconBreadcrumbPath = "../Content/images/icons/breadcrumb.png";
        public const string IconApprovedPath = "../Content/images/icons/approved.png";
        public const string IconRefeshPath = "../Content/images/icons/arrow_refresh.png";
        public const string IconWaittingPath = "../Content/images/icons/waitting.png";
        public const string IconRejectPath = "../Content/images/icons/reject.png";
        public const string IconApprovePath = "../Content/images/icons/approved_roster.png";
        public const string IconCopyPath = "../Content/images/icons/copy.png";
        public const string IconDownLoad = "../Content/images/icons/submenu/menu/Att_Download.jpg";
        public const string IconConfig = "../Content/images/icons/submenu/menu/Att_Config.jpg";
        public const string IconCancelLateEarly = "../Content/images/icons/submenu/menu/cancelLateEarly.png";
        public const string IconUpdateInOut = "../Content/images/icons/submenu/menu/updateInOut.png";
        public const string IconChangeInOut = "../Content/images/icons/submenu/menu/changeInOut.png";
        public const string IconChangeTheme = "../Content/images/icons/submenu/menu/theme.png";
        public const string IconCheckConnect = "../Content/images/icons/submenu/menu/Sys_CheckConnect.png";
        public const string IconUpdateSumLeaveDayPath = "../Content/images/icons/update_sumLeaveday.png";
        public const string IconWord = "../Content/images/icons/page_word.png";
        public const string IconNext = "../Content/images/icons/nexticon.png";
        public const string IconPrev = "../Content/images/icons/previousicon.png";
        public const string IconClear = "../Content/images/icons/clear.png";
        public const string IconCheckError = "../Content/images/icons/check_error.png";
        public const string IconHelp = "../Content/images/icons/help.png";
        #endregion

        #region Category

        /// <summary>
        /// Cat_Region
        /// </summary>
        public const string RegionIconPath = "../Content/images/icons/submenu/menu/Cat_Region.png";

        /// <summary>
        /// Cat_Import
        /// </summary>
        public const string CatImportIconPath = "../Content/images/icons/submenu/menu/Cat_Import.png";

        /// <summary>
        /// Cat_Import_List
        /// </summary>
        public const string CatImportListIconPath = "../Content/images/icons/submenu/menu/Cat_ImportList.png";

        /// <summary>
        /// Cat_Country
        /// </summary>
        public const string CountryIconPath = "../Content/images/icons/submenu/menu/Cat_Country.png";

        /// <summary>
        /// Cat_Province
        /// </summary>
        public const string ProvinceIconPath = "../Content/images/icons/submenu/menu/Cat_Province.png";

        /// <summary>
        /// Cat_District
        /// </summary>
        public const string DistrictIconPath = "../Content/images/icons/submenu/menu/Cat_District.png";

        /// <summary>
        /// Cat_Currency
        /// </summary>
        public const string CurrencyIconPath = "../Content/images/icons/submenu/menu/Cat_Currency.png";

        /// <summary>
        /// Cat_Religion
        /// </summary>
        public const string ReligionIconPath = "../Content/images/icons/submenu/menu/Cat_Religion.png";

        /// <summary>
        /// Cat_Position
        /// </summary>
        public const string PositionIconPath = "../Content/images/icons/submenu/menu/Cat_Position.png";

        /// <summary>
        /// Cat_JobTitle
        /// </summary>
        public const string JobTitleIconPath = "../Content/images/icons/submenu/menu/Cat_JobTitle.png";

        /// <summary>
        /// Cat_Export
        /// </summary>
        public const string ExportIconPath = "../Content/images/icons/submenu/menu/Cat_Export.png";

        /// <summary>
        /// Cat_ExportItem
        /// </summary>
        public const string ExportItemIconPath = "../Content/images/icons/submenu/menu/Cat_Export.png";

        /// <summary>
        /// Cat_CostCentre
        /// </summary>
        public const string CostCentreIconPath = "../Content/images/icons/submenu/menu/Cat_CostCentre.png";

        /// <summary>
        /// Cat_EmployeeType
        /// </summary>
        public const string EmployeeTypeIconPath = "../Content/images/icons/submenu/menu/Cat_EmployeeType.png";

        /// <summary>
        /// Cat_Bank
        /// </summary>
        public const string BankIconPath = "../Content/images/icons/submenu/menu/Cat_Bank.png";

        /// <summary>
        /// Cat_Category
        /// </summary>
        public const string CategoryIconPath = "../Content/images/icons/submenu/menu/Cat_Category.png";

        /// <summary>
        /// Cat_EthnicGroup
        /// </summary>
        public const string EthnicGroupIconPath = "../Content/images/icons/submenu/menu/Cat_EthnicGroup.png";

        /// <summary>
        /// Cat_LeaveDayType
        /// </summary>
        public const string LeaveDayTypeIconPath = "../Content/images/icons/submenu/menu/Cat_LeaveDayType.png";

        /// <summary>
        /// Cat_OrgStructure
        /// </summary>
        public const string OrgStructureIconPath = "../Content/images/icons/submenu/menu/HR_organizational_departments.png";


        /// <summary>
        /// Cat_OrgStructureType
        /// </summary>
        public const string OrgStructureTypeIconPath = "../Content/images/icons/submenu/menu/Cat_OrgStructureType.png";

        /// <summary>
        /// Cat_Element
        /// </summary>
        public const string ElementIconPath = "../Content/images/icons/submenu/menu/Cat_OrgStructureType.png";

        /// <summary>
        /// Cat_GradeAttendance
        /// </summary>
        public const string GradeAttendanceIconPath = "/Content/images/icons/submenu/menu/Attendance.png";
        /// <summary>
        /// Cat_GradeAttendance
        /// </summary>
        public const string LaurndryIconPath = "/Content/images/icons/submenu/menu/Laundry_icon.png";

        #endregion

        #region Hre
        /// <summary>
        /// Hre_WorkHistory 
        /// </summary>
        public const string HreProfileIconPath = "/Content/images/icons/submenu/menu/Hre_Profile.png";

        /// <summary>
        /// Hre_Dependant 
        /// </summary>
        public const string HreDependantIconPath = "/Content/images/icons/submenu/menu/Hre_Dependant.png";

        /// <summary>
        /// Hre_WorkHistory 
        /// </summary>
        public const string HreWorkHistoryIconPath = "/Content/images/icons/submenu/menu/Hre_WorkHistory.png";

        /// <summary>
        /// Hre_Contract  
        /// </summary>
        public const string HreContractIconPath = "/Content/images/icons/submenu/menu/Hre_Contract.png";

        /// <summary>
        ///  Hre_CardHistory
        /// </summary>
        public const string Hre_CardHistoryIconPath = "/Content/images/icons/submenu/menu/Hre_CardHistory.png";

        /// <summary>
        ///  Hre_Report
        /// </summary>
        /// 

        public const string Hre_Report = "/Content/images/icons/submenu/menu/Att_ShiftTimeSheet.png";
        /// <summary>
        /// Hre_Recruitment
        /// </summary>
        public const string Hre_Recruitment = "/Content/images/icons/submenu/menu/Hre_Recruitment.png";
        /// <summary>
        /// Hre_List_Payment
        /// </summary>
        public const string Hre_List_Payment = "/Content/images/icons/submenu/menu/Hre_List_Payment.png";





        #endregion

        #region Attendance
        /// <summary>
        ///  Att_Grade
        /// </summary>
        public const string Att_GradeIconPath = "/Content/images/icons/submenu/menu/Att_establish_the_work.png";
        /// <summary>
        ///  Att_Attendance
        /// </summary>
        public const string Att_AttendanceIconPath = "/Content/images/icons/submenu/menu/Attendance.png";

        /// <summary>
        ///  Att_InOut
        /// </summary>
        public const string Att_InOutIconPath = "/Content/images/icons/submenu/menu/Att_ShiftTimeSheet.png";

        /// <summary>
        ///  Att_ComputeWorkDay
        /// </summary>
        public const string Att_ComputeWorkDayIconPath = "/Content/images/icons/submenu/menu/Att_CalculateAttendance.png";

        /// <summary>
        ///  Att_Overtime
        /// </summary>
        public const string Att_OvertimeIconPath = "/Content/images/icons/submenu/menu/Att_overtime.png";

        /// <summary>
        ///  Att_Overtime
        /// </summary>
        public const string AttOvertimeIconPath = "/Content/images/icons/submenu/menu/Hre_Profile.png";

        /// <summary>
        ///  Att_Roster
        /// </summary>
        public const string Att_RosterIconPath = "/Content/images/icons/submenu/menu/Att_ShiftTimeSheet.png";
        public const string Att_Roster = "/Content/images/icons/submenu/menu/Att_Roster.png";


        /// <summary>
        ///  Att_RosterGroup
        /// </summary>
        public const string Att_RosterGroupIconPath = "/Content/images/icons/submenu/menu/Att_ShiftTimeSheet.png";
        public const string Att_RosterGroup = "/Content/images/icons/submenu/menu/Kind_Working.png";

        /// <summary>
        ///  Att_TAMScanLog
        /// </summary>
        public const string Att_TAMScanLogIconPath = "/Content/images/icons/submenu/menu/Hre_CardHistory.png";

        /// <summary>
        ///  Att_Pregnancy
        /// </summary>
        public const string Att_PregnancyIconPath = "/Content/images/icons/submenu/menu/Att_Pregnancy.png";

        /// <summary>
        /// AttLeaveday
        /// </summary>
        public const string AttAnnualLeaveIconPath = "/Content/images/icons/submenu/menu/Att_Leaveday.png";

        /// <summary>
        /// Att_AnnualLeave
        /// </summary>
        public const string AttLeavedayIconPath = "/Content/images/icons/submenu/menu/Att_ShiftTimeSheet.png";
        public const string AttWordDay = "/Content/images/icons/submenu/menu/Att_Date_of_listing.png";
        #endregion

        /// <summary>
        ///  Eva_Performance
        /// </summary>
        public const string Eva_PerformanceIconPath = "/Content/images/icons/submenu/menu/Eva_Performance.png";

        #region SYSTEM
        /// <summary>
        /// AllSetting
        /// </summary>
        public const string Sys_LockObjectIconPath = "../Content/images/icons/submenu/menu/Conf_SystemDataList.png";

        /// <summary>
        /// AllSetting
        /// </summary>
        public const string AllSettingIconPath = "../Content/images/icons/submenu/menu/Conf_SystemDataList.png"; 

        /// <summary>
        /// UserSetting
        /// </summary>
        public const string UserSettingIconPath = "../Content/images/icons/submenu/menu/Conf_SystemDataList.png";

        /// <summary>
        /// UserSetting
        /// </summary>
        public const string AttandanceConfigIconPath = "../Content/images/icons/submenu/menu/Conf_ConfigTAMServer.png";

        /// <summary>
        /// UserSetting
        /// </summary>
        public const string AsynTaskIconPath = "../Content/images/icons/submenu/menu/Conf_ConfigTAMServer.png";
        #endregion
        #region Canteen
        /// <summary>
        /// Can_Canteen
        /// </summary>
        public const string Can_CanteenIconPath = "/Content/images/icons/submenu/menu/Can_Canteen.png";
        /// <summary>
        /// Cat_Catering
        /// </summary>
        public const string Cat_CateringIconPath = "/Content/images/icons/submenu/menu/Can_Catering.png";
        /// <summary>
        /// Cat_Line
        /// </summary>
        public const string Cat_LineIconPath = "/Content/images/icons/submenu/menu/Can_Line.png";
        /// <summary>
        /// Cat_Location
        /// </summary>
        public const string Cat_LocationIconPath = "/Content/images/icons/submenu/menu/Can_Location.png";
        /// <summary>
        /// Can_MachineOfLine
        /// </summary>
        public const string Can_MachineOfLineIconPath = "/Content/images/icons/submenu/menu/Att_TAM.png";
        /// <summary>
        /// Can_MealRecord
        /// </summary>
        public const string Can_MealRecordIconPath = "/Content/images/icons/submenu/menu/can_mealrecord.png";
        #endregion Canteen

        #region Laundry
        /// <summary>
        /// Laundry_Provider
        /// </summary>
        public const string ProviderIconPath = "../Content/images/icons/submenu/menu/washing-house.png";

        /// <summary>
        /// Laundry_Locker
        /// </summary>
        public const string LockerIconPath = "../Content/images/icons/submenu/menu/locker_2.png";

        /// <summary>
        /// Laundry_Reader
        /// </summary>
        public const string ReaderIconPath = "../Content/images/icons/submenu/menu/reader.png";
        #endregion

        #region Recruitment

        public const string Rec_CandidateIconPath = "/Content/images/icons/submenu/menu/HR_Plan.png";
        #endregion

        public const string Hrm_ChangeColumnIcon = "../Content/images/icons/submenu/menu/Sys_table_edit.png";
        public const string ImportRosterTemplate = "ImportRosterTemplate.xls";
        public const string ImportLeaveTemplate = "ImportLeaveTemplate.xls";
    }
}
