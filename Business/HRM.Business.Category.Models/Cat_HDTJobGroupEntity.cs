using System;


namespace HRM.Business.Category.Models
{
    public class Cat_HDTJobGroupEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string HDTJobGroupName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
    }

    public class Cat_HDTJobGroupMultiEntity
    {
        public Guid ID { get; set; }
        public string HDTJobGroupName { get; set; }
    }
    public class Cat_HDTJobGroupCodeMultiEntity
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string HDTJobGroupName { get; set; }
        public int TotalRow { get; set; }
    }


}

