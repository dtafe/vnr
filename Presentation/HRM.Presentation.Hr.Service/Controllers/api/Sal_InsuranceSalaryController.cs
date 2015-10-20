using System.Collections.Generic;
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
    public class Sal_InsuranceSalaryController : ApiController
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
        public Sal_InsuranceSalaryModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_InsuranceSalaryModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_InsuranceSalaryEntity>(id, ConstantSql.hrm_sal_sp_get_InsuranceSalaryById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_InsuranceSalaryModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tho.Bui] - Xóa hoặc chuyển đổi trạng thái Quốc Gia(Cat_Country) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_InsuranceSalaryModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_InsuranceSalaryEntity, Sal_InsuranceSalaryModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Quốc Gia(Cat_Country)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_InsuranceSalary")]
        public Sal_InsuranceSalaryModel Post([Bind]Sal_InsuranceSalaryModel model)
        {
            #region Validate

            ActionService service = new ActionService(UserLogin);
            string message = string.Empty;
            bool checkValidate = true;
            if (model.IsCreateByProfile != null && model.IsCreateByProfile == true)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_InsuranceSalaryModel>(model, "Sal_InsuranceSalaryByProfile", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
                return service.UpdateOrCreate<Sal_InsuranceSalaryEntity, Sal_InsuranceSalaryModel>(model);
            }

            //var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_InsuranceSalaryModel>(model, "Sal_InsuranceSalary", ref message);
            if (model.OrgStructureID != null && model.OrgStructureID != string.Empty)
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_InsuranceSalaryModel>(model, "Sal_InsuranceSalaryOrgStructure", ref message);
            }
            else
            {
                checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_InsuranceSalaryModel>(model, "Sal_InsuranceSalary", ref message);
            }
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            string status = string.Empty;

            var hrService = new Hre_ProfileServices();
            var insuranceServices = new Sal_InsuranceSalaryServices();
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

                List<Sal_InsuranceSalaryEntity> lstGradeEntity = new List<Sal_InsuranceSalaryEntity>();

                foreach (var item in lstProfile)
                {
                    Sal_InsuranceSalaryEntity gradeEntity = new Sal_InsuranceSalaryEntity
                    {
                        ProfileID = item,
                        DateCreate = model.DateCreate,
                        DateLock = model.DateLock,
                        CurrencyID = model.CurrencyID,
                        InsuranceAmount = model.InsuranceAmount,
                        ID = model.ID,
                        DateEffect = model.DateEffect,
                        IsSocialIns = model.IsSocialIns,
                        IsMedicalIns = model.IsMedicalIns,
                        IsUnimploymentIns = model.IsUnimploymentIns,
                        IsDelete = model.IsDelete,
                        UserUpdate = model.UserCreate,
                        UserCreate = model.UserCreate,
                        DecisionNo = model.DecisionNo
                    };
                    //model.ActionStatus =  gradeServices.Add(gradeEntity);
                    lstGradeEntity.Add(gradeEntity);
                }
                model.ActionStatus = insuranceServices.Add(lstGradeEntity);
                return model;
            }
            if (!string.IsNullOrEmpty(model.ProfileIDs))
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
                List<Sal_InsuranceSalaryEntity> lstGradeEntity = new List<Sal_InsuranceSalaryEntity>();
                foreach (var item in listGuid)
                {
                    Sal_InsuranceSalaryEntity gradeEntity = new Sal_InsuranceSalaryEntity
                    {
                        ProfileID = item,
                        DateCreate = model.DateCreate,
                        DateLock = model.DateLock,
                        CurrencyID = model.CurrencyID,
                        InsuranceAmount = model.InsuranceAmount,
                        ID = model.ID,
                        DateEffect = model.DateEffect,
                        IsSocialIns = model.IsSocialIns,
                        IsMedicalIns = model.IsMedicalIns,
                        IsUnimploymentIns = model.IsUnimploymentIns,
                        IsDelete = model.IsDelete,
                        UserUpdate = model.UserCreate,
                        UserCreate = model.UserCreate,
                        DecisionNo = model.DecisionNo
                    };
                    //model.ActionStatus =  gradeServices.Add(gradeEntity);
                    lstGradeEntity.Add(gradeEntity);
                }
                model.ActionStatus = insuranceServices.Add(lstGradeEntity);
                return model;
            }

            return service.UpdateOrCreate<Sal_InsuranceSalaryEntity, Sal_InsuranceSalaryModel>(model);
        }
    }
}
