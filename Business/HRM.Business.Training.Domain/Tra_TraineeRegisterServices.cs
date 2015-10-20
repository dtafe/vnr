using System.Threading.Tasks;
using HRM.Data.Entity.Repositories;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Category.Domain;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Training.Models;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Business.HrmSystem.Models;
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
using System.Data;
using VnResource.Helper.Data;



namespace HRM.Business.Training.Domain
{
    public class Tra_TraineeRegisterServices : BaseService
    {

        #region Duyệt
        public  string ApprovedTraineeRegister (List<Guid> selectedIds , Guid UserId)
        {
            using (var context = new VnrHrmDataContext())
            {
                var message = string.Empty;
                var status = string.Empty;
                var UserLogin = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var firstApproved = EnumDropDown.TraineeRegisterStatus.E_SUBMIT.ToString();
                var approved = EnumDropDown.TraineeRegisterStatus.E_APPROVED.ToString();
                var objTraineeRegister = new List<object>();
                objTraineeRegister.AddRange(new object[12]);
                objTraineeRegister[10] = 1;
                objTraineeRegister[11] = int.MaxValue - 1;
                var lstTraineeRegister = GetData<Tra_TraineeRegisterEntity>(objTraineeRegister, ConstantSql.hrm_tra_sp_get_TraineeRegister,UserLogin, ref status).ToList();

               var repo_UserApproved = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
               var lstUserApproved = repo_UserApproved.GetAll().Where(s => s.IsDelete == null).ToList();

                if (selectedIds.Count > 0)
                {
                    lstTraineeRegister = lstTraineeRegister.Where(s => selectedIds.Contains(s.ID)).ToList();
                }

                var traineeServices = new Tra_TraineeServices();
                var isTraineeInRequireDetail = traineeServices.IsAddTraineeDependOnRequirementDetail();
                var traineeTopicServices = new Tra_TraineeTopicServices();
                var planServices = new Tra_PlanServices();
                var lstObjPlan = new List<object>();
                lstObjPlan.Add(null);
                lstObjPlan.Add(null);
                lstObjPlan.Add(null);
                lstObjPlan.Add(null);
                lstObjPlan.Add(1);
                lstObjPlan.Add(int.MaxValue - 1);
                var lstPlan = planServices.GetData<Tra_PlanEntity>(lstObjPlan, ConstantSql.hrm_tra_sp_get_Plan,UserLogin, ref status).ToList();


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

                //var lstObjTraineeInClass = new List<object>();
                //lstObjTraineeInClass.Add(model.ClassID);
                //var lstTraineeInClass = classServices.GetData<Tra_ClassEntity>(lstObjTraineeInClass, ConstantSql.hrm_cat_sp_get_TraineeByClassID, ref status).ToList();

                var courseTopicServices = new Tra_TraineeTopicServices();
                var objCourseTopic = new List<object>();
                objCourseTopic.Add(1);
                objCourseTopic.Add(int.MaxValue - 1);
                var lstCourseTopic = courseTopicServices.GetData<Tra_CourseTopicEntity>(objCourseTopic, ConstantSql.hrm_tra_sp_get_CourseTopic, UserLogin, ref status);


                //var classEntity = lstClass.Where(s => s.ID == model.ClassID).FirstOrDefault();
                var profileServices = new Hre_ProfileServices();

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

                foreach (var registerEntity in lstTraineeRegister)
                {
                    var classEntity = lstClass.Where(s => s.ID == registerEntity.ClassID).FirstOrDefault();
                    var traineeEntityInClass = lstObjTrainee.Where(s => s.ClassID == registerEntity.ClassID && registerEntity.ProfileID == s.ProfileID).ToList();

                    #region người duyệt đầu
                    if (registerEntity.UserApproveID1 != null && registerEntity.UserApproveID1.Value == UserId)
                    {
                        var userApprovedEntity = lstUserApproved.Where(s => registerEntity.UserApproveID1.Value == s.UserApproveID && s.Type == ApproveType.E_TRA_TRAINEEREGISTER.ToString()).FirstOrDefault();
                        if (userApprovedEntity != null)
                        {
                            if (userApprovedEntity.IsMasterApprove == null || userApprovedEntity.IsMasterApprove.Value == false)
                            {
                                registerEntity.Status = firstApproved;
                                message = Edit(registerEntity);
                            }
                        }
                    }
	                #endregion

                    #region người duyệt cuối
                    if (registerEntity.UserApproveID2 != null && registerEntity.UserApproveID2.Value == UserId)
                    {
                        var userApprovedEntity = lstUserApproved.Where(s => registerEntity.UserApproveID2.Value == s.UserApproveID && s.Type == ApproveType.E_TRA_TRAINEEREGISTER.ToString()).FirstOrDefault();
                        if (userApprovedEntity != null)
                        {
                            if (userApprovedEntity.IsMasterApprove != null && userApprovedEntity.IsMasterApprove.Value)
                            {
                                #region Cập Nhập Status lại vào bảng Tra_TraineeRegister
                                registerEntity.Status = approved;
                                message = Edit(registerEntity);
                                #endregion

                                #region Tạo hv vào bảng Tra_Trainee

                                if (isTraineeInRequireDetail)
                                {
                                    if (traineeEntityInClass.Count > 0)
                                    {
                                        message = ConstantMessages.WarningProfileHaveClass.ToString().TranslateString();
                                        return message;
                                    }
                                    if (registerEntity.ClassID != null)
                                    {
                                        if (classEntity != null && classEntity.PlanID != null)
                                        {
                                            var lstRequirementByPlanID = lstRequirement.Where(s => s.PlanID != null && s.PlanID.Value == classEntity.PlanID).ToList();
                                            foreach (var item in lstRequirementByPlanID)
                                            {
                                                var requirementDetailEntity = lstRequirementDetail.Where(s => item.ID == s.RequirementTrainID && s.CourseID == classEntity.CourseID && s.ProfileID == item.ProfileID).FirstOrDefault();
                                                if (requirementDetailEntity != null)
                                                {
                                                    var traineeEntity = new Tra_TraineeEntity();
                                                    traineeEntity.ProfileID = registerEntity.ProfileID.Value;
                                                    traineeEntity.ClassID = registerEntity.ClassID.Value;
                                                    traineeEntity.TraineeRegisterID = (Guid?)registerEntity.ID;
                                                    traineeServices.Add(traineeEntity);

                                                    var lstCourseTopicByCourseID = lstCourseTopic.Where(s => s.CourseID == classEntity.CourseID).ToList();
                                                    foreach (var courseTopic in lstCourseTopicByCourseID)
                                                    {
                                                        var traineeTopicEntity = new Tra_TraineeTopicEntity();
                                                        traineeTopicEntity.TopicID = courseTopic.TopicID;
                                                        traineeTopicEntity.TraineeID = traineeEntity.ID;
                                                        message = traineeTopicServices.Add(traineeTopicEntity);
                                                    }

                                                }
                                                else
                                                {
                                                    message = ConstantMessages.WarningProfileNotInRequirement.ToString().TranslateString();
                                                }
                                            }
                                        }
                                    }
                                }

                                if (classEntity != null)
                                {
                                    if (traineeEntityInClass.Count > 0)
                                    {
                                        message = ConstantMessages.WarningProfileHaveClass.ToString().TranslateString();
                                        return message;
                                    }

                                    var traineeEntity = new Tra_TraineeEntity();
                                    traineeEntity.ProfileID = registerEntity.ProfileID.Value;
                                    traineeEntity.ClassID = registerEntity.ClassID.Value;
                                    traineeEntity.TraineeRegisterID = (Guid?)registerEntity.ID;
                                    traineeServices.Add(traineeEntity);

                                    var lstCourseTopicByCourseID = lstCourseTopic.Where(s => s.CourseID == classEntity.CourseID).ToList();
                                    foreach (var courseTopic in lstCourseTopicByCourseID)
                                    {
                                        var traineeTopicEntity = new Tra_TraineeTopicEntity();
                                        traineeTopicEntity.TopicID = courseTopic.TopicID;
                                        traineeTopicEntity.TraineeID = traineeEntity.ID;
                                        message = traineeTopicServices.Add(traineeTopicEntity);
                                    }



                                }
                                #endregion
                            }
                        }
                    }
                    #endregion

                }
                return message;
            }
            
        }
        #endregion

