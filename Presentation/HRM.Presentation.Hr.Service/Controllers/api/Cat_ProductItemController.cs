using HRM.Business.Category.Domain;
using HRM.Presentation.Category.Models;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using System.Linq;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_ProductItemController : ApiController
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
        public Cat_ProductItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_ProductItemModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_ProductItemEntity>(id, ConstantSql.hrm_cat_sp_get_ProductItemById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_ProductItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>
        //public void Delete(Guid ID)
        //{
        //    var service = new Cat_PurchaseItemsServices();
        //    var result = service.Remove<Cat_ProductItemEntity>(ID);
        //}

        public Cat_ProductItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_ProductItemEntity, Cat_ProductItemModel>(id);
            return result;
        }

        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa thương hiệu(Cat_Brand)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_PurchaseItems")]
        public Cat_ProductItemModel Post([Bind]Cat_ProductItemModel model)
        {

            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_ProductItemModel>(model, "Cat_ProductItem", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_ProductItemEntity, Cat_ProductItemModel>(model);
        }
    }
}