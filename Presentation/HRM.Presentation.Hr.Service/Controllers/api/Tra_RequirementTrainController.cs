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
using System.Collections.Generic;
using HRM.Business.Training.Domain;
using System.Linq;
namespace HRM.Presentation.Hr.Service.Controllers.api
{

    public class Tra_RequirementTrainController : ApiController
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
        public Tra_RequirementTrainModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_RequirementTrainModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_RequirementTrainEntity>(id, ConstantSql.hrm_tra_sp_get_RequirementTrainIds, ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Tra_RequirementTrainModel>();
                if (entity.PersonRequirement != null)
                {
                    Guid profileid = Guid.Parse(entity.PersonRequirement);
                    var entityprofile = service.GetByIdUseStore<Tra_RequirementTrainEntity>(profileid, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
                    if (entityprofile != null && entityprofile.ProfileID != null && entityprofile.ProfileName != null)
                    {
                        model.ProfileID = entityprofile.ID;
                        model.ProfileName = entityprofile.ProfileName;
                        model.CodeEmp = entityprofile.CodeEmp;
                    }
                }
            }
            if (model.IsTrainingOutside != null && model.IsTrainingOutside == false)
                model.IsTrainingInside = true;
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Tra_Certificate) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_RequirementTrainModel DeleteOrRemove(string id)
        {
            var model = new Tra_RequirementTrainModel();
            ActionService service = new ActionService(UserLogin);
            var baseServices = new Tra_ClassServices();
            //var baseService = new BaseService();
            bool isDelete = true;
            string status = "";
            var idItem = id.Split(',');
            foreach (var item in idItem)
            {
                var idInt = Guid.Empty;
                Guid.TryParse(item, out idInt);
                if (idInt != Guid.Empty)
                {
                    var objs = new List<object>();
                    objs.Add(Guid.Parse(item));
                    var result = baseServices.GetData<Tra_RequirementTrainDetailModel>(objs, ConstantSql.hrm_tra_sp_get_RequirementTrainDetailByRMTDTID,UserLogin, ref status);
                    if (result != null && result.Count > 0)
                    {
                        isDelete = false;
                        break;
                    }
                }
            }
            if (isDelete)
                model = service.DeleteOrRemove<Tra_RequirementTrainEntity, Tra_RequirementTrainModel>(id);
            else
            {
                status = ConstantMessages.YouMustDeleteDetailRecord.TranslateString();
                model.SetPropertyValue(Constant.ActionStatus, status);
            }
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Tra_RequirementTrain)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_RequirementTrain")]
        public Tra_RequirementTrainModel Post([Bind]Tra_RequirementTrainModel model)
        {
            #region Validate
            string message = string.Empty;
            if(model != null && model.ProfileID != Guid.Empty)
            {
                model.PersonRequirement = model.ProfileID.ToString();
            }
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_RequirementTrainModel>(model, "Tra_RequirementTrain", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Tra_RequirementTrainEntity, Tra_RequirementTrainModel>(model);
        }
    }
}
