using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using System.Collections.Generic;
using HRM.Business.Evaluation.Models;
using HRM.Presentation.Evaluation.Models;
using System.Linq;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_NameEntityController : ApiController
    {
        #region UserLogin
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
        /// [Chuc.Nguyen] - Lấy dữ liệu Ngân Hàng(Cat_Bank) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_NameEntityModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_NameEntityModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_NameEntityEntity>(id, ConstantSql.hrm_cat_sp_get_NameEntityById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_NameEntityModel>();
                model.NameEntityID = model.ID;
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Cat_Bank) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_NameEntityModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_NameEntityEntity, Cat_NameEntityModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Cat_Bank)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_NameEntity")]
        public Cat_NameEntityModel Post([Bind]Cat_NameEntityModel model)
        {
            if (model != null && model.NameEntityType == EnumDropDown.EntityType.E_VEHICLE.ToString())
            {
                string message = string.Empty;
                var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_NameEntityModel>(model, "Cat_NameEntityVehicle", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
            }
            else if (model != null && model.NameEntityType == EnumDropDown.EntityType.E_COSTSOURCE.ToString())
            {
                string message = string.Empty;
                var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_NameEntityModel>(model,"Cat_NameEntityCOSTSOURCE", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }       
            }
            else if (model != null && model.NameEntityType == EnumDropDown.EntityType.E_CountAnalyzeHoldSalary.ToString())
            {
                string message = string.Empty;
                var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_NameEntityModel>(model, "Sal_CountAnalyzeHoldSalary", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }    
            }
            else
            {
                #region Validate
                string namevalidate = "Cat_NameEntity";
                if (model.NameEntityType == "E_DISCIPLINE_REASON")
                {
                    namevalidate = "Cat_NameEntity_DisReason";
                }
                //  model.Code = "null";
                string message = string.Empty;
                var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_NameEntityModel>(model, namevalidate, ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
                #endregion
            }
            


            #region Xu ly nut them
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            if (model != null && model.NameEntityType == EnumDropDown.EntityType.E_VEHICLE.ToString())
            {
                model.NameEntityName = model.VehicleName;
                return service.UpdateOrCreate<Cat_NameEntityEntity, Cat_NameEntityModel>(model);
            }
            if (model != null && model.NameEntityType == EnumDropDown.EntityType.E_COSTSOURCE.ToString())
            {
                model.NameEntityName = model.CostSourceName;
                return service.UpdateOrCreate<Cat_NameEntityEntity, Cat_NameEntityModel>(model);
            }
            if (model != null && model.NameEntityType == EnumDropDown.EntityType.E_CountAnalyzeHoldSalary.ToString())
            {
                model.NameEntityName = model.CountAnalyzeHoldSalary;
                return service.UpdateOrCreate<Cat_NameEntityEntity, Cat_NameEntityModel>(model);
            }
            if (model != null && model.IsAddKPI)
            {
                if (model.ID == Guid.Empty)
                {
                    model.NameEntityType = "E_CATEGORY_KPI";
                    model = service.UpdateOrCreate<Cat_NameEntityEntity, Cat_NameEntityModel>(model);
                    model.NameEntityID = model.ID;
                }
                string[] arrKPIIDS = model.Eva_KPIIds.Split(',');
                foreach (string strKPIID in arrKPIIDS)
                {
                    Guid KPIGuidID = Guid.Parse(strKPIID);
                    var entity = service.GetByIdUseStore<Eva_KPIEntity>(KPIGuidID, ConstantSql.hrm_eva_sp_get_KPIById, ref status);
                    if (entity != null)
                    {
                        var modelKPI = entity.CopyData<Eva_KPIModel>();
                        modelKPI.ID = Guid.Empty;
                        modelKPI.NameEntityID = model.ID;
                        modelKPI = service.UpdateOrCreate<Eva_KPIEntity, Eva_KPIModel>(modelKPI);
                        model.ActionStatus = modelKPI.ActionStatus;
                    }
                }
                model.ActionStatus = model.ActionStatus.TranslateString();
                return model;
            }
            else
            {
                //model.NameEntityType = "E_CATEGORY_KPI";
                model = service.UpdateOrCreate<Cat_NameEntityEntity, Cat_NameEntityModel>(model);
                model.NameEntityID = model.ID;
                //model.ActionStatus = model.ActionStatus.TranslateString();
                return model;
            }
            #endregion
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_NameEntity")]
        public String Put([Bind]Cat_NameEntityModel model)
        {
            //model.Eva_KPIIds = Common.DotNetToOracle(model.Eva_KPIIds);
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            string[] arrKPIIDS = model.Eva_KPIIds.Split(',');
            foreach (string strKPIID in arrKPIIDS)
            {
                Guid KPIGuidID = Guid.Parse(strKPIID);
                var entity = service.GetByIdUseStore<Eva_KPIEntity>(KPIGuidID, ConstantSql.hrm_eva_sp_get_KPIById, ref status);
                if (entity != null)
                {
                    var modelKPI = entity.CopyData<Eva_KPIModel>();
                    modelKPI.ID = Guid.Empty;
                    modelKPI.NameEntityID = model.ID;
                    service.UpdateOrCreate<Eva_KPIEntity, Eva_KPIModel>(modelKPI);
                }
            }
            return status;
            //List<object> lstObj = new List<object>();
            //lstObj.Add(model.ProfileIds);
            //lstObj.Add(model.Status);
            //var rs = service.UpdateData<Att_LeaveDayModel>(lstObj, ConstantSql.hrm_att_sp_get_Leaveday_UpdateStatus, ref status);
            //if (rs != null)
            //{
            //    return model;
            //}
            //return model;
        }
    }
}
