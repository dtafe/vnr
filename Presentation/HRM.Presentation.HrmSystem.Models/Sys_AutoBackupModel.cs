using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_AutoBackupModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_AutoBackupName)]
        public string AutoBackupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_ProcedureName)]
        public string ProcedureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_IsActivate)]
        public bool? IsActivate { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_Email)]
        public string Email { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_TimeWaiting)]
        public int? TimeWaiting { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_DateStart)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_DateExpired)]
        public DateTime? DateExpired { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_LastStart)]
        public DateTime? LastStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_Type)]
        public string Type { get; set; }

     public partial class FieldNames
     {
         public const string Code = "Code";
         public const string AutoBackupName = "AutoBackupName";
         public const string ProcedureName = "ProcedureName";
         public const string IsActivate = "IsActivate";
         public const string Email = "Email";
         public const string TimeWaiting = "TimeWaiting";
         public const string DateStart = "DateStart";
         public const string DateExpired = "DateExpired";
         public const string LastStart = "LastStart";
         public const string Description = "Description";
         public const string Type = "Type";
     }

    }

    public class Sys_AutoBackupSearchModel {
        [DisplayName(ConstantDisplay.HRM_System_AutoBackup_AutoBackupName)]
        public string AutoBackupName { get; set; }
        
    }
}
