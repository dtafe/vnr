using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Evaluation.Domain;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities;

using VnResource.Helper.Data;

namespace HRM.Presentation.Evaluation.Service.Controllers.api
{
    public class Eva_PerformanceProfileController : ApiController
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
        public Eva_PerformanceModel  GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_PerformanceModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_PerformanceEntity>(id, ConstantSql.hrm_eva_sp_get_PerformanceById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_PerformanceModel>();
                //Eva_PerformanceEvaServices service1 = new Eva_PerformanceEvaServices();
                //model.AttachFileLast = service1.GetAttachFile(model.ID);

                #region Đọc Xml từ db và chuyển sang string

                if (model != null)
                {
                    var performanceService = new Eva_PerformanceServices();
                    var doc = performanceService.ReadXml(model.AttachFile, AttachFileEntity.FieldNames.FullName);
                    model.AttachFileLast = string.Join(",", doc.ToArray());
                    model.AttachFile = model.AttachFileLast;
                    model.AttachFiles = doc;
                }
                #endregion
            }
            model.ActionStatus = status;
            return model;
        }

        public Eva_PerformanceModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_PerformanceEntity, Eva_PerformanceModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_Performance")]
        public Eva_PerformanceModel Post([Bind]Eva_PerformanceModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = Business.Main.Domain.ValidatorService.OnValidateData<Eva_PerformanceModel>(model, "Eva_PerformanceProfile", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

            var strEvaluatorIds = model.EvaluatorIDList;
            model.EvaluatorIDs = new List<Guid>();
            if (!string.IsNullOrEmpty(strEvaluatorIds))
            {
                var evaluators = strEvaluatorIds.Split(',');
                foreach (var evaluator in evaluators)
                {
                    if (!string.IsNullOrEmpty(evaluator))
                    {
                        var evaluatorGuid = Guid.Parse(evaluator);
                        model.EvaluatorIDs.Add(evaluatorGuid);
                    }
                } 
           }
            
            var evaluatorService = new Eva_PerformanceServices();
            var evaluatorEntity = model.CopyData<Eva_PerformanceEntity>();
            evaluatorEntity.KPIs = model.KPIs.Translate<Eva_KPIEntity>();
            if (model.ID == Guid.Empty && evaluatorService.CheckPerformanceExist(evaluatorEntity))
            {
                model.ActionStatus = ConstantDisplay.HRM_Evaluation_Performance_Duplicate.TranslateString();
                return model;
            }


            #region chuyển attachFile thành xml để ghi vào db
            if (evaluatorEntity != null && model.AttachFiles.Any())
            {
                var lstFiles = new List<AttachFileEntity>();
                foreach (var fileItem in model.AttachFiles)
                {
                    if (!string.IsNullOrEmpty(fileItem))
                    {
                        var attachFile = new AttachFileEntity(fileItem);
                        lstFiles.Add(attachFile);
                    }
                }
                var doc = evaluatorService.WriteXml(lstFiles);
                evaluatorEntity.AttachFile = evaluatorService.XmlToString(doc);
            }
            #endregion

            if (model.ID == Guid.Empty)
            {
                evaluatorService.AddPerfomance(evaluatorEntity, true);
            }
            else
            {
                evaluatorService.Edit(evaluatorEntity);
            }
            return model;
        }
	}
}