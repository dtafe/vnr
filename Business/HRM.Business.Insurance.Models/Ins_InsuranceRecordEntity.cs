using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    public class Ins_InsuranceRecordEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public System.Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string InsuranceType { get; set; }
        public string InsuranceTypeTrans { get; set; }
        public System.DateTime RecordDate { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<System.DateTime> DateSuckle { get; set; }
        public string TypeSuckle { get; set; }
        public string TypeSick { get; set; }
        public Nullable<System.DateTime> DateStartWorking { get; set; }
        public double DayCount { get; set; }
        public Nullable<double> DayCountOld { get; set; }
        public Nullable<double> LeaveInYear { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public Nullable<System.Guid> RelativesID { get; set; }
        public string RelativesName { get; set; }
        public Nullable<System.Guid> ChildSickID { get; set; }
        public string ChildSickName { get; set; }
        public string ProfileIds { get; set; }
        public string OrgStructureIDs { get; set; }
        public string TypeData { get; set; }
    }
}
