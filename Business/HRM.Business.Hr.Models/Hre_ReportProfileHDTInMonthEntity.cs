using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportProfileHDTInMonthEntity : HRMBaseModel
    {
        public DateTime? Month { get; set; }
        public string OrgStructureIDs { get; set; }
        public Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string Unit { get; set; }
        public string Dept { get; set; }
        public string Part { get; set; }
        public string Line { get; set; }
        public string Session { get; set; }
        public string HDTJobTypeName { get; set; }
        public string HDTJobTypeNameHVN { get; set; }
        public string StandardElement { get; set; }
        public string EncryptJob { get; set; }
        public string HDTJobGroupName { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string Data3 { get; set; }
        public string Data4 { get; set; }
        public string Data5 { get; set; }
        public string Data6 { get; set; }
        public string Data7 { get; set; }
        public string Data8 { get; set; }
        public string Data9 { get; set; }
        public string Data10 { get; set; }
        public string Data11 { get; set; }
        public string Data12 { get; set; }
        public string Data13 { get; set; }
        public string Data14 { get; set; }
        public string Data15 { get; set; }
        public string Data16 { get; set; }
        public string Data17 { get; set; }
        public string Data18 { get; set; }
        public string Data19 { get; set; }
        public string Data20 { get; set; }
        public string Data21 { get; set; }
        public string Data22 { get; set; }
        public string Data23 { get; set; }
        public string Data24 { get; set; }
        public string Data25 { get; set; }
        public string Data26 { get; set; }
        public string Data27 { get; set; }
        public string Data28 { get; set; }
        public string Data29 { get; set; }
        public string Data30 { get; set; }
        public string Data31 { get; set; }
        public Nullable<double> Price { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }


        public partial class FieldNames
        {
            public const string Month = "Month";
            public const string OrgStructureIDs = "OrgStructureIDs";
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Unit = "Unit";
            public const string Dept = "Dept";
            public const string Part = "Part";
            public const string Line = "Line";
            public const string Price = "Price";
            public const string Session = "Session";
            public const string HDTJobTypeName = "HDTJobTypeName";
            public const string HDTJobTypeNameHVN = "HDTJobTypeNameHVN";
            public const string StandardElement = "StandardElement";
            public const string EncryptJob = "EncryptJob";
            public const string HDTJobGroupName = "HDTJobGroupName";
            public const string Data = "Data";
            public const string Data1 = "Data1";
            public const string Data2 = "Data2";
            public const string Data3 = "Data3";
            public const string Data4 = "Data4";
            public const string Data5 = "Data5";
            public const string Data6 = "Data6";
            public const string Data7 = "Data7";
            public const string Data8 = "Data8";
            public const string Data9 = "Data9";
            public const string Data10 = "Data10";
            public const string Data11 = "Data11";
            public const string Data12 = "Data12";
            public const string Data13 = "Data13";
            public const string Data14 = "Data14";
            public const string Data15 = "Data15";
            public const string Data16 = "Data16";
            public const string Data17 = "Data17";
            public const string Data18 = "Data18";
            public const string Data19 = "Data19";
            public const string Data20 = "Data20";
            public const string Data21 = "Data21";
            public const string Data22 = "Data22";
            public const string Data23 = "Data23";
            public const string Data24 = "Data24";
            public const string Data25 = "Data25";
            public const string Data26 = "Data26";
            public const string Data27 = "Data27";
            public const string Data28 = "Data28";
            public const string Data29 = "Data29";
            public const string Data30 = "Data30";
            public const string Data31 = "Data31";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";


        }
    }
}
