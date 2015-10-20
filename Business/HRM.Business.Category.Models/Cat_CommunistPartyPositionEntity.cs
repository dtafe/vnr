﻿using System;


namespace HRM.Business.Category.Models
{
    public class Cat_CommunistPartyPositionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string CommunistPartyPositionName { get; set; }
        public string Notes { get; set; }
    }
    public class Cat_CommunistPartyPositionMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string CommunistPartyPositionName { get; set; }
    }
}

