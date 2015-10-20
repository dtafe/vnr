using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Linq;


namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Cat_SkillTopicController : ApiController
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
        public Cat_SkillTopicModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Cat_SkillTopicModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Cat_SkillTopicEntity>(id, ConstantSql.hrm_cat_sp_get_SkillTopicById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Cat_SkillTopicModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Cat_Topic) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_SkillTopicModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Cat_SkillTopicEntity, Cat_SkillTopicModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa (Cat_SkillTopic)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Cat_SkillTopic")]
        public Cat_SkillTopicModel Post([Bind]Cat_SkillTopicModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Cat_SkillTopicModel>(model, "Cat_SkillTopic", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Cat_SkillTopicEntity, Cat_SkillTopicModel>(model);
        }
    }
}
