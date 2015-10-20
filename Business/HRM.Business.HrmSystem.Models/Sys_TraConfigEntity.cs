
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_InsConfigEntity
    {
        #region HRM_Sys_Ins (tab Bảo Hiểm)
        /// <summary> Tổng hợp dữ liệu BH tạm đến ngày [int] </summary>
        public int? SumDataTempt { get; set; }
        /// <summary> Tổng hợp dữ liệu BH chính thức đến ngày [int] </summary>
        public int? SumDataMain { get; set; }

        /// <summary> Ngày Chốt BH (ngày 18 đối với honda) [int] </summary>
        public int? PeriodInsuranceDay { get; set; }

        /// <summary> Ngày Bắt Đầu Chu Kỳ Báo Giảm LĐ (ngày 15 tháng N-1 đối với honda) [int] </summary>
        public int? PeriodInsuranceDayDecreaseFrom { get; set; }
        /// <summary> Ngày Chốt Chu Kỳ Báo Giảm LĐ (ngày 14 tháng N đối với honda) [int] </summary>
        public int? PeriodInsuranceDayDecreaseTo { get; set; }
        /// <summary> Ngày Bắt Đầu Chu Kỳ BH  (ngày 15 tháng [N-1] đối với honda) [int] </summary>
        public int? PeriodInsuranceDayDefaultFrom { get; set; }
        /// <summary> Ngày Chốt Chu Kỳ BH (ngày 14 tháng N đối với honda) [int] </summary>
        public int? PeriodInsuranceDayDefaultTo { get; set; }

        /// <summary> Ngày bắt đầu số ngày nghỉ >=14 ngày  (ngày 1 tháng N đối với honda) [int] </summary>
        public int? PeriodInsurance14DayFrom { get; set; }
        /// <summary> Ngày kết thúc số ngày nghỉ >=14 ngày  (ngày 31 tháng N đối với honda) [int] </summary>
        public int? PeriodInsurance14DayTo { get; set; }

        /// <summary> Ngày bắt đầu Thay đổi lương  (ngày 15 tháng [N-1] đối với honda) [int] </summary>
        public int? PeriodInsuranceDayChangeSalFrom { get; set; }

        /// <summary> Ngày kết thúc Thay đổi lương  (ngày 14 tháng [N] đối với honda) [int] </summary>
        public int? PeriodInsuranceDayChangeSalTo { get; set; }


        #endregion
    }
}
