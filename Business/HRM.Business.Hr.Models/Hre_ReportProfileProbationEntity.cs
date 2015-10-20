using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileProbationEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureName { get; set; }
        public string CodeOrg { get; set; }
        public string JobTitleName { get; set; }
        public string PositionName { get; set; }
        public DateTime? DateHire { get; set; }
        public DateTime? DateEndProbation { get; set; }
        public DateTime? ProbExtendDate { get; set; }
        public string IDNo { get; set; }
        public DateTime? IDDateOfIssue { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string WorkPlace { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Gender { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }

        public partial class FieldNames
        {
            public const string Email = "Email";
            public const string Cellphone = "Cellphone";
            public const string Gender = "Gender";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string CodeOrg = "CodeOrg";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string DateHire = "DateHire";
            public const string DateEndProbation = "DateEndProbation";
            public const string ProbExtendDate = "ProbExtendDate";
            public const string IDNo = "IDNo";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string WorkPlace = "WorkPlace";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
        }
    }
}
