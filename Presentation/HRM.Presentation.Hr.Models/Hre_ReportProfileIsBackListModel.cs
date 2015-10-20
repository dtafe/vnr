using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportProfileIsBackListModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateApplyAttendanceCode)]
        [DataType(DataType.Date)]
        public DateTime? DateApplyAttendanceCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_StatusSyn)]
        public string StatusSyn { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_LastName)]
        [MaxLength(150)]
        public string LastName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_FirstName)]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_NameEnglish)]
        [MaxLength(150)]
        public string NameEnglish { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ImagePath)]
        [MaxLength(1000)]
        public string ImagePath { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeTax)]
        [MaxLength(50)]
        public string CodeTax { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeAttendance)]
        [MaxLength(50)]
        public string CodeAttendance { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        [DataType(DataType.Date)]
        public DateTime? DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateEndProbation)]
        [DataType(DataType.Date)]
        public DateTime? DateEndProbation { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateOfEffect)]
        [DataType(DataType.Date)]
        public DateTime? DateOfEffect { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostCentreID)]
        public Guid? CostCentreID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostCentreName)]
        [MaxLength(150)]
        public string CostCentreName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        [MaxLength(1000)]
        public string WorkingPlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        [MaxLength(50)]
        public string Gender { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        [MaxLength(50)]
        public string GenderView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DayOfBirth)]
        public int? DayOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_MonthOfBirth)]
        public int? MonthOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_YearOfBirth)]
        public int? YearOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlaceOfBirth)]
        [MaxLength(150)]
        public string PlaceOfBirth { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlaceOfBirth)]
        [MaxLength(150)]
        public string PlaceOfBirthName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_NationalityID)]
        public Guid? NationalityID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NationalityName)]
        [MaxLength(150)]
        public string NationalityName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EthnicID)]
        public Guid? EthnicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EthnicGroupName)]
        [MaxLength(150)]
        public string EthnicGroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReligionID)]
        public Guid? ReligionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReligionName)]
        [MaxLength(150)]
        public string ReligionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_BloodType)]
        public string BloodType { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Height)]
        public double? Height { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Weight)]
        public double? Weight { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        [MaxLength(50)]
        public string IDNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDDateOfIssue)]
        [DataType(DataType.Date)]
        public DateTime? IDDateOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        public string IDPlaceOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        [MaxLength(150)]
        public string PlaceOfIssueName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportNo)]
        [MaxLength(50)]
        public string PassportNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportDateOfExpiry)]
        [DataType(DataType.Date)]
        public DateTime? PassportDateOfExpiry { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportDateOfIssue)]
        [DataType(DataType.Date)]
        public DateTime? PassportDateOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportPlaceOfIssue)]
        public string PassportPlaceOfIssue { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportPlaceOfIssue)]
        [MaxLength(150)]
        public string PassportPlaceOfIssueName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Email)]
        [MaxLength(50)]
        public string Email { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        [MaxLength(50)]
        public string Cellphone { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HomePhone)]
        [MaxLength(50)]
        public string HomePhone { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_BusinessPhone)]
        [MaxLength(50)]
        public string BusinessPhone { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAddressID)]
        public Guid? PAddressID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAddressID)]
        public Guid? TAddressID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeTypeName)]
        public Guid? EmpTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EmployeeTypeName)]
        [MaxLength(150)]
        public string EmployeeTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_MarriageStatus)]
        [MaxLength(50)]
        public string MarriageStatus { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_MarriageStatus)]
        [MaxLength(50)]
        public string MarriageStatusView { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SupervisiorID)]
        public Guid? SupervisorID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HighSupervisiorID)]
        public Guid? HighSupervisorID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateFrom)]
        [DataType(DataType.Date)]
        public DateTime? DtFromQuit { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateTo)]
        [DataType(DataType.Date)]
        public DateTime? DtToQuit { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Notes)]
        [MaxLength(1000)]
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAAddress)]
        [MaxLength(150)]
        public string TAddress { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TACountry)]
        public Guid? TCountryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TACountry)]
        [MaxLength(150)]
        public string TCountry { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public Guid? TProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        [MaxLength(150)]
        public string TProvince { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TADistrict)]
        public Guid? TDistrictID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TADistrict)]
        [MaxLength(150)]
        public string TDistrict { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAAddress)]
        [MaxLength(150)]
        public string PAddress { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PACountry)]
        public Guid? PCountryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PACountry)]
        [MaxLength(150)]
        public string PCountry { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAProvince)]
        public Guid? PProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAProvince)]
        [MaxLength(150)]
        public string PProvince { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PADistrict)]
        public Guid? PDistrictID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PADistrict)]
        [MaxLength(150)]
        public string PDistrict { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_SupervisiorID)]
        [MaxLength(150)]
        public string SupervisorName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HighSupervisiorID)]
        [MaxLength(150)]
        public string HighSupervisorName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_RequestDate)]
        [DataType(DataType.Date)]
        public DateTime? RequestDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ResignReason)]
        public Guid? ResReasonID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ResignReason)]
        public string ResignReasonName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ResignNo)]
        public string ResignNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        [DataType(DataType.Date)]
        public DateTime? DateQuit { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsBlackList)]
        public bool? IsBlackList { get; set; }




        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public System.Guid? EducationLevelID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public string EducationLevelName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReasonBackList)]
        [MaxLength(1000)]
        public string ResonBackList { get; set; }

        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }

        public bool IsChecked { get; set; }

        public string JobtitleCode { get; set; }

        public string PositionCode { get; set; }

        public string WorkPlaceCode { get; set; }

        public string EmployeeCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkPlaceName { get; set; }

        public Guid? WorkPlaceID { get; set; }

        public string CostCentreCode { get; set; }

        public string OrgStructureCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_LastName)]
        public string NameFamily { get; set; }

        public string StatusVerify { get; set; }

        // Thông tin bảo hiểm
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthInsNo)]
        public string HealthInsNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthInsIssueDate)]
        [DataType(DataType.Date)]
        public DateTime? HealthInsIssueDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthInsExpiredDate)]
        [DataType(DataType.Date)]
        public DateTime? HealthInsExpiredDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthTreatmentPlace)]
        public string HealthTreatmentPlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReceiveHealthIns)]
        public bool? ReceiveHealthIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateReceiveHealthIns)]
        [DataType(DataType.Date)]
        public DateTime? ReceiveHealthInsDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_UnEmpInsDateReg)]
        [DataType(DataType.Date)]
        public DateTime? UnEmpInsDateReg { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_LaborBookNo)]
        public string LaborBookNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_LaborBookIssueDate)]
        [DataType(DataType.Date)]
        public DateTime? LaborBookIssueDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPermitStatus)]
        public string WorkPermitStatus { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPermitInsDate)]
        [DataType(DataType.Date)]
        public DateTime? WorkPermitInsDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_UnEmpInsCountMonthOld)]
        public int? UnEmpInsCountMonthOld { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsNo)]
        public string SocialInsNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsOldNo)]
        public string SocialInsOldNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsIssueDate)]
        [DataType(DataType.Date)]
        public DateTime? SocialInsIssueDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsDateReg)]
        [DataType(DataType.Date)]
        public DateTime? SocialInsDateReg { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReceiveSocialIns)]
        public bool? ReceiveSocialIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReceiveSocialInsDate)]
        [DataType(DataType.Date)]
        public DateTime? ReceiveSocialInsDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsSubmitBookStatus)]
        public string SocialInsSubmitBookStatus { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsSubmitBookDate)]
        [DataType(DataType.Date)]
        public DateTime? SocialInsSubmitBookDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPermitNo)]
        public string WorkPermitNo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkPermitExpiredDate)]
        [DataType(DataType.Date)]
        public DateTime? WorkPermitExpiredDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CommentIns)]
        public string CommentIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_SocialInsIssuePlace)]
        public string SocialInsIssuePlace { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_HealthTreatmentPlaceCode)]
        public string HealthTreatmentPlaceCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Email2)]
        public string Email2 { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_AddressEmergency)]
        public string AddressEmergency { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsRegisterSocialIns)]
        public bool? IsRegisterSocialIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsRegisterHealthIns)]
        public bool? IsRegisterHealthIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsRegisterUnEmploymentIns)]
        public bool? IsRegisterUnEmploymentIns { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuitSign)]
        [DataType(DataType.Date)]
        public DateTime? DateQuitSign { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuitRequest)]
        [DataType(DataType.Date)]
        public DateTime? DateQuitRequest { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileSign)]
        public string ProfileSign { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_FileStore)]
        public string FileStore { get; set; }

        public string ScreenName { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


     

        //public ExportFileType ExportType { get; set; }
        /// <summary>
        /// [Tho.Bui]: Model Hre_ProfilePartyUnion
        /// </summary>
        /// 

        public partial class FieldNames
        {
            public const string Email2 = "Email2";
            public const string SocialInsIssuePlace = "SocialInsIssuePlace";
            public const string HealthInsNo = "HealthInsNo";
            public const string HealthInsIssueDate = "HealthInsIssueDate";
            public const string HealthInsExpiredDate = "HealthInsExpiredDate";
            public const string HealthTreatmentPlace = "HealthTreatmentPlace";
            public const string ReceiveHealthIns = "ReceiveHealthIns";
            public const string ReceiveHealthInsDate = "ReceiveHealthInsDate";
            public const string UnEmpInsDateReg = "UnEmpInsDateReg";
            public const string LaborBookNo = "LaborBookNo";
            public const string LaborBookIssueDate = "LaborBookIssueDate";
            public const string WorkPermitStatus = "WorkPermitStatus";
            public const string WorkPermitInsDate = "WorkPermitInsDate";
            public const string UnEmpInsCountMonthOld = "UnEmpInsCountMonthOld";
            public const string SocialInsNo = "SocialInsNo";
            public const string SocialInsOldNo = "SocialInsOldNo";
            public const string SocialInsIssueDate = "SocialInsIssueDate";
            public const string SocialInsDateReg = "SocialInsDateReg";
            public const string ReceiveSocialIns = "ReceiveSocialIns";
            public const string ReceiveSocialInsDate = "ReceiveSocialInsDate";
            public const string SocialInsSubmitBookStatus = "SocialInsSubmitBookStatus";
            public const string SocialInsSubmitBookDate = "SocialInsSubmitBookDate";
            public const string WorkPermitNo = "WorkPermitNo";
            public const string WorkPermitExpiredDate = "WorkPermitExpiredDate";
            public const string NameFamily = "NameFamily";
            public const string WorkPlaceID = "WorkPlaceID";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string RequestDate = "RequestDate";
            public const string ResignNo = "ResignNo";
            public const string ResReasonID = "ResReasonID";
            public const string ResignReasonName = "ResignReasonName";
            public const string ResonBackList = "ResonBackList";
            public const string IsChecked = "IsChecked";
            public const string ID = "ID";
            public const string TAddress = "TAddress";
            public const string TCountryID = "TCountryID";
            public const string TCountry = "TCountry";
            public const string TProvinceID = "TProvinceID";
            public const string TProvince = "TProvince";
            public const string TDistrictID = "TDistrictID";
            public const string TDistrict = "TDistrict";
            public const string PAddress = "PAddress";
            public const string PCountryID = "PCountryID";
            public const string PCountry = "PCountry";
            public const string PProvinceID = "PProvinceID";
            public const string PProvince = "PProvince";
            public const string PDistrictID = "PDistrictID";
            public const string PDistrict = "PDistrict";

            public const string ProfileName = "ProfileName";
            public const string LastName = "LastName";
            public const string FirstName = "FirstName";
            public const string NameEnglish = "NameEnglish";
            public const string ImagePath = "ImagePath";
            public const string CodeEmp = "CodeEmp";
            public const string CodeTax = "CodeTax";
            public const string CodeAttendance = "CodeAttendance";
            public const string Status = "Status";
            public const string DateHire = "DateHire";
            public const string DateEndProbation = "DateEndProbation";
            public const string OrgStructureID = "OrgStructureID";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionID = "PositionID";
            public const string PositionName = "PositionName";
            public const string DateOfEffect = "DateOfEffect";
            public const string CostCentreID = "CostCentreID";
            public const string CostCentreName = "CostCentreName";
            public const string WorkingPlace = "WorkingPlace";
            public const string Gender = "Gender";
            public const string GenderView = "GenderView";
            public const string DayOfBirth = "DayOfBirth";
            public const string MonthOfBirth = "MonthOfBirth";
            public const string YearOfBirth = "YearOfBirth";
            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string PlaceOfBirthName = "PlaceOfBirthName";
            public const string NationalityID = "NationalityID";
            public const string NationalityName = "NationalityName";
            public const string EthnicID = "EthnicID";
            public const string EthnicGroupName = "EthnicGroupName";
            public const string ReligionID = "ReligionID";
            public const string ReligionName = "ReligionName";
            public const string BloodType = "BloodType";
            public const string Height = "Height";
            public const string Weight = "Weight";
            public const string IDNo = "IDNo";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string PlaceOfIssueName = "PlaceOfIssueName";
            public const string PassportNo = "PassportNo";
            public const string PassportDateOfExpiry = "PassportDateOfExpiry";
            public const string PassportDateOfIssue = "PassportDateOfIssue";
            public const string PassportPlaceOfIssue = "PassportPlaceOfIssue";
            public const string PassportPlaceOfIssueName = "PassportPlaceOfIssueName";
            public const string Email = "Email";
            public const string Cellphone = "Cellphone";
            public const string HomePhone = "HomePhone";
            public const string BusinessPhone = "BusinessPhone";
            public const string PAddressID = "PAddressID";
            public const string TAddressID = "TAddressID";
            public const string JobTitleID = "JobTitleID";
            public const string JobTitleName = "JobTitleName";
            public const string EmpTypeID = "EmpTypeID";
            public const string EmployeeTypeName = "EmployeeTypeName";
            public const string MarriageStatus = "MarriageStatus";
            public const string MarriageStatusView = "MarriageStatusView";
            public const string SupervisorID = "SupervisiorID";
            public const string HighSupervisorID = "HighSupervisiorID";
            public const string IsBlackList = "IsBlackList";
            public const string DateQuit = "DateQuit";
            public const string Notes = "Notes";
            public const string DateUpdate = "DateUpdate";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
        }
    }
}
