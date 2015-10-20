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
    public class Cat_YouthUnionPositionController : ApiController
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
        /// [Kiet.Chung] - Lấy dữ liệu WorkPlace(Cat_YouthUnionPosition) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_YouthUnionPositionModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_YouthUnionPositionModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_YouthUnionPositionEntity>(id, ConstantSql.hrm_cat_sp_get_YouthUnionPositionById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_YouthUnionPositionModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Kiet.Chung] - Xóa hoặc chuyển đổi trạng thái của WorkPlace(Cat_YouthUnionPosition) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_YouthUnionPositionModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_YouthUnionPositionEntity, Cat_YouthUnionPositionModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một WorkPlace(Cat_WorkPlace)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_YouthUnionPosition")]
        public Cat_YouthUnionPositionModel Post([Bind]Cat_YouthUnionPositionModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_YouthUnionPositionModel>(model, "Cat_YouthUnionPosition", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_YouthUnionPositionEntity, Cat_YouthUnionPositionModel>(model);
        }
    }
}