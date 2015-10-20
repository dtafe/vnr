using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatCurrencyController : ApiController
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

        #region HRM8.20/7/2014
        
        #endregion
        /// <summary>
        /// [Tin.Nguyen] - Lấy dữ liệu Loại Thiết Bị(Cat_Currency) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatCurrencyModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatCurrencyModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_CurrencyEntity>(id,ConstantSql.hrm_cat_sp_get_CurrencyById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatCurrencyModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Loại Thiết Bị(Cat_Currency) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatCurrencyModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_CurrencyEntity, CatCurrencyModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Loại Thiết Bị(Currency)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Currency")]
        public CatCurrencyModel Post([Bind]CatCurrencyModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatCurrencyModel>(model, "Cat_Currency", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_CurrencyEntity, CatCurrencyModel>(model);
        }
    }
}