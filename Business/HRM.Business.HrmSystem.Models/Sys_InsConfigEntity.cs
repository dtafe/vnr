
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_TraConfigEntity
    {
        #region HRM_Sys_Tra (tab Đào Tạo)

        public string ExpiredDayWarning { get; set; }
        public int? ExpiredDayWarningFrom { get; set; }
        public int? ExpiredDayWarningTo { get; set; }
        public bool? TraineeInRequirementDetail { get; set; }

        #endregion
    }
}
