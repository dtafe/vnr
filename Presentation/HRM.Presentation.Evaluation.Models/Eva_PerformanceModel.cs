using System;
using System.Collections.Generic;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_PerformanceModel : BaseViewModel
    {

        //#region lay them Enum xuat Word task 48004
        ////ngay hien tai
        //public Nullable<DateTime> DateNowFormat { get; set; }
        //public Nullable<System.DateTime> DateOfBirth { get; set; }
        ////dia chi thuong tru
        //public string PAddress { get; set; }
        ////CMND
        //public string IDNo { get; set; }
        ////ngay cap
        //public string IDDateOfIssue { get; set; }
        ////noi cap
        //public string IDPlaceOfIssue { get; set; }
        ////so giay pheo Lao dong
        public string WorkPermitNo { get; set; }
        ////ngay cap so giay phep Ld
        public Nullable<System.DateTime> WorkPermitInsDate { get; set; }
        public string WorkPermitInsDateFormat { get; set; }
        public string WorkPermitInsDateFormatEN { get; set; }
        //#endregion


        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTemplateOptionLabel)]
        public Guid? PerformanceTemplateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTemplate)]
        public string PerformanceTemplateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public Guid? PerformancePlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public string PerformancePlanName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_PeriodFromDate)]
        public DateTime? PeriodFromDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_PeriodToDate)]
        public DateTime? PeriodToDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? TagID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public string TagName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_DueDate)]
        public DateTime? DueDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public Nullable<double> TotalMark { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Strengths)]
        public string Strengths { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Weaknesses)]
        public string Weaknesses { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_ResultNote)]
        public string ResultNote { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_LevelID)]
        public Guid? LevelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public string LevelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Proportion)]
        public Nullable<double> Proportion { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_AttachFile)]
        public string AttachFile { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_CurrentStatus)]
        public string CurrentStatus { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_PerformanceTime)]
        public string PerformanceTime { get; set; }
        public string Formula { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator)]
        public Guid EvaluatorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTypeID)]
        public Guid? PerformanceTypeID { get; set; }
        public string PerformanceTypeName { get; set; }
        public string PerformanceTypeCode { get; set; }
        public List<Guid> EvaluatorIDs { get; set; }
        public string EvaluatorIDList { get; set; }
        public string AttachFileLast { get; set; }
        public string PerformanceEvaStatus { get; set; }
        public List<string> AttachFiles { get; set; }
        public Nullable<System.Guid> PerformanceExtendID { get; set; }
        public List<Eva_KPIModel> KPIs { get; set; }
        public Guid _id { get; set; }
        public Guid PerformanceID
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
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SupervisiorID)]
        public string SupervisorName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NationalityName)]
        public string NationalityName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HighSupervisiorID)]
        public string HighSupervisorName { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public Nullable<double> Allowance5 { get; set; }
        public Nullable<double> Allowance6 { get; set; }
        public Nullable<double> Allowance7 { get; set; }
        public string Level1Name { get; set; }
        public string Level2Name { get; set; }
        public double? BasicGrossAmount { get; set; }
        public string SalaryClassName { get; set; }
        public string SalaryRankName { get; set; }
        public Nullable<System.DateTime> DateEffect { get; set; }
        public string StatusView { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }


        public string Birthday { get; set; }
        public string PAddress { get; set; }
        public string IDNo { get; set; }
        public string IDDateOfIssue { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string DateSigned { get; set; }

        public Double? SalaryStandard { get; set; }
        public string DateNow { get; set; }

        public int? DayOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? YearOfBirth { get; set; }

        public string DateNowvn { get; set; }

        public string LaborBookNo { get; set; }

        public DateTime? LaborBookIssueDate { get; set; }

        public string DateSignedEN { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Level1ID)]
        public Guid? Level1ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_NeedImprove)]
        public string NeedImprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TransferTo)]
        public string TransferTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_DateTranfer)]
        public DateTime? DateTranfer { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Note1)]
        public string Note1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Note2)]
        public string Note2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Note3)]
        public string Note3 { get; set; }



        #region bo sung
        public Nullable<System.DateTime> Year { get; set; }
        public string GroupCode { get; set; }
        public string LocationByGroupCode { get; set; }
        public string ActualWokingPlace { get; set; }
        public DateTime? ReportEvalutionDataNode { get; set; }
        public int? Age { get; set; }
        public DateTime? Entering { get; set; }
        public string ServiceYear { get; set; }
        public string ServiceYearKi { get; set; }
        public string PayRankSalary { get; set; }
        public double? BasicSalary { get; set; }
        public string Rank { get; set; }
        public string RankZone { get; set; }

        public string CostCentreCode { get; set; }
        public string WorkPlaceCode { get; set; }
        public string QualificationAbilitytitle { get; set; }
        public Nullable<System.DateTime> HistoryDateFrom { get; set; }
        public double? TotalSalaryAllowance { get; set; }

        public Nullable<int> C1 { get; set; }
        public Nullable<int> C2 { get; set; }
        public Nullable<int> C3 { get; set; }
        public Nullable<int> C4 { get; set; }
        public Nullable<int> C5 { get; set; }
        public Nullable<int> C6 { get; set; }
        public Nullable<int> C7 { get; set; }
        public Nullable<int> C8 { get; set; }
        public Nullable<int> C9 { get; set; }
        public Nullable<int> C10 { get; set; }
        public Nullable<int> C11 { get; set; }
        public Nullable<int> C12 { get; set; }
        public Nullable<int> C13 { get; set; }
        public Nullable<int> C14 { get; set; }
        public Nullable<int> C15 { get; set; }
        public Nullable<int> C16 { get; set; }

        public string C17 { get; set; }
        public string C18 { get; set; }
        public double? C19 { get; set; }
        public double? C20 { get; set; }
        public double? C21 { get; set; }
        public string C22 { get; set; }
        public double? C23 { get; set; }
        public double? C24 { get; set; }
        public double? C25 { get; set; }
        public double? C26 { get; set; }
        public double? C27 { get; set; }
        public double? C28 { get; set; }
        public double? C29 { get; set; }
        public double? C30 { get; set; }
        public string C31 { get; set; }
        public string C32 { get; set; }
        public string TimeEvalutionData { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string TimeEvalutionDataCode { get; set; }

        public Nullable<double> Salary { get; set; }
        public string SalaryFormat { get; set; }
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
        public string WorkPlaceName { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string ContractNo { get; set; }
        public string DateSignedFormatEN { get; set; }

        public string DateSignedFormat { get; set; }
        public Nullable<System.Guid> ContractTypeID { get; set; }
        public string ContractTypeName { get; set; }
        public string PlaceOfIssueName { get; set; }


        #endregion

        public partial class FieldNames
        {
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";

            public const string BasicGrossAmount = "BasicGrossAmount";
            public const string SalaryClassName = "SalaryClassName";
            public const string SalaryRankName = "SalaryRankName";
            public const string ID = "ID";
            public const string CodeEmp = "CodeEmp";
            public const string Level1Name = "Level1Name";
            public const string Level2Name = "Level2Name";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string TagName = "TagName";
            public const string JobTitleName = "JobTitleName";
            public const string PerformanceTemplateName = "PerformanceTemplateName";
            public const string PerformancePlanName = "PerformancePlanName";
            public const string LevelName = "LevelName";
            public const string PerformanceTime = "PerformanceTime";
            public const string Status = "Status";
            public const string TotalMark = "TotalMark";
            public const string PerformanceTypeName = "PerformanceTypeName";
            public const string PerformanceEvaStatus = "PerformanceEvaStatus";
            public const string ProfileID = "ProfileID";
            public const string Allowance1 = "Allowance1";
            public const string Allowance2 = "Allowance2";
            public const string Allowance3 = "Allowance3";
            public const string Allowance4 = "Allowance4";
            public const string Allowance5 = "Allowance5";
            public const string Allowance6 = "Allowance6";
            public const string Allowance7 = "Allowance7";
            public const string DateEffect = "DateEffect";
            public const string StatusView = "StatusView";


            public const string Birthday = "Birthday";
            public const string PAddress = "PAddress";
            public const string IDNo = "IDNo";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string DateSigned = "DateSigned";
            public const string SalaryStandard = "SalaryStandard";
            public const string LaborBookNo = "LaborBookNo";
            public const string LaborBookIssueDate = "LaborBookIssueDate";
            public const string NeedImprove = "NeedImprove";
            public const string TransferTo = "TransferTo";
            public const string DateTranfer = "DateTranfer";
            public const string Note1 = "Note1";
            public const string Note2 = "Note2";
            public const string Note3 = "Note3";
        }
    }



    public class Eva_PerformanceSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public Guid? PerformancePlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public double? TotalMarkFrom { get; set; }
        public double? TotalMarkTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public System.Guid? LevelID { get; set; }
        public DateTime? DateForm { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }

    }
    public class Eva_PerformanceProfileSearchModel
    {

        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public Guid? PerformancePlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public double? TotalMarkFrom { get; set; }
        public double? TotalMarkTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public System.Guid? LevelID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }


    public class Eva_PerformanceExportModel
    {
        public string PerformanceID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
    }

    public class Eva_PerformanceGeneralSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public Guid? PerformancePlanID { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_TotalMark)]
        public double? TotalMarkFrom { get; set; }
        public double? TotalMarkTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public System.Guid? LevelID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }

        #region bo sung task 47702
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_WorkPlaceID)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_YearEvaluation)]
        //public Nullable<System.DateTime> YearEvaluation { get; set; }
        public Nullable<int> Year { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_Level1ID)]
        public Nullable<System.Guid> Level1ID { get; set; }

        public Nullable<Guid> UserID { get; set; }

        #endregion
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
