using System;
using System.Collections.Generic;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_PerformanceEntity : BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public Nullable<System.Guid> PerformanceTemplateID { get; set; }
        public string PerformanceTemplateName { get; set; }
        public Nullable<System.Guid> PerformancePlanID { get; set; }
        public string PerformancePlanName { get; set; }
        public Nullable<System.DateTime> PeriodFromDate { get; set; }
        public Nullable<System.DateTime> PeriodToDate { get; set; }
        public Nullable<System.Guid> TagID { get; set; }
        public string TagName { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<double> TotalMark { get; set; }
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
        public string ResultNote { get; set; }
        public Nullable<System.Guid> LevelID { get; set; }
        public string LevelName { get; set; }
        public Nullable<double> Proportion { get; set; }
        public string AttachFile { get; set; }
        public string CurrentStatus { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public string Formula { get; set; }
        public string PerformanceTime { get; set; }
        public List<Guid> EvaluatorIDs { get; set; }
        public Guid? PerformanceTypeID { get; set; }
        public string PerformanceTypeName { get; set; }
        public string PerformanceTypeCode { get; set; }
        public string EvaluatorIDList { get; set; }
        public List<Eva_KPIEntity> KPIs { get; set; }
        public string AttachFileLast { get; set; }
        public string PerformanceEvaStatus { get; set; }
        public Nullable<System.Guid> PerformanceExtendID { get; set; }
        public Guid _id { get; set; }
        public string SupervisorName { get; set; }
        public string NationalityName { get; set; }
        public string HighSupervisorName { get; set; }


        public Nullable<System.Guid> RankDetailID { get; set; }
        public Nullable<System.Guid> RankID { get; set; }
        public Nullable<System.DateTime> YearEvaluation { get; set; }
        public Nullable<System.Guid> Level1ID { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public Nullable<double> Allowance5 { get; set; }
        public Nullable<double> Allowance6 { get; set; }
        public Nullable<double> Allowance7 { get; set; }
        public Nullable<System.DateTime> DateEffect { get; set; }
        public Nullable<System.DateTime> MonthReward { get; set; }
        public string GrossAmount { get; set; }

        public string Level1Name { get; set; }
        public string Level2Name { get; set; }
        public double? BasicGrossAmount { get; set; }

        public string SalaryClassName { get; set; }
        public string SalaryRankName { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }

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

        public string LaborBookNo { get; set; }

        public DateTime? LaborBookIssueDate { get; set; }

        public string DateSignedEN { get; set; }
         public string WorkPermitNo { get; set; }
        ////ngay cap so giay phep Ld
        public Nullable<System.DateTime> WorkPermitInsDate { get; set; }
        public string WorkPermitInsDateFormat { get; set; }
        public string WorkPermitInsDateFormatEN { get; set; }
        ////ngay ky
        public string DateSignedFormat { get; set; }
        public string DateSignedFormatEN { get; set; }
        public Guid EvaluatorID { get; set; }
        public List<string> AttachFiles { get; set; }

        public string DateNowvn { get; set; }

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
        public string NeedImprove { get; set; }
        public string TransferTo { get; set; }
        public DateTime? DateTranfer { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string PlaceOfIssueName { get; set; }
        public string ContractTypeName { get; set; }
        public double? Salary { get; set; }
        public string WorkPlaceName { get; set; }
        public Guid? WorkPlaceID { get; set; }
        public string ContractNo { get; set; }
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
    }
}
