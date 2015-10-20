using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_EvaConfigModel : BaseViewModel
    {
        #region Cấu Hình Lưu Đánh Giá
        //cot C1
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC1)]
        public string DataC1 { get; set; }
        //cot C12
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC2)]
        public string DataC2 { get; set; }
        //cot C3
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC3)]
        public string DataC3 { get; set; }
        //cot C4
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC4)]
        public string DataC4 { get; set; }
        //cot C5
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC5)]
        public string DataC5 { get; set; }
        //cot C6
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC6)]
        public string DataC6 { get; set; }
        //cot C7
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC7)]
        public string DataC7 { get; set; }
        //cot C8
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC8)]
        public string DataC8 { get; set; }
        //cot C9
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC9)]
        public string DataC9 { get; set; }
        //cot C10
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC10)]
        public string DataC10 { get; set; }
        //cot C11
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC11)]
        public string DataC11 { get; set; }
        //cot C12
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC12)]
        public string DataC12 { get; set; }
        //cot C13
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC13)]
        public string DataC13 { get; set; }
        //cot C14
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC14)]
        public string DataC14 { get; set; }
        //cot C15
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC15)]
        public string DataC15 { get; set; }
        //cot C16
        [DisplayName(ConstantDisplay.HRM_Sys_Eva_DataC16)]
        public string DataC16 { get; set; }

        #endregion
        #region HRM_Sys_Eva (tab Đánh Giá)

        [DisplayName(ConstantDisplay.HRM_Hr_Hre_IsCheckDuplicateProfile)]
        public DateTime? DateStartMark { get; set; }
        public DateTime? DateEndMark { get; set; }
        public DateTime? DateStartTime1 { get; set; }
        public DateTime? DateEndTime1 { get; set; }
        public DateTime? DateStartTime2 { get; set; }
        public DateTime? DateEndTime2 { get; set; }

        #endregion
        #region Cau hinh gui mail nhac nho

        public bool? IsEmployeeEvaluation { get; set; }
        //public string EmployeeEvaluationContent { get; set; }
        public bool? IsUserApproveEvaluation1 { get; set; }
        //public string UserApproveEvaluationContent1 { get; set; }
        public bool? IsUserApproveEvaluation2 { get; set; }
        //public string UserApproveEvaluationContent2 { get; set; }
        public bool? IsCycle { get; set; }
        public int? CycleDay1 { get; set; }
        public int? CycleDay2 { get; set; }
        public int? CycleDay3 { get; set; }
        public bool? IsTime { get; set; }
        public int? TimeDateFrom { get; set; }
        public int? TimeDateTo { get; set; }
        #endregion


    }

}
