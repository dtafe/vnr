using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Web.Mvc;
using System;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_VisaInfoController : ApiController
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
        /// [Quoc.Do] - Lấy dữ liệu VisaInfo(Hre_VisaInfo) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_VisaInfoModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_VisaInfoModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_WorkHistoryEntity>(id, ConstantSql.hrm_hr_sp_get_VisaInfoById, ref status);
            var entity = service.GetData<Hre_VisaInfoEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_VisaInfoById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_VisaInfoModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Quoc.Do] - Xóa hoặc chuyển đổi trạng thái của VisaInfo(Hre_VisaInfo) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_VisaInfoModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_VisaInfoEntity, Hre_VisaInfoModel>(id);
            return result;
        }

        /// <summary>
        /// [Quoc.Do] - Xử lý thêm mới hoặc chỉnh sửa một  VisaInfo(Hre_VisaInfo)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_VisaInfo")]
        public Hre_VisaInfoModel Post([Bind]Hre_VisaInfoModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_VisaInfoModel>(model, "Hre_VisaInfo", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_VisaInfoEntity, Hre_VisaInfoModel>(model);
        }

    }
}
