using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_CandidateHistoryEntity : HRMBaseModel
    {
        public string ProfileName { get; set; }
        public int TotalRow { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string WordHistoryName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Position { get; set; }
        public string PositionLast { get; set; }
        public Nullable<double> SalaryLast { get; set; }
        public string Tasks { get; set; }
        public string JobDescription { get; set; }
        public string Phone { get; set; }
        public string BussinessType { get; set; }
        public string Salary { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateFinish { get; set; }
        public string Comment { get; set; }
        public string Code { get; set; }
        public string SupPosition { get; set; }
        public string SupName { get; set; }
        public string SupMobile { get; set; }
        public string SupEmail { get; set; }
        public string SupCompany { get; set; }
        public string SupComment { get; set; }
        public string SupRelation { get; set; }
        public string ResignReason { get; set; }

        private Guid _id = Guid.Empty;
        public Guid CandidateHistory_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        public Nullable<System.Guid> CandidateID { get; set; }
        public string CandidateName { get; set; }
        public Nullable<System.Guid> CountryID { get; set; }
        public string CountryName { get; set; }
        public Nullable<System.Guid> ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public Nullable<System.Guid> LastPositionID { get; set; }
        public string LastPositionName { get; set; }
        public Nullable<System.Guid> FirstPositionID { get; set; }
        public string FirstPositionName { get; set; }
        public string PositionFirst { get; set; }
        public Nullable<double> SalaryFirst { get; set; }
    }
}
