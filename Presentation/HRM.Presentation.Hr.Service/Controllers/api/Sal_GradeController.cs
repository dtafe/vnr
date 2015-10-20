﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Payroll.Domain;
using HRM.Presentation.Payroll.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;

namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_GradeController : ApiController
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
        /// <summary>
        /// [Tho.Bui] - Lấy dữ liệu Quốc Gia(Cat_Country) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_GradeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_GradeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_GradeEntity>(id, ConstantSql.hrm_sal_sp_get_GradeById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_GradeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tho.Bui] - Xóa hoặc chuyển đổi trạng thái Quốc Gia(Cat_Country) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_GradeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_GradeEntity, Sal_GradeModel>(id);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_Grade")]
        public Sal_GradeModel Post([Bind]Sal_GradeModel model)
        {
            #region Validate
            ActionService service = new ActionService(UserLogin);
            string message = string.Empty;
            var checkValidate = false;
            if (model.IsProfileNotGrade)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_GradeModel>(model, "Sal_Grade", ref message);
            }
            else if (model.IsCreateByProfile)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_GradeModel>(model,"Sal_GradeByProfile", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
                return service.UpdateOrCreate<Sal_GradeEntity, Sal_GradeModel>(model);
            }
            else
            {
                if (model.OrgStructureID != null && model.OrgStructureID != string.Empty)
                {
                    checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_GradeModel>(model, "Sal_GradeOrg", ref message);
                }
                else
                {
                    checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_GradeModel>(model, "Sal_Grade", ref message);
                }
            }
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion


            string status = string.Empty;
            //string message = string.Empty;
            var hrService = new Hre_ProfileServices();
            var gradeServices = new Sal_GradeServices();
            if (!string.IsNullOrEmpty(model.OrgStructureID))
            {
                List<Guid> listGuid = new List<Guid>();
                if (model.ProfileIDsExclude != null)
                {
                    var listStr = model.ProfileIDsExclude.Split(',');

                    if (listStr[0] != "")
                    {

                        foreach (var item in listStr)
                        {
                            listGuid.Add(Guid.Parse(item));
                        }
                    }
                }
                List<object> listObj = new List<object>();
                listObj.Add(model.OrgStructureID);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                var lstProfile = hrService.GetData<Hre_ProfileIdEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure, userLogin, ref status).Select(s => s.ID).ToList();
                if (listGuid != null)
                {
                    lstProfile = lstProfile.Where(s => !listGuid.Contains(s)).ToList();
                }
                if (lstProfile.Count == 0 && model.ProfileID == Guid.Empty)
                {
                    model.ActionStatus = ConstantDisplay.HRM_Common_NotEmployee.TranslateString();
                    return model;
                }

                List<Sal_GradeEntity> lstGradeEntity = new List<Sal_GradeEntity>();

                foreach (var item in lstProfile)
                {
                    Sal_GradeEntity gradeEntity = new Sal_GradeEntity
                    {
                        ProfileID = item,
                        DateCreate = model.DateCreate,
                        DateLock = model.DateLock,
                        GradeCfgName = model.GradeCfgName,
                        GradePayrollID = model.GradePayrollID,
                        ID = model.ID,
                        MonthEnd = model.MonthEnd,
                        MonthStart = model.MonthStart,
                        IsDelete = model.IsDelete,
                        UserUpdate = model.UserCreate,
                        UserCreate = model.UserCreate,
                    };
                    //model.ActionStatus =  gradeServices.Add(gradeEntity);
                    lstGradeEntity.Add(gradeEntity);
                }
                model.ActionStatus = gradeServices.Add(lstGradeEntity);
                return model;
            }
            if (model.ProfileIDs != null && model.ProfileIDs !=string.Empty)
            {
                var listStr = model.ProfileIDs.Split(',');
                List<Guid> listGuid = new List<Guid>();
                if (listStr[0] != "")
                {
                    foreach (var item in listStr)
                    {
                        listGuid.Add(Guid.Parse(item));
                    }
                }
                else
                {
                    model.ActionStatus = ConstantDisplay.HRM_Common_NotEmployee.TranslateString();
                    return model;
                }
                List<Sal_GradeEntity> lstGradeEntity = new List<Sal_GradeEntity>();
                foreach (var item in listGuid)
                {
                    Sal_GradeEntity gradeEntity = new Sal_GradeEntity
                    {
                        ProfileID = item,
                        DateCreate = model.DateCreate,
                        DateLock = model.DateLock,
                        GradeCfgName = model.GradeCfgName,
                        GradePayrollID = model.GradePayrollID,
                        ID = model.ID,
                        MonthEnd = model.MonthEnd,
                        MonthStart = model.MonthStart,
                        IsDelete = model.IsDelete,
                        UserUpdate = model.UserCreate,
                        UserCreate = model.UserCreate,
                    };
                    //model.ActionStatus =  gradeServices.Add(gradeEntity);
                    lstGradeEntity.Add(gradeEntity);
                }
                model.ActionStatus = gradeServices.Add(lstGradeEntity);
                return model;
            }
            if (model.ProfileID == Guid.Empty)
            {
                model.ActionStatus = ConstantDisplay.HRM_Common_NotEmployee.TranslateString();
                return model;
            }
            return service.UpdateOrCreate<Sal_GradeEntity, Sal_GradeModel>(model);
        }
    }
}
