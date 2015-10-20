using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System.Linq;
using System;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using System.IO;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_UserServices : BaseService
    {
        public DateTime CurrentDateTime()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_UserRepository(unitOfWork);
                return repo.CurrentDateTime();
            }
        }

        public bool IsDuplicationUser(string loginName, Guid id)
        {
            using (var context = new VnrHrmDataContext())
            {
                var isExistedUser = false;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_UserRepository(unitOfWork);
                var existedUser = repo.FindBy(p => p.UserLogin == loginName && (p.ID != id)).FirstOrDefault();
                if (existedUser != null)
                {
                    isExistedUser = true;
                }
                return isExistedUser;
            }
        }



        public Sys_UserInfoEntity Login(string userName, string password, string googleEmail, string googleProfileName, bool isSignIn)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_UserRepository(unitOfWork);
                var userInfoEntity = new Sys_UserInfoEntity();
                var status = string.Empty;

                if (isSignIn)
                {
                    //neu signIn bằng google mail => kt email co tồn tại trong hệ thống VNR không, nếu không thì tạo mới user
                    userInfoEntity = CheckEmailExisted(googleEmail, googleProfileName);
                }
                else
                {
                    //login he thong VNR
                    userInfoEntity = repo.Login(userName, password);
                }

                #region Kiem tra có dự Án không?

                if (userInfoEntity != null)
                {
             
                    
                    var vesionLastName = unitOfWork.CreateQueryable<Sys_Version>(s => s.IsDelete == null).OrderByDescending(s => s.DateCreate).Select(s => s.Value).FirstOrDefault();
                    var vesionLastNameFile = GetVesionLastNameFile();
                    if (!string.IsNullOrEmpty(vesionLastNameFile) && vesionLastName != vesionLastNameFile)
                    {
                        userInfoEntity.IsRunScriptNew = true;
                        userInfoEntity.VesionLastName = vesionLastName;
                    }
                    var masterDataGroups = unitOfWork.CreateQueryable<Sys_UserMasterData>(d =>
                        d.UserID == userInfoEntity.ID).Select(d => d.MasterDataGroups).ToList();

                    if (masterDataGroups.Any(d => d != null && d.ToNumbers().Count() > 0))
                    {
                        userInfoEntity.HasMasterDataGroup = true;
                    }
                }
                else
                {
                    userInfoEntity = new Sys_UserInfoEntity();
                    userInfoEntity.UserLogin = userName;
                    userInfoEntity.Password = password;
                    var vesionLastName = unitOfWork.CreateQueryable<Sys_Version>(s => s.IsDelete == null).OrderByDescending(s => s.DateCreate).Select(s => s.Value).FirstOrDefault();
                    var vesionLastNameFile = GetVesionLastNameFile();
                    if (!string.IsNullOrEmpty(vesionLastNameFile) && vesionLastName != vesionLastNameFile)
                    {
                        userInfoEntity.IsRunScriptNew = true;
                        userInfoEntity.VesionLastName = vesionLastName;
                    }
                    else
                    {
                        return null;
                    }
                }

                #endregion


                return userInfoEntity;
            }
        }

        string GetVesionLastNameFile()
        {
            try
            {
                var di = new DirectoryInfo(@Common.ScriptPath);
                var vesionLastNameFile = di.GetFiles().OrderByDescending(s => s.Name).Select(s => s.Name).FirstOrDefault();
                return Path.GetFileNameWithoutExtension(vesionLastNameFile);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>Kiểm tra googleEmail co tồn tại trong hệ thống VNR chưa</summary>
        /// <param name="googleEmail">Google Email</param>
        /// <param name="googleProfileName">ProfileName của google</param>
        /// <returns></returns>
        public Sys_UserInfoEntity CheckEmailExisted(string googleEmail, string googleProfileName)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_UserRepository(unitOfWork);
                var userInfo = unitOfWork.CreateQueryable<Sys_UserInfo>(Guid.Empty, m => m.UserLogin == googleEmail).FirstOrDefault();
                var userInfoEntity = userInfo.CopyData<Sys_UserInfoEntity>();
                if (userInfo == null && !string.IsNullOrEmpty(googleEmail))
                {
                    //neu chua co googleEmail => tao moi user
                    Sys_UserInfo usr = new Sys_UserInfo
                    {
                        Email = googleEmail,
                        FullName = googleProfileName,
                        UserLogin = googleEmail,
                        UserInfoName = googleProfileName,
                        Password = "123",
                        IsActivate = true
                    };
                    repo.Add(usr);
                    repo.SaveChanges();
                    userInfoEntity = usr.CopyData<Sys_UserInfoEntity>();
                }
                else
                {
                    return userInfoEntity;
                }
                return userInfoEntity;
            }
        }


        public List<Cat_MasterDataGroupMultiEntity> GetMultiCatTableForDashBoard(Guid userID)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);

                var masterDataGroups = unitOfWork.CreateQueryable<Sys_UserMasterData>(d =>
                    d.UserID == userID).Select(d => d.MasterDataGroups).ToList();

                var listMasterDataGroupNumber = masterDataGroups.Where(d =>
                    d != null).SelectMany(d => d.ToNumbers()).ToList();

                var lstMasterDataGroupMulti = unitOfWork.CreateQueryable<Cat_MasterDataGroup>(d => d.OrderNumber.HasValue &&
                     listMasterDataGroupNumber.Contains(d.OrderNumber.Value)).Select(d => new Cat_MasterDataGroupMultiEntity
                     {
                         ID = d.ID,
                         MasterDataGroupName = d.MasterDataGroupName
                     }).ToList();

                return lstMasterDataGroupMulti;
            }
        }

        public void SetUserMasterDataGroup(Guid userID, List<Guid> listMasterDataGroupID)
        {
            if (UnitOfWork.UserMasterDataGroup != null)
            {
                if (!UnitOfWork.UserMasterDataGroup.ContainsKey(userID))
                {
                    UnitOfWork.UserMasterDataGroup.Add(userID, listMasterDataGroupID);
                }
                else
                {
                    UnitOfWork.UserMasterDataGroup[userID] = listMasterDataGroupID;
                }
            }
        }

        public List<Sys_UserInfoEntity> ResourceUserApproved(Guid orgEmp, string type)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);

                List<object> listObj = new List<object>();
                listObj.Add(null);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);

                string status = string.Empty;
                //var dataProfile = GetData<Hre_Profile>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
                var dataUA = repoSys_UserApprove.FindBy(s => s.IsDelete == null && s.Type == type).ToList();

                // lấy UserInfo của Người Duyệt
                var lstUserInfo = dataUA.Select(s => s.UserApproveID).Distinct().ToList();
                // Lấy guid Nhân Viên từ UserInfo
                var lstUser = repoSys_UserInfo.FindBy(s => lstUserInfo.Contains(s.ID)).ToList();
                //var lstProGuid = lstUser.Select(s => s.ProfileID).Distinct().ToList();
                var dataCat_OrgStructure = repoCat_OrgStructure.FindBy(s => s.OrderNumber != null).ToList();

                List<Sys_UserInfoEntity> lstReturn = new List<Sys_UserInfoEntity>();
                foreach (var item in dataUA)
                {
                    var user = lstUser.Where(d => d.ID == item.UserApproveID).FirstOrDefault();
                    //var idUser = lstUserInfo.Where(s => s == item.UserApproveID).FirstOrDefault();
                    //var idOrg = lstUser.Where(s => s.ID == idUser).Select(s => s.OrgStructureID).FirstOrDefault();

                    if (item.OrgStructureID != null && IsParentOrgStructure(orgEmp, item.OrgStructureID.Value, dataCat_OrgStructure))
                    {
                        Sys_UserInfoEntity entity = new Sys_UserInfoEntity()
                        {
                            ID = item.UserApproveID,
                            UserInfoName = user.UserInfoName,
                            UserLogin = user.UserLogin

                        };
                        lstReturn.Add(entity);
                    }
                }
                return lstReturn;
            }
        }

        public bool IsParentOrgStructure(Guid idChild, Guid idParent, List<Cat_OrgStructure> lstSource)
        {
            if (idChild == idParent)
                return true;
            var parentOfChild = lstSource.Where(s => s.ID == idChild).Select(s => s.ParentID).ToList();
            if (parentOfChild.Contains(idParent))
                return true;

            while (parentOfChild.Count > 0)
            {
                parentOfChild = lstSource.Where(s => parentOfChild.Contains(s.ID)).Select(s => s.ParentID).ToList();
                if (parentOfChild.Contains(idParent))
                    return true;
            }
            return false;
        }

        /// <summary> Thay đổi mật khẩu </summary>
        /// <param name="id">id của User</param>
        /// <param name="oldPassword">Mật Khẩu Cũ</param>
        /// <param name="newPassword">Mật Khẩu Mới</param>
        /// <returns></returns>
        public bool ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            var success = true;
            var status = string.Empty;
            var passwordEncrypt = string.Empty;

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoUserInfo = new Sys_UserRepository(unitOfWork);
                if (id != Guid.Empty)
                {
                    var userInfo = repoUserInfo.GetById(id);

                    #region change password

                    if (userInfo != null)
                    {
                        //kt password
                        if (EncryptUtil.MD5(oldPassword) != userInfo.Password)
                        {
                            return false;
                        }

                        // neu password nhap vao co su thay doi => change password cho user
                        if (newPassword != userInfo.Password)
                        {
                            #region Change Password
                            //ma hoa password
                            if (!string.IsNullOrEmpty(newPassword))
                            {
                                passwordEncrypt = EncryptUtil.MD5(newPassword);//mã hoá giống bản 7
                            }
                            userInfo.Password = passwordEncrypt;
                            #endregion

                            repoUserInfo.Edit(userInfo);
                            var result = repoUserInfo.SaveChanges();
                            if (result != DataErrorCode.Success)
                            {
                                success = false;
                            }
                        }
                    }
                    #endregion
                }
            }
            return success;
        }
        #region Reset Password

        /// <summary>Reset Password (random 8 ki tu) theo ds User</summary>
        /// <param name="userIDs">Ds User</param>
        /// <returns></returns>
        public List<UserPasswordResetEntity> ResetPassword(List<Guid> userIDs)
        {
            var success = true;
            var status = string.Empty;
            var passwordEncrypt = string.Empty;
            var lstUserPassResets = new List<UserPasswordResetEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoUserInfo = new Sys_UserRepository(unitOfWork);
                if (userIDs.Any())
                {
                    //lay ds users
                    var lstUserInfo = new List<Sys_UserInfo>();
                    var users = unitOfWork.CreateQueryable<Sys_UserInfo>(Guid.Empty, m => userIDs.Contains(m.ID)).ToList();
                    var newPasswordReset = string.Empty;
                    foreach (var usr in users)
                    {
                        #region Change Password
                        //tu sinh password
                        newPasswordReset = GetRandomString();

                        //ma hoa password
                        if (!string.IsNullOrEmpty(newPasswordReset))
                        {
                            passwordEncrypt = EncryptUtil.MD5(newPasswordReset);//mã hoá giống bản 7
                        }
                        usr.Password = passwordEncrypt;
                        if (!string.IsNullOrEmpty(usr.Email))
                        {
                            var userPassReset = new UserPasswordResetEntity
                            {
                                Email = usr.Email,
                                Password = newPasswordReset,
                                UserInfoName = usr.UserInfoName,
                                UserLogin = usr.UserLogin,
                            };
                            lstUserPassResets.Add(userPassReset);

                            usr.DateChangePasssword = null;
                            lstUserInfo.Add(usr);
                        }
                        #endregion
                    }

                    if (lstUserInfo.Any())
                    {
                        repoUserInfo.Edit(lstUserInfo);
                        var result = repoUserInfo.SaveChanges();
                        if (result != DataErrorCode.Success)
                        {
                            return new List<UserPasswordResetEntity>();
                        }
                        else
                        {
                            ResetPasswordResponseEmail(lstUserPassResets);
                        }
                    }
                }
            }
            return lstUserPassResets;
        }

        /// <summary>Send Mail (reset password)</summary>
        /// <param name="lstUserPasswordResets"></param>
        /// <returns></returns>
        public bool ResetPasswordResponseEmail(List<UserPasswordResetEntity> lstUserPasswordResets)
        {
            var isSuccess = false;
            #region template
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string _typeTemplate = EnumDropDown.EmailType.E_NOTIFYRESETPASSWORD.ToString();
                var template = unitOfWork.CreateQueryable<Sys_TemplateSendMail>(Guid.Empty, m => m.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                {
                    return false;
                }
                else
                {

                    BaseService _base = new BaseService();
                    foreach (var user in lstUserPasswordResets)
                    {
                        string title = template.Subject;
                        var name = user.Email.Split('@');

                        #region magreData

                        string[] strsParaKey = null;
                        string[] strsParaValues = null;
                        strsParaKey = new string[] 
                        { 
                            EnumDropDown.EmailTypeDetails.E_Password.ToString(), 
                            EnumDropDown.EmailTypeDetails.E_UserInfoName.ToString(), 
                            EnumDropDown.EmailTypeDetails.E_UserLogin.ToString(), 
                        };
                        strsParaValues = new string[] 
                        { 
                            user.Password,
                            user.UserInfoName,
                            user.UserLogin                               
                        };
                        string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                        isSuccess = _base.SendMail(title, user.Email, body, string.Empty);
                        #endregion
                    }
                }
            }
            #endregion
            return isSuccess;
        }

        /// <summary>Tự sinh password(8 ki tu va số)</summary>
        /// <returns></returns>
        private string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            if (path.Length > 4)
            {
                path = path.Substring(0, 8);
            }
            return path;
        }


        #endregion
        public void UpdateUserMasterDataGroup(Guid userID, List<Guid?> listMasterDataGroupID)
        {
            if (userID != Guid.Empty && listMasterDataGroupID != null)
            {
                using (var context = new VnrHrmDataContext())
                {
                    IUnitOfWork unitOfWork = new UnitOfWork(context);

                    var listMasterDataGroupNumber = unitOfWork.CreateQueryable<Cat_MasterDataGroup>(d => d.OrderNumber.HasValue
                        && listMasterDataGroupID.Contains(d.ID)).Select(d => d.OrderNumber.Value).ToList();

                    var userMasterDataGroup = unitOfWork.CreateQueryable<Sys_UserMasterData>(d =>
                        d.UserID == userID).FirstOrDefault();

                    if (userMasterDataGroup == null)
                    {
                        userMasterDataGroup = new Sys_UserMasterData
                        {
                            UserID = userID
                        };

                        unitOfWork.AddObject(typeof(Sys_UserMasterData), userMasterDataGroup);
                    }

                    userMasterDataGroup.MasterDataGroups = listMasterDataGroupNumber.ToBinary();
                    userMasterDataGroup.MasterDataGroup = string.Join(",", listMasterDataGroupNumber);
                    unitOfWork.SaveChanges(userID);
                }
            }

        }

        public Hre_ProfileEntity GetProfileName(Guid userID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var profile = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty, m => m.ID == userID).Select(p => new Hre_ProfileEntity { ProfileName = p.ProfileName, ID = p.ID }).FirstOrDefault();
                return profile;
            }
        }

    }
}
