using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ReportEmpHardJoblEntity : HRMBaseModel
    {
        //
        public int Stt {get;set;}
        public string CodeEmp {get;set;}
        public string ProfileName {get;set;}

        public partial class FieldNames
        {
            public const string Stt = "Stt";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
        }
        
    }
}
