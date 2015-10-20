
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_GeneralConfigEntity
    {
        #region Cau hinh Ket thuc năm tài chính
        public Nullable<DateTime> DayEndFinance { get; set; }
        #endregion

        #region Tab Mail
        public String MailServer { get; set; }
        public String MailUserName { get; set; }
        public String MailPassword { get; set; }
        public bool? IMAP4 { get; set; }
        public bool? IMAP2 { get; set; }
        public bool? NoTLS { get; set; }
        public bool? TLS { get; set; }
        public bool? SSL { get; set; }

        #region Cấu hình xuất báo cáo
        public string ExcelPasswordReadonly { get; set; }
        public string ExcelPasswordOpenFile { get; set; }
        #endregion

        public bool? DefaultApproved { get; set; } //HRM_CONFIG_DEFAULTAPPROVED


        #endregion

        #region Config Hợp Đồng Lao Động Đến Hạn
        public bool ShowValue1 { get; set; }
        public string ShowAfterDate1 { get; set; }
        public string ShowBeforDate1 { get; set; }
        #endregion
        #region Config Hết Hạn Thử Việc Trong Tháng
        public bool ShowValue2 { get; set; }
        public string ShowAfterDate2 { get; set; }
        public string ShowBeforDate2 { get; set; }
        #endregion
        #region Config Nhân Viên Mới
        public bool ShowValue3 { get; set; }
        public string ShowAfterDate3 { get; set; }
        public string ShowBeforDate3 { get; set; }
        #endregion

        #region Config Nhân Viên Nghỉ Việc
        public bool ShowValue4 { get; set; }
        public string ShowAfterDate4 { get; set; }
        public string ShowBeforDate4 { get; set; }
        #endregion

        #region Config Sinh Nhật Hôm Nay
        public bool ShowValue5 { get; set; }
        public string ShowAfterDate5 { get; set; }
        public string ShowBeforDate5 { get; set; }
        #endregion

        #region Config Vía Đến Hạn
        public bool ShowValue6 { get; set; }
        public string ShowAfterDate6 { get; set; }
        public string ShowBeforDate6 { get; set; }
        #endregion
        #region Config Giấy Phép LĐ Đến Hạn
        public bool ShowValue7 { get; set; }
        public string ShowAfterDate7 { get; set; }
        public string ShowBeforDate7 { get; set; }
        #endregion

        #region Config Hợp Đồng Đến Hạn Của NV Đảng Đoàn
        public bool ShowValue8 { get; set; }
        public string ShowAfterDate8 { get; set; }
        public string ShowBeforDate8 { get; set; }
        #endregion

        #region Config NV Chưa Lộp Tài Liệu

        public bool ShowValue9 { get; set; }
        public string ShowAfterDate9 { get; set; }
        public string ShowBeforDate9 { get; set; }
        #endregion
        #region Config Yêu Cầu Tuyển Dụng

        public bool ShowValue10 { get; set; }
        public string ShowAfterDate10 { get; set; }
        public string ShowBeforDate10 { get; set; }
        #endregion

        #region Config Nhân Viên Nghỉ Việc Chưa Quyết Toán
        public bool ShowValue12 { get; set; }
        public string ShowAfterDate12 { get; set; }
        public string ShowBeforDate12 { get; set; }
        #endregion

        #region Config Chứng Chỉ Học Viên Sắp Hết Hạn

        public bool ShowValue11 { get; set; }
        public string ShowAfterDate11 { get; set; }
        public string ShowBeforDate11 { get; set; }
        #endregion
        #region Cau hinh hien thi luong co bang cho duyet
        public bool IsShowBasicSalaryWaitingAprove { get; set; }
        #region hien thi tu ngay den ngay
        public int? DayShowBasicSalaryWaitingAproveFrom { get; set; }
        public int? DayShowBasicSalaryWaitingAproveTo { get; set; }

        #endregion
       
        #endregion
        #region Cau hinh hien thi hop dong cho duyet
        public bool IsShowContractWaitingAprove { get; set; }
        public int? DayShowContractWaitingAproveFrom { get; set; }
        public int? DayShowContractWaitingAproveTo { get; set; }
        #endregion
        #region cau hinh hien thi phu luc hop dong den han
        public bool IsShowContractAppendixExpriday { get; set; }
        public int? DayShowContractAppendixExpridayFrom { get; set; }
        public int? DayShowContractAppendixExpridayTo { get; set; }
        #endregion
        #region Cau hinh hien thi nv lam HDT cho duyet
        public bool IsShowHDTJobWaitingApproved { get; set; }
        public int? DayShowHDTJobWaitingApprovedFrom { get; set; }
        public int? DayShowHDTJobWaitingApprovedTo { get; set; }
        #endregion
        #region cau hinh hien thi nv sap den ngay tam hoan
        public bool IsShowDaySuspenseExpiry { get; set; }
        public int? DayShowDaySuspenseExpiryFrom { get; set; }
        public int? DayShowDaySuspenseExpiryTo { get; set; }
        #endregion
        #region cau hinh hien thi nv sap den ngay nghi viec
        public bool IsShowDayStopWorking { get; set; }
        public int? DayShowDayStopWorkingFrom { get; set; }
        public int? DayShowDayStopWorkingTo { get; set; }
        #endregion
        #region cau hinh hien thi nv sap vao lam lai
        public bool IsShowDayComeBackExpiry { get; set; }
        public int? DayShowDayComeBackExpiryFrom { get; set; }
        public int? DayShowDayComeBackExpiryTo { get; set; }
        #endregion
        #region cau hinh hien thi nguoi chờ đánh giá
        public bool IsShowEvalutionWaiting { get; set; }
        public int? DayShowEvalutionWaitingFrom { get; set; }
        public int? DayShowEvalutionWaitingTo { get; set; }
        #endregion

        public string DefaultLanguage { get; set; }

    }
}
