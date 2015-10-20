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
using HRM.Data.Entity.Repositories;

namespace HRM.Business.Training.Domain
{
    public class Tra_TraineeServices : BaseService
    {
        public bool IsAddTraineeDependOnRequirementDetail()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_TRA_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting,string.Empty, ref status);

            if (config == null)
            {
                return false;
            }
            string valueConfig = AppConfig.HRM_TRA_CONFIG_TRAINEE_IN_REQUIREMENTDETAIL.ToString();

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }
        public string ImportTrainee(List<ImportTraineeEntity> lstImport,string UserLogin)
        {
            int NumTrainee = 0;
            var message = string.Empty;

            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var services = new Tra_TraineeServices();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTra_Class = new CustomBaseRepository<Tra_Class>(unitOfWork);
                var repoTra_CourseTopic = new CustomBaseRepository<Tra_CourseTopic>(unitOfWork);
                var repoTra_Trainee = new CustomBaseRepository<Tra_Trainee>(unitOfWork);
                var repoTra_TraineeTopic = new CustomBaseRepository<Tra_TraineeTopic>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var lstTraineeAdd = new List<Tra_TraineeEntity>();
                var lstTraineeTopicAdd = new List<Tra_TraineeTopicEntity>();
                var entity = new List<Tra_Trainee>();
                var lstClassCode = lstImport.Select(m => m.ClassCode).Distinct().ToList();
                var lstClass = repoTra_Class.FindBy(m => m.IsDelete == null &&
                    (lstClassCode.Contains(m.Code)))
                    .Select(m => new { m.ID, m.CourseID, m.Code }).ToList();
                var lstCourseID = lstClass.Select(m => m.CourseID).Distinct().ToList();
                var lstCourseTopic = repoTra_CourseTopic.FindBy(m => m.IsDelete == null
                    && (lstCourseID.Contains(m.CourseID))).Select(m => new { m.ID, m.CourseID, m.TopicID }).ToList();
                var lstProfileCode = lstImport.Select(m => m.CodeEmp).Distinct().ToList();
                var lstProfile = repoHre_Profile.FindBy(m => m.IsDelete == null && lstProfileCode.Contains(m.CodeEmp)).Select(m => new { m.ID, m.CodeEmp }).ToList();

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

                List<Tra_Trainee> lstTrainee = new List<Tra_Trainee>();
                List<Tra_TraineeTopic> lstTraineeTopic = new List<Tra_TraineeTopic>();


                foreach (var item in lstImport)
                {
                    var traClass = lstClass.Where(m => m.Code == item.ClassCode).FirstOrDefault();
                    if (traClass == null)
                    {
                        continue;
                    }
                    var requirementEntity = lstRequirement.Where(s => s.Code == item.RequirementCode).FirstOrDefault();
                    if (requirementEntity == null)
                    {
                        continue;
                    }
                    var requirementDetailEntity = lstRequirementDetail.Where(s => s.CourseID != null && lstCourseID.Contains(s.CourseID.Value) && s.ProfileID != null && !string.IsNullOrEmpty(s.CodeEmp) && item.CodeEmp == s.CodeEmp).FirstOrDefault();
                    if (requirementDetailEntity == null)
                    {
                        continue;
                    }

                    var profile = lstProfile.Where(m => m.CodeEmp == item.CodeEmp).FirstOrDefault();
                    if (profile == null)
                    {
                        continue;
                    }

                    Tra_Trainee trainee = new Tra_Trainee();
                    trainee.ID = Guid.NewGuid();
                    trainee.ClassID = traClass.ID;
                    trainee.ProfileID = profile.ID;
                    trainee.Status = item.Status;
                    lstTrainee.Add(trainee);
                    if (traClass.CourseID != null)
                    {
                        List<Guid> lstTopicIDs = lstCourseTopic.Where(m => m.CourseID == traClass.CourseID).Select(m => m.TopicID).ToList();
                        foreach (var topic in lstTopicIDs)
                        {
                            Tra_TraineeTopic traineeTopic = new Tra_TraineeTopic();
                            traineeTopic.ID = Guid.NewGuid();
                            traineeTopic.Tra_Trainee = trainee;
                            traineeTopic.TraineeID = (Guid?)trainee.ID;
                            traineeTopic.TopicID = topic;
                            lstTraineeTopic.Add(traineeTopic);
                        }
                    }
                    if (requirementDetailEntity != null && traClass != null)
                    {
                        requirementDetailEntity.ClassID = (Guid?)traClass.ID;
                        message = Edit(requirementDetailEntity);
                    }

                }

                lstTraineeAdd = lstTrainee.Translate<Tra_TraineeEntity>();
                lstTraineeTopicAdd = lstTraineeTopic.Translate<Tra_TraineeTopicEntity>();
                NumTrainee = lstTrainee.Count;
                message = Add(lstTraineeAdd);
                message = Add(lstTraineeTopicAdd);
            }

            return message;
        }
        public List<ImportTraineeEntity> GetListTrainee(List<ImportTraineeEntity> lstImport, string UserLogin)
        {
            int NumTrainee = 0;
            var message = string.Empty;
            var lstTraineeData = new List<ImportTraineeEntity>();

            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var services = new Tra_TraineeServices();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTra_Class = new CustomBaseRepository<Tra_Class>(unitOfWork);
                var repoTra_CourseTopic = new CustomBaseRepository<Tra_CourseTopic>(unitOfWork);
                var repoTra_Trainee = new CustomBaseRepository<Tra_Trainee>(unitOfWork);
                var repoTra_TraineeTopic = new CustomBaseRepository<Tra_TraineeTopic>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var lstTraineeTopicAdd = new List<Tra_TraineeTopicEntity>();
                var entity = new List<Tra_Trainee>();
                var lstClassCode = lstImport.Select(m => m.ClassCode).Distinct().ToList();
                var lstClass = repoTra_Class.FindBy(m => m.IsDelete == null &&
                    (lstClassCode.Contains(m.Code)))
                    .Select(m => new { m.ID, m.CourseID, m.Code }).ToList();
                var lstCourseID = lstClass.Select(m => m.CourseID).Distinct().ToList();
                var lstCourseTopic = repoTra_CourseTopic.FindBy(m => m.IsDelete == null
                    && (lstCourseID.Contains(m.CourseID))).Select(m => new { m.ID, m.CourseID, m.TopicID }).ToList();
                var lstProfileCode = lstImport.Select(m => m.CodeEmp).Distinct().ToList();
                var lstProfile = repoHre_Profile.FindBy(m => m.IsDelete == null && lstProfileCode.Contains(m.CodeEmp)).Select(m => new { m.ID, m.CodeEmp }).ToList();



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

                List<Tra_Trainee> lstTrainee = new List<Tra_Trainee>();
                List<Tra_TraineeTopic> lstTraineeTopic = new List<Tra_TraineeTopic>();

                foreach (var item in lstImport)
                {

                    var traClass = lstClass.Where(m => m.Code == item.ClassCode).FirstOrDefault();
                    if (traClass == null)
                    {
                        continue;
                    }
                    var requirementEntity = lstRequirement.Where(s => s.Code == item.RequirementCode).FirstOrDefault();
                    if (requirementEntity == null)
                    {
                        continue;
                    }
                    var requirementDetailEntity = lstRequirementDetail.Where(s => s.CourseID != null && lstCourseID.Contains(s.CourseID.Value) && s.ProfileID != null && !string.IsNullOrEmpty(s.CodeEmp) && item.CodeEmp == s.CodeEmp).FirstOrDefault();
                    if (requirementDetailEntity == null)
                    {
                        continue;
                    }

                    var profile = lstProfile.Where(m => m.CodeEmp == item.CodeEmp).FirstOrDefault();
                    if (profile == null)
                    {
                        continue;
                    }

                    ImportTraineeEntity trainee = new ImportTraineeEntity();

                    trainee.CodeEmp = profile.CodeEmp;
                    trainee.ClassCode = traClass.Code;
                    trainee.RequirementCode = requirementEntity.Code;
                    trainee.Status = item.Status;
                    lstTraineeData.Add(trainee);
                }
            }
            return lstTraineeData;
        }
        public List<Tra_TraineeChangeClassEntity> ChangeClass(List<Tra_TraineeChangeClassEntity> lstTraineeChangeClass)
        {
            var lstTraineeData = new List<Tra_TraineeChangeClassEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var lstClassOldCode = lstTraineeChangeClass.Select(m => m.ClassOldCode).Distinct().ToList();
                var lstClassNewCode = lstTraineeChangeClass.Select(m => m.ClassNewCode).Distinct().ToList();
                var lstCodeEmp = lstTraineeChangeClass.Select(m => m.CodeEmp).Distinct().ToList();

                var lstClassOld = unitOfWork.CreateQueryable<Tra_Class>(m => lstClassOldCode.Contains(m.Code)).Select(m => new { m.ID, m.CourseID, m.Code }).ToList();
                var lstClassNew = unitOfWork.CreateQueryable<Tra_Class>(m => lstClassNewCode.Contains(m.Code)).Select(m => new { m.ID, m.CourseID, m.Code }).ToList();
                var lstTraineeInDb = unitOfWork.CreateQueryable<Tra_Trainee>(m => lstClassOldCode.Contains(m.Tra_Class.Code) && lstCodeEmp.Contains(m.Hre_Profile.CodeEmp)).ToList();
                foreach (var item in lstTraineeChangeClass)
                {
                    string ClassOldID = item.ClassOldCode;
                    string ClassNewID = item.ClassNewCode;

                    Guid CourseOldID = lstClassOld.Where(m => m.Code == ClassOldID).Select(m => m.CourseID).FirstOrDefault();
                    Guid CourseNewID = lstClassNew.Where(m => m.Code == ClassNewID).Select(m => m.CourseID).FirstOrDefault();
                    if (CourseOldID != CourseNewID)
                        continue;
                    //var traineeInDB = lstTraineeInDb.Where(m => m.Hre_Profile.CodeEmp == item.CodeEmp && m.Tra_Class.Code == ClassOldID).FirstOrDefault();
                    //if(traineeInDB!= null)
                    //{
                    //    traineeInDB.Tra_Class1.Code = ClassNewID;
                    //}

                    Tra_TraineeChangeClassEntity trainee = new Tra_TraineeChangeClassEntity();

                    trainee.CodeEmp = item.CodeEmp;
                    trainee.ClassOldCode = item.ClassOldCode;
                    trainee.ClassNewCode = item.ClassNewCode;
                    lstTraineeData.Add(trainee);
                }
            }
             return lstTraineeData;
        }
        public string ImportChangeClass(List<Tra_TraineeChangeClassEntity> lstTraineeChangeClass,string UserLogin)
        {
            string message = string.Empty;
            var lstTraineeData = new List<Tra_TraineeChangeClassEntity>();
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTrainee = new CustomBaseRepository<Tra_Trainee>(unitOfWork);
                var lstClassOldCode = lstTraineeChangeClass.Select(m => m.ClassOldCode).Distinct().ToList();
                var lstClassNewCode = lstTraineeChangeClass.Select(m => m.ClassNewCode).Distinct().ToList();
                var lstCodeEmp = lstTraineeChangeClass.Select(m => m.CodeEmp).Distinct().ToList();

                var traineeServices = new Tra_TraineeServices();


                var lstClassOld = unitOfWork.CreateQueryable<Tra_Class>(m => lstClassOldCode.Contains(m.Code)).Select(m => new { m.ID, m.CourseID, m.Code }).ToList();
                var lstClassNew = unitOfWork.CreateQueryable<Tra_Class>(m => lstClassNewCode.Contains(m.Code)).Select(m => new { m.ID, m.CourseID, m.Code }).ToList();

                var objTrainee = new List<object>();
                objTrainee.AddRange(new object[15]);
                objTrainee[13] = 1;
                objTrainee[14] = int.MaxValue - 1;

                var lstTraineeInDb = GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status).Where(s => lstClassOldCode.Contains(s.ClassCode) && lstCodeEmp.Contains(s.CodeEmp)).ToList();
                
                //var lstTraineeInDb = unitOfWork.CreateQueryable<Tra_Trainee>(m => lstClassOldCode.Contains(m.Tra_Class1.Code) && lstCodeEmp.Contains(m.Hre_Profile.CodeEmp)).ToList();
                //var lstTraineeInDb = repoTrainee.GetAll().Where(s => s.IsDelete != null && lstClassOldCode.Contains(s.Tra_Class1.Code) && lstCodeEmp.Contains(s.CodeEmp)).ToList();


                foreach (var item in lstTraineeChangeClass)
                {
                  //  var traineeEntity = new Tra_TraineeEntity();
                    string ClassOldID = item.ClassOldCode;
                    string ClassNewID = item.ClassNewCode;

                    Guid CourseOldID = lstClassOld.Where(m => m.Code == ClassOldID).Select(m => m.CourseID).FirstOrDefault();
                    Guid CourseNewID = lstClassNew.Where(m => m.Code == ClassNewID).Select(m => m.CourseID).FirstOrDefault();
                    var ClassNewEntity = lstClassNew.Where(m => m.Code == ClassNewID).FirstOrDefault();
                    if (CourseOldID != CourseNewID)
                        continue;
                    var traineeInDB = lstTraineeInDb.Where(m => m.CodeEmp == item.CodeEmp && m.ClassCode == ClassOldID).FirstOrDefault();
                    if (traineeInDB != null)
                    {
                        traineeInDB.ClassID = ClassNewEntity != null ? ClassNewEntity.ID:Guid.Empty;
                     //   traineeEntity = traineeInDB.CopyData<Tra_TraineeEntity>();
                    }
                    message = traineeServices.Edit(traineeInDB);
                    //Tra_TraineeChangeClassEntity trainee = new Tra_TraineeChangeClassEntity();
                    //trainee.CodeEmp = item.CodeEmp;
                    //trainee.ClassOldCode = item.ClassOldCode;
                    //trainee.ClassNewCode = item.ClassNewCode;
                    //lstTraineeData.Add(trainee);
                    //message = ConstantMessages.Succeed.ToString();

                }
            //    unitOfWork.SaveChanges();
                return message;
            }
        }

        public List<Tra_TraineeEntity> GetListTrainees(List<Tra_TraineeEntity> lstistTrainee, List<Guid> lstRequirementTrainIDs)
        {
            var lstTrainees = new List<Tra_TraineeEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var lstrequirementTrainDetails = new List<Tra_RequirementTrainDetail>().Select(d => new
                 {
                     d.ProfileID,
                 }).ToList();
                if (lstRequirementTrainIDs.Count > 0)
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var requirementTrainDetailRepository = new Tra_RequirementTrainDetailRepository(unitOfWork);
                    var lstrequirementTrainDetail = new List<Tra_RequirementTrainDetail>().Select(d => new
                    {
                        d.ProfileID,
                        d.CourseID,
                        d.ClassID,
                        d.RequirementTrainID
                    }).ToList();
                    lstrequirementTrainDetail.AddRange(requirementTrainDetailRepository.FindBy(s => s.IsDelete == null && s.RequirementTrainID != null && lstRequirementTrainIDs.Contains(s.RequirementTrainID.Value)).Select(d => new
                            {
                                d.ProfileID,
                                d.CourseID,
                                d.ClassID,
                                d.RequirementTrainID
                            }).ToList());
                    var lstProfileid = lstrequirementTrainDetail.Select(s => s.ProfileID).Distinct().ToList();
                    var lstcourseid = lstrequirementTrainDetail.Select(s => s.CourseID).Distinct().ToList();
                    var lstclassid = lstrequirementTrainDetail.Select(s => s.ClassID).Distinct().ToList();
                    
                     var requirementTrainRepository = new Tra_RequirementTrainRepository(unitOfWork);
                    var lstrequirementTrain = new List<Tra_RequirementTrain>().Select(d => new
                    {
                        d.ID,
                        d.RequirementTrainName
                    }).ToList();
                    lstrequirementTrain.AddRange(requirementTrainRepository.FindBy(s => s.IsDelete == null && lstRequirementTrainIDs.Contains(s.ID)).Select(d => new
                            {
                                d.ID,
                        d.RequirementTrainName
                            }).ToList());

                    if (lstistTrainee != null)
                    {
                        if (lstProfileid.Count > 0)
                        {
                            lstistTrainee = lstistTrainee.Where(s => lstProfileid.Contains(s.ProfileID)).ToList();
                        }
                        //if (lstcourseid.Count > 0)
                        //{
                        //    lstistTrainee = lstistTrainee.Where(s => lstcourseid.Contains(s.CourseID)).ToList();
                        //}
                        //if (lstclassid.Count > 0)
                        //{
                        //    lstistTrainee = lstistTrainee.Where(s => lstclassid.Contains(s.ClassID)).ToList();
                        //}
                    }
                    foreach (var trainee in lstistTrainee)
                    {
                        var requiredtraintdetail = lstrequirementTrainDetail.Where(s => s.ProfileID == trainee.ProfileID).FirstOrDefault();
                        if(requiredtraintdetail != null)
                        {
                            var requiredTrainbyTrainee = lstrequirementTrain.Where(s => requiredtraintdetail.RequirementTrainID == s.ID).FirstOrDefault();
                            if (requiredTrainbyTrainee != null)
                            {
                                trainee.RequirementTrainName = requiredTrainbyTrainee.RequirementTrainName;
                            }
                        }
                       
                    }
                }
                lstTrainees = lstistTrainee;
                return lstTrainees;
            }
        }
    }
}
