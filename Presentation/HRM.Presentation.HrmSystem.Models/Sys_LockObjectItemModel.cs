using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_LockObjectItemModel : BaseViewModel
    {
        public System.Guid LockObjectID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ObjectName)]
        public string ObjectName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public System.DateTime DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public System.DateTime DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Comment)]
        public string Commend { get; set; }

        private Guid _id = Guid.Empty;
        public Guid LockObjectItem_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }

        public partial class FieldNames
        {
            public const string LockObjectID = "LockObjectID";
            public const string ObjectName = "ObjectName";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Type = "Type";
            public const string Commend = "Commend";
            public const string DateUpdate = "DateUpdate";
            public const string UserCreate = "UserCreate";
        }
    }
}
