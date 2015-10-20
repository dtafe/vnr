using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatContractTypeController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu ContractType(Cat_ContractType) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatContractTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatContractTypeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ContractTypeEntity>(id, ConstantSql.hrm_cat_sp_get_ContractTypeById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatContractTypeModel>();
                if(model.NoneEndContract == false)
                {
                    model.ComputeEndDateByFomular = true;
                }
                if (model.NoneTypeInsuarance == false)
                {
                    model.NoneTypeInsuaranceAdvance = true;
                }
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của ContractType(Cat_ContractType) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatContractTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ContractTypeEntity, CatContractTypeModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một ContractType(Cat_ContractType)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatContractType")]
        public CatContractTypeModel Post([Bind]CatContractTypeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatContractTypeModel>(model, "Cat_ContractType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            model.ContractNextID = model.ContractNextID != null ? model.ContractNextID = Common.DotNetToOracle(model.ContractNextID) : string.Empty; 
            
            return service.UpdateOrCreate<Cat_ContractTypeEntity, CatContractTypeModel>(model);
        }
    }
}