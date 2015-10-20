using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Training.Models;
using HRM.Business.Training.Models;
using HRM.Business.Training.Domain;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Tra_PlanDetailController : ApiController
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
        public Tra_PlanDetailModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_PlanDetailModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_PlanDetailEntity>(id, ConstantSql.hrm_tra_sp_get_PlanDetailById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Tra_PlanDetailModel>();
            }
           
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng (Tra_Plan) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_PlanDetailModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Tra_PlanDetailEntity, Tra_PlanDetailModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa (Tra_Plan)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_Plan")]
        public Tra_PlanDetailModel Post([Bind]Tra_PlanDetailModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_PlanDetailModel>(model, "Tra_PlanDetail", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
          
            
             ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Tra_PlanDetailEntity, Tra_PlanDetailModel>(model);
        }
    }
}
