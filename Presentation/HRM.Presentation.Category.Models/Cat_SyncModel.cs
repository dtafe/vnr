using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SyncModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_SyncName)]
        public string SyncName { get; set; }
          [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_IsFromInner)]
        public bool? IsFromInner { get; set; }
           [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_ServerName)]
        public string ServerName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_UserName)]
        public string UserName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_PassWord)]
        public string PassWord { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_DatabaseName)]
        public string DatabaseName { get; set; }
           [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_OuterTable)]
        public string OuterTable { get; set; }
          [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_InnerTable)]
        public string InnerTable { get; set; }
         [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_Description)]
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string SyncName = "SyncName";
            public const string IsFromInner = "IsFromInner";
            public const string ServerName = "ServerName";
            public const string UserName = "UserName";
            public const string PassWord = "PassWord";
            public const string DatabaseName = "DatabaseName";
            public const string OuterTable = "OuterTable";
            public const string InnerTable = "InnerTable";
            public const string Description = "Description";
        }
    }

    public class Cat_SyncSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_SyncName)]
        public string SyncName { get; set; }
        public string OuterTable { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_InnerTable)]
        public string InnerTable { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Cat_Sync_Description)]
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
   
}
