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
using System.Data;
using System;
using System.Collections.Generic;
using HRM.Data.Entity;
using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;


namespace HRM.Business.Training.Domain
{
    public class Tra_TraineeScoreServices : BaseService
    {
        #region Get Topic
        public DataTable CreateAddPointSchema(Guid[] selectedIds)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Tra_TraineeScoreEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Tra_TraineeScoreEntity.FieldNames.ProfileName);
            
            return tb;
        }
        public DataTable GetTopicByTraineeID(Guid[] selectedIds)
        {
            DataTable table = CreateAddPointSchema(selectedIds);
            return table;
        }

        public List<Tra_TraineeScoreEntity> GetListTraineeScore(List<Tra_TraineeScoreImport> lstTraineeScore, out List<Tra_TraineeScoreImport> DataReviewOutput)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTra_Class = new CustomBaseRepository<Tra_Class>(unitOfWork);
                var repoTra_Trainee = new CustomBaseRepository<Tra_Trainee>(unitOfWork);
                var repoTra_TraineeTopic = new CustomBaseRepository<Tra_TraineeTopic>(unitOfWork);
                var repoTra_TraineeScore = new CustomBaseRepository<Tra_TraineeScore>(unitOfWork);
                var repoTra_ScoreType = new CustomBaseRepository<Tra_ScoreType>(unitOfWork);
                List<Tra_TraineeScoreEntity> entity = new List<Tra_TraineeScoreEntity>();
                List<Tra_TraineeScore> result = new List<Tra_TraineeScore>();
                DataReviewOutput = new List<Tra_TraineeScoreImport>();
                List<string> ClassCode = lstTraineeScore.Select(m => m.ClassCode).Distinct().ToList();
                //Lấy DS ClassID
                List<Guid> ClassID = repoTra_Class.GetAll().Where(m => m.IsDelete == null && ClassCode.Contains(m.Code)).Select(m => m.ID).ToList<Guid>();
                //Lấy DS TraineeID
                List<Guid> TraineeID = repoTra_Trainee.GetAll().Where(m => m.IsDelete == null && ClassID.Contains(m.ClassID)).Select(m => m.ID).ToList<Guid>();
                //Lấy DS TraineetopicID
                List<Guid> lstTraineeTopicID = repoTra_TraineeTopic.GetAll().Where(m => m.IsDelete == null && TraineeID.Contains(m.TraineeID ?? Guid.Empty)).Select(m => m.ID).ToList<Guid>();
                //Lấy DS TraineeTopic
                var TraineeTopicAll = repoTra_TraineeTopic.GetAll().Where(m => m.IsDelete == null && lstTraineeTopicID.Contains(m.ID))
                    .Select(m => new { m.ID, TopicCode = m.Cat_Topic.Code, CodeEmp = m.Tra_Trainee.Hre_Profile.CodeEmp, ClassCode = m.Tra_Trainee.Tra_Class.Code }).ToList();
                //Lấy DS TraineeScore
                List<Tra_TraineeScore> LstTraineeScoreAll = repoTra_TraineeScore.GetAll().Where(m => m.IsDelete == null && lstTraineeTopicID.Contains(m.TraineeTopicID ?? Guid.Empty)).ToList<Tra_TraineeScore>();
                List<string> ScoreTypeCode = lstTraineeScore.Select(m => m.TypeScore).Distinct().ToList<string>();
                List<Tra_ScoreType> lstTypeScoreAll = repoTra_ScoreType.GetAll().Where(m => m.IsDelete == null && ScoreTypeCode.Contains(m.Code)).ToList<Tra_ScoreType>();
                var stt = 1;
                foreach (var TraineeScore in lstTraineeScore)
                {
                    var TraineeTopic = TraineeTopicAll.Where(m =>  m.CodeEmp == TraineeScore.CodeEmp && m.TopicCode == TraineeScore.TopicCode).FirstOrDefault();
                    Tra_ScoreType ScoreType = lstTypeScoreAll.Where(m => m.Code == TraineeScore.TypeScore).FirstOrDefault();
                    if (TraineeTopic == null) //Loi import
                    {
                        TraineeScore.Stt = stt++;
                        TraineeScore.TypeError = "Không Có Môn Học Của Học Viên";
                        DataReviewOutput.Add(TraineeScore);
                    }
                    else if (ScoreType == null)
                    {
                        TraineeScore.Stt = stt++;
                        TraineeScore.TypeError = "Không Có Loại Điểm Của Môn Học";
                        DataReviewOutput.Add(TraineeScore);
                    }
                    else
                    {
                        TraineeScore.Stt = stt++;
                        DataReviewOutput.Add(TraineeScore);
                        //Kiem tra diem da co trong DB chua
                        //neu co roi thi update neu chua thi tao moi
                        Guid TraineeTopicID = TraineeTopic.ID;
                        Tra_TraineeScore TraineeScore_Current = LstTraineeScoreAll.Where(m => m.TraineeTopicID == TraineeTopicID && m.ScoreTypeID == ScoreType.ID).FirstOrDefault();
                        if (TraineeScore_Current == null)
                        {
                            TraineeScore_Current = new Tra_TraineeScore();
                            TraineeScore_Current.ID = Guid.NewGuid();
                        }
                        TraineeScore_Current.TraineeTopicID = TraineeTopicID;
                        TraineeScore_Current.ScoreTypeID = ScoreType.ID;
                        TraineeScore_Current.Score = TraineeScore.Score;
                        result.Add(TraineeScore_Current);
                    }
                    entity = result.Translate<Tra_TraineeScoreEntity>();
                }
                return entity;
            }

            
        }

        public string SaveListTraineeScore(List<Tra_TraineeScoreImport> lstTraineeScore, out List<Tra_TraineeScoreImport> DataReviewOutput)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoTra_Class = new CustomBaseRepository<Tra_Class>(unitOfWork);
                var repoTra_Trainee = new CustomBaseRepository<Tra_Trainee>(unitOfWork);
                var repoTra_TraineeTopic = new CustomBaseRepository<Tra_TraineeTopic>(unitOfWork);
                var repoTra_TraineeScore = new CustomBaseRepository<Tra_TraineeScore>(unitOfWork);
                var repoTra_ScoreType = new CustomBaseRepository<Tra_ScoreType>(unitOfWork);
                var traineeScoreServices = new Tra_TraineeScoreServices();
                string message = string.Empty;

                List<Tra_TraineeScoreEntity> entityNew = new List<Tra_TraineeScoreEntity>();
                List<Tra_TraineeScore> entityUpdate = new List<Tra_TraineeScore>();
                List<Tra_TraineeScore> resultNew = new List<Tra_TraineeScore>();
                List<Tra_TraineeScore> resultUpdate= new List<Tra_TraineeScore>();
                DataReviewOutput = new List<Tra_TraineeScoreImport>();
                List<string> ClassCode = lstTraineeScore.Select(m => m.ClassCode).Distinct().ToList();
                //Lấy DS ClassID
                List<Guid> ClassID = repoTra_Class.GetAll().Where(m => m.IsDelete == null && ClassCode.Contains(m.Code)).Select(m => m.ID).ToList<Guid>();
                //Lấy DS TraineeID
                List<Guid> TraineeID = repoTra_Trainee.GetAll().Where(m => m.IsDelete == null && ClassID.Contains(m.ClassID)).Select(m => m.ID).ToList<Guid>();
                //Lấy DS TraineetopicID
                List<Guid> lstTraineeTopicID = repoTra_TraineeTopic.GetAll().Where(m => m.IsDelete == null && TraineeID.Contains(m.TraineeID ?? Guid.Empty)).Select(m => m.ID).ToList<Guid>();
                //Lấy DS TraineeTopic
                var TraineeTopicAll = repoTra_TraineeTopic.GetAll().Where(m => m.IsDelete == null && lstTraineeTopicID.Contains(m.ID))
                    .Select(m => new { m.ID, TopicCode = m.Cat_Topic.Code, CodeEmp = m.Tra_Trainee.Hre_Profile.CodeEmp, ClassCode = m.Tra_Trainee.Tra_Class.Code });
                //Lấy DS TraineeScore
                List<Tra_TraineeScore> LstTraineeScoreAll = repoTra_TraineeScore.GetAll().Where(m => m.IsDelete == null && lstTraineeTopicID.Contains(m.TraineeTopicID ?? Guid.Empty)).ToList<Tra_TraineeScore>();
                List<string> ScoreTypeCode = lstTraineeScore.Select(m => m.TypeScore).Distinct().ToList<string>();
                List<Tra_ScoreType> lstTypeScoreAll = repoTra_ScoreType.GetAll().Where(m => m.IsDelete == null && ScoreTypeCode.Contains(m.Code)).ToList<Tra_ScoreType>();
                var stt = 1;
                foreach (var TraineeScore in lstTraineeScore)
                {
                    var TraineeTopic = TraineeTopicAll.Where(m =>  m.CodeEmp == TraineeScore.CodeEmp && m.TopicCode == TraineeScore.TopicCode).FirstOrDefault();
                    Tra_ScoreType ScoreType = lstTypeScoreAll.Where(m => m.Code == TraineeScore.TypeScore).FirstOrDefault();
                    if (TraineeTopic == null) //Loi import
                    {
                        TraineeScore.Stt = stt++;
                        TraineeScore.TypeError = "Không Có Môn Học Của Học Viên";
                        DataReviewOutput.Add(TraineeScore);
                    }
                    else if (ScoreType == null)
                    {
                        TraineeScore.Stt = stt++;
                        TraineeScore.TypeError = "Không Có Loại Điểm Của Môn Học";
                        DataReviewOutput.Add(TraineeScore);
                    }
                    else
                    {
                        TraineeScore.Stt = stt++;
                        DataReviewOutput.Add(TraineeScore);
                        //Kiem tra diem da co trong DB chua
                        //neu co roi thi update neu chua thi tao moi
                        Guid TraineeTopicID = TraineeTopic.ID;
                        Tra_TraineeScore TraineeScore_Current = LstTraineeScoreAll.Where(m => m.TraineeTopicID == TraineeTopicID && m.ScoreTypeID == ScoreType.ID).FirstOrDefault();
                        bool isNew = false;
                        if (TraineeScore_Current == null)
                        {
                            isNew = true;
                            TraineeScore_Current = new Tra_TraineeScore();
                            TraineeScore_Current.ID = Guid.NewGuid();
                        }
                        TraineeScore_Current.TraineeTopicID = TraineeTopicID;
                        TraineeScore_Current.ScoreTypeID = ScoreType.ID;
                        TraineeScore_Current.Score = TraineeScore.Score;
                        if (isNew)
                        {
                            resultNew.Add(TraineeScore_Current);
                        }
                        else
                        {
                            var entityEdit = new Tra_TraineeScoreEntity 
                            {
                                ID = TraineeScore_Current.ID,
                                TraineeTopicID = TraineeScore_Current.TraineeTopicID,
                                ScoreTypeID = ScoreType.ID,
                                Score = TraineeScore.Score

                            };
                            
                            resultUpdate.Add(TraineeScore_Current);
                            message = traineeScoreServices.Edit(entityEdit);
                        }

                    }
                }
                entityNew = resultNew.Translate<Tra_TraineeScoreEntity>();
                entityUpdate = resultUpdate.Translate<Tra_TraineeScore>();
                if (entityNew.Count > 0)
                {
                    message += traineeScoreServices.Add(entityNew);
                
                }
              
                
              
                return message;
            }


        }



        #endregion
    }
}
