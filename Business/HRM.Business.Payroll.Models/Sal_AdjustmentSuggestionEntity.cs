using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Sal_AdjustmentSuggestionEntity : HRMBaseModel
    {
        public string Status { get; set; }
        public System.Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public Nullable<System.Guid> SalaryJobGroupID { get; set; }
        public string SalaryJobGroupName { get; set; }
        public Nullable<int> JobLevel { get; set; }
        public Nullable<System.Guid> RankRateID { get; set; }
        public string SalaryRankName { get; set; }
        public Nullable<System.Guid> ClassRateID { get; set; }
        public string SalaryClassName { get; set; }
        public string GrossAmount { get; set; }
        public string SalAdjustmentCampaignName { get; set; }
        public System.DateTime DateOfEffect { get; set; }
        public System.Guid CurrencyID { get; set; }
        public Nullable<System.Guid> AllowanceType1ID { get; set; }
        public Nullable<System.Guid> AllowanceType2ID { get; set; }
        public double InsuranceAmount { get; set; }
        public Nullable<double> AllowanceAmount1 { get; set; }
        public Nullable<double> AllowanceAmount2 { get; set; }
        public Nullable<double> AllowanceAmount3 { get; set; }
        public Nullable<double> AllowanceAmount4 { get; set; }
        public Nullable<double> AllowanceAmount5 { get; set; }
        public string DecisionNo { get; set; }
        public Nullable<System.Guid> SalAdjustmentCampaignID { get; set; }
        public Nullable<System.Guid> CurrencyID1 { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> DateOfRequest { get; set; }
        public Nullable<System.DateTime> MonthOfBackPay { get; set; }
        public Nullable<double> PersonalRate { get; set; }
        public Nullable<double> Coefficient { get; set; }
        public Nullable<System.Guid> AllowanceType3ID { get; set; }
        public string TypeBasicSalary { get; set; }
        public Nullable<System.Guid> AllowanceType4ID { get; set; }
        public Nullable<System.Guid> CurrencyID2 { get; set; }
        public Nullable<System.Guid> CurrencyID3 { get; set; }
        public Nullable<System.Guid> CurrencyID4 { get; set; }
        public Nullable<System.Guid> CurrencyID5 { get; set; }
        public Nullable<System.Guid> AllowanceTypeID { get; set; }
        public Nullable<double> AllowanceAmount6 { get; set; }
        public Nullable<System.Guid> CurrencyID6 { get; set; }
        public Nullable<System.Guid> AllowanceTypeID5 { get; set; }
        public string Attachment { get; set; }
        public string OldAmount { get; set; }
        public string CurrencyName { get; set; }


        public System.Guid OrgStructureNameID { get; set; } 
        public string ContractNo { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<System.DateTime> DateHire { get; set; }
        public Nullable<System.DateTime> DateQuit { get; set; }
       
        public string OrgStructureName { get; set; }
        public string CurrencyName1 { get; set; }
        public string CurrencyName2 { get; set; }
        public string CurrencyName3 { get; set; }
        public string CurrencyName4 { get; set; }
        public string CurrencyName5 { get; set; }
        public string DepartmentName { get; set; }
        public double? AmountTotal { get; set; }
        public string StatusView { get; set; }


        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }


        public partial class FieldNames
        {

            public const string ID = "ID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";
            public const string ProfileID = "ProfileID";
            public const string SalaryJobGroupID = "SalaryJobGroupID";
            public const string JobLevel = "JobLevel";
            public const string RankRateID = "RankRateID";
            public const string ClassRateID = "ClassRateID";
            public const string GrossAmount = "GrossAmount";
            public const string DateOfEffect = "DateOfEffect";
            public const string CurrencyID = "CurrencyID";
            public const string AllowanceType1ID = "AllowanceType1ID";
            public const string AllowanceType2ID = "AllowanceType2ID";
            public const string InsuranceAmount = "InsuranceAmount";
            public const string AllowanceAmount1 = "AllowanceAmount1";
            public const string AllowanceAmount2 = "AllowanceAmount2";
            public const string AllowanceAmount3 = "AllowanceAmount3";
            public const string AllowanceAmount4 = "AllowanceAmount4";
            public const string AllowanceAmount5 = "AllowanceAmount5";
            public const string DecisionNo = "DecisionNo";
            public const string SalAdjustmentCampaignID = "SalAdjustmentCampaignID";
            public const string CurrencyID1 = "CurrencyID1";
            public const string Note = "Note";
            public const string Code = "Code";
            public const string DateOfRequest = "DateOfRequest";
            public const string MonthOfBackPay = "MonthOfBackPay";
            public const string PersonalRate = "PersonalRate";
            public const string Coefficient = "Coefficient";
            public const string AllowanceType3ID = "AllowanceType3ID";
            public const string TypeBasicSalary = "TypeBasicSalary";
            public const string AllowanceType4ID = "AllowanceType4ID";
            public const string CurrencyID2 = "CurrencyID2";
            public const string CurrencyID3 = "CurrencyID3";
            public const string CurrencyID4 = "CurrencyID4";
            public const string CurrencyID5 = "CurrencyID5";
            public const string AllowanceTypeID = "AllowanceTypeID";
            public const string AllowanceAmount6 = "AllowanceAmount6";
            public const string CurrencyID6 = "CurrencyID6";
            public const string AllowanceTypeID5 = "AllowanceTypeID5";
            public const string Attachment = "Attachment";
            public const string OldAmount = "OldAmount";


            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateOfBirth = "DateOfBirth";
            public const string DateHire = "DateHire";
            public const string DateQuit = "DateQuit";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string ContractNo1 = "ContractNo1";
        }
    }
}
