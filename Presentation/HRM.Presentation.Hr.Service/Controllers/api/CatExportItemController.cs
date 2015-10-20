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
    public class CatExportItemController : ApiController
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
        /// [Tin.Nguyen] - Lấy dữ liệu Dân Tộc(Cat_ExportItem) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatExportItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatExportItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetById<Cat_ExportItemEntity>(id, ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatExportItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Dân Tộc(Cat_ExportItem) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatExportItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ExportItemEntity, CatExportItemModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa Dân Tộc(Cat_ExportItem)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatExportItem")]
        public CatExportItemModel Post([Bind]CatExportItemModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ExportItemEntity, CatExportItemModel>(model);
        }
	}

}