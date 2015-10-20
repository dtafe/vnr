using HRM.Business.Recruitment.Models;
using HRM.Business.Recruitment.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Recruitment.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Rec_InterviewController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
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
        public Rec_InterviewModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Rec_InterviewModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Rec_InterviewEntity>(id, ConstantSql.hrm_rec_sp_get_InterviewById, ref status);//note
            if (entity != null)
            {
                model = entity.CopyData<Rec_InterviewModel>();
            }
            model.ActionStatus = status;
            return model;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public Rec_InterviewModel DeleteOrRemove(string id)
        //{
        //    ActionService service = new ActionService(UserLogin);
        //    var result = service.DeleteOrRemove<Rec_InterviewEntity, Rec_InterviewModel>(id);
        //    return result;
        //}

        public void Delete(Guid ID)
        {
            var service = new Rec_CandidateServices();
            var result = service.Remove<Rec_InterviewEntity>(ID);
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rec_Interview")]
        public Rec_InterviewModel Post([Bind]Rec_InterviewModel model)
        {
            #region Validate
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rec_InterviewModel>(model, "Rec_Interview", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion

           
            ActionService service = new ActionService(UserLogin);
            var result = service.UpdateOrCreate<Rec_InterviewEntity, Rec_InterviewModel>(model);

            var RecruitmentHisService = new Rec_RecruitmentHistoryServices();
            var candService = new Rec_CandidateServices();
            var InTerCamDetaiServices = new Rec_InterviewCampaignDetailServices();

            Rec_RecruitmentHistoryEntity ObjRecruitmentHistory = null;
            Rec_RecruitmentHistoryServices RecruitmentHistoryServices = new Rec_RecruitmentHistoryServices();

            Rec_CandidateEntity ObjCandidate = null;
            Rec_CandidateServices CandidateServices = new Rec_CandidateServices();

            Rec_InterviewCampaignDetailEntity ObjInterviewCampaignDetail = null;
            Rec_InterviewCampaignDetailServices InterviewCampaignDetailServices = new Rec_InterviewCampaignDetailServices();

            string status = string.Empty;
            #region xử lý cap nhat lich su ung vien, cap nhat ung vien
            if (result != null)
            {
                ObjRecruitmentHistory = new Rec_RecruitmentHistoryEntity();
                ObjCandidate = new Rec_CandidateEntity();
                ObjInterviewCampaignDetail = new Rec_InterviewCampaignDetailEntity();

                var IlRecruitmentHistory = RecruitmentHisService.GetData<Rec_RecruitmentHistoryEntity>(result.CandidateID, ConstantSql.hrm_rec_sp_get_RecruitmentHistoryIdByCandidateId,UserLogin, ref status).OrderByDescending(s => s.DateApply).FirstOrDefault();
                var IlCandate = candService.GetData<Rec_CandidateEntity>(result.CandidateID, ConstantSql.hrm_rec_sp_get_CandidateById, UserLogin, ref status).FirstOrDefault();
                var IlInterviewcampaugnDetail = InTerCamDetaiServices.GetData<Rec_InterviewCampaignDetailEntity>(result.CandidateID, ConstantSql.hrm_rec_sp_get_InterviewCampaignDetailByCddId, UserLogin ,ref status).ToList();

                ObjCandidate = IlCandate;
                ObjRecruitmentHistory = IlRecruitmentHistory;
                if (IlCandate != null)
                {
                    string[] strCondition = model.ConditionTemp.Split('|').ToArray();
                    foreach (var Objitem in strCondition)
                    {
                        string[] item = Objitem.Split(',').ToArray();
                        if(item[0].ToString() == EnumDropDown.JobCondition.E_HEIGHT.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.Height = double.Parse(item[1].ToString());
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_WEIGHT.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.Weight = double.Parse(item[1].ToString());
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_LEVELEYES.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.LevelEye = double.Parse(item[1].ToString());
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_LEVERIGHTLEYES.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.LevelEyeRight = double.Parse(item[1].ToString());
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_MUSCULOSKELETAL.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.Musculoskeletal = double.Parse(item[1].ToString());
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_BLOODPRESSURE.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.BloodPressure = double.Parse(item[1].ToString());
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_HEARTBEAT.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.HeartBeat = double.Parse(item[1].ToString());
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_GENARALHEALTH.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.GenaralHealth = double.Parse(item[1].ToString());
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_DISEASELISTIDS.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.DiseaseListIDs = item[1].ToString();
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_WRITETEST.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.WriteTest = double.Parse(item[1].ToString());
                            }
                        }
                        else if (item[0].ToString() == EnumDropDown.JobCondition.E_INTERVIEW.ToString())
                        {
                            if (item[1] != null && item[1] != "")
                            {
                                ObjCandidate.Interview = double.Parse(item[1].ToString());
                            }
                        }
                    }
                    ObjCandidate.LevelInterview = result.LevelInterview;
                    if (result.ResultInterview.Equals(HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString()))
                    {
                        ObjCandidate.Status = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                        ObjCandidate.DateUpdate = DateTime.Now;
                    }
                    // Nếu có nhập điểm thì gán ngày đánh giá = ngày phỏng vấn
                    if (model != null && (model.Score1 != null || model.Score2 != null))
                    {
                        ObjCandidate.DateExam = model.DateInterview;
                        ObjCandidate.DateUpdate = DateTime.Now;
                    }
                    if(ObjCandidate.JobVacancyID != null)
                    {
                        var entityJobVacancy = service.GetData<Rec_JobVacancyEntity>(Common.DotNetToOracle(ObjCandidate.JobVacancyID.ToString()), ConstantSql.hrm_rec_sp_get_JobVacancyId, ref status).FirstOrDefault();
                        if (entityJobVacancy.NoLevelInterview == result.LevelInterview && result.ResultInterview.Equals(HRM.Infrastructure.Utilities.Interview.E_PASS.ToString()))
                        {
                            if (IlCandate != null)
                            {
                                ObjCandidate.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_PASS.ToString();
                                ObjCandidate.DateUpdate = DateTime.Now;
                            }
                            if (IlRecruitmentHistory != null)
                            {
                                ObjRecruitmentHistory.Status = HRM.Infrastructure.Utilities.Interview.E_PASS.ToString();
                                ObjRecruitmentHistory.DateUpdate = DateTime.Now;
                            }
                        }
                    }
                }

                if (IlRecruitmentHistory != null)
                {
                    // Nếu có nhập điểm thì gán ngày đánh giá = ngày phỏng vấn
                    if (model != null && (model.Score1 != null || model.Score2 != null))
                    {
                        IlRecruitmentHistory.DateExam = model.DateInterview;
                    }
                    ObjRecruitmentHistory = IlRecruitmentHistory;
                    ObjRecruitmentHistory.LevelInterview = result.LevelInterview;
                    if (result.ResultInterview.Equals(HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString()))
                    {
                        //cap nhat status cho lich su ung vien
                        ObjRecruitmentHistory.Status = HRM.Infrastructure.Utilities.Interview.E_FAIL.ToString();
                    }
                }

                //Cập nhật level cho intercampaigndetail
                if (IlInterviewcampaugnDetail != null)
                {
                    var ObjTemp = IlInterviewcampaugnDetail.Where(s => s.CandidateID == result.CandidateID && s.LevelInterview == null).FirstOrDefault();
                    if (ObjTemp != null)
                    {
                        ObjInterviewCampaignDetail = ObjTemp;
                        ObjInterviewCampaignDetail.LevelInterview = result.LevelInterview;
                    }
                }

                message = InterviewCampaignDetailServices.Edit(ObjInterviewCampaignDetail);
                message = RecruitmentHistoryServices.Edit(ObjRecruitmentHistory);
                message = CandidateServices.Edit(ObjCandidate);
            }
            #endregion
            return result;
        }

    }
}