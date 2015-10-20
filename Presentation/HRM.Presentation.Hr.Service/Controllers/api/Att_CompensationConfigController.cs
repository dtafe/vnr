using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Att_CompensationConfigController : ApiController
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
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_CompensationConfigModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Att_CompensationConfigModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Att_CompensationConfigEntity>(id, ConstantSql.hrm_att_sp_get_CompensationConfigById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Att_CompensationConfigModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        ///  - Xóa hoặc chuyển đổi trạng thái  sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Att_CompensationConfigModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Att_CompensationConfigEntity, Att_CompensationConfigModel>(id);
            return result;
        }

        /// <summary>
        /// - Xử lý thêm mới hoặc chỉnh sửa 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Att_CompensationConfig")]
        public Att_CompensationConfigModel Post([Bind]Att_CompensationConfigModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Att_CompensationConfigModel>(model, "Att_CompensationConfig", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Att_CompensationConfigEntity, Att_CompensationConfigModel>(model);
        }
    }
}