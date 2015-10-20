using HRM.Business.Category.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Recruitment.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Domain
{
    public class Rec_CandidateServices : BaseService
    {

        public string OutOfBlackList(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                string message = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                List<Guid> lstcandidateIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstCandidates = repoCandidate.FindBy(m => m.ID != null && lstcandidateIds.Contains(m.ID)).ToList();
                var repoHistory = new Rec_RecruitmentHistoryRepository(unitOfWork);
                var lstRecruimentHistory = repoHistory.FindBy(m => m.CandidateID != null && lstcandidateIds.Contains(m.CandidateID)).ToList();
                foreach (var Candidate in lstCandidates)
                {
                    Candidate.IsBlackList = null;
                    Candidate.ReasonBlackListID = null;
                    var hisbycandidate = lstRecruimentHistory.Where(s => s.CandidateID == Candidate.ID).OrderByDescending(s => s.DateApply).FirstOrDefault();
                    hisbycandidate.IsBlackList = null;
                }
                repoCandidate.SaveChanges();
                repoHistory.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
        public string ActionBlackListCandidate(string selectedIds, Guid? _ReasonBlackListID)
        {
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                string message = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                var repoHistory = new Rec_RecruitmentHistoryRepository(unitOfWork);
                List<Guid> lstcandidateIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstCandidates = repoCandidate.FindBy(m => m.ID != null && lstcandidateIds.Contains(m.ID)).ToList();

                var lstRecruimentHistory = repoHistory.FindBy(m => m.CandidateID != null && lstcandidateIds.Contains(m.CandidateID)).ToList();

                foreach (var Candidate in lstCandidates)
                {
                    Candidate.IsBlackList = true;
                    Candidate.ReasonBlackListID = _ReasonBlackListID;
                    var hisbycandidate = lstRecruimentHistory.Where(s => s.CandidateID == Candidate.ID).OrderByDescending(s => s.DateApply).FirstOrDefault();
                    hisbycandidate.IsBlackList = true;
                }
                repoCandidate.SaveChanges();
                repoHistory.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
        public string ActionApprovedCandidate(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                string message = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                var repoHistory = new Rec_RecruitmentHistoryRepository(unitOfWork);
                List<Guid> lstcandidateIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstCandidates = repoCandidate.FindBy(m => m.ID != null && lstcandidateIds.Contains(m.ID)).ToList();

                var lstRecruimentHistory = repoHistory.FindBy(m => m.CandidateID != null && lstcandidateIds.Contains(m.CandidateID)).ToList();

                foreach (var Candidate in lstCandidates)
                {
                    Candidate.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_APPROVE.ToString();
                    Candidate.DateUpdate = DateTime.Now;
                    var hisbycandidate = lstRecruimentHistory.Where(s => s.CandidateID == Candidate.ID).OrderByDescending(s => s.DateApply).FirstOrDefault();
                    hisbycandidate.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_APPROVE.ToString();
                    hisbycandidate.CandidateID = Candidate.ID;
                    hisbycandidate.DateUpdate = DateTime.Now;
                }
                repoCandidate.SaveChanges();
                repoHistory.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
        public string CombackToCandidate(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                string message = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                List<Guid> lstcandidateIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstCandidates = repoCandidate.FindBy(m => m.ID != null && lstcandidateIds.Contains(m.ID)).ToList();
                foreach (var Candidate in lstCandidates)
                {
                    Candidate.StatusHire = null;
                }
                repoCandidate.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
        public IList GetHistoryApprove(string HistoryApprove)
        {
            return null;
        }
        public List<Rec_CandidateEntity> FilterCandidate(DateTime dateFrom, DateTime dateTo, string jobVacancyIDs, string UserLogin, bool GetListFail = false, bool IsIncludeEvaCandidate = false)
        {
            List<Rec_CandidateEntity> lstCandidateResultPass = new List<Rec_CandidateEntity>();
            List<Rec_CandidateEntity> lstCandidateResultFail = new List<Rec_CandidateEntity>();
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoJobVacancy = new Rec_JobVacancyRepository(unitOfWork);
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var repoJobCondition = new Rec_JobConditionRepository(unitOfWork);
                var repoCadidate = new Rec_CandidateRepository(unitOfWork);
                var repoRecruimentHistory = new Rec_RecruitmentHistoryRepository(unitOfWork);
                string status = string.Empty;
                #region Lấy tất cả nv
                var profileRepository = new Hre_ProfileRepository(unitOfWork);
                var lstProfile = new List<Hre_Profile>().Select(s => new
                {
                    s.ID,
                    s.IDNo,
                    s.ProfileName,
                    s.DateOfBirth
                }).ToList();

                lstProfile.AddRange(profileRepository.FindBy(s => s.IsDelete == null).Select(s => new
                {
                    s.ID,
                    s.IDNo,
                    s.ProfileName,
                    s.DateOfBirth
                }).ToList());
                #endregion

                var lstJobCondition = repoJobCondition.FindBy(s => s.IsDelete == null).ToList();
                List<object> lstpara = new List<object>();
                lstpara.Add(dateFrom);
                lstpara.Add(dateTo);
                lstpara.Add(Common.DotNetToOracle(jobVacancyIDs));

                var lstCandidate = service.GetData<Rec_CandidateEntity>(lstpara, ConstantSql.hrm_rec_sp_getdata_FilterCandidate, UserLogin, ref status);
                if (lstCandidate == null || lstCandidate.Count == 0)
                {
                    return new List<Rec_CandidateEntity>();
                }

                if (IsIncludeEvaCandidate == false)
                {
                    lstCandidate = lstCandidate.Where(x => x.Status == null || x.Status == HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_NEW.ToString()).ToList();
                }

                if (lstCandidate == null || lstCandidate.Count == 0)
                {
                    return new List<Rec_CandidateEntity>();
                }

                var lstCandidateIds = lstCandidate.Select(s => s.ID).Distinct().ToList();


                #region Lấy ds pv
                var interviewRepository = new Rec_InterviewRepository(unitOfWork);
                var lstInterview = new List<Rec_Interview>().Select(s => new
                {
                    s.CandidateID,
                    s.LevelInterview,
                    s.Score1,
                    s.Score2,
                    s.Score3,
                    s.DateUpdate
                }).ToList();

                lstInterview.AddRange(interviewRepository.FindBy(s => s.IsDelete == null && lstCandidateIds.Contains(s.CandidateID.Value)).Select(s => new 
                {   s.CandidateID,
                    s.LevelInterview,
                    s.Score1,
                    s.Score2,
                    s.Score3,
                    s.DateUpdate
                }).ToList());

                #endregion
                var RecruitmentHistoryRepository = new Rec_RecruitmentHistoryRepository(unitOfWork);
                var lstrecruitmentHistory = RecruitmentHistoryRepository.FindBy(s => s.IsDelete == null && lstCandidateIds.Contains(s.CandidateID)).ToList();

                List<object> lstparaEdu = new List<object>();
                lstparaEdu.Add(null);
                lstparaEdu.Add(1);
                lstparaEdu.Add(int.MaxValue - 1);
                var lstEducationLevel = service.GetData<Cat_NameEntityEntity>(lstparaEdu, ConstantSql.hrm_cat_sp_get_EducationLevel, UserLogin,ref status).ToList();

                List<object> lstparadiseelist = new List<object>();
                lstparadiseelist.Add(null);
                lstparadiseelist.Add(EnumDropDown.EntityType.E_SICK_REC.ToString());
                lstparadiseelist.Add(1);
                lstparadiseelist.Add(int.MaxValue - 1);
                var lstsick = service.GetData<Cat_NameEntityEntity>(lstparadiseelist, ConstantSql.hrm_cat_sp_get_LevelGeneral, UserLogin,ref status).ToList();

                List<object> lstparaProvince = new List<object>();
                lstparaProvince.Add(null);
                lstparaProvince.Add(null);
                lstparaProvince.Add(null);
                lstparaProvince.Add(null);
                lstparaProvince.Add(1);
                lstparaProvince.Add(int.MaxValue - 1);
                var lstProvince = service.GetData<Cat_ProvinceEntity>(lstparaProvince, ConstantSql.hrm_cat_sp_get_Province, UserLogin, ref status).ToList();

                foreach (var item in lstCandidate)
                {
                    string ReasonFailFilter = string.Empty;
                    // nếu ko có điều kiện thì add vào ds fail
                    if (string.IsNullOrEmpty(item.JobConditionIDs))
                    {
                        lstCandidateResultPass.Add(item);
                        continue;
                    }

                    if (item.IsFilterCV == null || item.IsFilterCV == false)
                    {
                        lstCandidateResultPass.Add(item);
                        continue;
                    }

                    // nếu có thì bắt đầu lọc
                    else
                    {
                        List<Guid> ids = new List<Guid>();
                        ids = item.JobConditionIDs
                               .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => Guid.Parse(x))
                               .ToList();
                        var lstCondition = lstJobCondition.Where(x => ids.Contains(x.ID)).ToList();
                        int countCondition = lstCondition.Count;
                        int countAgree = 0;
                        foreach (var Condition in lstCondition)
                        {
                            #region Kiểm tra Tổng Điểm Phỏng Vấn
                            if (Condition.ConditionName == ConditionName.E_INTERVIEW.ToString())
                            {
                                double valueInterview1 = 0;
                                try
                                {
                                    valueInterview1 = double.Parse(Condition.Value1);
                                }
                                catch
                                {

                                }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.Interview == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueInterview1 != 0 && item.Interview.HasValue)
                                {

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueInterview1 <= item.Interview)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueInterview1 >= item.Interview)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueInterview1 == item.Interview)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueInterview1 != 0 && item.Interview.HasValue)
                                {


                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueInterview1 <= item.Interview)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueInterview1 >= item.Interview)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueInterview1 != item.Interview)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                            }
                            #endregion
                            #region Kiểm tra Điểm Thi Viết
                            if (Condition.ConditionName == ConditionName.E_WRITETEST.ToString())
                            {
                                double valueWriteTest1 = 0;
                                try
                                {
                                    valueWriteTest1 = double.Parse(Condition.Value1);
                                }
                                catch
                                { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.WriteTest == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueWriteTest1 != 0 && item.WriteTest.HasValue)
                                {

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueWriteTest1 <= item.WriteTest)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueWriteTest1 >= item.WriteTest)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueWriteTest1 == item.WriteTest)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueWriteTest1 != 0 && item.WriteTest.HasValue)
                                {


                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueWriteTest1 <= item.WriteTest)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueWriteTest1 >= item.WriteTest)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueWriteTest1 != item.WriteTest)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                            }
                            #endregion
                            #region Kiểm tra Tổng Sức Khỏe
                            if (Condition.ConditionName == ConditionName.E_GENERALHEALTH.ToString())
                            {
                                double valueHealth1 = 0;
                                try
                                {
                                    valueHealth1 = double.Parse(Condition.Value1);
                                }
                                catch
                                { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.GenaralHealth == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueHealth1 != 0 && item.GenaralHealth.HasValue)
                                {


                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueHealth1 <= item.GenaralHealth)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueHealth1 >= item.GenaralHealth)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueHealth1 == item.GenaralHealth)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && item.GenaralHealth.HasValue && valueHealth1 != 0)
                                {


                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueHealth1 <= item.GenaralHealth)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueHealth1 >= item.GenaralHealth)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueHealth1 != item.GenaralHealth)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #region Kiểm tra cao
                            if (Condition.ConditionName == ConditionName.E_HEIGHT.ToString())
                            {
                                double valueHeight1 = 0;
                                try
                                {
                                    valueHeight1 = double.Parse(Condition.Value1);
                                }
                                catch
                                { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.Height == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueHeight1 != 0 && item.Height.HasValue)
                                {


                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueHeight1 <= item.Height)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueHeight1 >= item.Height)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueHeight1 == item.Height)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && item.Height.HasValue && valueHeight1 != 0)
                                {


                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.Height <= valueHeight1)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.Height >= valueHeight1)
                                    {

                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueHeight1 != item.Height)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #region Kiểm tra Cân Nặng
                            if (Condition.ConditionName == ConditionName.E_WEIGHT.ToString())
                            {

                                double valueWeight1 = 0;
                                try
                                {
                                    valueWeight1 = double.Parse(Condition.Value1);
                                }
                                catch { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.Weight == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueWeight1 != 0 && item.Weight.HasValue)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.Weight == valueWeight1)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.Weight >= valueWeight1)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.Height <= valueWeight1)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueWeight1 != 0 && item.Weight.HasValue)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.Weight != valueWeight1)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.Weight <= valueWeight1)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.Height >= valueWeight1)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #region Kiểm Tra Mắt
                            #region Mắt Trái
                            if (Condition.ConditionName == ConditionName.E_LEVELEYES.ToString())
                            {

                                double valueEyes1 = 0;
                                // double Eyes = 0;
                                try
                                {
                                    valueEyes1 = double.Parse(Condition.Value1);
                                    //  Eyes = double.Parse(item.LevelEye);
                                }
                                catch { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (!item.LevelEye.HasValue)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueEyes1 != 0)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.LevelEye.Value == valueEyes1)
                                    {
                                        countAgree++;
                                        continue;

                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.LevelEye.Value >= valueEyes1)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.LevelEye.Value <= valueEyes1)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueEyes1 != 0)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.LevelEye.Value != valueEyes1)
                                    {
                                        countAgree++;
                                        continue;

                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.LevelEye.Value <= valueEyes1)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.LevelEye.Value >= valueEyes1)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #region Mắt Phải
                            if (Condition.ConditionName == ConditionName.E_LEVERIGHTLEYES.ToString())
                            {

                                double valueEyes1 = 0;
                                // double Eyes = 0;
                                try
                                {
                                    valueEyes1 = double.Parse(Condition.Value1);
                                    //  Eyes = double.Parse(item.LevelEye);
                                }
                                catch { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (!item.LevelEye.HasValue)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueEyes1 != 0)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.LevelEyeRight.Value == valueEyes1)
                                    {
                                        countAgree++;
                                        continue;

                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.LevelEyeRight.Value >= valueEyes1)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.LevelEyeRight.Value <= valueEyes1)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueEyes1 != 0)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.LevelEyeRight.Value != valueEyes1)
                                    {
                                        countAgree++;
                                        continue;

                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.LevelEyeRight.Value <= valueEyes1)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.LevelEyeRight.Value >= valueEyes1)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #endregion
                            #region Kiểm tra Thời Gian Trượt Kỳ Trước
                            if (Condition.ConditionName == ConditionName.E_DURATIONFAILPREVIOUS.ToString())
                            {
                                if (lstrecruitmentHistory == null || lstrecruitmentHistory.Count == 0)
                                {
                                    countAgree++;
                                    continue;
                                }
                                int valueDuration1 = int.Parse(Condition.Value1);
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                                {

                                    var listCandidateHis = lstrecruitmentHistory.Where(x => x.CandidateID == item.ID && x.Status == HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_FAIL.ToString())
                                        .OrderByDescending(x => x.DateApply).ToList();
                                    if (listCandidateHis == null || listCandidateHis.Count == 0)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (listCandidateHis != null && listCandidateHis.Count != 0)
                                    {
                                        var CandidateHistory = listCandidateHis.FirstOrDefault();
                                        double month = 0;
                                        if (CandidateHistory.DateApply != null && item.DateApply != null)
                                        {

                                            month = (new DateTime(item.DateApply.Value.Subtract(CandidateHistory.DateApply.Value).Ticks).Year - 1) > 0 ? (new DateTime(item.DateApply.Value.Subtract(CandidateHistory.DateApply.Value).Ticks).Year - 1) : (item.DateApply.Value.Subtract(CandidateHistory.DateApply.Value).TotalDays / (365.25 / 12));

                                        }

                                        if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && month == valueDuration1)
                                        {
                                            countAgree++;
                                            continue;
                                        }

                                        if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && month > valueDuration1)
                                        {
                                            countAgree++;
                                            continue;
                                        }

                                        if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && month < valueDuration1)
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                        else
                                        {
                                            ReasonFailFilter += Condition.ConditionName + ",";
                                        }
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                {

                                    var listCandidateHis = lstrecruitmentHistory.Where(x => x.CandidateID == item.ID && x.Status == HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_FAIL.ToString())
                                        .OrderByDescending(x => x.DateApply).ToList();
                                    if (listCandidateHis == null || listCandidateHis.Count == 0)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (listCandidateHis != null && listCandidateHis.Count != 0)
                                    {

                                        var CandidateHistory = listCandidateHis.FirstOrDefault();
                                        double month = 0;
                                        if (CandidateHistory.DateApply != null && item.DateApply != null)
                                        {
                                            month = (new DateTime(item.DateApply.Value.Subtract(CandidateHistory.DateApply.Value).Ticks).Year - 1) > 0 ? (new DateTime(item.DateApply.Value.Subtract(CandidateHistory.DateApply.Value).Ticks).Year - 1) : (item.DateApply.Value.Subtract(CandidateHistory.DateApply.Value).TotalDays / (365.25 / 12));
                                        }

                                        if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && month != valueDuration1)
                                        {
                                            countAgree++;
                                            continue;

                                        }

                                        if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && month < valueDuration1)
                                        {
                                            countAgree++;
                                            continue;
                                        }

                                        if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && month > valueDuration1)
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                        else
                                        {
                                            ReasonFailFilter += Condition.ConditionName + ",";
                                        }
                                    }
                                }

                            }
                            #endregion
                            #region Kiểm tra Từ Tuổi
                            if (Condition.ConditionName == ConditionName.E_AGE.ToString())
                            {
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.DateOfBirth == null)
                                {
                                    countAgree++;
                                    continue;
                                }

                                //double AgeCadidate = DateTime.Now.Subtract(item.DateOfBirth).TotalDays / 365.242199;

                                //AgeCadidate = Math.Round(AgeCadidate, 2);

                                double? AgeCadidate = (new DateTime(DateTime.Now.Subtract(item.DateOfBirth).Ticks).Year - 1) > 0 ? (new DateTime(DateTime.Now.Subtract(item.DateOfBirth).Ticks).Year - 1) : (DateTime.Now.Subtract(item.DateOfBirth).TotalDays / 365.242199);

                                double? valueAge = 0;
                                try
                                {
                                    valueAge = int.Parse(Condition.Value1);
                                }
                                catch
                                {
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && AgeCadidate > 0)
                                {

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && AgeCadidate >= valueAge)
                                    {
                                        countAgree++;
                                        continue;

                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && AgeCadidate <= valueAge)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && AgeCadidate == valueAge)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && AgeCadidate > 0)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && AgeCadidate <= valueAge)
                                    {
                                        countAgree++;
                                        continue;

                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && AgeCadidate >= valueAge)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && AgeCadidate != valueAge)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                            }
                            #endregion
                            #region Kiểm tra Giới Tính
                            if (Condition.ConditionName == ConditionName.E_GENDER.ToString())
                            {
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (string.IsNullOrEmpty(item.Gender))
                                {
                                    countAgree++;
                                    continue;
                                }
                                string valueGender = Condition.Value1;

                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                                {

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.Gender.Equals(valueGender))
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && !item.Gender.Equals(valueGender))
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                            }
                            #endregion
                            #region Kiểm Tra Đã làm ở cty chưa
                            if (Condition.ConditionName == ConditionName.E_ISTERMINATEINCOMPANY.ToString() && Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString())
                            {
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                                {

                                    if (lstProfile != null && lstProfile.Count != 0)
                                    {
                                        // nếu có CMND thì ưu tiên kiểm tra trước
                                        if (!string.IsNullOrEmpty(item.IdentifyNumber))
                                        {
                                            var Profile = lstProfile.FirstOrDefault(x => !string.IsNullOrEmpty(x.IDNo) && x.IDNo == item.IdentifyNumber);
                                            if (Profile == null)
                                            {
                                                countAgree++;
                                                continue;
                                            }
                                        }
                                        // nếu ko có CMND thì kiểm tra tên và ngày sinh 
                                        else if (!string.IsNullOrEmpty(item.CandidateName) && item.DateOfBirth != null)
                                        {
                                            var Profile = lstProfile.FirstOrDefault(x => !string.IsNullOrEmpty(x.ProfileName) && x.DateOfBirth.HasValue && x.ProfileName == item.CandidateName && item.DateOfBirth == x.DateOfBirth.Value);
                                            if (Profile == null)
                                            {
                                                countAgree++;
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            ReasonFailFilter += Condition.ConditionName + ",";
                                        }
                                    }
                                }
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                {
                                    if (lstProfile != null && lstProfile.Count != 0)
                                    {
                                        // nếu có CMND thì ưu tiên kiểm tra trước
                                        if (!string.IsNullOrEmpty(item.IdentifyNumber))
                                        {
                                            var Profile = lstProfile.FirstOrDefault(x => !string.IsNullOrEmpty(x.IDNo) && x.IDNo == item.IdentifyNumber);
                                            if (Profile == null)
                                            {
                                                countAgree++;
                                                continue;
                                            }
                                            else
                                            {
                                                ReasonFailFilter += Condition.ConditionName + ",";
                                            }
                                        }
                                        // nếu ko có CMND thì kiểm tra tên và ngày sinh 
                                        else if (!string.IsNullOrEmpty(item.CandidateName) && item.DateOfBirth != null)
                                        {
                                            var Profile = lstProfile.FirstOrDefault(x => !string.IsNullOrEmpty(x.ProfileName) && x.DateOfBirth.HasValue && x.ProfileName == item.CandidateName && item.DateOfBirth.Date == x.DateOfBirth.Value.Date);
                                            if (Profile == null)
                                            {
                                                countAgree++;
                                                continue;
                                            }
                                            else
                                            {
                                                ReasonFailFilter += Condition.ConditionName + ",";
                                            }
                                        }
                                        else
                                        {
                                            ReasonFailFilter += Condition.ConditionName + ",";
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Kiểm Tra Bệnh Loại Trừ
                            if (Condition.ConditionName == ConditionName.E_DISEASEIDS.ToString())
                            {
                                // nếu ko có bệnh thì qua
                                if (string.IsNullOrEmpty(item.DiseaseListIDs))
                                {
                                    countAgree++;
                                    continue;

                                }
                                // nếu có chứa bệnh nào thì ko qua
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                {
                                    if (!string.IsNullOrEmpty(Condition.Value1))
                                    {
                                        //lấy ds bệnh của đk tuyển
                                        var lstDiseaseCondition = Condition.Value1.Split(',').Select(x => x).ToList();

                                        var lstsickbycondition = lstsick.Where(s => lstDiseaseCondition.Contains(Common.DotNetToOracle(s.ID.ToString()))).ToList();
                                        // lấy ds mã bệnh của candidate - vì candidate lưu mã
                                        var lstDiseseCandidate = item.DiseaseListIDs.Split(',').ToList();
                                        if (lstsickbycondition.Where(x => lstDiseseCandidate.Contains(x.Code)).Count() == 0)
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                        else
                                        {
                                            ReasonFailFilter += Condition.ConditionName + ",";
                                        }
                                    }
                                }
                                else if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                                {
                                    if (!string.IsNullOrEmpty(Condition.Value1))
                                    {
                                        //lấy ds bệnh của đk tuyển
                                        var lstDiseaseCondition = Condition.Value1.Split(',').Select(x => x).ToList();

                                        var lstsickbycondition = lstsick.Where(s => lstDiseaseCondition.Contains(Common.DotNetToOracle(s.ID.ToString()))).ToList();
                                        // lấy ds mã bệnh của candidate - vì candidate lưu mã
                                        var lstDiseseCandidate = item.DiseaseListIDs.Split(',').ToList();
                                        if (lstsickbycondition.Where(x => lstDiseseCandidate.Contains(x.Code)).Count() != 0)
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                        else
                                        {
                                            ReasonFailFilter += Condition.ConditionName + ",";
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Kiểm tra điểm thi
                            #region Điểm 1
                            if (Condition.ConditionName == ConditionName.E_SCORE1.ToString())
                            {
                                if (lstInterview == null)
                                {
                                    ReasonFailFilter += Condition.ConditionName + ",";
                                    continue;
                                }
                                var lstinterviewbyCandidate = lstInterview.Where(s => s.CandidateID == item.ID).ToList();
                                if (lstinterviewbyCandidate == null || (lstinterviewbyCandidate != null && lstinterviewbyCandidate.Count == 0))
                                {
                                    countAgree++;
                                    continue;
                                }

                                var interviewbyCan = lstinterviewbyCandidate.Where(s => s.LevelInterview == item.LevelInterview).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                                if (interviewbyCan == null)
                                    continue;
                                double? valueScore1Condition = double.Parse(Condition.Value1);
                                double? valueScore1 = interviewbyCan.Score1;
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueScore1 == valueScore1Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueScore1 > valueScore1Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueScore1 < valueScore1Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueScore1 != valueScore1Condition)
                                    {
                                        countAgree++;
                                        continue;

                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueScore1 < valueScore1Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueScore1 > valueScore1Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                            }
                            #endregion
                            #region Điểm 2
                            if (Condition.ConditionName == ConditionName.E_SCORE2.ToString())
                            {
                                if (lstInterview == null)
                                {
                                    ReasonFailFilter += Condition.ConditionName + ",";
                                    continue;
                                }
                                var lstinterviewbyCandidate = lstInterview.Where(s => s.CandidateID == item.ID).ToList();
                                if (lstinterviewbyCandidate == null || (lstinterviewbyCandidate != null && lstinterviewbyCandidate.Count == 0))
                                {
                                    countAgree++;
                                    continue;
                                }

                                var interviewbyCan = lstinterviewbyCandidate.Where(s => s.LevelInterview == item.LevelInterview).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                                if (interviewbyCan == null)
                                    continue;
                                double? valueScore2Condition = double.Parse(Condition.Value1);
                                double? valueScore2 = interviewbyCan.Score2;
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueScore2 == valueScore2Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueScore2 > valueScore2Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueScore2 < valueScore2Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueScore2 != valueScore2Condition)
                                    {
                                        countAgree++;
                                        continue;

                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueScore2 < valueScore2Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueScore2 > valueScore2Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                            }
                            #endregion
                            #region Điểm 3
                            if (Condition.ConditionName == ConditionName.E_SCORE3.ToString())
                            {
                                if (lstInterview == null || lstInterview.Count == 0)
                                {
                                    ReasonFailFilter += Condition.ConditionName + ",";
                                    continue;
                                }
                                var lstinterviewbyCandidate = lstInterview.Where(s => s.CandidateID == item.ID).ToList();
                                if (lstinterviewbyCandidate == null || (lstinterviewbyCandidate != null && lstinterviewbyCandidate.Count == 0))
                                {
                                    countAgree++;
                                    continue;
                                }

                                var interviewbyCan = lstinterviewbyCandidate.Where(s => s.LevelInterview == item.LevelInterview).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                                if (interviewbyCan == null)
                                    continue;
                                double? valueScore3Condition = double.Parse(Condition.Value1);
                                double? valueScore3 = interviewbyCan.Score3;
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueScore3 == valueScore3Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueScore3 > valueScore3Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueScore3 < valueScore3Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueScore3 != valueScore3Condition)
                                    {
                                        countAgree++;
                                        continue;

                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueScore3 < valueScore3Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueScore3 > valueScore3Condition)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                            }
                            #endregion
                            #endregion
                            #region Kiểm tra Hệ Vận Động
                            if (Condition.ConditionName == ConditionName.E_MUSCULOSKELETAL.ToString())
                            {

                                double valuemusculoskeletal = 0;
                                try
                                {
                                    valuemusculoskeletal = double.Parse(Condition.Value1);
                                }
                                catch { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.Musculoskeletal == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valuemusculoskeletal != 0 && item.Musculoskeletal.HasValue)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.Musculoskeletal == valuemusculoskeletal)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.Musculoskeletal >= valuemusculoskeletal)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.Musculoskeletal <= valuemusculoskeletal)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valuemusculoskeletal != 0 && item.Musculoskeletal.HasValue)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.Musculoskeletal != valuemusculoskeletal)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.Musculoskeletal < valuemusculoskeletal)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.Musculoskeletal > valuemusculoskeletal)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #region Kiểm tra Huyết Áp
                            if (Condition.ConditionName == ConditionName.E_BLOODPRESSURE.ToString())
                            {

                                double bloodpressure = 0;
                                try
                                {
                                    bloodpressure = double.Parse(Condition.Value1);
                                }
                                catch { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.BloodPressure == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && bloodpressure != 0 && item.BloodPressure.HasValue)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.BloodPressure == bloodpressure)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.BloodPressure >= bloodpressure)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.BloodPressure <= bloodpressure)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && bloodpressure != 0 && item.BloodPressure.HasValue)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.BloodPressure != bloodpressure)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.BloodPressure < bloodpressure)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.BloodPressure > bloodpressure)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #region Kiểm tra Nhịp Tim
                            if (Condition.ConditionName == ConditionName.E_HEARTBEAT.ToString())
                            {
                                double heartbeat = 0;
                                try
                                {
                                    heartbeat = double.Parse(Condition.Value1);
                                }
                                catch { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.HeartBeat == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && heartbeat != 0 && item.HeartBeat.HasValue)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.HeartBeat == heartbeat)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.HeartBeat >= heartbeat)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.HeartBeat <= heartbeat)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && heartbeat != 0 && item.HeartBeat.HasValue)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && item.HeartBeat != heartbeat)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.HeartBeat < heartbeat)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.HeartBeat > heartbeat)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #region Kiểm tra Trình độ học vấn
                            if (Condition.ConditionName == ConditionName.E_EDUCATIONLEVEL.ToString())
                            {
                                if (item.EducationLevelID == null)
                                {
                                    countAgree++;
                                    continue;

                                }
                                else
                                {
                                    if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                                    {
                                        if (!string.IsNullOrEmpty(Condition.Value1))
                                        {
                                            // Lấy list string mã trình độ học vấn
                                            var lstCodeEducationLevelOfCondition = Condition.Value1.Split(',').Select(x => x).ToList();
                                            // lấy mã trình độ học vấn của Candidate
                                            var CodeEduOfCandidate = lstEducationLevel.Where(s => s.ID == item.EducationLevelID).Select(s => s.Code).FirstOrDefault();

                                            if (lstCodeEducationLevelOfCondition.Contains(CodeEduOfCandidate))
                                            {
                                                countAgree++;
                                                continue;
                                            }
                                            else
                                            {
                                                ReasonFailFilter += Condition.ConditionName + ",";
                                            }
                                        }
                                    }
                                    if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                    {
                                        var lstEduCondition = Condition.Value1.Split(',').Select(x => x).ToList();
                                        var CodeEduOfCandidate = lstEducationLevel.Where(s => s.ID == item.EducationLevelID).Select(s => s.Code).FirstOrDefault();
                                        if (!lstEduCondition.Contains(CodeEduOfCandidate))
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                        else
                                        {
                                            ReasonFailFilter += Condition.ConditionName + ",";
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Kiểm tra số năm kinh nghiệm
                            if (Condition.ConditionName == ConditionName.E_YEAROFEXPERIENCE.ToString())
                            {
                                double yearofexperienceFilter = 0;
                                try
                                {
                                    yearofexperienceFilter = double.Parse(Condition.Value1);
                                }
                                catch
                                { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.YearOfExperience == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && yearofexperienceFilter != 0 && item.YearOfExperience.HasValue)
                                {


                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && yearofexperienceFilter <= item.YearOfExperience)
                                    {
                                        countAgree++;
                                        continue;
                                    }

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && yearofexperienceFilter >= item.YearOfExperience)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && yearofexperienceFilter == item.YearOfExperience)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && item.YearOfExperience.HasValue && yearofexperienceFilter != 0)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && item.YearOfExperience <= yearofexperienceFilter)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && item.YearOfExperience >= yearofexperienceFilter)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && yearofexperienceFilter != item.YearOfExperience)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #region Kiểm tra tỉnh thường trú
                            if (Condition.ConditionName == ConditionName.E_CANDIDATEP_PPROVINCE.ToString())
                            {
                                if (item.PProvinceID == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                else
                                {
                                    if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                                    {
                                        if (!string.IsNullOrEmpty(Condition.Value1))
                                        {
                                            // Lấy list string mã trình độ học vấn
                                            var lstCodeProvinceFilter = Condition.Value1.Split(',').Select(x => x).ToList();
                                            // lấy mã trình độ học vấn của Candidate
                                            var codeProvinceCandidate = lstProvince.Where(s => s.ID == item.PProvinceID).Select(s => s.Code).FirstOrDefault();

                                            if (lstCodeProvinceFilter.Contains(codeProvinceCandidate))
                                            {
                                                countAgree++;
                                                continue;
                                            }
                                            else
                                            {
                                                ReasonFailFilter += Condition.ConditionName + ",";
                                            }
                                        }
                                    }
                                    if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                    {
                                        var lstCodeProvinceFilter = Condition.Value1.Split(',').Select(x => x).ToList();
                                        var codeProvinceCandidate = lstProvince.Where(s => s.ID == item.PProvinceID).Select(s => s.Code).FirstOrDefault();
                                        if (!lstCodeProvinceFilter.Contains(codeProvinceCandidate))
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                        else
                                        {
                                            ReasonFailFilter += Condition.ConditionName + ",";
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Kiểm tra chuyên ngành
                            if (Condition.ConditionName == ConditionName.E_SPECIALISATION.ToString())
                            {
                                string specialisationFilter = null;
                                try
                                {
                                    specialisationFilter = Condition.Value1;
                                }
                                catch
                                { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.Specialisation == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && specialisationFilter != null && item.Specialisation != null)
                                {

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && specialisationFilter.ToLower() == item.Specialisation.ToLower())
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && specialisationFilter != null && item.Specialisation != null)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && specialisationFilter.ToLower() != item.Specialisation.ToLower())
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                            #region Kiểm tra trường tốt nghiệp
                            if (Condition.ConditionName == ConditionName.E_GRADUATESCHOOL.ToString())
                            {
                                string schoolGraduateFilter = null;
                                try
                                {
                                    schoolGraduateFilter = Condition.Value1;
                                }
                                catch
                                { }
                                // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                                if (item.GraduateSchool == null)
                                {
                                    countAgree++;
                                    continue;
                                }
                                // Điều kiện thỏa
                                if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && schoolGraduateFilter != null && item.GraduateSchool != null)
                                {

                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && schoolGraduateFilter.ToLower() == item.GraduateSchool.ToLower())
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }
                                //không thỏa
                                else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && schoolGraduateFilter != null && item.GraduateSchool != null)
                                {
                                    if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && schoolGraduateFilter.ToLower() != item.GraduateSchool.ToLower())
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                    else
                                    {
                                        ReasonFailFilter += Condition.ConditionName + ",";
                                    }
                                }

                            }
                            #endregion
                        }
                        if (ReasonFailFilter != string.Empty)
                        {
                            ReasonFailFilter = ReasonFailFilter.Substring(0, ReasonFailFilter.Length - 1);
                            item.ReasonFailFilter = ReasonFailFilter;
                        }
                        if (countAgree == countCondition)
                            lstCandidateResultPass.Add(item);
                        else
                            lstCandidateResultFail.Add(item);
                    }
                }
                // cap  nhat status và history cho cadidate pass
                if (lstCandidateResultPass != null && lstCandidateResultPass.Count != 0)
                {
                    var candidateservices = new Rec_CandidateServices();
                    var recruimenthistoryservices = new Rec_RecruitmentHistoryServices();
                    var ListIDsCadidate = lstCandidateResultPass.Select(x => x.ID).ToList();
                    var listCadidate = repoCadidate.FindBy(x => ListIDsCadidate.Contains(x.ID)).ToList();
                    //cập nhật status cho cadidate
                    foreach (var item1 in listCadidate)
                    {
                        item1.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_WAITINTERVIEW.ToString();
                        item1.PassFilterResume = true;
                        if (lstrecruitmentHistory != null)
                        {
                            var listCandidateHis = lstrecruitmentHistory.Where(x => x.CandidateID == item1.ID).ToList();
                            if (listCandidateHis != null && listCandidateHis.Count != 0)
                            {
                                listCandidateHis = listCandidateHis.OrderByDescending(x => x.DateApply).ToList();
                                var objcandidatehis = listCandidateHis.FirstOrDefault();
                                objcandidatehis.PassFilterResume = true;
                                objcandidatehis.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_WAITINTERVIEW.ToString();
                                recruimenthistoryservices.Edit(objcandidatehis);
                            }
                        }
                        candidateservices.Edit(item1);
                    }
                    repoRecruimentHistory.SaveChanges();
                    repoCadidate.SaveChanges();
                }

                // cap  nhat status và history cho cadidate fail
                if (lstCandidateResultFail != null && lstCandidateResultFail.Count != 0)
                {
                    var candidateservices = new Rec_CandidateServices();
                    var recruimenthistoryservices = new Rec_RecruitmentHistoryServices();
                    var ListIDsCadidate = lstCandidateResultFail.Select(x => x.ID).ToList();
                    var listCadidate = repoCadidate.FindBy(x => ListIDsCadidate.Contains(x.ID)).ToList();
                    //cập nhật status cho cadidate
                    foreach (var item1 in listCadidate)
                    {
                        //if()
                        //item1.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_FAIL.ToString();
                        var reasonbyCandidate = lstCandidateResultFail.Where(s => s.ID == item1.ID).FirstOrDefault();
                        item1.PassFilterResume = false;
                        item1.Status = EnumDropDown.CandidateStatus.E_FAILFILTERRESUME.ToString();
                        item1.ReasonFailFilter = reasonbyCandidate != null ? reasonbyCandidate.ReasonFailFilter : null;
                        if (lstrecruitmentHistory != null)
                        {
                            var listCandidateHis = lstrecruitmentHistory.Where(x => x.CandidateID == item1.ID).ToList();
                            if (listCandidateHis != null && listCandidateHis.Count != 0)
                            {
                                listCandidateHis = listCandidateHis.OrderByDescending(x => x.DateApply).ToList();
                                var objcandidatehis = listCandidateHis.FirstOrDefault();
                                objcandidatehis.PassFilterResume = false;
                                objcandidatehis.Status = EnumDropDown.CandidateStatus.E_FAILFILTERRESUME.ToString();
                                recruimenthistoryservices.Edit(objcandidatehis);
                            }
                        }
                        candidateservices.Edit(item1);
                    }
                    repoRecruimentHistory.SaveChanges();
                    repoCadidate.SaveChanges();
                }

            }
            if (GetListFail)
            {
                return lstCandidateResultFail;
            }
            else
            {
                return lstCandidateResultPass;
            }
        }
        // Click nút gọi điện ds ứng viên trúng tuyển
        public void SubmitCall(List<Guid> selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_CandidateRepository(unitOfWork);

                var lstCandidates = repo.FindBy(m => m.ID != null && selectedIds.Contains(m.ID)).ToList();
                foreach (var Candidate in lstCandidates)
                {
                    if (Candidate.CallNumber.HasValue)
                    {
                        Candidate.CallNumber = Candidate.CallNumber + 1;
                    }
                    else
                    {
                        Candidate.CallNumber = 1;
                    }
                }
                repo.SaveChanges();
            }
        }
        public void UpdateStatusHireCandidate(string selectedIds, string userID, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var baseService = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string code = "";
                string status = string.Empty;
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                var repoHistory = new Rec_RecruitmentHistoryRepository(unitOfWork);
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var CandidateHistoryRepository = new Hre_CandidateHistoryRepository(unitOfWork);
                var lstProfile = new List<Hre_Profile>();
                List<Guid> lstIDs = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstCandidates = repoCandidate.FindBy(x => lstIDs.Contains(x.ID)).ToList();
                var lstRecruimentHistory = repoHistory.FindBy(m => m.CandidateID != null && lstIDs.Contains(m.CandidateID)).ToList();
                 var objs = new List<object>();
                objs.Add(1);
                objs.Add(int.MaxValue - 1);
                var lstcandidatehistory = baseService.GetData<Hre_CandidateHistory>(objs, ConstantSql.hrm_rec_sp_get_CandidateHistory, UserLogin,ref status);
                foreach (var Candidate in lstCandidates)
                {
                    string firstname = string.Empty;
                    string namefamily = string.Empty;

                    if (Candidate.CandidateName != null)
                    {
                        var col_array = Candidate.CandidateName.Split(' ');
                        for (var i = 0; i < col_array.Length; i++)
                        {
                            if (i == col_array.Length - 1)
                                firstname = col_array[i];
                            else
                                namefamily += col_array[i] + " ";
                        }
                    }

                    Candidate.StatusHire = HRM.Infrastructure.Utilities.EnumDropDown.HireStatus.E_HIRE.ToString();
                    Candidate.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_HIRE.ToString();
                    Candidate.DateUpdate = DateTime.Now; 
                    Hre_Profile Profile = new Hre_Profile();
                    Profile.ID = Guid.NewGuid();
                    Profile.CandidateID = Candidate.ID;
                    Profile.ProfileName = Candidate.CandidateName;
                    Profile.FirstName = firstname;
                    Profile.NameFamily = namefamily;
                    Profile.NameEnglish = Candidate.NameEnglish;
                    Profile.ImagePath = Candidate.ImagePath;
                    Profile.StatusSyn = ProfileStatusSyn.E_WAITING.ToString();
                    Profile.CandidateID = Candidate.ID;
                    Profile.OrgStructureID = Candidate.OrgStructureID;
                    Profile.PositionID = Candidate.PositionID;
                    Profile.Gender = Candidate.Gender;
                    Profile.DateOfBirth = Candidate.DateOfBirth;
                    if (Candidate.DateOfBirth != null)
                    {
                        Profile.DayOfBirth = Candidate.DateOfBirth.Day;
                        Profile.MonthOfBirth = Candidate.DateOfBirth.Month;
                        Profile.YearOfBirth = Candidate.DateOfBirth.Year;
                    }
                    Profile.PlaceOfBirth = Candidate.PlaceOfBirth;
                    Profile.NationalityID = Candidate.NationalityID;
                    Profile.EthnicID = Candidate.EthnicID;
                    Profile.ReligionID = Candidate.ReligionID;
                    Profile.Height = Candidate.Height;
                    Profile.Weight = Candidate.Weight;
                    Profile.IDNo = Candidate.IdentifyNumber;
                    Profile.IDDateOfIssue = Candidate.IDDateOfIssue;
                    Profile.IDPlaceOfIssue = Candidate.IDPlaceOfIssue;
                    Profile.PassportNo = Candidate.PassportNo;
                    Profile.PassportDateOfExpiry = Candidate.DateExpiresPassport;
                    Profile.Email = Candidate.Email;
                    //Profile.Cellphone = Candidate.Phone;
                    Profile.PADistrictID = Candidate.PDistrictID;
                    Profile.PAddress = Candidate.PAddress;
                    Profile.TADistrictID = Candidate.TDistrictID;
                    Profile.TAddress = Candidate.TAddress;
                    Profile.JobTitleID = Candidate.JobTitleID;
                    Profile.TagID = Candidate.TagID;
                    Profile.EducationLevelID = Candidate.EducationLevelID;
                    Profile.MarriageStatus = Candidate.MarriageStatus;
                    Profile.Origin = Candidate.Origin;
                    Profile.FileAttach = Candidate.FileAttachment;
                    Profile.SikillLevel = Candidate.SkillLevel;
                    Profile.PlaceOfIssueID = Candidate.PlaceOfIssueID;
                    Profile.PlaceOfBirth = Candidate.PlaceOfBirth;
                    Profile.ReasonDeny = Candidate.ReasonDeny;
                    Profile.StatusHire = Candidate.StatusHire;
                    Profile.StatusSyn = ProfileStatusSyn.E_WAITING.ToString();
                    Profile.PassportDateOfIssue = Candidate.DateIssuePassport;
                    Profile.Cellphone = Candidate.Mobile;
                    Profile.TCountryID = Candidate.TCountryID;
                    Profile.TProvinceID = Candidate.TProvinceID;
                    Profile.TDistrictID = Candidate.TDistrictID;
                    Profile.PCountryID = Candidate.PCountryID;
                    Profile.PProvinceID = Candidate.PProvinceID;
                    Profile.PDistrictID = Candidate.PDistrictID;
                    Profile.HomePhone = Candidate.Phone;
                    Profile.Notes = Candidate.Assessment;
                    // cập nhật trạng thái của lịch sử
                    var hisbycandidate = lstRecruimentHistory.Where(s => s.CandidateID == Candidate.ID).OrderByDescending(s => s.DateApply).FirstOrDefault();
                    hisbycandidate.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_HIRE.ToString();
                    var candidateHistoryByCandidate = lstcandidatehistory.Where(s => s.CandidateID == Candidate.ID).OrderByDescending(s => s.DateStart).FirstOrDefault();
                    repoProfile.Add(Profile);
                    if (candidateHistoryByCandidate != null)
                    {
                        candidateHistoryByCandidate.ProfileID = Profile.ID;
                        CandidateHistoryRepository.Edit(candidateHistoryByCandidate);
                    }
                }
                unitOfWork.SaveChanges();
                repoHistory.SaveChanges();
            }
        }
        public Hre_Profile AddProfileByCandidate(Rec_Candidate Candidate)
        {

            Hre_Profile Profile = new Hre_Profile();
            Profile.ID = Guid.NewGuid();
            Profile.CandidateID = Candidate.ID;
            Profile.ProfileName = Candidate.CandidateName;
            Profile.NameEnglish = Candidate.NameEnglish;
            Profile.ImagePath = Candidate.ImagePath;
            Profile.StatusSyn = ProfileStatusSyn.E_WAITING.ToString();
            Profile.CandidateID = Candidate.ID;
            Profile.OrgStructureID = Candidate.OrgStructureID;
            Profile.PositionID = Candidate.PositionID;
            Profile.Gender = Candidate.Gender;
            Profile.DateOfBirth = Candidate.DateOfBirth;
            Profile.PlaceOfBirth = Candidate.PlaceOfBirth;
            Profile.NationalityID = Candidate.NationalityID;
            Profile.EthnicID = Candidate.EthnicID;
            Profile.ReligionID = Candidate.ReligionID;
            Profile.Height = Candidate.Height;
            Profile.Weight = Candidate.Weight;
            Profile.IDNo = Candidate.IdentifyNumber;
            Profile.IDDateOfIssue = Candidate.IDDateOfIssue;
            Profile.IDPlaceOfIssue = Candidate.IDPlaceOfIssue;
            Profile.PassportNo = Candidate.PassportNo;
            Profile.PassportDateOfExpiry = Candidate.DateExpiresPassport;
            Profile.Email = Candidate.Email;
            Profile.Cellphone = Candidate.Phone;
            Profile.TCountryID = Candidate.TCountryID;
            Profile.TProvinceID = Candidate.TProvinceID;
            Profile.TDistrictID = Candidate.TDistrictID;
            Profile.PADistrictID = Candidate.PDistrictID;
            Profile.PAddress = Candidate.PAddress;
            Profile.TADistrictID = Candidate.TDistrictID;
            Profile.TAddress = Candidate.TAddress;
            Profile.PCountryID = Candidate.PCountryID;
            Profile.PProvinceID = Candidate.PProvinceID;
            Profile.PDistrictID = Candidate.PDistrictID;
            Profile.JobTitleID = Candidate.JobTitleID;
            Profile.TagID = Candidate.TagID;
            Profile.EducationLevelID = Candidate.EducationLevelID;
            Profile.MarriageStatus = Candidate.MarriageStatus;
            Profile.Origin = Candidate.Origin;
            Profile.FileAttach = Candidate.FileAttachment;
            Profile.SikillLevel = Candidate.SkillLevel;
            Profile.PlaceOfIssueID = Candidate.PlaceOfIssueID;
            Profile.PlaceOfBirth = Candidate.PlaceOfBirth;
            Profile.ReasonDeny = Candidate.ReasonDeny;
            Profile.StatusHire = Candidate.StatusHire;
            Profile.Origin = Candidate.Origin;
            Profile.PassportDateOfIssue = Candidate.DateIssuePassport;
            return Profile;
        }
        // Trở lại ds chờ phỏng vấn
        public string BackToInterview(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                string message = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                var repoHistory = new Rec_RecruitmentHistoryRepository(unitOfWork);
                List<Guid> lstcandidateIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstCandidates = repoCandidate.FindBy(m => m.ID != null && lstcandidateIds.Contains(m.ID)).ToList();
                var lstRecruimentHistory = repoHistory.FindBy(m => m.CandidateID != null && lstcandidateIds.Contains(m.CandidateID)).ToList();
                foreach (var Candidate in lstCandidates)
                {
                    Candidate.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_WAITINTERVIEW.ToString();
                    Candidate.StatusHire = null;
                    Candidate.IsBlackList = null;
                    var hisbycandidate = lstRecruimentHistory.Where(s => s.CandidateID == Candidate.ID).OrderByDescending(s => s.DateApply).FirstOrDefault();
                    hisbycandidate.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_WAITINTERVIEW.ToString();
                    hisbycandidate.CandidateID = Candidate.ID;
                }
                repoCandidate.SaveChanges();
                repoHistory.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
        #region Send Mail
        public bool SendMailCandidateFail(List<Guid> candidateIds, bool isPass)
        {
            var isSuccess = false;
            #region template
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoRecCandidate = new Rec_CandidateRepository(unitOfWork);
                var candidates = unitOfWork.CreateQueryable<Rec_Candidate>(Guid.Empty, m => candidateIds.Contains(m.ID)).ToList();
                var jobVacancyIds = candidates.Select(m => m.JobVacancyID).ToList();
                var jobVacancyObjs = unitOfWork.CreateQueryable<Rec_JobVacancy>(Guid.Empty, m => jobVacancyIds.Contains(m.ID)).Select(m => new {
                m.ID,m.JobVacancyName
                }).ToList();

                string _typeTemplate = EnumDropDown.EmailType.E_NOTIFYCANDIDATEFAIL.ToString();
                var template = unitOfWork.CreateQueryable<Sys_TemplateSendMail>(Guid.Empty, m => m.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                {
                    return false;
                }
                else
                {
                    BaseService _base = new BaseService();
                    foreach (var item in candidates)
                    {

                        #region set so lan goi email
                        if (item.NoEmailFail == null)
                        {
                            item.NoEmailFail = 0;
                        }
                        if (item.NoEmailPass == null)
                        {
                            item.NoEmailPass = 0;
                        }
                        if (isPass)
                        {
                            item.NoEmailPass++;
                        }
                        else
                        {
                            item.NoEmailFail++;
                        }
                        #endregion

                        string title = template.Subject;
                        #region mergeData
                        var jobVancancyName = jobVacancyObjs.Where(m => m.ID == item.JobVacancyID).Select(m => m.JobVacancyName).FirstOrDefault();
                        string[] strsParaKey = null;
                        string[] strsParaValues = null;
                        strsParaKey = new string[] 
                        { 
                            EnumDropDown.EmailCandidateParam.E_CandidateName.ToString(), 
                            EnumDropDown.EmailCandidateParam.E_CodeCandidate.ToString(), 
                            EnumDropDown.EmailCandidateParam.E_ReasonFailFilter.ToString(), 
                            EnumDropDown.EmailCandidateParam.E_WorkplaceSuggestion.ToString(), 
                            EnumDropDown.EmailCandidateParam.E_JobVacancyID.ToString(), 
                            EnumDropDown.EmailCandidateParam.E_NoEmailPass.ToString(), 
                            EnumDropDown.EmailCandidateParam.E_NoEmailFail.ToString(), 

                        };
                        strsParaValues = new string[] 
                        { 
                            item.CandidateName,
                            item.CodeCandidate,
                            item.ReasonFailFilter,
                            item.WorkplaceSuggestion,
                            jobVancancyName,
                            item.NoEmailPass.ToString(),
                            item.NoEmailFail.ToString()
                        };
                        string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                        isSuccess = _base.SendMail(title, item.Email, body, string.Empty);
                        #endregion
                    }

                    if (candidates.Any())
                    {
                        repoRecCandidate.Edit(candidates);
                        repoRecCandidate.SaveChanges();
                    }
                }
            }
            #endregion

            return isSuccess;
        }
        #endregion
        public string ActionCancelCandidate(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                string message = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                var repoHistory = new Rec_RecruitmentHistoryRepository(unitOfWork);
                List<Guid> lstcandidateIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstCandidates = repoCandidate.FindBy(m => m.ID != null && lstcandidateIds.Contains(m.ID)).ToList();
                var lstRecruimentHistory = repoHistory.FindBy(m => m.CandidateID != null && lstcandidateIds.Contains(m.CandidateID)).ToList();
                foreach (var Candidate in lstCandidates)
                {
                    Candidate.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_CANCEL.ToString();
                    Candidate.DateUpdate = DateTime.Now;
                    var hisbycandidate = lstRecruimentHistory.Where(s => s.CandidateID == Candidate.ID).OrderByDescending(s => s.DateApply).FirstOrDefault();
                    hisbycandidate.Status = HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_CANCEL.ToString();
                    hisbycandidate.CandidateID = Candidate.ID;
                    hisbycandidate.DateUpdate = DateTime.Now;
                }
                repoCandidate.SaveChanges();
                repoHistory.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
    }
}
