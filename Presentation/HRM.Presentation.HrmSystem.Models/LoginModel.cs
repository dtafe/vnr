using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class LoginModel
    {
        [DisplayName(ConstantDisplay.HRM_System_User_UserName)]
        public string UserName { get; set; }
        
        [MaxLength(32)]
        [DisplayName(ConstantDisplay.HRM_System_User_Password)]
        public string Password { get; set; }
        public string Language { get; set; }
        [DefaultValue(false)]
        public bool Logout { get; set; }

        public Guid ID { get; set; }

        public bool IsRunScriptNew { get; set; }
        public string VesionLastName { get; set; }

        public bool IsActive { get; set; }
        public bool LoginStatus { get; set; }

        public string FullName { get; set; }    
        public string UserInfoName { get; set; }
        public Guid? ProfileID { get; set; }
        /// <summary>Kiểm tra có dự án không</summary>
        public bool HasMasterDataGroup { get; set; }
        public bool IsFirstLogin { get; set; }
        public Nullable<System.DateTime> DateChangePasssword { get; set; }

        #region Google signIn
        /// <summary>Kiểm tra đăng nhập bằng google API</summary>
        public bool IsGoogleSign { get; set; }
        /// <summary>Google Mail</summary>
        public string GoogleMail { get; set; }
        /// <summary>Tên tài khoản Google </summary>
        public string GoogleProfileName { get; set; }

        #endregion

        //ErrorModel Script
        public List<string> Name { get; set; }
        public List<string> Execption { get; set; }

        public partial class FieldNames
        {           
            public const string UserName = "UserName";          
            public const string Password = "Password";
            public const string ID = "ID";
            public const string IsActive = "IsActive";
            public const string LoginStatus = "LoginStatus";
            public const string ProfileID = "ProfileID";
            public const string Language = "Language";
            public const string IsFirstLogin = "IsFirstLogin";
        }
      
    }
    public class ErrorModel
    {
        public List<string> Name { get; set; }
        public List<string> Execption { get; set; }

        public ErrorModel()
        {
            Name = new List<string>();
            Execption = new List<string>();
        }
    }
}