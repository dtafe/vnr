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
using HRM.Business.Hr.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Training.Models;
using HRM.Business.Training.Models;
using HRM.Business.Hr.Domain;
using HRM.Presentation.Hr.Models;
using Kendo.Mvc.Extensions;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;

using System.Collections;
using HRM.Business.Training.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Tra_RequirementTrainCustormController : ApiController
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
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Tra_RequirementTrainEntity, Tra_RequirementTrainModel>(id);
            return result;
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
            if(model.isAnalysis==true)
            {
                string message = string.Empty;
                var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_RequirementTrainModel>(model,"Tra_RequirementTrain_Tab", "Tra_RequirementTrain", ref message);
                if (!checkValidate)
                {
                    model.ActionStatus = message;
                    return model;
                }
            }
            #endregion

            var requirementDetailServices = new Tra_RequirementTrainDetailServices();
            var profileServices = new Hre_ProfileServices();
             string[] arrCodeEmp = new string[1];
             if (!string.IsNullOrEmpty(model.lstCodeEmp) && model.lstCodeEmp.IndexOf(',') > 1)
             {
                 arrCodeEmp = model.lstCodeEmp.Split(',').ToArray();
             }
             else {
                 arrCodeEmp[0] = model.lstCodeEmp;
             }
        
            ActionService service = new ActionService(UserLogin);
            string status = string.Empty;
            //var objProfile = new List<object>();
            //objProfile.AddRange(new object[17]);
            //objProfile[15] = 1;
            //objProfile[16] = int.MaxValue - 1;
            //var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, UserLogin, ref status).ToList().Translate<Hre_ProfileModel>();
            //if (arrCodeEmp != null)
            //{
            //    lstProfile = lstProfile.Where(s => arrCodeEmp.Contains(s.CodeEmp)).ToList();
            //}
            var lstProfile = profileServices.GetData<Hre_ProfileEntity>(model.lstCodeEmp, ConstantSql.hrm_hr_sp_get_ProfileAllByCodeEmps, UserLogin, ref status).ToList().Translate<Hre_ProfileModel>();

            model = service.UpdateOrCreate<Tra_RequirementTrainEntity, Tra_RequirementTrainModel>(model);

            var objRequirementDetail = new List<object>();
            objRequirementDetail.Add(null);
            objRequirementDetail.Add(1);
            objRequirementDetail.Add(int.MaxValue -1);
            var lstRequirementDetail = requirementDetailServices.GetData<Tra_RequirementTrainDetailEntity>(objRequirementDetail,ConstantSql.hrm_tra_sp_get_RequirementDetail,UserLogin,ref status).ToList();
            foreach (var item in lstProfile)
            {
               
                var entity = lstRequirementDetail.Where(s => s.ProfileID == item.ID && s.RequirementTrainID == model.ID && s.CourseID == model.CourseID).FirstOrDefault();
             
                var requirementDetailEntity = new Tra_RequirementTrainDetailEntity();
                requirementDetailEntity.ProfileID = item.ID;
                requirementDetailEntity.RequirementTrainID = model.ID;
                requirementDetailEntity.CourseID = model.CourseID;
                requirementDetailEntity.YearAnalyze = model.DateSeniority;
                if (item.DateHire != null && model.DateSeniority != null)
                {
                    requirementDetailEntity.Seniority = Math.Floor(model.DateSeniority.Value.Subtract(item.DateHire.Value).TotalDays/30);
                }
                if (entity == null)
                {
                    model.ActionStatus = requirementDetailServices.Add(requirementDetailEntity);
                }
            }

            return model;
            //return service.UpdateOrCreate<Tra_RequirementTrainEntity, Tra_RequirementTrainModel>(model);
        }
    }
}
