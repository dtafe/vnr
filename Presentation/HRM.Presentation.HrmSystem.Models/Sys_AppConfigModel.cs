using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_AppConfigModel : BaseViewModel
    {
        public string Code { get; set; }
        public string Info { get; set; }
        public string Operator { get; set; }
        public string Value52 { get; set; }
        public string Value53 { get; set; }
        public string Value54 { get; set; }
        public string Value55 { get; set; }
        public string Value56 { get; set; }
        public string Value57 { get; set; }
        public string Value58 { get; set; }
        public string Value59 { get; set; }

        

       
        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string Info = "Info";
            public const string Operator = "Operator";
            public const string Value52 = "Value2";
            public const string Value53 = "Value3";
            public const string Value54 = "Value4";
            public const string Value55 = "Value5";
            public const string Value56 = "Value6";
            public const string Value57 = "Value7";
            public const string Value58 = "Value8";
            public const string Value59 = "Value9";
        }

    }

}
