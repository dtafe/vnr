using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_ConfigDBLauModel : BaseViewModel
    {
        // Attendance ConfigDB
        // Server 1
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        public string Lau_ServerName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        public string Lau_UserID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        public string Lau_Password1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        public string Lau_DBName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        public string Lau_TableName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        public string Lau_CardColName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        public string Lau_DataColName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_SerialNumber)]
        public string Lau_SerialNumber1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_In_Out)]
        public string Lau_In_Out1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsCodeEmp)]
        public bool? Lau_IsCodeEmp1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsActive)]
        public bool? Lau_IsActive1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterData)]
        public string Lau_FilterData1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterDataSplit)]
        public string Lau_FilterDataSplit1 { get; set; }

        // Server 2
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        public string Lau_ServerName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        public string Lau_UserID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        public string Lau_Password2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        public string Lau_DBName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        public string Lau_TableName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        public string Lau_CardColName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        public string Lau_DataColName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_SerialNumber)]
        public string Lau_SerialNumber2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_In_Out)]
        public string Lau_In_Out2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsCodeEmp)]
        public bool? Lau_IsCodeEmp2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsActive)]
        public bool? Lau_IsActive2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterData)]
        public string Lau_FilterData2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterDataSplit)]
        public string Lau_FilterDataSplit2 { get; set; }

        public partial class FieldNames
        {
            // Laury ConfigDB
            // Server 1
            public const string Lau_ServerName1 = "Lau_ServerName1";
            public const string Lau_UserID1 = "Lau_UserID1";
            public const string Lau_Password1 = "Lau_Password1";
            public const string Lau_DBName1 = "Lau_DBName1";
            public const string Lau_TableName1 = "Lau_TableName1";
            public const string Lau_CardColName1 = "Lau_CardColName1";
            public const string Lau_DataColName1 = "Lau_DataColName1";
            // Server 2
            public const string Lau_ServerName2 = "Lau_ServerName2";
            public const string Lau_UserID2 = "Lau_UserID2";
            public const string Lau_Password2 = "Lau_Password2";
            public const string Lau_DBName2 = "Lau_DBName2";
            public const string Lau_TableName2 = "Lau_TableName2";
            public const string Lau_CardColName2 = "Lau_CardColName2";
            public const string Lau_DataColName2 = "Lau_DataColName2";
        }
    }
}
