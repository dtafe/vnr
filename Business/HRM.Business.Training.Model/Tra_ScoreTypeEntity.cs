using System;


namespace HRM.Business.Training.Models
{
    public class Tra_ScoreTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ScoreTypeName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? NumOrder { get; set; }
        public bool? IsTotal { get; set; }
     
    }

    public class Tra_ScoreTypeMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string ScoreTypeName { get; set; }
    }

}

