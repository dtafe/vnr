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
    public class Tra_RequirementTrainDetailController : ApiController
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
        public Tra_RequirementTrainDetailModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_RequirementTrainDetailModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_RequirementTrainDetailEntity>(id, ConstantSql.hrm_tra_sp_get_RequirementTrainDetailIds, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Tra_RequirementTrainDetailModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] -
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_RequirementTrainDetailModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Tra_RequirementTrainDetailEntity, Tra_RequirementTrainDetailModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Tra_RequirementTrain)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_RequirementTrainDetail")]
        public Tra_RequirementTrainDetailModel Post([Bind]Tra_RequirementTrainDetailModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_RequirementTrainDetailModel>(model, "Tra_RequirementTrainDetail", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Tra_RequirementTrainDetailEntity, Tra_RequirementTrainDetailModel>(model);
        }
    }
}
