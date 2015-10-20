using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.HrmSystem.Domain;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using HRM.Business.Evaluation.Domain;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Eva_PerformanceEvaController : ApiController
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
        public Eva_PerformanceEvaModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_PerformanceEvaModel();
            var service = new ActionService(UserLogin);

            #region Thêm vào bảng Eva_performanceEvaDetail trước khi vào màn hình chờ đánh giá
            //truoc khi vào màn hình chờ đánh giá , sẽ thêm vào bảng eva_performanceEvaDetail nếu bảng này chưa có data ứng với performanceID
            var performanceService = new Eva_PerformanceServices();
            performanceService.AddPerformanceEvaDetail360(id);
            #endregion
            

           // var isEvaluation = false;
            var entity = service.GetByIdUseStore<Eva_PerformanceEvaEntity>(id, ConstantSql.hrm_eva_sp_get_PerformanceEvaById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_PerformanceEvaModel>();
                
                Eva_PerformanceEvaServices service1 = new Eva_PerformanceEvaServices();
                var performanceservice = new Eva_PerformanceServices();
                if (model != null)
                {
                    model.IsSuperiorHasPerformance = service1.CheckSuperiorHasPerformance(model.PerformanceID ?? Guid.Empty, model.ID);
                    model.IsEvaluation = service1.CheckOrder(model.PerformanceID ?? Guid.Empty, model.ID);
                    model.AttachFileLast = performanceservice.GetAttachFileByOrderNo(model.PerformanceID ?? Guid.Empty, model.OrderEva ?? 0);
                    model.AttachFile = performanceservice.GetAttachFileByOrderNo(model.PerformanceID ?? Guid.Empty, model.OrderEva ?? 0);
                }
            }
            model.ActionStatus = status;

            #region lấy khoảng thời gian cho phép nhập điểm
            var serv = new Sys_AttOvertimePermitConfigServices();
            var dateEndMark = serv.GetConfigValue<DateTime>(AppConfig.HRM_EVA_CONFIG_DATEENDMARK);
            var dateStartMark = serv.GetConfigValue<DateTime?>(AppConfig.HRM_EVA_CONFIG_DATESTARTMARK);
            model.DateStartMark = dateStartMark;
            model.DateEndMark = dateEndMark;
            #endregion


            return model;
        }


        public Eva_PerformanceEvaModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_PerformanceEvaEntity, Eva_PerformanceEvaModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_PerformanceEva")]
        public Eva_PerformanceEvaModel Post([Bind]Eva_PerformanceEvaModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_PerformanceEvaModel>(model, "Eva_PerformanceEva", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion

            //ActionService service = new ActionService(UserLogin);
            //return service.UpdateOrCreate<Eva_PerformanceEvaEntity, Eva_PerformanceEvaModel>(model);
            var PerformanceEvaSevice = new Eva_PerformanceEvaServices();
            var PerformanceEvaEntity = model.CopyData<Eva_PerformanceEvaEntity>();
            PerformanceEvaEntity.PerformanceEvaDetails = model.PerformanceEvaDetails.Translate<Eva_PerformanceEvaDetailEntity>();
            
            #region chuyển attachFile thành xml để ghi vào db
            if (PerformanceEvaEntity != null && model.AttachFiles.Any())
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
                var performanceService = new Eva_PerformanceServices();
                var doc = performanceService.WriteXml(lstFiles);
                PerformanceEvaEntity.AttachFile = performanceService.XmlToString(doc);
            }
            #endregion


            PerformanceEvaSevice.EditPerformanceEva(PerformanceEvaEntity);
            return model;

        }
	}
}