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
using System.Data;
using System.Collections.Generic;
using HRM.Business.Training.Domain;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Tra_AddPointController : ApiController
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
        public Tra_AddPointModel GetById(string id)
        {
            Tra_AddPointModel result = new Tra_AddPointModel();
            List<Guid> SelectedIds = new List<Guid>();

            if (!string.IsNullOrEmpty(id))
            {
                SelectedIds = id.Split(',').Select(s => Guid.Parse(s)).ToList();
            }

            if (SelectedIds != null && SelectedIds.Count > 0)
            {
               
                //Guid[] traineeID = null;
                var message = string.Empty;
                string status = string.Empty;

                List<Guid> lstClassID = new List<Guid>();
                List<Guid> lstTraineeID = new List<Guid>();
                #region load data
                var traineeServices = new Tra_TraineeServices();
                var objTrainee = new List<object>();
                objTrainee.AddRange(new object[15]);
                objTrainee[13] = 1;
                objTrainee[14] = int.MaxValue - 1;
                List<Tra_TraineeEntity> lstTrainee = traineeServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status);


                var courseSevices = new Tra_CourseServices();
                var objCoures = new List<object>();
                objCoures.AddRange(new object[11]);
                objCoures[9] = 1;
                objCoures[10] = int.MaxValue - 1;
                List<Tra_CourseEntity> lstCourse = courseSevices.GetData<Tra_CourseEntity>(objCoures, ConstantSql.hrm_tra_sp_get_Course, UserLogin, ref status);

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
                List<Tra_ClassEntity> lstClass = classServices.GetData<Tra_ClassEntity>(objClass, ConstantSql.hrm_tra_sp_get_Class, UserLogin, ref status);

                var traineeTopicServices = new Tra_TraineeTopicServices();
                var objTraineeTopic = new List<object>();
                objTraineeTopic.Add(1);
                objTraineeTopic.Add(int.MaxValue -1);
                var lstTraineeTopic = traineeTopicServices.GetData<Tra_TraineeTopicEntity>(objTraineeTopic, ConstantSql.hrm_tra_sp_get_TraineeTopic, UserLogin, ref status).ToList();

                var scoreTopicServices = new Tra_ScoreTopicServices();
                var objScoreTopic = new List<object>();
                objScoreTopic.Add(1);
                objScoreTopic.Add(int.MaxValue - 1);
                var lstScoreTopic = scoreTopicServices.GetData<Tra_ScoreTopicEntity>(objScoreTopic, ConstantSql.hrm_tra_sp_get_ScoreTopic, UserLogin, ref status).ToList();

                var courseTopicServices = new Tra_TraineeTopicServices();
                var objCourseTopic = new List<object>();
                objCourseTopic.Add(1);
                objCourseTopic.Add(int.MaxValue - 1);
                List<Tra_CourseTopicEntity> lstCourseTopic = courseTopicServices.GetData<Tra_CourseTopicEntity>(objCourseTopic, ConstantSql.hrm_tra_sp_get_CourseTopic, UserLogin, ref status);

                var traineeScoreServices = new Tra_TraineeScoreServices();
                var objTraineeScore = new List<object>();
                objTraineeScore.Add(1);
                objTraineeScore.Add(int.MaxValue - 1);
                var lstTraineeScore = traineeScoreServices.GetData<Tra_TraineeScoreEntity>(objTraineeScore, ConstantSql.hrm_tra_sp_get_TraineeScore, UserLogin, ref status).ToList();

                if (SelectedIds != null)
                {
                    lstTrainee = lstTrainee.Where(s => SelectedIds.Contains(s.ID)).ToList();
                    lstTraineeID = lstTrainee.Select(s => s.ID).ToList();
                    lstClassID = lstTrainee.Select(s => s.ClassID).Distinct().ToList();
                }
                if (lstClassID.Count > 1)
                {
                    DataTable tb = new DataTable();
                    result.ActionStatus = ConstantMessages.PlsChooseTraineeSameClass.ToString();
                    result.Ids = id;
                    result.Table = tb;
                    return result;
                }
                DataTable table = new DataTable();
                table.Columns.Add(Tra_TraineeScoreEntity.FieldNames.ID);
                table.Columns.Add(Tra_TraineeScoreEntity.FieldNames.CodeEmp);
                table.Columns.Add(Tra_TraineeScoreEntity.FieldNames.ProfileName);
                if (lstClassID.Count == 1)
                {
                    var isSimple = false;
                    Guid firstClass = lstClassID[0];
                    var courseEntity = lstClass.Where(s => s.ID == firstClass).Select(s => new { s.CourseID, s.IsComplex }).FirstOrDefault();

                    if (courseEntity != null)
                    {
                        var lstCourseTopicByCourseID = lstCourseTopic.Where(s => courseEntity.CourseID == s.CourseID).ToList();
                        if (courseEntity.IsComplex == null || courseEntity.IsComplex == false)
                        {
                            isSimple = true;
                        }
                        if (lstCourseTopicByCourseID.Count == 0)
                        {
                            DataTable tb = new DataTable();
                            result.ActionStatus = ConstantMessages.CourseNoHaveTopic.ToString();
                            result.Ids = id;
                            result.Table = tb;
                            return result;
                        }
                        if (isSimple == false)
                        {
                           
                            foreach (var trainee in lstTrainee)
                            {
                                DataRow dr = table.NewRow();
                                dr[Tra_TraineeScoreEntity.FieldNames.ID] = trainee.ID != null ? trainee.ID : Guid.Empty;
                                dr[Tra_TraineeScoreEntity.FieldNames.CodeEmp] = trainee.CodeEmp != null ? trainee.CodeEmp : string.Empty;
                                dr[Tra_TraineeScoreEntity.FieldNames.ProfileName] = trainee.ProfileName != null ? trainee.ProfileName : string.Empty;
                                var lstTopicbyCourseID = lstCourseTopic.Where(s => s.CourseID == courseEntity.CourseID).Select(s => new { s.CourseID, s.TopicID, s.IsComplex, s.Code }).ToList();
                                foreach (var item in lstTopicbyCourseID)
                                {
                                    
                                    var traineeTopicEnity = lstTraineeTopic.Where(s => s.TraineeID == trainee.ID && item.TopicID == s.TopicID).FirstOrDefault();
                                  
                                    var lstTraineeScoreByTraineeTopicID = lstTraineeScore.Where(s => traineeTopicEnity != null && s.TraineeTopicID == traineeTopicEnity.ID).ToList(); 
                                    var classeEntity = lstClass.Where(s => s.ID == trainee.ClassID).FirstOrDefault();
                                   
                                    Double value = 0;

                                    if (item.IsComplex == null || item.IsComplex == true)
                                    {
                                        isSimple = true;
                                        if (classeEntity != null)
                                        {
                                            var courseEntityByCourseID = lstCourse.Where(s => s.ID == classeEntity.CourseID).FirstOrDefault();
                                            if (courseEntityByCourseID != null)
                                            {
                                                var lstScoreTopicByTopicID = lstScoreTopic.Where(s => item.TopicID == s.TopicID).OrderBy(s => s.DateCreate).ToList();
                                                foreach (var scoreTopic in lstScoreTopicByTopicID)
                                                {
                                                    var traineeScoreEntity = lstTraineeScore.Where(s => traineeTopicEnity != null && s.ScoreTypeID == scoreTopic.ScoreTypeID && traineeTopicEnity.ID == s.TraineeTopicID).FirstOrDefault();
                                                    if (!table.Columns.Contains(item.Code + "_" + scoreTopic.Code))
                                                    {
                                                        table.Columns.Add(item.Code + "_" + scoreTopic.Code);
                                                    }
                                                    if (table.Columns.Contains(item.Code + "_" + scoreTopic.Code))
                                                    {
                                                        if (traineeScoreEntity != null && traineeScoreEntity.Score != null)
                                                        {
                                                            dr[item.Code + "_" + scoreTopic.Code] = traineeScoreEntity != null ? traineeScoreEntity.Score.Value : 0;
                                                        }
                                                        else
                                                        {
                                                            dr[item.Code + "_" + scoreTopic.Code] = null;
                                                        }

                                                    }
                                                }
                                               
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        isSimple = false;
                                        break;
                                    }
                                }
                                table.Rows.Add(dr);
                            }
                            //  var a = table;
                        }
                    }
                }
                result.Table = table;
                return result;
                #endregion
            }
            return null;
        }
	}
}