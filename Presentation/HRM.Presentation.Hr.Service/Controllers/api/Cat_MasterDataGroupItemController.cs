using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;
using System.Collections.Generic;
using HRM.Business.Category.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_MasterDataGroupItemController : ApiController
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
        /// <summary>Lấy dữ liệuMasterDataGroupItem</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_MasterDataGroupItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_MasterDataGroupItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_MasterDataGroupItemEntity>(id, ConstantSql.hrm_cat_sp_get_MasterDataGroupid, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Cat_MasterDataGroupItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>Xóa hoặc chuyển đổi trạng thái (Cat_MasterDataGroup) sang IsDelete</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_MasterDataGroupItemModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_MasterDataGroupItemEntity, Cat_MasterDataGroupItemModel>(id);
            return result;
        }

        /// <summary>[Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Cat_Bank)</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_MasterDataGroupItem")]
        public string Post([Bind]Cat_MasterDataGroupItemModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_MasterDataGroupItemModel>(model, "Cat_MasterDataGroupItem", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
               // return model;
                return "false";
            }
            #endregion

            #region chuyển string sang list<guid> (MasterDataGroup)
            if (model != null && !string.IsNullOrEmpty(model.ObjectIDStr))
            {
                var lstObjectIDStr = model.ObjectIDStr.Split(',');
                Guid objectID = Guid.Empty;
                model.ObjectIDs = new List<Guid?>();
                foreach (var item in lstObjectIDStr)
                {
                    Guid.TryParse(item, out objectID);
                    if (objectID != Guid.Empty)
                    {
                        model.ObjectIDs.Add(objectID);
                    }
                }
            }

            #endregion

            Cat_MasterDataGroupServices catServices = new Cat_MasterDataGroupServices();
            var masterDataGroupItemEntity = model.CopyData<Cat_MasterDataGroupItemEntity>();
            var result = catServices.AddMasterDataGroupItems(masterDataGroupItemEntity);
           // return result.CopyData<Cat_MasterDataGroupItemModel>();
            return "Success";

            //ActionService service = new ActionService(UserLogin);
            //return service.UpdateOrCreate<Cat_MasterDataGroupItemEntity, Cat_MasterDataGroupItemModel>(model);
        }
    }
}
