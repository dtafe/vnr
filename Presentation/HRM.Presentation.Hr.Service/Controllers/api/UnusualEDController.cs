using HRM.Business.Payroll.Domain;

using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Payroll.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Payroll.Models;
using System.Linq;

namespace HRM.Presentation.Payroll.Service.Controllers.api
{
    public class UnusualEDController : ApiController
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
        //
        // GET: /UnusualED/

        public UnusualEDModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new UnusualEDModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<UnusualEDEntity>(id, ConstantSql.hrm_cat_sp_get_AccountTypeByIds, ref status);
            if (entity != null)
            {
                model = entity.CopyData<UnusualEDModel>();
            }
            model.ActionStatus = status;
            return model;
        }


        // DELETE api/<controller>/5
        //public void Delete(Guid ID)
        //{
        //    var service = new Cat_AccountTypeServices();
        //    var result = service.Remove<Cat_AccountTypeEntity>(ID);
        //}

        public UnusualEDModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<UnusualEDEntity, UnusualEDModel>(id);
            return result;
        }


        /// <summary>
        /// Xử lý thêm mới hoặc chỉnh sửa loại tài khoản(Cat_AccountType)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/UnusualED")]
        public UnusualEDModel Post([Bind]UnusualEDModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<UnusualEDEntity, UnusualEDModel>(model);
        }
	}
}