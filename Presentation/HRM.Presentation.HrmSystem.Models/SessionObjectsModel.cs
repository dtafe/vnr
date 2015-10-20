using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class SessionObjectsModel
    {
        public Guid UserID { get; set; }
        public string LoginUserName { get; set; }
        public bool IsActive { get; set; }

        public class FieldsName
        {
            public const string UserID = "UserID";
            public const string LoginUserName = "LoginUserName";
            public const string IsActive = "IsActive";
        }
    }
}
