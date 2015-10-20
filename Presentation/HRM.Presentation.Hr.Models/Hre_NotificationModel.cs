using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_NotificationModel
    {
        private int? _countOvertime;
        private int? _countLeaveday;
        private int? _countRoster;

        public Guid ID { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
        public Guid? ProfileID { get; set; }
        public string FullName { get; set; }
        public string ProfileName { get; set; }
        public string WorkingPlace { get; set; }
        public string EmployeeTypeName { get; set; }
        public string JobTitleName { get; set; }
        public Guid? OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public string LanguageCode { get; set; }
        public int? CountOvertime
        {
            get
            {
                if (_countOvertime == null)
                {
                    _countOvertime = 0;
                }
                return _countOvertime;
            }
            set
            {
                _countOvertime = value;
            }
        }
        public int? CountLeaveday
        {
            get
            {
                if (_countLeaveday == null)
                {
                    _countLeaveday = 0;
                }
                return _countLeaveday;
            }
            set
            {
                _countLeaveday = value;
            }
        }
        public int? CountRoster
        {
            get
            {
                if (_countRoster == null)
                {
                    _countRoster = 0;
                }
                return _countRoster;
            }
            set
            {
                _countRoster = value;
            }
        }
        public string ActionStatus { get; set; }
    }
}
