using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Training.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HRM.Business.Main.Domain;

using System.Collections.Generic;

using System.Threading.Tasks;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Setting;
using VnResource.Helper.Utility;
using System.Linq.Dynamic;
using System.Data;
using System.IO;
using System.Xml;

namespace HRM.Business.Training.Domain
{
    public class Tra_ClassServices : BaseService
    {
        // Son.Vo - Thêm học viên vào lớp học
        public string AddTraineeToClass(List<Guid> lstTraineeIDs, Guid classID, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string message = string.Empty;
                var planServices = new Tra_PlanServices();
                #region Tin.Nguyen
                var lstObjPlan = new List<object>();
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
                var lstClass = classServices.GetData<Tra_ClassEntity>(objClass, ConstantSql.hrm_tra_sp_get_Class, UserLogin, ref status).ToList();

                var lstObjTraineeInClass = new List<object>();
                lstObjTraineeInClass.Add(classID);
                var lstTraineeInClass = classServices.GetData<Tra_ClassEntity>(lstObjTraineeInClass, ConstantSql.hrm_cat_sp_get_TraineeByClassID, UserLogin, ref status).ToList();

                var traineeTopicServices = new Tra_TraineeTopicServices();
                var courseTopicServices = new Tra_TraineeTopicServices();
                var objCourseTopic = new List<object>();
                objCourseTopic.Add(1);
                objCourseTopic.Add(int.MaxValue - 1);
                var lstCourseTopic = courseTopicServices.GetData<Tra_CourseTopicEntity>(objCourseTopic, ConstantSql.hrm_tra_sp_get_CourseTopic, UserLogin, ref status);

                var classEntity = lstClass.Where(s => s.ID == classID).FirstOrDefault();
                var profileServices = new Hre_ProfileServices();
                var objProfile = new List<object>();
                objProfile.AddRange(new object[18]);
                objProfile[16] = 1;
                objProfile[17] = int.MaxValue - 1;
                var lstProfileSelected = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).ToList();
                if(lstTraineeIDs != null)
                {
                    lstProfileSelected = lstProfileSelected.Where(s => lstTraineeIDs.Contains(s.ID)).ToList();
                }

                var traineeServices = new Tra_TraineeServices();

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

                foreach (var profileEntity in lstProfileSelected)
                {
                    if (classEntity != null)
                    {
                        if (classEntity.PlanID != null)
                        {
                            var planEntity = lstPlan.Where(s => s.ID == classEntity.PlanID.Value).FirstOrDefault();
                            if (planEntity != null)
                            {
                                var lstPlanDetailByPlanID = lstPlanDetail.Where(s => s.PlanID == planEntity.ID).ToList();

                                foreach (var item in lstPlanDetailByPlanID)
                                {
                                    var lstTraineeInClassForOrg = lstTraineeInClass.Where(s => s.OrgStructureID.Value == item.OrgStructureID.Value).ToList();
                                    //Kiểm tra số lượng của plan detail với sumQuantity của plan
                                    if (planEntity.SumQuantity == lstPlanDetailByPlanID.Sum(s => s.Quantity))
                                    {
                                        if (lstTraineeInClass.Count < lstPlanDetailByPlanID.Sum(s => s.Quantity))
                                        {
                                            // Xử Lý kiểm tra nv có thuộc phòng ban trong plan detail ko
                                            if (profileEntity != null)
                                            {
                                                if (lstTraineeInClassForOrg.Count < item.Quantity)
                                                {
                                                    if (profileEntity.OrgStructureID.Value == item.OrgStructureID.Value)
                                                    {
                                                        var traineeEntity = new Tra_TraineeEntity();
                                                      
                                                        traineeEntity.ProfileID = profileEntity.ID;
                                                        traineeEntity.ClassID = classID;
                                                        traineeEntity.OrgStructureID = profileEntity.OrgStructureID != null ? profileEntity.OrgStructureID.Value : profileEntity.OrgStructureID;

                                                        message = traineeServices.Add(traineeEntity);
                                                        


                                                        var lstCourseTopicByCourseID = lstCourseTopic.Where(s => s.CourseID == classEntity.CourseID).ToList();
                                                        foreach (var courseTopic in lstCourseTopicByCourseID)
                                                        {
                                                            var traineeTopicEntity = new Tra_TraineeTopicEntity();
                                                            traineeTopicEntity.TopicID = courseTopic.TopicID;
                                                            traineeTopicEntity.TraineeID = traineeEntity.ID;
                                                            message = traineeTopicServices.Add(traineeTopicEntity);
                                                        }
                                                        return message;
                                                    }
                                                    message = ConstantMessages.WarningProfileNotDependantOrgStructure.TranslateString();
                                                }
                                                else
                                                {
                                                    message = ConstantMessages.WarningTraineeOverLimit.TranslateString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            message = ConstantMessages.WarningTraineeOverLimit.TranslateString();
                                        }
                                    }
                                    if (planEntity.SumQuantity > lstPlanDetailByPlanID.Sum(s => s.Quantity))
                                    {
                                        var traineeEntity = new Tra_TraineeEntity();

                                        traineeEntity.ProfileID = profileEntity.ID;
                                        traineeEntity.ClassID = classID;
                                        traineeEntity.OrgStructureID = profileEntity.OrgStructureID != null ? profileEntity.OrgStructureID.Value : profileEntity.OrgStructureID;

                                        message = traineeServices.Add(traineeEntity);



                                        var lstCourseTopicByCourseID = lstCourseTopic.Where(s => s.CourseID == classEntity.CourseID).ToList();
                                        foreach (var courseTopic in lstCourseTopicByCourseID)
                                        {
                                            var traineeTopicEntity = new Tra_TraineeTopicEntity();
                                            traineeTopicEntity.TopicID = courseTopic.TopicID;
                                            traineeTopicEntity.TraineeID = traineeEntity.ID;
                                            message = traineeTopicServices.Add(traineeTopicEntity);
                                        }
                                        return message;
                                    }

                                }

                                return message;
                            }
                        }
                    }
                }
                #endregion
                return message;
            }
        }

        public string TransferTraineeForClass(List<Guid> lstTraineeIDs, Guid classID, Guid tranferClassID,string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string message = string.Empty;
                var traineeServices = new Tra_TraineeServices();
                var objTrainee = new List<object>();
                objTrainee.AddRange(new object[15]);
                objTrainee[13] = 1;
                objTrainee[14] = int.MaxValue - 1;
                var lstTrainee = traineeServices.GetData<Tra_TraineeEntity>(objTrainee,ConstantSql.hrm_tra_sp_get_Trainee,UserLogin,ref status).ToList();

                if (lstTraineeIDs != null)
                {
                    lstTrainee = lstTrainee.Where(s => lstTraineeIDs.Contains(s.ID) && s.ClassID == classID).ToList();
                }

                foreach (var item in lstTrainee)
                {
                    item.ClassOldID = classID;
                    item.ClassID = tranferClassID;
                    message =  traineeServices.Edit(item);
                }
                
                return message;
            }
        }


    }
}
