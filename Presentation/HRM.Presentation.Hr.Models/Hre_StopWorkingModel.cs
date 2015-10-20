using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_StopWorkingModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReqDocumentID)]
        public Nullable<System.Guid> ReqDocumentID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReqDocumentID)]
        public string ReqDocumentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_StoredDocuments)]
        [MaxLength(150)]
        public string StoredDocuments { get; set; }
        public string StoredDocumentCodes { get; set; }
        public string RegisterSuspenseType { get; set; }
        public int TotalRow { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public Guid? OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_StopWorkType)]
        public string StopWorkType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_StopWorkType)]
        public string StopWorkTypeView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonName)]
        public Nullable<System.Guid> ResignReasonID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonName)]
        public string ResignReasonName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateStop)]
        public Nullable<System.DateTime> DateStop { get; set; }
        public Nullable<System.DateTime> DateStopView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string StatusView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsHoldSal)]
        public Nullable<bool> IsHoldSal { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_LastStatusSyn)]
        public string LastStatusSyn { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Notes)]
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TypeSuspense)]
        public string TypeSuspense { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TypeSuspense)]
        public string TypeSuspenseView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateComeBack)]
        public Nullable<System.DateTime> DateComeBack { get; set; }

        public Nullable<System.DateTime> DateComeBackView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateExpired)]
        public Nullable<System.DateTime> DateExpired { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_StatusComeBack)]
        public string StatusComeBack { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_StopWorking_RequestDate)]
        public Nullable<System.DateTime> RequestDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_StopWorking_TypeOfStop)]
        public string TypeOfStop { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_StopWorking_DateHire)]
        public DateTime DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_StopWorking_DeptPath)]
        public string DeptPath { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_StopWorking_RequestDateComeBack)]
        public Nullable<System.DateTime> RequestDateComeBack { get; set; }
        [DisplayName(ConstantDisplay.NoPrint)]
        public Nullable<int> NoPrint { get; set; }
        public Guid UserID { get; set; }
        public string DecisionNo { get; set; }
        public string StatusComeBackView { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_StopWorking_TypeOfStop)]
        public Nullable<System.Guid> TypeOfStopID { get; set; }
        public string TypeOfStopName { get; set; }
        public string SalaryRankName { get; set; }
        public string Gender { get; set; }
        public string PlaceOfBirth { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string HomePhone { get; set; }
        public string Cellphone { get; set; }
        public string IDNo { get; set; }
        public string IDDateOfIssue { get; set; }
        public string EducationLevel { get; set; }
        public string TrainningLevel { get; set; }
        public Guid? SalaryClassID { get; set; }
        public partial class FieldNames
        {
            public const string StatusComeBackView = "StatusComeBackView";
            public const string DecisionNo = "DecisionNo";
            public const string ID = "ID";
            public const string RequestDateComeBack = "RequestDateComeBack";
            public const string NoPrint = "NoPrint";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string StopWorkType = "StopWorkType";
            public const string StopWorkTypeView = "StopWorkTypeView";
            public const string ResignReasonID = "ResignReasonID";
            public const string ResignReasonName = "ResignReasonName";
            public const string DateStop = "DateStop";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string IsHoldSal = "IsHoldSal";
            public const string LastStatusSyn = "LastStatusSyn";
            public const string CodeEmp = "CodeEmp";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string OrgStructureName = "OrgStructureName";
            public const string Note = "Note";
            public const string TypeSuspense = "TypeSuspense";
            public const string TypeSuspenseView = "TypeSuspenseView";
            public const string DateComeBack = "DateComeBack";
            public const string DateExpired = "DateExpired";
            public const string StatusComeBack = "StatusComeBack";
            public const string RequestDate = "RequestDate";
            public const string OrgStructureID = "OrgStructureID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string DateComeBackView = "DateComeBackView";
            public const string DateStopView = "DateStopView";

        }
    }

    public class Hre_StopWorkingSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Uniform_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonName)]
        public Guid? ResignReasonID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_StopWorkType)]
        public string StopWorkType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_StopWorking_TypeOfStop)]
        public Nullable<System.Guid> TypeOfStopID { get; set; }
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        public Nullable<System.Guid> RankID { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }
    public class Hre_StopWorking_DataStop
    {
        public Guid ID { get; set; }
        public Nullable<System.DateTime> DateQuit { get; set; }
    }
    public class Hre_RegisterSuspenseSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Uniform_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TypeSuspense)]
        public string TypeSuspense { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }
        public Nullable<System.Guid> RankID { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }

    public class Hre_SuspenseSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Uniform_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TypeSuspense)]
        public string TypeSuspense { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }

        public string StatusComeBack { get; set; }

        // Ngày đk vào làm lại
        public DateTime? RequestDateComebackFrom { get; set; }

        public DateTime? RequestDateComebackTo { get; set; }

        // Ngày vào làm lại
        public DateTime? DateComebackFrom { get; set; }

        public DateTime? DateComebackTo { get; set; }
        public Nullable<System.Guid> RankID { get; set; }
        public bool IsCreateTemplate { get; set; }

        public Guid ExportId { get; set; }

        public ExportFileType ExportType { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }

    public class Hre_RegisterComeBackSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Uniform_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonName)]
        public Guid? ResignReasonID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_StopWorkType)]
        public string StopWorkType { get; set; }
        public Nullable<System.Guid> RankID { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }
}
