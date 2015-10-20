
using System;

namespace HRM.Business.HrmSystem.Models
{

    public class Sys_UserMultiEntity
    {
        public Guid ID { get; set; }
        public string UserInfoName { get; set; }
        public string UserLogin { get; set; }
    }

    public class UserPasswordResetEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserInfoName { get; set; }
        public string UserLogin { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }

    }

    public class Sys_UserInfoEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string UserType { get; set; }
        public string UserInfoName { get; set; }
        public string UserLogin { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> DateChangePasssword { get; set; }
        public DateTime? DateExpiredPass { get; set; }
        public bool IsActivate { get; set; }
        public bool? IsCheckLDAP { get; set; }
        public string Description { get; set; }
        public int? GroupUserID { get; set; }
        public Guid? ProfileID { get; set; }
        public string LDAPDatasource { get; set; }
        public DateTime? LastLogin { get; set; }
        public string GroupPermission { get; set; }
        public string Email { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? EffectedDate { get; set; }
        public bool HasMasterDataGroup { get; set; }
        public bool IsRunScriptNew { get; set; }
        public string VesionLastName { get; set; }

    }
}
