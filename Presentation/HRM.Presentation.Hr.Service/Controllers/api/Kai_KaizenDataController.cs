using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Business.Recruitment.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Payroll.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{ 
    public class Kai_KaizenDataController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
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

        public Kai_KaizenDataModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Kai_KaizenDataModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Kai_KaizenDataEntity>(id, ConstantSql.hrm_kai_sp_get_KaiZenDataById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Kai_KaizenDataModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Kai_KaizenDataModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Kai_KaizenDataEntity, Kai_KaizenDataModel>(id);
            return result;
        }

        
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Kai_kaizenData")]
        public Kai_KaizenDataModel Post([Bind]Kai_KaizenDataModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Kai_KaizenDataModel>(model, "Kai_KaizenData", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            if (model.IsNotRankMask != null && model.IsNotRankMask == true)
            {
                model.ActionStatus = ConstantDisplay.HRM_Kai_KaiZenData_IsNotRankMask_Mess.TranslateString();
                return model;
            }
            #endregion

            #region Xử lý bắt trùng điểm áp dụng

            string status = string.Empty;
            var baseService = new BaseService();
            var listModel = new List<object>();
            listModel.AddRange(new object[3]);
            listModel[1] = 1;
            listModel[2] = Int32.MaxValue - 1;
            var result = baseService.GetData<Kai_RankMarkEntity>(listModel, ConstantSql.hrm_kai_sp_get_RankMark, UserLogin,ref status);

            if (model.MarkIdea != null && model.MarkPerform != null)
            {
                result = result.Where(m => m.MarkIdea != null && m.MarkIdea == model.MarkIdea).ToList();
                if (!result.Any(m => m.MarkPerform == model.MarkPerform))
                {
                    model.ActionStatus = ConstantDisplay.HRM_Kai_RankMark_NotCheckValidateMarkIdeaAndMarkPerform.TranslateString();
                    return model;
                }
            }

            #endregion

            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Kai_KaizenDataEntity, Kai_KaizenDataModel>(model);
        }
	}
}