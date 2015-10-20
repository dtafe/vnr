using Antlr.Runtime.Misc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatWorkPlaceController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu WorkPlace(Cat_WorkPlace) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatWorkPlaceModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatWorkPlaceModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_WorkPlaceEntity>(id, ConstantSql.hrm_cat_sp_get_WorkPlaceById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatWorkPlaceModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của WorkPlace(Cat_workplace) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatWorkPlaceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_WorkPlaceEntity, CatWorkPlaceModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một WorkPlace(Cat_WorkPlace)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatWorkPlace")]
        public CatWorkPlaceModel Post([Bind]CatWorkPlaceModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatWorkPlaceModel>(model, "Cat_WorkPlace", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            #region Get max OrderNumber of WorkPlace
            if (model != null && model.ID == Guid.Empty)
            {
                Cat_WorkPlaceServices orgService = new Cat_WorkPlaceServices();
                model.OrderNumber = orgService.GetMaxWorkPlaceOrder();
            }
            #endregion


            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_WorkPlaceEntity, CatWorkPlaceModel>(model);
        }

    }


}