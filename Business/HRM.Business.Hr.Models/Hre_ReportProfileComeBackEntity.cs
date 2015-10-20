using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileComeBackEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string OrgStructureIDs { get; set; }
        public Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string PositionName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PlaceOfBirth { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string PAddress { get; set; }
        public string HomePhone { get; set; }
        public string Cellphone { get; set; }
        public string IDNo { get; set; }
        public DateTime? DateComeBack { get; set; }
        public string SalaryRankName { get; set; }
        public DateTime? DateHire { get; set; }
        public DateTime? DateQuit { get; set; }
        public string TypeOfStop { get; set; }
        public string ResignReasonName { get; set; }
        public string EducationLevel { get; set; }
        public string TrainningLevel { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



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
        }
    }
}
