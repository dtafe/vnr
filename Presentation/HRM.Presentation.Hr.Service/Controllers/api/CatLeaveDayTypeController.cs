using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System.Linq;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using System.Collections.Generic;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class CatLeaveDayTypeController : ApiController
    {
        #region MyRegion
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
        /// [Hien.Nguyen] Get All Cat_Shift for Multi Control
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CatLeaveDayTypeModel> Get()
        {
            BaseService service = new BaseService();
            string status = string.Empty;
            var listEntity = service.GetData<CatLeaveDayTypeModel>(string.Empty, ConstantSql.hrm_cat_sp_get_LeaveDayType_multi,UserLogin, ref status);

            if (listEntity != null)
            {
                var listModel = listEntity.Translate<CatLeaveDayTypeModel>();
                return listModel;
            }

            return new List<CatLeaveDayTypeModel>();
        }


        /// <summary>
        /// [Tin.Nguyen] - Lấy dữ liệu Loại Ngày Nghỉ(Cat_LeaveDayType) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatLeaveDayTypeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new CatLeaveDayTypeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_LeaveDayTypeEntity>(id,ConstantSql.hrm_cat_sp_get_LeaveDayTypeById ,ref status);
            if (entity != null)
            {
                model = entity.CopyData<CatLeaveDayTypeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Loại Thiết Bị(Cat_Currency) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatLeaveDayTypeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_LeaveDayTypeEntity, CatLeaveDayTypeModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Loại Thiết Bị(Currency)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/CatLeaveDayType")]
        public CatLeaveDayTypeModel Post([Bind]CatLeaveDayTypeModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatLeaveDayTypeModel>(model, "Cat_LeaveDayType", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_LeaveDayTypeEntity, CatLeaveDayTypeModel>(model);
        }

    }


}