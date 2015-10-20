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
    public class CatLateEarlyRuleController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu Lý Do Sớm Muộn(Cat_LateEarlyRule) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatLateEarlyRuleModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatLateEarlyRuleModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_LateEarlyRuleEntity>(id,ConstantSql.hrm_cat_sp_get_LateEarlyRuleById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatLateEarlyRuleModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của LoạiLý Do Sớm Muộn(CatLateEarlyRule) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatLateEarlyRuleModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_LateEarlyRuleEntity, CatLateEarlyRuleModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa Lý Do Sớm Muộn(Cat_LateEarlyRule)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatLateEarlyRule")]
        public CatLateEarlyRuleModel Post([Bind]CatLateEarlyRuleModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_LateEarlyRuleEntity, CatLateEarlyRuleModel>(model);
        }

    }


}