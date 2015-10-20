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
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_DisciplinedTypesController : ApiController
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
        /// [Tam.Le] - Lấy dữ liệu RelativeType theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_DisciplinedTypesModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_DisciplinedTypesModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_DisciplinedTypesEntity>(id, ConstantSql.hrm_cat_sp_get_DisciplinedTypesById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_DisciplinedTypesModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tam.Le] - Xóa hoặc chuyển đổi trạng thái của RelativeType sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_DisciplinedTypesModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_DisciplinedTypesEntity, Cat_DisciplinedTypesModel>(id);
            return result;
        }

        /// <summary>
        /// [Tam.Le] - Xử lý thêm mới hoặc chỉnh sửa một RelativeType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatRelativeType")]
        public Cat_DisciplinedTypesModel Post([Bind]Cat_DisciplinedTypesModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_DisciplinedTypesModel>(model, "Cat_DisciplinedTypes", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_DisciplinedTypesEntity, Cat_DisciplinedTypesModel>(model);
        }
        

    }
}
