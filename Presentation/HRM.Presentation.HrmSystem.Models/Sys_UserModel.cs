using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_UserMultiModel
    {
        public Guid ID { get; set; }
        public string UserInfoName { get; set; }
        public string UserLogin { get; set; }
    }

    public class UserPasswordResetModel
    {
        public string UserInfoName { get; set; }
        public string UserLogin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Sys_UserModel : BaseViewModel
    {
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_User_Code)]
        public string Code { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_User_UserType)]
        public string UserType { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_UserName)]
        public string UserInfoName { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_UserName)]
        public string UserInfoName1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_Email)]
        public string Email { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_EffectedDate)]
        public DateTime? EffectedDate { get; set; }

        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_System_User_LoginName)]
        public string UserLogin { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_User_Password)]
        public string Password { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_DateChangePass)]
        public DateTime? DateChangePass { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_DateExpiredPass)]
        public DateTime? DateExpiredPass { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_IsActivate)]
        public bool IsActivate { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_IsCheckLDAP)]
        public bool? IsCheckLDAP { get; set; }

        [MaxLength(1000)]
        [DisplayName(ConstantDisplay.HRM_System_User_Description)]
        public string Description { get; set; }

        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_System_User_LDAPDatasource)]
        public string LDAPDatasource { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_GroupPermission)]
        public string GroupPermission { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_ProfileID)]
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_User_MasterDataGroup)]
        public List<Guid?> MasterDataGroupID { get; set; }
        public string MasterDataGroupIDstr { get; set; }

        public string ProfileName { get; set; }
        public string FullName { get; set; }

        /// <summary>Dùng để tự load ds dự án lên multiSelect control</summary>
        public Dictionary<Guid, string> MasterDataGroups { get; set; }

        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string UserType = "UserType";
            public const string UserInfoName = "UserInfoName";
            public const string UserInfoName1 = "UserInfoName1";
            public const string UserLogin = "UserLogin";
            public const string Password = "Password";
            public const string DateChangePass = "DateChangePass";
            public const string DateExpiredPass = "DateExpiredPass";
            public const string IsActivate = "IsActivate";
            public const string IsCheckLDAP = "IsCheckLDAP";
            public const string Description = "Description";
            public const string LDAPDatasource = "LDAPDatasource";
            public const string ProfileID = "ProfileID";
            public const string EffectedDate = "EffectedDate";
            public const string Email = "Email";
            public const string ProfileName = "ProfileName";
            public const string GroupPermission = "GroupPermission";
        }      
    }

    public class Sys_UserChangePasswordModel : BaseViewModel
    {
        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_System_User_LoginName)]
        public string UserLogin { get; set; }

        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_System_User_LoginName)]
        public string UserInfoName { get; set; }
        

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_User_OldPassword)]
        public string OldPassword { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_User_NewPassword)]
        public string NewPassword { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_User_ConfirmPassword)]
        public string ConfirmPassword { get; set; }
    }


    public class Sys_UserSearchModel 
    {
        public string UserInfoName { get; set; }
        public string UserLogin { get; set; }
    }
}