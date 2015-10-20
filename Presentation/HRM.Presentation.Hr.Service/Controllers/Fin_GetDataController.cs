using HRM.Business.Attendance.Domain;
using HRM.Business.Main.Domain;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Domain;
using System.Reflection;
using System.Collections;
using System.Data.SqlTypes; 
using HRM.Business.Attendance.Models;
using HRM.Presentation.Service;
using HRM.Business.Hr.Models;
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.HrmSystem.Domain;
using HRM.Presentation.Hr.Models;
using HRM.Business.Finance.Domain;
using HRM.Business.Finance.Models;



namespace HRM.Presentation.Hr.Service.Controllers
{
    public class Fin_GetDataController : BaseController
    {
        string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        public ActionResult GetClaimApprovedList([DataSourceRequest] DataSourceRequest request, FIN_ApprovedClaimSearchModel model)
        {
            var baseService = new BaseService();
            FIN_ClaimService service = new FIN_ClaimService();
            
            var result = service.GetClaimApprovedList(model.UserApproveID.Value);
                        

            if (model.IsExport)
            {
                var fullPath = ExportService.Export(result, model.ValueFields.Split(','));
                return Json(fullPath);
            }

            request.Page = 1;
            var dataSourceResult = result.ToDataSourceResult(request);
            dataSourceResult.Total = result.Count() <= 0 ? 0 : result.FirstOrDefault().TotalRow;
            return new JsonResult { Data = dataSourceResult, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult GetHistoryApproveECLAIMList([DataSourceRequest] DataSourceRequest request, FIN_HistoryApproveECLAIMSearchModel model)
        {
            return GetListDataAndReturn<FIN_HistoryApproveECLAIMModel, FIN_HistoryApproveECLAIMEntity, FIN_HistoryApproveECLAIMSearchModel>(request, model, ConstantSql.hrm_hr_sp_get_HistoryApprovedClaim);
        }

        public ActionResult ExportHistoryApproveECLAIMByTemplate([DataSourceRequest] DataSourceRequest request, FIN_HistoryApproveECLAIMSearchModel model)
        {
            string status = string.Empty;
            var baseService = new BaseService();
            var isDataTable = false;
            object obj = new FIN_HistoryApproveECLAIMModel();
            ListQueryModel lstModel = new ListQueryModel
            {
                PageSize = int.MaxValue - 1,
                PageIndex = 1,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            var result = baseService.GetData<FIN_HistoryApproveECLAIMModel>(lstModel, ConstantSql.hrm_hr_sp_get_HistoryApprovedClaim, UserLogin, ref status);
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new FIN_HistoryApproveECLAIMModel(),
                    FileName = "FIN_HistoryApproveECLAIM",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));
        }

    }
}