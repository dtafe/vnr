

namespace HRM.Infrastructure.Utilities
{
    public class Constant
    {
        public const string ID = "ID";
        public const string VN = "VN";
        public const string EN = "EN";
        public const string PageSize = "PageSize";
        public const string Close = "Close";
        public const string Code = "Code";
        public const string Settings = "Settings";
        public const string Tracking = "Tracking";
        public const string UserUpdate = "UserUpdate";
        public const string UserCreate = "UserCreate";
        public const string UserCreateID = "UserCreateID";
        public const string DateUpdate = "DateUpdate";
        public const string DateCreate = "DateCreate";
        public const string IsDelete = "IsDelete";
        public const string ServerUpdate = "ServerUpdate";
        public const string ServerCreate = "ServerCreate";
        public const string IPUpdate = "IPUpdate";
        public const string IPCreate = "IPCreate";
        public const string RowIndex = "RowIndex"; 
        public const string ObjectType = "ObjectType";
        public const string Checking = "Checking";
        public const string GrossAmount = "GrossAmount";
        public const string Amount = "Amount";
        public const string IntegerFormat = "IntegerFormat";
        public const string DecimalFormat = "DecimalFormat";
        public const string YearFormat = "YearFormat";
        public const string MonthFormat = "MonthFormat";
        public const string DateFormat = "DateFormat";
        public const string TimeFormat = "TimeFormat";
        public const string DateTimeFormat = "DateTimeFormat";
        public const string LongTimeFormat = "LongTimeFormat";
        public const string ShortTimeFormat = "ShortTimeFormat";
        public const string Languages = "Languages";
        public const string HRM_SYS_COMPUTE_TAMLOG_CAN = "HRM_SYS_COMPUTE_TAMLOG_CAN";
        public const string HRM_SYS_COMPUTE_TAMLOG_LAU = "HRM_SYS_COMPUTE_TAMLOG_LAU";
        public const string ActionStatus = "ActionStatus";
        public const string TempColumn = "TempColumn";

        public const string HRM_CONFIG_MENU_OPTION = "HRM_CONFIG_MENU_OPTION";
        public const string HRM_CONFIG_MODULE_OPTION = "HRM_CONFIG_MODULE_OPTION";
        public const string HRM_CONFIG_DASHBOARD_OPTION = "HRM_CONFIG_DASHBOARD_OPTION";


        public static byte[] IV = {119,178,149,86,166,231,12,219
                                    ,165,25,69,158,206,60,101,131};
        public static byte[] key = {117,8,145,54,145,91,211,62
                                    ,29,91,176,178,56,242,98,196
                                    ,255,162,134,35,78,165,118,64
                                    ,224,220,229,145,138,2,75,109};
    }

    #region SessionObject

    public class SessionObjects
    {
        public const string ScreenWidth = "ScreenWidth";
        public const string UserId = "UserId";
        public const string LoginUserName = "LoginUserName";
        public const string FullName = "FullName";
        public const string UserInfoName = "UserInfoName";
        public const string Sys_User = "Sys_User";
        public const string UsingUserLastCheck = "UsingUserLastCheck";
        public const string ProfileID = "ProfileID";
        public const string ProfileName = "ProfileName";
        public const string Permission = "Permission";
        public const string HasMasterDataGroup = "HasMasterDataGroup";
        public const string IsFirstLogin = "IsFirstLogin";
        public const string LanguageCode = "LanguageCode";
    }

    #endregion

    #region Hreader Object
    public class HeaderObject
    {
        public const string UserID = "UserID";
        public const string UserLogin = "UserLogin";
    }
    #endregion
}