        #region Từ Chối
        public string ActionRejectTraineeRegister(List<Guid> selectedIds, Guid UserId, string Reason)
        {
            using (var context = new VnrHrmDataContext())
            {
                var message = string.Empty;
                var status = string.Empty;
                var UserLogin = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var objTraineeRegister = new List<object>();
                objTraineeRegister.AddRange(new object[12]);
                objTraineeRegister[10] = 1;
                objTraineeRegister[11] = int.MaxValue - 1;
                var lstTraineeRegister = GetData<Tra_TraineeRegisterEntity>(objTraineeRegister, ConstantSql.hrm_tra_sp_get_TraineeRegister, UserLogin, ref status).ToList();

                var repo_UserApproved = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var lstUserApproved = repo_UserApproved.GetAll().Where(s => s.IsDelete != null).ToList();

                if (selectedIds.Count > 0)
                {
                    lstTraineeRegister = lstTraineeRegister.Where(s => selectedIds.Contains(s.ID)).ToList();
                }

                foreach (var item in lstTraineeRegister)
                {
                    #region Check Loại Người Duyệt
                    if (item.UserApproveID1 != null && item.UserApproveID1.Value == UserId)
                    {
                    
                                item.Status = EnumDropDown.TraineeRegisterStatus.E_REJECTED.ToString();
                                item.Reason1 = Reason;
                                message = Edit(item);
                            
                          
                        
                    }
                    #endregion

                    #region Người duyệt cuối
                    if (item.UserApproveID2 != null && item.UserApproveID2.Value == UserId)
                    {
                      
                                item.Status = EnumDropDown.TraineeRegisterStatus.E_REJECTED.ToString();
                                item.Reason2 = Reason;
                                message = Edit(item);
                            
                            
                        
                    }
                    #endregion
                }
                return message;
            }
        }
        #endregion

