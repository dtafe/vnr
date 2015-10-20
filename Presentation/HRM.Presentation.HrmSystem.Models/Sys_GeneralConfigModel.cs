using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_GeneralConfigModel : BaseViewModel
    {
        #region  (tab Mail)
        [Description(ConstantDisplay.HRM_General_Mail_Desc_MailServer)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_MailServer)]
        public String MailServer { get; set; }
        [Description(ConstantDisplay.HRM_General_Mail_Desc_MailPort)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_MailPort)]
        public int Port { get; set; }
        [Description(ConstantDisplay.HRM_General_Mail_Desc_MailUserName)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_MailUserName)]
        public String MailUserName { get; set; }
        [Description(ConstantDisplay.HRM_General_Mail_Desc_EmailTest)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_EmailTest)]
        public String EmailTest { get; set; }
        [Description(ConstantDisplay.HRM_General_Mail_Desc_MailPassword)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_MailPassword)]
        public String MailPassword { get; set; }
        [Description(ConstantDisplay.HRM_General_Mail_Desc_IMAPOption)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_IMAP4)]
        public bool? IMAP4 { get; set; }
        [Description(ConstantDisplay.HRM_General_Mail_Desc_IMAPOption)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_IMAP2)]
        public bool? IMAP2 { get; set; }
        [Description(ConstantDisplay.HRM_General_Mail_Desc_TLSOption)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_NoTLS)]
        public bool? NoTLS { get; set; }
        [Description(ConstantDisplay.HRM_General_Mail_Desc_TLSOption)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_TLS)]
        public bool? TLS { get; set; }
        [Description(ConstantDisplay.HRM_General_Mail_Desc_SSL)]
        [DisplayName(ConstantDisplay.HRM_General_Mail_SSL)]
        public bool? SSL { get; set; }

        #region Cấu hình xuất báo cáo
        [DisplayName(ConstantDisplay.HRM_General_Mail_ExcelPasswordReadonly)]
        public string ExcelPasswordReadonly { get; set; }

        [DisplayName(ConstantDisplay.HRM_General_Mail_ExcelPasswordOpenFile)]
        public string ExcelPasswordOpenFile { get; set; }

        [DisplayName(ConstantDisplay.HRM_General_Mail_DefaultApproved)]
        public bool? DefaultApproved { get; set; } //HRM_CONFIG_DEFAULTAPPROVED




        #endregion
        #region Cau hinh Ket thuc năm tài chính
        [DisplayName(ConstantDisplay.HRM_System_ConfigGeneral_DayEndFinance)]
        public Nullable<DateTime> DayEndFinance { get; set; }
        #endregion

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

        #region Config Chứng Chỉ Học Viên Sắp Hết Hạn

        public bool ShowValue11 { get; set; }
        public string ShowAfterDate11 { get; set; }
        public string ShowBeforDate11 { get; set; }
        #endregion

        #region Config Nhân Viên Nghỉ Việc Chưa Quyết Toán
        public bool ShowValue12 { get; set; }
        public string ShowAfterDate12 { get; set; }
        public string ShowBeforDate12 { get; set; }
        #endregion

        #region tab Trang Chu
        #region kiem tra hien thi canh bao luong co ban cho duyet
        [DisplayName(ConstantDisplay.HRM_Payroll_BasicSalaryPending)]
        public bool IsShowBasicSalaryWaitingAprove { get; set; }
        #region hien thi tu ngay den ngay
        public int? DayShowBasicSalaryWaitingAproveFrom { get; set; }
        public int? DayShowBasicSalaryWaitingAproveTo { get; set; }

        #endregion
        #endregion
        #region Cau hinh hien thi hop dong cho duyet
        [DisplayName(ConstantDisplay.HRM_Dashboard_sumContractWaiting)]
        public bool IsShowContractWaitingAprove { get; set; }
        public int? DayShowContractWaitingAproveFrom { get; set; }
        public int? DayShowContractWaitingAproveTo { get; set; }

        #endregion
        #region Cau hinh hien thi phu luc hd den han
        [DisplayName(ConstantDisplay.HRM_Dashboard_sumAppendixContract)]
        public bool IsShowContractAppendixExpriday { get; set; }
        public int? DayShowContractAppendixExpridayFrom { get; set; }
        public int? DayShowContractAppendixExpridayTo { get; set; }
        #endregion
        #region cau hinh hien thi nv lam hdt cho duyet
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
        #endregion

        [DisplayName(ConstantDisplay.HRM_General_DefaultLanguage)]
        public string DefaultLanguage { get; set; }

    }

}
