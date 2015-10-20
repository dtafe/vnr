using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using VnResource.Helper.Data;
using HRM.Presentation.Service;
using System;
using System.Linq;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Training.Models;
using HRM.Business.Training.Models;
using System.Collections.Generic;

using HRM.Business.Training.Domain;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Tra_TraineeController : ApiController
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
        public Tra_TraineeModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Tra_TraineeModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Tra_TraineeEntity>(id,ConstantSql.hrm_tra_sp_get_TraineeById ,ref status);
            if (entity!=null)
            {
                model = entity.CopyData<Tra_TraineeModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xóa hoặc chuyển đổi trạng thái của Ngân Hàng(Tra_Trainee) sang IsDelete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tra_TraineeModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Tra_TraineeEntity, Tra_TraineeModel>(id);
            return result;
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xử lý thêm mới hoặc chỉnh sửa một Ngân Hàng(Tra_Trainee)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Tra_Trainee")]
        public Tra_TraineeModel Post([Bind]Tra_TraineeModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Tra_TraineeModel>(model, "Tra_Trainee", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
        
            var traineeServices = new Tra_TraineeServices();
            ActionService service = new ActionService(UserLogin);
            var isTraineeInRequireDetail = traineeServices.IsAddTraineeDependOnRequirementDetail();
            var traineeTopicServices = new Tra_TraineeTopicServices();
            var planServices = new Tra_PlanServices();
            var status = string.Empty;
            var lstObjPlan = new List<object>();
            lstObjPlan.Add(null);
            lstObjPlan.Add(null);
            lstObjPlan.Add(null);
            lstObjPlan.Add(null);
            lstObjPlan.Add(1);
            lstObjPlan.Add(int.MaxValue - 1);
            var lstPlan = planServices.GetData<Tra_PlanEntity>(lstObjPlan, ConstantSql.hrm_tra_sp_get_Plan, UserLogin, ref status).ToList();


            var planDetailServices = new Tra_PlanDetailServices();
            var lstObjPlanDetail = new List<object>();
            lstObjPlanDetail.Add(null);
            lstObjPlanDetail.Add(1);
            lstObjPlanDetail.Add(int.MaxValue - 1);
            var lstPlanDetail = planDetailServices.GetData<Tra_PlanDetailEntity>(lstObjPlanDetail, ConstantSql.hrm_tra_sp_get_PlanDetail, UserLogin, ref status).ToList();

            var classServices = new Tra_ClassServices();
            var lstObjClass = new List<object>();
            lstObjClass.Add(null);
            lstObjClass.Add(null);
            lstObjClass.Add(null);
            lstObjClass.Add(null);
            lstObjClass.Add(null);
            lstObjClass.Add(null);
            lstObjClass.Add(null);
            lstObjClass.Add(null);
            lstObjClass.Add(1);
            lstObjClass.Add(int.MaxValue - 1);
            var lstClass = classServices.GetData<Tra_ClassEntity>(lstObjClass, ConstantSql.hrm_tra_sp_get_Class, UserLogin, ref status).ToList();

            var lstObjTraineeInClass = new List<object>();
            lstObjTraineeInClass.Add(model.ClassID);
            var lstTraineeInClass = classServices.GetData<Tra_ClassEntity>(lstObjTraineeInClass, ConstantSql.hrm_cat_sp_get_TraineeByClassID, UserLogin, ref status).ToList();

            var courseTopicServices = new Tra_TraineeTopicServices();
            var objCourseTopic = new List<object>();
            objCourseTopic.Add(1);
            objCourseTopic.Add(int.MaxValue - 1);
            var lstCourseTopic = courseTopicServices.GetData<Tra_CourseTopicEntity>(objCourseTopic, ConstantSql.hrm_tra_sp_get_CourseTopic, UserLogin, ref status);


            var classEntity = lstClass.Where(s => s.ID == model.ClassID).FirstOrDefault();
            var profileServices = new Hre_ProfileServices();
            var profileEntity = profileServices.GetData<Hre_ProfileEntity>(model.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, UserLogin, ref status).FirstOrDefault();

            var requirementServices = new Tra_RequirementTrainServices();
            var objRequirement = new List<object>();
            objRequirement.AddRange(new object[7]);
            objRequirement[5] = 1;
            objRequirement[6] = int.MaxValue - 1;
            var lstRequirement = requirementServices.GetData<Tra_RequirementTrainEntity>(objRequirement, ConstantSql.hrm_tra_sp_get_RequirementTrain, UserLogin, ref status).ToList();

            var requirementDetailServices = new Tra_RequirementTrainDetailServices();
            var objRequirementDetail = new List<object>();
            objRequirementDetail.Add(null);
            objRequirementDetail.Add(1);
            objRequirementDetail.Add(int.MaxValue - 1);
            var lstRequirementDetail = requirementDetailServices.GetData<Tra_RequirementTrainDetailEntity>(objRequirementDetail, ConstantSql.hrm_tra_sp_get_RequirementDetail, UserLogin, ref status).ToList();

            var objTrainee = new List<object>();
            objTrainee.AddRange(new object[15]);
            objTrainee[13] = 1;
            objTrainee[14] = int.MaxValue - 1;
            var lstObjTrainee = traineeServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status).ToList();
            var traineeEntity = lstObjTrainee.Where(s => s.ClassID == model.ClassID && model.ProfileID == s.ProfileID).ToList();

            #region Check Học Viên
            if(model.ID == Guid.Empty)
            {
                if (isTraineeInRequireDetail)
                {
                    if (traineeEntity.Count > 0)
                    {
                        model.ActionStatus = ConstantMessages.WarningProfileHaveClass.ToString().TranslateString();
                        return model;
                    }
                    if (classEntity != null)
                    {

                        if (classEntity.PlanID != null)
                        {
                            var lstRequirementByPlanID = lstRequirement.Where(s => s.PlanID != null && s.PlanID.Value == classEntity.PlanID).ToList();
                            foreach (var item in lstRequirementByPlanID)
                            {
                                var requirementDetailEntity = lstRequirementDetail.Where(s => item.ID == s.RequirementTrainID && s.CourseID == classEntity.CourseID && s.ProfileID == model.ProfileID).FirstOrDefault();
                                if (requirementDetailEntity != null)
                                {
                                    model = service.UpdateOrCreate<Tra_TraineeEntity, Tra_TraineeModel>(model);

                                    var lstCourseTopicByCourseID = lstCourseTopic.Where(s => s.CourseID == classEntity.CourseID).ToList();
                                    foreach (var courseTopic in lstCourseTopicByCourseID)
                                    {
                                        var traineeTopicEntity = new Tra_TraineeTopicEntity();
                                        traineeTopicEntity.TopicID = courseTopic.TopicID;
                                        traineeTopicEntity.TraineeID = model.ID;
                                        message = traineeTopicServices.Add(traineeTopicEntity);
                                    }
                                    // edit classID trong RequirementDetail
                                    requirementDetailEntity.ClassID = model.ClassID;
                                    message = requirementDetailServices.Edit(requirementDetailEntity);
                                }
                                else
                                {
                                    model.ActionStatus = ConstantMessages.WarningProfileNotInRequirement.ToString().TranslateString();
                                }
                            }
                        }
                        else
                        {
                            model.ActionStatus = ConstantMessages.WarningProfileNotInRequirement.ToString().TranslateString();
                        }
                        model.ActionStatus = ConstantMessages.WarningProfileNotInRequirement.ToString().TranslateString();
                    }
                    if (message == NotificationType.Success.ToString())
                    {
                        model.ActionStatus = message;
                    }
                    return model;

                    //if (classEntity != null)
                    //{
                    //    if (classEntity.PlanID != null)
                    //    {
                    //        var planEntity = lstPlan.Where(s => s.ID == classEntity.PlanID.Value).FirstOrDefault();
                    //        if (planEntity != null)
                    //        {
                    //            var lstPlanDetailByPlanID = lstPlanDetail.Where(s => s.PlanID == planEntity.ID).ToList();

                    //            foreach (var item in lstPlanDetailByPlanID)
                    //            {
                    //                var lstTraineeInClassForOrg = lstTraineeInClass.Where(s => s.OrgStructureID.Value == item.OrgStructureID.Value).ToList();
                    //                //Kiểm tra số lượng của plan detail với sumQuantity của plan
                    //                if (planEntity.SumQuantity == lstPlanDetailByPlanID.Sum(s => s.Quantity))
                    //                {
                    //                    if (lstTraineeInClass.Count < lstPlanDetailByPlanID.Sum(s => s.Quantity))
                    //                    {
                    //                        // Xử Lý kiểm tra nv có thuộc phòng ban trong plan detail ko
                    //                        if (profileEntity != null)
                    //                        {
                    //                            if (lstTraineeInClassForOrg.Count < item.Quantity)
                    //                            {
                    //                                if (profileEntity.OrgStructureID.Value == item.OrgStructureID.Value)
                    //                                {
                    //                                    var traineeEntity = new Tra_TraineeEntity();
                    //                                    model.OrgStructureID = profileEntity.OrgStructureID != null ? profileEntity.OrgStructureID.Value : profileEntity.OrgStructureID;

                    //                                    model = service.UpdateOrCreate<Tra_TraineeEntity, Tra_TraineeModel>(model);

                    //                                    var lstCourseTopicByCourseID = lstCourseTopic.Where(s => s.CourseID == classEntity.CourseID).ToList();
                    //                                    foreach (var courseTopic in lstCourseTopicByCourseID)
                    //                                    {
                    //                                        var traineeTopicEntity = new Tra_TraineeTopicEntity();
                    //                                        traineeTopicEntity.TopicID = courseTopic.TopicID;
                    //                                        traineeTopicEntity.TraineeID = model.ID;
                    //                                        message = traineeTopicServices.Add(traineeTopicEntity);
                    //                                    }
                    //                                    return model;  
                    //                                }
                    //                                model.ActionStatus = ConstantMessages.WarningProfileNotDependantOrgStructure.TranslateString();
                    //                            }
                    //                            else
                    //                            {
                    //                                model.ActionStatus = ConstantMessages.WarningTraineeOverLimit.TranslateString();
                    //                            }
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        model.ActionStatus = ConstantMessages.WarningTraineeOverLimit.TranslateString();
                    //                    }
                    //                }
                    //                if (planEntity.SumQuantity > lstPlanDetailByPlanID.Sum(s => s.Quantity))
                    //                {
                    //                        model = service.UpdateOrCreate<Tra_TraineeEntity, Tra_TraineeModel>(model);
                    //                        var lstCourseTopicByCourseID = lstCourseTopic.Where(s => s.CourseID == classEntity.CourseID).ToList();
                    //                        foreach (var courseTopic in lstCourseTopicByCourseID)
                    //                        {
                    //                            var traineeTopicEntity = new Tra_TraineeTopicEntity();
                    //                            traineeTopicEntity.TopicID = courseTopic.TopicID;
                    //                            traineeTopicEntity.TraineeID = model.ID;
                    //                            message = traineeTopicServices.Add(traineeTopicEntity);
                    //                        }
                    //                        return model;  
                    //                }

                    //            }

                    //            return model;
                    //        }
                    //    }
                    //}

                }
                if (classEntity != null)
                {
                    if (traineeEntity.Count > 0)
                    {
                        model.ActionStatus = ConstantMessages.WarningProfileHaveClass.ToString().TranslateString();
                        return model;
                    }

                    model = service.UpdateOrCreate<Tra_TraineeEntity, Tra_TraineeModel>(model);

                    var lstCourseTopicByCourseID = lstCourseTopic.Where(s => s.CourseID == classEntity.CourseID).ToList();
                    if (lstCourseTopicByCourseID.Count == 0)
                    {
                        return model;
                    }
                    foreach (var courseTopic in lstCourseTopicByCourseID)
                    {
                        var traineeTopicEntity = new Tra_TraineeTopicEntity();
                        traineeTopicEntity.TopicID = courseTopic.TopicID;
                        traineeTopicEntity.TraineeID = model.ID;
                        model.ActionStatus = traineeTopicServices.Add(traineeTopicEntity);
                    }


                    return model;

                }
            }

            return service.UpdateOrCreate<Tra_TraineeEntity, Tra_TraineeModel>(model);

            



            #endregion
        }
    }
}
