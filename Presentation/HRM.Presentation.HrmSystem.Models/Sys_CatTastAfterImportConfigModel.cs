using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_CatTastAfterImportConfigModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_AutoUpdateHisAtt)]
        public bool? IsAllowAutoUpdateHistoryAttendanceCode { get; set; }
    }
}
