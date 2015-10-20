using HRM.Business.BaseModel;
using System;
using System.ComponentModel;

namespace HRM.Business.Attendance.Models
{
    public class Att_AnalysysLeaveAndLateEarlyEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public DateTime? WorkDate { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string CodeLeaveDayType { get; set; }
        public string StatusLeaveDay { get; set; }
        public string ShiftName { get; set; }
        /// <summary>
        /// Về Sớm
        /// </summary>
        public double? Early { get; set; }
        /// <summary>
        /// Về Sớm( Làm Tròn)
        /// </summary>
        public double? RoundEarly { get; set; }
        /// <summary>
        /// Vào Trễ
        /// </summary>
        public double? Late { get; set; }
        /// <summary>
        /// Vào Trễ( Làm Tròn)
        /// </summary>
        public double? RoundLate { get; set; }
        /// <summary>
        /// Trễ Sớm( Phút)
        /// </summary>
        public double? LateEarly { get; set; }
        private string _udLeavedayCode1;
        public string udLeavedayCode1
        {
            get
            {
                return _udLeavedayCode1;
            }
            set
            {
                _udLeavedayCode1 = value;
            }
        }
        private string _udLeavedayCode2;
        public string udLeavedayCode2
        {
            get
            {
                return _udLeavedayCode2;
            }
            set
            {
                _udLeavedayCode2 = value;
            }
        }

        private string _udLeavedayStatus1;
        public string udLeavedayStatus1
        {
            get
            {
                return _udLeavedayStatus1;
            }
            set
            {
                _udLeavedayStatus1 = value;
            }
        }
        private string _udLeavedayStatus2;
        public string udLeavedayStatus2
        {
            get
            {
                return _udLeavedayStatus2;
            }
            set
            {
                _udLeavedayStatus2 = value;
            }
        }
      
    }
}
