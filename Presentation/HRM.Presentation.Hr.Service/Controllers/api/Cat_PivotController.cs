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
    public class Cat_PivotController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu để Xuất Dữ Liệu(Cat_Export) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_PivotModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_PivotModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_PivotEntity>(id, ConstantSql.hrm_cat_sp_get_PivotById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_PivotModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Xuất Dữ Liệu(Cat_Export) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_PivotModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_PivotEntity, Cat_PivotModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatExport")]
        public Cat_PivotModel Post([Bind]Cat_PivotModel model)
        {
            //#region Validate
            //string message = NotificationType.Success.ToString();
            //var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_PivotModel>(model, "Cat_Pivot", ref message);
            //if (!checkValidate)
            //{
            //    model.ActionStatus = message;
            //    return model;
            //}
            //#endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_PivotEntity, Cat_PivotModel>(model);
        }

	}

}