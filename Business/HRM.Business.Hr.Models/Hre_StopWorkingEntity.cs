using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_StopWorkingEntity : HRMBaseModel
    {
        public string StatusComeBackView { get; set; }
        public int TotalRow { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string PositionName { get; set; }
        public Guid? PositionID { get; set; }
        public string JobTitleName { get; set; }
        public Guid? JobTitleID { get; set; }
        public string OrgStructureName { get; set; }
        public Guid? OrgStructureID { get; set; }
        public string StopWorkType { get; set; }
        public string StopWorkTypeView { get; set; }
        public Nullable<System.Guid> ResignReasonID { get; set; }
        public string ResignReasonName { get; set; }
        public Nullable<System.DateTime> DateStop { get; set; }
        public Nullable<System.DateTime> DateStopView { get; set; }
        public string DateStopString { get; set; }
        public string PAddress { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public Nullable<bool> IsHoldSal { get; set; }
        public string LastStatusSyn { get; set; }
        public string Note { get; set; }
        public string ContractTypeName { get; set; }
        public string TypeSuspense { get; set; }
        public string TypeSuspenseView { get; set; }
        public Nullable<System.DateTime> DateComeBack { get; set; }
        public Nullable<System.DateTime> DateComeBackView { get; set; }

        public string DateComeBackString { get; set; }
        public Nullable<System.DateTime> DateExpired { get; set; }
        public string DateExpiredString { get; set; }
        public string StatusComeBack { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string RequestDateString { get; set; }
        public string DateHireString { get; set; }
        public Guid? ExportID { get; set; }
        public string TypeOfStop { get; set; }
        public DateTime? DateHire { get; set; }
        public string DeptPath { get; set; }
        public Nullable<System.DateTime> RequestDateComeBack { get; set; }
        public Nullable<int> NoPrint { get; set; }
        public Guid UserID { get; set; }
        public string DecisionNo { get; set; }

        public string SocialInsNo { get; set; }
        public string SocialInsIssuePlace { get; set; }

        public DateTime? SocialInsDateReg { get; set; }
        public string SocialInsDateRegFormat { get; set; }
        public double? Salary { get; set; }
        public string ContractNo { get; set; }

        public DateTime? DateQuit { get; set; }
        public string DateQuitFormat { get; set; }

        public DateTime? DateStart { get; set; }

        public string DateStartFormat { get; set; }

        public DateTime? DateSigned { get; set; }
        public string DateSignedFormat { get; set; }

        public string DateEndFormat { get; set; }
        public string DateEndAppendixContractFormat { get; set; }
        public string CodeAppendixContract { get; set; }
       
        public int? YearOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? DayOfBirth { get; set; }
      
       
        public string DateOfBirth { get; set; }
        public string DateNow_Day { get; set; }
        public string DateNow_Month { get; set; }
        public string DateNow_Year { get; set; }
      
        public string DateStopFormat { get; set; }
        public string RequestDateFormat { get; set; }
        public string RequestDate_Day { get; set; }
        public string RequestDate_Month { get; set; }
        public string RequestDate_Year { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public Nullable<System.Guid> TypeOfStopID { get; set; }
        public string TypeOfStopName { get; set; }
        public string ReqDocumentName { get; set; }
        public string StoredDocuments { get; set; }

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
    }
}
