using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_AppendixContractSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        public DateTime? DateEnd { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }

    public class Hre_AppendixContractModel : BaseViewModel
    {
        public Guid? AppendixProfileID { get; set; }
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_ContractID)]
        public Guid ContractID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_ContractID)]

        public Guid ProfileContractID { get; set; }
        public string ContractName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ContractNo)]
        public string ContractNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ProfileID)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_AppendixContractName)]
        public string AppendixContractName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Code)]
        public string Code { get; set; }
        public string AppendixContractCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_DateofEffect)]
        public DateTime? DateofEffect { get; set; }
        public DateTime? DateofEffectView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Contents)]
        public string Contents { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Attachment)]
        public string Attachment { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_AppendixContractType)]
        public string AppendixContractType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_DateSignedAppendixContract)]
        public DateTime? DateSignedAppendixContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Amount)]
        public double? Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_DateStartAppendixContract)]
        public DateTime? DateStartAppendixContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_DateEndAppendixContract)]
        public DateTime? DateEndAppendixContract { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_RankRateID)]
        public Guid? RankRateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_ClassRateID)]
        public Guid? ClassRateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_ContractTypeID)]
        public Guid? ContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_AllowanceID1)]
        public Guid? AllowanceID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Allowance1)]
        public double? Allowance1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_AllowanceID2)]
        public Guid? AllowanceID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Allowance2)]
        public double? Allowance2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_AllowanceID3)]
        public Guid? AllowanceID3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Allowance3)]
        public double? Allowance3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Allowance4)]
        public double? Allowance4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Allowance)]
        public double? Allowance { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Salary)]
        public double? Salary { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_CurenncyID)]
        public Guid? CurenncyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_CurenncyID1)]
        public Guid? CurenncyID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_CurenncyID2)]
        public Guid? CurenncyID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_CurenncyID3)]
        public Guid? CurenncyID3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_AllowanceID4)]
        public Guid? AllowanceID4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_CurenncyIDSalary)]
        public Guid? CurenncyIDSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_CurenncyID4)]
        public Guid? CurenncyID4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_ReportMappingID)]
        public Guid? ReportMappingID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_CurenncyID5)]
        public Guid? CurenncyID5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_InsuranceAmount)]
        public double? InsuranceAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_CurenncyID6)]
        public Guid? CurenncyID6 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_AppendixContractTypeID)]
        public Guid? AppendixContractTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_DateAuthorize)]
        public DateTime? DateAuthorize { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_HourWorkInMonth)]
        public double? HourWorkInMonth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_WorkPlaceID)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_JobDescription)]
        public string JobDescription { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Bonus)]
        public double? Bonus { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_KPI)]
        public string KPI { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_AllowanceID5)]
        public Guid? AllowanceID5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_KPICompany)]
        public string KPICompany { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_Tax)]
        public double? Tax { get; set; }
        public Guid UserID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        [MaxLength(50)]
        public string IDNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        public string IDPlaceOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        [MaxLength(150)]
        public string PlaceOfIssueName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateEnd)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleName { get; set; }
         //Nơi làm việc
         [DisplayName(ConstantDisplay.HRM_HR_Contract_WorkPlaceID)]
         public string WorkPlaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateHire { get; set; }
        public string CurrencySalName { get; set; }
    
     
        public string CurenncyInsName { get; set; }
      
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID1)]
        public string AllowanceID1Name { get; set; }
        // Tiền PC1
      
        public string CurenncyAllowance1Name { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID2)]
        public string AllowanceID2Name { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID4)]
        public string AllowanceID4Name { get; set; }
        // Tiền PC4
  
        public string CurenncyOAllowanceName { get; set; }
      
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ClassRateID)]
        public string ClassRateName { get; set; }
        //Bậc/ hệ số lương

        [DisplayName(ConstantDisplay.HRM_HR_Contract_RankRateID)]
        public string RankRateName { get; set; }

        //Người đại diện ký
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ProfileSingID)]
        public Nullable<System.Guid> ProfileSingID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_ProfileSingID)]
        public string ProfileSingName { get; set; }
        //Hệ số cá nhân
        [DisplayName(ConstantDisplay.HRM_HR_Contract_PersonalRate)]
        public Nullable<double> PersonalRate { get; set; }
        //Loại Mã lương
        [DisplayName(ConstantDisplay.HRM_HR_Contract_SalaryClassTypeID)]
        public Nullable<System.Guid> SalaryClassTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_SalaryClassTypeID)]
        public string SalaryClassTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_ContractType_ContractTypeName)]
        [MaxLength(150)]
        public string ContractTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_CreateBasicSalary)]
        public bool? AppendixCreateBasicSalary { get; set; }
        public string CurenncyAllowance2Name { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_AllowanceID3)]
        public string AllowanceID3Name { get; set; }
        public string CurenncyAllowance3Name { get; set; }

        public string CurenncyAllowance4Name { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }
        private Guid _id = Guid.Empty;
        public Guid AppendixContract_ID
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

        private Guid? _profileid = Guid.Empty;
        public Guid? ResAppendixContract_ProfileID
        {
            get
            {
                _profileid = ProfileID;
                return _profileid;
            }
            set
            {
                _profileid = value;
                ProfileID = value;
            }
        }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateSigned)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateSigned { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateSigned)]
        public string PLDateSign { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Contract_DateStart)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }
        public string HDStarDate { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_RankDetailID)]
        public Nullable<System.Guid> RankDetailID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_RankDetailID)]
        public string RankDetailName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_RankID)]
        public string RankName { get; set; }
        public string AppendixContractTypeName { get; set; }
        //Ngày Sinh
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DayOfBirth)]
        [DataType(DataType.Date)]
        public System.DateTime DateOfBirth { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
 
        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string AppendixContractTypeName = "AppendixContractTypeName";
            public const string ID = "ID";
            public const string ContractID = "ContractID";
            public const string ContractNo = "ContractNo";
            public const string AppendixContractName = "AppendixContractName";
            public const string Code = "Code";
            public const string DateofEffect = "DateofEffect";
            public const string Contents = "Contents";
            public const string Attachment = "Attachment";
            public const string AppendixContractType = "AppendixContractType";
            public const string DateSignedAppendixContract = "DateSignedAppendixContract";
            public const string Amount = "Amount";
            public const string WorkPlaceID = "WorkPlaceID";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string DateEndAppendixContract = "DateEndAppendixContract";
            public const string DateStartAppendixContract = "DateStartAppendixContract";
            public const string JobTitleID = "JobTitleID";
            public const string RankRateID = "RankRateID";
            public const string ClassRateID = "ClassRateID";
            public const string PositionID = "PositionID";
            public const string ContractTypeID = "ContractTypeID";
            public const string AllowanceID1 = "AllowanceID1";
            public const string Allowance1 = "Allowance1";
            public const string AllowanceID2 = "AllowanceID2";
            public const string Allowance2 = "Allowance2";
            public const string AllowanceID3 = "AllowanceID3";
            public const string Allowance3 = "Allowance3";
            public const string Allowance4 = "Allowance4";
            public const string Allowance = "Allowance";
            public const string Salary = "Salary";
            public const string CurenncyID = "CurenncyID";
            public const string CurenncyID1 = "CurenncyID1";
            public const string CurenncyID2 = "CurenncyID2";
            public const string CurenncyID3 = "CurenncyID3";
            public const string AllowanceID4 = "AllowanceID4";
            public const string CurenncyIDSalary = "CurenncyIDSalary";
            public const string CurenncyID4 = "CurenncyID4";
            public const string ReportMappingID = "ReportMappingID";
            public const string CurenncyID5 = "CurenncyID5";
            public const string InsuranceAmount = "InsuranceAmount";
            public const string CurenncyID6 = "CurenncyID6";
            public const string AppendixContractTypeID = "AppendixContractTypeID";
            public const string DateAuthorize = "DateAuthorize";
            public const string HourWorkInMonth = "HourWorkInMonth";
            public const string JobDescription = "JobDescription";
            public const string Bonus = "Bonus";
            public const string KPI = "KPI";
            public const string AllowanceID5 = "AllowanceID5";
            public const string KPICompany = "KPICompany";
            public const string Tax = "Tax";
            public const string ContractName = "ContractName";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string OrgStructureName = "OrgStructureName";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string PositionName = "PositionName"; 
            public const string JobTitleName = "JobTitleName";
            public const string DateOfBirth = "DateOfBirth";
            public const string DateofEffectView = "DateofEffectView";
            public const string AppendixContractCode = "AppendixContractCode";
        }
    }

    public class HreAppendixContractTypeMultiModel
    {
        public Guid ID { get; set; }
        public string AppendixContractName { get; set; }
    }



}
