using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Payroll.Domain;
using HRM.Presentation.Payroll.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;

namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_ProductCapacityController : ApiController
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
        /// [Tho.Bui] - Lấy dữ liệu Quốc Gia(Cat_Country) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_ProductCapacityModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_ProductCapacityModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_ProductCapacityEntity>(id, ConstantSql.hrm_sal_sp_get_ProductCapacityById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_ProductCapacityModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Cat_Topic) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_ProductCapacityModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_ProductCapacityEntity, Sal_ProductCapacityModel>(id);
            return result;
        }

        /// <summary>
        /// - Xử lý thêm mới hoặc chỉnh sửa (Sal_ProductCapacity)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_ProductCapacity")]
        public Sal_ProductCapacityModel Post([Bind]Sal_ProductCapacityModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_ProductCapacityModel>(model, "Sal_ProductCapacity", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_ProductCapacityEntity, Sal_ProductCapacityModel>(model);
        }
    }
}
