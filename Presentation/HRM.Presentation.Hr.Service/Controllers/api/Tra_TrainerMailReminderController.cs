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
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Tra_TrainerMailReminderController : ApiController
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
        public Tra_TrainerMailReminderModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_TrainerMailReminderModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_TrainerMailReminderEntity>(id,ConstantSql.hrm_tra_sp_get_TrainerMailReminderById ,ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Tra_TrainerMailReminderModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Tra_TrainerMailReminder) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_TrainerMailReminderModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Tra_TrainerMailReminderEntity, Tra_TrainerMailReminderModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Tra_TrainerMailReminder)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_TrainerMailReminder")]
        public Tra_TrainerMailReminderModel Post([Bind]Tra_TrainerMailReminderModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_TrainerMailReminderModel>(model, "Tra_TrainerMailReminder", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Tra_TrainerMailReminderEntity, Tra_TrainerMailReminderModel>(model);
        }
    }
}
