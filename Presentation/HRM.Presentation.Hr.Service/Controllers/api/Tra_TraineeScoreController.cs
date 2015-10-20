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
    public class Tra_TraineeScoreController : ApiController
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
        public Tra_TraineeScoreModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_TraineeScoreModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_TraineeScoreEntity>(id, ConstantSql.hrm_tra_sp_get_TraineeScoreById, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Tra_TraineeScoreModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng (Tra_TraineeScore) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_TraineeScoreModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Tra_TraineeScoreEntity, Tra_TraineeScoreModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa (Tra_TraineeScore)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_TraineeScore")]
        public Tra_TraineeScoreModel Post([Bind]Tra_TraineeScoreModel model)
        {
            #region Validate
            ActionService service = new ActionService(UserLogin);
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_TraineeScoreModel>(model, "Tra_TraineeScore", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            return service.UpdateOrCreate<Tra_TraineeScoreEntity, Tra_TraineeScoreModel>(model);
        }
    }
}
