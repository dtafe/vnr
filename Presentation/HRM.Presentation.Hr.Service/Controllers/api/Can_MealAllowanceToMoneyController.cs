using System;
using HRM.Business.Canteen.Domain;
using HRM.Business.Canteen.Models;
using HRM.Business.Hr.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Canteen.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Presentation.Service;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Can_MealAllowanceToMoneyController : ApiController
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
        
        // GET api/<controller>/5
        public Can_MealAllowanceToMoneyModel Get(Guid id)
        {
            var service = new Can_MealAllowanceToMoneyServices();
            var model = new Can_MealAllowanceToMoneyModel();
            var result = service.GetData<Can_MealAllowanceToMoneyEntity>(id, ConstantSql.hrm_can_get_MealAllowanceToMoneyById,UserLogin, ref status).FirstOrDefault();

            if (result != null)
            {
                model = result.CopyData<Can_MealAllowanceToMoneyModel>();
            }
            model.ActionStatus = status;
            return model;
            
        }
       

        /// <summary>
        /// Xử lí eidt và add new truyền theo script
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>     
        public Can_MealAllowanceToMoneyModel Post(Can_MealAllowanceToMoneyModel model)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.UpdateOrCreate<Can_MealAllowanceToMoneyEntity, Can_MealAllowanceToMoneyModel>(model);
            return result;
        }

        public Can_MealAllowanceToMoneyModel Put(Can_MealAllowanceToMoneyModel model)
        {
            var can_Meal = new Can_MealAllowanceToMoneyEntity
            {
                ID = model.ID,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                ProfileID = model.ProfileID,
                Note = model.Note
            };
            var service = new Can_MealAllowanceToMoneyServices();
            if (model.ID != null)
            {
                can_Meal.ID = model.ID;
                service.Edit<Can_MealAllowanceToMoneyEntity>(can_Meal);
            }
            else
            {
                service.Add<Can_MealAllowanceToMoneyEntity>(can_Meal);
            }
            return model;
        }
      
        public Can_MealAllowanceToMoneyModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);

            service.DeleteOrRemove<Can_MealAllowanceToMoneyEntity,Can_MealAllowanceToMoneyModel>(id);
            var result = new Can_MealAllowanceToMoneyModel();
            return result;
        }
    }
}
