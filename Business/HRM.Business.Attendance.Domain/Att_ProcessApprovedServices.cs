using HRM.Business.Attendance.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Domain
{
    public class Att_ProcessApprovedServices : BaseService
    {
        #region Get Dữ Liệu
        public List<Att_OvertimeEntity> Get_Overtime_WillBeApproveByUser(Guid UserLoginID, List<Att_OvertimeEntity> lstOvertimeAfterFilter, string userLogin)
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.AddRange(new object[10]);
            var lstDelegateApprove = service.GetData<Sys_DelegateApprove>(lstParam, ConstantSql.hrm_sys_sp_get_DelegateApprove, userLogin, ref status);

            DateTime today = DateTime.Today;
            string E_OVERTIME = DelegateApproveType.E_OVERTIME.ToString();
            List<Guid> lstUserCanView = lstDelegateApprove.Where(m => m.UserID != null
                && m.DataTypeDelegate == E_OVERTIME
                && m.DateFrom <= today
                && m.DateTo >= today
                && m.UserDelegateID == UserLoginID).Select(m => m.UserID.Value).ToList();
            lstUserCanView.Add(UserLoginID);
            string E_SUBMIT = OverTimeStatus.E_SUBMIT.ToString();
            string E_APPROVED = OverTimeStatus.E_APPROVED.ToString();
            string E_CONFIRM = OverTimeStatus.E_CONFIRM.ToString();
            string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
            string E_CANCEL = OverTimeStatus.E_CANCEL.ToString();

            var lstOvertime = lstOvertimeAfterFilter.Where(m =>
                m.Status != E_APPROVED
                && m.Status != E_CONFIRM
                && m.Status != E_REJECTED
                && m.Status != E_CANCEL
                && ((m.Status == E_SUBMIT && lstUserCanView.Contains(m.UserApproveID))
                || (m.UserApproveID2 != null && lstUserCanView.Contains(m.UserApproveID2.Value)))).ToList();
            return lstOvertime;
        }
        public List<Att_RosterEntity> Get_Roster_WillBeApproveByUser(Guid UserLoginID, List<Att_RosterEntity> lstRosterAfterFilter, string userLogin)
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.AddRange(new object[10]);
            var lstDelegateApprove = service.GetData<Sys_DelegateApprove>(lstParam, ConstantSql.hrm_sys_sp_get_DelegateApprove, userLogin, ref status);

            DateTime today = DateTime.Today;
            string E_ROSTER = DelegateApproveType.E_ROSTER.ToString();
            List<Guid> lstUserCanView = lstDelegateApprove.Where(m => m.UserID != null
                && m.DataTypeDelegate == E_ROSTER
                && m.DateFrom <= today
                && m.DateTo >= today
                && m.UserDelegateID == UserLoginID).Select(m => m.UserID.Value).ToList();
            lstUserCanView.Add(UserLoginID);
            string E_SUBMIT = RosterStatus.E_SUBMIT.ToString();
            string E_APPROVED = RosterStatus.E_APPROVED.ToString();
            string E_REJECTED = RosterStatus.E_REJECTED.ToString();
            string E_CANCEL = RosterStatus.E_CANCEL.ToString();

            var lstRoster = lstRosterAfterFilter.Where(m =>
                m.Status != E_APPROVED
                && m.Status != E_REJECTED
                && m.Status != E_CANCEL
                && ((m.Status == E_SUBMIT && m.UserApproveID != null && lstUserCanView.Contains(m.UserApproveID.Value))
                || (m.UserApproveID2 != null && lstUserCanView.Contains(m.UserApproveID2.Value)))).ToList();
            return lstRoster;
        }
        public List<Att_LeaveDayEntity> Get_LeaveDay_WillBeApproveByUser(Guid UserLoginID, List<Att_LeaveDayEntity> lstLeavedayAfterFilter, string userLogin)
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.AddRange(new object[10]);
            var lstDelegateApprove = service.GetData<Sys_DelegateApprove>(lstParam, ConstantSql.hrm_sys_sp_get_DelegateApprove, userLogin, ref status);

            DateTime today = DateTime.Today;
            string E_LEAVEDAY = DelegateApproveType.E_LEAVE_DAY.ToString();


            List<Guid> lstUserCanView = lstDelegateApprove.Where(m => m.UserID != null
                && m.DataTypeDelegate == E_LEAVEDAY
                && m.DateFrom <= today
                && m.DateTo >= today
                && m.UserDelegateID == UserLoginID).Select(m => m.UserID.Value).ToList();
            lstUserCanView.Add(UserLoginID);
            string E_SUBMIT = LeaveDayStatus.E_SUBMIT.ToString();
            string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
            string E_REJECTED = LeaveDayStatus.E_REJECTED.ToString();
            string E_CANCEL = LeaveDayStatus.E_CANCEL.ToString();

            var lstLeaveDay = lstLeavedayAfterFilter.Where(m =>
                m.Status != E_APPROVED
                && m.Status != E_REJECTED
                && m.Status != E_CANCEL
                && ((m.Status == E_SUBMIT && m.UserApproveID != null && lstUserCanView.Contains(m.UserApproveID.Value))
                || (m.UserApproveID2 != null && lstUserCanView.Contains(m.UserApproveID2.Value)))).ToList();
            return lstLeaveDay;
        }
        #endregion

        #region SendMail (Khi Approve)
        public string Set_ApproveOvertime_ByModuleApprove(string host, List<Guid> lstOvertime, Guid UserLoginID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                string statusSendMail = DataErrorCode.Error.ToString();
                BaseService service = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Overtime = new CustomBaseRepository<Att_Overtime>(unitOfWork);

                #region getdata

                List<object> lstParamUser = new List<object>();
                lstParamUser.AddRange(new object[4]);
                lstParamUser[3] = Int32.MaxValue;
                var lstUserAll = service.GetData<Sys_UserInfo>(lstParamUser, ConstantSql.hrm_sys_sp_get_users, userLogin, ref status);
                var lstUser = lstUserAll.Where(m => !string.IsNullOrEmpty(m.Email)).Select(m => new { m.ID, m.Email }).ToList();

                //Step1 : Check 1 Approve or 2 approve
                DateTime today = DateTime.Today;
                string E_OVERTIME = DelegateApproveType.E_OVERTIME.ToString();

                List<object> lstParam = new List<object>();
                lstParam.AddRange(new object[10]);
                var lstDelegateApproveAll = service.GetData<Sys_DelegateApprove>(lstParam, ConstantSql.hrm_sys_sp_get_DelegateApprove, userLogin, ref status);

                var lstDelegateApprove = lstDelegateApproveAll.Where(m => m.UserID != null
                    && m.UserDelegateID != null
                    && m.DataTypeDelegate == E_OVERTIME
                    && m.DateFrom <= today
                    && m.DateTo >= today).ToList();

                List<Guid> lstUserCanView = lstDelegateApprove.Where(m => m.UserDelegateID == UserLoginID).Select(m => m.UserID.Value).ToList();
                lstUserCanView.Add(UserLoginID);

                #endregion

                #region process

                string E_SUBMIT = OverTimeStatus.E_SUBMIT.ToString();
                string E_APPROVED = OverTimeStatus.E_APPROVED.ToString();
                string E_FIRST_APPROVED = OverTimeStatus.E_FIRST_APPROVED.ToString();
                List<Att_Overtime> lstOvertimeSendMail = new List<Att_Overtime>();
                foreach (var overID in lstOvertime)
                {
                    var overtime = repoAtt_Overtime.GetById(overID);
                    lstOvertimeSendMail.Add(overtime);


                    string Status = (new Att_OvertimeServices()).GetStatusApproveOvertime(overtime, UserLoginID);

                    if (lstUserCanView.Contains(overtime.UserApproveID) || (overtime.UserApproveID2 != null && lstUserCanView.Contains(overtime.UserApproveID2.Value))
                        || (overtime.UserApproveID3 != null && lstUserCanView.Contains(overtime.UserApproveID3.Value)))
                    {
                        overtime.Status = Status;
                    }


                    //if (overtime.UserApproveID2 != null && lstUserCanView.Contains(overtime.UserApproveID2.Value))
                    //{
                    //    overtime.Status = E_APPROVED;
                    //}
                    //if (overtime.UserApproveID != null && lstUserCanView.Contains(overtime.UserApproveID))
                    //{
                    //    overtime.Status = E_FIRST_APPROVED;
                    //}
                }
                var DataErrorCodeRef = repoAtt_Overtime.SaveChanges();

                #endregion

                if (DataErrorCodeRef == DataErrorCode.Success && lstOvertimeSendMail.Count > 0)
                {
                    #region getdata

                    List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                    List<Sys_TemplateSendMail> template = new List<Sys_TemplateSendMail>();
                    Sys_TemplateSendMail tempApprove = new Sys_TemplateSendMail();
                    Sys_TemplateSendMail tempReturn = new Sys_TemplateSendMail();
                    string[] strsParaKey = null;
                    string bodyContent = null;
                    string titleMail = null;
                    string[] strsParaValues = null;
                    string[] strsParaValues_Return = null;

                    string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_OVERTIME.ToString();
                    string _typeTemplate_return = EnumDropDown.EmailType.E_APPROVED_OVERTIME_RETURN.ToString();
                    var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);
                    template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate || s.Type == _typeTemplate_return).ToList();
                    if (template.Count < 2)
                        return DataErrorCode.Error_NoTemplateMail.ToString();
                    tempApprove = template.Where(s => s.Type == _typeTemplate).FirstOrDefault();
                    tempReturn = template.Where(s => s.Type == _typeTemplate_return).FirstOrDefault();

                    string proIDS = string.Join(",", lstOvertimeSendMail.Select(s => s.ProfileID.ToString()).Distinct().ToList());
                    proIDS = Common.DotNetToOracle(proIDS);
                    lstProfile = GetData<Hre_ProfileEntity>(proIDS, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);

                    strsParaKey = new string[] 
                    { 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_UserName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ProfileName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_CodeEmp.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_OrgStructureName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_WorkDate.ToString(), 

                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_RegisterHours.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ReasonOT.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_Status.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_DurationType.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_OvertimeTypeName.ToString(), 

                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ApprovedHours.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ApprovedBy1.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ApprovedBy2.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_Payment.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_Comment.ToString(), 

                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_IsNightShift.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_LinkContent.ToString(), 
                    };

                    #endregion
                    foreach (var Overtime in lstOvertimeSendMail)
                    {
                        Guid? UserNextApprove = null;
                        if (Overtime.Status == E_FIRST_APPROVED)
                        {
                            UserNextApprove = Overtime.UserApproveID2;
                        }
                        List<Guid> lstDelegateApproveUserID = new List<Guid>();
                        if (UserNextApprove != null)
                        {
                            lstDelegateApproveUserID = lstDelegateApprove.Where(m => m.UserID == UserNextApprove).Select(m => m.UserDelegateID.Value).ToList();
                            lstDelegateApproveUserID.Add(UserNextApprove.Value);
                        }
                        var UserRegister = lstUserAll.Where(m => m.ProfileID == Overtime.ProfileID).FirstOrDefault();
                        if (UserRegister != null)
                        {
                            lstDelegateApproveUserID.Add(UserRegister.ID);
                        }
                        lstDelegateApproveUserID = lstDelegateApproveUserID.Distinct().ToList();
                        var lstUserInfoCanView = lstUser.Where(m => lstDelegateApproveUserID.Contains(m.ID)).ToList();
                        List<Att_EmailRequireEntity> lstEmailRequire = new List<Att_EmailRequireEntity>();
                        foreach (var UserInfoCanView in lstUserInfoCanView)
                        {
                            if (UserInfoCanView.Email == null)
                                continue;
                            Att_EmailRequireEntity EmailRequireEntity = new Att_EmailRequireEntity();
                            EmailRequireEntity.IdObject = Overtime.ID;
                            EmailRequireEntity.EmailUserApprove = UserInfoCanView.Email;
                            EmailRequireEntity.IdUserApprove = UserInfoCanView.ID;
                            if (UserRegister != null && UserInfoCanView.ID == UserRegister.ID)
                            {
                                EmailRequireEntity.IsRegister = true;
                            }
                            lstEmailRequire.Add(EmailRequireEntity);
                        }



                        if (lstEmailRequire.Count > 0)
                        {
                            Guid ID = Overtime.ID;
                            var lstEmailToSend_ByOT = lstEmailRequire.Where(m => m.IdObject == ID).ToList();
                            var profile = lstProfile.Where(s => s.ID == Overtime.ProfileID).FirstOrDefault();
                            var userApproved1 = lstUserAll.Where(s => s.ID == Overtime.UserApproveID).FirstOrDefault();
                            var userApproved2 = lstUserAll.Where(s => s.ID == Overtime.UserApproveID2).FirstOrDefault();




                            foreach (var mail in lstEmailRequire)
                            {
                                bodyContent = string.Empty;

                                if (mail.IsRegister == true)
                                {
                                    #region Send Cho nguoi đăng ký
                                    titleMail = tempReturn.Subject;

                                    strsParaValues_Return = new string[]{
                                            profile.ProfileName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            profile.OrgStructureName,
                                            Overtime.WorkDate.ToString("dd/MMM/yyyy"),

                                            Overtime.RegisterHours.ToString(),
                                            Overtime.ReasonOT,
                                            Overtime.Status,
                                            Overtime.DurationType,
                                            string.Empty,

                                            Overtime.ApproveHours.ToString(),
                                            userApproved1.UserInfoName,
                                            userApproved2.UserInfoName,
                                            Overtime.MethodPayment,
                                            Overtime.Comment,

                                            Overtime.IsNightShift.ToString(),
                                            string.Empty,
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempReturn.Content, strsParaKey, strsParaValues_Return);
                                    #endregion
                                }
                                else
                                {
                                    #region Send cho ngươi duyệt

                                    titleMail = tempApprove.Subject;

                                    string linkcontent = "<br/> <a href='" + host + "Att_ApprovedOvertime/ProcessApprovedPage?loginID=" + mail.IdUserApprove + "&recordID=" + Overtime.ID + "'>Click this link to approve<a/><br /><br />";
                                    linkcontent += "<br/> <a href='" + host + "Att_ApprovedOvertime/ProcessRejectPage?loginID=" + mail.IdUserApprove + "&recordID=" + Overtime.ID + "'>Click this link to Reject<a/><br /><br />";

                                    strsParaValues = new string[]{
                                            userApproved1.UserInfoName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            profile.OrgStructureName,
                                            Overtime.WorkDate.ToString("dd/MMM/yyyy"),

                                            Overtime.RegisterHours.ToString(),
                                            Overtime.ReasonOT,
                                            Overtime.Status,
                                            Overtime.DurationType,
                                            string.Empty,

                                            Overtime.ApproveHours.ToString(),
                                            userApproved1.UserInfoName,
                                            userApproved2.UserInfoName,
                                            Overtime.MethodPayment,
                                            Overtime.Comment,

                                            Overtime.IsNightShift.ToString(),
                                            linkcontent
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempApprove.Content, strsParaKey, strsParaValues);
                                    #endregion
                                }
                                var sta = service.SendMail(titleMail, mail.EmailUserApprove, bodyContent, null);
                                if (sta == true)
                                {
                                    statusSendMail = DataErrorCode.Success.ToString();
                                }
                            }
                        }
                    }
                }
                return statusSendMail;
            }
        }
        public string Set_ApproveRoster_ByModuleApprove(string host, List<Guid> lstRoster, Guid UserLoginID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                string statusSendMail = DataErrorCode.Error.ToString();
                BaseService service = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);

                #region getdata

                List<object> lstParamUser = new List<object>();
                lstParamUser.AddRange(new object[4]);
                lstParamUser[3] = Int32.MaxValue;
                var lstUserAll = service.GetData<Sys_UserInfo>(lstParamUser, ConstantSql.hrm_sys_sp_get_users, userLogin, ref status);
                var lstUser = lstUserAll.Where(m => !string.IsNullOrEmpty(m.Email)).Select(m => new { m.ID, m.Email }).ToList();

                //Step1 : Check 1 Approve or 2 approve
                DateTime today = DateTime.Today;
                string E_ROSTER = DelegateApproveType.E_ROSTER.ToString();

                List<object> lstParam = new List<object>();
                lstParam.AddRange(new object[10]);
                var lstDelegateApproveAll = service.GetData<Sys_DelegateApprove>(lstParam, ConstantSql.hrm_sys_sp_get_DelegateApprove, userLogin, ref status);
                var lstDelegateApprove = lstDelegateApproveAll.Where(m => m.UserID != null
                    && m.UserDelegateID != null
                    && m.DataTypeDelegate == E_ROSTER
                    && m.DateFrom <= today
                    && m.DateTo >= today).ToList();

                List<Guid> lstUserCanView = lstDelegateApprove.Where(m => m.UserDelegateID == UserLoginID).Select(m => m.UserID.Value).ToList();
                lstUserCanView.Add(UserLoginID);

                #endregion

                #region process

                string E_APPROVED = RosterStatus.E_APPROVED.ToString();
                string E_FIRST_APPROVED = RosterStatus.E_FIRST_APPROVED.ToString();
                List<Att_Roster> lstRosterSendMail = new List<Att_Roster>();
                foreach (var rosID in lstRoster)
                {
                    var Roster = repoAtt_Roster.GetById(rosID);
                    lstRosterSendMail.Add(Roster);
                    if (Roster.UserApproveID2 != null && lstUserCanView.Contains(Roster.UserApproveID2.Value))
                    {
                        Roster.Status = E_APPROVED;
                    }
                    if (Roster.UserApproveID != null && lstUserCanView.Contains(Roster.UserApproveID.Value))
                    {
                        Roster.Status = E_FIRST_APPROVED;
                    }
                }
                var stt = repoAtt_Roster.SaveChanges();

                #endregion

                if (stt == DataErrorCode.Success)
                {
                    #region getdata

                    List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                    List<Sys_TemplateSendMail> template = new List<Sys_TemplateSendMail>();
                    Sys_TemplateSendMail tempApprove = new Sys_TemplateSendMail();
                    Sys_TemplateSendMail tempReturn = new Sys_TemplateSendMail();
                    string[] strsParaKey = null;
                    string bodyContent = null;
                    string titleMail = null;
                    string[] strsParaValues = null;
                    string[] strsParaValues_Return = null;

                    string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_ROSTER.ToString();
                    string _typeTemplate_return = EnumDropDown.EmailType.E_APPROVED_ROSTER_RETURN.ToString();
                    var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);
                    template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate || s.Type == _typeTemplate_return).ToList();
                    if (template.Count < 2)
                        return DataErrorCode.Error_NoTemplateMail.ToString();
                    tempApprove = template.Where(s => s.Type == _typeTemplate).FirstOrDefault();
                    tempReturn = template.Where(s => s.Type == _typeTemplate_return).FirstOrDefault();

                    string proIDS = string.Join(",", lstRosterSendMail.Select(s => s.ProfileID.ToString()).Distinct().ToList());
                    proIDS = Common.DotNetToOracle(proIDS);
                    lstProfile = GetData<Hre_ProfileEntity>(proIDS, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);

                    strsParaKey = new string[] 
                    { 

                        EnumDropDown.EmailType_APPROVED_ROSTER.E_UserName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_ProfileName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_CodeEmp.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_DateStart.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_DateEnd.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_Status.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_Type.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_LinkContent.ToString(), 
                    };

                    #endregion
                    foreach (var Roster in lstRosterSendMail)
                    {
                        Guid? UserNextApprove = null;
                        if (Roster.Status == E_FIRST_APPROVED)
                        {
                            UserNextApprove = Roster.UserApproveID2;
                        }
                        List<Guid> lstDelegateApproveUserID = new List<Guid>();
                        if (UserNextApprove != null)
                        {
                            lstDelegateApproveUserID = lstDelegateApprove.Where(m => m.UserID == UserNextApprove).Select(m => m.UserDelegateID.Value).ToList();
                            lstDelegateApproveUserID.Add(UserNextApprove.Value);
                        }
                        var UserRegister = lstUserAll.Where(m => m.ProfileID == Roster.ProfileID).FirstOrDefault();
                        if (UserRegister != null)
                        {
                            lstDelegateApproveUserID.Add(UserRegister.ID);
                        }
                        lstDelegateApproveUserID = lstDelegateApproveUserID.Distinct().ToList();



                        var lstUserInfoCanView = lstUser.Where(m => lstDelegateApproveUserID.Contains(m.ID)).ToList();
                        List<Att_EmailRequireEntity> lstEmailRequire = new List<Att_EmailRequireEntity>();
                        foreach (var UserInfoCanView in lstUserInfoCanView)
                        {
                            if (UserInfoCanView.Email == null)
                                continue;
                            Att_EmailRequireEntity EmailRequireEntity = new Att_EmailRequireEntity();
                            EmailRequireEntity.IdObject = Roster.ID;
                            EmailRequireEntity.EmailUserApprove = UserInfoCanView.Email;
                            EmailRequireEntity.IdUserApprove = UserInfoCanView.ID;
                            if (UserRegister != null && UserInfoCanView.ID == UserRegister.ID)
                            {
                                EmailRequireEntity.IsRegister = true;
                            }
                            lstEmailRequire.Add(EmailRequireEntity);
                        }

                        if (lstEmailRequire.Count > 0)
                        {
                            Guid ID = Roster.ID;
                            var lstEmailToSend_ByOT = lstEmailRequire.Where(m => m.IdObject == ID).ToList();
                            var profile = lstProfile.Where(s => s.ID == Roster.ProfileID).FirstOrDefault();
                            var userApproved1 = lstUserAll.Where(s => s.ID == Roster.UserApproveID).FirstOrDefault();
                            var userApproved2 = lstUserAll.Where(s => s.ID == Roster.UserApproveID2).FirstOrDefault();
                            foreach (var mail in lstEmailRequire)
                            {
                                bodyContent = string.Empty;

                                if (mail.IsRegister == true)
                                {
                                    #region Send Cho nguoi đăng ký
                                    titleMail = tempReturn.Subject;
                                    strsParaValues_Return = new string[]{
                                            profile.ProfileName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Roster.DateStart.ToString("dd/MM/yyyy"),
                                            Roster.DateEnd.ToString("dd/MM/yyyy"),
                                            Roster.Status,
                                            Roster.Type,
                                            string.Empty,
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempReturn.Content, strsParaKey, strsParaValues_Return);
                                    #endregion
                                }
                                else
                                {
                                    #region Send cho ngươi duyệt
                                    string UserNameNext = lstUserAll.Where(m => m.ID == mail.IdUserApprove).Select(m => m.UserInfoName).FirstOrDefault();
                                    titleMail = tempApprove.Subject;
                                    string linkcontent = "<br/> <a href='" + host + "Att_ApprovedOvertime/ProcessApprovedPage?loginID=" + mail.IdUserApprove + "&recordID=" + Roster.ID + "'>Click this link to approve<a/><br /><br />";
                                    linkcontent += "<br/> <a href='" + host + "Att_ApprovedOvertime/ProcessRejectPage?loginID=" + mail.IdUserApprove + "&recordID=" + Roster.ID + "'>Click this link to Reject<a/><br /><br />";
                                    strsParaValues_Return = new string[]{
                                            UserNameNext,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Roster.DateStart.ToString("dd/MM/yyyy"),
                                            Roster.DateEnd.ToString("dd/MM/yyyy"),
                                            Roster.Status,
                                            Roster.Type,
                                            string.Empty,
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempApprove.Content, strsParaKey, strsParaValues_Return);
                                    #endregion
                                }
                                var sta = service.SendMail(titleMail, mail.EmailUserApprove, bodyContent, null);
                                if (sta == true)
                                {
                                    statusSendMail = DataErrorCode.Success.ToString();
                                }
                            }
                        }
                    }
                }
                return statusSendMail;
            }
        }
        public string Set_ApproveLeaveDay_ByModuleApprove(string host, List<Guid> lstLeaveDay, Guid UserLoginID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                string statusSendMail = DataErrorCode.Error.ToString();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                BaseService service = new BaseService();
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);

                #region getdata

                List<object> lstParamUser = new List<object>();
                lstParamUser.AddRange(new object[4]);
                lstParamUser[3] = Int32.MaxValue;
                var lstUserAll = service.GetData<Sys_UserInfo>(lstParamUser, ConstantSql.hrm_sys_sp_get_users, userLogin, ref status);
                var lstUser = lstUserAll.Where(m => !string.IsNullOrEmpty(m.Email)).Select(m => new { m.ID, m.Email }).ToList();

                //Step1 : Check 1 Approve or 2 approve
                DateTime today = DateTime.Today;
                string E_LEAVEDAY = DelegateApproveType.E_LEAVE_DAY.ToString();

                List<object> lstParam = new List<object>();
                lstParam.AddRange(new object[10]);
                var lstDelegateApproveAll = service.GetData<Sys_DelegateApprove>(lstParam, ConstantSql.hrm_sys_sp_get_DelegateApprove, userLogin, ref status);

                var lstDelegateApprove = lstDelegateApproveAll.Where(m => m.UserID != null
                    && m.UserDelegateID != null
                    && m.DataTypeDelegate == E_LEAVEDAY
                    && m.DateFrom <= today
                    && m.DateTo >= today).ToList();

                List<Guid> lstUserCanView = lstDelegateApprove.Where(m => m.UserDelegateID == UserLoginID).Select(m => m.UserID.Value).ToList();
                lstUserCanView.Add(UserLoginID);

                #endregion

                #region process

                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                string E_FIRST_APPROVED = LeaveDayStatus.E_FIRST_APPROVED.ToString();
                List<Att_LeaveDay> lstLeaveDaySendMail = new List<Att_LeaveDay>();
                foreach (var leaID in lstLeaveDay)
                {
                    var LeaveDay = repoAtt_LeaveDay.GetById(leaID);
                    lstLeaveDaySendMail.Add(LeaveDay);
                    if (LeaveDay.UserApproveID2 != null && lstUserCanView.Contains(LeaveDay.UserApproveID2.Value))
                    {
                        LeaveDay.Status = E_APPROVED;
                    }
                    if (LeaveDay.UserApproveID != null && lstUserCanView.Contains(LeaveDay.UserApproveID.Value))
                    {
                        LeaveDay.Status = E_FIRST_APPROVED;
                    }
                }
                var stt = repoAtt_LeaveDay.SaveChanges();

                #endregion


                if (stt == DataErrorCode.Success)
                {
                    #region getdata

                    List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                    List<Sys_TemplateSendMail> template = new List<Sys_TemplateSendMail>();
                    Sys_TemplateSendMail tempApprove = new Sys_TemplateSendMail();
                    Sys_TemplateSendMail tempReturn = new Sys_TemplateSendMail();
                    List<Cat_LeaveDayType> lstLeaveDayType = new List<Cat_LeaveDayType>();
                    string[] strsParaKey = null;
                    string bodyContent = null;
                    string titleMail = null;
                    string[] strsParaValues = null;
                    string[] strsParaValues_Return = null;

                    string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_LEAVEDAY.ToString();
                    string _typeTemplate_return = EnumDropDown.EmailType.E_APPROVED_LEAVEDAY_RETURN.ToString();
                    var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);
                    template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate || s.Type == _typeTemplate_return).ToList();
                    if (template.Count < 2)
                        return DataErrorCode.Error_NoTemplateMail.ToString();

                     var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                    lstLeaveDayType = repoCat_LeaveDayType.FindBy(s => s.IsDelete == null).ToList();

                    tempApprove = template.Where(s => s.Type == _typeTemplate).FirstOrDefault();
                    tempReturn = template.Where(s => s.Type == _typeTemplate_return).FirstOrDefault();

                    string proIDS = string.Join(",", lstLeaveDaySendMail.Select(s => s.ProfileID.ToString()).Distinct().ToList());
                    proIDS = Common.DotNetToOracle(proIDS);
                    lstProfile = GetData<Hre_ProfileEntity>(proIDS, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);

                    strsParaKey = new string[] 
                    { 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_UserName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_ProfileName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_CodeEmp.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_DateStart.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_DateEnd.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_Status.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LeaveDayTypeName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LeaveHours.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LeaveDays.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LinkContent.ToString(), 
                    };

                    #endregion

                    foreach (var Leaveday in lstLeaveDaySendMail)
                    {
                        Guid? UserNextApprove = null;
                        if (Leaveday.Status == E_FIRST_APPROVED)
                        {
                            UserNextApprove = Leaveday.UserApproveID2;
                        }
                         string LeaveDayType = string.Empty;
                        var leavedayTypeObject = lstLeaveDayType.Where(m=>m.ID==Leaveday.ID).FirstOrDefault();
                        if(leavedayTypeObject!= null)
                        {
                            LeaveDayType = leavedayTypeObject.LeaveDayTypeName;
                        }
                        List<Guid> lstDelegateApproveUserID = new List<Guid>();
                        if (UserNextApprove != null)
                        {
                            lstDelegateApproveUserID = lstDelegateApprove.Where(m => m.UserID == UserNextApprove).Select(m => m.UserDelegateID.Value).ToList();
                            lstDelegateApproveUserID.Add(UserNextApprove.Value);
                        }
                        var UserRegister = lstUserAll.Where(m => m.ProfileID == Leaveday.ProfileID).FirstOrDefault();
                        if (UserRegister != null)
                        {
                            lstDelegateApproveUserID.Add(UserRegister.ID);
                        }
                        lstDelegateApproveUserID = lstDelegateApproveUserID.Distinct().ToList();

                        var lstUserInfoCanView = lstUser.Where(m => lstDelegateApproveUserID.Contains(m.ID)).ToList();
                        List<Att_EmailRequireEntity> lstEmailRequire = new List<Att_EmailRequireEntity>();
                        foreach (var UserInfoCanView in lstUserInfoCanView)
                        {
                            if (UserInfoCanView.Email == null)
                                continue;
                            Att_EmailRequireEntity EmailRequireEntity = new Att_EmailRequireEntity();
                            EmailRequireEntity.IdObject = Leaveday.ID;
                            EmailRequireEntity.EmailUserApprove = UserInfoCanView.Email;
                            EmailRequireEntity.IdUserApprove = UserInfoCanView.ID;
                            if (UserRegister != null && UserInfoCanView.ID == UserRegister.ID)
                            {
                                EmailRequireEntity.IsRegister = true;
                            }
                            lstEmailRequire.Add(EmailRequireEntity);
                        }
                        if (lstEmailRequire.Count > 0)
                        {
                            Guid ID = Leaveday.ID;
                            var lstEmailToSend_ByOT = lstEmailRequire.Where(m => m.IdObject == ID).ToList();
                            var profile = lstProfile.Where(s => s.ID == Leaveday.ProfileID).FirstOrDefault();
                            var userApproved1 = lstUserAll.Where(s => s.ID == Leaveday.UserApproveID).FirstOrDefault();
                            var userApproved2 = lstUserAll.Where(s => s.ID == Leaveday.UserApproveID2).FirstOrDefault();
                            foreach (var mail in lstEmailRequire)
                            {
                                bodyContent = string.Empty;

                                if (mail.IsRegister == true)
                                {
                                    #region Send Cho nguoi đăng ký
                                    titleMail = tempReturn.Subject;
                                    strsParaValues_Return = new string[]{
                                            profile.ProfileName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Leaveday.DateStart.ToString("dd/MM/yyyy"),
                                            Leaveday.DateEnd.ToString("dd/MM/yyyy"),
                                            Leaveday.Status,
                                            LeaveDayType,
                                            (Leaveday.LeaveHours??0).ToString(),
                                            (Leaveday.LeaveDays??0).ToString(),
                                            string.Empty,
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempReturn.Content, strsParaKey, strsParaValues_Return);
                                    #endregion
                                }
                                else
                                {
                                    #region Send cho ngươi duyệt
                                    titleMail = tempApprove.Subject;
                                    string UserName = lstUserAll.Where(m => m.ID == mail.IdUserApprove).Select(m => m.UserInfoName).FirstOrDefault();
                                    string linkcontent = "<br/> <a href='" + host + "Att_ApprovedLeaveday/ProcessApprovedPage?loginID=" + mail.IdUserApprove + "&recordID=" + Leaveday.ID + "'>Click this link to approve<a/><br /><br />";
                                    linkcontent += "<br/> <a href='" + host + "Att_ApprovedLeaveday/ProcessRejectPage?loginID=" + mail.IdUserApprove + "&recordID=" + Leaveday.ID + "'>Click this link to Reject<a/><br /><br />";
                                    strsParaValues = new string[]{
                                            UserName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Leaveday.DateStart.ToString("dd/MM/yyyy"),
                                            Leaveday.DateEnd.ToString("dd/MM/yyyy"),
                                            Leaveday.Status,
                                            LeaveDayType,
                                            (Leaveday.LeaveHours??0).ToString(),
                                            (Leaveday.LeaveDays??0).ToString(),
                                            linkcontent
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempApprove.Content, strsParaKey, strsParaValues);
                                    #endregion
                                }
                                var sta = service.SendMail(titleMail, mail.EmailUserApprove, bodyContent, null);
                                if (sta == true)
                                {
                                    statusSendMail = DataErrorCode.Success.ToString();
                                }
                            }
                        }
                    }
                }
                return statusSendMail;
            }


        }
        #endregion

        #region SendMail (Khi Reject)
        public string Set_RejectOvertime_ByModuleApprove(string host, List<Guid> lstOvertime, Guid UserLoginID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                string statusSendMail = DataErrorCode.Error.ToString();
                BaseService service = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Overtime = new CustomBaseRepository<Att_Overtime>(unitOfWork);

                #region getdata

                List<object> lstParamUser = new List<object>();
                lstParamUser.AddRange(new object[4]);
                lstParamUser[3] = Int32.MaxValue;
                var lstUserAll = service.GetData<Sys_UserInfo>(lstParamUser, ConstantSql.hrm_sys_sp_get_users, userLogin, ref status);
                var lstUser = lstUserAll.Where(m => !string.IsNullOrEmpty(m.Email)).Select(m => new { m.ID, m.Email }).ToList();
                #endregion

                #region process

                string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
                List<Att_Overtime> lstOvertimeSendMail = new List<Att_Overtime>();
                foreach (var overID in lstOvertime)
                {
                    var overtime = repoAtt_Overtime.GetById(overID);
                    lstOvertimeSendMail.Add(overtime);
                    overtime.Status = E_REJECTED;
                }
                var DataErrorCodeRef = repoAtt_Overtime.SaveChanges();

                #endregion

                if (DataErrorCodeRef == DataErrorCode.Success)
                {
                    #region getdata

                    List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                    List<Sys_TemplateSendMail> template = new List<Sys_TemplateSendMail>();
                    Sys_TemplateSendMail tempApprove = new Sys_TemplateSendMail();
                    Sys_TemplateSendMail tempReturn = new Sys_TemplateSendMail();
                    string[] strsParaKey = null;
                    string bodyContent = null;
                    string titleMail = null;
                    string[] strsParaValues = null;
                    string[] strsParaValues_Return = null;

                    string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_OVERTIME.ToString();
                    string _typeTemplate_return = EnumDropDown.EmailType.E_APPROVED_OVERTIME_RETURN.ToString();
                    var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);
                    template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate || s.Type == _typeTemplate_return).ToList();
                    if (template.Count < 2)
                        return DataErrorCode.Error_NoTemplateMail.ToString();
                    tempApprove = template.Where(s => s.Type == _typeTemplate).FirstOrDefault();
                    tempReturn = template.Where(s => s.Type == _typeTemplate_return).FirstOrDefault();

                    string proIDS = string.Join(",", lstOvertimeSendMail.Select(s => s.ProfileID.ToString()).Distinct().ToList());
                    proIDS = Common.DotNetToOracle(proIDS);
                    lstProfile = GetData<Hre_ProfileEntity>(proIDS, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);

                    strsParaKey = new string[] 
                    { 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_UserName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ProfileName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_CodeEmp.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_OrgStructureName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_WorkDate.ToString(), 

                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_RegisterHours.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ReasonOT.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_Status.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_DurationType.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_OvertimeTypeName.ToString(), 

                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ApprovedHours.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ApprovedBy1.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ApprovedBy2.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_Payment.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_Comment.ToString(), 

                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_IsNightShift.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_LinkContent.ToString(), 
                    };

                    #endregion

                    foreach (var Overtime in lstOvertimeSendMail)
                    {
                        List<Guid> lstDelegateApproveUserID = new List<Guid>();
                        var UserRegister = lstUserAll.Where(m => m.ProfileID == Overtime.ProfileID).FirstOrDefault();
                        if (UserRegister != null)
                        {
                            lstDelegateApproveUserID.Add(UserRegister.ID);
                        }
                        lstDelegateApproveUserID = lstDelegateApproveUserID.Distinct().ToList();
                        var lstUserInfoCanView = lstUser.Where(m => lstDelegateApproveUserID.Contains(m.ID)).ToList();
                        List<Att_EmailRequireEntity> lstEmailRequire = new List<Att_EmailRequireEntity>();
                        foreach (var UserInfoCanView in lstUserInfoCanView)
                        {
                            if (UserInfoCanView.Email == null)
                                continue;
                            Att_EmailRequireEntity EmailRequireEntity = new Att_EmailRequireEntity();
                            EmailRequireEntity.IdObject = Overtime.ID;
                            EmailRequireEntity.EmailUserApprove = UserInfoCanView.Email;
                            EmailRequireEntity.IdUserApprove = UserInfoCanView.ID;
                            if (UserRegister != null && UserInfoCanView.ID == UserRegister.ID)
                            {
                                EmailRequireEntity.IsRegister = true;
                            }
                            lstEmailRequire.Add(EmailRequireEntity);
                        }


                        if (lstEmailRequire.Count > 0)
                        {
                            Guid ID = Overtime.ID;
                            var lstEmailToSend_ByOT = lstEmailRequire.Where(m => m.IdObject == ID).ToList();
                            var profile = lstProfile.Where(s => s.ID == Overtime.ProfileID).FirstOrDefault();
                            var userApproved1 = lstUserAll.Where(s => s.ID == Overtime.UserApproveID).FirstOrDefault();
                            var userApproved2 = lstUserAll.Where(s => s.ID == Overtime.UserApproveID2).FirstOrDefault();




                            foreach (var mail in lstEmailRequire)
                            {
                                bodyContent = string.Empty;

                                if (mail.IsRegister == true)
                                {
                                    #region Send Cho nguoi đăng ký
                                    titleMail = tempReturn.Subject;

                                    strsParaValues_Return = new string[]{
                                            profile.ProfileName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            profile.OrgStructureName,
                                            Overtime.WorkDate.ToString("dd/MMM/yyyy"),

                                            Overtime.RegisterHours.ToString(),
                                            Overtime.ReasonOT,
                                            Overtime.Status,
                                            Overtime.DurationType,
                                            string.Empty,

                                            Overtime.ApproveHours.ToString(),
                                            userApproved1.UserInfoName,
                                            userApproved2.UserInfoName,
                                            Overtime.MethodPayment,
                                            Overtime.Comment,

                                            Overtime.IsNightShift.ToString(),
                                            string.Empty,
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempReturn.Content, strsParaKey, strsParaValues_Return);
                                    #endregion
                                }
                                else
                                {
                                    #region Send cho ngươi duyệt

                                    titleMail = tempApprove.Subject;

                                    string linkcontent = "<br/> <a href='" + host + "Att_ApprovedOvertime/ProcessApprovedPage?loginID=" + mail.IdUserApprove + "&recordID=" + Overtime.ID + "'>Click this link to approve<a/><br /><br />";
                                    linkcontent += "<br/> <a href='" + host + "Att_ApprovedOvertime/ProcessRejectPage?loginID=" + mail.IdUserApprove + "&recordID=" + Overtime.ID + "'>Click this link to Reject<a/><br /><br />";

                                    strsParaValues = new string[]{
                                            userApproved1.UserInfoName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            profile.OrgStructureName,
                                            Overtime.WorkDate.ToString("dd/MMM/yyyy"),

                                            Overtime.RegisterHours.ToString(),
                                            Overtime.ReasonOT,
                                            Overtime.Status,
                                            Overtime.DurationType,
                                            string.Empty,

                                            Overtime.ApproveHours.ToString(),
                                            userApproved1.UserInfoName,
                                            userApproved2.UserInfoName,
                                            Overtime.MethodPayment,
                                            Overtime.Comment,

                                            Overtime.IsNightShift.ToString(),
                                            linkcontent
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempApprove.Content, strsParaKey, strsParaValues);
                                    #endregion
                                }
                                var sta = service.SendMail(titleMail, mail.EmailUserApprove, bodyContent, null);
                                if (sta == true)
                                {
                                    statusSendMail = DataErrorCode.Success.ToString();
                                }
                            }
                        }

                    }
                }
                return statusSendMail;
            }
        }
        public string Set_RejectRoster_ByModuleApprove(string host, List<Guid> lstRoster, Guid UserLoginID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                string statusSendMail = DataErrorCode.Error.ToString();
                BaseService service = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);

                #region getdata

                List<object> lstParamUser = new List<object>();
                lstParamUser.AddRange(new object[4]);
                lstParamUser[3] = Int32.MaxValue;
                var lstUserAll = service.GetData<Sys_UserInfo>(lstParamUser, ConstantSql.hrm_sys_sp_get_users, userLogin, ref status);
                var lstUser = lstUserAll.Where(m => !string.IsNullOrEmpty(m.Email)).Select(m => new { m.ID, m.Email }).ToList();
                #endregion

                #region process

                string E_REJECTED = RosterStatus.E_REJECTED.ToString();
                List<Att_Roster> lstRosterSendMail = new List<Att_Roster>();
                foreach (var rosID in lstRoster)
                {
                    var Roster = repoAtt_Roster.GetById(rosID);
                    lstRosterSendMail.Add(Roster);
                    Roster.Status = E_REJECTED;
                }
                var stt = repoAtt_Roster.SaveChanges();

                #endregion

                if (stt == DataErrorCode.Success)
                {
                    #region getdata

                    List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                    List<Sys_TemplateSendMail> template = new List<Sys_TemplateSendMail>();
                    Sys_TemplateSendMail tempApprove = new Sys_TemplateSendMail();
                    Sys_TemplateSendMail tempReturn = new Sys_TemplateSendMail();
                    string[] strsParaKey = null;
                    string bodyContent = null;
                    string titleMail = null;
                    string[] strsParaValues = null;
                    string[] strsParaValues_Return = null;

                    string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_ROSTER.ToString();
                    string _typeTemplate_return = EnumDropDown.EmailType.E_APPROVED_ROSTER_RETURN.ToString();
                    var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);
                    template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate || s.Type == _typeTemplate_return).ToList();
                    if (template.Count < 2)
                        return DataErrorCode.Error_NoTemplateMail.ToString();
                    tempApprove = template.Where(s => s.Type == _typeTemplate).FirstOrDefault();
                    tempReturn = template.Where(s => s.Type == _typeTemplate_return).FirstOrDefault();

                    string proIDS = string.Join(",", lstRosterSendMail.Select(s => s.ProfileID.ToString()).Distinct().ToList());
                    proIDS = Common.DotNetToOracle(proIDS);
                    lstProfile = GetData<Hre_ProfileEntity>(proIDS, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);

                    strsParaKey = new string[] 
                    { 

                        EnumDropDown.EmailType_APPROVED_ROSTER.E_UserName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_ProfileName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_CodeEmp.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_DateStart.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_DateEnd.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_Status.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_Type.ToString(), 
                        EnumDropDown.EmailType_APPROVED_ROSTER.E_LinkContent.ToString(), 
                    };

                    #endregion

                    foreach (var Roster in lstRosterSendMail)
                    {
                        List<Guid> lstDelegateApproveUserID = new List<Guid>();
                        var UserRegister = lstUserAll.Where(m => m.ProfileID == Roster.ProfileID).FirstOrDefault();
                        if (UserRegister != null)
                        {
                            lstDelegateApproveUserID.Add(UserRegister.ID);
                        }
                        lstDelegateApproveUserID = lstDelegateApproveUserID.Distinct().ToList();



                        var lstUserInfoCanView = lstUser.Where(m => lstDelegateApproveUserID.Contains(m.ID)).ToList();
                        List<Att_EmailRequireEntity> lstEmailRequire = new List<Att_EmailRequireEntity>();
                        foreach (var UserInfoCanView in lstUserInfoCanView)
                        {
                            if (UserInfoCanView.Email == null)
                                continue;
                            Att_EmailRequireEntity EmailRequireEntity = new Att_EmailRequireEntity();
                            EmailRequireEntity.IdObject = Roster.ID;
                            EmailRequireEntity.EmailUserApprove = UserInfoCanView.Email;
                            EmailRequireEntity.IdUserApprove = UserInfoCanView.ID;
                            if (UserRegister != null && UserInfoCanView.ID == UserRegister.ID)
                            {
                                EmailRequireEntity.IsRegister = true;
                            }
                            lstEmailRequire.Add(EmailRequireEntity);
                        }

                        if (lstEmailRequire.Count > 0)
                        {
                            Guid ID = Roster.ID;
                            var lstEmailToSend_ByOT = lstEmailRequire.Where(m => m.IdObject == ID).ToList();
                            var profile = lstProfile.Where(s => s.ID == Roster.ProfileID).FirstOrDefault();
                            var userApproved1 = lstUserAll.Where(s => s.ID == Roster.UserApproveID).FirstOrDefault();
                            var userApproved2 = lstUserAll.Where(s => s.ID == Roster.UserApproveID2).FirstOrDefault();
                            foreach (var mail in lstEmailRequire)
                            {
                                bodyContent = string.Empty;

                                if (mail.IsRegister == true)
                                {
                                    #region Send Cho nguoi đăng ký
                                    titleMail = tempReturn.Subject;
                                    strsParaValues_Return = new string[]{
                                            profile.ProfileName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Roster.DateStart.ToString("dd/MM/yyyy"),
                                            Roster.DateEnd.ToString("dd/MM/yyyy"),
                                            Roster.Status,
                                            Roster.Type,
                                            string.Empty,
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempReturn.Content, strsParaKey, strsParaValues_Return);
                                    #endregion
                                }
                                else
                                {
                                    #region Send cho ngươi duyệt
                                    string UserNameNext = lstUserAll.Where(m => m.ID == mail.IdUserApprove).Select(m => m.UserInfoName).FirstOrDefault();
                                    titleMail = tempApprove.Subject;
                                    string linkcontent = "<br/> <a href='" + host + "Att_ApprovedOvertime/ProcessApprovedPage?loginID=" + mail.IdUserApprove + "&recordID=" + Roster.ID + "'>Click this link to approve<a/><br /><br />";
                                    linkcontent += "<br/> <a href='" + host + "Att_ApprovedOvertime/ProcessRejectPage?loginID=" + mail.IdUserApprove + "&recordID=" + Roster.ID + "'>Click this link to Reject<a/><br /><br />";
                                    strsParaValues_Return = new string[]{
                                            UserNameNext,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Roster.DateStart.ToString("dd/MM/yyyy"),
                                            Roster.DateEnd.ToString("dd/MM/yyyy"),
                                            Roster.Status,
                                            Roster.Type,
                                            string.Empty,
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempApprove.Content, strsParaKey, strsParaValues);
                                    #endregion
                                }
                                var sta = service.SendMail(titleMail, mail.EmailUserApprove, bodyContent, null);
                                if (sta == true)
                                {
                                    statusSendMail = DataErrorCode.Success.ToString();
                                }
                            }
                        }
                    }
                }
                return statusSendMail;
            }
        }
        public string Set_RejectLeaveDay_ByModuleApprove(string host, List<Guid> lstLeaveDay, Guid UserLoginID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                string statusSendMail = DataErrorCode.Error.ToString();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                BaseService service = new BaseService();
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);

                #region getdata

                List<object> lstParamUser = new List<object>();
                lstParamUser.AddRange(new object[4]);
                lstParamUser[3] = Int32.MaxValue;
                var lstUserAll = service.GetData<Sys_UserInfo>(lstParamUser, ConstantSql.hrm_sys_sp_get_users, userLogin, ref status);
                var lstUser = lstUserAll.Where(m => !string.IsNullOrEmpty(m.Email)).Select(m => new { m.ID, m.Email }).ToList();
                #endregion

                #region process

                string E_REJECTED = LeaveDayStatus.E_REJECTED.ToString();
                List<Att_LeaveDay> lstLeaveDaySendMail = new List<Att_LeaveDay>();
                foreach (var leaID in lstLeaveDay)
                {
                    var LeaveDay = repoAtt_LeaveDay.GetById(leaID);
                    lstLeaveDaySendMail.Add(LeaveDay);
                    LeaveDay.Status = E_REJECTED;

                }
                var stt = repoAtt_LeaveDay.SaveChanges();

                #endregion


                if (stt == DataErrorCode.Success)
                {
                    #region getdata

                    List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                    List<Sys_TemplateSendMail> template = new List<Sys_TemplateSendMail>();
                    Sys_TemplateSendMail tempApprove = new Sys_TemplateSendMail();
                    Sys_TemplateSendMail tempReturn = new Sys_TemplateSendMail();
                    List<Cat_LeaveDayType> lstLeaveDayType = new List<Cat_LeaveDayType>();
                    string[] strsParaKey = null;
                    string bodyContent = null;
                    string titleMail = null;
                    string[] strsParaValues = null;
                    string[] strsParaValues_Return = null;

                    string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_LEAVEDAY.ToString();
                    string _typeTemplate_return = EnumDropDown.EmailType.E_APPROVED_LEAVEDAY_RETURN.ToString();
                    var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);
                    template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate || s.Type == _typeTemplate_return).ToList();
                    if (template.Count < 2)
                        return DataErrorCode.Error_NoTemplateMail.ToString();

                    var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                    lstLeaveDayType = repoCat_LeaveDayType.FindBy(s => s.IsDelete == null).ToList();

                    tempApprove = template.Where(s => s.Type == _typeTemplate).FirstOrDefault();
                    tempReturn = template.Where(s => s.Type == _typeTemplate_return).FirstOrDefault();

                    string proIDS = string.Join(",", lstLeaveDaySendMail.Select(s => s.ProfileID.ToString()).Distinct().ToList());
                    proIDS = Common.DotNetToOracle(proIDS);
                    lstProfile = GetData<Hre_ProfileEntity>(proIDS, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);

                    strsParaKey = new string[] 
                    { 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_UserName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_ProfileName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_CodeEmp.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_DateStart.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_DateEnd.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_Status.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LeaveDayTypeName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LeaveHours.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LeaveDays.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LinkContent.ToString(), 
                    };

                    #endregion

                    foreach (var Leaveday in lstLeaveDaySendMail)
                    {
                        List<Guid> lstDelegateApproveUserID = new List<Guid>();
                        var UserRegister = lstUserAll.Where(m => m.ProfileID == Leaveday.ProfileID).FirstOrDefault();
                        if (UserRegister != null)
                        {
                            lstDelegateApproveUserID.Add(UserRegister.ID);
                        }
                        lstDelegateApproveUserID = lstDelegateApproveUserID.Distinct().ToList();
                        string LeaveDayType = string.Empty;
                        var leavedayTypeObject = lstLeaveDayType.Where(m => m.ID == Leaveday.ID).FirstOrDefault();
                        if (leavedayTypeObject != null)
                        {
                            LeaveDayType = leavedayTypeObject.LeaveDayTypeName;
                        }
                        var lstUserInfoCanView = lstUser.Where(m => lstDelegateApproveUserID.Contains(m.ID)).ToList();
                        List<Att_EmailRequireEntity> lstEmailRequire = new List<Att_EmailRequireEntity>();
                        foreach (var UserInfoCanView in lstUserInfoCanView)
                        {
                            if (UserInfoCanView.Email == null)
                                continue;
                            Att_EmailRequireEntity EmailRequireEntity = new Att_EmailRequireEntity();
                            EmailRequireEntity.IdObject = Leaveday.ID;
                            EmailRequireEntity.EmailUserApprove = UserInfoCanView.Email;
                            EmailRequireEntity.IdUserApprove = UserInfoCanView.ID;
                            if (UserRegister != null && UserInfoCanView.ID == UserRegister.ID)
                            {
                                EmailRequireEntity.IsRegister = true;
                            }
                            lstEmailRequire.Add(EmailRequireEntity);
                        }
                        if (lstEmailRequire.Count > 0)
                        {
                            Guid ID = Leaveday.ID;
                            var lstEmailToSend_ByOT = lstEmailRequire.Where(m => m.IdObject == ID).ToList();
                            var profile = lstProfile.Where(s => s.ID == Leaveday.ProfileID).FirstOrDefault();
                            var userApproved1 = lstUserAll.Where(s => s.ID == Leaveday.UserApproveID).FirstOrDefault();
                            var userApproved2 = lstUserAll.Where(s => s.ID == Leaveday.UserApproveID2).FirstOrDefault();
                            foreach (var mail in lstEmailRequire)
                            {
                                bodyContent = string.Empty;

                                if (mail.IsRegister == true)
                                {
                                    #region Send Cho nguoi đăng ký
                                    titleMail = tempReturn.Subject;
                                    strsParaValues_Return = new string[]{
                                            profile.ProfileName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Leaveday.DateStart.ToString("dd/MM/yyyy"),
                                            Leaveday.DateEnd.ToString("dd/MM/yyyy"),
                                            Leaveday.Status,
                                            LeaveDayType,
                                            (Leaveday.LeaveHours??0).ToString(),
                                            (Leaveday.LeaveDays??0).ToString(),
                                            string.Empty,
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempReturn.Content, strsParaKey, strsParaValues_Return);
                                    #endregion
                                }
                                else
                                {
                                    #region Send cho ngươi duyệt
                                    titleMail = tempApprove.Subject;
                                    string UserName = lstUserAll.Where(m => m.ID == mail.IdUserApprove).Select(m => m.UserInfoName).FirstOrDefault();
                                    string linkcontent = "<br/> <a href='" + host + "Att_ApprovedLeaveday/ProcessApprovedPage?loginID=" + mail.IdUserApprove + "&recordID=" + Leaveday.ID + "'>Click this link to approve<a/><br /><br />";
                                    linkcontent += "<br/> <a href='" + host + "Att_ApprovedLeaveday/ProcessRejectPage?loginID=" + mail.IdUserApprove + "&recordID=" + Leaveday.ID + "'>Click this link to Reject<a/><br /><br />";
                                    strsParaValues = new string[]{
                                            UserName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Leaveday.DateStart.ToString("dd/MM/yyyy"),
                                            Leaveday.DateEnd.ToString("dd/MM/yyyy"),
                                            Leaveday.Status,
                                            LeaveDayType,
                                            (Leaveday.LeaveHours??0).ToString(),
                                            (Leaveday.LeaveDays??0).ToString(),
                                            linkcontent
                                        };
                                    bodyContent = LibraryService.ReplaceContentFile(tempApprove.Content, strsParaKey, strsParaValues);
                                    #endregion
                                }
                                var sta = service.SendMail(titleMail, mail.EmailUserApprove, bodyContent, null);
                                if (sta == true)
                                {
                                    statusSendMail = DataErrorCode.Success.ToString();
                                }
                            }
                        }
                    }
                }
                return statusSendMail;
            }


        }
        #endregion

        #region SendMAIL (Khi Đăng Ký)
        public string GetEmailToSend_Overtime(List<Att_OvertimeEntity> lstOvertime, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DateTime today = DateTime.Today;
                string statusSendMail = DataErrorCode.Error.ToString();
                string E_OVERTIME = DelegateApproveType.E_OVERTIME.ToString();
                BaseService service = new BaseService();
                string status = string.Empty;

                #region Process get lstEmail
                List<object> lstParamUser = new List<object>();
                lstParamUser.AddRange(new object[4]);
                lstParamUser[3] = Int32.MaxValue;
                var lstUserAll = service.GetData<Sys_UserInfo>(lstParamUser, ConstantSql.hrm_sys_sp_get_users, userLogin, ref status);
                List<object> lstParam = new List<object>();
                lstParam.AddRange(new object[10]);
                var lstDelegateApprove = service.GetData<Sys_DelegateApprove>(lstParam, ConstantSql.hrm_sys_sp_get_DelegateApprove, userLogin, ref status);
                List<Att_EmailRequireEntity> lstEmailRequire = new List<Att_EmailRequireEntity>();
                foreach (var item in lstOvertime)
                {
                    List<Guid> UserApproveByItem = new List<Guid>();
                    if (item.UserApproveID != null)
                    {
                        UserApproveByItem.Add(item.UserApproveID);
                    }
                    if (item.UserApproveID2 != null)
                    {
                        UserApproveByItem.Add(item.UserApproveID2.Value);
                    }
                    List<Guid> lstUserCanView = lstDelegateApprove.Where(m => m.UserID != null
                     && m.DataTypeDelegate == E_OVERTIME
                     && m.DateFrom <= today
                     && m.DateTo >= today
                     && m.UserID != null
                     && UserApproveByItem.Contains(m.UserID.Value)).Select(m => m.UserDelegateID.Value).ToList();
                    lstUserCanView.AddRange(UserApproveByItem);
                    var UserInfoRegisterID = lstUserAll.Where(m => m.ProfileID == item.ProfileID).Select(m => m.ID).FirstOrDefault();
                    if (UserInfoRegisterID != null && UserInfoRegisterID != Guid.Empty)
                    {
                        lstUserCanView.Add(UserInfoRegisterID);
                    }
                    lstUserCanView = lstUserCanView.Distinct().ToList();

                    var lstUserInfoCanView = lstUserAll.Where(m => lstUserCanView.Contains(m.ID)).ToList();
                    foreach (var UserInfoCanView in lstUserInfoCanView)
                    {
                        if (UserInfoCanView.Email == null)
                            continue;
                        Att_EmailRequireEntity EmailRequireEntity = new Att_EmailRequireEntity();
                        EmailRequireEntity.IdObject = item.ID;
                        EmailRequireEntity.EmailUserApprove = UserInfoCanView.Email;
                        EmailRequireEntity.IdUserApprove = UserInfoCanView.ID;
                        if (UserInfoRegisterID != null && UserInfoRegisterID != Guid.Empty && UserInfoCanView.ID == UserInfoRegisterID)
                        {
                            EmailRequireEntity.IsRegister = true;
                        }
                        lstEmailRequire.Add(EmailRequireEntity);
                    }
                }
                #endregion

                #region get Template

                List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                List<Sys_TemplateSendMail> template = new List<Sys_TemplateSendMail>();
                Sys_TemplateSendMail tempApprove = new Sys_TemplateSendMail();
                Sys_TemplateSendMail tempReturn = new Sys_TemplateSendMail();
                string[] strsParaKey = null;
                string bodyContent = null;
                string titleMail = null;
                string[] strsParaValues = null;
                string[] strsParaValues_Return = null;

                if (lstOvertime.Count > 0)
                {
                    string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_OVERTIME.ToString();
                    string _typeTemplate_return = EnumDropDown.EmailType.E_APPROVED_OVERTIME_RETURN.ToString();
                    var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);
                    template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate || s.Type == _typeTemplate_return).ToList();
                    if (template.Count < 2)
                        return DataErrorCode.Error_NoTemplateMail.ToString();
                    tempApprove = template.Where(s => s.Type == _typeTemplate).FirstOrDefault();
                    tempReturn = template.Where(s => s.Type == _typeTemplate_return).FirstOrDefault();

                    string proIDS = string.Join(",", lstOvertime.Select(s => s.ProfileID.ToString()).Distinct().ToList());
                    proIDS = Common.DotNetToOracle(proIDS);
                    lstProfile = GetData<Hre_ProfileEntity>(proIDS, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);

                    strsParaKey = new string[] 
                    { 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_UserName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ProfileName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_CodeEmp.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_OrgStructureName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_WorkDate.ToString(), 

                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_RegisterHours.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ReasonOT.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_Status.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_DurationType.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_OvertimeTypeName.ToString(), 

                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ApprovedHours.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ApprovedBy1.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_ApprovedBy2.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_Payment.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_Comment.ToString(), 

                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_IsNightShift.ToString(), 
                        EnumDropDown.EmailType_APPROVED_OVERTIME.E_LinkContent.ToString(), 
                    };

                }

                #endregion

                #region process Send Mail

                foreach (var Overtime in lstOvertime)
                {
                    Guid ID = Overtime.ID;
                    var lstEmailToSend_ByOT = lstEmailRequire.Where(m => m.IdObject == ID).ToList();
                    var profile = lstProfile.Where(s => s.ID == Overtime.ProfileID).FirstOrDefault();
                    var userApproved1 = lstUserAll.Where(s => s.ID == Overtime.UserApproveID).FirstOrDefault();
                    var userApproved2 = lstUserAll.Where(s => s.ID == Overtime.UserApproveID2).FirstOrDefault();

                    foreach (var item in lstEmailToSend_ByOT)
                    {
                        bodyContent = string.Empty;

                        if (item.IsRegister == true)
                        {
                            #region Send Cho nguoi đăng ký
                            titleMail = tempReturn.Subject;

                            strsParaValues_Return = new string[]{
                                            profile.ProfileName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            profile.OrgStructureName,
                                            Overtime.WorkDate.ToString("dd/MMM/yyyy"),

                                            Overtime.RegisterHours.ToString(),
                                            Overtime.ReasonOT,
                                            Overtime.Status,
                                            Overtime.DurationType,
                                            string.Empty,

                                            Overtime.ApproveHours.ToString(),
                                            userApproved1.UserInfoName,
                                            userApproved2.UserInfoName,
                                            Overtime.MethodPayment,
                                            Overtime.Comment,

                                            Overtime.IsNightShift.ToString(),
                                            string.Empty,
                                        };
                            bodyContent = LibraryService.ReplaceContentFile(tempReturn.Content, strsParaKey, strsParaValues_Return);
                            #endregion
                        }
                        else
                        {
                            #region Send cho ngươi duyệt

                            titleMail = tempApprove.Subject;

                            string linkcontent = "<br/> <a href='" + Overtime.Host + "Att_ApprovedOvertime/ProcessApprovedPage?loginID=" + item.IdUserApprove + "&recordID=" + Overtime.ID + "'>Click this link to approve<a/><br /><br />";
                            linkcontent += "<br/> <a href='" + Overtime.Host + "Att_ApprovedOvertime/ProcessRejectPage?loginID=" + item.IdUserApprove + "&recordID=" + Overtime.ID + "'>Click this link to Reject<a/><br /><br />";

                            strsParaValues = new string[]{
                                            userApproved1.UserInfoName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            profile.OrgStructureName,
                                            Overtime.WorkDate.ToString("dd/MMM/yyyy"),

                                            Overtime.RegisterHours.ToString(),
                                            Overtime.ReasonOT,
                                            Overtime.Status,
                                            Overtime.DurationType,
                                            string.Empty,

                                            Overtime.ApproveHours.ToString(),
                                            userApproved1.UserInfoName,
                                            userApproved2.UserInfoName,
                                            Overtime.MethodPayment,
                                            Overtime.Comment,

                                            Overtime.IsNightShift.ToString(),
                                            linkcontent
                                        };
                            bodyContent = LibraryService.ReplaceContentFile(tempApprove.Content, strsParaKey, strsParaValues);
                            #endregion
                        }
                        var sta = service.SendMail(titleMail, item.EmailUserApprove, bodyContent, null);
                        if (sta == true)
                        {
                            statusSendMail = DataErrorCode.Success.ToString();
                        }
                    }
                }
                #endregion
                return statusSendMail;
            }
        }
        public string GetEmailToSend_LeaveDay(List<Att_LeaveDayEntity> lstLeaveDay, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                DateTime today = DateTime.Today;
                string statusSendMail = DataErrorCode.Error.ToString();
                string E_LEAVEDAY = DelegateApproveType.E_LEAVE_DAY.ToString();
                BaseService service = new BaseService();
                string status = string.Empty;

                #region Process get lstEmail

                List<object> lstParamUser = new List<object>();
                lstParamUser.AddRange(new object[4]);
                lstParamUser[3] = Int32.MaxValue;
                var lstUserAll = service.GetData<Sys_UserInfo>(lstParamUser, ConstantSql.hrm_sys_sp_get_users, userLogin, ref status);

                List<object> lstParam = new List<object>();
                lstParam.AddRange(new object[10]);
                var lstDelegateApprove = service.GetData<Sys_DelegateApprove>(lstParam, ConstantSql.hrm_sys_sp_get_DelegateApprove, userLogin, ref status);

                List<Att_EmailRequireEntity> lstEmailRequire = new List<Att_EmailRequireEntity>();
                foreach (var item in lstLeaveDay)
                {
                    List<Guid> UserApproveByItem = new List<Guid>();
                    if (item.UserApproveID != null)
                    {
                        UserApproveByItem.Add(item.UserApproveID.Value);
                    }
                    if (item.UserApproveID2 != null)
                    {
                        UserApproveByItem.Add(item.UserApproveID2.Value);
                    }
                    List<Guid> lstUserCanView = lstDelegateApprove.Where(m => m.UserID != null
                     && m.DataTypeDelegate == E_LEAVEDAY
                     && m.DateFrom <= today
                     && m.DateTo >= today
                     && m.UserID != null
                     && UserApproveByItem.Contains(m.UserID.Value)).Select(m => m.UserDelegateID.Value).ToList();
                    lstUserCanView.AddRange(UserApproveByItem);
                    var UserInfoRegisterID = lstUserAll.Where(m => m.ProfileID == item.ProfileID).Select(m => m.ID).FirstOrDefault();
                    if (UserInfoRegisterID != null && UserInfoRegisterID != Guid.Empty)
                    {
                        lstUserCanView.Add(UserInfoRegisterID);
                    }
                    lstUserCanView = lstUserCanView.Distinct().ToList();

                    var lstUserInfoCanView = lstUserAll.Where(m => lstUserCanView.Contains(m.ID)).ToList();
                    foreach (var UserInfoCanView in lstUserInfoCanView)
                    {
                        if (UserInfoCanView.Email == null)
                            continue;
                        Att_EmailRequireEntity EmailRequireEntity = new Att_EmailRequireEntity();
                        EmailRequireEntity.IdObject = item.ID;
                        EmailRequireEntity.EmailUserApprove = UserInfoCanView.Email;
                        EmailRequireEntity.IdUserApprove = UserInfoCanView.ID;
                        if (UserInfoRegisterID != null && UserInfoRegisterID != Guid.Empty && UserInfoCanView.ID == UserInfoRegisterID)
                        {
                            EmailRequireEntity.IsRegister = true;
                        }
                        lstEmailRequire.Add(EmailRequireEntity);
                    }
                }

                #endregion

                #region get Template

                List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();
                List<Sys_TemplateSendMail> template = new List<Sys_TemplateSendMail>();
                Sys_TemplateSendMail tempApprove = new Sys_TemplateSendMail();
                Sys_TemplateSendMail tempReturn = new Sys_TemplateSendMail();
                List<Cat_LeaveDayType> lstLeaveDayType = new List<Cat_LeaveDayType>();
                string[] strsParaKey = null;
                string bodyContent = null;
                string titleMail = null;
                string[] strsParaValues = null;
                string[] strsParaValues_Return = null;

                if (lstLeaveDay.Count > 0)
                {
                    string _typeTemplate = EnumDropDown.EmailType.E_APPROVED_LEAVEDAY.ToString();
                    string _typeTemplate_return = EnumDropDown.EmailType.E_APPROVED_LEAVEDAY_RETURN.ToString();
                    var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);
                    template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate || s.Type == _typeTemplate_return).ToList();
                    if (template.Count < 2)
                        return DataErrorCode.Error_NoTemplateMail.ToString();

                     var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                    lstLeaveDayType = repoCat_LeaveDayType.FindBy(s => s.IsDelete == null).ToList();


                    tempApprove = template.Where(s => s.Type == _typeTemplate).FirstOrDefault();
                    tempReturn = template.Where(s => s.Type == _typeTemplate_return).FirstOrDefault();

                    string proIDS = string.Join(",", lstLeaveDay.Select(s => s.ProfileID.ToString()).Distinct().ToList());
                    proIDS = Common.DotNetToOracle(proIDS);
                    lstProfile = GetData<Hre_ProfileEntity>(proIDS, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);

                    strsParaKey = new string[] 
                    { 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_UserName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_ProfileName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_CodeEmp.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_DateStart.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_DateEnd.ToString(), 
                                                        
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_Status.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LeaveDayTypeName.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_Status.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LeaveDays.ToString(), 
                        EnumDropDown.EmailType_APPROVED_LEAVEDAY.E_LinkContent.ToString()
                    };

                }

                #endregion

                #region process Send Mail

                foreach (var Leaveday in lstLeaveDay)
                {
                    Guid ID = Leaveday.ID;
                    var lstEmailToSend_ByOT = lstEmailRequire.Where(m => m.IdObject == ID).ToList();
                    var profile = lstProfile.Where(s => s.ID == Leaveday.ProfileID).FirstOrDefault();
                    var userApproved1 = lstUserAll.Where(s => s.ID == Leaveday.UserApproveID).FirstOrDefault();
                    var userApproved2 = lstUserAll.Where(s => s.ID == Leaveday.UserApproveID2).FirstOrDefault();

                    string LeaveDayType = string.Empty;
                    var leavedayTypeObject = lstLeaveDayType.Where(m=>m.ID==Leaveday.ID).FirstOrDefault();
                    if(leavedayTypeObject!= null)
                    {
                        LeaveDayType = leavedayTypeObject.LeaveDayTypeName;
                    }

                    foreach (var item in lstEmailToSend_ByOT)
                    {
                        bodyContent = string.Empty;

                        if (item.IsRegister == true)
                        {
                            #region Send Cho nguoi đăng ký
                            titleMail = tempReturn.Subject;

                            strsParaValues_Return = new string[]{
                                            profile.ProfileName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Leaveday.DateStart.ToString("dd/MM/yyyy"),
                                            Leaveday.DateEnd.ToString("dd/MM/yyyy"),
                                            Leaveday.Status,
                                            LeaveDayType,
                                            (Leaveday.LeaveHours ?? 0).ToString(),
                                            (Leaveday.TotalLeave ?? 0).ToString(),
                                            string.Empty,
                                        };
                            bodyContent = LibraryService.ReplaceContentFile(tempReturn.Content, strsParaKey, strsParaValues_Return);
                            #endregion
                        }
                        else
                        {
                            #region Send cho ngươi duyệt

                            string UserName = lstUserAll.Where(m => m.ID == item.IdUserApprove).Select(m => m.UserInfoName).FirstOrDefault();

                            titleMail = tempApprove.Subject;

                            string linkcontent = "<br/> <a href='" + Leaveday.Host + "Att_ApprovedLeaveDay/ProcessApprovedPage?loginID=" + item.IdUserApprove + "&recordID=" + Leaveday.ID + "'>Click this link to approve<a/><br /><br />";
                            linkcontent += "<br/> <a href='" + Leaveday.Host + "Att_ApprovedLeaveday/ProcessRejectPage?loginID=" + item.IdUserApprove + "&recordID=" + Leaveday.ID + "'>Click this link to Reject<a/><br /><br />";

                            strsParaValues = new string[]{
                                            UserName,
                                            profile.ProfileName,
                                            profile.CodeEmp,
                                            Leaveday.DateStart.ToString("dd/MM/yyyy"),
                                            Leaveday.DateEnd.ToString("dd/MM/yyyy"),
                                            Leaveday.Status,
                                            LeaveDayType,
                                            (Leaveday.LeaveHours ?? 0).ToString(),
                                            (Leaveday.TotalLeave ?? 0).ToString(),
                                            linkcontent
                                        };
                            bodyContent = LibraryService.ReplaceContentFile(tempApprove.Content, strsParaKey, strsParaValues);
                            #endregion
                        }
                        var sta = service.SendMail(titleMail, item.EmailUserApprove, bodyContent, null);
                        if (sta == true)
                        {
                            statusSendMail = DataErrorCode.Success.ToString();
                        }
                    }
                }
                #endregion
                return statusSendMail;
            }
        }
        #endregion
    }
}
