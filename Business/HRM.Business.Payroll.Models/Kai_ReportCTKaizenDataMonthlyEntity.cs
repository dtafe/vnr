using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Payroll.Models
{
    public class Kai_ReportCTKaizenDataMonthlyEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public Nullable<System.DateTime> Month { get; set; }
        public Nullable<double> SumAmount { get; set; }
        public string OrgStructureName { get; set; }

         public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Month = "Month";
            public const string OrgStructureName = "OrgStructureName";
            public const string SumAmount = "SumAmount";
        }
    }
}
