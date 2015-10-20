using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class DashboardApproveController : ApiController
    {
        string status = string.Empty;
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
        public DashboardApproveModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new DashboardApproveModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<DashboardApproveEntity>(id, ConstantSql.hrm_sys_sp_get_DashboardApproveById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<DashboardApproveModel>();
            }
            //model.ActionStatus = status;
            return model;
        }
    }
}
