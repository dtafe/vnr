using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Canteen.Models
{
    public class Can_ReportMultiSlIDeCardEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserPrint { get; set; }
        public DateTime? DatePrint { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public List<Guid?> CateringID { get; set; }
        public string CateringName { get; set; }
        public List<Guid?> CanteenID { get; set; }
        public string CanteenName { get; set; }
        public List<Guid?> LineID { get; set; }
        public string LineName { get; set; }
        public List<Guid?> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
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
        public DateTime? DateExport { get; set; }
        public double? SumCardMore { get; set; }

        //public int SumCardMore1 { get; set; }
        //public int SumCardMore2 { get; set; }
        //public int SumCardMore3 { get; set; }
        //public int SumCardMore4 { get; set; }
        //public int SumCardMore5 { get; set; }
        //public int SumCardMore6 { get; set; }
        //public int SumCardMore7 { get; set; }
        //public int SumCardMore8 { get; set; }
        //public int SumCardMore9 { get; set; }
        //public int SumCardMore10 { get; set; }
        //public int SumCardMore11 { get; set; }
        //public int SumCardMore12 { get; set; }
        //public int SumCardMore13 { get; set; }
        //public int SumCardMore14 { get; set; }
        //public int SumCardMore15 { get; set; }
        //public int SumCardMore16 { get; set; }
        //public int SumCardMore17 { get; set; }
        //public int SumCardMore18 { get; set; }
        //public int SumCardMore19 { get; set; }
        //public int SumCardMore20 { get; set; }
        //public int SumCardMore21 { get; set; }
        //public int SumCardMore22 { get; set; }
        //public int SumCardMore23 { get; set; }
        //public int SumCardMore24 { get; set; }
        //public int SumCardMore25 { get; set; }
        //public int SumCardMore26 { get; set; }
        //public int SumCardMore27 { get; set; }
        //public int SumCardMore28 { get; set; }
        //public int SumCardMore29 { get; set; }
        //public int SumCardMore30 { get; set; }
        //public int SumCardMore31 { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; } 
        public partial class FieldNames
        {
            public const string DateExport = "DateExport";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Division = "Division";
            public const string Department = "Department";
            public const string Section = "Section";
            public const string CateringName = "CateringName";
            public const string CanteenName = "CanteenName";
            public const string LineName = "LineName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
            public const string OrgStructureName = "OrgStructureName";
            public const string Data1  = "Data1";
            public const string Data2  = "Data2";
            public const string Data3  = "Data3";
            public const string Data4 =  "Data4";
            public const string Data5  = "Data5";
            public const string Data6  = "Data6";
            public const string Data7  = "Data7";
            public const string Data8  = "Data8";
            public const string Data9  = "Data9";
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

            //public const string SumCardMore1 = "SumCardMore1";
            //public const string SumCardMore2 = "SumCardMore2";
            //public const string SumCardMore3 = "SumCardMore3";
            //public const string SumCardMore4 = "SumCardMore4";
            //public const string SumCardMore5 = "SumCardMore5";
            //public const string SumCardMore6 = "SumCardMore6";
            //public const string SumCardMore7 = "SumCardMore7";
            //public const string SumCardMore8 = "SumCardMore8";
            //public const string SumCardMore9 = "SumCardMore9";
            //public const string SumCardMore10 = "SumCardMore10";
            //public const string SumCardMore11 = "SumCardMore11";
            //public const string SumCardMore12 = "SumCardMore12";
            //public const string SumCardMore13 = "SumCardMore13";
            //public const string SumCardMore14 = "SumCardMore14";
            //public const string SumCardMore15 = "SumCardMore15";
            //public const string SumCardMore16 = "SumCardMore16";
            //public const string SumCardMore17 = "SumCardMore17";
            //public const string SumCardMore18 = "SumCardMore18";
            //public const string SumCardMore19 = "SumCardMore19";
            //public const string SumCardMore20 = "SumCardMore20";
            //public const string SumCardMore21 = "SumCardMore21";
            //public const string SumCardMore22 = "SumCardMore22";
            //public const string SumCardMore23 = "SumCardMore23";
            //public const string SumCardMore24 = "SumCardMore24";
            //public const string SumCardMore25 = "SumCardMore25";
            //public const string SumCardMore26 = "SumCardMore26";
            //public const string SumCardMore27 = "SumCardMore27";
            //public const string SumCardMore28 = "SumCardMore28";
            //public const string SumCardMore29 = "SumCardMore29";
            //public const string SumCardMore30 = "SumCardMore30";
            //public const string SumCardMore31 = "SumCardMore31";

            public const string SumCardMore = "SumCardMore";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string Amount = "Amount";
            public const string Date = "Date";
            public const string Hour = "Hour";

        }
    }
}
