using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM.Presentation.EmpPortal.Models
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ReNewPassword { get; set; }
    }
}