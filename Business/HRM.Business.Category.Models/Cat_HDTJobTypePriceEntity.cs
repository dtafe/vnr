using System;


namespace HRM.Business.Category.Models
{
    public class Cat_HDTJobTypePriceEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string HDTJobTypePriceName { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> GroupID { get; set; }
        public Nullable<System.Guid> HDTJobTypeID { get; set; }
        public Nullable<double> Price { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public Nullable<System.DateTime> DateEffect { get; set; }
        public string EncryptJob { get; set; }
        public string JobType { get; set; }
        public string StandardElement { get; set; }
        public Nullable<System.DateTime> DateOfEffect { get; set; }
        public string HDTJobTypeCode { get; set; }
        public Guid? HDTJobGroupID { get; set; }
        public string HDTJobGroupName { get; set; }
        public string HDTJobTypeName { get; set; }
        public string Type { get; set; }
        public string TypeView { get; set; }
        public string HDTJobGroupCode { get; set; }
    }
}

