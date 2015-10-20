using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_ConfigDBCanModel : BaseViewModel
    {
        // Canendance ConfigDB
        // Server 1
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        public string Can_ServerName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        public string Can_UserID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        public string Can_Password1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        public string Can_DBName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        public string Can_TableName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        public string Can_CardColName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]        
        public string Can_DataColName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_SerialNumber)]
        public string Can_SerialNumber1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_In_Out)]
        public string Can_In_Out1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsCodeEmp)]
        public bool? Can_IsCodeEmp1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsActive)]
        public bool? Can_IsActive1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterData)]
        public string Can_FilterData1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterDataSplit)]
        public string Can_FilterDataSplit1 { get; set; }
        // Server 2
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        public string Can_ServerName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        public string Can_UserID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        public string Can_Password2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        public string Can_DBName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        public string Can_TableName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        public string Can_CardColName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        public string Can_DataColName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_SerialNumber)]
        public string Can_SerialNumber2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_In_Out)]
        public string Can_In_Out2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsCodeEmp)]
        public bool? Can_IsCodeEmp2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsActive)]
        public bool? Can_IsActive2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterData)]
        public string Can_FilterData2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterDataSplit)]
        public string Can_FilterDataSplit2 { get; set; }
        public partial class FieldNames
        {
            // Canteen ConfigDB
            // Server 1
            public const string Can_ServerName1 = "Can_ServerName1";
            public const string Can_UserID1 = "Can_UserID1";
            public const string Can_Password1 = "Can_Password1";
            public const string Can_DBName1 = "Can_DBName1";
            public const string Can_TableName1 = "Can_TableName1";
            public const string Can_CardColName1 = "Can_CardColName1";
            public const string Can_DataColName1 = "Can_DataColName1";
            // Server 2
            public const string Can_ServerName2 = "Can_ServerName2";
            public const string Can_UserID2 = "Can_UserID2";
            public const string Can_Password2 = "Can_Password2";
            public const string Can_DBName2 = "Can_DBName2";
            public const string Can_TableName2 = "Can_TableName2";
            public const string Can_CardColName2 = "Can_CardColName2";
            public const string Can_DataColName2 = "Can_DataColName2";
        }
    }
}
