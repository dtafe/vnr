
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_EvaConfigEntity
    {
        public string Messages { get; set; }
        #region Cấu Hình Lưu Đánh Giá
        //cot C1
        public string DataC1 { get; set; }
        //cot C12
        public string DataC2 { get; set; }
        //cot C3
        public string DataC3 { get; set; }
        //cot C4
        public string DataC4 { get; set; }
        //cot C5
        public string DataC5 { get; set; }
        //cot C6
        public string DataC6 { get; set; }
        //cot C7
        public string DataC7 { get; set; }
        //cot C8
        public string DataC8 { get; set; }
        //cot C9
        public string DataC9 { get; set; }
        //cot C10
        public string DataC10 { get; set; }
        //cot C11
        public string DataC11 { get; set; }
        //cot C12
        public string DataC12 { get; set; }
        //cot C13
        public string DataC13 { get; set; }
        //cot C14
        public string DataC14 { get; set; }
        //cot C15
        public string DataC15 { get; set; }
        //cot C16
        public string DataC16 { get; set; }

        #endregion

        #region Đánh Giá

        public DateTime? DateStartMark { get; set; }
        public DateTime? DateEndMark { get; set; }

        public DateTime? DateStartTime1 { get; set; }
        public DateTime? DateEndTime1 { get; set; }
        public DateTime? DateStartTime2 { get; set; }
        public DateTime? DateEndTime2 { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }

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
    
        #endregion
    }
}
