using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using System.Linq;
using System.Collections.Generic;
using HRM.Business.Evaluation.Models;
using HRM.Presentation.Evaluation.Models;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_CutOffDurationTypeController : ApiController
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
            var entity = service.GetByIdUseStore<Cat_NameEntityEntity>(id, ConstantSql.hrm_cat_sp_get_CutOffDurationTypeById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_NameEntityModel>();
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
            model.NameEntityType = "E_CUTOFFDURATION_TYPE";
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_NameEntityEntity, Cat_NameEntityModel>(model);
        }
     
    }
}
