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
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;

namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class Sal_CodeAlocalController : ApiController
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
        public Sal_CodeAlocalModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Sal_CodeAlocalModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sal_CodeAlocalEntity>(id, ConstantSql.hrm_sal_sp_get_CodeAlocalById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Sal_CodeAlocalModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Tho.Bui] - Xóa hoặc chuyển đổi trạng thái Quốc Gia(Cat_Country) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sal_CodeAlocalModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Sal_CodeAlocalEntity, Sal_CodeAlocalModel>(id);
            return result;
        }

        /// <summary>
        /// [Tin.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Quốc Gia(Cat_Country)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sal_CodeAlocal")]
        public Sal_CodeAlocalModel Post([Bind]Sal_CodeAlocalModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Sal_CodeAlocalModel>(model, "Sal_CodeAlocal", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sal_CodeAlocalEntity, Sal_CodeAlocalModel>(model);
        }
    }
}
