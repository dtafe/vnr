using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Business.Recruitment.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Main.Domain
{
    public class DoActionAfterImport
    {
        public void ImportRecruitmentHistory(List<Guid> listID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_RecruitmentHistoryRepository(unitOfWork);
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                BaseService baseService = new BaseService();
                string status = string.Empty;
                var listCandidates = repoCandidate.FindBy(x => listID.Contains(x.ID)).ToList();
                foreach (var candidateModel in listCandidates)
                {
                    List<object> lstObjhistory = new List<object>();
                    lstObjhistory.Add(Common.DotNetToOracle(candidateModel.ID.ToString()));
                    lstObjhistory.Add(candidateModel.DateApply);
                    var recruimentHistory = baseService.GetData<Rec_RecruitmentHistory>(lstObjhistory, ConstantSql.hrm_rec_sp_checkduplidaterecruimentHistory,userLogin, ref status)
                        .Where(s => s.DateApply == candidateModel.DateApply).FirstOrDefault();


                    if (recruimentHistory == null || (candidateModel.DateApply != recruimentHistory.DateApply))
                    {
                        recruimentHistory = new Rec_RecruitmentHistory();
                        recruimentHistory.CandidateID = candidateModel.ID;
                        recruimentHistory.PassFilterResume = candidateModel.PassFilterResume;
                        recruimentHistory.Status = candidateModel.Status;
                        recruimentHistory.CodeCandidate = candidateModel.CodeCandidate;
                        recruimentHistory.CandidateName = candidateModel.CandidateName;
                        recruimentHistory.OrgStructureID = candidateModel.OrgStructureID;
                        recruimentHistory.JobTitleID = candidateModel.JobTitleID;
                        recruimentHistory.WorkingType = candidateModel.WorkingType;
                        recruimentHistory.SalarySuggest = candidateModel.SalarySuggest;
                        recruimentHistory.SalaryCurrent = candidateModel.SalaryCurrent;
                        recruimentHistory.SalaryApprove = candidateModel.SalaryApprove;
                        recruimentHistory.SalaryProbationary = candidateModel.SalaryProbationary;
                        recruimentHistory.AllowanceID1 = candidateModel.AllowanceID1;
                        recruimentHistory.Allowance1 = candidateModel.Allowance1;
                        recruimentHistory.CurrencyID = candidateModel.CurrencyID;
                        recruimentHistory.AllowanceID3 = candidateModel.AllowanceID3;
                        recruimentHistory.Allowance3 = candidateModel.Allowance3;
                        recruimentHistory.CurrencyID2 = candidateModel.CurrencyID2;
                        recruimentHistory.SkillLevel = candidateModel.SkillLevel;
                        recruimentHistory.TagID = candidateModel.TagID;
                        recruimentHistory.WorkplaceSuggestion = candidateModel.WorkplaceSuggestion;
                        recruimentHistory.Strong = candidateModel.Strong;
                        recruimentHistory.Weak = candidateModel.Weak;
                        recruimentHistory.Description = candidateModel.Description;
                        recruimentHistory.Assessment = candidateModel.Assessment;
                        recruimentHistory.IsReadyBizTrip = candidateModel.IsReadyBizTrip;
                        recruimentHistory.WorkingTypePeriod = candidateModel.WorkingTypePeriod;
                        recruimentHistory.Allowance = candidateModel.Allowance;
                        recruimentHistory.TimeWorkType = candidateModel.TimeWorkType;
                        recruimentHistory.DateStartWorking = candidateModel.DateStartWorking;
                        recruimentHistory.DateHire = candidateModel.DateHire;
                        recruimentHistory.AllowanceID2 = candidateModel.AllowanceID2;
                        recruimentHistory.Allowance2 = candidateModel.Allowance2;
                        recruimentHistory.CurrencyID1 = candidateModel.CurrencyID1;
                        recruimentHistory.AllowanceID4 = candidateModel.AllowanceID4;
                        recruimentHistory.Allowance4 = candidateModel.Allowance4;
                        recruimentHistory.CurrencyID3 = candidateModel.CurrencyID3;
                        recruimentHistory.ProbationDay = candidateModel.ProbationDay;
                        recruimentHistory.IsBlackList = candidateModel.IsBlackList;
                        recruimentHistory.SourceAdsID = candidateModel.SourceAdsID;
                        recruimentHistory.DateOfBirth = candidateModel.DateOfBirth;
                        recruimentHistory.Gender = candidateModel.Gender;
                        recruimentHistory.Phone = candidateModel.Phone;
                        recruimentHistory.Mobile = candidateModel.Mobile;
                        recruimentHistory.Email = candidateModel.Email;
                        recruimentHistory.YearOfExperience = candidateModel.YearOfExperience != null ? candidateModel.YearOfExperience.Value : 0;
                        recruimentHistory.PositionID = candidateModel.PositionID;
                        recruimentHistory.JobVacancyID = candidateModel.JobVacancyID;
                        recruimentHistory.DateApply = candidateModel.DateApply;
                        recruimentHistory.ScorePotential = candidateModel.ScorePotential;
                        recruimentHistory.HealthStatus = candidateModel.HealthStatus;
                        recruimentHistory.IdentifyNumber = candidateModel.IdentifyNumber;
                        recruimentHistory.Status = candidateModel.Status;
                        repo.Add(recruimentHistory);
                    }
                    else
                    {

                        recruimentHistory.CodeCandidate = candidateModel.CodeCandidate;
                        recruimentHistory.CandidateName = candidateModel.CandidateName;
                        recruimentHistory.OrgStructureID = candidateModel.OrgStructureID;
                        recruimentHistory.JobTitleID = candidateModel.JobTitleID;
                        recruimentHistory.WorkingType = candidateModel.WorkingType;
                        recruimentHistory.SalarySuggest = candidateModel.SalarySuggest;
                        recruimentHistory.SalaryCurrent = candidateModel.SalaryCurrent;
                        recruimentHistory.SalaryApprove = candidateModel.SalaryApprove;
                        recruimentHistory.SalaryProbationary = candidateModel.SalaryProbationary;
                        recruimentHistory.AllowanceID1 = candidateModel.AllowanceID1;
                        recruimentHistory.Allowance1 = candidateModel.Allowance1;
                        recruimentHistory.CurrencyID = candidateModel.CurrencyID;
                        recruimentHistory.AllowanceID3 = candidateModel.AllowanceID3;
                        recruimentHistory.Allowance3 = candidateModel.Allowance3;
                        recruimentHistory.CurrencyID2 = candidateModel.CurrencyID2;
                        recruimentHistory.SkillLevel = candidateModel.SkillLevel;
                        recruimentHistory.TagID = candidateModel.TagID;
                        recruimentHistory.WorkplaceSuggestion = candidateModel.WorkplaceSuggestion;
                        recruimentHistory.Strong = candidateModel.Strong;
                        recruimentHistory.Weak = candidateModel.Weak;
                        recruimentHistory.Description = candidateModel.Description;
                        recruimentHistory.Assessment = candidateModel.Assessment;
                        recruimentHistory.IsReadyBizTrip = candidateModel.IsReadyBizTrip;
                        recruimentHistory.WorkingTypePeriod = candidateModel.WorkingTypePeriod;
                        recruimentHistory.Allowance = candidateModel.Allowance;
                        recruimentHistory.TimeWorkType = candidateModel.TimeWorkType;
                        recruimentHistory.DateStartWorking = candidateModel.DateStartWorking;
                        recruimentHistory.DateHire = candidateModel.DateHire;
                        recruimentHistory.AllowanceID2 = candidateModel.AllowanceID2;
                        recruimentHistory.Allowance2 = candidateModel.Allowance2;
                        recruimentHistory.CurrencyID1 = candidateModel.CurrencyID1;
                        recruimentHistory.AllowanceID4 = candidateModel.AllowanceID4;
                        recruimentHistory.Allowance4 = candidateModel.Allowance4;
                        recruimentHistory.CurrencyID3 = candidateModel.CurrencyID3;
                        recruimentHistory.ProbationDay = candidateModel.ProbationDay;
                        recruimentHistory.IsBlackList = candidateModel.IsBlackList;
                        recruimentHistory.SourceAdsID = candidateModel.SourceAdsID;
                        recruimentHistory.DateOfBirth = candidateModel.DateOfBirth;
                        recruimentHistory.Gender = candidateModel.Gender;
                        recruimentHistory.Phone = candidateModel.Phone;
                        recruimentHistory.Mobile = candidateModel.Mobile;
                        recruimentHistory.Email = candidateModel.Email;
                        recruimentHistory.YearOfExperience = candidateModel.YearOfExperience != null ? candidateModel.YearOfExperience.Value : 0;
                        recruimentHistory.PositionID = candidateModel.PositionID;
                        recruimentHistory.JobVacancyID = candidateModel.JobVacancyID;
                        recruimentHistory.DateApply = candidateModel.DateApply;
                        recruimentHistory.ScorePotential = candidateModel.ScorePotential;
                        recruimentHistory.HealthStatus = candidateModel.HealthStatus;
                        recruimentHistory.IdentifyNumber = candidateModel.IdentifyNumber;
                        recruimentHistory.Status = candidateModel.Status;
                        repo.Edit(recruimentHistory);
                    }
                }
                repo.SaveChanges();
            }
        }
    }
}
