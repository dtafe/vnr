using System.Web.Mvc;
using Antlr.Runtime.Misc;
using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using HRM.Presentation.Service;
using VnResource.Helper.Data;
using HRM.Business.Category.Models;
using System;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatImportItemController : ApiController
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
        /// Xử lí eidt và add new truyền theo script
        /// </summary>
        /// <param name="Leaveday"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatImportItem")]
        public CatImportItemModel Post([Bind]CatImportItemModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ImportItemEntity, CatImportItemModel>(model);
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatImportItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ImportItemEntity, CatImportItemModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Lấy dữ liệu  theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatImportItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatImportItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ImportItemEntity>(id, ConstantSql.hrm_cat_sp_get_ImportItemById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatImportItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }
    }
}
        
