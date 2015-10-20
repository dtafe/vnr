using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Service;
using VnResource.Helper.Data;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_AccidentController : ApiController
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
        /// [Chuc.Nguyen] - Lấy dữ liệu bảng Tai Nạn(Hre_Accident) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_AccidentModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_AccidentModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Hre_AccidentEntity>(id, ConstantSql.hrm_hr_sp_get_AccidentById, ref status);
            //var entity = service.GetData<Hre_AccidentEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_AccidentById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_AccidentModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Tai Nạn(Hre_Accident) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_AccidentModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_AccidentEntity, Hre_AccidentModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Tai Nạn(Hre_Accident)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_Accident")]
        public Hre_AccidentModel Post([Bind]Hre_AccidentModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_AccidentModel>(model, "Hre_Accident", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_AccidentEntity, Hre_AccidentModel>(model);
        }
    }
}
