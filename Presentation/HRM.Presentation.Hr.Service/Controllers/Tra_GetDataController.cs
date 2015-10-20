using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using Kendo.Mvc.UI;
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
using System.Reflection;
using System.Collections;
using System.Data.SqlTypes;
using HRM.Business.Category.Models;
using HRM.Presentation.Category.Models;
using System.Data;
using HRM.Business.Category.Domain;
using HRM.Business.Training.Domain;
using HRM.Infrastructure.Utilities.Helper;
using System.Web.Script.Serialization;

using System.IO;
using HRM.Business.HrmSystem.Models;

namespace HRM.Presentation.Hr.Service.Controllers
{
    public class Tra_GetDataController : BaseController
    {
        string Hrm_Main_Web = System.Configuration.ConfigurationManager.AppSettings["Hrm_Main_Web"];
        BaseService baseService = new BaseService();
        string _status = string.Empty;
        public ActionResult Index()
        {
            return View();
        }

        #region Tra_Course
        [HttpPost]
        public ActionResult GetCourseList([DataSourceRequest] DataSourceRequest request, Tra_CourseSearchModel model)
        {
            return GetListDataAndReturn<Tra_CourseModel, Tra_CourseEntity, Tra_CourseSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Course);

        }

        public ActionResult ExportCourseListByTemplate([DataSourceRequest] DataSourceRequest request, Tra_CourseSearchModel model)
        {
            string status = string.Empty;
            var isDataTable = false;
            object obj = new Tra_CourseModel();
            var result = GetListData<Tra_CourseModel, Tra_CourseEntity, Tra_CourseSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Course, ref status);



            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Tra_CourseModel(),
                    FileName = "Tra_Course",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            var courseServices = new ActionService(UserLogin);
            var objCourse = new List<object>();
            objCourse.AddRange(new object[11]);
            objCourse[9] = 1;
            objCourse[10] = int.MaxValue - 1;
            var lstCourse = courseServices.GetData<Tra_CourseEntity>(objCourse, ConstantSql.hrm_tra_sp_get_Course, ref status).ToList();
            var orgServices = new ActionService(UserLogin);
            var objOrg = new List<object>();
            objOrg.AddRange(new object[5]);
            objOrg[3] = 1;
            objOrg[4] = int.MaxValue - 1;
            var lstOrg = orgServices.GetData<Cat_OrgStructureEntity>(objOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

            var jobTitleServices = new ActionService(UserLogin);
            var objJobtitle = new List<object>();
            objJobtitle.AddRange(new object[5]);
            objJobtitle[3] = 1;
            objJobtitle[4] = int.MaxValue - 1;
            var lstJobTitle = jobTitleServices.GetData<Cat_JobTitleEntity>(objJobtitle, ConstantSql.hrm_cat_sp_get_JobTitle, ref status).ToList();



            var lstResult = new List<Tra_CourseModel>();
            foreach (var item in result)
            {
                int[] orgForTrainee = new int[1];
                int[] orgForTrainer = new int[1];
                string[] jobTitleForTrainee = new string[1];
                string[] jobTitleForTrainer = new string[1];
                string[] courseForTrainee = new string[1];
                string[] courseForTrainer = new string[1];

                if (!string.IsNullOrEmpty(item.OrgListCode1))
                {
                    if (item.OrgListCode1.IndexOf(',') != -1)
                    {
                        orgForTrainee = item.OrgListCode1.Split(',').Select(s => int.Parse(s)).ToArray();
                    }
                    else
                    {
                        orgForTrainee[0] = int.Parse(item.OrgListCode1);
                    }

                }

                var lstOrgForTrainee = lstOrg.Where(s => orgForTrainee.Contains(s.OrderNumber)).ToList();
                item.OrgStructureNameForTrainee = string.Join(",", lstOrgForTrainee.Select(s => s.OrgStructureName));
                item.OrgStructureCodeForTrainee = string.Join(",", lstOrgForTrainee.Select(s => s.Code));

                if (!string.IsNullOrEmpty(item.JobTitleListCode1))
                {
                    if (item.JobTitleListCode1.IndexOf(',') != -1)
                    {
                        jobTitleForTrainee = item.JobTitleListCode1.Split(',').ToArray();
                    }
                    else
                    {
                        jobTitleForTrainee[0] = item.JobTitleListCode1;
                    }
                }

                var lstJobTitleForTrainee = lstJobTitle.Where(s => jobTitleForTrainee.Contains(s.Code)).ToList();
                item.JobTileNameForTrainee = string.Join(",", lstJobTitleForTrainee.Select(s => s.JobTitleName));

                if (!string.IsNullOrEmpty(item.CourseListID))
                {
                    if (item.CourseListID.IndexOf(',') != -1)
                    {
                        courseForTrainee = item.CourseListID.Split(',').ToArray();
                    }
                    else
                    {
                        courseForTrainee[0] = item.CourseListID;
                    }
                }
                var lstCourseForTrainee = lstCourse.Where(s => courseForTrainee.Contains(s.Code)).ToList();
                item.CourseNameForTrainee = string.Join(",", lstCourseForTrainee.Select(s => s.CourseName));

                if (!string.IsNullOrEmpty(item.OrgListTrainerCode))
                {
                    if (item.OrgListTrainerCode.IndexOf(',') != -1)
                    {
                        orgForTrainer = item.OrgListTrainerCode.Split(',').Select(s => int.Parse(s)).ToArray();
                    }
                    else
                    {
                        orgForTrainer[0] = int.Parse(item.OrgListTrainerCode);
                    }
                }
                var lstOrgForTrainer = lstOrg.Where(s => orgForTrainer.Contains(s.OrderNumber)).ToList();
                item.OrgStructureNameForTrainer = string.Join(",", lstOrgForTrainer.Select(s => s.OrgStructureName));
                item.OrgStructureCodeForTrainer = string.Join(",", lstOrgForTrainer.Select(s => s.Code));

                if (!string.IsNullOrEmpty(item.JobTitleListTrainerCode))
                {
                    if (item.JobTitleListTrainerCode.IndexOf(',') != -1)
                    {
                        jobTitleForTrainer = item.JobTitleListTrainerCode.Split(',').ToArray();
                    }
                    else
                    {
                        jobTitleForTrainer[0] = item.JobTitleListTrainerCode;
                    }
                }
                var lstJobTitleForTrainer = lstJobTitle.Where(s => jobTitleForTrainer.Contains(s.Code)).ToList();
                item.JobTileNameForTrainer = string.Join(",", lstJobTitleForTrainer.Select(s => s.JobTitleName));

                if (!string.IsNullOrEmpty(item.CourseListTrainerID))
                {
                    if (item.CourseListTrainerID.IndexOf(',') != -1)
                    {
                        courseForTrainer = item.CourseListTrainerID.Split(',').ToArray();
                    }
                    else
                    {
                        courseForTrainer[0] = item.CourseListTrainerID;
                    }
                }
                var lstCourseForTrainer = lstCourse.Where(s => courseForTrainer.Contains(s.Code)).ToList();
                item.CourseNameForTrainer = string.Join(",", lstCourseForTrainer.Select(s => s.CourseName));

                lstResult.Add(item);

            }



            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, lstResult, null, model.ExportType);
                return Json(fullPath);
            }

            return Json(result.ToDataSourceResult(request));



        }
        public JsonResult GetMultiRequirementTrain(string text)
        {
            return GetDataForControl<Tra_RequirementTrainModel, Tra_RequirementTrainEntity>(text, ConstantSql.hrm_tra_sp_get_RequirementTrain_Multi);
        }

        public JsonResult GetMultiTraCourse(string text)
        {
            return GetDataForControl<Tra_CourseMultiModel, Tra_CourseMultiEntity>(text, ConstantSql.hrm_tra_sp_get_Course_Multi);
        }

        public JsonResult GetMultiTraCostCenter(string text)
        {
            return GetDataForControl<Tra_CostCenterMultiModel, Tra_CostCenterMultiEntity>(text, ConstantSql.hrm_tra_sp_get_CostCenter_Multi);
        }

        public JsonResult GetCourseByCode(string courseCode)
        {
            if (courseCode != "")
                courseCode = courseCode.Replace(" ", "");
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            var result = service.GetData<Tra_CourseMultiEntity>(courseCode, ConstantSql.hrm_tra_sp_get_CourseByCode_Multi, ref status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDataByCourseID([DataSourceRequest] DataSourceRequest request, Guid CourseID)
        {
            var service = new ActionService(UserLogin);
            string status = string.Empty;
            var entity = service.GetByIdUseStore<Tra_CourseModel>(CourseID, ConstantSql.hrm_tra_sp_get_CourseById, ref status);

            if (entity.IsComplex == false)
            {
                entity.IsNormal = true;
            }
            if (entity.TypeHandleFomular == EnumDropDown.FormulaType.E_TYPEHANDLEFORMULA_AVERAGE.ToString())
            {
                entity.IsTypeHandleFomularAuto = true;
            }
            if (entity.TypeHandleFomular == EnumDropDown.FormulaType.E_TYPEHANDLEFORMULA_NORMAL.ToString())
            {
                entity.IsTypeHandleFomular = true;
            }

            var courseTopicServices = new ActionService(UserLogin);
            var objCourseTopic = new List<object>();
            objCourseTopic.Add(1);
            objCourseTopic.Add(int.MaxValue - 1);
            List<Tra_CourseTopicEntity> lstCourseTopic = courseTopicServices.GetData<Tra_CourseTopicEntity>(objCourseTopic, ConstantSql.hrm_tra_sp_get_CourseTopic, ref status);
            var lstCourseTopicByCourseID = lstCourseTopic.Where(s => s.CourseID == CourseID).ToList();
            var str = string.Empty;
            foreach (var item in lstCourseTopicByCourseID)
            {
                str += "[" + item.Code + "],";
            }
            if (!string.IsNullOrEmpty(str))
            {
                entity.strTopicCode = str.Substring(0, str.Length - 1);
            }
            return Json(entity, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExportCourseSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_CourseEntity, Tra_CourseModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_CourseByIds);
        }
        public ActionResult ExportAllCourseList([DataSourceRequest] DataSourceRequest request, Tra_CourseSearchModel model)
        {
            return ExportAllAndReturn<Tra_CourseEntity, Tra_CourseModel, Tra_CourseSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Course);
        }

        #endregion

        #region Tra_Certificate
        [HttpPost]
        public ActionResult GetCertificateList([DataSourceRequest] DataSourceRequest request, Tra_CertificateSearchModel model)
        {
            return GetListDataAndReturn<Tra_CertificateModel, Tra_CertificateEntity, Tra_CertificateSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Certificate);

        }

        public JsonResult GetMultiCertificate(string text)
        {
            return GetDataForControl<Tra_CertificateMultiModel, Tra_CertificateMultiEntity>(text, ConstantSql.hrm_tra_sp_get_Certificate_Multi);
        }
        public JsonResult GetCertificateByCourseID(Guid CourseID)
        {
            var result = new List<Tra_CertificateMultiModel>();

            string status = string.Empty;
            if (CourseID != Guid.Empty)
            {
                var service = new ActionService(UserLogin);
                result = service.GetData<Tra_CertificateMultiModel>(CourseID, ConstantSql.hrm_tra_sp_get_CertificateByCourseID, ref status);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetMultiCertificateByCourseID(Guid ClassID)
        {
            var result = new List<Tra_CertificateMultiModel>();

            string status = string.Empty;
            if (ClassID != Guid.Empty)
            {
                var classService = new ActionService(UserLogin);
                var service = new ActionService(UserLogin);
                var classEntity = classService.GetData<Tra_ClassEntity>(ClassID, ConstantSql.hrm_tra_sp_get_ClassById, ref status).FirstOrDefault();
                if (classEntity != null)
                {
                    result = service.GetData<Tra_CertificateMultiModel>(classEntity.CourseID, ConstantSql.hrm_tra_sp_get_CertificateByCourseID, ref status);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMultiTopicByCourseID(Guid CourseID)
        {
            var result = new List<Tra_ClassModel>();

            string status = string.Empty;
            if (CourseID != Guid.Empty)
            {
                var service = new ActionService(UserLogin);
                result = service.GetData<Tra_ClassModel>(CourseID, ConstantSql.hrm_tra_sp_get_TopicByCourseID_Multi, ref status);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCertificateList([DataSourceRequest] DataSourceRequest request, List<Guid> lstTraineeID)
        {
            var classService = new ActionService(UserLogin);
            string status = string.Empty;
            if (lstTraineeID != null)
            {
                foreach (var trainee in lstTraineeID)
                {
                    var traineeEntity = classService.GetData<Tra_TraineeEntity>(trainee, ConstantSql.hrm_tra_sp_get_TraineeById, ref status).FirstOrDefault();
                }
            }
            return null;

        }

        [HttpPost]
        public ActionResult ExportCertificateList([DataSourceRequest] DataSourceRequest request, Tra_CertificateSearchModel model)
        {
            return ExportAllAndReturn<Tra_CertificateEntity, Tra_CertificateModel, Tra_CertificateSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Certificate);
        }
        #endregion

        #region Tra_TrainerMailReminder
        [HttpPost]
        public ActionResult GetTrainerMailReminderList([DataSourceRequest] DataSourceRequest request, Tra_TrainerMailReminderSearchModel model)
        {
            return GetListDataAndReturn<Tra_TrainerMailReminderModel, Tra_TrainerMailReminderEntity, Tra_TrainerMailReminderSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TrainerMailReminder);

        }

        [HttpPost]
        public ActionResult ExportTrainerMailReminderList([DataSourceRequest] DataSourceRequest request, Tra_TrainerMailReminderSearchModel model)
        {
            return ExportAllAndReturn<Tra_TrainerMailReminderEntity, Tra_TrainerMailReminderModel, Tra_TrainerMailReminderSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TrainerMailReminder);
        }
        #endregion

        #region Tra_ScoreType
        [HttpPost]
        public ActionResult GetScoreTypeList([DataSourceRequest] DataSourceRequest request, Tra_ScoreTypeSearchModel model)
        {
            return GetListDataAndReturn<Tra_ScoreTypeModel, Tra_ScoreTypeEntity, Tra_ScoreTypeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ScoreType);

        }

        public JsonResult GetMultiScoreType(string text)
        {
            return GetDataForControl<Tra_ScoreTypeMultiModel, Tra_ScoreTypeMultiEntity>(text, ConstantSql.hrm_tra_sp_get_ScoreType_Multi);
        }

        public ActionResult ExportAllScoreTypeList([DataSourceRequest] DataSourceRequest request, Tra_ScoreTypeSearchModel model)
        {
            return ExportAllAndReturn<Tra_ScoreTypeEntity, Tra_ScoreTypeModel, Tra_ScoreTypeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ScoreType);
        }
        public ActionResult ExportScoreTypeSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_ScoreTypeEntity, Tra_ScoreTypeModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_ScoreTypeByIds);
        }
        #endregion

        #region Tra_TraineeFinishInside
        public ActionResult GetTraineeFinishInsideList([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {
            return GetListDataAndReturn<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeFinishInside);
        }
        public ActionResult ExportTraineeFinishInsideSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_TraineeEntity, Tra_TraineeModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_TraineeFinishInsideIds);
        }
        public ActionResult ExportAllTraineeFinishInsideList([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {
            return ExportAllAndReturn<Tra_TraineeEntity, Tra_TraineeModel, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeFinishInside);
        }
        #endregion

        #region Tra_TraineeRegister
         [HttpPost]
        public ActionResult GetTraineeRegisterList([DataSourceRequest] DataSourceRequest request, Tra_TraineeRegisterSearchModel model)
        {
            var status = string.Empty;
            var traineeServices = new ActionService(UserLogin);
            var objTrainee = new List<object>();
            objTrainee.AddRange(new object[12]);
            objTrainee[1] = model.CodeEmp;
            objTrainee[2] = model.DateFrom;
            objTrainee[3] = model.DateTo;
            objTrainee[7] = model.OrgStructureID;
            objTrainee[8] = model.ProfileName;
            objTrainee[9] = model.Status;
            objTrainee[10] = 1;
            objTrainee[11] = int.MaxValue -1 ;
            var result = traineeServices.GetData<Tra_TraineeRegisterEntity>(objTrainee,ConstantSql.hrm_tra_sp_get_TraineeRegister,ref status).ToList().Translate<Tra_TraineeRegisterModel>();
            Guid[] classID = null;
            if(!string.IsNullOrEmpty(model.ClassID))
            {
                classID = model.ClassID.Split(',').Select(s =>Guid.Parse(s)).ToArray();
                if (classID != null)
                {
                    result = result.Where(s => s.ClassID != null && classID.Contains(s.ClassID.Value)).ToList();
                }
            }
        
            return Json(result.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
           // var result = GetListData<Tra_TraineeModel>();
           // return GetListDataAndReturn<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Trainee);
        }
        #endregion

        #region Tra_Trainee
         [HttpPost]
         public ActionResult GetTraineeList([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel1 model)
         {
             var services = new ActionService(UserLogin);
             var traineeServices = new Tra_TraineeServices();
             string status = string.Empty;
             var objTrainee = new List<object>();
             objTrainee.AddRange(new object[15]);
             objTrainee[0] = model.ClassID;
             objTrainee[1] = model.CodeEmp;
             objTrainee[2] = model.CourseID;
             objTrainee[3] = model.DateFrom;
             objTrainee[4] = model.DateTo;
             objTrainee[8] = model.OrgStructureID;
             objTrainee[9] = model.ProfileName;
             objTrainee[10] = model.Status;
             objTrainee[11] = model.RequirementTrainName;
             objTrainee[12] = null;
             objTrainee[13] = 1;
             objTrainee[14] = int.MaxValue - 1;
             var result = services.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, ref status);
             
             if (model.RequirementTrainID != null)
             {
                List<Guid> _RequirementTrainIDs = model.RequirementTrainID.Split(',').Select(s => Guid.Parse(s)).ToList();
                result = traineeServices.GetListTrainees(result, _RequirementTrainIDs);
             }

             return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
         }

        [HttpPost]
        public ActionResult GetTraineeForAddList([DataSourceRequest] DataSourceRequest request, Tra_TraineeForAddSearchModel model)
        {
            return GetListDataAndReturn<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeForAddSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeForAdd);
        }

       // [HttpPost]
        public ActionResult ExportTraineeList([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel1 model)
        {
            return ExportAllAndReturn<Tra_TraineeEntity, Tra_TraineeModel, Tra_TraineeSearchModel1>(request, model, ConstantSql.hrm_tra_sp_get_Trainee);
        }

        [HttpPost]
        public ActionResult GetTraineeFinishList([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {
            return GetListDataAndReturn<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeFinish);
        }

        [HttpPost]
        public ActionResult GetTraineeProgressList([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {
            return GetListDataAndReturn<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeProgress);
        }

        [HttpPost]
        public ActionResult GetTraineePassList([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {
            //return GetListDataAndReturn<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineePass);
            var status = string.Empty;
            var traineeServices = new ActionService(UserLogin);
            var objTrainee = new List<object>();
            objTrainee.AddRange(new object[12]);
            objTrainee[0] = model.ProfileName;
            objTrainee[1] = model.CodeEmp;
            objTrainee[2] = model.OrgStructureID;
            objTrainee[4] = model.DateFrom;
            objTrainee[5] = model.DateTo;
            objTrainee[10] = 1;
            objTrainee[11] = int.MaxValue - 1;
            var result = traineeServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_TraineePass, ref status).ToList().Translate<Tra_TraineeModel>();
            Guid[] classID = null;
            if (!string.IsNullOrEmpty(model.ClassID))
            {
                classID = model.ClassID.Split(',').Select(s => Guid.Parse(s)).ToArray();
                if (classID != null)
                {
                    result = result.Where(s => classID.Contains(s.ClassID)).ToList();
                }
            }
            Guid[] courseID = null;
            if (!string.IsNullOrEmpty(model.CourseID))
            {
                courseID = model.CourseID.Split(',').Select(s => Guid.Parse(s)).ToArray();
                if (courseID != null)
                {
                    result = result.Where(s => courseID.Contains(s.CourseID)).ToList();
                }
            }
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        
        }
        public ActionResult ExportAllTraineePassList([DataSourceRequest]DataSourceRequest request,Tra_TraineeSearchModel model)
        {
            return ExportAllAndReturn<Tra_TraineeEntity, Tra_TraineeModel, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineePass);
        }

        [HttpPost]
        public ActionResult GetTraineeFailList([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {
            return GetListDataAndReturn<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeFail);
        }
        public ActionResult ExportTraineeFailList([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {
          return  ExportAllAndReturn<Tra_TraineeEntity,Tra_TraineeModel,Tra_TraineeSearchModel>(request,model,ConstantSql.hrm_tra_sp_get_TraineeFail);
        }

        public ActionResult ExportTraineeFailListByTemplate([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {

            string status = string.Empty;
            var isDataTable = false;
            object obj = new Tra_TraineeModel();
            var result = GetListData<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeFail, ref status);

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Tra_TraineeModel(),
                    FileName = "Tra_Trainee",
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

        [HttpPost]
        public ActionResult GetTraineeWithOutProcess([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {
            return GetListDataAndReturn<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeWithOutProcess);
        }

        public ActionResult GetTraineeExpiredCertificateList([DataSourceRequest] DataSourceRequest request, Tra_TraineeCertificateExpireSearchModel model)
        {
            var services = new Tra_TraineeCertificateServices();
            int valueConfigFrom = services.DateWarningExpireCertificateFrom();
            int valueConfigTo = services.DateWarningExpireCertificateTo();
            DateTime FromDay = DateTime.Now;
            DateTime ToDay = DateTime.Now;
            if (valueConfigFrom != null)
            {
                FromDay = FromDay.AddDays(-valueConfigFrom);
            }
            if (valueConfigTo != null)
            {
                ToDay = ToDay.AddDays(valueConfigTo);
            }
            model.DateExpireCertificateFrom = FromDay;
            model.DateExpireCertificateTo = ToDay;
            return GetListDataAndReturn<Tra_TraineeCertificateModel, Tra_TraineeCertificateEntity, Tra_TraineeCertificateExpireSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeExpiredCertificate);
        }

        public ActionResult ExportTraineeExpiredCertificate([DataSourceRequest] DataSourceRequest request, Tra_TraineeCertificateExpireSearchModel model)
        {
            var services = new Tra_TraineeCertificateServices();
            int valueConfigFrom = services.DateWarningExpireCertificateFrom();
            int valueConfigTo = services.DateWarningExpireCertificateTo();
            DateTime FromDay = DateTime.Now;
            DateTime ToDay = DateTime.Now;
            if (valueConfigFrom != null)
            {
                FromDay = FromDay.AddDays(-valueConfigFrom);
            }
            if (valueConfigTo != null)
            {
                ToDay = ToDay.AddDays(valueConfigTo);
            }
            model.DateExpireCertificateFrom = FromDay;
            model.DateExpireCertificateTo = ToDay;
            return ExportAllAndReturn<Tra_TraineeCertificateEntity, Tra_TraineeCertificateModel, Tra_TraineeCertificateExpireSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeExpiredCertificate);
        }

        public ActionResult ExportTraineeListByTemplate([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel1 model)
        {

            string status = string.Empty;
            var isDataTable = false;
            object obj = new Tra_TraineeModel();
            request.Page = 1;
            request.PageSize = int.MaxValue - 1;
            var result = GetListData<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel1>(request, model, ConstantSql.hrm_tra_sp_get_Trainee, ref status);
            //Guid[] classID = null;
            //if (!string.IsNullOrEmpty(model.ClassID.Value))
            //{
            //    classID = model.ClassID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            //    if (classID != null)
            //    {
            //        result = result.Where(s => classID.Contains(s.ClassID)).ToList();
            //    }
            //}
            //Guid[] courseID = null;
            //if (!string.IsNullOrEmpty(model.CourseID.Value))
            //{
            //    courseID = model.CourseID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            //    if (courseID != null)
            //    {
            //        result = result.Where(s => courseID.Contains(s.CourseID)).ToList();
            //    }
            //}
            //Guid[] RequirementTrainIDs = null;
            //if (!string.IsNullOrEmpty(model.RequirementTrainID))
            //{
            //    RequirementTrainIDs = model.RequirementTrainID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            //    result = result.Where(s => s.RequirementTrainID != null && RequirementTrainIDs.Contains((Guid)s.RequirementTrainID)).ToList();
            //}
            var listModel = new List<Tra_TraineeModel>();
            var hreServiceProfile = new Hre_ProfileServices();
            var lstProfile = hreServiceProfile.GetProfileNameAll();
            foreach (var item in result)
            {
                if (item.TrainerOtherList != null)
                {
                    string _strTrainerOtherList = item.TrainerOtherList.Replace(" ", "");
                    string[] lstitem = _strTrainerOtherList.Split(',').ToArray();
                    if (lstProfile != null && lstitem != null)
                    {
                        List<Hre_ProfileEntity> lstitemprofile = lstProfile.Where(s => lstitem.Contains(s.CodeEmp)).ToList();
                        string Stringresult = string.Empty;
                        foreach (var itemstring in lstitemprofile)
                        {
                            Stringresult += itemstring.ProfileName + ", ";
                        }
                        if (!string.IsNullOrEmpty(Stringresult))
                        {
                            Stringresult = Stringresult.Substring(0, Stringresult.Length - 2);
                        }
                        item.TeacherName = Stringresult;
                    }
                }
                var newModle = (Tra_TraineeModel)typeof(Tra_TraineeModel).CreateInstance();
                foreach (var property in item.GetType().GetProperties())
                {
                    newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                }
                
                listModel.Add(newModle);
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Tra_TraineeModel(),
                    FileName = "Tra_Trainee",
                    OutPutPath = path,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            if (model.ExportId != Guid.Empty)
            {
                var fullPath = ExportService.Export(model.ExportId, listModel, null, model.ExportType);
                return Json(fullPath);
            }

            return Json(listModel.ToDataSourceResult(request));
        }

        public ActionResult ExportTraineePassListByTemplate([DataSourceRequest] DataSourceRequest request, Tra_TraineeSearchModel model)
        {

            string status = string.Empty;
            var isDataTable = false;
            object obj = new Tra_TraineeModel();
            var result = GetListData<Tra_TraineeModel, Tra_TraineeEntity, Tra_TraineeSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineePass, ref status);
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = new Tra_TraineeModel(),
                    FileName = "Tra_Trainee",
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
        #endregion

        #region Tra_RequirementTrain
        [HttpPost]

        public ActionResult GetRequirementTrainList([DataSourceRequest] DataSourceRequest request, Tra_RequirementTrainSearchModel model)
        {
            model.PlanIDs = Common.DotNetToOracle(model.PlanIDs);
            return GetListDataAndReturn<Tra_RequirementTrainModel, Tra_RequirementTrainEntity, Tra_RequirementTrainSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_RequirementTrain);
        }
        public ActionResult ExportRequirementTrainSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_RequirementTrainEntity, Tra_RequirementTrainModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_RequirementTrainIds);
        }
        public ActionResult ExportAllRequirementTrainList([DataSourceRequest] DataSourceRequest request, Tra_RequirementTrainSearchModel model)
        {
            return ExportAllAndReturn<Tra_RequirementTrainEntity, Tra_RequirementTrainModel, Tra_RequirementTrainSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_RequirementTrain);
        }
        #endregion

        #region Tra_RequirementTrainInSide
        [HttpPost]

        public ActionResult GetRequirementTrainInSideList([DataSourceRequest] DataSourceRequest request, Tra_RequirementTrainInSideSearchModel model)
        {
            return GetListDataAndReturn<Tra_RequirementTrainModel, Tra_RequirementTrainEntity, Tra_RequirementTrainInSideSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_RequirementTrainInSide);
        }

        public ActionResult ExportAllRequirementTrainInSideList([DataSourceRequest] DataSourceRequest request, Tra_RequirementTrainInSideSearchModel model)
        {
            return ExportAllAndReturn<Tra_RequirementTrainEntity, Tra_RequirementTrainModel, Tra_RequirementTrainInSideSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_RequirementTrainInSide);
        }
        #endregion

        #region Tra_PlanInside

        public ActionResult GetPlanInsideList([DataSourceRequest] DataSourceRequest request, Tra_PlanSearchModel model)
        {
            return GetListDataAndReturn<Tra_PlanModel, Tra_PlanEntity, Tra_PlanSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_PlanInside);
        }
        public ActionResult ExportPlanInsideSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_PlanEntity, Tra_PlanModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_PlanIds);
        }
        public ActionResult ExportAllPlanInsideList([DataSourceRequest] DataSourceRequest request, Tra_PlanSearchModel model)
        {
            return ExportAllAndReturn<Tra_PlanEntity, Tra_PlanModel, Tra_PlanSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_PlanInside);
        }
        #endregion

        #region Tra_PlanOutside
        public ActionResult GetPlanOutsideList([DataSourceRequest] DataSourceRequest request, Tra_PlanSearchModel model)
        {
            return GetListDataAndReturn<Tra_PlanModel, Tra_PlanEntity, Tra_PlanSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_PlanOutside);
        }
        public ActionResult ExportPlanOutsideSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_PlanEntity, Tra_PlanModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_PlanIds);
        }
        public ActionResult ExportAllPlanOutsideList([DataSourceRequest] DataSourceRequest request, Tra_PlanSearchModel model)
        {
            return ExportAllAndReturn<Tra_PlanEntity, Tra_PlanModel, Tra_PlanSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_PlanOutside);
        }
        #endregion

   
        
        #region Tra_Plan
        public JsonResult GetMultiPlanName(string text)
        {
            return GetDataForControl<Tra_PlanMultiModel, Tra_PlanEntity>(text, ConstantSql.hrm_tra_sp_get_PlanName_Multi);
        }
        public ActionResult GetPlanList([DataSourceRequest] DataSourceRequest request, Tra_PlanSearchModel model)
        {
            return GetListDataAndReturn<Tra_PlanModel, Tra_PlanEntity, Tra_PlanSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Plan);
        }
        public ActionResult ExportPlanSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_PlanEntity, Tra_PlanModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_PlanIds);
        }
        public ActionResult ExportAllPlanList([DataSourceRequest] DataSourceRequest request, Tra_PlanSearchModel model)
        {
            return ExportAllAndReturn<Tra_PlanEntity, Tra_PlanModel, Tra_PlanSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Plan);
        }

        public ActionResult GetPlanDetailyPlanID([DataSourceRequest] DataSourceRequest request, Guid planID)
        {
            string status = string.Empty;

            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(planID);
            var result = baseService.GetData<Tra_PlanDetailModel>(objs, ConstantSql.hrm_tra_sp_get_PlanDetailByPlanId, ref status);
            if (result != null)

                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }

        public ActionResult GetClassByPlanID([DataSourceRequest] DataSourceRequest request, Guid planID)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(planID);
            var result = baseService.GetData<Tra_ClassModel>(objs, ConstantSql.hrm_tra_sp_get_ClassByPlanId, ref status);
            if (result != null)
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }
        public ActionResult GetRequirementTrainByPlanID([DataSourceRequest] DataSourceRequest request, Guid planID)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(planID);
            var result = baseService.GetData<Tra_RequirementTrainModel>(objs, ConstantSql.hrm_tra_sp_get_RmTrainByPlanId, ref status);
            if (result != null)
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            return null;
        }
        #endregion

        #region Tra_PlanDetail
        public ActionResult GetPlanDetailList([DataSourceRequest] DataSourceRequest request, Tra_PlanDetailSearchModel model)
        {
            return GetListDataAndReturn<Tra_PlanDetailModel, Tra_PlanDetailEntity, Tra_PlanDetailSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_PlanDetail);
        }

        #endregion

        #region Tra_Class
        #region Lớp học
        [HttpPost]
        public ActionResult GetClassList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            var services = new ActionService(UserLogin);
            var traineeServices = new Tra_TraineeServices();
            var objTrainee = new List<object>();
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            Guid[] _RequirementTrainIDs=null;
            if (!string.IsNullOrEmpty(model.RequirementTrainID))
            {
                _RequirementTrainIDs = model.RequirementTrainID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            ListQueryModel lstModel = new ListQueryModel
            {
                PageIndex = request.Page,
                PageSize = request.PageSize,
                Filters = ExtractFilterAttributes(request),
                Sorts = ExtractSortAttributes(request),
                AdvanceFilters = ExtractAdvanceFilterAttributes(model)
            };
            //List<object> paraprofile = new List<object>();
            //paraprofile.AddRange(new object[17]);
            //paraprofile[15] = 1;
            //paraprofile[16] = int.MaxValue - 1;
            //var lstProfile = service.GetData<Hre_ProfileEntity>(paraprofile, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status);

            var hreServiceProfile = new Hre_ProfileServices();
            var lstProfile = hreServiceProfile.GetProfileNameAll();

            var listEntity = service.GetData<Tra_ClassEntity>(lstModel, ConstantSql.hrm_tra_sp_get_Class, ref status);
            
            
            if (_RequirementTrainIDs != null)
            {
                List<object> paraprofile = new List<object>();
                paraprofile.AddRange(new object[7]);
                paraprofile[5] = 1;
                paraprofile[6] = int.MaxValue - 1;
                var ListRequirementTrain = service.GetData<Tra_RequirementTrainEntity>(paraprofile, ConstantSql.hrm_tra_sp_get_RequirementTrain, ref status);
               // var _lstRequirementTrainByPlanID = new List<Guid>();
              //  _lstRequirementTrainByPlanID = ListRequirementTrain.Where(s => s.ID != null && _RequirementTrainIDs.Contains(s.ID)).Select(s => (Guid)s.PlanID).ToList();
                var result = services.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, ref status);
                List<Guid> _RequirementTrainIDs1 = model.RequirementTrainID.Split(',').Select(s => Guid.Parse(s)).ToList();
                result = traineeServices.GetListTrainees(result, _RequirementTrainIDs1);
                //if (listEntity != null)
                //{
                //    listEntity = listEntity.Where(s => s.PlanID != null && _lstRequirementTrainByPlanID.Contains((Guid)s.PlanID)).ToList();
                //}
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
               
            }
            var listModel = new List<Tra_ClassModel>();
            if (listEntity != null)
            {
                request.Page = 1;
                foreach (var item in listEntity)
                {
                    if (item.TrainerOtherList != null)
                    {
                        string _strTrainerOtherList = item.TrainerOtherList.Replace(" ", "");
                        string[] lstitem = _strTrainerOtherList.Split(',').ToArray();
                        if(lstProfile !=null && lstitem!=null)
                        {
                            List<Hre_ProfileEntity> lstitemprofile = lstProfile.Where(s => lstitem.Contains(s.CodeEmp)).ToList();
                            string Stringresult = string.Empty;
                            foreach (var itemstring in lstitemprofile)
                            {
                                Stringresult += itemstring.ProfileName + ", ";
                            }
                            if (!string.IsNullOrEmpty(Stringresult))
                            {
                                Stringresult = Stringresult.Substring(0, Stringresult.Length - 2);
                            }
                           
                            item.TeacherName = Stringresult;
                        }
                    }
                    var newModle = (Tra_ClassModel)typeof(Tra_ClassModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }
                var dataSourceResult = listModel.ToDataSourceResult(request);
                if (listModel.FirstOrDefault().GetPropertyValue("TotalRow") != null)
                {
                    dataSourceResult.Total = listModel.Count() <= 0 ? 0 : (int)listModel.FirstOrDefault().GetPropertyValue("TotalRow");
                }
                return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
            }
            var listModelNull = new List<Tra_ClassModel>();
            ModelState.AddModelError("Id", status);
            return Json(listModelNull.ToDataSourceResult(request, ModelState));

            //return GetListDataAndReturn<Tra_ClassModel, Tra_ClassEntity, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Class);
        }


        public ActionResult ExportClassSelected(string selectedIds, string valueFields)
        {
            //return ExportSelectedAndReturn<Tra_ClassEntity, Tra_ClassModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_ClassByIds);
            selectedIds = Common.DotNetToOracle(selectedIds);
            var service = new ActionService(UserLogin);
            var status = string.Empty;
            var listModel = new List<Tra_ClassModel>();
            var listEntity = service.GetData<Tra_ClassEntity>(selectedIds, ConstantSql.hrm_tra_sp_get_ClassByIds, ref status);
            if (listEntity != null && status == NotificationType.Success.ToString())
            {
                var hreServiceProfile = new Hre_ProfileServices();
                var lstProfile = hreServiceProfile.GetProfileNameAll();
                foreach (var item in listEntity)
                {
                    if (item.TrainerOtherList != null)
                    {
                        string[] lstitem = item.TrainerOtherList.Split(',').ToArray();
                        if (lstProfile != null && lstitem != null)
                        {
                            List<Hre_ProfileEntity> lstitemprofile = lstProfile.Where(s => lstitem.Contains(s.CodeEmp)).ToList();
                            string Stringresult = string.Empty;
                            foreach (var itemstring in lstitemprofile)
                            {
                                Stringresult += itemstring.ProfileName + ',';
                            }
                            Stringresult = Stringresult.Substring(0, Stringresult.Length - 1);
                            item.TeacherName = Stringresult;
                        }
                    }
                    var newModle = (Tra_ClassModel)typeof(Tra_ClassModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }
            //    listModel = listEntity.Translate<Tra_ClassModel>();
                Guid _templateID = Guid.Empty;
                status = ExportService.Export(_templateID, listModel, valueFields.Split(','), null);
            }
            return Json(status);
        }
        public ActionResult ExportAllClassList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            var service = new ActionService(UserLogin);
            model.SetPropertyValue("IsExport", true);
            string fullPath = string.Empty, status = string.Empty;
            var listEntity = GetListData<Tra_ClassModel, Tra_ClassEntity, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Class, ref status);
            var listModel = new List<Tra_ClassModel>();
            List<object> paraprofile = new List<object>();
            paraprofile.AddRange(new object[17]);
            paraprofile[15] = 1;
            paraprofile[16] = int.MaxValue - 1;
            var lstProfile = service.GetData<Hre_ProfileEntity>(paraprofile, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status);
            if (listEntity != null)
            {
                foreach (var item in listEntity)
                {
                    if (item.TrainerOtherList != null)
                    {
                        string[] lstitem = item.TrainerOtherList.Split(',').ToArray();
                        if (lstProfile != null && lstitem != null)
                        {
                            List<Hre_ProfileEntity> lstitemprofile = lstProfile.Where(s => lstitem.Contains(s.CodeEmp)).ToList();
                            string Stringresult = string.Empty;
                            foreach (var itemstring in lstitemprofile)
                            {
                                Stringresult += itemstring.ProfileName + ',';
                            }
                            Stringresult = Stringresult.Substring(0, Stringresult.Length - 1);
                            item.TeacherName = Stringresult;
                        }
                    }
                    var newModle = (Tra_ClassModel)typeof(Tra_ClassModel).CreateInstance();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        newModle.SetPropertyValue(property.Name, item.GetPropertyValue(property.Name));
                    }
                    listModel.Add(newModle);
                }
            }
            if (status == NotificationType.Success.ToString())
            {
                status = ExportService.Export(listModel, model.GetPropertyValue("ValueFields").TryGetValue<string>().Split(','));
            }
            return Json(status);
            //return ExportAllAndReturn<Tra_ClassEntity, Tra_ClassModel, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_Class);
        }


        public ActionResult GetDataByClassID(string classID)
        {
            string status = string.Empty;
            Tra_ClassModel result = new Tra_ClassModel();
            if (classID != null && classID != string.Empty)
            {
                ActionService Base = new ActionService(UserLogin);
                result = Base.GetData<Tra_ClassModel>(Common.DotNetToOracle(classID), ConstantSql.hrm_tra_sp_get_ClassById, ref status).FirstOrDefault();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion
 



        #region Lớp học đang học
        public ActionResult GetClassProcessList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            return GetListDataAndReturn<Tra_ClassModel, Tra_ClassEntity, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ClassProcess);
        }
        public ActionResult ExportAllClassProcessList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            return ExportAllAndReturn<Tra_ClassEntity, Tra_ClassModel, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ClassProcess);
        }

        public ActionResult ExportClassProcessSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_ClassEntity, Tra_ClassModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_ClassProcessByIds);
        }
        #endregion

        #region Lớp học đang học bên trong
        [HttpPost]
        public ActionResult GetClassProcessInSideList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            return GetListDataAndReturn<Tra_ClassModel, Tra_ClassEntity, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ClassProcessInSide);
        }

        public ActionResult ExportClassProcessInSideList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            return ExportAllAndReturn<Tra_ClassEntity, Tra_ClassModel, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ClassProcessInSide);
        }
        public ActionResult ExportClassProcessInSideSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_ClassEntity, Tra_ClassModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_ClassProcessInSideByIds);
        }
        #endregion

        #region Lớp học đã học

        [HttpPost]
        public ActionResult GetClassFinishList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            return GetListDataAndReturn<Tra_ClassModel, Tra_ClassEntity, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ClassFinish);
        }

        public ActionResult ExportClassFinishList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            return ExportAllAndReturn<Tra_ClassEntity, Tra_ClassModel, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ClassFinish);
        }
        public ActionResult ExportClassFinishSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_ClassEntity, Tra_ClassModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_ClassFinishByIds);
        }

        #endregion

        #region Lớp học đã học bên trong
        [HttpPost]
        public ActionResult GetClassFinishInSideList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            return GetListDataAndReturn<Tra_ClassModel, Tra_ClassEntity, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ClassInSide);
        }

        public ActionResult ExportClassFinishInSideList([DataSourceRequest] DataSourceRequest request, Tra_ClassSearchModel model)
        {
            return ExportAllAndReturn<Tra_ClassEntity, Tra_ClassModel, Tra_ClassSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_ClassInSide);
        }

        public ActionResult ExportClassFinishInSideSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_ClassEntity, Tra_ClassModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_ClassInSideByIds);
        }

        #endregion


        public JsonResult GetMultiClass(string text)
        {
            return GetDataForControl<Tra_ClassMultiModel, Tra_ClassMultiEntity>(text, ConstantSql.hrm_tra_sp_get_Class_Multi);
        }
        public ActionResult GetTraineeByClassIDList([DataSourceRequest] DataSourceRequest request, Guid ClassID)
        {
            
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var traineeCertificateServices = new Tra_TraineeCertificateServices();
            var objTraineeCertificate = new List<object>();
            objTraineeCertificate.Add(1);
            objTraineeCertificate.Add(int.MaxValue - 1);
            var lstTraineeCertificate = baseService.GetData<Tra_TraineeCertificateEntity>(objTraineeCertificate, ConstantSql.hrm_tra_sp_get_TraineeCertificate, ref status).ToList();
            var objs = new List<object>();
            objs.Add(ClassID);
            var lstresult = baseService.GetData<Tra_TraineeEntity>(objs, ConstantSql.hrm_cat_sp_get_TraineeByClassID, ref status);
            var lstTrainee = new List<Tra_TraineeEntity>();
            foreach (var item in lstresult)
            {
                var traineeEntity = new Tra_TraineeEntity();
                traineeEntity = item.CopyData<Tra_TraineeEntity>();
                var traineeCertificate = lstTraineeCertificate.Where(s => item.ID == s.TraineeID.Value).FirstOrDefault();
                if (traineeCertificate == null)
                {
                    traineeEntity.CertificateName = string.Empty;
                    traineeEntity.DateCertificate = null;
                    traineeEntity.DateExpireCertificate = null;
                }
                lstTrainee.Add(traineeEntity);
            }

            Tra_ReportServices service = new Tra_ReportServices();
            DataTable result = service.GetTraineeByClassID(lstresult, false);

            return Json(result.ToDataSourceResult(request));
        }


        #endregion

        #region Tra_CourseTopic

        [HttpPost]
        public ActionResult AddTopicToCourse(Guid CourseID, Guid TopicID)
        {
            var model = new Tra_CourseTopicModel();
            string status = string.Empty;
            if (CourseID != Guid.Empty && TopicID != Guid.Empty)
            {
                model.CourseID = CourseID;
                model.TopicID = TopicID;
            }
            #region Validate
            string message = string.Empty;

            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_CourseTopicModel>(model, "Tra_CourseTopic", ref message);
            if (!checkValidate)
            {

                model.ActionStatus = ConstantMessages.TopicIsExistedInCourse.ToString();
                return Json(model.ActionStatus, JsonRequestBehavior.AllowGet);
            }
            #endregion

            var courseServices = new Tra_CourseServices();
            var courseTopicServices = new Tra_CourseTopicServices();
            var entity = new Tra_CourseTopicEntity();
            if (model != null)
            {
            
                entity = model.CopyData<Tra_CourseTopicEntity>();
                message = courseTopicServices.Add(entity);
                return Json(message, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpPost]
        public ActionResult UpdateScoreInTraineeTopic(Dictionary<string, object> model)
        {
            string status = string.Empty;
            string message = string.Empty;
            var DictraineeID = model.ElementAt(0);
            string[] traineeID = (string[])DictraineeID.Value;
            // var entity = new Tra_TraineeTopicEntity();
            var lstEntity = new List<Tra_TraineeTopicEntity>();
            var topicServices = new ActionService(UserLogin);
            var objTopic = new List<object>();
            objTopic.Add(null);
            objTopic.Add(null);
            objTopic.Add(null);
            objTopic.Add(null);
            objTopic.Add(1);
            objTopic.Add(int.MaxValue - 1);
            var lstTopic = topicServices.GetData<Cat_TopicEntity>(objTopic, ConstantSql.hrm_cat_sp_get_Topic, ref status).ToList();

            var traineeScoreServices = new Tra_TraineeScoreServices();
            var objTraineeScore = new List<object>();
            objTraineeScore.Add(1);
            objTraineeScore.Add(int.MaxValue - 1);
            var lstTraineeScore = topicServices.GetData<Tra_TraineeScoreEntity>(objTraineeScore, ConstantSql.hrm_tra_sp_get_TraineeScore, ref status).ToList();

            var scoreTopicServices = new ActionService(UserLogin);
            var objScoreTopic = new List<object>();
            objScoreTopic.Add(1);
            objScoreTopic.Add(int.MaxValue - 1);
            var lstScoreTopic = scoreTopicServices.GetData<Tra_ScoreTopicEntity>(objScoreTopic, ConstantSql.hrm_tra_sp_get_ScoreTopic, ref status).ToList();

            var traineeTopicServices = new Tra_TraineeTopicServices();
            var objTraineeTopic = new List<object>();
            objTraineeTopic.Add(1);
            objTraineeTopic.Add(int.MaxValue - 1);
            var lstTraineeTopic = topicServices.GetData<Tra_TraineeTopicEntity>(objTraineeTopic, ConstantSql.hrm_tra_sp_get_TraineeTopic, ref status).ToList();
            if (model.Count > 0)
            {
                for (int i = 3; i < model.Count; i++)
                {
                    var entity = new Tra_TraineeTopicEntity();
                    var item = model.ElementAt(i);
                    var strTitle = item.Key.Split('_');
                    string[] score = (string[])item.Value;

                    var topicEnity = lstTopic.Where(s => item.Key.Contains(s.Code)).FirstOrDefault();

                    if (topicEnity != null)
                    {
                        var scoreTopicEntity = lstScoreTopic.Where(s => topicEnity.ID == s.TopicID && strTitle[1] == s.Code).FirstOrDefault();

                        var traineeTopicEntity = lstTraineeTopic.Where(s => s.TraineeID != null && topicEnity.ID == s.TopicID && Guid.Parse(traineeID[0]) == s.TraineeID.Value).FirstOrDefault();
                        if (traineeTopicEntity != null)
                        {
                            //Edit cột score trong bảng traineeTopic

                            if (!string.IsNullOrEmpty(score[0]))
                            {
                                traineeTopicEntity.Score = double.Parse(score[0]);
                            }
                            else {
                                traineeTopicEntity.Score = null;
                            }
                            
                            message = traineeTopicServices.Edit(traineeTopicEntity);

                            //Kiểm tra trong bảng TraineeScoree. Nếu chưa có record thì tạo mới nguoc lai thì edit record
                            var traineeScoreEntity = lstTraineeScore.Where(s => s.TraineeTopicID == traineeTopicEntity.ID && scoreTopicEntity.ScoreTypeID == s.ScoreTypeID).FirstOrDefault();
                            if (traineeScoreEntity == null)
                            {
                                Tra_TraineeScoreEntity newTraineeScoreEntity = new Tra_TraineeScoreEntity();
                                newTraineeScoreEntity.ScoreTypeID = scoreTopicEntity.ScoreTypeID;
                                newTraineeScoreEntity.TraineeTopicID = traineeTopicEntity.ID;
                                newTraineeScoreEntity.Score = traineeTopicEntity.Score;
                                message = traineeScoreServices.Add(newTraineeScoreEntity);
                            }
                            else
                            {
                                traineeScoreEntity.Score = traineeTopicEntity.Score;
                                message = traineeScoreServices.Edit(traineeScoreEntity);
                            }
                            // lstEntity.Add(traineeTopicEntity);
                        }
                    }
                }
                //message = traineeTopicServices.Edit(lstEntity);
            }
            return null;
        }

        public ActionResult GetCourseTopicByCourseIDList([DataSourceRequest] DataSourceRequest request, Guid? CourseID)
        {

            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(CourseID);
            var result = new List<Tra_CourseTopicModel>();
            if (CourseID != Guid.Empty)
            {
                result = baseService.GetData<Tra_CourseTopicModel>(objs, ConstantSql.hrm_cat_sp_get_CourseTopicByCourseID, ref status);
            }
            return Json(result.ToDataSourceResult(request));
        }





        #endregion

        #region Tra_ScoreTopic
        [HttpPost]
        public ActionResult AddScoreTypeToTopic(Guid ScoreTypeID, Guid TopicID)
        {
            var model = new Tra_ScoreTopicModel();
            if (ScoreTypeID != Guid.Empty && TopicID != Guid.Empty)
            {
                model.ScoreTypeID = ScoreTypeID;
                model.TopicID = TopicID;
            }
            #region Validate
            string message = string.Empty;

            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_ScoreTopicModel>(model, "Tra_ScoreTopic", ref message);
            if (!checkValidate)
            {

                model.ActionStatus = ConstantMessages.ScoreTypeIsExistedInTopic.ToString();
                return Json(model.ActionStatus, JsonRequestBehavior.AllowGet);
            }
            #endregion


            var courseTopicServices = new Tra_ScoreTopicServices();
            var entity = new Tra_ScoreTopicEntity();
            if (model != null)
            {
                entity = model.CopyData<Tra_ScoreTopicEntity>();
                message = courseTopicServices.Add(entity);
                return Json(message, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        #endregion

        #region Tra_Report
        public ActionResult GetReportTraineeJoinCourse([DataSourceRequest] DataSourceRequest request, Tra_ReportTraineeJoinCourseModel model)
        {

            string status = string.Empty;

            Tra_ReportServices service = new Tra_ReportServices();
            Cat_PayrollGroupServices payrollGroupServices = new Cat_PayrollGroupServices();
            ActionService service1 = new ActionService(UserLogin);
            var hrService = new Hre_ProfileServices();
            Guid[] _RankIDs = null;
            if (model.RankID != null)
                _RankIDs = model.RankID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            string[] _CourseIDs = null;
            if (model.CourseID != null)
                _CourseIDs = model.CourseID.Split(',').ToArray();


            List<object> strOrgIDs = new List<object>();
            strOrgIDs.AddRange(new object[3]);
            strOrgIDs[0] = (object)model.OrgStructureID;

           // List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();

            DataTable result = service.GetReportTraineeJoinCourse(_RankIDs, _CourseIDs, model.OrgStructureID, model.IsCreateTemplate, model.DateSeniory, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {

                obj = result;
                isDataTable = true;
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Tra_ReportTraineeJoinCourseEntity",
                    OutPutPath = path,
                    //     HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);

                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);

                return Json(fullPath);
            }
            if (model.IsAllPage)
            {
                string strCodeEmp = "";
                foreach (DataRow item in result.Rows)
                {
                    strCodeEmp += item[Tra_ReportTraineeJoinCourseEntity.FieldNames.CodeEmp] + ",";
                }
                return Json(strCodeEmp, JsonRequestBehavior.AllowGet);
                //ViewBag.checkAll = strCodeEmp;
            }
            return new JsonResult() { Data = result.ToDataSourceResult(request), MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //    return Json(result.ToDataSourceResult(request));

        }

        public ActionResult GetReportTraineeJoinCoursePass([DataSourceRequest] DataSourceRequest request, Tra_ReportTraineeJoinCourseModel model)
        {

            string status = string.Empty;

            Tra_ReportServices service = new Tra_ReportServices();
            Cat_PayrollGroupServices payrollGroupServices = new Cat_PayrollGroupServices();
            ActionService service1 = new ActionService(UserLogin);
            var hrService = new Hre_ProfileServices();
            Guid[] _RankIDs = null;
            if (model.RankID != null)
                _RankIDs = model.RankID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            Guid[] _CourseIDs = null;
            if (model.CourseID != null)
                _CourseIDs = model.CourseID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            Guid[] _RequirementTrainIDs = null;
            if (model.RequirementTrainID != null)
            {
                _RequirementTrainIDs = model.RequirementTrainID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }

            //List<object> strOrgIDs = new List<object>();
            //strOrgIDs.AddRange(new object[3]);
            //strOrgIDs[0] = (object)model.OrgStructureID;

            //List<Guid> lstProfileIDs = hrService.GetData<Hre_ProfileIdEntity>(strOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).Select(s => s.ID).ToList();

            DataTable result = service.GetReportTraineeJoinCoursePass(_CourseIDs, model.OrgStructureID, model.IsCreateTemplate, _RequirementTrainIDs, UserLogin);
            //HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.DateFrom };
            //HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.DateTo };
            //List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {

                obj = result;
                isDataTable = true;
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Tra_ReportTraineeJoinCourseEntity",
                    OutPutPath = path,
                    // HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);

                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));

        }
        public ActionResult GetReportTraineeByMonth([DataSourceRequest] DataSourceRequest request, Tra_ReportTraineeByMonthModel model)
        {

            string status = string.Empty;

            Tra_ReportServices service = new Tra_ReportServices();
            Cat_PayrollGroupServices payrollGroupServices = new Cat_PayrollGroupServices();
            ActionService service1 = new ActionService(UserLogin);
            var hrService = new Hre_ProfileServices();



            DataTable result = service.GetReportTraineeByMonth(model.Month.Value, model.CourseID, model.IsCreateTemplate, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Tra_ReportTraineeByMonthEntity",
                    OutPutPath = path,
                    //     HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);

                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));

        }

        #region Tra_ReportTraineeResult
         public ActionResult GetReportTraineeResult([DataSourceRequest] DataSourceRequest request, Tra_ReportTraineeResultSearchModel model)
         {
             List<Tra_TraineeEntity> lstresult = new List<Tra_TraineeEntity>();
             //if (model.IsCreateTemplate != true)
             //{
             //    string status = string.Empty;
             //    var traineeServices = new ActionService(UserLogin);
             //    var objTrainee = new List<object>();
             //    objTrainee.AddRange(new object[15]);
             //    objTrainee[1] = model.CodeEmp;
             //    objTrainee[3] = model.DateFrom;
             //    objTrainee[4] = model.DateTo;
             //    objTrainee[8] = model.OrgStructureID;
             //    objTrainee[9] = model.ProfileName;
             //    objTrainee[10] = model.Status;
             //    objTrainee[11] = model.RequirementTrainName;
             //    objTrainee[13] = 1;
             //    objTrainee[14] = int.MaxValue - 1;
             //    lstresult = traineeServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, ref status).ToList();
             //    Guid[] classID = null;
             //    if (!string.IsNullOrEmpty(model.ClassID))
             //    {
             //        classID = model.ClassID.Split(',').Select(s => Guid.Parse(s)).ToArray();
             //        if (classID != null)
             //        {
             //            lstresult = lstresult.Where(s => classID.Contains(s.ClassID)).ToList();
             //        }
             //    }
             //    Guid[] courseID = null;
             //    if (!string.IsNullOrEmpty(model.CourseID))
             //    {
             //        courseID = model.CourseID.Split(',').Select(s => Guid.Parse(s)).ToArray();
             //        if (courseID != null)
             //        {
             //            lstresult = lstresult.Where(s => courseID.Contains(s.CourseID)).ToList();
             //        }
             //    }
             //}
            Tra_ReportServices service = new Tra_ReportServices();

             //DataTable result = service.GetReportTraineeResult(lstresult, model.IsCreateTemplate, model.ScoreTypeID);
            model.OrgStructureID = Common.DotNetToOracle(model.OrgStructureID);
            model.CourseID = Common.DotNetToOracle(model.CourseID);
            model.ClassID = Common.DotNetToOracle(model.ClassID);
            model.ScoreTypeID = Common.DotNetToOracle(model.ScoreTypeID);
            model.RequirementTrainIDs = Common.DotNetToOracle(model.RequirementTrainIDs);
            DataTable result = service.GetReportTraineeResultNew(model.Copy<Tra_ReportTraineeResultSearchEntity>(), UserLogin, model.IsCreateTemplate);


            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
             //   result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Tra_ReportTraineeResultEntity",
                    OutPutPath = path,
                    //     HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var col = result.Columns.Count;
              //  result.Columns.RemoveAt(col - 1);

                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));


        }
        #endregion


        public ActionResult GetReportTransferTrainee([DataSourceRequest] DataSourceRequest request, Tra_ReportTransferTraineeModel model)
        {

            string status = string.Empty;

            Tra_ReportServices service = new Tra_ReportServices();
            Cat_PayrollGroupServices payrollGroupServices = new Cat_PayrollGroupServices();
            ActionService service1 = new ActionService(UserLogin);
            var hrService = new Hre_ProfileServices();

            DataTable result = service.GetReportTransferTrainee(model.CodeEmp, model.ProfileName, model.OrgStructureID, model.CourseID, model.ClassID, model.ClassOldID, model.IsCreateTemplate, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                //var col = result.Columns.Count;
                //result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Tra_ReportTransferTraineeEntity",
                    OutPutPath = path,
                    //     HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                //var col = result.Columns.Count;
                //result.Columns.RemoveAt(col - 1);

                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));

        }


        public ActionResult GetReportTraineeUnAttend([DataSourceRequest] DataSourceRequest request, Tra_ReportTraineeUnAttendModel model)
        {

            string status = string.Empty;

            Tra_ReportServices service = new Tra_ReportServices();
            Cat_PayrollGroupServices payrollGroupServices = new Cat_PayrollGroupServices();
            ActionService service1 = new ActionService(UserLogin);
            var hrService = new Hre_ProfileServices();
            Guid[] courseID = null;
            if (!string.IsNullOrEmpty(model.CourseID))
            {
                courseID = model.CourseID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }

            Guid[] classID = null;
            if (!string.IsNullOrEmpty(model.ClassID))
            {
                classID = model.ClassID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            Guid[] RequirementTrainIDs = null;
            if(!string.IsNullOrEmpty(model.RequirementTrainID))
            {               
                RequirementTrainIDs = model.RequirementTrainID.Split(',').Select(s => Guid.Parse(s)).ToArray();
            }
            DataTable result = service.GetReportTraineeUnAttend(model.CodeEmp, model.ProfileName, model.OrgStructureID, courseID, classID, model.StartDate, model.EndDate, model.IsCreateTemplate, RequirementTrainIDs, UserLogin);

            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Tra_ReportTraineeUnAttendEntity",
                    OutPutPath = path,
                    //     HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);

                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));

        }

        public ActionResult GetReportTraineeNotInRequirementDetail([DataSourceRequest] DataSourceRequest request, Tra_ReportTraineeNotInRequirementDetailModel model)
        {

            string status = string.Empty;

            Tra_ReportServices service = new Tra_ReportServices();

            ActionService service1 = new ActionService(UserLogin);
            var hrService = new Hre_ProfileServices();

            Guid[] _ClassIDs = null;
            Guid[] _CourseIDs = null;
            Guid[] _RequirementTrainIDs = null;
            if (model.ClassIDs != null)
                _ClassIDs = model.ClassIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            if (model.CourseIDs != null)
                _CourseIDs = model.CourseIDs.Split(',').Select(s => Guid.Parse(s)).ToArray();
            //if (model.RequirementTrainID != null)
            //    _RequirementTrainIDs = model.RequirementTrainID.Split(',').Select(s => Guid.Parse(s)).ToArray();

            //DataTable result = service.GetReportTraineeNotInRequirementDetail(model.Month, model.OrgStructureID, _ClassIDs, model.IsCreateTemplate, _CourseIDs, _RequirementTrainIDs);
            DataTable result = service.GetReportTraineeNotInRequirementDetail(model.Month, model.OrgStructureID, _ClassIDs, model.IsCreateTemplate, _CourseIDs, UserLogin);
            #region Xử lý cách export mới
            var isDataTable = false;
            DataTable obj = null;
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }

            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();

                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Tra_ReportTraineeNotInRequirementDetailModel",
                    OutPutPath = path,
                    //     HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }

            #endregion
            if (model.ExportId != Guid.Empty)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);

                var fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);

                return Json(fullPath);
            }
            return Json(result.ToDataSourceResult(request));

        }



        #endregion

        #region Tra_RequirementTrainDetail
        public ActionResult GetRequirementTrainDetailByRequirementTrainID([DataSourceRequest] DataSourceRequest request, Guid? requirementTrainID)
        {
            if (requirementTrainID != null)
            {
                string status = string.Empty;
                var baseService = new ActionService(UserLogin);
                var objs = new List<object>();
                objs.Add(requirementTrainID);
                var result = baseService.GetData<Tra_RequirementTrainDetailModel>(objs, ConstantSql.hrm_tra_sp_get_RequirementTrainDetailByRMTDTID, ref status);
                if (result != null)
                    return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult GetTopicByCourseId(Guid courceid)
        {
            var result = new List<Tra_RequirementTrainDetailModel>();
            string status = string.Empty;
            if (courceid != Guid.Empty)
            {
                var obj = new List<object>();
                obj.Add(courceid);
                var service = new ActionService(UserLogin);
                result = service.GetData<Tra_RequirementTrainDetailModel>(obj, ConstantSql.hrm_tra_sp_get_TopicByCourseID, ref status);
            }
            string listTopicName = "";
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item.TopicName != null)
                    {
                        listTopicName += item.TopicName + ", ";
                    }
                }
            }
            if (listTopicName != "")
            {
                listTopicName = listTopicName.Substring(0, listTopicName.Length - 2);
            }
            return Json(listTopicName, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Tra_TraineeScore
        [HttpPost]
        public ActionResult GetTraineeScoreList([DataSourceRequest] DataSourceRequest request, Tra_TraineeScoreSearchModel model)
        {
            return GetListDataAndReturn<Tra_TraineeScoreModel, Tra_TraineeScoreEntity, Tra_TraineeScoreSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeScoreData);

        }

        public ActionResult ExportAllTraineeScore([DataSourceRequest] DataSourceRequest request, Tra_TraineeScoreSearchModel model)
        {
            return ExportAllAndReturn<Tra_TraineeScoreEntity, Tra_TraineeScoreModel, Tra_TraineeScoreSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeScoreData);

        }

        public ActionResult ExportTraineeScoreSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_TraineeScoreEntity, Tra_TraineeScoreModel>(selectedIds, valueFields, ConstantSql.hrm_cat_sp_get_TraineeScoreByIds);
        }

        #endregion

        #region Tra_RankingGroup
        public JsonResult GetMultiRankingGroup(string text)
        {
            return GetDataForControl<Tra_RankingGroupMultiModel, Tra_RankingGroupMultiEntity>(text, ConstantSql.hrm_tra_sp_get_RankingGroup_Multi);
        }

        #endregion

        public ActionResult GetDataByCodeEmp([DataSourceRequest] DataSourceRequest request, string CodeEmp)
        {
            var profileServices = new ActionService(UserLogin);
            string[] arrCodeEmp = new string[1];
            if (!string.IsNullOrEmpty(CodeEmp) && CodeEmp.IndexOf(',') > 1)
            {
                arrCodeEmp = CodeEmp.Split(',').ToArray();
            }
            else
            {
                arrCodeEmp[0] = CodeEmp;
            }
            string status = string.Empty;
            var objProfile = new List<object>();
            objProfile.AddRange(new object[18]);
            objProfile[16] = 1;
            objProfile[17] = int.MaxValue - 1;
            var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_Profile, ref status).ToList().Translate<Hre_ProfileModel>();
            if (arrCodeEmp != null)
            {
                lstProfile = lstProfile.Where(s => arrCodeEmp.Contains(s.CodeEmp)).ToList();
                return Json(lstProfile.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public ActionResult GetDataTraineeByCodeEmp([DataSourceRequest] DataSourceRequest request, string CodeEmp)
        {
            var profileServices = new ActionService(UserLogin);
            string status = string.Empty;
            if (!string.IsNullOrEmpty(CodeEmp))
            {
                var objProfile = new List<object>();
                objProfile.Add(CodeEmp);
                var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByCodeEmp, ref status).ToList().Translate<Hre_ProfileModel>();
                return Json(lstProfile.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        #region ComputePoint

        public ActionResult ComputePoint(List<string> selectedIds)
        {
            string status = string.Empty;
            string message = string.Empty;

            var traineeIds = new List<Guid>();
            var classIds = new List<Guid>();
            foreach (var item in selectedIds)
            {
                var str = item.Split(',');
                traineeIds.Add(Common.ConvertToGuid(str[0]));
                classIds.Add(Common.ConvertToGuid(str[1]));
            }
            var courseServices = new ActionService(UserLogin);

            var traineeServices = new Tra_TraineeServices();

            var objTrainee = new List<object>();
            objTrainee.AddRange(new object[15]);
            objTrainee[13] = 1;
            objTrainee[14] = int.MaxValue - 1;
            var lstTrainee = courseServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, ref status).ToList();

         
            var objCourse = new List<object>();
            objCourse.AddRange(new object[11]);
            objCourse[9] = 1;
            objCourse[10] = int.MaxValue - 1;
            var lstCourse = courseServices.GetData<Tra_CourseEntity>(objCourse, ConstantSql.hrm_tra_sp_get_Course, ref status).ToList();

            var classServices = new ActionService(UserLogin);
            var objClass = new List<object>();
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(1);
            objClass.Add(int.MaxValue - 1);
            var lstClass = classServices.GetData<Tra_ClassEntity>(objClass, ConstantSql.hrm_tra_sp_get_Class, ref status).ToList();

            var topicServices = new ActionService(UserLogin);
            var objTopic = new List<object>();
            objTopic.Add(null);
            objTopic.Add(null);
            objTopic.Add(null);
            objTopic.Add(null);
            objTopic.Add(1);
            objTopic.Add(int.MaxValue - 1);
            var lstTopic = topicServices.GetData<Cat_TopicEntity>(objTopic, ConstantSql.hrm_cat_sp_get_Topic, ref status).ToList();

            var traineeTopicServices = new ActionService(UserLogin);
            var objTraineeTopic = new List<object>();
            objTraineeTopic.Add(1);
            objTraineeTopic.Add(int.MaxValue - 1);
            var lstTraineeTopic = traineeTopicServices.GetData<Tra_TraineeTopicEntity>(objTraineeTopic, ConstantSql.hrm_tra_sp_get_TraineeTopic, ref status).ToList();

            var courseTopicServices = new ActionService(UserLogin);
            var objCourseTopic = new List<object>();
            objCourseTopic.Add(1);
            objCourseTopic.Add(int.MaxValue - 1);
            var lstCourseTopic = courseTopicServices.GetData<Tra_CourseTopicEntity>(objCourseTopic, ConstantSql.hrm_tra_sp_get_CourseTopic, ref status).ToList();

            var scoreTypeService = new ActionService(UserLogin);
            var objScoreType = new List<object>();
            objScoreType.Add(null);
            objScoreType.Add(null);
            objScoreType.Add(1);
            objScoreType.Add(int.MaxValue - 1);
            var lstScoreType = scoreTypeService.GetData<Tra_ScoreTypeEntity>(objScoreType, ConstantSql.hrm_tra_sp_get_ScoreType, ref status).ToList();

            var scoreTopicServices = new ActionService(UserLogin);
            var objScoreTopic = new List<object>();
            objScoreTopic.Add(1);
            objScoreTopic.Add(int.MaxValue - 1);
            var lstScoreTopic = scoreTopicServices.GetData<Tra_ScoreTopicEntity>(objScoreTopic, ConstantSql.hrm_tra_sp_get_ScoreTopic, ref status).ToList();

            var traineeScoreServices = new ActionService(UserLogin);
            var objTraineeScore = new List<object>();
            objTraineeScore.Add(1);
            objTraineeScore.Add(int.MaxValue - 1);
            var lstTraineeScore = traineeScoreServices.GetData<Tra_TraineeScoreEntity>(objTraineeScore, ConstantSql.hrm_tra_sp_get_TraineeScore, ref status).ToList();


            var lstClassIds = new List<Guid>();
            if (classIds != null)
            {
                lstClassIds = classIds.Distinct().ToList();
                lstClass = lstClass.Where(s => classIds.Contains(s.ID)).ToList();
            }



            foreach (var item in lstClass)
            {
                if (item.StandardScoreToPass == null)
                {
                    continue;
                }
                var courseEntity = lstCourse.Where(s => item.CourseID == s.ID).FirstOrDefault();
                var topicByCourseID = lstCourseTopic.Where(s => item.CourseID == s.CourseID).Select(s => s.TopicID).ToList();

                if (courseEntity != null)
                {
                    lstTrainee = lstTrainee.Where(s => traineeIds.Contains(s.ID) && item.ID == s.ClassID).ToList();
                    foreach (var trainee in lstTrainee)
                    {
                        var lstTraineeByTopicIDAndTraineeID = lstTraineeTopic.Where(s => topicByCourseID.Contains(s.TopicID.Value) && trainee.ID == s.TraineeID).ToList();
                        var lstTraineeTopicByTraineeID = lstTraineeTopic.Where(s => s.TraineeID == trainee.ID).ToList();

                        var lstTopicForTrainee = lstTopic.Where(s => topicByCourseID.Contains(s.ID)).ToList();

                        if (!string.IsNullOrEmpty(courseEntity.Formula))
                        {
                            double traineeScore = -1;

                            #region HienNguyen
                            if (lstTopicForTrainee.Count > 0)
                            {
                                List<ElementFormula> listElementFormula = new List<ElementFormula>();
                                foreach (var topicItem in lstTopicForTrainee)
                                {
                                    Tra_TraineeTopicEntity TraineeTopicItem = lstTraineeByTopicIDAndTraineeID.Where(m => m.TopicID == topicItem.ID).FirstOrDefault();
                                    var lstScoreTopicByTopicID = lstScoreTopic.Where(s => s.TopicID == topicItem.ID).Select(s => s.ScoreTypeID).ToList();
                                    var lstScoreForTrainee = lstTraineeScore.Where(s => s.ScoreTypeID != null && s.TraineeTopicID != null && s.TraineeTopicID.Value == TraineeTopicItem.ID && lstScoreTopicByTopicID.Contains(s.ScoreTypeID.Value)).ToList();
                                    var score = lstScoreForTrainee.Where(s => s.Score!=null).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                                    if (score != null)
                                    {
                                        var ElementFormulaItem = new ElementFormula(topicItem.Code.ReplaceSpace(), score != null ? score.Score != null ? score.Score : 0 : 0, 0);
                                        listElementFormula.Add(ElementFormulaItem);
                                        HRM.Infrastructure.Utilities.Helper.FormulaHelper.FormulaHelperModel ElementResult = FormulaHelper.ParseFormula(courseEntity.Formula.Replace("[", "").Replace("]", ""), listElementFormula);
                                        if (ElementResult.ErrorMessage != null || ElementResult.ErrorMessage == string.Empty)
                                        {
                                            traineeScore = (double)ElementResult.Value;
                                        }
                                    }
                                    
                                }
                                
                            }
                            #endregion

                            #region Update score cho hv
                            if (traineeScore == -1)
                            {
                                trainee.Status = EnumDropDown.TraineeStatus.E_FAILED.ToString();
                                trainee.Result = null;
                                message = traineeServices.Edit(trainee);
                            }
                            else {
                                if (item.StandardScoreToPass != null && traineeScore >= item.StandardScoreToPass)
                                {
                                    trainee.Status = EnumDropDown.TraineeStatus.E_PASSED.ToString();
                                    trainee.Result = traineeScore;
                                }
                                else
                                {
                                    trainee.Status = EnumDropDown.TraineeStatus.E_FAILED.ToString();
                                    trainee.Result = traineeScore;
                                }
                                message = traineeServices.Edit(trainee);
                            }
                           
                            #endregion

                         
                        }
                    }
                }
            }

            return Json(message, JsonRequestBehavior.AllowGet);



        }
        #endregion


        [HttpPost]
        public ActionResult AddTraineeForClass(List<Guid> selectedIds, Guid classID)
        {

            var services = new Tra_ClassServices();
            var message = string.Empty;
            message = services.AddTraineeToClass(selectedIds, classID, UserLogin);
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TransferTraineeForClassValidate(List<Guid> selectedIds, Guid classID, Guid tranferClassID)
        {

            ActionService service = new ActionService(UserLogin);
            var message = string.Empty;
            var entity = service.GetByIdUseStore<Tra_ClassEntity>(classID, ConstantSql.hrm_tra_sp_get_ClassById, ref message);
            if (entity != null && entity.Status != HRM.Infrastructure.Utilities.EnumDropDown.ClassStatus.E_PASSED_CERTIFICATE.ToString())
            {
                message = NotificationType.Success.ToString();
            }
            else
            {
                message = NotificationType.Error.ToString();
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TransferTraineeForClass(List<Guid> selectedIds, Guid classID, Guid tranferClassID)
        {

            var services = new Tra_ClassServices();
            var message = string.Empty;
            message = services.TransferTraineeForClass(selectedIds, classID, tranferClassID, UserLogin);
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        #region Tra_TraineeCertificate
        public ActionResult GetTraineeCertificateList([DataSourceRequest] DataSourceRequest request, Tra_TraineeCertificateSearchModel model)
        {
            return GetListDataAndReturn<Tra_TraineeCertificateModel, Tra_TraineeCertificateEntity, Tra_TraineeCertificateSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeCertificateList);

        }

        public ActionResult ExportAllTraineeCertificateList([DataSourceRequest] DataSourceRequest request, Tra_TraineeCertificateSearchModel model)
        {
            return ExportAllAndReturn<Tra_TraineeCertificateEntity, Tra_TraineeCertificateModel, Tra_TraineeCertificateSearchModel>(request, model, ConstantSql.hrm_tra_sp_get_TraineeCertificateList);

        }

        public ActionResult ExportTraineeCertificateSelected(string selectedIds, string valueFields)
        {
            return ExportSelectedAndReturn<Tra_TraineeCertificateEntity, Tra_TraineeCertificateModel>(selectedIds, valueFields, ConstantSql.hrm_tra_sp_get_TraineeCertificateByIds);
        }

        public ActionResult GetScoreTopicByTopic([DataSourceRequest] DataSourceRequest request, Guid? TopicID)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var result = new List<Tra_ScoreTopicModel>();
            if (TopicID != Guid.Empty)
            {
                result = baseService.GetData<Tra_ScoreTopicModel>(Common.DotNetToOracle(TopicID.ToString()), ConstantSql.hrm_cat_sp_get_ScoreTopicByTopicID, ref status);
            }

            return Json(result.ToDataSourceResult(request));
        }
        #endregion

        #region Import TraineeScore

        public ActionResult ImportValidate(CatImportModel Model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<CatImportModel>(Model, "Tra_Import_List", ref message);
            if (!checkValidate)
            {
                var ls = new object[] { "error", message };
                return Json(ls);
            }
            #endregion
            return Json(message);
        }

        /// <summary>
        /// Dữ liệu đúng dùng để lưu
        /// </summary>
        public static IList ListImportDataTemp { get; set; }
        /// <summary>
        /// Dữ liệu sai dùng để export ra lại excel.
        /// </summary>
        public static IList ListInvalidDataTemp { get; set; }
        private static ImportService ImportService { get; set; }
        [HttpPost]
        public JsonResult ImportScore([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var services = new Tra_TraineeScoreServices();
            List<Tra_TraineeScoreImport> lstTraineeScore = new List<Tra_TraineeScoreImport>();
            List<Tra_TraineeScoreImport> lstTraineeScoreTranslate = new List<Tra_TraineeScoreImport>();
            List<Tra_TraineeScoreImport> DataError = new List<Tra_TraineeScoreImport>();
            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");
            ImportService = new ImportService
            {
                //FileName = Common.GetPath(model.TemplateFile),
                FileName = _fileName,
                DateTimeFormat = model.FormatDate,
                ImportTemplateID = model.ID,

            };

            try
            {

                var table = ImportService.ImportNew(_fileName, model.TemplateFile, model.ID);

                lstTraineeScore = table.Translate<Tra_TraineeScoreImport>();

                foreach (var item in lstTraineeScore)
                {
                    var entity = new Tra_TraineeScoreImport
                    {
                        ClassCode = item.Column2,
                        CodeEmp = item.Column3,
                        TopicCode = item.Column4,
                        TypeScore = item.Column5,
                        Score = item.Column6,
                        TypeError = item.Column7,
                    };
                    lstTraineeScoreTranslate.Add(entity);
                }

                var lstEntity = services.GetListTraineeScore(lstTraineeScoreTranslate, out DataError);
                if (DataError.Count > 0)
                {
                    return Json(DataError.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }
            return Json(DataError.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveImortScore([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var services = new Tra_TraineeScoreServices();
            List<Tra_TraineeScoreImport> lstTraineeScore = new List<Tra_TraineeScoreImport>();
            List<Tra_TraineeScoreImport> lstTraineeScoreTranslate = new List<Tra_TraineeScoreImport>();
            List<Tra_TraineeScoreImport> DataError = new List<Tra_TraineeScoreImport>();
            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");
            ImportService = new ImportService
            {
                //FileName = Common.GetPath(model.TemplateFile),
                FileName = _fileName,
                DateTimeFormat = model.FormatDate,
                ImportTemplateID = model.ID,

            };

            try
            {

                var table = ImportService.ImportNew(_fileName, model.TemplateFile, model.ID);

                lstTraineeScore = table.Translate<Tra_TraineeScoreImport>();

                foreach (var item in lstTraineeScore)
                {
                    var entity = new Tra_TraineeScoreImport
                    {
                        ClassCode = item.Column2,
                        CodeEmp = item.Column3,
                        TopicCode = item.Column4,
                        TypeScore = item.Column5,
                        Score = item.Column6,
                        TypeError = item.Column7,
                    };
                    lstTraineeScoreTranslate.Add(entity);
                }

                var message = services.SaveListTraineeScore(lstTraineeScoreTranslate, out DataError);
                if (message == NotificationType.Success.ToString())
                {
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }
            return Json(DataError.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public JsonResult ImportTrainee([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var services = new Tra_TraineeServices();
            List<Tra_TraineeScoreImport> lstTraineeScore = new List<Tra_TraineeScoreImport>();
            List<Tra_TraineeScoreImport> DataError = new List<Tra_TraineeScoreImport>();
            List<ImportTraineeEntity> lstTrainee = new List<ImportTraineeEntity>();
            List<ImportTraineeEntity> lstTraineeTranslate = new List<ImportTraineeEntity>();

            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");
            ImportService = new ImportService
            {
                //FileName = Common.GetPath(model.TemplateFile),
                FileName = _fileName,
                DateTimeFormat = model.FormatDate,
                ImportTemplateID = model.ID,

            };

            try
            {

                var table = ImportService.ImportNew(_fileName, model.TemplateFile, model.ID);

                lstTrainee = table.Translate<ImportTraineeEntity>();

                foreach (var item in lstTrainee)
                {
                    var entity = new ImportTraineeEntity
                    {
                        CodeEmp = item.Column2,
                        ClassCode = item.Column3,
                        RequirementCode = item.Column4,
                        Status = item.Column5
                    };
                    lstTraineeTranslate.Add(entity);
                }

                var lstEntity = services.GetListTrainee(lstTraineeTranslate,UserLogin);
                if (lstEntity.Count > 0)
                {
                    return Json(lstEntity.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }
            return Json(DataError.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveImortTrainee([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var services = new Tra_TraineeServices();
            List<ImportTraineeEntity> lstTrainee = new List<ImportTraineeEntity>();
            List<ImportTraineeEntity> lstTraineeTranslate = new List<ImportTraineeEntity>();
            List<Tra_TraineeScoreImport> DataError = new List<Tra_TraineeScoreImport>();
            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");
            ImportService = new ImportService
            {
                //FileName = Common.GetPath(model.TemplateFile),
                FileName = _fileName,
                DateTimeFormat = model.FormatDate,
                ImportTemplateID = model.ID,

            };

            try
            {

                var table = ImportService.ImportNew(_fileName, model.TemplateFile, model.ID);

                lstTrainee = table.Translate<ImportTraineeEntity>();
                foreach (var item in lstTrainee)
                {
                    var entity = new ImportTraineeEntity 
                    { 
                        CodeEmp = item.Column2,
                        ClassCode = item.Column3,
                        RequirementCode = item.Column4,
                        Status = item.Column5
                    };
                    lstTraineeTranslate.Add(entity);
                }

                var message = services.ImportTrainee(lstTraineeTranslate,UserLogin);
                if (message == NotificationType.Success.ToString())
                {
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }
            return Json(DataError.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeClass([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var services = new Tra_TraineeServices();
            List<Tra_TraineeScoreImport> lstTraineeScore = new List<Tra_TraineeScoreImport>();
            List<Tra_TraineeScoreImport> DataError = new List<Tra_TraineeScoreImport>();
            List<ImportTraineeEntity> lstTrainee = new List<ImportTraineeEntity>();
            List<Tra_TraineeChangeClassEntity> lstTraineeTranslate = new List<Tra_TraineeChangeClassEntity>();
            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");
            ImportService = new ImportService
            {
                FileName = _fileName,
                DateTimeFormat = model.FormatDate,
                ImportTemplateID = model.ID,
            };
            try
            {
                var table = ImportService.ImportNew(_fileName, model.TemplateFile, model.ID);
              //  table.Rows.RemoveAt(table.Rows.Count);
                lstTrainee = table.Translate<ImportTraineeEntity>();

                foreach (var item in lstTrainee)
                {
                    var entity = new Tra_TraineeChangeClassEntity
                    {
                        CodeEmp = item.Column2,
                        ClassOldCode = item.Column3,
                        ClassNewCode = item.Column4
                    };
                    lstTraineeTranslate.Add(entity);
                }
                var lstEntity = services.ChangeClass(lstTraineeTranslate);
                if (lstEntity.Count > 0)
                {
                    return Json(lstEntity.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }
            return Json(DataError.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveChangeClass([DataSourceRequest] DataSourceRequest request, CatImportModel model)
        {
            var services = new Tra_TraineeServices();
            List<ImportTraineeEntity> lstTrainee = new List<ImportTraineeEntity>();
            List<Tra_TraineeChangeClassEntity> lstTraineeTranslate = new List<Tra_TraineeChangeClassEntity>();
            List<Tra_TraineeScoreImport> DataError = new List<Tra_TraineeScoreImport>();
            var _fileName = Common.GetPath(Common.TemplateURL) + model.TemplateFile;
            _fileName = _fileName.Replace("/", "\\");
            ImportService = new ImportService
            {
                FileName = _fileName,
                DateTimeFormat = model.FormatDate,
                ImportTemplateID = model.ID,
            };
            try
            {

                var table = ImportService.ImportNew(_fileName, model.TemplateFile, model.ID);

                lstTrainee = table.Translate<ImportTraineeEntity>();
                foreach (var item in lstTrainee)
                {
                    var entity = new Tra_TraineeChangeClassEntity
                    {
                        CodeEmp = item.Column2,
                        ClassOldCode = item.Column3,
                        ClassNewCode = item.Column4
                    };
                    lstTraineeTranslate.Add(entity);
                }
                var message = services.ImportChangeClass(lstTraineeTranslate,UserLogin);
                if (message == NotificationType.Success.ToString())
                {
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }
            return Json(DataError.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        #endregion
        #region Tra_ReportClass
        public ActionResult GetReportClassList([DataSourceRequest] DataSourceRequest request, Tra_ReportClassSearchModel model)
        {
            string status = string.Empty;
            var service = new ActionService(UserLogin);
            HeaderInfo headerInfo1 = new HeaderInfo() { Name = "DateFrom", Value = model.StartDate };
            HeaderInfo headerInfo2 = new HeaderInfo() { Name = "DateTo", Value = model.EndDate };
            List<HeaderInfo> listHeaderInfo = new List<HeaderInfo>() { headerInfo1, headerInfo2 };

            List<object> paraClass = new List<object>();
            paraClass.AddRange(new object[10]);
            paraClass[3] = model.StartDate;
            paraClass[4] = model.EndDate;
            paraClass[5] = model.ScheduleTimeStart;
            paraClass[6] = model.ScheduleTimeEnd;
            paraClass[7] = model.RequirementTrainID;
            paraClass[8] = 1;
            paraClass[9] = int.MaxValue - 1;
            var listClass = service.GetData<Tra_ClassEntity>(paraClass, ConstantSql.hrm_tra_sp_get_Class, ref status);
            var ReportServices = new Tra_ReportServices();
            var result = ReportServices.GetReportClass(listClass, model.ClassID,model.IsCreateTemplate);
            var isDataTable = false;
            object obj = new DataTable();
            if (model.IsCreateTemplateForDynamicGrid)
            {
                var col = result.Columns.Count;
                result.Columns.RemoveAt(col - 1);
                obj = result;
                isDataTable = true;
            }
            if (model != null && model.IsCreateTemplate)
            {
                var path = Common.GetPath("Templates");
                ExportService exportService = new ExportService();
                ConfigExport cfgExport = new ConfigExport()
                {
                    Object = obj,
                    FileName = "Tra_ReportClassEntity",
                    OutPutPath = path,
                    HeaderInfo = listHeaderInfo,
                    DownloadPath = Hrm_Main_Web + "Templates",
                    IsDataTable = isDataTable
                };
                var str = exportService.CreateTemplate(cfgExport);
                return Json(str);
            }
            if (model.ExportId != Guid.Empty)
            {
                var fullPath = string.Empty;
                if (headerInfo1.Value == null || headerInfo2.Value == null)
                {
                    fullPath = ExportService.Export(model.ExportId, result, null, model.ExportType);
                }
                else
                {
                    fullPath = ExportService.Export(model.ExportId, result, listHeaderInfo, model.ExportType);
                }
                return Json(fullPath);
            }
            #region mapping dataTable to dataList
            List<Tra_ReportClassModel> dataList = new List<Tra_ReportClassModel>();
            Tra_ReportClassModel aTSource = null;

            if (result.Rows.Count > 0)
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(Tra_ReportClassModel).GetProperties(flags)
                                     select new
                                     {
                                         Name = aProp.Name,
                                         Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                     }).ToList();
                var dataTblFieldNames = (from DataColumn aHeader in result.Columns
                                         select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
                var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();
                foreach (DataRow dataRow in result.AsEnumerable().ToList())
                {
                    aTSource = new Tra_ReportClassModel();
                    foreach (var aField in commonFields)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                        if (dataRow[aField.Name] == DBNull.Value)
                            continue;
                        propertyInfos.SetValue(aTSource, dataRow[aField.Name], null);
                    }
                    dataList.Add(aTSource);
                }
            }
            #endregion
            return Json(dataList.ToDataSourceResult(request));
        }
        #endregion

        public ActionResult GetTrainningResultByProfileID([DataSourceRequest] DataSourceRequest request, Guid profileID)
        {
            string status = string.Empty;
            var baseService = new ActionService(UserLogin);
            var objs = new List<object>();
            objs.Add(Common.DotNetToOracle(profileID.ToString()));
            var result = baseService.GetData<Tra_TraineeEntity>(objs, ConstantSql.hrm_tra_sp_get_TrainningResultByProfileID, ref status);
            return Json(result.ToDataSourceResult(request));
        }


        public ActionResult UpdateStatusSelected(List<Guid> selectedIds, Guid UserId)
        {
            Tra_TraineeRegisterModel model = new Tra_TraineeRegisterModel();
            var traineeRegisterService = new Tra_TraineeRegisterServices();
            
            if (selectedIds.Count >= 0)
            {
                var status = traineeRegisterService.ApprovedTraineeRegister(selectedIds,UserId);
            }
            return Json(null);
        }

        public ActionResult ActionRejectSelected(List<Guid> selectedIds, Guid UserId, string Reason)
        {
            Tra_TraineeRegisterModel model = new Tra_TraineeRegisterModel();
            var traineeRegisterService = new Tra_TraineeRegisterServices();

            if (selectedIds.Count >= 0)
            {
                var status = traineeRegisterService.ActionRejectTraineeRegister(selectedIds, UserId,Reason);
            }
            return Json(null);
        }
        public ActionResult ActionCancelSected(List<Guid> selectedIds)
        {
            Tra_TraineeRegisterModel model = new Tra_TraineeRegisterModel();
            var traineeRegisterService = new Tra_TraineeRegisterServices();
            var status = string.Empty;
            if (selectedIds.Count >= 0)
            {
                 status = traineeRegisterService.ActionCancelTraineeRegister(selectedIds);
            }
            return Json(status,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiProfileByClassId(string text, Guid ClassID)
        {
            List<Hre_ProfileMultiModel> lstHre_ProfileMultiModel = new List<Hre_ProfileMultiModel>();
            ActionService _baseService = new ActionService(UserLogin);
            var services = new ActionService(UserLogin);
            var objCourse = new List<object>();
            string status = string.Empty;
            var key = AppConfig.HRM_TRA_CONFIG_TRAINEE_IN_REQUIREMENTDETAIL.ToString();
            var lstConfig = services.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status);
            if (lstConfig != null && lstConfig.Count > 0)
            {
                string strValue = lstConfig.FirstOrDefault().Value1;
                if (strValue == "True")
                {
                    objCourse.Add(text);
                    objCourse.Add(ClassID);
                    //string status = "";
                    lstHre_ProfileMultiModel = services.GetData<Hre_ProfileMultiModel>(objCourse, ConstantSql.hrm_tra_sp_get_ProfileByClassId, ref status);
                }
                else
                {
                    lstHre_ProfileMultiModel = services.GetData<Hre_ProfileMultiModel>(text, ConstantSql.hrm_hr_sp_get_Profile_multi, ref status);
                }
            }
            else
                lstHre_ProfileMultiModel = services.GetData<Hre_ProfileMultiModel>(text, ConstantSql.hrm_hr_sp_get_Profile_multi, ref status);
            
            return Json(lstHre_ProfileMultiModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMultiClassByClassId(string text, Guid ClassID)
        {
            var services = new ActionService(UserLogin);
            var objCourse = new List<object>();
            objCourse.Add(text);
            objCourse.Add(ClassID);
            string status = "";
            var lstHre_ProfileMultiModel = services.GetData<Tra_ClassMultiModel>(objCourse, ConstantSql.hrm_tra_sp_get_ClassByClassId, ref status);

            return Json(lstHre_ProfileMultiModel, JsonRequestBehavior.AllowGet);
        }
    }
}