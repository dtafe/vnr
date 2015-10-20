using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Canteen.Models;
using HRM.Presentation.Service;
using VnResource.Helper.Data;
using System.Linq;
using System;
using HRM.Business.Canteen.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Can_MealPriceTypeSettingController : ApiController
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
        public Can_MealAllowanceTypeSettingModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Can_MealAllowanceTypeSettingModel();
            ActionService service = new ActionService(UserLogin);
            //var entity = service.GetById<Can_MealAllowanceTypeSettingEntity>(id, ref status);
            var entity = service.GetData<Can_MealAllowanceTypeSettingEntity>(id, ConstantSql.hrm_can_sp_get_mealallowtypebyid, ref status).FirstOrDefault();
            if (entity != null)
            {
                model = entity.CopyData<Can_MealAllowanceTypeSettingModel>();
            }
            model.ActionStatus= status;
            return model;
        }

        /// <summary>
        /// [Quan.Nguyen] - Xóa hoặc chuyển đổi trạng thái của MealAllowanceTypeSetting sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Can_MealAllowanceTypeSettingModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Can_MealAllowanceTypeSettingEntity, Can_MealAllowanceTypeSettingModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        public Can_MealAllowanceTypeSettingModel Post([Bind]Can_MealAllowanceTypeSettingModel model)
        {
            
            ActionService service = new ActionService(UserLogin);
            model.Standard = true;
            return service.UpdateOrCreate<Can_MealAllowanceTypeSettingEntity, Can_MealAllowanceTypeSettingModel>(model);
        }
    }
}
