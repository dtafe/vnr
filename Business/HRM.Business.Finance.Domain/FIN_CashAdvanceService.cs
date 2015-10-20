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
    public class FIN_CashAdvanceService : BaseService
    {

        public void ActionApproved(List<Guid> lstId, string status, Guid userId)
        {
            var statusMes = string.Empty;
            var objTravelRequest = new List<object>();
            var UserLogin = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTravelRequestItem = new CustomBaseRepository<Fin_CashAdvanceItem>(unitOfWork);
                var lstTravelRequestItem = repoTravelRequestItem.GetAll().Where(s => s.IsDelete == null).ToList();

                if(status == "E_APPROVED")
                {
                    var objApprovedClaim = new List<object>();
                    objApprovedClaim.AddRange(new object[6]);
                    objApprovedClaim[4] = 1;
                    objApprovedClaim[5] = int.MaxValue - 1;
                    var lstApprovedClaim = GetData<FIN_ApproverECLAIMEntity>(objApprovedClaim, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, UserLogin, ref statusMes).ToList();

                    var approvedType = FinApproverType.Fin_CashAdvance.ToString();

                    var approvedEntity = lstApprovedClaim.Where(s => s.ApprovedID != null && s.ApprovedID.Value == userId && s.ApprovedType == approvedType).FirstOrDefault();

                    if (lstId.Count > 0)
                    {
                        if (approvedEntity != null)
                        {
                            lstTravelRequestItem = lstTravelRequestItem.Where(s => lstId.Contains(s.ID)).ToList();
                            foreach (var item in lstTravelRequestItem)
                            {
                                item.Status = status + "_" + approvedEntity.OrderNo.ToString();
                            }
                            repoTravelRequestItem.SaveChanges();
                        }
                       
                    }
                }

                if (status == "E_REJECTED")
                {
                    if (lstId.Count > 0)
                    {
                        lstTravelRequestItem = lstTravelRequestItem.Where(s => lstId.Contains(s.ID)).ToList();
                        foreach (var item in lstTravelRequestItem)
                        {
                            item.Status = status;
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
            var UserLogin = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCashAdvanceItem = new CustomBaseRepository<Fin_CashAdvanceItem>(unitOfWork);
                var repoCashAdvance = new CustomBaseRepository<Fin_CashAdvance>(unitOfWork);

                var lstCashAdvanceId = repoCashAdvance.GetAll().Where(s => s.IsDelete == null && lstId.Contains(s.ID)).Select(s => s.ID).ToList();

                var lstCashAdvanceItem = repoCashAdvanceItem.GetAll().Where(s => s.IsDelete == null).ToList();

               
                    var objApprovedClaim = new List<object>();
                    objApprovedClaim.AddRange(new object[6]);
                    objApprovedClaim[4] = 1;
                    objApprovedClaim[5] = int.MaxValue - 1;
                    var lstApprovedClaim = GetData<FIN_ApproverECLAIMEntity>(objApprovedClaim, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, UserLogin, ref statusMes).ToList();

                    var approvedType = FinApproverType.Fin_CashAdvance.ToString();

                    var approvedEntity = lstApprovedClaim.Where(s => s.ApprovedID != null && s.ApprovedID.Value == userId && s.ApprovedType == approvedType).FirstOrDefault();

                if(status == "E_REQUEST")
                {
                if (lstCashAdvanceId.Count > 0)
                    {
                       
                            lstCashAdvanceItem = lstCashAdvanceItem.Where(s => s.CashAdvanceID != null && lstCashAdvanceId.Contains(s.CashAdvanceID.Value)).ToList();
                            foreach (var item in lstCashAdvanceItem)
                            {
                                item.Status = status ;
                            }
                            repoCashAdvanceItem.SaveChanges();
                        

                    }
                }
                if(status == "E_APPROVED")
                {
                    if (lstCashAdvanceId.Count > 0)
                    {
                        if (approvedEntity != null)
                        {
                            lstCashAdvanceItem = lstCashAdvanceItem.Where(s => s.CashAdvanceID != null && lstCashAdvanceId.Contains(s.CashAdvanceID.Value)).ToList();
                            foreach (var item in lstCashAdvanceItem)
                            {
                                item.Status = status + "_" + approvedEntity.OrderNo.ToString();
                            }
                            repoCashAdvanceItem.SaveChanges();
                        }

                    }
                }
                    
                
            }
        }

        public void ActionSubmit(List<Guid> lstId, string status, Guid userId)
        {
            var statusMes = string.Empty;
            var objTravelRequest = new List<object>();
            var UserLogin = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTravelRequestItem = new CustomBaseRepository<Fin_CashAdvanceItem>(unitOfWork);
                var lstTravelRequestItem = repoTravelRequestItem.GetAll().Where(s => s.IsDelete == null).ToList();

                var cashAdvanceID = lstTravelRequestItem.Where(s => s.CashAdvanceID != null && lstId.Contains(s.ID)).Select(s => s.CashAdvanceID).Distinct().FirstOrDefault();
                var userEntity = GetData<Sys_UserInfoEntity>(userId, ConstantSql.hrm_sys_sp_get_UserbyId, UserLogin, ref statusMes).FirstOrDefault();

                var cashAdvanceEntity = new Fin_CashAdvanceEntity();
                if (cashAdvanceID != null)
                {
                    cashAdvanceEntity = GetData<Fin_CashAdvanceEntity>(cashAdvanceID.Value, ConstantSql.hrm_hr_sp_get_CashAdvanceById, UserLogin, ref statusMes).FirstOrDefault();
                }

                if (lstId.Count > 0)
                {

                    if (userEntity != null && cashAdvanceEntity != null)
                    {
                        if (userEntity.ProfileID.Value == cashAdvanceEntity.ProfileID.Value)
                        {
                            lstTravelRequestItem = lstTravelRequestItem.Where(s => lstId.Contains(s.ID)).ToList();
                            foreach (var item in lstTravelRequestItem)
                            {
                                item.Status = status;
                            }
                            repoTravelRequestItem.SaveChanges();
                        }
                    }
                    
                }

            }
        }



        #region Duyệt CashAdvance
        public string ApprovedCashAdvance(Guid cashAdvanceId, Guid userId, string type)
        {
            using (var context = new VnrHrmDataContext())
            {
                var message = string.Empty;
                var status = string.Empty;
                var UserLogin = string.Empty;
                var services = new BaseService();
                var objApprovedClaim = new List<object>();
                objApprovedClaim.AddRange(new object[6]);
                objApprovedClaim[4] = 1;
                objApprovedClaim[5] = int.MaxValue - 1;
                var lstApprovedClaim = GetData<FIN_ApproverECLAIMEntity>(objApprovedClaim, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, UserLogin, ref status).ToList();

                var approvedType = FinApproverType.Fin_CashAdvance.ToString();

                var approvedEntity = lstApprovedClaim.Where(s => s.ApprovedID != null && s.ApprovedID.Value == userId && s.ApprovedType == approvedType).FirstOrDefault();

                var cashAdvanceEntity = GetData<Fin_CashAdvanceEntity>(cashAdvanceId, ConstantSql.hrm_hr_sp_get_CashAdvanceById, UserLogin, ref status).FirstOrDefault();

                var objHistory = new List<object>();
                objHistory.AddRange(new object[10]);
                objHistory[8] = 1;
                objHistory[9] = int.MaxValue - 1;
                var lstHistory = GetData<FIN_HistoryApproveECLAIMEntity>(objHistory, ConstantSql.hrm_hr_sp_get_HistoryApprovedClaim, UserLogin, ref status).ToList();

                if (approvedEntity != null)
                {
                    if (cashAdvanceEntity.TravelRequestID != null)
                    {
                        var travelEntity = GetData<FIN_TravelRequestEntity>(cashAdvanceEntity.TravelRequestID.Value, ConstantSql.hrm_hr_sp_get_TravelRequestById, UserLogin, ref status).FirstOrDefault();
                        var lstHistoryTravel = lstHistory.Where(s => s.TravelRequestID != null && s.TravelRequestID.Value == cashAdvanceEntity.TravelRequestID.Value && s.Status == "E_APPROVED" && s.UserApproveClaimID != null && s.UserApproveClaimID.Value == userId).ToList();
                        if (travelEntity != null)
                        {
                            if (lstHistoryTravel.Count == 0)
                            {
                                var entityAdd = new FIN_HistoryApproveECLAIMEntity();
                                entityAdd.TravelRequestID = cashAdvanceEntity.TravelRequestID;
                                entityAdd.UserApproveClaimID = (Guid?)approvedEntity.ID;
                                entityAdd.DateApproved = DateTime.Now;
                                entityAdd.Status = type;
                                message = Add(entityAdd);
                            }
                        }
                    }

                    var entity = new FIN_HistoryApproveECLAIMEntity();
                    entity.CashAdvanceID = (Guid?)cashAdvanceId;
                    if (cashAdvanceEntity != null)
                    {
                        entity.TravelRequestID = cashAdvanceEntity.TravelRequestID;
                    }
                    entity.UserApproveClaimID = (Guid?)approvedEntity.ID;
                    entity.DateApproved = DateTime.Now;
                    entity.Status = type;
                    message = Add(entity);
                }
                else
                {
                    message = "Bạn Không Có Quyền Duyệt!";
                }


                return message;
            }
        }

        #endregion


        public DataErrorCode SendMaToNextApproved(string host, Guid recordID, Guid userApprovedID)
        {
            using (var context = new VnrHrmDataContext())
            {
                string statusMess = string.Empty;
                var Services = new BaseService();
                string status = string.Empty;
                var UserLogin = string.Empty;
                string typeApprover = FinApproverType.Fin_CashAdvance.ToString();

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoFin_Approver = new CustomBaseRepository<FIN_ApproverECLAIM>(unitOfWork);
                var lstApprover = repoFin_Approver.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);

                var repo_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var lstJobTitle = repo_JobTitle.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoOrg = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var lstOrg = repoOrg.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var lstProfile = repoProfile.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoCashItem = new CustomBaseRepository<Fin_CashAdvanceItem>(unitOfWork);
                var totalItem = repoCashItem.GetAll().Where(s => s.CashAdvanceID == recordID && s.Status.Contains("E_APPROVED")).Sum(s => s.Amount);

                var repoFIN_TravelRequest = new CustomBaseRepository<Fin_CashAdvance>(unitOfWork);
                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();

                string _typeTemplate = EnumDropDown.EmailType.E_REQUEST_CASHADVANCE.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                    return DataErrorCode.Error_NoTemplateMail;

                #region KT nếu là nguoi duyet cuoi thì gui email lại cho người yêu cầu duyệt
                if (userApprovedID == Guid.Empty)
                {
                   
                    if (record != null)
                    {
                        string _typeTemplateApproved = EnumDropDown.EmailType.E_APPROVED_CASHADVANCE.ToString();
                        var typeTemplateApproved = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplateApproved).FirstOrDefault();
                        if (typeTemplateApproved == null)
                        {
                            return DataErrorCode.Error_NoTemplateMail;

                        }
                        var lastApprover = lstApprover.Where(s => s.ProfileID == record.ProfileID && s.ApprovedID != null).OrderByDescending(s => s.OrderNo).FirstOrDefault();
                        var userInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ID == lastApprover.ApprovedID.Value).FirstOrDefault();
                        var lastProfileEntity = lstProfile.Where(s => userInfo != null && userInfo.ProfileID.Value == s.ID).FirstOrDefault();
                        var profileEntity = lstProfile.Where(s => record.ProfileID != null && record.ProfileID.Value == s.ID).FirstOrDefault();
                        if (profileEntity != null)
                        {
                            var jobTitleEntity = lstJobTitle.Where(s => profileEntity.JobTitleID != null && s.ID == profileEntity.JobTitleID.Value).FirstOrDefault();
                            var orgEntity = lstOrg.Where(s => profileEntity.OrgStructureID != null && s.ID == profileEntity.OrgStructureID.Value).FirstOrDefault();
                            if (template != null)
                            {
                                string MailTo = profileEntity.Email;
                                string urlClickHere = host + "#Hrm_Main_Web/Fin_SendMailApprovedCashAdvance/DetailTravelRequest/" + recordID + "#NewTab";

                                try
                                {
                                    #region magreData

                                    string[] strsParaKey = null;
                                    string[] strsParaValues = null;
                                    strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_AREA.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCECODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_TOTALADVANCE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_DENIES.ToString(),
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_REJECTLINK.ToString(), 

                                    };
                                    strsParaValues = new string[] 
                                    { 
                                        lastProfileEntity != null? lastProfileEntity.ProfileName : string.Empty, 
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileEntity.ProfileName, 
                                        record.CashAdvanceName, 
                                        record.Code != null ? record.Code:"" , 
                                        totalItem.ToString(),
                                        "Đã được duyệt.",
                                        "",
                                        ""
                                    };
                                    string body = LibraryService.ReplaceContentFile(typeTemplateApproved.Content, strsParaKey, strsParaValues);

                                    #endregion
                                    string titleMail = typeTemplateApproved.Subject;
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


                    return DataErrorCode.Success;
                }
                #endregion

                var approverEntity = repoFin_Approver.GetAll().Where(s => s.IsDelete == null && s.ID == userApprovedID).FirstOrDefault();

                if (approverEntity != null && approverEntity.OrderNo != null)
                {
                    var profileInfoEntity = lstProfile.Where(s => s.ID == approverEntity.ProfileID.Value).FirstOrDefault();
                    var jobTitleEntity = lstJobTitle.Where(s => profileInfoEntity.JobTitleID != null && s.ID == profileInfoEntity.JobTitleID.Value).FirstOrDefault();
                    var orgEntity = lstOrg.Where(s => profileInfoEntity.OrgStructureID != null && s.ID == profileInfoEntity.OrgStructureID.Value).FirstOrDefault();
                    var lastApprover = lstApprover.Where(s => s.ProfileID == approverEntity.ProfileID && s.ApprovedType == typeApprover && s.ApprovedID != null).OrderByDescending(s => s.OrderNo).FirstOrDefault();

                    var lstApproverByClaim = lstApprover.Where(s => s.ProfileID == approverEntity.ProfileID && s.ApprovedType == typeApprover && s.ApprovedID != null && s.OrderNo > approverEntity.OrderNo).OrderBy(s => s.OrderNo).ToList();
                    var nextApprover = new FIN_ApproverECLAIM();

                    foreach (var item in lstApproverByClaim)
                    {
                        if (approverEntity.OrderNo == item.OrderNo)
                        {
                            continue;
                        }
                        nextApprover = item;
                        break;
                    }

                    #region Nguoi duyet cuoi
                    if (lastApprover != null && lastApprover.OrderNo != null && lastApprover.OrderNo == approverEntity.OrderNo)
                    {
                        var userInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ID == approverEntity.ApprovedID.Value).FirstOrDefault();

                        if (userInfo != null)
                        {
                            var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();

                            var profileRequester = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(approverEntity.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();

                            if (template != null)
                            {
                                try
                                {
                                    string MailTo = profileEntity.Email;

                                    string urlClickHere = host + "#Hrm_Main_Web/Fin_SendMailApprovedCashAdvance/DetailTravelRequest/" + recordID + "#NewTab";

                                    string linkcontent = string.Empty;

                                    string linkcontentReject = string.Empty;

                                    linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_SendMailApprovedCashAdvance/ProcessApprovedPage"
                                   + "?loginID=" + approverEntity.ApprovedID
                                   + "&userApprovedID=" + Guid.Empty
                                   + "&recordID=" + recordID
                                   + "'>đây" + "<a/>" + " để đồng ý.";

                                    linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_SendMailApprovedCashAdvance/ProcessRejectPage"
                            + "?loginID=" + approverEntity.ApprovedID
                            + "&userApprovedID=" + approverEntity.ID
                            + "&recordID=" + recordID
                            + "'>đây" + "<a/>" + " để từ chối.";

                                    #region magreData

                                    string[] strsParaKey = null;
                                    string[] strsParaValues = null;
                                    strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_AREA.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCECODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_TOTALADVANCE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_DENIES.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_REJECTLINK.ToString(), 
                                    };
                                    strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                         jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileInfoEntity != null ? profileInfoEntity.ProfileName : string.Empty, 
                                        record.CashAdvanceName, 
                                        record.Code,
                                        totalItem.ToString(),
                                        "",
                                        linkcontent,
                                        linkcontentReject
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

                  //  var nextApprover = repoFin_Approver.GetAll().Where(s => s.IsDelete == null && s.ProfileID == approverEntity.ProfileID && s.ApprovedType == typeApprover && s.OrderNo == approverEntity.OrderNo + 1).FirstOrDefault();

                    if (nextApprover != null)
                    {
                        var userInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ID == approverEntity.ApprovedID.Value).FirstOrDefault();

                        var nextUserInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ID == nextApprover.ApprovedID.Value).FirstOrDefault();

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

                                        string urlClickHere = host + "#Hrm_Main_Web/Fin_SendMailApprovedCashAdvance/DetailTravelRequest/" + recordID + "#NewTab";

                                        string linkcontent = string.Empty;
                                        string linkcontentReject = string.Empty;


                                        var name = nextUserInfo != null ? nextUserInfo.UserInfoName : nextUserInfo.UserLogin;

                                        linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_SendMailApprovedCashAdvance/ProcessApprovedPage"
                                       + "?loginID=" + nextApprover.ApprovedID
                                       + "&userApprovedID=" + nextApprover.ID
                                       + "&recordID=" + recordID
                                       + "'>đây" + "<a/>" + " để đồng ý.";

                                        linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_SendMailApprovedCashAdvance/ProcessRejectPage"
                                  + "?loginID=" + approverEntity.ApprovedID
                                  + "&userApprovedID=" + approverEntity.ID
                                  + "&recordID=" + recordID
                                  + "'>đây" + "<a/>" + " để từ chối.";

                                        #region magreData

                                        string[] strsParaKey = null;
                                        string[] strsParaValues = null;
                                        strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_PROFILENAME.ToString(),
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_AREA.ToString(), 
 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCECODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_TOTALADVANCE.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_DENIES.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_REJECTLINK.ToString(), 

                                    };
                                        strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileInfoEntity != null ? profileInfoEntity.ProfileName : string.Empty, 
                                        record.CashAdvanceName, 
                                        record.Code,
                                        totalItem.ToString(),
                                        "",
                                        linkcontent ,
                                        linkcontentReject
                        
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
                        if(userInfo != null)
                        {
                            var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();
                            if (profileEntity != null)
                            {
                                if(template != null)
                                {
                                    try
                                    {
                                        string MailTo = profileEntity.Email;

                                        string urlClickHere = host + "#Hrm_Main_Web/Fin_SendMailApprovedCashAdvance/DetailTravelRequest/" + recordID + "#NewTab";

                                        string linkcontent = string.Empty;
                                        string linkcontentReject = string.Empty;

                                        linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_SendMailApprovedCashAdvance/ProcessApprovedPage"
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
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_PROFILENAME.ToString(),
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_AREA.ToString(), 
 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCECODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_TOTALADVANCE.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_DENIES.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_REJECTLINK.ToString(), 

                                    };
                                        strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileInfoEntity != null ? profileInfoEntity.ProfileName : string.Empty, 
                                        record.CashAdvanceName, 
                                        record.Code,
                                        totalItem.ToString(),
                                        "",
                                        linkcontent ,
                                        linkcontentReject
                        
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
                return DataErrorCode.Error;
            }
        }

        public string ProcessSendMailForFirstApprove(string host, Guid userId, Guid recordID)
        {
            DataErrorCode status = DataErrorCode.Success;
            using (var context = new VnrHrmDataContext())
            {
                var Services = new BaseService();
                var UserLogin = string.Empty;
                string statusMess = string.Empty;
                string type = ApproveType.E_FIN_CashAdvance.ToString();
                string typeApprover = FinApproverType.Fin_CashAdvance.ToString();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);

                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var lstUserInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null).ToList();
                var userInfoEntity = lstUserInfo.Where(s => userId == s.ID).FirstOrDefault();
                var repo_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var lstJobTitle = repo_JobTitle.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoOrg = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var lstOrg = repoOrg.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var lstProfile = repoProfile.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoFIN_TravelRequest = new CustomBaseRepository<Fin_CashAdvance>(unitOfWork);
                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();

                if (record != null)
                {
                    List<Guid> lstIds = new List<Guid>();
                    lstIds.Add(record.ID);
               
                    ActionApprovedAll(lstIds, "E_REQUEST", userId);
                }

                var repoCashItem = new CustomBaseRepository<Fin_CashAdvanceItem>(unitOfWork);
                var totalItem = repoCashItem.GetAll().Where(s => s.IsDelete == null && s.CashAdvanceID != null && s.CashAdvanceID.Value == recordID).Sum(s => s.Amount);

                var repoFin_Approver = new CustomBaseRepository<FIN_ApproverECLAIM>(unitOfWork);
                var lstApprover = repoFin_Approver.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);

                string _typeTemplate = EnumDropDown.EmailType.E_REQUEST_CASHADVANCE.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();

                if (userInfoEntity != null)
                {
                    var profileInfoEntity = lstProfile.Where(s => s.ID == userInfoEntity.ProfileID.Value).FirstOrDefault();
                    var jobTitleEntity = lstJobTitle.Where(s => profileInfoEntity.JobTitleID != null && s.ID == profileInfoEntity.JobTitleID.Value).FirstOrDefault();
                    var orgEntity = lstOrg.Where(s => profileInfoEntity.OrgStructureID != null && s.ID == profileInfoEntity.OrgStructureID.Value).FirstOrDefault();
                    var lstApproverByClaim = lstApprover.Where(s => s.ProfileID == userInfoEntity.ProfileID && s.ApprovedType == typeApprover && s.ApprovedID != null).OrderBy(s => s.OrderNo).ToList();
                    var approvedEntity = new FIN_ApproverECLAIM();

                   // var firstApprover = lstApprover.Where(s => s.ProfileID == userInfoEntity.ProfileID && s.ApprovedType == typeApprover).OrderBy(s => s.OrderNo).FirstOrDefault();


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

                  //  var userIdSecondApprover = lstApprover.Where(s => s.ProfileID == userInfoEntity.ProfileID && s.ApprovedType == typeApprover && s.OrderNo == 2).FirstOrDefault();
                    var userIdSecondApprover = lstApprover.Where(s => s.ID == approvedEntity.ID).FirstOrDefault();

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

                                    string urlClickHere = host + "#Hrm_Main_Web/Fin_SendMailApprovedCashAdvance/DetailTravelRequest/" + recordID + "#NewTab";

                                    string linkcontent = string.Empty;
                                    string linkcontentReject = string.Empty;

                                    var temp = lstUserInfo.Where(s => s.ID == userIdSecondApprover.ApprovedID.Value).FirstOrDefault();
                                    var name = temp.UserInfoName != null ? temp.UserInfoName : temp.UserLogin;

                                    linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_SendMailApprovedCashAdvance/ProcessApprovedPage"
                                   + "?loginID=" + firstApprover.ApprovedID
                                   + "&userApprovedID=" + userIdSecondApprover.ID
                                   + "&recordID=" + recordID
                                   + "'>đây" + "<a/>" + " để đồng ý.";


                                    linkcontentReject += "Nhấn vào " +  "<a href='" + host + "Fin_SendMailApprovedCashAdvance/ProcessRejectPage"
                                     + "?loginID=" + firstApprover.ApprovedID
                                     + "&userApprovedID=" + userIdSecondApprover.ID
                                     + "&recordID=" + recordID
                                     + "'>đây" + "<a/>" + " để từ chối.";

                                    #region magreData

                                    string[] strsParaKey = null;
                                    string[] strsParaValues = null;
                                    strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_PROFILENAME.ToString(),
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_AREA.ToString(), 
 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCECODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_TOTALADVANCE.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_DENIES.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_REJECTLINK.ToString(), 


                                    };
                                    strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        userInfoEntity.UserInfoName, 
                                        record.CashAdvanceName, 
                                        record.Code, 
                                        totalItem.ToString(),
                                        "",
                                        linkcontent,
                                        linkcontentReject
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

        public string ProcessApprovedCashAdvance(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            string stt = string.Empty;
            var Services = new BaseService();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoCashAdavance = new CustomBaseRepository<Fin_CashAdvance>(unitOfWork);
                var cashAdvanceEntity = repoCashAdavance.FindBy(s => s.IsDelete == null && s.ID == recordID).FirstOrDefault();

                List<Guid> lstId = new List<Guid>();
                if (cashAdvanceEntity != null)
                {
                    lstId.Add(cashAdvanceEntity.ID);
                }

                var result = ApprovedCashAdvance(recordID, LoginID,type);

                if (result == "Success")
                {
                    ActionApprovedAll(lstId, "E_APPROVED", LoginID);
                }

                if (result == "Success")
                {
                    status = SendMaToNextApproved(host, recordID, userApprovedID);
                }

                return status.ToString();
            }
        }


        public DataErrorCode SendMaToPreReject(string host, Guid recordID, Guid userApprovedID, Guid loginId)
        {
            using (var context = new VnrHrmDataContext())
            {
                string statusMess = string.Empty;
                var Services = new BaseService();
                var UserLogin = string.Empty;
                string status = string.Empty;
                string typeApprover = FinApproverType.Fin_CashAdvance.ToString();

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoFin_Approver = new CustomBaseRepository<FIN_ApproverECLAIM>(unitOfWork);

                var repo_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var lstJobTitle = repo_JobTitle.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoOrg = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var lstOrg = repoOrg.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoCashItem = new CustomBaseRepository<Fin_CashAdvanceItem>(unitOfWork);
                var totalItem = repoCashItem.GetAll().Where(s => s.CashAdvanceID == recordID && s.Status.Contains("E_APPROVED")).Sum(s => s.Amount);

                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);


                var repoFIN_TravelRequest = new CustomBaseRepository<Fin_CashAdvance>(unitOfWork);
                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();

                string _typeTemplate = EnumDropDown.EmailType.E_REJECTED_CASHADVANCE.ToString();
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

                                        string urlClickHere = host + "#Hrm_Main_Web/Fin_SendMailApprovedCashAdvance/DetailTravelRequest/" + recordID + "#NewTab";

                                        string linkcontent = string.Empty;

                                        string linkcontentReject = string.Empty;


                                       

                                        linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_SendMailApprovedCashAdvance/ProcessApprovedPage"
                                       + "?loginID=" + approverEntity.ApprovedID
                                       + "&userApprovedID=" + approverEntity.ID
                                       + "&recordID=" + recordID
                                       + "'>đây" + "<a/>" + " để đồng ý.";

                                        linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_SendMailApprovedCashAdvance/ProcessRejectPage"
                                      + "?loginID=" + approverEntity.ApprovedID
                                      + "&userApprovedID=" + approverEntity.ID
                                      + "&recordID=" + recordID
                                      + "'>đây" + "<a/>" + " để từ chối.";

                                        #region magreData

                                        string[] strsParaKey = null;
                                        string[] strsParaValues = null;
                                        strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_AREA.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCECODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_TOTALADVANCE.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_DENIES.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_REJECTLINK.ToString(), 
                                        
                                   

                                    };
                                        strsParaValues = new string[] 
                                    { 
                                         profileEntity != null ? profileEntity.ProfileName : string.Empty, 
                                       
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileRequest.ProfileName, 
                                        record.CashAdvanceName, 
                                        record.Code, 
                                        totalItem.ToString() != null ?  totalItem.ToString(): string.Empty,
                                        "Đã Bị Từ Chối Bởi " + profileEntity.ProfileName ,
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

                    
                    //else
                    //{
                    //    var userInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null && s.ProfileID == record.ProfileID.Value).FirstOrDefault();

                    //    if (userInfo != null)
                    //    {
                    //        var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, ref statusMess).FirstOrDefault();
                    //        if (profileEntity != null)
                    //        {
                    //            var jobTitleEntity = lstJobTitle.Where(s => profileEntity.JobTitleID != null && s.ID == profileEntity.JobTitleID.Value).FirstOrDefault();
                    //            var orgEntity = lstOrg.Where(s => profileEntity.OrgStructureID != null && s.ID == profileEntity.OrgStructureID.Value).FirstOrDefault();
                    //            if (template != null)
                    //            {
                    //                #region magreData

                    //                string[] strsParaKey = null;
                    //                string[] strsParaValues = null;
                    //                strsParaKey = new string[] 
                    //                { 
                    //                    EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_PROFILENAME.ToString(),
                    //                    EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_JOBTITLE.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_AREA.ToString(),
                    //                    EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_USERINFONAME.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCENAME.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_CASHADVANCECODE.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CASHADVANCE.E_DENIES.ToString(), 
                                   

                    //                };
                    //                strsParaValues = new string[] 
                    //                { 
                    //                    profileEntity.ProfileName, 
                    //                     jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                    //                    orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                    //                    profileEntity != null ? profileEntity.ProfileName : string.Empty, 
                    //                    record.CashAdvanceName, 
                    //                    record.Code, 
                    //                    "Đã Bị Từ Chối"
                    //                };
                    //                string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                    //                #endregion
                    //            }
                    //        }

                    //    }

                    //}

                }
                return DataErrorCode.Error;
            }
        }

        public string ProcessRejectCashAdvance(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            string stt = string.Empty;
            var Services = new BaseService();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var claimServices = new FIN_ClaimService();
                var result = ApprovedCashAdvance(recordID, LoginID, type);

                if (result == "Success")
                {
                    status = SendMaToPreReject(host, recordID, userApprovedID, LoginID);
                }

                return status.ToString();
            }
        }


    }
}
