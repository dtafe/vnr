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
    public class Hre_ProfileQualificationController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu Trình độ chuyên môn(Hre_Qualification) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_ProfileQualificationModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_ProfileQualificationModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_QualificationEntity>(id,ConstantSql.hrm_hr_sp_get_QualificationById, ref status);
            var entity = service.GetData<Hre_ProfileQualificationEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileQualificationById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_ProfileQualificationModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của Trình độ chuyên môn(Hre_Qualification) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_ProfileQualificationModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_ProfileQualificationEntity, Hre_ProfileQualificationModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một Trình độ chuyên môn(Hre_Qualification)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_ProfileQualificationModel")]
        public Hre_ProfileQualificationModel Post([Bind]Hre_ProfileQualificationModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_ProfileQualificationModel>(model, "Hre_ProfileQualification", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_ProfileQualificationEntity, Hre_ProfileQualificationModel>(model);
        }
    }
}
