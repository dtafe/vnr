using System.Data.SqlClient;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using VnResource.Helper.Linq;
using VnResource.Helper.Data;
using VnResource.Helper.Utility;
using System.Reflection;
using System.Collections;
using HRM.Data.Entity;
using System.Data.Entity.Validation;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Hr.Models;
using System.Net.Mail;
using System.Net;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Finance.Models;

namespace HRM.Business.Finance.Domain
{
    public class FIN_PurchaseRequestService : BaseService
    {
        #region Lấy trạng thái tiếp theo của bản ghi
        public string GetNextStatusOfRecord(Guid recordID)
        {
            using (var context = new VnrHrmDataContext())
            {
                string type = ApproveType.E_FIN_PurchaseRequest.ToString();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var UserLogin = string.Empty;
                string stt = string.Empty;
                var record = GetData<FIN_PurchaseRequestEntity>(recordID, ConstantSql.hrm_hr_sp_get_PurchaseRequestById, UserLogin, ref stt).FirstOrDefault();
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == record.Status).FirstOrDefault();
                double total = record.Total != null ? record.Total.Value : 0.0;
                List<ElementFormula> listFomula = new List<ElementFormula>();
                ElementFormula FomulaItem = new ElementFormula("Total", total, 0);
                ElementFormula statusItem = new ElementFormula(configProcess.NextStatusFormular, configProcess.NextStatusFormular, 1);
                listFomula.Add(FomulaItem);
                listFomula.Add(statusItem);
                string nextStatus = GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular).ToString();
                return nextStatus;
            }

        }
        #endregion

        #region xử lý Yêu Cầu Phê Duyệt động đa cấp và Gửi Tới Cấp Tiếp Theo

        /// <summary>
        /// [Hieu.Van]
        /// Xử lý Gửi yêu cầu phê duyệt
        /// </summary>
        /// <param name="LoginID"></param>
        /// <param name="userApprovedID"></param>
        /// <param name="recordID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string ProcessSendRequest(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoFIN_PurchaseRequest = new CustomBaseRepository<FIN_PurchaseRequest>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);

                var record = repoFIN_PurchaseRequest.FindBy(s => s.ID == recordID).FirstOrDefault();
                if (record.UserCreateID != LoginID)
                {
                    return DataErrorCode.NoPermission.ToString();
                }
                if (record.Status == EnumDropDown.Status.E_REJECTED.ToString())
                {
                    return DataErrorCode.Error_Reject.ToString();
                }
                if (record.Status != null)
                {
                    return DataErrorCode.Error_SendRequest.ToString();
                }
                var userApproved = repoSys_UserApprove.FindBy(s => s.ID == userApprovedID).FirstOrDefault();
                // Lấy status hiện tại của bản ghi
                string currentStatus = record.Status;

                // truyền từ Dropdownlist chọn người duyệt, lấy ra giá trị Status tiếp theo
                string nextStatus = userApproved.CurrentStatus;

                record.Status = nextStatus;
                record.UserApproveID = userApprovedID;
                repoFIN_PurchaseRequest.Edit(record);
                status = repoFIN_PurchaseRequest.SaveChanges();

                
                if (status == DataErrorCode.Success)
                {
                    if (nextStatus != "Approved")
                    {
                        status = SendMail_ToNextApproved(host, recordID, userApprovedID, nextStatus, type);
                    }

                    var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                    var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == record.Status).FirstOrDefault();
                    if (configProcess == null)
                    {
                        return DataErrorCode.Success.ToString();
                    }
                    else
                    {
                        return DataErrorCode.Success_Forward.ToString();
                    }
                }

                return status.ToString();
            }
        }

        #endregion

        #region xử lý Duyệt động đa cấp và Gửi Tới Cấp Tiếp Theo

        /// <summary>
        /// [Hieu.Van]
        /// Xử lý duyệt động đa cấp
        /// ProcessApproved (Tài khoản Login - LoginID, Value Dropdownlist - userApprovedID, Id Bản ghi - recordID, Enum màn hình - type)
        /// </summary>
        /// <param name="LoginID"></param>
        /// <param name="userApprovedID"></param>
        /// <param name="recordID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string ProcessApproved(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            string stt = string.Empty;
            var Services = new BaseService();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoFIN_PurchaseRequest = new CustomBaseRepository<FIN_PurchaseRequest>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var UserLogin = string.Empty;
                var record = GetData<FIN_PurchaseRequestEntity>(recordID, ConstantSql.hrm_hr_sp_get_PurchaseRequestById, UserLogin, ref stt).FirstOrDefault();
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == record.Status).FirstOrDefault();
                double total = record.Total != null ? record.Total.Value : 0.0;
                List<ElementFormula> listFomula = new List<ElementFormula>();
                ElementFormula FomulaItem = new ElementFormula("Total", total, 0);
                ElementFormula statusItem = new ElementFormula(configProcess.NextStatusFormular, configProcess.NextStatusFormular, 1);
                listFomula.Add(FomulaItem);
                listFomula.Add(statusItem);
                string nextStatus = Services.GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular).ToString();


                if (configProcess != null && userApprovedID == Guid.Empty && nextStatus != "Approved")
                {
                    return DataErrorCode.Error_NoUserApproved.ToString();
                }
                if (record.Status == EnumDropDown.Status.E_REJECTED.ToString())
                {
                    return DataErrorCode.Error_Reject.ToString();
                }
                var recordEdit = repoFIN_PurchaseRequest.FindBy(s => s.ID == recordID).FirstOrDefault();
                // lấy UserInfoID hiện tại của bản ghi
                var userApprovedCurrent = repoSys_UserApprove.FindBy(s => s.ID == recordEdit.UserApproveID).FirstOrDefault();
                // Kiểm tra tài khoản login có được phép Duyệt hay ko?
                if (LoginID == Guid.Empty || userApprovedCurrent.UserApproveID != LoginID)
                {
                    // kiểm tra nếu bản ghi có tk duyệt trống (tức bản ghi mới tạo) và tk login = tk tạo thì cho duyệt tiếp tục
                    if (recordEdit.UserApproveID == null && record.UserCreateID == LoginID)
                    {
                    }
                    else
                    {
                        return DataErrorCode.NoPermission.ToString();
                    }
                }
                recordEdit.Status = nextStatus;
                if (userApprovedID == Guid.Empty)
                {
                    recordEdit.UserApproveID = null;
                }
                else
                {
                    recordEdit.UserApproveID = userApprovedID;
                }
                repoFIN_PurchaseRequest.Edit(recordEdit);
                status = repoFIN_PurchaseRequest.SaveChanges();

                if (nextStatus != "Approved")
                {
                    status = SendMail_ToNextApproved(host, recordID, userApprovedID, nextStatus, type);
                }

                if (status == DataErrorCode.Success)
                {
                    //var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == nextStatus).FirstOrDefault();
                    if (nextStatus == "Approved")
                    {
                        return DataErrorCode.Success.ToString();
                    }
                    else
                    {
                        return DataErrorCode.Success_Forward.ToString();
                    }
                }

                return status.ToString();
            }
        }

        #endregion

        #region xử lý Từ Chối Duyệt động đa cấp

        /// <summary>
        /// [Hieu.Van]
        /// Xử lý Từ Chối
        /// </summary>
        /// <param name="LoginID"></param>
        /// <param name="userApprovedID"></param>
        /// <param name="recordID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string ProcessReject(string host, Guid LoginID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoFIN_PurchaseRequest = new CustomBaseRepository<FIN_PurchaseRequest>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);

                var record = repoFIN_PurchaseRequest.FindBy(s => s.ID == recordID).FirstOrDefault();
                if (record.Status == EnumDropDown.Status.E_REJECTED.ToString())
                {
                    return DataErrorCode.Error_Reject.ToString();
                }
                var userApproved = repoSys_UserApprove.FindBy(s => s.ID == record.UserApproveID).FirstOrDefault();
                // Kiểm tra tài khoản login có được phép Duyệt hay ko?
                if (LoginID == Guid.Empty || userApproved.UserApproveID != LoginID)
                {
                    // kiểm tra nếu bản ghi có tk duyệt trống (tức bản ghi mới tạo) và tk login = tk tạo thì cho duyệt tiếp tục
                    if (record.UserApproveID == null && record.UserCreateID == LoginID)
                    {
                    }
                    else
                    {
                        return DataErrorCode.NoPermission.ToString();
                    }
                }

                record.Status = EnumDropDown.Status.E_REJECTED.ToString();
                repoFIN_PurchaseRequest.Edit(record);
                status = repoFIN_PurchaseRequest.SaveChanges();

                return status.ToString();
            }
        }

        #endregion

        #region Gửi mail tới người Duyệt tiếp theo được chỉ định

        /// <summary>
        /// Hieu.Van
        /// Gửi mail duyệt
        /// recordID : ID bản ghi
        /// userApprovedID : Id ngưởi nhận dc mail
        /// currentStatus : trạng thái hiện tại của bản ghi
        /// type : ApproveType.E_FIN_PurchaseRequest
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="userApprovedID"></param>
        /// <param name="currentStatus"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataErrorCode SendMail_ToNextApproved(string host, Guid recordID, Guid userApprovedID, string currentStatus, string type)
        {
            using (var context = new VnrHrmDataContext())
            {
                var Services = new BaseService();
                var UserLogin = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);

                string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_PURCHASEREQUEST.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                    return DataErrorCode.Error_NoTemplateMail;

                var userApproved = repoSys_UserApprove.FindBy(s => s.ID == userApprovedID).FirstOrDefault();
                var lstUserInfo = repoSys_UserInfo.FindBy(s => s.ID != null).ToList();
                var userInfo = lstUserInfo.Where(s => s.ID == userApproved.UserApproveID).FirstOrDefault();
                var Profile = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref status).FirstOrDefault();
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == currentStatus).FirstOrDefault();
         
                var record = GetData<FIN_PurchaseRequestEntity>(recordID, ConstantSql.hrm_hr_sp_get_PurchaseRequestById, UserLogin, ref status).FirstOrDefault();
                double total = record.Total != null ? record.Total.Value : 0.0;
                List<ElementFormula> listFomula = new List<ElementFormula>();
                ElementFormula FomulaItem = new ElementFormula("Total", total, 0);
                ElementFormula statusItem = new ElementFormula(configProcess.NextStatusFormular, configProcess.NextStatusFormular, 1);
                listFomula.Add(FomulaItem);
                listFomula.Add(statusItem);
                string nextStatus = Services.GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular).ToString();

                var lstUserApprovedNextStatus = repoSys_UserApprove.FindBy(s => s.IsDelete == null && s.Type == type && s.CurrentStatus == nextStatus).ToList();
                var RequestUserInfo = lstUserInfo.Where(s => s.ID == record.UserCreateID).FirstOrDefault();

                string MailTo = Profile.Email;
                try 
	            {
                    string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedPurchaseRequest/DetailPurchaseRequest/" + recordID + "#NewTab";
                    string dateFrom = record.From != null ? record.From.Value.ToString(ConstantFormat.HRM_Format_DayMonthYear.ToString()) : "...";
                    string dateTo = record.To != null ? record.To.Value.ToString(ConstantFormat.HRM_Format_DayMonthYear.ToString()) : "...";

                    #region process Link Content

                    string linkcontent = string.Empty;
                    foreach (var item in lstUserApprovedNextStatus)
                    {
                        var temp = lstUserInfo.Where(s => s.ID == item.UserApproveID).FirstOrDefault();
                        var name = temp.UserInfoName != null ? temp.UserInfoName : temp.UserLogin;

                        linkcontent += "<a href='" + host + "Fin_ApprovedPurchaseRequest/ProcessApprovedPage"
                                + "?loginID=" + userInfo.ID
                                + "&userApprovedID=" + item.ID
                                + "&recordID=" + recordID
                                + "'>Click this link to approve and forward to '" + name + "'<a/><br /><br />";
                    }
                    // trường hợp duyệt không có cấp cao hơn userApprovedID  Guid.Empty
                    if (lstUserApprovedNextStatus == null)
                    {
                        linkcontent += "<a href='" + host + "Fin_ApprovedPurchaseRequest/ProcessApprovedPage"
                                + "?loginID=" + userInfo.ID
                                + "&userApprovedID=" + Guid.Empty
                                + "&recordID=" + recordID
                                + "'>Click this link to approve<a/><br /><br />";
                    }
                    linkcontent += "<a href='" + host + "Fin_ApprovedPurchaseRequest/ProcessRejectPage"
                                + "?loginID=" + userInfo.ID
                                + "&recordID=" + recordID
                                + "'>Click this link to reject<a/><br /><br />";

                    linkcontent += "<a href='" + urlClickHere + "'>Click this link to view detail information and approve or reject<a/> <br /> ";
                    
                    #endregion
                    #region magreData

                    string[] strsParaKey = null;
                    string[] strsParaValues = null;
                    strsParaKey = new string[] 
                    { 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_PROFILENAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_USERINFONAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_FUNCTIONNAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_BUDGETOWNERNAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_SUPPLIERNAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_BUDGETCHARGEDIN.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_FROMTO.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_DESCRIPTION.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_TOTAL.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_STATUS.ToString(), 
                        EnumDropDown.EmailType_APPROVED_PURCHASEREQUEST.E_LINKCONTENT.ToString()                      
                    };
                    strsParaValues = new string[] 
                    { 
                        Profile.ProfileName, 
                        RequestUserInfo.UserInfoName, 
                        record.FunctionName, 
                        record.BudgetOwnerName, 
                        record.SupplierName, 
                        record.BudgetChargedIn.ToString(), 
                        dateFrom + " - " + dateTo, 
                        record.Description, 
                        record.Total.ToString(), 
                        record.Status, 
                        linkcontent 
                        
                    };
                    string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);

                    #endregion

                    //string body = "<html><head><title></title></head><body class='scayt-enabled'>Dear " + Profile.ProfileName + ",<br /><br />"
                    //            + "<b>Purchase Request is waiting for your approval:</b><br />"
                    //            + "=========================================<br />"
                    //            + "Request for : " + RequestUserInfo.UserInfoName + " <br />"
                    //            + "Function : " + record.FunctionName + " <br />"
                    //            + "Budget Owner : " + record.BudgetOwnerName + " <br />"
                    //            + "Vender/Supplier : " + record.SupplierName + " <br />"
                    //            + "Budget charged in : " + record.BudgetChargedIn + " <br />"
                    //            + "From/To : " + dateFrom + " - " + dateTo + " <br />"
                    //            + "Description : " + record.Description + " <br />"
                    //            + "Total : " + record.Total + " <br />"
                    //            + "Status : " + record.Status + " <br />"
                    //            + "=========================================<br />"
                    //            ;
                    //+ "=========================================<br />"
                    //+ "This message is auto-sent by VnResource HRM System. Any feedback for this software, <br />"
                    //+ "Please send to <a href='mailto:contact@VnResource.vn' target='_blank'>contact@VnResource.vn</a>, <a href='http://www.vnresource.vn/' target='_blank'>www.VnResource.vn</a> <br />"
                    //+ "</body> </html>";

                    string titleMail = template.Subject;
                    bool isSuccess = Services.SendMail(titleMail, MailTo, body, string.Empty);
                    
                    if (isSuccess)
                        return DataErrorCode.Success;
                    else
                        return DataErrorCode.Error;

	            }
	            catch (Exception ex)
	            {
                    return DataErrorCode.Error_NoConfigMail;
	            }
            }
        }

        #endregion


    }
}
