using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportProfileComeBackModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_BranchName)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DepartmentName)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_TeamName)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_SectionName)]
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Pregnancy_DateOfBirth)]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlaceOfBirth)]
        public string PlaceOfBirth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        public string IDPlaceOfIssue { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAAddress)]
        public string PAddress { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HomePhone)]
        public string HomePhone { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Cellphone { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_IDNo)]
        public string IDNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateComeBack)]
        public DateTime? DateComeBack { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_SalaryRank_SalaryRankName)]
        public string SalaryRankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateHire { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateQuit)]
        public DateTime? DateQuit { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TypeOfStop)]
        public string TypeOfStop { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ResignReason_ReasonName)]
        public string ResignReasonName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_EducationLevel_Name)]
        public string EducationLevel { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_TrainningLevel)]
        public string TrainningLevel { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public Guid ExportID { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public DateTime? IDDateOfIssue { get; set; }

        //public DateTime? DateFromHire { get; set; }
        //public DateTime? DateToHire { get; set; }

        //public DateTime? DateFromQuit { get; set; }
        //public DateTime? DateToQuit { get; set; }

        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceGeneral_WorkPlaceID)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }


        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string PositionName = "PositionName";
            public const string DateOfBirth = "DateOfBirth";
            public const string Gender = "Gender";
            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string PAddress = "PAddress";
            public const string HomePhone = "HomePhone";
            public const string Cellphone = "Cellphone";
            public const string IDNo = "IDNo";
            public const string DateComeBack = "DateComeBack";
            public const string SalaryRankName = "SalaryRankName";
            public const string DateHire = "DateHire";
            public const string DateQuit = "DateQuit";
            public const string TypeOfStop = "TypeOfStop";
            public const string ResignReasonName = "ResignReasonName";
            public const string EducationLevel = "EducationLevel";
            public const string TrainningLevel = "TrainningLevel";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
            public const string IDDateOfIssue = "IDDateOfIssue";

        }
      
    }
}
