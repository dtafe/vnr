using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ReportInsuranceTotalEntity : HRMBaseModel
    {
        //
        public int Stt {get;set;}
        public string Name {get;set;}
        public int TotalEmp {get;set;}
        public double? BHXH {get;set;}
        public double? BHYT {get;set;}
        public double? BHTN { get; set; }
        public double? TotalAll { get; set; }
        public partial class FieldNames
        {
            public const string Stt = "Stt";
            public const string Name = "Name";
            public const string TotalEmp = "TotalEmp";
            public const string BHXH = "BHXH";
            public const string BHYT = "BHYT ";
            public const string BHTN = "BHTN";
            public const string TotalAll = "TotalAll";
        }
        
    }
}
