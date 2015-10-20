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
    public class Tra_ClassController : ApiController
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
        public Tra_ClassModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_ClassModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_ClassEntity>(id, ConstantSql.hrm_tra_sp_get_ClassById, ref status);
            if (entity!=null)
            {
                if (entity.TrainerOtherList != null)
                    entity.TrainerOtherList = entity.TrainerOtherList.Replace(" ", "");
                model = entity.CopyData<Tra_ClassModel>();
                if(model.IsTrainingOut == false)
                {
                    model.IsTrainingInside = true;
                }
                    if (entity.Teacher != null)
                    {
                        //Guid profileid = Guid.Parse(entity.Teacher);
                        var lstEntityprofile = service.GetData<Tra_RequirementTrainEntity>(entity.Teacher, ConstantSql.hrm_hr_sp_get_ProfileById, ref status);
                        if (lstEntityprofile!=null && lstEntityprofile.Count > 0)
                        {
                            var entityprofile = lstEntityprofile[0];
                            if (entityprofile != null && entityprofile.ProfileID != null && entityprofile.ProfileName != null)
                            {
                                model.ProfileID = entityprofile.ID;
                                model.ProfileName = entityprofile.ProfileName;
                                model.CodeEmp = entityprofile.CodeEmp;
                            }
                        }
                        
                    }
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Tra_Certificate) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_ClassModel DeleteOrRemove(string id)
        {
            var model = new Tra_ClassModel();
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
                    var result = baseServices.GetData<Tra_TraineeEntity>(objs, ConstantSql.hrm_cat_sp_get_TraineeByClassID, UserLogin, ref status);
                    if (result != null && result.Count > 0)
                    {
                        isDelete = false;
                        break;
                    }
                }
            }
            if (isDelete)
                model = service.DeleteOrRemove<Tra_ClassEntity, Tra_ClassModel>(id);
            else
            {
                status = ConstantMessages.YouMustDeleteDetailRecord.TranslateString();
                model.SetPropertyValue(Constant.ActionStatus, status);
            }
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Tra_Certificate)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_Course")]
        public Tra_ClassModel Post([Bind]Tra_ClassModel model)
         {
            #region Validate
            string message = string.Empty;
            if (model != null && model.ProfileID != Guid.Empty)
            {
                model.Teacher = Common.DotNetToOracle(model.ProfileID.ToString());

            }
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_ClassModel>(model, "Tra_Class", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            model.Formula = model.Formula.Replace("[+]", "+");
            return service.UpdateOrCreate<Tra_ClassEntity, Tra_ClassModel>(model);
        }
    }
}
