using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{

    public class Sys_UserController : ApiController
    {
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    userLogin = headerValues.FirstOrDefault();
                }
                return userLogin;
            }
        }
        #endregion
        string status = string.Empty;

        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        public IEnumerable<Sys_UserMultiModel> Get()
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Sys_UserMultiModel>(string.Empty, ConstantSql.hrm_sys_sp_get_user_multi, UserLogin, ref status);

            if (listEntity != null)
            {
                var listModel = listEntity.Translate<Sys_UserMultiModel>();
                return listModel;
            }

            return new List<Sys_UserMultiModel>();
        }

        // GET api/<controller>/5
        public Sys_UserModel Get(Guid id)
        {
            var profileName = string.Empty;
            var service = new Sys_UserServices();
            // var result = service.GetById<Sys_UserInfoEntity>(id, ref status);

            ActionService actionService = new ActionService(UserLogin);
            var result = actionService.GetByIdUseStore<Sys_UserInfoEntity>(id, ConstantSql.hrm_sys_sp_get_UserbyId, ref status);

          

            if (result == null)
            {
                return new Sys_UserModel();
            }

            #region Get ProfileName
            var userService = new Sys_UserServices();
            var profile = userService.GetProfileName(result.ProfileID ?? Guid.Empty);
            var profileID = Guid.Empty;
            if (profile != null)
            {
                profileName = profile.ProfileName;
                profileID = profile.ID;
            }
            #endregion

            var Sys_UserEntity = new Sys_UserModel
            {
                ID = result.ID,
                Code = result.Code,
                UserType = result.UserType,
                UserInfoName = result.UserInfoName,
                UserInfoName1 = result.UserInfoName,
                UserLogin = result.UserLogin,
                FullName = result.FullName,
                Password = result.Password,
                Email = result.Email,
                EffectedDate = result.DateEnd,
                ProfileName = profileName,
                ProfileID = profileID,
                IsActivate = result.IsActivate,
                IsCheckLDAP = result.IsCheckLDAP,
                Description = result.Description,
                LDAPDatasource = result.LDAPDatasource,
            };

            #region Tự động load ds các dự án vào multiSelect control
            var sysUserService = new Sys_UserServices();
            var masterDataGroup = sysUserService.GetMultiCatTableForDashBoard(id);
            var dic = new Dictionary<Guid, string>();
            foreach (var item in masterDataGroup)
            {
                dic.Add(item.ID, item.MasterDataGroupName);
            }
            Sys_UserEntity.MasterDataGroups = dic; 
            #endregion

            return Sys_UserEntity;

        }

        public Sys_UserModel Put(Sys_UserModel model)
        {
            var Sys_UserEntity = new Sys_UserInfoEntity
            {
                ID = model.ID,
                Code = model.Code,
                UserType = model.UserType,
                UserInfoName = model.UserInfoName,
                UserLogin = model.UserLogin,
                Password = model.Password,
                IsActivate = model.IsActivate,
                IsCheckLDAP = model.IsCheckLDAP,
                LDAPDatasource = model.LDAPDatasource,
            };
            var service = new Sys_UserServices();
            if (model.ID != Guid.Empty)
            {
                Sys_UserEntity.ID = model.ID;
                service.Edit<Sys_UserInfoEntity>(Sys_UserEntity);
            }
            else
            {
                service.Add<Sys_UserInfoEntity>(Sys_UserEntity);
            }

            return model;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_UserInfoEntity")]
        public string Post([Bind]Sys_UserModel user)
        {
            #region chuyển string sang list<guid> (MasterDataGroup)
            if (user != null && !string.IsNullOrEmpty(user.MasterDataGroupIDstr))
            {
                var lstDataGroupIDstr = user.MasterDataGroupIDstr.Split(',');
                Guid dataGroupId = Guid.Empty;
                user.MasterDataGroupID = new List<Guid?>();
                foreach (var item in lstDataGroupIDstr)
                {
                    Guid.TryParse(item, out dataGroupId);
                    if (dataGroupId != Guid.Empty)
                    {
                        user.MasterDataGroupID.Add(dataGroupId);
                    }
                }
            }

            #endregion

            var success = string.Empty;
            var passwordEncrypt = string.Empty;

            #region Validate

            string message = string.Empty;
            var checkValidate = ValidatorService.OnValidateData<Sys_UserModel>(user, "Sys_UserInfo", ref message);
            if (!checkValidate)
            {
                user.ActionStatus = message;
                return message;
            }
            else
            {
                user.UserInfoName = user.UserInfoName1;
            }
            #endregion

            if (user == null)
            {
                return string.Empty;
            }

            var model = new Sys_UserInfoEntity
            {
                ID = user.ID,
                Code = user.Code,
                UserType = user.UserType,
                ProfileID = user.ProfileID,
                UserLogin = user.UserLogin,
                UserInfoName = user.UserInfoName,
                Email = user.Email,
                IsActivate = user.IsActivate,
                IsCheckLDAP = user.IsCheckLDAP,
                LDAPDatasource = user.LDAPDatasource,
                DateEnd = user.EffectedDate,
                DateStart = DateTime.Now,
                FullName = user.UserInfoName
            };

            var service = new Sys_UserServices();

            #region change password

            ActionService actionService = new ActionService(UserLogin);
            var userInfo = actionService.GetByIdUseStore<Sys_UserInfoEntity>(user.ID, ConstantSql.hrm_sys_sp_get_UserbyId, ref status);

            if (userInfo != null)
            {
                // neu password nhap vao co su thay doi => change password cho user
                if (user.Password != userInfo.Password)
                {
                    #region Change Password
                    //ma hoa password
                    if (user != null && !string.IsNullOrEmpty(user.Password))
                    {
                        passwordEncrypt = EncryptUtil.MD5(user.Password);//mã hoá giống bản 7
                    }
                    model.Password = passwordEncrypt;
                    #endregion
                }
                else
                {
                    //neu khong co su thay doi password => set giá tri password là password cua db vào model
                    model.Password = userInfo.Password;
                }
            }
            else
            {
                //tạo mới password cho user
                model.Password = passwordEncrypt;
            }
            #endregion

            if (user.ID != Guid.Empty)
            {
                model.ID = user.ID;
                success = service.Edit<Sys_UserInfoEntity>(model) + ",0";
            }
            else
            {
                success = service.Add<Sys_UserInfoEntity>(model) + ",1";
            }

            if (user != null && user.MasterDataGroupID != null)
            {
                service.UpdateUserMasterDataGroup(user.ID, user.MasterDataGroupID);
            }

            return success;
        }

        // DELETE api/<controller>/5
        public void Delete(Guid ID)
        {
            var service = new Sys_UserServices();
            var result = service.Remove<Sys_UserInfoEntity>(ID);
        }

    }
}