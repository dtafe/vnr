using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_TAMEntity : HRMBaseModel
    {
        public List<int> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public List<int?> OrgStructureID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CardCode { get; set; }
        public DateTime? TimeLog { get; set; }
        public string Status { get; set; }
        public bool StatusCheck { get; set; }
        public string StatusView { get; set; }
    }

    public class Att_TAMEntityForDelete
    {
        public string CardCode { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
