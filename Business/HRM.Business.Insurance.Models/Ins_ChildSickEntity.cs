using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    public class Ins_ChildSickEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public System.Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string ChildSickName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<System.Guid> RelativeID { get; set; }
        public string RelativeName { get; set; }
        public string ProfileIDs { get; set; }
       
    }
    public class Ins_ChildSickMultiEntity
    {
        public Guid ID { get; set; }
        public string ChildSickName { get; set; }
    }
}
