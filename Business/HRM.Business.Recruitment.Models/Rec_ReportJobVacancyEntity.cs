using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_ReportJobVacancyEntity : HRMBaseModel
    {
        public string OrgStructureName { get; set; }
        public string PositionName { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string SalaryRankName { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> DateProposal { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<System.DateTime> DateCheck { get; set; }
        public partial class FieldNames
        {
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string Quantity = "Quantity";
            public const string SalaryRankName = "SalaryRankName";
            public const string Type = "Type";
            public const string DateProposal = "DateProposal";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string DateCheck = "DateCheck";
        }
    }
}
