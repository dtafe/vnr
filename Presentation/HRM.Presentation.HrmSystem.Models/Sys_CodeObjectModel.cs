using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_CodeObjectSearchModel
    {
        public string TableName { get; set; }
    }

    public class Sys_CodeObjectModel : BaseViewModel
    {
        public string Code { get; set; }
        public string TableName { get; set; }
        public string Prefix { get; set; }
        public Nullable<int> Ordinal { get; set; }
        public Nullable<bool> IsResetByDate { get; set; }
        public Nullable<bool> IsResetByUser { get; set; }
        public string FieldName { get; set; }
        public Nullable<bool> IsResetByMonth { get; set; }
        public Nullable<bool> IsResetByYear { get; set; }
        public Nullable<bool> IsResetByObject { get; set; }
        public string ResetObjectName { get; set; }
        public string ResetFieldName { get; set; }
        public Nullable<bool> IsManual { get; set; }
        public string ResetObjectName1 { get; set; }
        public string ResetFieldName1 { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string TableName = "TableName";
            public const string Prefix = "Prefix";
            public const string Ordinal = "Ordinal";
            public const string IsResetByDate = "IsResetByDate";
            public const string IsResetByUser = "IsResetByUser";
            public const string ServerUpdate = "ServerUpdate";
            public const string ServerCreate = "ServerCreate";
            public const string UserUpdate = "UserUpdate";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string DateLock = "DateLock";
            public const string IsDelete = "IsDelete";
            public const string IPCreate = "IPCreate";
            public const string FieldName = "FieldName";
            public const string IsResetByMonth = "IsResetByMonth";
            public const string IsResetByYear = "IsResetByYear";
            public const string IsResetByObject = "IsResetByObject";
            public const string ResetObjectName = "ResetObjectName";
            public const string ResetFieldName = "ResetFieldName";
            public const string IsManual = "IsManual";
            public const string ResetObjectName1 = "ResetObjectName1";
            public const string ResetFieldName1 = "ResetFieldName1";
        }
    }
}
