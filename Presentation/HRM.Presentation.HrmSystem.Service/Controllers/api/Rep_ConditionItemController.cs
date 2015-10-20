using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using System.Web;
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Report.Domain;
using HRM.Business.Report.Models;


namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Rep_ConditionItemController : ApiController
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
        public Rep_ConditionItemModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rep_ConditionItemModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetData<Rep_ConditionItemEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_rep_sp_get_ConditionItemByID, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Rep_ConditionItemModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Hieu.Van] - Xóa hoặc chuyển đổi trạng thái sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rep_ConditionItemModel DeleteOrRemove(Guid id)
        {
            //ActionService service = new ActionService(UserLogin);
            //var result = service.DeleteOrRemove<Att_CutOffDurationEntity, Att_CutOffDurationModel>(id);
            //return result;
            Rep_ConditionItemServices service = new Rep_ConditionItemServices();
            var result = service.Remove<Rep_ConditionItemEntity>(id);
            //return result;
            return new Rep_ConditionItemModel();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rep_ConditionItem")]
        public Rep_ConditionItemModel Post([Bind]Rep_ConditionItemModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rep_ConditionItemModel>(model, "Rep_ConditionItem", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rep_ConditionItemEntity, Rep_ConditionItemModel>(model);
        }

	}
}