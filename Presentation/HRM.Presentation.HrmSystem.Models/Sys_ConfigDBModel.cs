using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_ConfigDBModel : BaseViewModel
    {
        // Attendance ConfigDB
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        public string Att_ServerName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        public string Att_UserID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        public string Att_Password1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        public string Att_DBName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        public string Att_TableName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        public string Att_CardColName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        public string Att_DataColName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_SerialNumber)]
        public string Att_SerialNumber1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterData)]
        public string Att_FilterData1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterDataSplit)]
        public string Att_FilterDataSplit1 { get; set; }


        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_In_Out)]
        public string Att_In_Out1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CodeAttendance)]
        public string Att_CodeAttendance1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsCodeEmp)]
        public bool? Att_IsCodeEmp1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsActive)]
        public bool? Att_IsActive1 { get; set; }


        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        public string Att_ServerName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        public string Att_UserID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        public string Att_Password2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        public string Att_DBName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        public string Att_TableName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        public string Att_CardColName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        public string Att_DataColName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_SerialNumber)]
        public string Att_SerialNumber2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterData)]
        public string Att_FilterData2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_FilterDataSplit)]
        public string Att_FilterDataSplit2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_In_Out)]
        public string Att_In_Out2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_CodeAttendance)]
        public string Att_CodeAttendance2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsCodeEmp)]
        public bool? Att_IsCodeEmp2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigDB_IsActive)]
        public bool? Att_IsActive2 { get; set; }
        //// Canteen ConfigDB
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        //public string Can_ServerName1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        //public string Can_UserID1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        //public string Can_Password1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        //public string Can_DBName1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        //public string Can_TableName1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        //public string Can_CardColName1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        //public string Can_DataColName1 { get; set; }

        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        //public string Can_ServerName2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        //public string Can_UserID2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        //public string Can_Password2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        //public string Can_DBName2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        //public string Can_TableName2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        //public string Can_CardColName2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        //public string Can_DataColName2 { get; set; }

        //// Laury ConfigDB
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        //public string Lau_ServerName1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        //public string Lau_UserID1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        //public string Lau_Password1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        //public string Lau_DBName1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        //public string Lau_TableName1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        //public string Lau_CardColName1 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        //public string Lau_DataColName1 { get; set; }

        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_ServerName)]
        //public string Lau_ServerName2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_UserName)]
        //public string Lau_UserID2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_Password)]
        //public string Lau_Password2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_DBName)]
        //public string Lau_DBName2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_TableName)]
        //public string Lau_TableName2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_CardColName)]
        //public string Lau_CardColName2 { get; set; }
        //[DisplayName(ConstantDisplay.HRM_System_ConfigDB_DataColName)]
        //public string Lau_DataColName2 { get; set; }

        public partial class FieldNames
        {
            // Attendance ConfigDB
            public const string Att_ServerName1 = "Att_ServerName1";
            public const string Att_UserID1 = "Att_UserID1";
            public const string Att_Password1 = "Att_Password1";
            public const string Att_DBName1 = "Att_DBName1";
            public const string Att_TableName1 = "Att_TableName1";
            public const string Att_CardColName1 = "Att_CardColName1";
            public const string Att_DataColName1 = "Att_DataColName1";

            public const string Att_ServerName2 = "Att_ServerName2";
            public const string Att_UserID2 = "Att_UserID2";
            public const string Att_Password2 = "Att_Password2";
            public const string Att_DBName2 = "Att_DBName2";
            public const string Att_TableName2 = "Att_TableName2";
            public const string Att_CardColName2 = "Att_CardColName2";
            public const string Att_DataColName2 = "Att_DataColName2";

            //// Canteen ConfigDB
            //public const string Can_ServerName1 = "Can_ServerName1";
            //public const string Can_UserID1 = "Can_UserID1";
            //public const string Can_Password1 = "Can_Password1";
            //public const string Can_DBName1 = "Can_DBName1";
            //public const string Can_TableName1 = "Can_TableName1";
            //public const string Can_CardColName1 = "Can_CardColName1";
            //public const string Can_DataColName1 = "Can_DataColName1";

            //public const string Can_ServerName2 = "Can_ServerName2";
            //public const string Can_UserID2 = "Can_UserID2";
            //public const string Can_Password2 = "Can_Password2";
            //public const string Can_DBName2 = "Can_DBName2";
            //public const string Can_TableName2 = "Can_TableName2";
            //public const string Can_CardColName2 = "Can_CardColName2";
            //public const string Can_DataColName2 = "Can_DataColName2";

            //// Laury ConfigDB
            //public const string Lau_ServerName1 = "Lau_ServerName1";
            //public const string Lau_UserID1 = "Lau_UserID1";
            //public const string Lau_Password1 = "Lau_Password1";
            //public const string Lau_DBName1 = "Lau_DBName1";
            //public const string Lau_TableName1 = "Lau_TableName1";
            //public const string Lau_CardColName1 = "Lau_CardColName1";
            //public const string Lau_DataColName1 = "Lau_DataColName1";

            //public const string Lau_ServerName2 = "Lau_ServerName2";
            //public const string Lau_UserID2 = "Lau_UserID2";
            //public const string Lau_Password2 = "Lau_Password2";
            //public const string Lau_DBName2 = "Lau_DBName2";
            //public const string Lau_TableName2 = "Lau_TableName2";
            //public const string Lau_CardColName2 = "Lau_CardColName2";
            //public const string Lau_DataColName2 = "Lau_DataColName2";
        }
    }
}
