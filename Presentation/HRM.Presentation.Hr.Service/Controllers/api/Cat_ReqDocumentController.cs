using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_ReqDocumentController : ApiController
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
        /// [To.Le] - Lấy dữ liệu ReqDocument theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_ReqDocumentModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_ReqDocumentModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ReqDocumentEntity>(id, ConstantSql.hrm_cat_sp_get_ReqDocumentById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_ReqDocumentModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [To.Le] - Xóa hoặc chuyển đổi trạng thái của Religion sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_ReqDocumentModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ReqDocumentEntity, Cat_ReqDocumentModel>(id);
            return result;
        }

        /// <summary>
        /// [To.Le] - Xử lý thêm mới hoặc chỉnh sửa một ReqDocument
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_ReqDocument")]
        public Cat_ReqDocumentModel Post([Bind]Cat_ReqDocumentModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_ReqDocumentModel>(model, "Cat_ReqDocument", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ReqDocumentEntity, Cat_ReqDocumentModel>(model);
        }
	}
}