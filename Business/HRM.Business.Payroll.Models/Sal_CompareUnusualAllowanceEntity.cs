using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Sal_CompareUnusualAllowanceEntity : HRMBaseModel
    {
        public string UnusualAllowanceCfgName { get; set; }
        public string EDEDUCTION1 { get; set; }
        public string EDEDUCTION2 { get; set; }
        public string EEARING1 { get; set; }
        public string EEARING2 { get; set; }

        

        public partial class FieldNames
        {
            public const string UnusualAllowanceCfgName = "UnusualAllowanceCfgName";
            public const string EDEDUCTION1 = "EDEDUCTION1";
            public const string EDEDUCTION2 = "EDEDUCTION2";
            public const string EEARING1 = "EEARING1";
            public const string EEARING2 = "EEARING2";
        
           
        }
    }
}
