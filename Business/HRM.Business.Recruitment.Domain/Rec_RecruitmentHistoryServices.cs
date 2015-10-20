using System;
using System.Linq.Dynamic;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Business.Recruitment.Models;

namespace HRM.Business.Recruitment.Domain
{
    public class Rec_RecruitmentHistoryServices : BaseService
    {
        public void ImportRecruitmentHistory(string candidateIds, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_RecruitmentCampaignRepository(unitOfWork);
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                var lstIds = candidateIds.Split(',');
                List<Guid> listID = new List<Guid>();
                for (int i = 0; i < lstIds.Length; i++)
                {
                    try
                    {
                        listID.Add(Guid.Parse(lstIds[i].ToString()));
                    }
                    catch
                    { }
                }
                BaseService baseService = new BaseService();
                string status = string.Empty;
                var listCandidates = repoCandidate.FindBy(x => listID.Contains(x.ID)).ToList();
                foreach (var candidateModel in listCandidates)
                {
                    List<object> lstObjhistory = new List<object>();
                    lstObjhistory.Add(candidateModel.IdentifyNumber);
                    lstObjhistory.Add(candidateModel.CandidateName);
                    lstObjhistory.Add(candidateModel.DateOfBirth);
                    lstObjhistory.Add(candidateModel.DateApply);
                    
                    var recruimentHistory = baseService.ActionData<Rec_RecruitmentHistoryEntity>(lstObjhistory, ConstantSql.hrm_rec_sp_checkduplidaterecruimentHistory, true, UserLogin, ref status).FirstOrDefault();
                    if (recruimentHistory != null)
                    {
                        recruimentHistory.CandidateName = candidateModel.CandidateName;
                        recruimentHistory.DateOfBirth = candidateModel.DateOfBirth;
                        recruimentHistory.Gender = candidateModel.Gender;
                        recruimentHistory.Phone = candidateModel.Phone;
                        recruimentHistory.Mobile = candidateModel.Mobile;
                        recruimentHistory.Email = candidateModel.Email;
                        recruimentHistory.YearOfExperience = candidateModel.YearOfExperience.Value != null ? candidateModel.YearOfExperience.Value : 0;
                        recruimentHistory.PositionID = candidateModel.PositionID;
                        recruimentHistory.JobVacancyID = candidateModel.JobVacancyID;
                        recruimentHistory.DateApply = candidateModel.DateApply;
                        recruimentHistory.ScorePotential = candidateModel.ScorePotential;
                        recruimentHistory.HealthStatus = candidateModel.HealthStatus;
                        recruimentHistory.IdentifyNumber = candidateModel.IdentifyNumber;
                        baseService.Edit(recruimentHistory);
                    }
                    else
                    {
                        Rec_RecruitmentHistoryEntity recruimentHistoryentity = new Rec_RecruitmentHistoryEntity();
                        recruimentHistoryentity.CandidateID = candidateModel.ID;
                        recruimentHistoryentity.CandidateName = candidateModel.CandidateName;
                        recruimentHistoryentity.DateOfBirth = candidateModel.DateOfBirth;
                        recruimentHistoryentity.Gender = candidateModel.Gender;
                        recruimentHistoryentity.Phone = candidateModel.Phone;
                        recruimentHistoryentity.Mobile = candidateModel.Mobile;
                        recruimentHistoryentity.Email = candidateModel.Email;
                        recruimentHistoryentity.YearOfExperience = candidateModel.YearOfExperience.Value != null ? candidateModel.YearOfExperience.Value : 0;
                        recruimentHistoryentity.PositionID = candidateModel.PositionID;
                        recruimentHistoryentity.JobVacancyID = candidateModel.JobVacancyID;
                        recruimentHistoryentity.DateApply = candidateModel.DateApply;
                        recruimentHistoryentity.ScorePotential = candidateModel.ScorePotential;
                        recruimentHistoryentity.HealthStatus = candidateModel.HealthStatus;
                        recruimentHistoryentity.PassFilterResume = candidateModel.PassFilterResume;
                        recruimentHistoryentity.Status = candidateModel.Status;
                        recruimentHistoryentity.IdentifyNumber = candidateModel.IdentifyNumber;
                        baseService.Add(recruimentHistoryentity);
                    } 
                }
                repo.SaveChanges();

            }
        }

      
    }
}
