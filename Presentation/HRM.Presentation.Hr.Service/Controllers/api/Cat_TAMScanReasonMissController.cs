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
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_TAMScanReasonMissController : ApiController
    {
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
        /// <summary>
        /// Get All Cat_TamScan for Multi Control
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Cat_TAMScanReasonMissMuitlModel> Get()
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<Cat_TAMScanReasonMissMuitlModel>(string.Empty, ConstantSql.hrm_cat_sp_get_TamScanReasonMiss_multi, UserLogin, ref status);

            if (listEntity != null)
            {
                var listModel = listEntity.Translate<Cat_TAMScanReasonMissMuitlModel>();
                return listModel;
            }

            return new List<Cat_TAMScanReasonMissMuitlModel>();
        }


        /// <summary>
        /// [Quan.Nguyen] - Lấy dữ liệu Cat_TAMScanReasonMiss(Cat_TAMScanReasonMiss) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_TAMScanReasonMissModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_TAMScanReasonMissModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Cat_TAMScanReasonMissEntity>(id, ConstantSql.hrm_cat_sp_get_TAMScanReasonMiss_ById, ref status).FirstOrDefault(); 
            if (entity != null)
            {
                model = entity.CopyData<Cat_TAMScanReasonMissModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quan.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Cat_TAMScanReasonMiss(Cat_TAMScanReasonMiss) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_TAMScanReasonMissModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_TAMScanReasonMissEntity, Cat_TAMScanReasonMissModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một ResignReason(Cat_ResignReason)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_TAMScanReasonMiss")]
        public Cat_TAMScanReasonMissModel Post([Bind]Cat_TAMScanReasonMissModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_TAMScanReasonMissModel>(model, "Cat_TAMScanReasonMiss", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            if(model.ID == null){
                model.IsForCMS = true;
            }
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_TAMScanReasonMissEntity, Cat_TAMScanReasonMissModel>(model);
        }

    }

}