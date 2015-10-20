using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_AsynTaskController : Controller
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
        // GET: Sys_AsynTask
        /// <summary>
        /// [Hien.Nguyen] Get & table Sys_AsynTask
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_AsynTask")]
        public double GetPercentComplete(Guid id)
        {
            var service1 = new Sys_AsynTaskServices();
      //      Guid list1 = service1.Get();

            string status = string.Empty;
            var model = new Sys_AsynTaskModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Sys_AsynTaskEntity>(id, ConstantSql.hrm_sys_sp_get_AsynTask_ById, ref status);
            if (entity != null)
            {
                return entity.PercentComplete * 100;
            }
            return 100;

        }
    }
}