using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ReportNotHaveSocialEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public System.Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string PositionName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string JobTitleName { get; set; }
        public string EthnicGroupName { get; set; }
        public string SocialInsNo { get; set; }
        public string LaborBookNo { get; set; }
        public string IDPOI_Code { get; set; }
        public string IDPOI { get; set; }
        public string Origin { get; set; }
    
        public DateTime? DateQuit { get; set; }
        public string IDNo { get; set; }
        
        public string IDPlaceOfIssue { get; set; }
        public DateTime? IDDateOfIssue { get; set; }
        public string TAddress { get; set; }
        public string PAddress { get; set; }
        public string CodeEmp { get; set; }
        public string OrgStructureName { get; set; }
        public string HealthTreatmentPlace { get; set; }
        public string HealthTreatmentPlaceCode { get; set; }
        public string PlaceOfTreatmentProvince { get; set; }
        public DateTime? DateHire { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string OrgStructureID = "OrgStructureID";
            public const string ProfileName = "ProfileName";
            public const string PositionName = "PositionName";
            public const string Gender = "Gender";
            public const string DateOfBirth = "DateOfBirth";
            public const string JobTitleName = "JobTitleName";
            public const string EthnicGroupName = "EthnicGroupName";
            public const string SocialInsNo = "SocialInsNo";
            public const string LaborBookNo = "LaborBookNo";
            public const string IDNo = "IDNo";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string TAddress = "TAddress";
            public const string PAddress = "PAddress";
            public const string CodeEmp = "CodeEmp";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateHire = "DateHire";
            public const string HealthTreatmentPlace = "HealthTreatmentPlace";
            public const string HealthTreatmentPlaceCode = "HealthTreatmentPlaceCode";
            public const string PlaceOfTreatmentProvince = "PlaceOfTreatmentProvince";
            public const string IDPOI_Code = "IDPOI_Code";
            public const string IDPOI = "IDPOI";
            public const string Origin = "Origin";
            public const string DateQuit = "DateQuit";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

          
        }
    }
   
}
