using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    public class CustomWorkDayEntity
    {
        public Guid ProfileID { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime? InTime1 { get; set; }
        public DateTime? OutTime1 { get; set; }
    }
}
