using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Laundry.Models
{
    public class Lau_DataTableReportEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string CodeAttendance { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }
        public string TeamCode { get; set; }
        public string SectionCode { get; set; }

        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }

        public List<Guid?> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }

        public string ShiftName { get; set; }
        public List<Guid?> MarkerID { get; set; }
        public string MarkerName { get; set; }
        public List<Guid?> LockerID { get; set; }
        public string LockerName { get; set; }
        public List<Guid?> LineID { get; set; }
        public string LineName { get; set; }

        public int? Price { get; set; }
        public int? Volume { get; set; }
        public int? TotalAmount { get; set; }

        public DateTime? Data1 { get; set; }
        public DateTime? Data2 { get; set; }
        public DateTime? Data3 { get; set; }
        public DateTime? Data4 { get; set; }
        public DateTime? Data5 { get; set; }
        public DateTime? Data6 { get; set; }
        public DateTime? Data7 { get; set; }
        public DateTime? Data8 { get; set; }
        public DateTime? Data9 { get; set; }
        public DateTime? Data10 { get; set; }
        public DateTime? Data11 { get; set; }
        public DateTime? Data12 { get; set; }
        public DateTime? Data13 { get; set; }
        public DateTime? Data14 { get; set; }
        public DateTime? Data15 { get; set; }
        public DateTime? Data16 { get; set; }
        public DateTime? Data17 { get; set; }
        public DateTime? Data18 { get; set; }
        public DateTime? Data19 { get; set; }
        public DateTime? Data20 { get; set; }
        public DateTime? Data21 { get; set; }
        public DateTime? Data22 { get; set; }
        public DateTime? Data23 { get; set; }
        public DateTime? Data24 { get; set; }
        public DateTime? Data25 { get; set; }
        public DateTime? Data26 { get; set; }
        public DateTime? Data27 { get; set; }
        public DateTime? Data28 { get; set; }
        public DateTime? Data29 { get; set; }
        public DateTime? Data30 { get; set; }
        public DateTime? Data31 { get; set; }
        public int? Total { get; set; }

        public DateTime? TimeLog { get; set; }
        public string MachineCode { get; set; }

        public int? CountMonthAmount { get; set; }
        public int? StandardAmount { get; set; }

        public int? AmountClothing { get; set; }
        public int? SumMonthAmount { get; set; }
        public int? ExceedingStandards { get; set; }
        public double? SubtractSalary { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string TimeLog = "TimeLog";
            public const string MachineCode = "MachineCode";

            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string CodeAttendance = "CodeAttendance";

            public const string BranchCode = "BranchCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";

            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";

            public const string Price = "Price";
            public const string Volume = "Volume";
            public const string TotalAmount = "TotalAmount";

            public const string CountMonthAmount = "CountMonthAmount";
            public const string StandardAmount = "StandardAmount";
            public const string AmountClothing = "AmountClothing";
            public const string SumMonthAmount = "SumMonthAmount";
            public const string ExceedingStandards = "ExceedingStandards";
            public const string SubtractSalary = "SubtractSalary";

            public const string ShiftName = "ShiftName";
            public const string MarkerName = "MarkerName";
            public const string LockerName = "LockerName";
            public const string LineName = "LineName";
            public const string MarkerID = "MarkerID";
            public const string LockerID = "LockerID";
            public const string LineID = "LineID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
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
            public const string Total = "Total";
        }
    }
}
