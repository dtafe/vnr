using System;

namespace HRM.Business.Category.Models
{
    public class Cat_DayOffEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public DateTime DateOff { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public Guid? OrgStructureID { get; set; }
        public bool? IsLeaveDay { get; set; }
        public string OrgStructureName { get; set; }
        public string UserUpdate { get; set; }
        public DateTime? DateUpdate { get; set; }

        public DateTime? DateTo { get; set; }
        private DateTime _dateOff;

        public DateTime Cat_DateOff
        {
            get
            {
                _dateOff = DateOff;
                return _dateOff;
            }
            set
            {
                _dateOff = value;
                DateOff = value;
            }
        }

        private DateTime? _dateTo;
        public DateTime? Cat_DateTo
        {
            get
            {
                _dateTo = DateTo;
                return _dateTo;
            }
            set
            {
                _dateTo = value;
                DateTo = value;
            }
        }
    }
}
