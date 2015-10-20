namespace HRM.Infrastructure.Utilities
{
    /// <summary>
    /// Sử dụng cho mục đích tạo ra biến không thay đổi cho thông báo
    /// vd: 
    /// </summary>
    public class ConstantMessages
    {
        #region General
        // <summary>
        // Sử dụng trường hợp thông báo sử dụng chung
        // vd: Lưu thành công.....
        // </summary>

        public const string AccessDenied = "AccessDenied";
        public const string AccessDeniedTitle = "AccessDeniedTitle";
        public const string AccessDeniedContent = "AccessDeniedContent";
        public const string AccessDeniedReturn = "AccessDeniedReturn";

        //"Không tìm thấy template"
        public const string CanNotFindFileTemplate = "CanNotFindFileTemplate";

        //"NotTemplate"
        public const string NotTemplate = "NotTemplate";

        //Bạn đã duyệt đăng ký này
        public const string YouHaveToRegisterToBrowseThis = "YouHaveToRegisterToBrowseThis";

        //Người duyệt đầu duyệt thành công
        public const string ApprovedFirstSuccessfulBrowser = "ApprovedFirstSuccessfulBrowser";

        //Người duyệt đầu chưa duyệt đăng ký này
        public const string ApprovedUnapprovedEarlyThisRegistration = "ApprovedUnapprovedEarlyThisRegistration";

        //Thay Đổi Trạng Thái Thành Công
        public const string ChangeStatusSuccess = "ChangeStatusSuccess";

        //Đăng Ký Đã Được Duyệt
        public const string RegisterApproved = "RegisterApproved";

        //Đăng Ký Đã Từ Chối Không Được Phép Duyệt
        public const string RegistrationRefusedPermittedBrowse = "RegistrationRefusedPermittedBrowse";

        //Bạn có chắc chắn muốn xóa
        public const string AreYouSureYouTantToDelete = "AreYouSureYouTantToDelete";

        //Bạn Không Có Quyền Xử Lý Bản Ghi Này
        public const string YouDoNotHaveTheRightTreatmentMemorandumHey = "YouDoNotHaveTheRightTreatmentMemorandumHey";

        //Không Cho Phép Duyệt Tăng Ca Cho Bản Thân 
        public const string DisallowBrowseOvertimeForYourself = "DisallowBrowseOvertimeForYourself";

        //Không có dữ liệu nào được chọn
        public const string NoDataIsSelected = "NoDataIsSelected";

        //Ngày tháng không hợp lệ
        public const string InvalidDate = "InvalidDate";

        //Dữ Liệu Đã Duyệt Không Được Phép Xóa
        public const string DataWasNotPermittedDeleteBrowsing = "DataWasNotPermittedDeleteBrowsing";

        //Bạn có chắc chắn muốn xóa 
        public const string AreYouSureYouWantToDelete = "AreYouSureYouWantToDelete";

        //dòng dữ liệu đã chọn
        public const string SelectedDataLines = "SelectedDataLines";

        //Đối Tượng Đã Bị Khóa
        public const string AudienceLocked = "AudienceLocked";

        //Warinning
        public const string Warinning = "Warinning";

        //Xác Nhận Lưu Và Áp Dụng 
        public const string ValidationSaveAndApply = "ValidationSaveAndApply";

        //OK
        public const string Ok = "Ok";

        // Dòng có trạng thái Duyệt không xóa được
        public const string LineStatusIndelibleBrowse = "LineStatusIndelibleBrowse";

        //Dữ Liệu Có Trạng Thái Duyệt Không Được Xóa
        public const string BrowseDataHaveNotBeJobStatus = "BrowseDataHaveNotBeJobStatus";

        //Thêm Mới Thành Công
        public const string InsertSuccess = "InsertSuccess";

        //Thêm Mới Thất Bại
        public const string InsertFail = "InsertFail";

        //Cập Nhật Thành Công
        public const string UpdateSuccess = "UpdateSuccess";

        //Cập Nhatr Thất Bại
        public const string UpdateFail = "UpdateFail";

        //Vui Lòng Chỉ Chọn 1 Loại Template Để Xuất Báo Cáo
        public const string PleaseSelectOnlyOneTypeTemplateToExportAlert = "PleaseSelectOnlyOneTypeTemplateToExportAlert";

        //Vui Lòng Chọn Template Để Xuất Báo Cáo
        public const string PleaseSelectTemplateToExportAlert = "PleaseSelectTemplateToExportAlert";

        //Vui Lòng Chọn Dữ Liệu Để Xóa
        public const string PleaseSelectDataToDelete = "PleaseSelectDataToDelete";

        //Vui Lòng Chọn Dữ Liệu Để Xuất Excel
        public const string PleaseSelectExportDataToExcel = "PleaseSelectExportDataToExcel";

        //Không Có Dữ Liệu Để Xuất Excel
        public const string NoSelectExportDataToExcel = "NoSelectExportDataToExcel";

        //Đã Tồn Tại !
        public const string AlreadyExists = "AlreadyExists";

        //Status
        public const string Status = "Status";

        //Truy Cập
        public const string Access = "Access";

        //Tạo Mới
        public const string Create = "Create";

        //Chình Sửa
        public const string Update = "Update";

        //Xóa
        public const string Delete = "Delete";

        //Nhập Dữ Liệu
        public const string Import = "Import";

        //Xuất Dữ Liệu
        public const string Export = "Export";

        //Bạn không có quyền
        public const string YouNotAuthorities = "YouNotAuthorities";

        //Change
        public const string Change = "Change";

        //ErrorDateOfBirth
        public const string ErrorDateOfBirth = "ErrorDateOfBirth";

        //Bạn Chưa Cấu Hình Template
        public const string YouNoConfigurationTemplate = "YouNoConfigurationTemplate";

        #endregion 

        #region Cat_EthnicGroup
        #endregion

        #region Cat_Bank
        public const string HRM_Category_Bank_Confirm_Delete = "HRM_Category_Bank_Confirm_Delete";
        #endregion

        #region Cat_Category
        #endregion

        #region Cat_Country
        #endregion

        #region Cat_Currency
        #endregion

        #region Cat_District
        #endregion

        #region Cat_Province
        #endregion.

        #region Cat_Region
        #endregion

        #region Cat_Religion
        #endregion

        #region Cat_EmployeeType
        #endregion

        #region Cat_CostCentre
        #endregion

        #region Cat_JobTitle
        #endregion

        #region Cat_Position

        #endregion

        #region Hre_Attendance
        #region Att_ReportOvertime
        public const string HRM_Att_ReportDetailOvertime_NoTemplate = "HRM_Att_ReportDetailOvertime_NoTemplate";
        #endregion
        #endregion

        #region Cat_TAMScanReasonMissForAtt
        public const string HRM_Category_TAMScanReasonMiss_RequiredMealAllowanceTypeSettingName = "HRM_Category_TAMScanReasonMiss_RequiredMealAllowanceTypeSettingName";
        #endregion

        #region Validate Message
        public const string Notification = "Hrm_Notification";
        public const string Required = "Hrm_Required";
        public const string Email = "Hrm_Email";
        public const string Number = "Hrm_Number";
        public const string Date = "Hrm_Date";
        public const string PathNotExists = "Hrm_PathNotExists";
        public const string Succeed = "Hrm_Succeed";
        public const string Locked = "Hrm_Locked";
        public const string Error = "Hrm_Error";
        public const string Fail = "Hrm_Fail";
        public const string FieldNotAllowNull = "FieldNotAllowNull";
        public const string FieldNull = "FieldNull";
        public const string FieldInvalid = "FieldInvalid";
        public const string FieldInvalidFormat = "FieldInvalidFormat";
        public const string FieldInvalidMinLenght = "FieldInvalidMinLenght";
        public const string FieldMustGreater = "FieldMustGreater";
        public const string FieldInvalidMaxLenght = "FieldInvalidMaxLenght";
        public const string FieldMustLessThan = "FieldMustLessThan";
        public const string FieldMustMoreThan = "FieldMustMoreThan";
        public const string FieldDuplicate = "FieldDuplicate";
        public const string FieldMustLessThanCurrentDay = "FieldMustLessThanCurrentDay";
        public const string DateFromMustLessThanDateTo = "DateFromMustLessThanDateTo";

        #endregion

        #region Can_MealRecord
        public const string HRM_Canteen_MealRecord_Confirm_Delete = "HRM_Canteen_MealRecord_Confirm_Delete";
        #endregion

        #region Hre_Profile

        #endregion

        #region system
        public const string HRM_System_SysAllSetting_Confirm_ConfigDefault = "HRM_System_SysAllSetting_Confirm_ConfigDefault";
        #endregion

        #region Att
        public const string DataNotEnoughToMakeLeave = "DataNotEnoughToMakeLeave";
        public const string PlsCheckRosterOfEmpByDate = "PlsCheckRosterOfEmpByDate";
        public const string DoNotRegisterLeaveOverLastNextMonthOvertimeDay = "DoNotRegisterLeaveOverLastNextMonthOvertimeDay";
        public const string StatusApproveCannotEdit = "StatusApproveCannotEdit";
        public const string PlsSelectStatusApprove = "PlsSelectStatusApprove";
        public const string DoNotSelectPaymentMethodLeave = "DoNotSelectPaymentMethodLeave";
        public const string EmpDoNotConfigTimeOffBegin = "EmpDoNotConfigTimeOffBegin";
        public const string StatusRejectcannotEdit = "StatusRejectcannotEdit";
        public const string LeavedayIsNotExist = "LeavedayIsNotExist";
        public const string GradeAttendanceIsNotExist = "GradeAttendanceIsNotExist";
        public const string PlsSelectLeaveType = "PlsSelectLeaveType";
        public const string plsInputTAMScanReasonMissBeforeChangeInOut = "plsInputTAMScanReasonMissBeforeChangeInOut";
        public const string CantRegisterCO = "CantRegisterCO";
        public const string MissingInOut = "MissingInOut";
        public const string DifferenceShiftRosterAndInOut = "DifferenceShiftRosterAndInOut";
        public const string Hour = "Hour";
        public const string WorkingHoursLess = "WorkingHoursLess";
        public const string OverMaxNumdayLeaveInMonth = "OverMaxNumdayLeaveInMonth";
        public const string Att_EditFutureRoster = "Att_EditFutureRoster";
        public const string Att_EditPastRoster = "Att_EditPastRoster";

        
        
        
        #endregion
        public const string YouMustSaveFirst = "YouMustSaveFirst";
        public const string WarningTraineeOverLimit = "WarningTraineeOverLimit";
        public const string WarningProfileNotDependantOrgStructure = "WarningProfileNotDependantOrgStructure";
        public const string WarningProfileNotInRequirement = "WarningProfileNotInRequirement";
        public const string WarningProfileHaveCertificate = "WarningProfileHaveCertificate";
        public const string WarningProfileHaveClass = "WarningProfileHaveClass";
        public const string WarningProfileHaveNotContract = "WarningProfileHaveNotContract";
        public const string WarningContractIsNotApproved = "WarningContractIsNotApproved";
        public const string PlsChooseTraineeSameClass = "PlsChooseTraineeSameClass";
        public const string CourseNoHaveTopic = "CourseNoHaveTopic";
        public const string TopicIsExistedInCourse = "TopicIsExistedInCourse";
        public const string ScoreTypeIsExistedInTopic = "ScoreTypeIsExistedInTopic";
        public const string WarningQuantityPlanMustBeGreaterQuantityPlandetail = "WarningQuantityPlanMustBeGreaterQuantityPlandetail";
        public const string WrongDataWhenImportTraineeScore = "WrongDataWhenImportTraineeScore";
        public const string WrongFormulaInContractType = "WrongFormulaInContractType";
        public const string WarningPleaseChooseContractToCreateNextContractAndNextSalary = "WarningPleaseChooseContractToCreateNextContractAndNextSalary";
        public const string WarningContractHaveNotNextContract = "WarningContractHaveNotNextContract";
        public const string WarningRankIsNotEmpty = "WarningRankIsNotEmpty";
        public const string WarningDateOfEffectIsNotEmpty = "WarningDateOfEffectIsNotEmpty";
        public const string PlaceHolderRankLevel = "PlaceHolderRankLevel";
        public const string WarningRelativeGreaterThan18 = "WarningRelativeGreaterThan18";
        public const string CreateTemplateFormulaSuccess = "CreateTemplateFormulaSuccess";
        public const string CreateTemplateFormulaError = "CreateTemplateFormulaError";
        public const string NotCreateGradePayroll = "NotCreateGradePayroll";
        public const string NotPermission = "NotPermission";
        public const string DayOfMonth = "DayOfMonth";
        public const string DayOfWeek = "DayOfWeek";

        public const string WaringDateEndGreaterThanDateStart = "WaringDateEndGreaterThanDateStart";
        public const string YouMustDeleteDetailRecord = "YouMustDeleteDetailRecord";
        public const string DoYouWantToChooseFinishClass = "DoYouWantToChooseFinishClass";
        public const string RegisterDuplicate = "RegisterDuplicate";

        
    }
}
