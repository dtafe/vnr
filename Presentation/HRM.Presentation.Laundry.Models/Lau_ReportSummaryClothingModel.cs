using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System.Collections.Generic;

namespace HRM.Presentation.Laundry.Models
{
    public class Lau_ReportSummaryClothingModel : BaseViewModel
    {
        public DateTime? TimeLog { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public List<Guid?> ShiftID { get; set; }
        public string ShiftName { get; set; }
        public int? Total { get; set; }

        public List<Guid?> MarkerID { get; set; }
        public string MarkerName { get; set; }
        public List<Guid?> LockerID { get; set; }
        public string LockerName { get; set; }
        public List<Guid?> LineID { get; set; }
        public string LineName { get; set; }

        public double? Data1 { get; set; }
        public double? Data2 { get; set; }
        public double? Data3 { get; set; }
        public double? Data4 { get; set; }
        public double? Data5 { get; set; }
        public double? Data6 { get; set; }
        public double? Data7 { get; set; }
        public double? Data8 { get; set; }
        public double? Data9 { get; set; }
        public double? Data10 { get; set; }
        public double? Data11 { get; set; }
        public double? Data12 { get; set; }
        public double? Data13 { get; set; }
        public double? Data14 { get; set; }
        public double? Data15 { get; set; }
        public double? Data16 { get; set; }
        public double? Data17 { get; set; }
        public double? Data18 { get; set; }
        public double? Data19 { get; set; }
        public double? Data20 { get; set; }
        public double? Data21 { get; set; }
        public double? Data22 { get; set; }
        public double? Data23 { get; set; }
        public double? Data24 { get; set; }
        public double? Data25 { get; set; }
        public double? Data26 { get; set; }
        public double? Data27 { get; set; }
        public double? Data28 { get; set; }
        public double? Data29 { get; set; }
        public double? Data30 { get; set; }
        public double? Data31 { get; set; }

        public partial class FieldNames
        {
            public const string TimeLog = "TimeLog";
            public const string Total = "Total";
            public const string ShiftID = "ShiftID";
            public const string ShiftName = "ShiftName";
            public const string MarkerName = "MarkerName";
            public const string LockerName = "LockerName";
            public const string LineName = "LineName";
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
        }
    }
}
