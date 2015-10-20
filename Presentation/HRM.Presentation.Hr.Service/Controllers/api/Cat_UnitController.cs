using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_UnitController : ApiController
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
        /// [T Lấy dữ liệu Thương Hiệu(Cat_Brand) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_UnitModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_UnitModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_UnitEntity>(id, ConstantSql.hrm_cat_sp_get_UnitById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_UnitModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        public void Delete(Guid ID)
        {
            var service = new Cat_BrandServices();
            var result = service.Remove<Cat_UnitEntity>(ID);
        }

        //public Cat_BrandModel DeleteOrRemove(string id)
        //{
        //    ActionService service = new ActionService(UserLogin);
        //    var result = service.DeleteOrRemove<Cat_BrandEntity, Cat_BrandModel>(id);
        //    return result;
        //}

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa thương hiệu(Cat_Brand)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_Unit")]
        public Cat_UnitModel Post([Bind]Cat_UnitModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_UnitModel>(model, "Cat_Unit", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_UnitEntity, Cat_UnitModel>(model);
        }
    }
}