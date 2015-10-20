using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileIsBackListEntity : HRMBaseModel
    {

        [DataType(DataType.Date)]
        public DateTime? DateApplyAttendanceCode { get; set; }


        public string StatusSyn { get; set; }


        [MaxLength(150)]
        public string ProfileName { get; set; }


        [MaxLength(150)]
        public string LastName { get; set; }


        [MaxLength(150)]
        public string FirstName { get; set; }


        [MaxLength(150)]
        public string NameEnglish { get; set; }


        [MaxLength(1000)]
        public string ImagePath { get; set; }

        [MaxLength(50)]
        public string CodeEmp { get; set; }


        [MaxLength(50)]
        public string CodeTax { get; set; }


        [MaxLength(50)]
        public string CodeAttendance { get; set; }


        [MaxLength(50)]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateHire { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DateEndProbation { get; set; }

        public string OrgStructureID { get; set; }


        [MaxLength(150)]
        public string OrgStructureName { get; set; }


        public Guid? PositionID { get; set; }

        [MaxLength(150)]
        public string PositionName { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DateOfEffect { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Guid? CostCentreID { get; set; }

        [MaxLength(150)]
        public string CostCentreName { get; set; }


        [MaxLength(1000)]
        public string WorkingPlace { get; set; }

        [MaxLength(50)]
        public string Gender { get; set; }


        [MaxLength(50)]
        public string GenderView { get; set; }


        public int? DayOfBirth { get; set; }

        public int? MonthOfBirth { get; set; }

        public int? YearOfBirth { get; set; }

        [MaxLength(150)]
        public string PlaceOfBirth { get; set; }

        [MaxLength(150)]
        public string PlaceOfBirthName { get; set; }

        public Guid? NationalityID { get; set; }

        [MaxLength(150)]
        public string NationalityName { get; set; }


        public Guid? EthnicID { get; set; }

        [MaxLength(150)]
        public string EthnicGroupName { get; set; }

        public Guid? ReligionID { get; set; }

        [MaxLength(150)]
        public string ReligionName { get; set; }


        public string BloodType { get; set; }

        public double? Height { get; set; }

        public double? Weight { get; set; }


        [MaxLength(50)]
        public string IDNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? IDDateOfIssue { get; set; }

        public string IDPlaceOfIssue { get; set; }


        [MaxLength(150)]
        public string PlaceOfIssueName { get; set; }

        [MaxLength(50)]
        public string PassportNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PassportDateOfExpiry { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PassportDateOfIssue { get; set; }

        public string PassportPlaceOfIssue { get; set; }


        [MaxLength(150)]
        public string PassportPlaceOfIssueName { get; set; }


        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Cellphone { get; set; }


        [MaxLength(50)]
        public string HomePhone { get; set; }

        [MaxLength(50)]
        public string BusinessPhone { get; set; }


        public Guid? PAddressID { get; set; }


        public Guid? TAddressID { get; set; }

        public Guid? JobTitleID { get; set; }

  
        [MaxLength(150)]
        public string JobTitleName { get; set; }

        public Guid? EmpTypeID { get; set; }


        [MaxLength(150)]
        public string EmployeeTypeName { get; set; }


        [MaxLength(50)]
        public string MarriageStatus { get; set; }


        [MaxLength(50)]
        public string MarriageStatusView { get; set; }

        public Guid? SupervisorID { get; set; }

        public Guid? HighSupervisorID { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DtFromQuit { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DtToQuit { get; set; }


        [MaxLength(1000)]
        public string Notes { get; set; }

        [MaxLength(150)]
        public string TAddress { get; set; }

        public Guid? TCountryID { get; set; }

        [MaxLength(150)]
        public string TCountry { get; set; }

        public Guid? TProvinceID { get; set; }

        [MaxLength(150)]
        public string TProvince { get; set; }

        public Guid? TDistrictID { get; set; }

        [MaxLength(150)]
        public string TDistrict { get; set; }


        [MaxLength(150)]
        public string PAddress { get; set; }

        public Guid? PCountryID { get; set; }

        [MaxLength(150)]
        public string PCountry { get; set; }

        public Guid? PProvinceID { get; set; }

        [MaxLength(150)]
        public string PProvince { get; set; }

        public Guid? PDistrictID { get; set; }

        [MaxLength(150)]
        public string PDistrict { get; set; }

        [MaxLength(150)]
        public string SupervisorName { get; set; }

        [MaxLength(150)]
        public string HighSupervisorName { get; set; }


        [DataType(DataType.Date)]
        public DateTime? RequestDate { get; set; }


        public Guid? ResReasonID { get; set; }

        public string ResignReasonName { get; set; }

        public string ResignNo { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DateQuit { get; set; }


        public bool? IsBlackList { get; set; }



       public System.Guid? EducationLevelID { get; set; }

        public string EducationLevelName { get; set; }

        [MaxLength(1000)]
        public string ResonBackList { get; set; }

        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }

        public bool IsChecked { get; set; }

        public string JobtitleCode { get; set; }

        public string PositionCode { get; set; }

        public string WorkPlaceCode { get; set; }

        public string EmployeeCode { get; set; }

        public string WorkPlaceName { get; set; }

        public Guid? WorkPlaceID { get; set; }

        public string CostCentreCode { get; set; }

        public string OrgStructureCode { get; set; }

        public string NameFamily { get; set; }

        public string StatusVerify { get; set; }

        // Thông tin bảo hiểm

        public string HealthInsNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HealthInsIssueDate { get; set; }


        [DataType(DataType.Date)]
        public DateTime? HealthInsExpiredDate { get; set; }


        public string HealthTreatmentPlace { get; set; }


        public bool? ReceiveHealthIns { get; set; }


        [DataType(DataType.Date)]
        public DateTime? ReceiveHealthInsDate { get; set; }


        [DataType(DataType.Date)]
        public DateTime? UnEmpInsDateReg { get; set; }


        public string LaborBookNo { get; set; }


        [DataType(DataType.Date)]
        public DateTime? LaborBookIssueDate { get; set; }

        public string WorkPermitStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime? WorkPermitInsDate { get; set; }


        public int? UnEmpInsCountMonthOld { get; set; }


        public string SocialInsNo { get; set; }

        public string SocialInsOldNo { get; set; }


        [DataType(DataType.Date)]
        public DateTime? SocialInsIssueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SocialInsDateReg { get; set; }

        public bool? ReceiveSocialIns { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReceiveSocialInsDate { get; set; }


        public string SocialInsSubmitBookStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SocialInsSubmitBookDate { get; set; }

        public string WorkPermitNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? WorkPermitExpiredDate { get; set; }

        public string CommentIns { get; set; }

        public string SocialInsIssuePlace { get; set; }

        public string HealthTreatmentPlaceCode { get; set; }

        public string Email2 { get; set; }

        public string AddressEmergency { get; set; }

        public bool? IsRegisterSocialIns { get; set; }


        public bool? IsRegisterHealthIns { get; set; }


        public bool? IsRegisterUnEmploymentIns { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DateQuitSign { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateQuitRequest { get; set; }


        public string ProfileSign { get; set; }


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

    }
}
