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
    public class FIN_TravelRequestService : BaseService
    {
        public Guid GetProfileIDByTravelRequests(Guid TravelRequestID)
        {
            Guid rs = Guid.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTravelRequest = new CustomBaseRepository<FIN_TravelRequest>(unitOfWork);
                var TravelRequest = repoTravelRequest.GetById(TravelRequestID);
                rs = TravelRequest.ProfileID.Value;
            }
            return rs;
        }

        #region Lấy trạng thái tiếp theo của bản ghi
        public string GetNextStatusOfRecord(Guid recordID)
        {
            using (var context = new VnrHrmDataContext())
            {
                string type = ApproveType.E_FIN_TravelRequest.ToString();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var UserLogin = string.Empty;
                string stt = string.Empty;
                var record = GetData<FIN_TravelRequestEntity>(recordID, ConstantSql.hrm_hr_sp_get_TravelRequestById, UserLogin, ref stt).FirstOrDefault();
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == record.Status).FirstOrDefault();
                double total = record.Total != null ? record.Total.Value : 0.0;
                List<ElementFormula> listFomula = new List<ElementFormula>();
                ElementFormula FomulaItem = new ElementFormula("Total", total, 0);
                ElementFormula statusItem = new ElementFormula(configProcess.NextStatusFormular, configProcess.NextStatusFormular, 1);
                listFomula.Add(FomulaItem);
                listFomula.Add(statusItem);
                var nextStatus = (HRM.Infrastructure.Utilities.Helper.FormulaHelper.FormulaHelperModel)GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular);
                //string nextStatus = GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular).ToString();
                return nextStatus.Value.ToString();
            }

        }
        #endregion

        public string ProcessSendRequest(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoFIN_TravelRequest = new CustomBaseRepository<FIN_TravelRequest>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);

                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();
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
                repoFIN_TravelRequest.Edit(record);
                status = repoFIN_TravelRequest.SaveChanges();

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

        public string ProcessApproved(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            string stt = string.Empty;
            var Services = new BaseService();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoFIN_TravelRequest = new CustomBaseRepository<FIN_TravelRequest>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var UserLogin = string.Empty;
                var record = GetData<FIN_TravelRequestEntity>(recordID, ConstantSql.hrm_hr_sp_get_TravelRequestById, UserLogin, ref stt).FirstOrDefault();
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == record.Status).FirstOrDefault();
                double total = record.Total != null ? record.Total.Value : 0.0;
                List<ElementFormula> listFomula = new List<ElementFormula>();
                ElementFormula FomulaItem = new ElementFormula("Total", total, 0);
                ElementFormula statusItem = new ElementFormula(configProcess.NextStatusFormular, configProcess.NextStatusFormular, 1);
                listFomula.Add(FomulaItem);
                listFomula.Add(statusItem);
                var nextStatus = (HRM.Infrastructure.Utilities.Helper.FormulaHelper.FormulaHelperModel)GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular);
                //string nextStatus = Services.GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular).ToString();


                if (configProcess != null && userApprovedID == Guid.Empty && nextStatus.Value.ToString() != "Approved")
                {
                    return DataErrorCode.Error_NoUserApproved.ToString();
                }
                if (record.Status == EnumDropDown.Status.E_REJECTED.ToString())
                {
                    return DataErrorCode.Error_Reject.ToString();
                }
                var recordEdit = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();
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
                recordEdit.Status = nextStatus.Value.ToString();
                if (userApprovedID == Guid.Empty)
                {
                    recordEdit.UserApproveID = null;
                }
                else
                {
                    recordEdit.UserApproveID = userApprovedID;
                }
                repoFIN_TravelRequest.Edit(recordEdit);
                status = repoFIN_TravelRequest.SaveChanges();

                if (nextStatus.Value.ToString() != "Approved")
                {
                    status = SendMail_ToNextApproved(host, recordID, userApprovedID, nextStatus.Value.ToString(), type);
                }

                if (status == DataErrorCode.Success)
                {
                    //var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == nextStatus).FirstOrDefault();
                    if (nextStatus.Value.ToString() == "Approved")
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

                string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_TRAVELREQUEST.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                    return DataErrorCode.Error_NoTemplateMail;

                var userApproved = repoSys_UserApprove.FindBy(s => s.ID == userApprovedID).FirstOrDefault();
                var lstUserInfo = repoSys_UserInfo.FindBy(s => s.ID != null).ToList();
                var userInfo = lstUserInfo.Where(s => s.ID == userApproved.UserApproveID).FirstOrDefault();
                var Profile = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref status).FirstOrDefault();
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == currentStatus).FirstOrDefault();
             
                var record = GetData<FIN_TravelRequestEntity>(recordID, ConstantSql.hrm_hr_sp_get_TravelRequestById, UserLogin, ref status).FirstOrDefault();
                double total = record.Total != null ? record.Total.Value : 0.0;
                List<ElementFormula> listFomula = new List<ElementFormula>();
                ElementFormula FomulaItem = new ElementFormula("Total", total, 0);
                ElementFormula statusItem = new ElementFormula(configProcess.NextStatusFormular, configProcess.NextStatusFormular, 1);
                listFomula.Add(FomulaItem);
                listFomula.Add(statusItem);
                var nextStatus = (HRM.Infrastructure.Utilities.Helper.FormulaHelper.FormulaHelperModel)GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular);
                //string nextStatus = Services.GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular).ToString();

                var lstUserApprovedNextStatus = repoSys_UserApprove.FindBy(s => s.IsDelete == null && s.Type == type && s.CurrentStatus == nextStatus.Value.ToString()).ToList();
                var RequestUserInfo = lstUserInfo.Where(s => s.ID == record.UserCreateID).FirstOrDefault();

                string MailTo = Profile.Email;
                try
                {
                    string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedTravelRequest/DetailTravelRequest/" + recordID + "#NewTab";

                    #region process Link Content

                    string linkcontent = string.Empty;
                    foreach (var item in lstUserApprovedNextStatus)
                    {
                        var temp = lstUserInfo.Where(s => s.ID == item.UserApproveID).FirstOrDefault();
                        var name = temp.UserInfoName != null ? temp.UserInfoName : temp.UserLogin;

                        linkcontent += "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessApprovedPage"
                                + "?loginID=" + userInfo.ID
                                + "&userApprovedID=" + item.ID
                                + "&recordID=" + recordID
                                + "'>Click this link to approve and forward to '" + name + "'<a/><br /><br />";
                    }
                    // trường hợp duyệt không có cấp cao hơn userApprovedID  Guid.Empty
                    if (lstUserApprovedNextStatus == null)
                    {
                        linkcontent += "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessApprovedPage"
                                + "?loginID=" + userInfo.ID
                                + "&userApprovedID=" + Guid.Empty
                                + "&recordID=" + recordID
                                + "'>Click this link to approve<a/><br /><br />";
                    }
                    linkcontent += "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessRejectPage"
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
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_PROFILENAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_USERINFONAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTNAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTCODE.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_SUBJECT.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_PAIDAMOUNT.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_ACCOUNTCODE.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_BALANCE.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_CASHADVANCE.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_DESCRIPTION.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TOTAL.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_STATUS.ToString(), 
                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_LINKCONTENT.ToString(), 
                    };
                    strsParaValues = new string[] 
                    { 
                        Profile.ProfileName, 
                        RequestUserInfo.UserInfoName, 
                        record.TravelRequestName, 
                        record.TravelRequestCode, 
                        record.Subject, 
                        record.PaidAmount.ToString(), 
                        record.AccountCode, 
                        record.Balance.ToString(), 
                        record.CashAdvance.ToString(), 
                        record.Description, 
                        record.Total.ToString(), 
                        record.Status,  
                        linkcontent                         
                    };
                    string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);

                    #endregion

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

        public string ProcessReject(string host, Guid LoginID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoFIN_TravelRequest = new CustomBaseRepository<FIN_TravelRequest>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);

                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();
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
                repoFIN_TravelRequest.Edit(record);
                status = repoFIN_TravelRequest.SaveChanges();

                return status.ToString();
            }
        }


        public void ActionApproved(List<Guid> lstId, string status, Guid userId) {
            var statusMes = string.Empty;
            var objTravelRequest = new List<object>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTravelRequestItem = new CustomBaseRepository<FIN_TravelRequestItem>(unitOfWork);
                var lstTravelRequestItem = repoTravelRequestItem.GetAll().Where(s => s.IsDelete == null).ToList();

                if(status == "E_APPROVED")
                {
                    var UserLogin = string.Empty;
                    var objApprovedClaim = new List<object>();
                    objApprovedClaim.AddRange(new object[6]);
                    objApprovedClaim[4] = 1;
                    objApprovedClaim[5] = int.MaxValue - 1;
                    var lstApprovedClaim = GetData<FIN_ApproverECLAIMEntity>(objApprovedClaim, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, UserLogin, ref statusMes).ToList();

                    var approvedType1 = FinApproverType.FIN_TravelRequest_Domestic.ToString();
                    var approvedType2 = FinApproverType.FIN_TravelRequest_External.ToString();

                    var approvedEntity = lstApprovedClaim.Where(s => s.ApprovedID != null && s.ApprovedID.Value == userId && (s.ApprovedType == approvedType1 || s.ApprovedType == approvedType2)).FirstOrDefault();

                    if (lstId.Count > 0)
                    {
                        if (approvedEntity != null)
                        {
                            lstTravelRequestItem = lstTravelRequestItem.Where(s => lstId.Contains(s.ID)).ToList();
                            foreach (var item in lstTravelRequestItem)
                            {
                                item.Description2 = status + "_" + approvedEntity.OrderNo.ToString();
                            }
                            repoTravelRequestItem.SaveChanges();
                        }
                   
                    }
                }
                
                if(status == "E_REJECTED")
                {
                    if (lstId.Count > 0)
                    {
                        lstTravelRequestItem = lstTravelRequestItem.Where(s => lstId.Contains(s.ID)).ToList();
                        foreach (var item in lstTravelRequestItem)
                        {
                            item.Description2 = status;
                        }
                        repoTravelRequestItem.SaveChanges();
                    }
                }

                

            }
        }


        public void ActionApprovedAll(List<Guid> lstId, string status, Guid userId)
        {
            var statusMes = string.Empty;
            var objTravelRequest = new List<object>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTravelRequestItem = new CustomBaseRepository<FIN_TravelRequestItem>(unitOfWork);
                var repoTravelRequest = new CustomBaseRepository<FIN_TravelRequest>(unitOfWork);
                var lstTravelRequestId = repoTravelRequest.GetAll().Where(s => s.IsDelete == null && lstId.Contains(s.ID)).Select(s => s.ID).ToList();

                var lstTravelRequestItem = repoTravelRequestItem.GetAll().Where(s => s.IsDelete == null).ToList();
                var UserLogin = string.Empty;
              
                    var objApprovedClaim = new List<object>();
                    objApprovedClaim.AddRange(new object[6]);
                    objApprovedClaim[4] = 1;
                    objApprovedClaim[5] = int.MaxValue - 1;
                    var lstApprovedClaim = GetData<FIN_ApproverECLAIMEntity>(objApprovedClaim, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, UserLogin, ref statusMes).ToList();

                    var approvedType1 = FinApproverType.FIN_TravelRequest_Domestic.ToString();
                    var approvedType2 = FinApproverType.FIN_TravelRequest_External.ToString();

                    var approvedEntity = lstApprovedClaim.Where(s => s.ApprovedID != null && s.ApprovedID.Value == userId && (s.ApprovedType == approvedType1 || s.ApprovedType == approvedType2)).FirstOrDefault();
                    if(status == "E_REQUEST")
                    {
                        if (lstTravelRequestId.Count > 0)
                        {
                           
                                lstTravelRequestItem = lstTravelRequestItem.Where(s => s.TravelRequestID != null && lstTravelRequestId.Contains(s.TravelRequestID.Value)).ToList();
                                foreach (var item in lstTravelRequestItem)
                                {
                                    item.Description2 = status;
                                }
                                repoTravelRequestItem.SaveChanges();
                            

                        }
                    }
                if(status == "E_APPROVED")
                {
                    if (lstTravelRequestId.Count > 0)
                    {
                        if (approvedEntity != null)
                        {
                            lstTravelRequestItem = lstTravelRequestItem.Where(s => s.TravelRequestID != null && lstTravelRequestId.Contains(s.TravelRequestID.Value)).ToList();
                            foreach (var item in lstTravelRequestItem)
                            {
                                item.Description2 = status + "_" + approvedEntity.OrderNo.ToString();
                            }
                            repoTravelRequestItem.SaveChanges();
                        }

                    }
                }
                    

                

            }
        }

        public void ActionSubmit(List<Guid> lstId, string status, Guid userId)
        {
            var statusMes = string.Empty;
            var objTravelRequest = new List<object>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTravelRequestItem = new CustomBaseRepository<FIN_TravelRequestItem>(unitOfWork);
                var UserLogin = string.Empty;
                var lstTravelRequestItem = repoTravelRequestItem.GetAll().Where(s => s.IsDelete == null).ToList();
                var travelID = lstTravelRequestItem.Where(s => s.TravelRequestID != null && lstId.Contains(s.ID)).Select(s => s.TravelRequestID).Distinct().FirstOrDefault();
                var userEntity = GetData<Sys_UserInfoEntity>(userId, ConstantSql.hrm_sys_sp_get_UserbyId, UserLogin, ref statusMes).FirstOrDefault();
                var travelEntity = new FIN_TravelRequestEntity();
                if (travelID != null)
                {
                    travelEntity = GetData<FIN_TravelRequestEntity>(travelID.Value, ConstantSql.hrm_hr_sp_get_TravelRequestById, UserLogin, ref statusMes).FirstOrDefault();
                }

                if (lstId.Count > 0)
                {
                    if (userEntity != null && travelEntity != null)
                    {
                        if (userEntity.ProfileID.Value == travelEntity.ProfileID.Value)
                        {
                            lstTravelRequestItem = lstTravelRequestItem.Where(s => lstId.Contains(s.ID)).ToList();
                            foreach (var item in lstTravelRequestItem)
                            {
                                item.Description2 = status;
                            }
                            repoTravelRequestItem.SaveChanges();
                        }
                
                    }
                    
                }

            }
        }


        public DataErrorCode SendMaToNextApproved(string host, Guid recordID, Guid userApprovedID)
        {
            using (var context = new VnrHrmDataContext())
            {
                string statusMess = string.Empty;
                var UserLogin = string.Empty;
                var Services = new BaseService();
                string status = string.Empty;
                string typeApprover = FinApproverType.FIN_TravelRequest_Domestic.ToString();
               // string typeApprover2 = FinApproverType.FIN_TravelRequest_External.ToString();

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoFin_Approver = new CustomBaseRepository<FIN_ApproverECLAIM>(unitOfWork);
                var lstApprover = repoFin_Approver.GetAll().Where(s => s.IsDelete == null).ToList();

                var repo_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var lstJobTitle = repo_JobTitle.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoOrg = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var lstOrg = repoOrg.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var lstProfile = repoProfile.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);


                var repoFIN_TravelRequest = new CustomBaseRepository<FIN_TravelRequest>(unitOfWork);
                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();

                var repoFin_TravelRequestItem = new CustomBaseRepository<FIN_TravelRequestItem>(unitOfWork);
                var totalItem = repoFin_TravelRequestItem.FindBy(s => s.IsDelete == null && s.Description2.Contains("E_APPROVED") && s.TravelRequestID != null && s.TravelRequestID.Value == recordID).ToList();

                string _typeTemplate = EnumDropDown.EmailType.E_REQUEST_TRAVELREQUEST.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                    return DataErrorCode.Error_NoTemplateMail;

                #region KT nếu là nguoi duyet cuoi thì gui email lại cho người yêu cầu duyệt
                if (userApprovedID == Guid.Empty)
                {
                    var flag = false;
                    var des = string.Empty;
                    foreach (var item in totalItem)
                    {
                        if(item.IsBookingHotel != null && item.IsBookingHotel == true)
                        {
                            flag = true;
                            des = item.Description;
                        }
                    }

                    if (record != null)
                    {
                        string _typeTemplateApproved = EnumDropDown.EmailType.E_APPROVED_TRAVELREQUEST.ToString();
                        var typeTemplateApproved = repoSys_TemplateSendMail.FindBy(s => s.IsDelete == null && s.Type == _typeTemplateApproved).FirstOrDefault();
                        if (typeTemplateApproved == null)
                        {
                            return DataErrorCode.Error_NoTemplateMail;

                        }
                        var lastApprover = lstApprover.Where(s => record.Type != null && s.ProfileID == record.ProfileID && s.ApprovedType == record.Type && s.ApprovedID != null).OrderByDescending(s => s.OrderNo).FirstOrDefault();

                        var userInfo = repoSys_UserInfo.FindBy(s => s.IsDelete == null &&  s.ID == lastApprover.ApprovedID.Value).FirstOrDefault();

                        var lastProfileEntity = lstProfile.Where(s => userInfo != null && userInfo.ProfileID.Value == s.ID).FirstOrDefault();
                        var profileEntity = lstProfile.Where(s => record.ProfileID != null && record.ProfileID.Value == s.ID).FirstOrDefault();
                        if (profileEntity != null)
                        {
                            var jobTitleEntity = lstJobTitle.Where(s => profileEntity.JobTitleID != null && s.ID == profileEntity.JobTitleID.Value).FirstOrDefault();
                            var orgEntity = lstOrg.Where(s => profileEntity.OrgStructureID != null && s.ID == profileEntity.OrgStructureID.Value).FirstOrDefault();
                            if(template != null)
                            {
                                string MailTo = profileEntity.Email;
                                string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedTravelRequest/DetailTravelRequest/" + recordID + "#NewTab";

                                try
                                {
                                    #region magreData

                                    string[] strsParaKey = null;
                                    string[] strsParaValues = null;
                                    strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_AREA.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_DENIES.ToString(),
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_REJECTLINK.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_ISBOOKINGHOTEL.ToString(), 


                                    };
                                    strsParaValues = new string[] 
                                    { 
                                        lastProfileEntity != null ? lastProfileEntity.ProfileName : string.Empty, 
                                      
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                          profileEntity.ProfileName, 
                                        record.TravelRequestName, 
                                        record.TravelRequestCode != null ? record.TravelRequestCode:"" , 
                                        "Đã được duyệt.",
                                        "",
                                        "",
                                        "Ghi chú: chuyến đi có yêu cầu đặt khách sạn " + des + "<br/>" + "Note: hotel booking is requested " + des
                                    };
                                    string body = LibraryService.ReplaceContentFile(typeTemplateApproved.Content, strsParaKey, strsParaValues);

                                    #endregion
                                    string titleMail = typeTemplateApproved.Subject;
                                    
                                    if (flag == true)
                                    {
                                        List<string> CCMail = new List<string>();
                                        CCMail.Add("tin.nguyen@vnresource.net");
                                        bool isSuccess = Services.SendMail(titleMail, MailTo, body, string.Empty, CCMail);
                                        if (isSuccess)
                                            return DataErrorCode.Success;
                                        else
                                            return DataErrorCode.Error;
                                    }
                                    else {
                                        bool isSuccess = Services.SendMail(titleMail, MailTo, body, string.Empty);
                                        if (isSuccess)
                                            return DataErrorCode.Success;
                                        else
                                            return DataErrorCode.Error;
                                    }
                                  
                                }
                                catch (Exception ex)
                                {
                                    
                                    throw;
                                }
                                
                            }

                        }
                    }
                    

                    return DataErrorCode.Success;
                }
                #endregion

                

                var approverEntity = repoFin_Approver.GetAll().Where(s => s.IsDelete == null && s.ID == userApprovedID).FirstOrDefault();

                if (approverEntity != null && approverEntity.OrderNo != null)
                {
                    var profileInfoEntity = lstProfile.Where(s => s.ID == approverEntity.ProfileID.Value).FirstOrDefault();
                    var jobTitleEntity = lstJobTitle.Where(s => profileInfoEntity.JobTitleID != null && s.ID == profileInfoEntity.JobTitleID.Value).FirstOrDefault();
                    var orgEntity = lstOrg.Where(s => profileInfoEntity.OrgStructureID != null && s.ID == profileInfoEntity.OrgStructureID.Value).FirstOrDefault();

                    var lastApprover = lstApprover.Where(s => record.Type != null && s.ProfileID == approverEntity.ProfileID && s.ApprovedType == record.Type && s.ApprovedID != null).OrderByDescending(s => s.OrderNo).FirstOrDefault();


                    var lstApproverByClaim = lstApprover.Where(s => record.Type != null && s.ProfileID == approverEntity.ProfileID && s.ApprovedType == record.Type && s.ApprovedID != null && s.OrderNo > approverEntity.OrderNo).OrderBy(s => s.OrderNo).ToList();
                    var nextApproverEntity = new FIN_ApproverECLAIM();

                    foreach (var item in lstApproverByClaim)
                    {
                        if (approverEntity.OrderNo == item.OrderNo)
                        {
                            continue;
                        }
                        nextApproverEntity = item;
                        break;
                    }

                    #region Nguoi duyet cuoi
                    if (lastApprover != null && lastApprover.OrderNo != null && lastApprover.OrderNo == approverEntity.OrderNo)
                    {
                        var userInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ID == approverEntity.ApprovedID.Value).FirstOrDefault();

                        if (userInfo != null)
                        {
                            var des = string.Empty;
                            foreach (var item in totalItem)
                            {
                                if (item.IsBookingHotel != null && item.IsBookingHotel == true)
                                {
                                   
                                    des = item.Description;
                                }
                            }

                            var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();

                            var profileRequester = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(approverEntity.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();

                            if (template != null)
                            {
                                try
                                {
                                    string MailTo = profileEntity.Email;

                                    string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedTravelRequest/DetailTravelRequest/" + recordID + "#NewTab";

                                    string linkcontent = string.Empty;

                                    string linkcontentReject = string.Empty;

                                    linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessApprovedPage"
                                   + "?loginID=" + approverEntity.ApprovedID
                                   + "&userApprovedID=" + Guid.Empty
                                   + "&recordID=" + recordID
                                   + "'>đây" + "<a/>" + " để đồng ý.";

                                    linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessRejectPage"
                            + "?loginID=" + approverEntity.ApprovedID
                            + "&userApprovedID=" + approverEntity.ID
                            + "&recordID=" + recordID
                            + "'>đây" + "<a/>" + " để từ chối.";

                                    #region magreData

                                    string[] strsParaKey = null;
                                    string[] strsParaValues = null;
                                    strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_AREA.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_DENIES.ToString(),
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_REJECTLINK.ToString(),  
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_ISBOOKINGHOTEL.ToString(), 

                                    };
                                    strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                         jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileInfoEntity != null ? profileInfoEntity.ProfileName : string.Empty, 
                                        record.TravelRequestName, 
                                        record.TravelRequestCode,
                                        "",
                                        linkcontent,
                                        linkcontentReject,
                                        "Ghi chú: chuyến đi có yêu cầu đặt khách sạn " + des + "</br>" + "Note: hotel booking is requested" + des
                                    };
                                    string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                                    #endregion

                                    string titleMail = template.Subject;
                                    bool isSuccess = Services.SendMail(titleMail, MailTo, body, string.Empty);
                                    if (isSuccess)
                                        return DataErrorCode.Success;
                                    else
                                        return DataErrorCode.Error;
                                }
                                catch (Exception ex)
                                {

                                    throw;
                                }
                            }

                        }
                    }
                    #endregion

                  //  var nextApprover = repoFin_Approver.GetAll().Where(s => s.IsDelete == null && s.ProfileID == approverEntity.ProfileID && (s.ApprovedType == typeApprover1 || s.ApprovedType == typeApprover2) && s.OrderNo == approverEntity.OrderNo + 1).FirstOrDefault();
                    var preApprover = repoFin_Approver.GetAll().Where(s => record.Type != null && s.IsDelete == null && s.ProfileID == approverEntity.ProfileID && s.ApprovedType == record.Type && s.OrderNo == approverEntity.OrderNo - 1).FirstOrDefault();

                    if (nextApproverEntity != null)
                    {
                        var userInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ID == approverEntity.ApprovedID.Value).FirstOrDefault();

                        var nextUserInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ID == nextApproverEntity.ApprovedID.Value).FirstOrDefault();

                        if (userInfo != null)
                        {

                            var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();
                            var profileEntityForRecord = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(nextUserInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();
                            if (profileEntity != null)
                            {
                                if (template != null)
                                {
                                    try
                                    {
                                        string MailTo = profileEntity.Email;

                                        string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedTravelRequest/DetailTravelRequest/" + recordID + "#NewTab";

                                        string linkcontent = string.Empty;

                                        string linkcontentReject = string.Empty;


                                        var name = nextUserInfo != null ? nextUserInfo.UserInfoName : nextUserInfo.UserLogin;

                                        linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessApprovedPage"
                                       + "?loginID=" + nextApproverEntity.ApprovedID
                                       + "&userApprovedID=" + nextApproverEntity.ID
                                       + "&recordID=" + recordID
                                       + "'>đây" + "<a/>" + " để đồng ý.";

                                        linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessRejectPage"
                                      + "?loginID=" + approverEntity.ApprovedID
                                      + "&userApprovedID=" + approverEntity.ID
                                      + "&recordID=" + recordID
                                      + "'>đây" + "<a/>" + " để từ chối.";

                                        #region magreData

                                        string[] strsParaKey = null;
                                        string[] strsParaValues = null;
                                        strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_AREA.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_DENIES.ToString(),
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_REJECTLINK.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_ISBOOKINGHOTEL.ToString(), 


                                    };
                                        strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileInfoEntity != null ? profileInfoEntity.ProfileName : string.Empty, 
                                        record.TravelRequestName, 
                                        record.TravelRequestCode, 
                                        "",
                                        linkcontent,
                                        linkcontentReject,
                                        ""
                                    };
                                        string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                                        #endregion

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

                        }
                    }
                    else {
                        var userInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ID == approverEntity.ApprovedID.Value).FirstOrDefault();
                        if (userInfo != null)
                        {
                            var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();
                            if(profileEntity != null)
                            {
                                if(template != null)
                                {
                                try
                                    {
                                        string MailTo = profileEntity.Email;

                                        string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedTravelRequest/DetailTravelRequest/" + recordID + "#NewTab";

                                        string linkcontent = string.Empty;

                                        string linkcontentReject = string.Empty;

                                        linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessApprovedPage"
                                        + "?loginID=" + approverEntity.ApprovedID
                                        + "&userApprovedID=" + Guid.Empty
                                        + "&recordID=" + recordID
                                         + "'>đây" + "<a/>" + " để đồng ý.";
                                     

                                        linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessRejectPage"
                                      + "?loginID=" + approverEntity.ApprovedID
                                      + "&userApprovedID=" + approverEntity.ID
                                      + "&recordID=" + recordID
                                      + "'>đây" + "<a/>" + " để từ chối.";

                                        #region magreData

                                        string[] strsParaKey = null;
                                        string[] strsParaValues = null;
                                        strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_AREA.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_DENIES.ToString(),
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_REJECTLINK.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_ISBOOKINGHOTEL.ToString(), 


                                    };
                                        strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileInfoEntity != null ? profileInfoEntity.ProfileName : string.Empty, 
                                        record.TravelRequestName, 
                                        record.TravelRequestCode, 
                                        "",
                                        linkcontent,
                                        linkcontentReject,
                                        ""
                                    };
                                        string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                                        #endregion

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
                        }
                    }
                    
                }
                return DataErrorCode.Success;
            }
        }

        public string ProcessSendMailForFirstApprove(string host, Guid userId, Guid recordID)
        {
            DataErrorCode status = DataErrorCode.Success;
            using (var context = new VnrHrmDataContext())
            {
                var Services = new BaseService();

                 string statusMess = string.Empty;
                 var UserLogin = string.Empty;
                string type = ApproveType.E_FIN_TravelRequest.ToString();
                //string typeApprover1 = FinApproverType.FIN_TravelRequest_Domestic.ToString();
                //string typeApprover2 = FinApproverType.FIN_TravelRequest_External.ToString();

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repo_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var lstJobTitle = repo_JobTitle.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoOrg = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var lstOrg = repoOrg.GetAll().Where(s => s.IsDelete == null).ToList();
           

                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var lstUserInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null).ToList();
                var userInfoEntity = lstUserInfo.Where(s => userId == s.ID).FirstOrDefault();

                var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var lstProfile = repoProfile.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoFIN_TravelRequest = new CustomBaseRepository<FIN_TravelRequest>(unitOfWork);
                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();


                if (record != null)
                {
                    
                    List<Guid> lstIds = new List<Guid>();

                    lstIds.Add(record.ID);

                    var claimServices = new FIN_ClaimService();
                    ActionApprovedAll(lstIds, "E_REQUEST", userId);
                }


                var repoFin_Approver = new CustomBaseRepository<FIN_ApproverECLAIM>(unitOfWork);
                var lstApprover = repoFin_Approver.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);

                string _typeTemplate = EnumDropDown.EmailType.E_REQUEST_TRAVELREQUEST.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();

                if (userInfoEntity != null)
                {
                    var profileInfoEntity = lstProfile.Where(s => s.ID == userInfoEntity.ProfileID.Value).FirstOrDefault();
                    var jobTitleEntity = lstJobTitle.Where(s => profileInfoEntity.JobTitleID != null && s.ID == profileInfoEntity.JobTitleID.Value).FirstOrDefault();
                    var orgEntity = lstOrg.Where(s => profileInfoEntity.OrgStructureID != null && s.ID == profileInfoEntity.OrgStructureID.Value).FirstOrDefault();

                    var lstApproverByClaim = lstApprover.Where(s => record.Type != null && s.ProfileID == userInfoEntity.ProfileID && s.ApprovedType == record.Type && s.ApprovedID != null).OrderBy(s => s.OrderNo).ToList();
                    var approvedEntity = new FIN_ApproverECLAIM();
                    var firstApprover = lstApproverByClaim.FirstOrDefault();

                    foreach (var item in lstApproverByClaim)
                    {
                        if (firstApprover.OrderNo == item.OrderNo)
                        {
                            continue;
                        }
                        approvedEntity = item;
                        break;
                    }
                    var userIdSecondApprover = lstApprover.Where(s => s.ID == approvedEntity.ID).FirstOrDefault();

                    //var firstApprover = lstApprover.Where(s => s.ProfileID == userInfoEntity.ProfileID && (s.ApprovedType == typeApprover1 || s.ApprovedType == typeApprover2)).OrderBy(s => s.OrderNo).FirstOrDefault();

                    //var userIdSecondApprover = lstApprover.Where(s => s.ProfileID == userInfoEntity.ProfileID && (s.ApprovedType == typeApprover1 || s.ApprovedType == typeApprover2) && s.OrderNo == 2).FirstOrDefault();
                    if (firstApprover != null)
                    {
                        var userEnityByProfileID = lstUserInfo.Where(s => s.ID == firstApprover.ApprovedID.Value).FirstOrDefault();
                        var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userEnityByProfileID.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();
                        if (profileEntity != null)
                        {
                            if (userIdSecondApprover != null)
                            {
                                if (template != null)
                                {
                                    string MailTo = profileEntity.Email;

                                    string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedTravelRequest/DetailTravelRequest/" + recordID + "#NewTab";

                                    string linkcontent = string.Empty;
                                    string linkcontentReject = string.Empty;

                                    var temp = lstUserInfo.Where(s => s.ID == userIdSecondApprover.ApprovedID.Value).FirstOrDefault();
                                    var name = temp.UserInfoName != null ? temp.UserInfoName : temp.UserLogin;

                                    linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessApprovedPage"
                                   + "?loginID=" + firstApprover.ApprovedID
                                   + "&userApprovedID=" + userIdSecondApprover.ID
                                   + "&recordID=" + recordID
                                   + "'>đây" + "</a>" + " để đồng ý.";

                                    linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessRejectPage"
                                     + "?loginID=" + firstApprover.ApprovedID
                                     + "&userApprovedID=" + userIdSecondApprover.ID
                                     + "&recordID=" + recordID
                                     + "'>đây" + "<a/>" + " để từ chối.";


                                    #region magreData

                                    string[] strsParaKey = null;
                                    string[] strsParaValues = null;
                                    strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_AREA.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_DENIES.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_REJECTLINK.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_ISBOOKINGHOTEL.ToString(), 


                                    };
                                    strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        userInfoEntity.UserInfoName, 
                                        record.TravelRequestName, 
                                        record.TravelRequestCode,
                                        "",
                                        linkcontent,
                                        linkcontentReject,
                                        ""
                                    };
                                    string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);

                                    #endregion

                                    string titleMail = template.Subject;
                                    bool isSuccess = Services.SendMail(titleMail, MailTo, body, string.Empty);
                                }
                            }
                            

                        }
                    } 

                }

                return status.ToString();
            }
        }

        public string ProcessApprovedTravelRequest(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            string stt = string.Empty;
            var Services = new BaseService();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

         
                var repoTravelRequest = new CustomBaseRepository<FIN_TravelRequest>(unitOfWork);
                var travelEntity = repoTravelRequest.FindBy(s => s.IsDelete == null && s.ID == recordID).FirstOrDefault();

                List<Guid> lstId = new List<Guid>();
                if (travelEntity != null)
                {
                    lstId.Add(travelEntity.ID);
                }
                var claimServices = new FIN_ClaimService();
                var result = claimServices.ApprovedTravelRequest(recordID, LoginID,type);

                if(result == "Success")
                {
                    ActionApprovedAll(lstId,"E_APPROVED",LoginID);
                }

                if (result == "Success")
                {
                    status = SendMaToNextApproved(host,recordID,userApprovedID);
                }
                
                return status.ToString();
            }
        }


        public DataErrorCode SendMaToPreReject(string host, Guid recordID, Guid userApprovedID ,Guid loginId)
        {
            using (var context = new VnrHrmDataContext())
            {
                var UserLogin = string.Empty;
                string statusMess = string.Empty;
                var Services = new BaseService();
                string status = string.Empty;
                string typeApprover1 = FinApproverType.FIN_TravelRequest_Domestic.ToString();
                string typeApprover2 = FinApproverType.FIN_TravelRequest_External.ToString();

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoFin_Approver = new CustomBaseRepository<FIN_ApproverECLAIM>(unitOfWork);
                var repo_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var lstJobTitle = repo_JobTitle.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoOrg = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var lstOrg = repoOrg.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);


                var repoFIN_TravelRequest = new CustomBaseRepository<FIN_TravelRequest>(unitOfWork);
                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();

                string _typeTemplate = EnumDropDown.EmailType.E_REJECTED_TRAVELREQUEST.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                    return DataErrorCode.Error_NoTemplateMail;

                var approverEntity = repoFin_Approver.GetAll().Where(s => s.IsDelete == null && s.ApprovedID != null && s.ApprovedID.Value == loginId).FirstOrDefault();

                if (approverEntity != null && approverEntity.OrderNo != null)
                {
                
                        var userInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ID == approverEntity.ApprovedID.Value).FirstOrDefault();


                        if (userInfo != null)
                        {
                            var profileRequest = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(record.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();
                            var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();
                            if (profileEntity != null)
                            {
                                var jobTitleEntity = lstJobTitle.Where(s => profileEntity.JobTitleID != null && s.ID == profileEntity.JobTitleID.Value).FirstOrDefault();
                                var orgEntity = lstOrg.Where(s => profileEntity.OrgStructureID != null && s.ID == profileEntity.OrgStructureID.Value).FirstOrDefault();
                                if (template != null)
                                {
                                    try
                                    {
                                        string MailTo = profileRequest.Email;

                                        string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedTravelRequest/DetailTravelRequest/" + recordID + "#NewTab";

                                        string linkcontent = string.Empty;

                                        string linkcontentReject = string.Empty;



                                        linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessApprovedPage"
                                       + "?loginID=" + approverEntity.ApprovedID
                                       + "&userApprovedID=" + approverEntity.ID
                                       + "&recordID=" + recordID
                                       + "'>đây" + "<a/>" + " để đồng ý.";

                                        linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedTravelRequest/ProcessRejectPage"
                                      + "?loginID=" + approverEntity.ApprovedID
                                      + "&userApprovedID=" + approverEntity.ID
                                      + "&recordID=" + recordID
                                      + "'>đây" + "<a/>" + " để từ chối.";

                                        #region magreData

                                        string[] strsParaKey = null;
                                        string[] strsParaValues = null;
                                        strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_AREA.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_DENIES.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_REJECTLINK.ToString(), 
                                   

                                    };
                                        strsParaValues = new string[] 
                                    { 
                                        profileEntity != null ? profileEntity.ProfileName : string.Empty, 

                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileRequest.ProfileName, 

                                        record.TravelRequestName, 
                                        record.TravelRequestCode, 
                                        "Đã Bị Từ Chối Bởi " + profileEntity.ProfileName,
                                        "",
                                        ""
                                    };
                                        string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                                        #endregion

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

                        }

                    
                    //else {
                    //    var userInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ProfileID == record.ProfileID.Value).FirstOrDefault();
                       
                    //    if (userInfo != null)
                    //    {
                    //        var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, ref statusMess).FirstOrDefault();
                    //        if (profileEntity != null)
                    //        {
                    //            var jobTitleEntity = lstJobTitle.Where(s => profileEntity.JobTitleID != null && s.ID == profileEntity.JobTitleID.Value).FirstOrDefault();
                    //            var orgEntity = lstOrg.Where(s => profileEntity.OrgStructureID != null && s.ID == profileEntity.OrgStructureID.Value).FirstOrDefault();
                    //        if(template != null)
                    //        {
                    //            #region magreData

                    //            string[] strsParaKey = null;
                    //            string[] strsParaValues = null;
                    //            strsParaKey = new string[] 
                    //                { 
                    //                    EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_PROFILENAME.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_JOBTITLE.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_AREA.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_USERINFONAME.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTNAME.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_TRAVELREQUESTCODE.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_DENIES.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_LINKCONTENT.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_TRAVELREQUEST.E_REJECTLINK.ToString(), 

                    //                };
                    //            strsParaValues = new string[] 
                    //                { 
                    //                    profileEntity.ProfileName, 
                    //                    jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                    //                    orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                    //                    profileEntity != null ? profileEntity.ProfileName : string.Empty, 
                    //                    record.TravelRequestName, 
                    //                    record.TravelRequestCode, 
                    //                    "Đã Bị Từ Chối"  ,
                    //                    "",
                    //                    ""
                    //                };
                    //            string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                    //            #endregion
                    //        }
                    //        }
                    //    }
                        
                    //}

                }
                return DataErrorCode.Error;
            }
        }

        public string ProcessRejectTravelRequest(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            string stt = string.Empty;
            var Services = new BaseService();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var claimServices = new FIN_ClaimService();
                var result = claimServices.ApprovedTravelRequest(recordID, LoginID, type);

                if (result == "Success")
                {
                    status = SendMaToPreReject(host, recordID, userApprovedID,LoginID);
                }

                return status.ToString();
            }
        }




    }
}
