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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Domain
{
    public class Rec_InterviewServices : BaseService
    {
        // Click nút gọi điện ds ứng viên trúng tuyển
        public string ComputeResultInterview(string selectedIds, string UserLogin)
        {
            List<Rec_Interview> lstInterviewResultPass = new List<Rec_Interview>();
            List<Rec_Interview> lstInterviewResultFail = new List<Rec_Interview>();
            List<Rec_Candidate> lstCandidate = new List<Rec_Candidate>();
            List<Rec_RecruitmentHistory> lstRecruitmentHistory = new List<Rec_RecruitmentHistory>();
            using (var context = new VnrHrmDataContext())
            {
                int? levelinterview = 0;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_InterviewRepository(unitOfWork);
                var services = new Rec_InterviewServices();
                var Candidateservices = new Rec_CandidateServices();
                var Historyservices = new Rec_RecruitmentHistoryServices();
                BaseService service = new BaseService();
                List<Guid> lstIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstinterviews = repo.FindBy(m => m.ID != null && lstIds.Contains(m.ID)).ToList();

                #region Lấy ứng viên theo phỏng vấn
                var lstcandidateIDs = lstinterviews.Select(s => s.CandidateID).ToList();
                string strCandidateIDs = string.Empty;
                foreach (var candidatebyinterview in lstcandidateIDs)
                {
                    strCandidateIDs += Common.DotNetToOracle(candidatebyinterview.ToString()) + ",";
                }
                if (strCandidateIDs.IndexOf(",") > 0)
                    strCandidateIDs = strCandidateIDs.Substring(0, strCandidateIDs.Length - 1);

                var lstcandidate = service.GetData<Rec_Candidate>(strCandidateIDs, ConstantSql.hrm_rec_sp_get_CandidateByIds, UserLogin, ref status).ToList();
                var lstrecruitmentHistory = service.GetData<Rec_RecruitmentHistory>(strCandidateIDs, ConstantSql.hrm_hr_sp_get_RecHisByCandidateIds, UserLogin, ref status).ToList();
                #endregion

                #region Lấy nhóm đk tuyển theo phỏng vấn
                var lstgroupconditionIDs = lstinterviews.Where(s => s.CandidateID != null).Select(s => s.GroupConditionID).ToList();
                string strgroupIDs = string.Empty;
                foreach (var candidatebyinterview in lstgroupconditionIDs)
                {
                    strgroupIDs += Common.DotNetToOracle(candidatebyinterview.ToString()) + ",";
                }
                if (strgroupIDs.IndexOf(",") > 0)
                    strgroupIDs = strgroupIDs.Substring(0, strgroupIDs.Length - 1);

                var lstgroupconditions = service.GetData<Rec_GroupConditionEntity>(strgroupIDs, ConstantSql.hrm_rec_sp_get_GroupConditionByIds, UserLogin, ref status).ToList();
                if (lstgroupconditions == null)
                {
                    return null;
                }
                #endregion

                #region Lấy yêu cầu tuyển theo ứng viên
                var lstJobVaCancyIDs = lstcandidate.Select(s => s.JobVacancyID).ToList();
                string strJobVacancyIds = string.Empty;
                foreach (Guid candidatebyinterview in lstJobVaCancyIDs)
                {
                    strJobVacancyIds += candidatebyinterview;
                    strJobVacancyIds += ",";
                }
                if (strJobVacancyIds.Length > 0)
                {
                    strJobVacancyIds = strJobVacancyIds.Substring(0, strJobVacancyIds.Length - 1);
                }
                var lstJobVacancy = service.GetData<Rec_JobVacancyEntity>(Common.DotNetToOracle(strJobVacancyIds), ConstantSql.hrm_rec_sp_get_JobVacancyByIds, UserLogin, ref status).ToList();
                var repoJobCondition = new Rec_JobConditionRepository(unitOfWork);
                var lstJobCondition = repoJobCondition.FindBy(s => s.IsDelete == null).ToList();
                #endregion

                #region Lấy ds nv nghỉ việc
                var ProfileServices = new Hre_ProfileServices();
                List<object> lstparasearchpro = new List<object>();
                lstparasearchpro = new List<object>();
                lstparasearchpro.AddRange(new object[19]);
                lstparasearchpro[17] = 1;
                lstparasearchpro[18] = Int32.MaxValue - 1;
                var lstProfile = service.GetData<Hre_ProfileEntity>(lstparasearchpro, ConstantSql.hrm_hr_sp_get_ProfileQuit, UserLogin, ref status).ToList();
                #endregion

                #region Lấy ds bệnh
                List<object> lstparadiseelist = new List<object>();
                lstparadiseelist.Add(null);
                lstparadiseelist.Add(EnumDropDown.EntityType.E_SICK_REC.ToString());
                lstparadiseelist.Add(1);
                lstparadiseelist.Add(int.MaxValue - 1);
                var lstsick = service.GetData<Cat_NameEntityEntity>(lstparadiseelist, ConstantSql.hrm_cat_sp_get_LevelGeneral, UserLogin, ref status).ToList();

                #endregion

                #region Lấy ds trình độ học vấn
                List<object> lstparaEdu = new List<object>();
                lstparaEdu.Add(null);
                lstparaEdu.Add(1);
                lstparaEdu.Add(int.MaxValue - 1);
                var lstEducationLevel = service.GetData<Cat_NameEntityEntity>(lstparaEdu, ConstantSql.hrm_cat_sp_get_EducationLevel, UserLogin, ref status).ToList();
                #endregion

                string message = NotificationType.Success.ToString();
                foreach (var interview in lstinterviews)
                {
                    var candidatebyinterview = lstcandidate.Where(s => s.ID == interview.CandidateID.Value && s.JobVacancyID != null).FirstOrDefault();
                    if(candidatebyinterview.JobVacancyID == null)
                    {
                        message = ConstantMessages.Fail;
                        continue;
                    }
                    var jobvacancybyCandidate = lstJobVacancy.Where(s => candidatebyinterview.JobVacancyID != null && s.ID == candidatebyinterview.JobVacancyID.Value).FirstOrDefault();
                    var lstconditionidsbygroupcondition = lstgroupconditions.Where(s => s.ID == interview.GroupConditionID).FirstOrDefault();
                    if (lstconditionidsbygroupcondition == null)
                    {
                        continue;
                    }

                    if (interview.LevelInterview > jobvacancybyCandidate.NoLevelInterview || candidatebyinterview.Status == EnumDropDown.CandidateStatus.E_PASS.ToString()
                    || candidatebyinterview.Status == EnumDropDown.CandidateStatus.E_FAIL.ToString() || candidatebyinterview.Status == EnumDropDown.CandidateStatus.E_HIRE.ToString())
                    {
                        message = ConstantDisplay.HRM_Rec_EmpHaveResult;
                        continue;
                    }
                    List<Guid> ids = new List<Guid>();
                    ids = lstconditionidsbygroupcondition.JobConditionIDs
                           .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(x => Guid.Parse(x))
                           .ToList();
                    var lstconditionbyGroup = lstJobCondition.Where(s => ids.Contains(s.ID)).ToList();
                    int countCondition = lstconditionbyGroup.Count;
                    int countAgree = 0;
                    #region MyRegion
                    foreach (var Condition in lstconditionbyGroup)
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
                            if (candidatebyinterview.Interview == null)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueInterview1 != 0 && candidatebyinterview.Interview.HasValue)
                            {

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueInterview1 <= candidatebyinterview.Interview)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueInterview1 >= candidatebyinterview.Interview)
                                {
                                    countAgree++;
                                    continue;
                                }
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueInterview1 == candidatebyinterview.Interview)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueInterview1 != 0 && candidatebyinterview.Interview.HasValue)
                            {


                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueInterview1 <= candidatebyinterview.Interview)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueInterview1 >= candidatebyinterview.Interview)
                                {
                                    countAgree++;
                                    continue;
                                }
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueInterview1 == candidatebyinterview.Interview)
                                {
                                    countAgree++;
                                    continue;
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
                            if (candidatebyinterview.WriteTest == null)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueWriteTest1 != 0 && candidatebyinterview.WriteTest.HasValue)
                            {

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueWriteTest1 <= candidatebyinterview.WriteTest)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueWriteTest1 >= candidatebyinterview.WriteTest)
                                {
                                    countAgree++;
                                    continue;
                                }
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueWriteTest1 == candidatebyinterview.WriteTest)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueWriteTest1 != 0 && candidatebyinterview.WriteTest.HasValue)
                            {


                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueWriteTest1 <= candidatebyinterview.WriteTest)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueWriteTest1 >= candidatebyinterview.WriteTest)
                                {
                                    countAgree++;
                                    continue;
                                }
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueWriteTest1 == candidatebyinterview.WriteTest)
                                {
                                    countAgree++;
                                    continue;
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
                            if (candidatebyinterview.GenaralHealth == null)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueHealth1 != 0 && candidatebyinterview.GenaralHealth.HasValue)
                            {


                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueHealth1 <= candidatebyinterview.GenaralHealth)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueHealth1 >= candidatebyinterview.GenaralHealth)
                                {
                                    countAgree++;
                                    continue;
                                }
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueHealth1 == candidatebyinterview.GenaralHealth)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && candidatebyinterview.GenaralHealth.HasValue && valueHealth1 != 0)
                            {


                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueHealth1 <= candidatebyinterview.GenaralHealth)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueHealth1 >= candidatebyinterview.GenaralHealth)
                                {
                                    countAgree++;
                                    continue;
                                }
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueHealth1 == candidatebyinterview.GenaralHealth)
                                {
                                    countAgree++;
                                    continue;
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
                            if (candidatebyinterview.Height == null)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueHeight1 != 0 && candidatebyinterview.Height.HasValue)
                            {


                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && valueHeight1 <= candidatebyinterview.Height)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && valueHeight1 >= candidatebyinterview.Height)
                                {
                                    countAgree++;
                                    continue;
                                }
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueHeight1 == candidatebyinterview.Height)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && candidatebyinterview.Height.HasValue && valueHeight1 != 0)
                            {


                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.Height <= valueHeight1)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.Height >= valueHeight1)
                                {

                                    countAgree++;
                                    continue;
                                }
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && valueHeight1 == candidatebyinterview.Height)
                                {
                                    countAgree++;
                                    continue;
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
                            if (candidatebyinterview.Weight == null)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueWeight1 != 0 && candidatebyinterview.Weight.HasValue)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.Weight == valueWeight1)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.Weight >= valueWeight1)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.Height <= valueWeight1)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueWeight1 != 0 && candidatebyinterview.Weight.HasValue)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.Weight == valueWeight1)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.Weight <= valueWeight1)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.Height >= valueWeight1)
                                {
                                    countAgree++;
                                    continue;
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
                                //  Eyes = double.Parse(candidatebyinterview.LevelEye);
                            }
                            catch { }
                            // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                            if (!candidatebyinterview.LevelEye.HasValue)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueEyes1 != 0)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.LevelEye.Value == valueEyes1)
                                {
                                    countAgree++;
                                    continue;

                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.LevelEye.Value >= valueEyes1)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.LevelEye.Value <= valueEyes1)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueEyes1 != 0)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.LevelEye.Value == valueEyes1)
                                {
                                    countAgree++;
                                    continue;

                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.LevelEye.Value <= valueEyes1)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.LevelEye.Value >= valueEyes1)
                                {
                                    countAgree++;
                                    continue;
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
                                //  Eyes = double.Parse(candidatebyinterview.LevelEye);
                            }
                            catch { }
                            // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                            if (!candidatebyinterview.LevelEye.HasValue)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valueEyes1 != 0)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.LevelEyeRight.Value == valueEyes1)
                                {
                                    countAgree++;
                                    continue;

                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.LevelEyeRight.Value >= valueEyes1)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.LevelEyeRight.Value <= valueEyes1)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valueEyes1 != 0)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.LevelEyeRight.Value == valueEyes1)
                                {
                                    countAgree++;
                                    continue;

                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.LevelEyeRight.Value <= valueEyes1)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.LevelEyeRight.Value >= valueEyes1)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }

                        }
                        #endregion
                        #endregion
                        #region Kiểm tra Thời Gian Trượt Kỳ Trước
                        if (Condition.ConditionName == ConditionName.E_DURATIONFAILPREVIOUS.ToString())
                        {
                            int valueDuration1 = int.Parse(Condition.Value1);
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                            {

                                var listCandidateHis = lstrecruitmentHistory.Where(x => x.CandidateID == candidatebyinterview.ID && x.Status == HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_FAIL.ToString())
                                    .OrderByDescending(x => x.DateApply).ToList();
                                if (listCandidateHis == null || listCandidateHis.Count == 0)
                                {
                                    countAgree++;
                                    continue;
                                }
                                if (listCandidateHis != null && listCandidateHis.Count != 0)
                                {
                                    var CandidateHistory = listCandidateHis.FirstOrDefault();
                                    int Monthhistory = CandidateHistory.DateApply.Value.Month;
                                    int monthcheck = candidatebyinterview.DateApply.Value.Month;

                                    int month = Math.Abs(monthcheck - Monthhistory);

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
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                            {

                                var listCandidateHis = lstrecruitmentHistory.Where(x => x.CandidateID == candidatebyinterview.ID && x.Status == HRM.Infrastructure.Utilities.EnumDropDown.CandidateStatus.E_FAIL.ToString())
                                    .OrderByDescending(x => x.DateApply).ToList();
                                if (listCandidateHis == null || listCandidateHis.Count == 0)
                                {
                                    countAgree++;
                                    continue;
                                }
                                if (listCandidateHis != null && listCandidateHis.Count != 0)
                                {

                                    var CandidateHistory = listCandidateHis.FirstOrDefault();
                                    int Monthhistory = CandidateHistory.DateApply.Value.Month;

                                    int monthcheck = candidatebyinterview.DateApply.Value.Month;

                                    int month = Math.Abs(monthcheck - Monthhistory);

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
                                }
                            }

                        }
                        #endregion
                        #region Kiểm tra Từ Tuổi
                        if (Condition.ConditionName == ConditionName.E_AGE.ToString())
                        {
                            // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                            if (candidatebyinterview.DateOfBirth == null)
                            {
                                countAgree++;
                                continue;
                            }
                            double? AgeCadidate = (new DateTime(DateTime.Now.Subtract(candidatebyinterview.DateOfBirth).Ticks).Year - 1) > 0 ? (new DateTime(DateTime.Now.Subtract(candidatebyinterview.DateOfBirth).Ticks).Year - 1) : (DateTime.Now.Subtract(candidatebyinterview.DateOfBirth).TotalDays / 365.242199);
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
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && AgeCadidate > 0)
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
                            }
                        }
                        #endregion
                        #region Kiểm tra Giới Tính
                        if (Condition.ConditionName == ConditionName.E_GENDER.ToString())
                        {
                            // nếu giá trị của ứng viên là null thì ko kiểm tra =>thỏa
                            if (string.IsNullOrEmpty(candidatebyinterview.Gender))
                            {
                                countAgree++;
                                continue;
                            }
                            string valueGender = Condition.Value1;

                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString())
                            {

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.Gender.Equals(valueGender))
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && !candidatebyinterview.Gender.Equals(valueGender))
                                {
                                    countAgree++;
                                    continue;
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
                                    if (!string.IsNullOrEmpty(candidatebyinterview.IdentifyNumber))
                                    {
                                        var Profile = lstProfile.FirstOrDefault(x => !string.IsNullOrEmpty(x.IDNo) && x.IDNo == candidatebyinterview.IdentifyNumber);
                                        if (Profile == null)
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                    }
                                    // nếu ko có CMND thì kiểm tra tên và ngày sinh 
                                    else if (!string.IsNullOrEmpty(candidatebyinterview.CandidateName) && candidatebyinterview.DateOfBirth != null)
                                    {
                                        var Profile = lstProfile.FirstOrDefault(x => !string.IsNullOrEmpty(x.ProfileName) && x.DateOfBirth.HasValue && x.ProfileName == candidatebyinterview.CandidateName && candidatebyinterview.DateOfBirth == x.DateOfBirth.Value);
                                        if (Profile == null)
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                    }

                                }
                            }
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                            {
                                if (lstProfile != null && lstProfile.Count != 0)
                                {
                                    // nếu có CMND thì ưu tiên kiểm tra trước
                                    if (!string.IsNullOrEmpty(candidatebyinterview.IdentifyNumber))
                                    {
                                        var Profile = lstProfile.FirstOrDefault(x => !string.IsNullOrEmpty(x.IDNo) && x.IDNo == candidatebyinterview.IdentifyNumber);
                                        if (Profile == null)
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                    }
                                    // nếu ko có CMND thì kiểm tra tên và ngày sinh 
                                    else if (!string.IsNullOrEmpty(candidatebyinterview.CandidateName) && candidatebyinterview.DateOfBirth != null)
                                    {
                                        var Profile = lstProfile.FirstOrDefault(x => !string.IsNullOrEmpty(x.ProfileName) && x.DateOfBirth.HasValue && x.ProfileName == candidatebyinterview.CandidateName && candidatebyinterview.DateOfBirth.Date == x.DateOfBirth.Value.Date);
                                        if (Profile == null)
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                    }

                                }


                            }
                        }
                        #endregion
                        #region Kiểm Tra Bệnh Loại Trừ
                        if (Condition.ConditionName == ConditionName.E_DISEASEIDS.ToString())
                        {
                            // nếu ko có bệnh thì qua
                            if (string.IsNullOrEmpty(candidatebyinterview.DiseaseListIDs))
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
                                    var lstDiseseCandidate = candidatebyinterview.DiseaseListIDs.Split(',').ToList();
                                    if (lstsickbycondition.Where(x => lstDiseseCandidate.Contains(x.Code)).Count() == 0)
                                    {
                                        countAgree++;
                                        continue;
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
                                    var lstDiseseCandidate = candidatebyinterview.DiseaseListIDs.Split(',').ToList();
                                    if (lstsickbycondition.Where(x => lstDiseseCandidate.Contains(x.Code)).Count() != 0)
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Kiểm tra điểm thi
                        #region Điểm 1
                        if (Condition.ConditionName == ConditionName.E_SCORE1.ToString())
                        {

                            double? valueScore1Condition = double.Parse(Condition.Value1);
                            double? valueScore1 = interview.Score1;
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
                            }
                        }
                        #endregion
                        #region Điểm 2
                        if (Condition.ConditionName == ConditionName.E_SCORE2.ToString())
                        {
                            double? valueScore2Condition = double.Parse(Condition.Value1);
                            double? valueScore2 = interview.Score2;
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
                            }
                        }
                        #endregion
                        #region Điểm 3
                        if (Condition.ConditionName == ConditionName.E_SCORE3.ToString())
                        {
                            double? valueScore3Condition = double.Parse(Condition.Value1);
                            double? valueScore3 = interview.Score3;
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
                            if (candidatebyinterview.Musculoskeletal == null)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && valuemusculoskeletal != 0 && candidatebyinterview.Musculoskeletal.HasValue)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.Musculoskeletal == valuemusculoskeletal)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.Musculoskeletal >= valuemusculoskeletal)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.Musculoskeletal <= valuemusculoskeletal)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && valuemusculoskeletal != 0 && candidatebyinterview.Musculoskeletal.HasValue)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.Musculoskeletal != valuemusculoskeletal)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.Musculoskeletal < valuemusculoskeletal)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.Musculoskeletal > valuemusculoskeletal)
                                {
                                    countAgree++;
                                    continue;
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
                            if (candidatebyinterview.BloodPressure == null)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && bloodpressure != 0 && candidatebyinterview.BloodPressure.HasValue)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.BloodPressure == bloodpressure)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.BloodPressure >= bloodpressure)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.BloodPressure <= bloodpressure)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && bloodpressure != 0 && candidatebyinterview.BloodPressure.HasValue)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.BloodPressure != bloodpressure)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.BloodPressure < bloodpressure)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.BloodPressure > bloodpressure)
                                {
                                    countAgree++;
                                    continue;
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
                            if (candidatebyinterview.HeartBeat == null)
                            {
                                countAgree++;
                                continue;
                            }
                            // Điều kiện thỏa
                            if (Condition.ConditionBrand == ConditionBrand.E_AGREEMENT.ToString() && heartbeat != 0 && candidatebyinterview.HeartBeat.HasValue)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.HeartBeat == heartbeat)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.HeartBeat >= heartbeat)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.HeartBeat <= heartbeat)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }
                            //không thỏa
                            else if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString() && heartbeat != 0 && candidatebyinterview.HeartBeat.HasValue)
                            {
                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_EQUAL.ToString() && candidatebyinterview.HeartBeat != heartbeat)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_THAN.ToString() && candidatebyinterview.HeartBeat < heartbeat)
                                {
                                    countAgree++;
                                    continue;
                                }

                                if (Condition.ValueType == HRM.Infrastructure.Utilities.ValueType.E_LESS.ToString() && candidatebyinterview.HeartBeat > heartbeat)
                                {
                                    countAgree++;
                                    continue;
                                }
                            }

                        }
                        #endregion
                        #region Kiểm tra Trình độ học vấn
                        if (Condition.ConditionName == ConditionName.E_EDUCATIONLEVEL.ToString())
                        {
                            if (candidatebyinterview.EducationLevelID == null)
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
                                        var CodeEduOfCandidate = lstEducationLevel.Where(s => s.ID == candidatebyinterview.EducationLevelID).Select(s => s.Code).FirstOrDefault();

                                        if (lstCodeEducationLevelOfCondition.Contains(CodeEduOfCandidate))
                                        {
                                            countAgree++;
                                            continue;
                                        }
                                    }
                                }
                                if (Condition.ConditionBrand == ConditionBrand.E_NOTAGREEMENT.ToString())
                                {
                                    var lstEduCondition = Condition.Value1.Split(',').Select(x => x).ToList();
                                    var CodeEduOfCandidate = lstEducationLevel.Where(s => s.ID == candidatebyinterview.EducationLevelID).Select(s => s.Code).FirstOrDefault();
                                    if (!lstEduCondition.Contains(CodeEduOfCandidate))
                                    {
                                        countAgree++;
                                        continue;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                    // Son.Vo làm theo task 0045675 Nếu ko có vòng pv thì vòng pv = 0.
                    if (interview.LevelInterview == null) 
                    {
                        message = ConstantDisplay.HRM_Common_NotHaveLevelInterview.ToString();
                        continue;
                    }
                    else
                    {
                        levelinterview = interview.LevelInterview;
                    }

                    // Cập nhật lại trạng Ứng viên + lịch sử.
                    candidatebyinterview.LevelInterview = levelinterview;
                    var rechisbycandidate = lstrecruitmentHistory.Where(s => s.CandidateID == candidatebyinterview.ID).OrderByDescending(s => s.DateApply).FirstOrDefault();
                    interview.LevelInterview = levelinterview;
                    rechisbycandidate.LevelInterview = levelinterview;
                    if (countAgree == countCondition)
                    {
                        interview.Status = "E_PASS";
                        interview.ResultInterview = "E_PASS";
                        lstInterviewResultPass.Add(interview);
                        // Nếu là vòng pv cuối và kq là đậu thì cập nhật ứng viên + lịch sử
                        if (jobvacancybyCandidate != null && jobvacancybyCandidate.NoLevelInterview == levelinterview)
                        {
                            candidatebyinterview.Status = "E_PASS";
                            rechisbycandidate.Status = "E_PASS";
                        }
                    }
                    else
                    {
                        interview.Status = "E_FAIL";
                        interview.ResultInterview = "E_FAIL";
                        // Nếu là là rớt thì cập nhật ứng viên + lịch sử
                        candidatebyinterview.Status = "E_FAIL";
                        rechisbycandidate.Status = "E_FAIL";
                        lstInterviewResultFail.Add(interview);
                    }
                    lstCandidate.Add(candidatebyinterview);
                    lstRecruitmentHistory.Add(rechisbycandidate);
                }
                if (lstInterviewResultPass.Count > 0)
                {
                    services.Edit(lstInterviewResultPass);
                }
                if (lstInterviewResultFail.Count > 0)
                {

                    services.Edit(lstInterviewResultFail);
                }
                Candidateservices.Edit(lstCandidate);
                Historyservices.Edit(lstRecruitmentHistory);
                repo.SaveChanges();
                return message;
            }
        }
    }
}