        #region Hủy
        public string ActionCancelTraineeRegister(List<Guid> selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                var message = string.Empty;
                var status = string.Empty;
                var UserLogin = string.Empty;
                var objTraineeRegister = new List<object>();
                objTraineeRegister.AddRange(new object[12]);
                objTraineeRegister[10] = 1;
                objTraineeRegister[11] = int.MaxValue - 1;
                var lstTraineeRegister = GetData<Tra_TraineeRegisterEntity>(objTraineeRegister, ConstantSql.hrm_tra_sp_get_TraineeRegister, UserLogin, ref status).ToList();
                if (selectedIds.Count > 0)
                {
                    lstTraineeRegister = lstTraineeRegister.Where(s => selectedIds.Contains(s.ID)).ToList();
                }

                var traineeServices = new Tra_TraineeServices();
                var objTrainee = new List<object>();
                objTrainee.AddRange(new object[15]);
                objTrainee[13] = 1;
                objTrainee[14]= int.MaxValue - 1;
                var lstObjTrainee = traineeServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status).ToList();

                foreach (var item in lstTraineeRegister)
                {
                    var entity = lstObjTrainee.Where(s => s.TraineeRegisterID != null && item.ID == s.TraineeRegisterID.Value && item.ProfileID.Value == s.ProfileID && item.ClassID.Value == item.ClassID).FirstOrDefault();
                    if (entity == null)
                    {
                        item.Status = EnumDropDown.TraineeRegisterStatus.E_CANCEL.ToString();
                        message = Edit(item);
                    }
                }
                return message;
            }
        }
        #endregion
    }
}
