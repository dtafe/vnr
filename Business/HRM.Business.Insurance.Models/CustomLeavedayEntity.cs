using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    public class CustomLeavedayEntity
    {
        public Guid ProfileID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Guid? LeaveDayTypeID { get; set; }
    }
}
