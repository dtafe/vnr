using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Recruitment.Models
{
    public class HistoryApprove 
    {
        private Guid _Id;
        public Guid ID
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }

        }
            private string _userApprove;
            public string UserApprove
            {
                get
                {
                    return _userApprove;
                }
                set
                {
                    _userApprove = value;
                }
            }
            private string _userModified;
            public string UserModified
            {
                get
                {
                    return _userModified;
                }
                set
                {
                    _userModified = value;
                }
            }
            private string _change;
            public string Changes
            {
                get
                {
                    return _change;
                }
                set
                {
                    _change = value;
                }
            }
            private string _status;
            public string Status
            {
                get
                {
                    return _status;
                }
                set
                {
                    _status = value;
                }
            }
            private string _dateApprove;
            public string DateApprove
            {
                get
                {
                    return _dateApprove;
                }
                set
                {
                    _dateApprove = value;
                }
            }
            private string _waitUserApprove;
            public string WaitUserApprove
            {
                get
                {
                    return _waitUserApprove;
                }
                set
                {
                    _waitUserApprove = value;
                }
            }
            public class FieldNames
            {
                public const string DateApprove = "DateApprove";
                public const string UserApprove = "UserApprove";
                public const string Status = "Status";
                public const string WaitUserApprove = "WaitUserApprove";
                public const string Changes = "Changes";
            }
        }
    

   
    
}
