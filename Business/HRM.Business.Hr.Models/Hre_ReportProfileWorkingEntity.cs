using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileWorkingEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string IDNo { get; set; }
        public string Gender { get; set; }
        public string CodeAttendance { get; set; }
        public string PAStreet { get; set; }
        public string EmployeeTypeName { get; set; }
        public string PositionName { get; set; }
        public DateTime? DateHire { get; set; }
        public string CodeOrg { get; set; }
        public string CodeBranch { get; set; }
        public string CodeTeam { get; set; }
        public string CodeSection { get; set; }
        public string BranchName { get; set; }
        public string OrgName { get; set; }
        public string SectionName { get; set; }
        public string TeamName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string EthnicGroupName { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string PassportPlaceOfIssue { get; set; }
        public string Cellphone { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Origin { get; set; }
        public string PAddress { get; set; }
        public string GraduatedLevel { get; set; }
        public string QualificationName { get; set; }
        public string Senior { get; set; }
        public string BasicSalary { get; set; }
        public double? InSuranceSalary { get; set; }
        public string SocialInsNo { get; set; }
        public int? TotalDependant { get; set; }
        public string CodeTax { get; set; }
        public DateTime? DateQuit { get; set; }
        public string ResignReason { get; set; }
        public DateTime? TradeUnionistEnrolledDate { get; set; }
        public string Size { get; set; }
        public string HealthInsNo { get; set; }


        public DateTime? DateSigned { get; set; }

        public int? VentureLevel { get; set; }
        public double? E_GrossAmount { get; set; }
        public float? Allowance1 { get; set; }
        public float? Allowance2 { get; set; }
        public float? Allowance3 { get; set; }
        public float? Allowance4 { get; set; }

        public string AllowanceID1Name { get; set; }
        public string AllowanceID2Name { get; set; }
        public string AllowanceID3Name { get; set; }
        public string AllowanceID4Name { get; set; }

        public float? InsuranceAmount { get; set; }
        public string AccountNo { get; set; }
        public string AccountNo2 { get; set; }
        public string resignreasonname { get; set; }

        public string PassportNo { get; set; }
        public string PProvinceName { get; set; }


        public string GraduatedLevelName { get; set; }
        public string EducationLevelName { get; set; }
        //public DateTime? datequit { get; set; }


        public partial class FieldNames
        {
            public const string CodeTax = "CodeTax";
            public const string DateQuit = "DateQuit";
            public const string ResignReason = "ResignReason";
            public const string TradeUnionistEnrolledDate = "TradeUnionistEnrolledDate";
            public const string Size = "Size";
            public const string HealthInsNo = "HealthInsNo";
            public const string QualificationName = "QualificationName";
            public const string Senior = "Senior";
            public const string BasicSalary = "BasicSalary";
            public const string InSuranceSalary = "InSuranceSalary";
            public const string SocialInsNo = "SocialInsNo";
            public const string TotalDependant = "TotalDependant";
            public const string DateOfBirth = "DateOfBirth";
            public const string EthnicGroupName = "EthnicGroupName";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string PassportPlaceOfIssue = "PassportPlaceOfIssue";
            public const string Cellphone = "Cellphone";
            public const string PlaceOfBirth = "PlaceOfBirth";
            public const string Origin = "Origin";
            public const string PAddress = "PAddress";
            public const string GraduatedLevel = "GraduatedLevel";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string IDNo = "IDNo";
            public const string Gender = "Gender";
            public const string CodeAttendance = "CodeAttendance";
            public const string PAStreet = "PAStreet";
            public const string EmployeeTypeName = "EmployeeTypeName";
            public const string PositionName = "PositionName";
            public const string DateHire = "DateHire";
            public const string CodeOrg = "CodeOrg";
            public const string CodeBranch = "CodeBranch";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string SectionName = "SectionName";
            public const string TeamName = "TeamName";

            public const string DateSigned = "DateSigned";
            public const string VentureLevel = "VentureLevel";
            public const string E_GrossAmount = "E_GrossAmount";
            public const string Allowance1 = "Allowance1";
            public const string Allowance2 = "Allowance2";
            public const string Allowance3 = "Allowance3";
            public const string Allowance4 = "Allowance4";
            public const string AllowanceID1Name = "AllowanceID1Name";
            public const string AllowanceID2Name = "AllowanceID2Name";
            public const string AllowanceID3Name = "AllowanceID3Name";
            public const string AllowanceID4Name = "AllowanceID4Name";
            public const string InsuranceAmount = "InsuranceAmount";
            public const string AccountNo = "AccountNo";
            public const string AccountNo2 = "AccountNo2";
            public const string resignreasonname = "resignreasonname";
            public const string PassportNo = "PassportNo";
            public const string PProvinceName = "PProvinceName";
            public const string GraduatedLevelName = "GraduatedLevelName";
            public const string EducationLevelName = "EducationLevelName";
         //   public const string datequit = "datequit";
        }
    }
}
