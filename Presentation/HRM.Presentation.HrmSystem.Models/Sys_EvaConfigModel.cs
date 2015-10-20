using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_TraConfigModel : BaseViewModel
    {
        #region HRM_Sys_Tra (tab Đào Tạo)

        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsCheckDuplicateProfile)]
        public string ExpiredDayWarning{ get; set; }
        public int? ExpiredDayWarningFrom{ get; set; }
        public int? ExpiredDayWarningTo{ get; set; }
        /// <summary>     Học Viên Có Trong Yêu Cầu Đào Tạo </summary>
        public bool? TraineeInRequirementDetail { get; set; }
        
        
        #endregion


    }

}
