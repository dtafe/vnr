using System;


namespace HRM.Business.Category.Models
{
    public class Cat_HDTJobTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string HDTJobTypeName { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string TypeView { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.Guid> GroupID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Unit { get; set; }
        public string Dept { get; set; }
        public string Part { get; set; }
        public string Line { get; set; }
        public string Session { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public Nullable<System.Guid> HDTJobGroupID { get; set; }
        public string EncryptJob { get; set; }
        public string JobType { get; set; }
        public string StandardElement { get; set; }
        public string HDTJobGroupName { get; set; }
        public Nullable<System.DateTime> DateOfEffect { get; set; }
        public Nullable<System.DateTime> DateEffect { get; set; }
        public string HDTJobGroupCode { get; set; }
        public string HDTJobTypeNameHVN { get; set; }
        public string HDTJobTypeCode { get; set; }
    }

    public class Cat_HDTJobTypeMultiEntity
    {
        public Guid ID { get; set; }
        public string HDTJobTypeName { get; set; }
    }
}

