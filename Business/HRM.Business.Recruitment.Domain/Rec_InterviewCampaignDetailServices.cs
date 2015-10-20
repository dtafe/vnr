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
    public class Rec_InterviewCampaignDetailServices : BaseService
    {

        public string ActionPassing(string selectedIds, string userLogin)
        {

            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                Rec_InterviewServices services = new Rec_InterviewServices();
                Rec_InterviewCampaignDetailServices InterviewCampaignDetailServices = new Rec_InterviewCampaignDetailServices();
                Rec_RecruitmentHistoryServices RecruitmentHistoryServices = new Rec_RecruitmentHistoryServices();
                Rec_CandidateServices CandidateServices = new Rec_CandidateServices();
                string status = string.Empty;
                string message = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_InterviewCampaignDetailRepository(unitOfWork);
                var InterviewRepository = new Rec_InterviewRepository(unitOfWork);
                var InterviewCampaignDetailRepository = new Rec_InterviewCampaignDetailRepository(unitOfWork);
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstInterviewCampaignDetail = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();
                var lstcandidateid = lstInterviewCampaignDetail.Select(s => s.CandidateID).ToList();
                var objs = new List<object>();
                string strIDs = string.Empty;
                foreach (var item in lstcandidateid)
                {
                    strIDs += Common.DotNetToOracle(item.ToString()) + ",";
                }
                if (strIDs.IndexOf(",") > 0)
                    strIDs = strIDs.Substring(0, strIDs.Length - 1);
                objs.Add(strIDs);

                var lstcandidate = service.GetData<Rec_CandidateEntity>(strIDs, ConstantSql.hrm_rec_sp_get_CandidateByIds, userLogin, ref status).ToList();
                var lstrecruimenthistory = service.GetData<Rec_RecruitmentHistoryEntity>(strIDs, ConstantSql.hrm_hr_sp_get_RecHisByCandidateIds, userLogin, ref status).ToList();
                var lstinterviewbycandidateids = service.GetData<Rec_InterviewEntity>(strIDs, ConstantSql.hrm_hr_sp_get_InterViewByCandidateIds, userLogin, ref status).ToList();


                List<Rec_InterviewEntity> lstinterview = new List<Rec_InterviewEntity>();
                List<Rec_InterviewCampaignDetail> lstAllInterviewCampaignDetail = new List<Rec_InterviewCampaignDetail>();
                var lstJobVaCancyIDs = lstcandidate.Select(s => s.JobVacancyID).ToList();
                

                string strJobVacancyIds = string.Empty;
                foreach (Guid item in lstJobVaCancyIDs)
                {
                    strJobVacancyIds += item;
                    strJobVacancyIds += ",";
                }

                if (strJobVacancyIds.Length > 0)
                {
                    strJobVacancyIds = strJobVacancyIds.Substring(0, strJobVacancyIds.Length - 1);
                }

                var lstJobVacancy = service.GetData<Rec_JobVacancyEntity>(Common.DotNetToOracle(strJobVacancyIds), ConstantSql.hrm_rec_sp_get_JobVacancyByIds, userLogin, ref status).ToList();

                Rec_InterviewCampaignDetail ObjRecInterviewDt = null;


                foreach (var candidate in lstcandidate)
                {
                    ObjRecInterviewDt = new Rec_InterviewCampaignDetail();
                    Rec_InterviewEntity Interview = new Rec_InterviewEntity();
                    var hisbycandidate = lstrecruimenthistory.Where(s => s.CandidateID == candidate.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    Interview.CandidateID = candidate.ID;
                    Interview.RecruitmentHistoryID = hisbycandidate.ID;

                    var interviewbycan = lstinterviewbycandidateids.Where(s => s.RecruitmentHistoryID == hisbycandidate.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                    int? level = 0;
                    if (interviewbycan != null && interviewbycan.LevelInterview != null)
                    {
                        level = interviewbycan.LevelInterview + 1;
                    }
                    else
                    {
                        level = 1;
                    }

                    var InterviewCampaignDetail = lstInterviewCampaignDetail.Where(s => s.RecruitmentHistoryID == hisbycandidate.ID && s.LevelInterview == null).FirstOrDefault();
                    if (InterviewCampaignDetail == null)
                    {
                        continue;
                    }
                    ObjRecInterviewDt = InterviewCampaignDetail;
                    ObjRecInterviewDt.LevelInterview = level;

                    var jobVacancybyCandidate = lstJobVacancy.Where(s => s.ID == candidate.JobVacancyID).FirstOrDefault();
                    var rechisbycandidate = lstrecruimenthistory.Where(s => s.CandidateID == candidate.ID).OrderByDescending(s => s.DateApply).FirstOrDefault();

                    // nếu là vòng phỏng vấn cuối cùng thì cập nhật lại trạng thái.
                    if (jobVacancybyCandidate != null && jobVacancybyCandidate.NoLevelInterview == level)
                    {
                        rechisbycandidate.Status = "E_PASS";
                        candidate.Status = "E_PASS";
                    }
                    candidate.LevelInterview = level;
                    rechisbycandidate.LevelInterview = level;
                    rechisbycandidate.CandidateID = candidate.ID;
                    Interview.Status = "E_PASS";
                    Interview.ResultInterview = "E_PASS";
                    Interview.LevelInterview = level;
                    lstinterview.Add(Interview);
                    lstAllInterviewCampaignDetail.Add(ObjRecInterviewDt);
                    RecruitmentHistoryServices.Edit(rechisbycandidate);
                    CandidateServices.Edit(candidate);
                }
                services.Add(lstinterview);
                InterviewCampaignDetailServices.Edit(lstAllInterviewCampaignDetail);
                InterviewRepository.SaveChanges();
                InterviewCampaignDetailRepository.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
    }
}
