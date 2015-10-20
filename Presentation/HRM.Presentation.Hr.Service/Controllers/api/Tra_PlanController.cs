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
using System.Collections.Generic;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Tra_PlanController : ApiController
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
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_PlanModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_PlanModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_PlanEntity>(id, ConstantSql.hrm_tra_sp_get_PlanIds, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Tra_PlanModel>();
            }
            if (model.Outside != null && model.Outside == false)
                model.Inside = true;
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng (Tra_Plan) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_PlanModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Tra_PlanEntity, Tra_PlanModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa (Tra_Plan)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_Plan")]
        public Tra_PlanModel Post([Bind]Tra_PlanModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_PlanModel>(model, "Tra_Plan", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            #region Check kiểm tra cột sum quantity
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            if(model.ID != null)
            {
                var planDetailServices = new Tra_PlanDetailServices();
                var lstObjPlanDetail = new List<object>();
                lstObjPlanDetail.Add(null);
                lstObjPlanDetail.Add(1);
                lstObjPlanDetail.Add(int.MaxValue - 1);
                var lstPlanDetail = planDetailServices.GetData<Tra_PlanDetailEntity>(lstObjPlanDetail, ConstantSql.hrm_tra_sp_get_PlanDetail,UserLogin, ref status).ToList();
                var lstPlanDetailByPlanID = lstPlanDetail.Where(s => s.PlanID == model.ID).ToList();
                if (lstPlanDetailByPlanID != null)
                {
                   
                       if (model.SumQuantity < lstPlanDetailByPlanID.Count)
                       {
                           model.ActionStatus = ConstantMessages.WarningQuantityPlanMustBeGreaterQuantityPlandetail.TranslateString();
                           return model;
                       }
                   
                }
            }
            #endregion
           
            return service.UpdateOrCreate<Tra_PlanEntity, Tra_PlanModel>(model);
        }
    }
}
