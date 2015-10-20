using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using HRM.Business.Hr.Models;
using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using System.Web.Mvc;
using System;
namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Hre_SoftSkillController : ApiController
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
        /// [Son.Vo] - Lấy dữ liệu SoftSkill(Hre_SoftSkill) theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_SoftSkillModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Hre_SoftSkillModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetByIdUseStore<Hre_SoftSkillEntity>(id, ConstantSql.hrm_hr_sp_get_SoftSkillById, ref status);
            var entity = service.GetData<Hre_SoftSkillEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_SoftSkillById, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Hre_SoftSkillModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Son.Vo] - Xóa hoặc chuyển đổi trạng thái của SoftSkill(Hre_SoftSkill) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Hre_SoftSkillModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Hre_SoftSkillEntity, Hre_SoftSkillModel>(id);
            return result;
        }

        /// <summary>
        /// [Son.Vo] - Xử lý thêm mới hoặc chỉnh sửa một SoftSkill(Hre_SoftSkill)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Hre_SoftSkill")]
        public Hre_SoftSkillModel Post([Bind]Hre_SoftSkillModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Hre_SoftSkillModel>(model, "Hre_SoftSkill", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Hre_SoftSkillEntity, Hre_SoftSkillModel>(model);
        }
    }
}
