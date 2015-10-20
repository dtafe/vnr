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
    public class FIN_ClaimService : BaseService
    {
        #region Quy trình Duyệt Mới

        public List<FIN_ClaimEntity> GetClaimApprovedList(Guid UserLogin)
        {
            List<FIN_ClaimEntity> lstReturn = new List<FIN_ClaimEntity>();

            string appType = ApproveType.E_FIN_ClaimRequest.ToString();
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoFIN_Claim = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                List<Sys_ConditionApproved> lstCondition = new List<Sys_ConditionApproved>();
                List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                GetProfileWithPermissionLogin(UserLogin, appType, out lstCondition, out lstProfile);

                var lstExpensesCondition = lstCondition.Select(s => s.ExpensesType).Distinct().ToList();
                var dataClaim = repoFIN_Claim
                    .FindBy(s => s.IsDelete == null && lstExpensesCondition.Contains(s.ExpensesType))
                    .ToList().Translate<FIN_ClaimEntity>();

                foreach (var condition in lstCondition)
                {
                    List<Guid> lstGuidOrg = GetOrgInCondition(condition);
                    var profileInOrg = lstProfile.Where(s => lstGuidOrg.Contains(s.OrgStructureID.Value)).Select(a => a.ID).ToList();
                    
                    var resultCondition = dataClaim.Where(s => profileInOrg.Contains(s.ProfileID.Value)).ToList();
                    lstReturn.AddRange(resultCondition);
                }
            }
            return lstReturn;
        }

        /// <summary>
        /// Join danh sách phòng ban trong condition thành 1 List<Guid>
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<Guid> GetOrgInCondition(Sys_ConditionApproved condition)
        {
            List<Guid> lstOrg = new List<Guid>();
            if (condition.OrgID1 != null)
            {
                lstOrg.Add(condition.OrgID1.Value);
            }
            if (condition.OrgID2 != null)
            {
                lstOrg.Add(condition.OrgID2.Value);
            }
            if (condition.OrgID3 != null)
            {
                lstOrg.Add(condition.OrgID3.Value);
            }
            if (condition.OrgID4 != null)
            {
                lstOrg.Add(condition.OrgID4.Value);
            }
            if (condition.OrgID5 != null)
            {
                lstOrg.Add(condition.OrgID5.Value);
            }
            if (condition.OrgID6 != null)
            {
                lstOrg.Add(condition.OrgID6.Value);
            }
            return lstOrg;
        }

        /// <summary>
        /// Lấy những nhân viên mà tài khoản Login có quyền
        /// </summary>
        /// <param name="UserLogin"></param>
        /// <returns></returns>
        public static void GetProfileWithPermissionLogin(Guid UserLogin, String appType, out List<Sys_ConditionApproved> lstCondition, out List<Hre_ProfileEntity> lstProfile)
        {
            lstCondition = new List<Sys_ConditionApproved>();
            lstProfile = new List<Hre_ProfileEntity>();
            BaseService service = new BaseService();
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoSys_ConditionApproved = new CustomBaseRepository<Sys_ConditionApproved>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);

                var userInfo = repoSys_UserInfo.FindBy(s => s.ID == UserLogin).FirstOrDefault();
                var profileInfo = repoHre_Profile.FindBy(s => s.ID == userInfo.ProfileID).FirstOrDefault();

                lstCondition = repoSys_ConditionApproved.FindBy(s => s.IsDelete == null
                    && s.ApprovedType == appType
                    && (s.JobTitleID == null || s.JobTitleID == profileInfo.JobTitleID)
                    && (s.PositionID == null || s.PositionID == profileInfo.PositionID)
                    && (s.WorkPlaceID == null || s.WorkPlaceID == profileInfo.WorkPlaceID)
                    && ((s.OrgID6 != null || s.OrgID6 == profileInfo.OrgStructureID)
                        || (s.OrgID5 != null || s.OrgID5 == profileInfo.OrgStructureID)
                        || (s.OrgID4 != null || s.OrgID4 == profileInfo.OrgStructureID)
                        || (s.OrgID3 != null || s.OrgID3 == profileInfo.OrgStructureID)
                        || (s.OrgID2 != null || s.OrgID2 == profileInfo.OrgStructureID)
                        || (s.OrgID1 != null || s.OrgID1 == profileInfo.OrgStructureID)
                    )).ToList();

                if (lstCondition != null)
                {
                    List<Guid> lstorgID = new List<Guid>();
                    lstorgID.AddRange(lstCondition.Where(s => s.OrgID1 != null).Select(s => s.OrgID1).ToList());
                    lstorgID.AddRange(lstCondition.Where(s => s.OrgID2 != null).Select(s => s.OrgID2).ToList());
                    lstorgID.AddRange(lstCondition.Where(s => s.OrgID3 != null).Select(s => s.OrgID3).ToList());
                    lstorgID.AddRange(lstCondition.Where(s => s.OrgID4 != null).Select(s => s.OrgID4).ToList());
                    lstorgID.AddRange(lstCondition.Where(s => s.OrgID5 != null).Select(s => s.OrgID5).ToList());
                    lstorgID.AddRange(lstCondition.Where(s => s.OrgID6 != null).Select(s => s.OrgID6).ToList());

                    lstorgID = lstorgID.Distinct().ToList();
                    var lstOrgForProfile = repoCat_OrgStructure.FindBy(s => lstorgID.Contains(s.ID)).Select(s => s.OrderNumber).ToList();
                    var userLogin = string.Empty;
                    List<object> lstParamPro = new List<object>();
                    lstParamPro.AddRange(new object[3]);
                    lstParamPro[0] = String.Join(",", lstOrgForProfile);
                    lstProfile = service.GetData<Hre_ProfileEntity>(lstParamPro, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).ToList();
                }
            }
        }


        #endregion


        #region Lấy trạng thái tiếp theo của bản ghi
        public string GetNextStatusOfRecord(Guid recordID)
        {
            using (var context = new VnrHrmDataContext())
            {
                string type = ApproveType.E_FIN_ClaimRequest.ToString();
                var UserLogin = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);

                string stt = string.Empty;
                var record = GetData<FIN_ClaimEntity>(recordID, ConstantSql.hrm_hr_sp_get_ClaimById, UserLogin, ref stt).FirstOrDefault();
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
                var repoFIN_Claim = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);

                var record = repoFIN_Claim.FindBy(s => s.ID == recordID).FirstOrDefault();
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
                repoFIN_Claim.Edit(record);
                status = repoFIN_Claim.SaveChanges();

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
                var repoFIN_Claim = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var UserLogin = string.Empty;
                var record = GetData<FIN_ClaimEntity>(recordID, ConstantSql.hrm_hr_sp_get_ClaimById, UserLogin, ref stt).FirstOrDefault();
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
                var recordEdit = repoFIN_Claim.FindBy(s => s.ID == recordID).FirstOrDefault();
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
                repoFIN_Claim.Edit(recordEdit);
                status = repoFIN_Claim.SaveChanges();

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
                var repoFIN_Claim = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);

                var record = repoFIN_Claim.FindBy(s => s.ID == recordID).FirstOrDefault();
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
                repoFIN_Claim.Edit(record);
                status = repoFIN_Claim.SaveChanges();

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
        /// type : ApproveType.E_FIN_Claim
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

                string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_CLAIM.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                    return DataErrorCode.Error_NoTemplateMail;

                var userApproved = repoSys_UserApprove.FindBy(s => s.ID == userApprovedID).FirstOrDefault();
                var lstUserInfo = repoSys_UserInfo.FindBy(s => s.ID != null).ToList();
                var userInfo = lstUserInfo.Where(s => s.ID == userApproved.UserApproveID).FirstOrDefault();
                var Profile = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref status).FirstOrDefault();
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == currentStatus).FirstOrDefault();
            
                var record = GetData<FIN_ClaimEntity>(recordID, ConstantSql.hrm_hr_sp_get_ClaimById, UserLogin, ref status).FirstOrDefault();
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
                    string urlClickHere = host + "##Hrm_Main_Web/Fin_ApprovedClaim/DetailClaim/" + recordID + "#NewTab";

                    #region process Link Content

                    string linkcontent = string.Empty;
                    foreach (var item in lstUserApprovedNextStatus)
                    {
                        var temp = lstUserInfo.Where(s => s.ID == item.UserApproveID).FirstOrDefault();
                        var name = temp.UserInfoName != null ? temp.UserInfoName : temp.UserLogin;

                        linkcontent += "<a href='" + host + "Fin_ApprovedClaim/ProcessApprovedPage"
                                + "?loginID=" + userInfo.ID
                                + "&userApprovedID=" + item.ID
                                + "&recordID=" + recordID
                                + "'>Click this link to approve and forward to '" + name + "'<a/><br /><br />";
                    }
                    // trường hợp duyệt không có cấp cao hơn userApprovedID  Guid.Empty
                    if (lstUserApprovedNextStatus == null)
                    {
                        linkcontent += "<a href='" + host + "Fin_ApprovedClaim/ProcessApprovedPage"
                                + "?loginID=" + userInfo.ID
                                + "&userApprovedID=" + Guid.Empty
                                + "&recordID=" + recordID
                                + "'>Click this link to approve<a/><br /><br />";
                    }
                    linkcontent += "<a href='" + host + "Fin_ApprovedClaim/ProcessRejectPage"
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
                        EnumDropDown.EmailType_APPROVED_CLAIM.E_PROFILENAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_CLAIM.E_USERINFONAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMNAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMCODE.ToString(), 
                        EnumDropDown.EmailType_APPROVED_CLAIM.E_TRAVELREQUESTNAME.ToString(), 
                        EnumDropDown.EmailType_APPROVED_CLAIM.E_ACCOUNTCODE.ToString(), 
                        EnumDropDown.EmailType_APPROVED_CLAIM.E_TOTAL.ToString(), 
                        EnumDropDown.EmailType_APPROVED_CLAIM.E_STATUS.ToString(), 
                        EnumDropDown.EmailType_APPROVED_CLAIM.E_LINKCONTENT.ToString(), 
                    };
                    strsParaValues = new string[] 
                    { 
                        Profile.ProfileName, 
                        RequestUserInfo.UserInfoName, 
                        record.ClaimName, 
                        record.ClaimCode, 
                        record.TravelRequestName, 
                        record.AccountCode, 
                        record.Total.ToString(), 
                        record.Status,  
                        linkcontent                         
                    };
                    string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);

                    #endregion
                                        
                    //string body = "<html><head><title></title></head><body class='scayt-enabled'>Dear " + Profile.ProfileName + ",<br /><br />"
                    //            + "<b>Claim Request is waiting for your approval:</b><br />"
                    //            + "=========================================<br />"
                    //            + "Request for : " + RequestUserInfo.UserInfoName + " <br />"
                    //            + "Claim Name : " + record.ClaimName + " <br />"
                    //            + "Claim Code : " + record.ClaimCode + " <br />"
                    //            + "TravelRequestName : " + record.TravelRequestName + " <br />"
                    //            + "Account Code : " + record.AccountCode + " <br />"
                    //            + "ProfileName : " + record.ProfileName + " <br />"
                    //            + "Status : " + record.Status + " <br />"
                    //            + "Total : " + record.Total + " <br />"
                    //            + "=========================================<br />"
                    //            ;
                                      
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




        #region Duyệt TravelRequest
        public string ApprovedTravelRequest(Guid travelRequestId, Guid userId, string type) {
            using (var context = new VnrHrmDataContext())
            {
                var message = string.Empty;
                var status = string.Empty;
                var UserLogin = string.Empty;
                var objApprovedClaim = new List<object>();
                objApprovedClaim.AddRange(new object[6]);
                objApprovedClaim[4] = 1;
                objApprovedClaim[5] = int.MaxValue - 1;
                var lstApprovedClaim = GetData<FIN_ApproverECLAIMEntity>(objApprovedClaim, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, UserLogin, ref status).ToList();

                var approvedType1 = FinApproverType.FIN_TravelRequest_Domestic.ToString();
                var approvedType2 = FinApproverType.FIN_TravelRequest_External.ToString();

                var approvedEntity = lstApprovedClaim.Where(s => s.ApprovedID != null && s.ApprovedID.Value == userId && (s.ApprovedType == approvedType1 || s.ApprovedType == approvedType2)).FirstOrDefault();


                //var obTravelRequestItem = new List<object>();
                //obTravelRequestItem.Add(travelRequestId);
                //var lstTravelRequestItem = GetData<FIN_TravelRequestItemEntity>(obTravelRequestItem, ConstantSql.hrm_hr_sp_get_TravelRequestItemByTravelRequestID, ref status).Where(s => s.Description2 != null && s.Description2 == "E_REJECTED").ToList();

                if (approvedEntity != null)
                {
                           
                        var entity = new FIN_HistoryApproveECLAIMEntity();
                        entity.TravelRequestID = (Guid?)travelRequestId;
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

        #region Duyệt Claim
        public string ApprovedClaim(Guid claimId, Guid userId, string type)
        {
            using (var context = new VnrHrmDataContext())
            {
                var message = string.Empty;
                var status = string.Empty;
                var UserLogin = string.Empty;
                var objApprovedClaim = new List<object>();
                objApprovedClaim.AddRange(new object[6]);
                objApprovedClaim[4] = 1;
                objApprovedClaim[5] = int.MaxValue - 1;
                var lstApprovedClaim = GetData<FIN_ApproverECLAIMEntity>(objApprovedClaim, ConstantSql.hrm_fin_sp_get_ApproverECLAIM, UserLogin, ref status).ToList();

                var approvedType1 = FinApproverType.FIN_Claim_Profile.ToString();
                var approvedType2 = FinApproverType.FIN_Claim_Manufacture.ToString();

                var approvedEntity = lstApprovedClaim.Where(s => s.ApprovedID != null && s.ApprovedID.Value == userId && (s.ApprovedType == approvedType1 || s.ApprovedType == approvedType2)).FirstOrDefault();
                var claimEntity = GetData<FIN_ClaimEntity>(claimId, ConstantSql.hrm_hr_sp_get_ClaimById, UserLogin, ref status).FirstOrDefault();

                var objHistory = new List<object>();
                objHistory.AddRange(new object[10]);
                objHistory[8] = 1;
                objHistory[9] = int.MaxValue - 1;
                var lstHistory = GetData<FIN_HistoryApproveECLAIMEntity>(objHistory, ConstantSql.hrm_hr_sp_get_HistoryApprovedClaim, UserLogin, ref status).ToList();

                //var obTravelRequestItem = new List<object>();
                //obTravelRequestItem.Add(claimId);
                //var lstTravelRequestItem = GetData<FIN_TravelRequestItemEntity>(obTravelRequestItem, ConstantSql.hrm_hr_sp_get_TravelRequestItemByTravelRequestID, ref status).Where(s => s.Description2 != null && s.Description2 == "E_REJECTED").ToList();

                if (approvedEntity != null)
                {
                    if (claimEntity != null)
                    {
                        if (claimEntity.CashAdvanceID != null)
                        {
                            var cashAdvanceEntity = GetData<Fin_CashAdvanceEntity>(claimEntity.CashAdvanceID.Value, ConstantSql.hrm_hr_sp_get_CashAdvanceById, UserLogin, ref status).FirstOrDefault();
                            var lstHistoryCash = lstHistory.Where(s => s.CashAdvanceID != null && s.CashAdvanceID.Value == claimEntity.CashAdvanceID.Value && s.Status == "E_APPROVED" && s.UserApproveClaimID != null && s.UserApproveClaimID.Value == userId).ToList();
                            if (cashAdvanceEntity != null)
                            {
                                if (cashAdvanceEntity.TravelRequestID != null)
                                {
                                    var travelRequestEntity = GetData<FIN_TravelRequestEntity>(cashAdvanceEntity.TravelRequestID.Value, ConstantSql.hrm_hr_sp_get_TravelRequestById, UserLogin, ref status).FirstOrDefault();
                                    var lstHistoryTravel = lstHistory.Where(s => s.TravelRequestID != null && s.TravelRequestID.Value == cashAdvanceEntity.TravelRequestID.Value && s.Status == "E_APPROVED" && s.UserApproveClaimID != null && s.UserApproveClaimID.Value == userId).ToList();

                                    if (travelRequestEntity != null)
                                    {
                                        if (lstHistoryTravel.Count == 0)
                                        {
                                            var travelAddentity = new FIN_HistoryApproveECLAIMEntity();
                                            travelAddentity.TravelRequestID = cashAdvanceEntity.TravelRequestID;
                                            travelAddentity.UserApproveClaimID = (Guid?)approvedEntity.ID;
                                            travelAddentity.DateApproved = DateTime.Now;
                                            travelAddentity.Status = type;
                                            message = Add(travelAddentity);
                                        }
                                       
                                    }
                                }

                                if (lstHistoryCash.Count == 0)
                                {
                                    var cashAddEntity = new FIN_HistoryApproveECLAIMEntity();
                                    cashAddEntity.TravelRequestID = cashAdvanceEntity.TravelRequestID;
                                    cashAddEntity.CashAdvanceID = claimEntity.CashAdvanceID;
                                    cashAddEntity.UserApproveClaimID = (Guid?)approvedEntity.ID;
                                    cashAddEntity.DateApproved = DateTime.Now;
                                    cashAddEntity.Status = type;
                                    message = Add(cashAddEntity);
                                }
                                
                            }

                        }
                        var entity = new FIN_HistoryApproveECLAIMEntity();
                        entity.ClaimID = (Guid?)claimId;
                        entity.TravelRequestID = claimEntity.TravelRequestID;
                        entity.CashAdvanceID = claimEntity.CashAdvanceID;
                        entity.UserApproveClaimID = (Guid?)approvedEntity.ID;
                        entity.DateApproved = DateTime.Now;
                        entity.Status = type;
                        message = Add(entity);
                    }

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
                //string typeApprover1 = FinApproverType.FIN_Claim_Profile.ToString();
              //  string typeApprover2 = FinApproverType.FIN_Claim_Manufacture.ToString();
                var UserLogin = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);

                var repoFin_Approver = new CustomBaseRepository<FIN_ApproverECLAIM>(unitOfWork);
                var lstApprover = repoFin_Approver.GetAll().Where(s => s.IsDelete == null).ToList();


                var repoCashItem = new CustomBaseRepository<FIN_ClaimItem>(unitOfWork);
                var totalItem = repoCashItem.GetAll().Where(s => s.ClaimID != null && s.ClaimID.Value == recordID && s.Description2.Contains("E_APPROVED")).Sum(s => s.Amount);

                var repo_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var lstJobTitle = repo_JobTitle.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoOrg = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var lstOrg = repoOrg.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var lstProfile = repoProfile.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);


                var repoFIN_TravelRequest = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();

                string _typeTemplate = EnumDropDown.EmailType.E_REQUEST_CLAIM.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                    return DataErrorCode.Error_NoTemplateMail;


                #region KT nếu là nguoi duyet cuoi thì gui email lại cho người yêu cầu duyệt
                if (userApprovedID == Guid.Empty)
                {
                    if (record != null)
                    {
                        string _typeTemplateApproved = EnumDropDown.EmailType.E_APPROVED_CLAIM.ToString();
                        var typeTemplateApproved = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplateApproved).FirstOrDefault();
                        if (typeTemplateApproved == null)
                        {
                            return DataErrorCode.Error_NoTemplateMail;

                        }
                        var lastApprover = lstApprover.Where(s => record.Type != null && s.ProfileID == record.ProfileID && s.ApprovedType == record.Type && s.ApprovedID != null).OrderByDescending(s => s.OrderNo).FirstOrDefault();
                        var userInfo = repoSys_UserInfo.GetAll().Where(s =>  s.IsDelete == null && s.ID == lastApprover.ApprovedID.Value).FirstOrDefault();
                        var lastProfileEntity = lstProfile.Where(s => userInfo != null && userInfo.ProfileID.Value == s.ID).FirstOrDefault();

                        var profileEntity = lstProfile.Where(s => record.ProfileID != null && record.ProfileID.Value == s.ID).FirstOrDefault();
                        if (profileEntity != null)
                        {
                            var jobTitleEntity = lstJobTitle.Where(s => profileEntity.JobTitleID != null && s.ID == profileEntity.JobTitleID.Value).FirstOrDefault();
                            var orgEntity = lstOrg.Where(s => profileEntity.OrgStructureID != null && s.ID == profileEntity.OrgStructureID.Value).FirstOrDefault();
                            if (typeTemplateApproved != null)
                            {
                                string MailTo = profileEntity.Email;
                                string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedClaim/DetailClaim/" + recordID + "#NewTab";

                                try
                                {
                                    #region magreData

                                    string[] strsParaKey = null;
                                    string[] strsParaValues = null;
                                    strsParaKey = new string[] 
                                    { 
                                         EnumDropDown.EmailType_APPROVED_CLAIM.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_AREA.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMCODE.ToString(),
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_TOTALCLAIM.ToString(), 
 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_DENIES.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_REJECTLINK.ToString(), 

                                    };
                                    strsParaValues = new string[] 
                                    { 
                                        lastProfileEntity != null ? lastProfileEntity.ProfileName : string.Empty,
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileEntity.ProfileName, 
                                        record.ClaimName, 
                                        record.ClaimCode != null ? record.ClaimCode:"" , 
                                         totalItem.ToString(),
                                        "Đã được duyệt.",
                                        "",
                                        ""
                                    };
                                    string body = LibraryService.ReplaceContentFile(typeTemplateApproved.Content, strsParaKey, strsParaValues);

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


                    return DataErrorCode.Success;
                }
                #endregion
                var approverEntity = repoFin_Approver.GetAll().Where(s => s.IsDelete == null && s.ID == userApprovedID).FirstOrDefault();

                if (approverEntity != null && approverEntity.OrderNo != null)
                {
                    var profileInfoEntity = lstProfile.Where(s => s.ID == approverEntity.ProfileID.Value).FirstOrDefault();
                    var jobTitleEntity = lstJobTitle.Where(s => profileInfoEntity.JobTitleID != null && s.ID == profileInfoEntity.JobTitleID.Value).FirstOrDefault();
                    var orgEntity = lstOrg.Where(s => profileInfoEntity.OrgStructureID != null && s.ID == profileInfoEntity.OrgStructureID.Value).FirstOrDefault();
                    var nextApproverEntity = new FIN_ApproverECLAIM();
                    var lastApprover = lstApprover.Where(s => record.Type != null && s.ProfileID == approverEntity.ProfileID && s.ApprovedType == record.Type && s.ApprovedID != null).OrderByDescending(s => s.OrderNo).FirstOrDefault();

                    var lstApproverByClaim = lstApprover.Where(s => record.Type != null && s.ProfileID == approverEntity.ProfileID && s.ApprovedType == record.Type && s.ApprovedID != null && s.OrderNo > approverEntity.OrderNo).OrderBy(s => s.OrderNo).ToList();

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
                            var profileEntity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(userInfo.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();

                            var profileRequester = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(approverEntity.ProfileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref statusMess).FirstOrDefault();

                            if (template != null)
                            {
                                try
                                {
                                    string MailTo = profileEntity.Email;

                                    string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedClaim/DetailClaim/" + recordID + "#NewTab";

                                    string linkcontent = string.Empty;

                                    string linkcontentReject = string.Empty;

                                    linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedClaim/ProcessApprovedPage"
                                   + "?loginID=" + approverEntity.ApprovedID
                                   + "&userApprovedID=" + Guid.Empty
                                   + "&recordID=" + recordID
                                   + "'>đây" + "<a/>" + " để đồng ý.";

                                    linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedClaim/ProcessRejectPage"
                            + "?loginID=" + approverEntity.ApprovedID
                            + "&userApprovedID=" + approverEntity.ID
                            + "&recordID=" + recordID
                            + "'>đây" + "<a/>" + " để từ chối.";

                                    #region magreData

                                    string[] strsParaKey = null;
                                    string[] strsParaValues = null;
                                    strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_AREA.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_TOTALCLAIM.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_DENIES.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_REJECTLINK.ToString(), 
                                    };
                                    strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                         jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileInfoEntity != null ? profileInfoEntity.ProfileName : string.Empty, 
                                        record.ClaimName, 
                                        record.ClaimCode,
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

                    

                   // var nextApproverEntity = repoFin_Approver.GetAll().Where(s => s.IsDelete == null && s.ProfileID == approverEntity.ProfileID && (s.ApprovedType == typeApprover1 || s.ApprovedType == typeApprover2) && s.OrderNo == approverEntity.OrderNo + 1).FirstOrDefault();

                    if (nextApproverEntity != null && nextApproverEntity.ApprovedID != null)
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

                                        string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedClaim/DetailClaim/" + recordID + "#NewTab";

                                        string linkcontent = string.Empty;

                                        string linkcontentReject = string.Empty;

                                        var name = nextUserInfo != null ? nextUserInfo.UserInfoName : nextUserInfo.UserLogin;

                                        linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedClaim/ProcessApprovedPage"
                                       + "?loginID=" + nextApproverEntity.ApprovedID
                                       + "&userApprovedID=" + nextApproverEntity.ID
                                       + "&recordID=" + recordID
                                       + "'>đây" + "<a/>" + "để đồng ý.";

                                        linkcontentReject += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedClaim/ProcessRejectPage"
                                + "?loginID=" + approverEntity.ApprovedID
                                + "&userApprovedID=" + approverEntity.ID
                                + "&recordID=" + recordID
                                + "'>đây" + "<a/>" + "để từ chối.";

                                        #region magreData

                                        string[] strsParaKey = null;
                                        string[] strsParaValues = null;
                                        strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_AREA.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_TOTALCLAIM.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_DENIES.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_REJECTLINK.ToString(), 
                                    };
                                        strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                         jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileInfoEntity != null ? profileInfoEntity.ProfileName : string.Empty, 
                                        record.ClaimName, 
                                        record.ClaimCode,
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
                            if(profileEntity != null)
                            {
                                if(template != null)
                                {
                                    try
                                    {
                                        string MailTo = profileEntity.Email;

                                        string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedClaim/DetailClaim/" + recordID + "#NewTab";

                                        string linkcontent = string.Empty;

                                        string linkcontentReject = string.Empty;

                                        linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedClaim/ProcessApprovedPage"
                                       + "?loginID=" + nextApproverEntity.ApprovedID
                                       + "&userApprovedID=" + Guid.Empty
                                       + "&recordID=" + recordID
                                       + "'>đây" + "<a/>" + " để đồng ý.";

                                        linkcontentReject += "Nhấn vào " +  "<a href='" + host + "Fin_ApprovedClaim/ProcessRejectPage"
                                + "?loginID=" + approverEntity.ApprovedID
                                + "&userApprovedID=" + approverEntity.ID
                                + "&recordID=" + recordID
                                + "'>đây" + "<a/>" + " để từ chối.";

                                        #region magreData

                                        string[] strsParaKey = null;
                                        string[] strsParaValues = null;
                                        strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_AREA.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_TOTALCLAIM.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_DENIES.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_REJECTLINK.ToString(), 
                                    };
                                        strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                         jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileInfoEntity != null ? profileInfoEntity.ProfileName : string.Empty, 
                                        record.ClaimName, 
                                        record.ClaimCode,
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

                string statusMess = string.Empty;
                string type = ApproveType.E_FIN_CashAdvance.ToString();
              //  string typeApprover1 = FinApproverType.FIN_Claim_Profile.ToString();
                //string typeApprover2 = FinApproverType.FIN_Claim_Manufacture.ToString();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var UserLogin = string.Empty;
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var lstUserInfo = repoSys_UserInfo.GetAll().Where(s => s.IsDelete == null).ToList();
                var userInfoEntity = lstUserInfo.Where(s => userId == s.ID).FirstOrDefault();

                var repoCashItem = new CustomBaseRepository<FIN_ClaimItem>(unitOfWork);
                var totalItem = repoCashItem.GetAll().Where(s => s.IsDelete == null && s.ClaimID != null && s.ClaimID.Value == recordID ).Sum(s => s.Amount);

                var repo_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var lstJobTitle = repo_JobTitle.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoOrg = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var lstOrg = repoOrg.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var lstProfile = repoProfile.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoFIN_TravelRequest = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();

                if (record != null)
                {
                    List<Guid> lstIds = new List<Guid>();
                var claimServices = new FIN_ClaimItemService();

                lstIds.Add(record.ID);

                    claimServices.ActionApprovedAll(lstIds, "E_REQUEST", userId);
                }

                var repoFin_Approver = new CustomBaseRepository<FIN_ApproverECLAIM>(unitOfWork);
                var lstApprover = repoFin_Approver.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);

                string _typeTemplate = EnumDropDown.EmailType.E_REQUEST_CLAIM.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();

                if (userInfoEntity != null)
                {
                    var profileInfoEntity = lstProfile.Where(s => s.ID == userInfoEntity.ProfileID.Value).FirstOrDefault();
                    var jobTitleEntity = lstJobTitle.Where(s => profileInfoEntity.JobTitleID != null && s.ID == profileInfoEntity.JobTitleID.Value).FirstOrDefault();
                    var orgEntity = lstOrg.Where(s => profileInfoEntity.OrgStructureID != null && s.ID == profileInfoEntity.OrgStructureID.Value).FirstOrDefault();
                    var lstApproverByClaim = lstApprover.Where(s => record.Type != null &&  s.ProfileID == userInfoEntity.ProfileID && s.ApprovedType == record.Type && s.ApprovedID != null).OrderBy(s => s.OrderNo).ToList();
                    var approvedEntity = new FIN_ApproverECLAIM();


                    var firstApprover = lstApproverByClaim.FirstOrDefault();
                    foreach (var item in lstApproverByClaim)
                    {
                        if(firstApprover.OrderNo == item.OrderNo)
                        {
                            continue;
                        }
                        approvedEntity = item;
                        break;
                    }
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

                                    string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedClaim/DetailClaim/" + recordID + "#NewTab";

                                    string linkcontent = string.Empty;
                                    string linkcontentReject = string.Empty;


                                    var temp = lstUserInfo.Where(s => s.ID == userIdSecondApprover.ApprovedID.Value).FirstOrDefault();
                                    var name = temp.UserInfoName != null ? temp.UserInfoName : temp.UserLogin;

                                    linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedClaim/ProcessApprovedPage"
                                   + "?loginID=" + firstApprover.ApprovedID
                                   + "&userApprovedID=" + userIdSecondApprover.ID
                                   + "&recordID=" + recordID
                                   + "'>đây" + "<a/>" + " để đồng ý.";

                                    linkcontentReject += "Nhấn vào " +  "<a href='" + host + "Fin_ApprovedClaim/ProcessRejectPage"
                                    + "?loginID=" + firstApprover.ApprovedID
                                    + "&userApprovedID=" + userIdSecondApprover.ID
                                    + "&recordID=" + recordID
                                    + "'>đây" + "<a/>" + " để từ chối.";

                                    #region magreData

                                    string[] strsParaKey = null;
                                    string[] strsParaValues = null;
                                    strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_AREA.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_TOTALCLAIM.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_DENIES.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_REJECTLINK.ToString(), 

                                    };
                                    strsParaValues = new string[] 
                                    { 
                                        profileEntity.ProfileName, 
                                        jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        userInfoEntity.UserInfoName, 
                                        record.ClaimName, 
                                        record.ClaimCode,
                                        totalItem.ToString(),
                                        "",
                                         linkcontent ,
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

        public string ProcessApprovedClaim(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            string stt = string.Empty;
            var Services = new BaseService();
            using (var context = new VnrHrmDataContext())
            {
                var services = new FIN_ClaimItemService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoClaim = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                var claimEntity = repoClaim.FindBy(s => s.IsDelete == null && s.ID == recordID).FirstOrDefault();
                
                var claimServices = new FIN_ClaimService();
                var result = ApprovedClaim(recordID, LoginID,type);


                List<Guid> lstId = new List<Guid>();
                if (claimEntity != null)
                {
                    lstId.Add(claimEntity.ID);
                }
                if (result == "Success")
                {
                   services.ActionApprovedAll(lstId, "E_APPROVED", LoginID);
                }
                if (result == "Success")
                {
                    status = SendMaToNextApproved(host, recordID, userApprovedID);
                }

                return status.ToString();
            }
        }


        public DataErrorCode SendMaToPreReject(string host, Guid recordID, Guid userApprovedID, Guid LoginId)
        {
            using (var context = new VnrHrmDataContext())
            {
                string statusMess = string.Empty;
                var Services = new BaseService();
                string status = string.Empty;
                var UserLogin = string.Empty;
                string typeApprover1 = FinApproverType.FIN_Claim_Profile.ToString();
                string typeApprover2 = FinApproverType.FIN_Claim_Manufacture.ToString();

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoFin_Approver = new CustomBaseRepository<FIN_ApproverECLAIM>(unitOfWork);

                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);
                var repo_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var lstJobTitle = repo_JobTitle.GetAll().Where(s => s.IsDelete == null).ToList();
                var repoOrg = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var lstOrg = repoOrg.GetAll().Where(s => s.IsDelete == null).ToList();

                var repoFIN_TravelRequest = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                var record = repoFIN_TravelRequest.FindBy(s => s.ID == recordID).FirstOrDefault();

                var repoCashItem = new CustomBaseRepository<FIN_ClaimItem>(unitOfWork);
                var totalItem = repoCashItem.GetAll().Where(s => s.ClaimID != null && s.ClaimID.Value == recordID && s.Description2.Contains("E_APPROVED")).Sum(s => s.Amount);


                string _typeTemplate = EnumDropDown.EmailType.E_REJECTED_CLAIM.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                    return DataErrorCode.Error_NoTemplateMail;

                var approverEntity = repoFin_Approver.GetAll().Where(s => s.IsDelete == null && s.ApprovedID != null && s.ApprovedID.Value == LoginId).FirstOrDefault();

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

                                        string urlClickHere = host + "#Hrm_Main_Web/Fin_ApprovedClaim/DetailClaim/" + recordID + "#NewTab";

                                        string linkcontent = string.Empty;

                                        string linkcontentReject = string.Empty;

                                        linkcontent += "Nhấn vào " + "<a href='" + host + "Fin_ApprovedClaim/ProcessApprovedPage"
                                       + "?loginID=" + approverEntity.ApprovedID
                                       + "&userApprovedID=" + approverEntity.ID
                                       + "&recordID=" + recordID
                                       + "'>đây" + "<a/>" + " để đồng ý.";

                                        linkcontentReject += "Nhấn vào " +  "<a href='" + host + "Fin_ApprovedClaim/ProcessRejectPage"
                                      + "?loginID=" + approverEntity.ApprovedID
                                      + "&userApprovedID=" + approverEntity.ID
                                      + "&recordID=" + recordID
                                      + "'>đây" + "<a/>" + " để từ chối.";

                                        #region magreData

                                        string[] strsParaKey = null;
                                        string[] strsParaValues = null;
                                        strsParaKey = new string[] 
                                    { 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_PROFILENAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_JOBTITLE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_AREA.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_USERINFONAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMNAME.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMCODE.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_TOTALCLAIM.ToString(), 

                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_DENIES.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_LINKCONTENT.ToString(), 
                                        EnumDropDown.EmailType_APPROVED_CLAIM.E_REJECTLINK.ToString(), 
                                        
                                   

                                    };
                                        strsParaValues = new string[] 
                                    { 
                                        profileEntity != null ? profileEntity.ProfileName : string.Empty, 
                                        
                                         jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                                        orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                                        profileRequest.ProfileName, 

                                        record.ClaimName, 
                                        record.ClaimCode,
                                        totalItem.ToString() != null ? totalItem.ToString(): string.Empty,
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

                    //                string MailTo = profileEntity.Email;


                    //                #region magreData

                    //                string[] strsParaKey = null;
                    //                string[] strsParaValues = null;
                    //                strsParaKey = new string[] 
                    //                { 
                    //                    EnumDropDown.EmailType_APPROVED_CLAIM.E_PROFILENAME.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CLAIM.E_JOBTITLE.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CLAIM.E_AREA.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CLAIM.E_USERINFONAME.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMNAME.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CLAIM.E_CLAIMCODE.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CLAIM.E_DENIES.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CLAIM.E_LINKCONTENT.ToString(), 
                    //                    EnumDropDown.EmailType_APPROVED_CLAIM.E_REJECTLINK.ToString(), 

                    //                };
                    //                strsParaValues = new string[] 
                    //                { 
                    //                    profileEntity.ProfileName, 
                    //                     jobTitleEntity != null ? jobTitleEntity.JobTitleName : string.Empty,
                    //                    orgEntity != null ? orgEntity.OrgStructureName : string.Empty,
                    //                    profileEntity != null ? profileEntity.ProfileName : string.Empty, 
                    //                    record.ClaimName, 
                    //                    record.ClaimCode, 
                    //                    "Đã Bị Từ Chối",
                    //                    "",
                    //                    ""
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

        public string ProcessRejectClaim(string host, Guid LoginID, Guid userApprovedID, Guid recordID, string type)
        {
            DataErrorCode status = DataErrorCode.Success;
            string stt = string.Empty;
            var Services = new BaseService();
            using (var context = new VnrHrmDataContext())
            {
                var services = new FIN_ClaimItemService();
                
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoClaim = new CustomBaseRepository<FIN_Claim>(unitOfWork);
                var claimEntity = repoClaim.FindBy(s => s.IsDelete == null && s.ID == recordID).FirstOrDefault();

                var claimServices = new FIN_ClaimService();
                var result = ApprovedClaim(recordID, LoginID, type);


                List<Guid> lstId = new List<Guid>();
                if (claimEntity != null)
                {
                    lstId.Add(claimEntity.ID);
                }
             
                if (result == "Success")
                {
                    status = SendMaToPreReject(host, recordID, userApprovedID, LoginID);
                }

                return status.ToString();
            }
        }
        

    }
}
