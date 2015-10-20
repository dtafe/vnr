using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Business.Category.Models;
using HRM.Business.Evaluation.Models;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Business.Evaluation.Domain
{
    /// <summary> BC Kết Quả Đánh Giá </summary>
    public class Eva_ReportEvaluationResultServices : BaseService
    {
        private string status = string.Empty;

        public List<Eva_ReportEvaluationResultEntity> GetReportEvaluationResult(string orgs, DateTime periodFromDate, DateTime periodToDate, Guid performanceTemplateId,string userLogin)
        {
            /*
            *  Goal(Báo cáo kết quả đánh giá)
            *  Steps :
            *      Step1  :  Lay thông tin ds NV trong Eva_PerformanceEva
            *      Step2  :  Lay mark,Evaluation của các cấp đánh giá ứng với 1 NV
            *      Step3  :  
            */

            var reportResult = new List<Eva_ReportEvaluationResultEntity>();

            #region Get PerformanceTypeID
            var performanceTypeId = Guid.Empty;
            var strPerfromanceTemplateId = Common.DotNetToOracle(performanceTemplateId.ToString());
            var performanceType = GetFirstData<Eva_PerformanceTemplateEntity>(strPerfromanceTemplateId, ConstantSql.hrm_cat_sp_get_TemplateById, userLogin, ref status).CopyData<Eva_PerformanceTemplateEntity>();
            if (performanceType != null)
            {
                performanceTypeId = performanceType.PerformanceTypeID ?? Guid.Empty;
            }
            #endregion

            #region Get performances by store
            var lstperformanceObjs = new List<object> { null, null, null, null, null, null, null, null, null, null, null, null, 1, 1000 };
            var performances = GetData<Eva_PerformanceEntity>(lstperformanceObjs, ConstantSql.hrm_eva_sp_get_Performance, userLogin, ref status);

            #endregion

            #region Get All Evaluators
            var lstObj = new List<object> { null, null, 1, 1000 };
            var listEvaluators = GetData<Eva_EvaluatorEntity>(lstObj, ConstantSql.hrm_eva_sp_get_Evaluator, userLogin, ref status);
            //get profileIds from Eva_Evaluator table
            var profileIds = performances.Select(p => p.ProfileID ?? Guid.Empty).ToList();

            var dicProfiles = new Dictionary<Guid, List<Guid>>();
            foreach (var dicProfile in profileIds)
            {
                var itemEvaluatorIds = listEvaluators.Where(p => p.ProfileID != Guid.Empty && p.ProfileID == dicProfile
                    && p.PerformanceTypeID == performanceTypeId)
                    .Select(p => p.EvaluatorID ?? Guid.Empty).ToList();
                if (!dicProfiles.ContainsKey(dicProfile) && itemEvaluatorIds.Any())
                {
                    dicProfiles.Add(dicProfile, itemEvaluatorIds);
                }
            }

            #endregion

            #region Get Profiles by orgs
            List<object> listObj = new List<object>();
            listObj.Add(orgs);
            listObj.Add(string.Empty);
            listObj.Add(string.Empty);
            var profilebyOrgs = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status);
            var profiles = profilebyOrgs.Where(p => profileIds.Contains(p.ID)).ToList();

            //get nguoi quan ly 
            var lstSupervisorIds = profilebyOrgs.Select(p => p.SupervisorID).ToList();
            var lstSupervisor = profilebyOrgs.Where(p => lstSupervisorIds.Contains(p.ID)).ToList();

            #region get orgStructure
            var orgStructures = GetDataNotParam<Cat_OrgStructureEntity>("CAT_SP_GET_ORGSTRUCTUREALL",userLogin, ref status);
            #endregion

            #endregion

            #region Get Eva_PerformanceEvas
            List<object> lstperformanceEvaObjs = new List<object>();
            lstperformanceEvaObjs.Add(1);
            lstperformanceEvaObjs.Add(1000);
            var performanceEvas = GetData<Eva_PerformanceEvaEntity>(lstperformanceEvaObjs, ConstantSql.hrm_eva_sp_get_PerformanceEva, userLogin, ref status);

            #endregion

            #region iterate profiles
            foreach (var profile in profiles)
            {
                var reportEvaluationResult = new Eva_ReportEvaluationResultEntity();
                var profileId = profile.ID;
                var lstEvaluatorIds = new List<Guid>();
                if (dicProfiles.ContainsKey(profileId))
                {
                    lstEvaluatorIds = dicProfiles[profileId];
                }

                #region get performance from-to date and follow templateId,profile
                var performanceObj = performances.Where(p => p.ProfileID == profile.ID
                    && (p.PeriodFromDate <= periodFromDate && periodFromDate <= p.PeriodToDate)
                    && (p.PeriodToDate <= periodToDate && periodToDate <= p.PeriodToDate)
                    && p.PerformanceTemplateID == performanceTemplateId).FirstOrDefault();
                var performanceID = Guid.Empty;
                if (performanceObj != null)
                {
                    performanceID = performanceObj.ID;
                    reportEvaluationResult.Level = performanceObj.LevelName;
                }
                #endregion

                #region Set value for MarkLevel1,MarkLevel2,MarkLevel3

                var performanceEva = performanceEvas.Where(p => lstEvaluatorIds.Contains(p.EvaluatorID ?? Guid.Empty)
                    && p.PerformanceID == performanceID).OrderBy(p => p.OrderEva).ToList();
                var marks = new double?[3];
                //chỉ lấy 3 cấp đánh giá
                for (int i = 0; i < performanceEva.Count && marks.Length >= performanceEva.Count; i++)
                {
                    marks[i] = performanceEva[i].TotalMark.HasValue ? performanceEva[i].TotalMark : null;
                    if (i == 1)
                    {
                        reportEvaluationResult.EvaluationLevel2 = performanceEva[1].ResultNote;
                    }
                }
                reportEvaluationResult.MarkLevel1 = marks[0];
                reportEvaluationResult.MarkLevel2 = marks[1];
                reportEvaluationResult.MarkLevel3 = marks[2];
                #endregion

                #region Lấy phòng ban cha
                var parentOrgStructure = string.Empty;
                if (profile.OrgStructureID.HasValue)
                {
                    parentOrgStructure = GetCodeOrgStructure(orgStructures, profile.OrgStructureID ?? Guid.Empty);
                }
                reportEvaluationResult.Department = parentOrgStructure;
                #endregion

                #region Lấy tổng số tháng làm việc
                if (profile.DateHire.HasValue)
                {
                    reportEvaluationResult.YearOfService = GetTotalMonths(profile.DateHire.Value, DateTime.Now);
                }
                #endregion

                reportEvaluationResult.CodeEmp = profile.CodeEmp;
                reportEvaluationResult.ProfileName = profile.ProfileName;
                if (profile.DateHire.HasValue)
                {
                    reportEvaluationResult.DateHire = profile.DateHire;
                }
                reportEvaluationResult.E_DEPARTMENT = profile.E_DEPARTMENT;
                reportEvaluationResult.E_DIVISION = profile.E_DIVISION;
                reportEvaluationResult.E_SECTION = profile.E_SECTION;
                reportEvaluationResult.E_TEAM = profile.E_TEAM;
                reportEvaluationResult.E_UNIT = profile.E_UNIT;
               // reportEvaluationResult.OrgStructureName = profile.OrgStructureName;
                reportEvaluationResult.CostCentreName = profile.CostCentreName;
                reportEvaluationResult.JobTitle = profile.JobTitleName;
                reportEvaluationResult.PositionName = profile.PositionName;
                //Người quản lý
                reportEvaluationResult.SupervisorID = lstSupervisor.Where(p => p.ID == profile.SupervisorID)
                    .Select(p => p.ProfileName).FirstOrDefault() ?? string.Empty;

                //Thêm vào ds
                reportResult.Add(reportEvaluationResult);
            }
            #endregion
      
            return reportResult;
        }

        /// <summary> Tổng số tháng từ ngày đến ngày </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public int GetTotalMonths(DateTime dtStart, DateTime dtEnd)
        {
            int monthsApart = 12 * (dtStart.Year - dtEnd.Year) + dtStart.Month - dtEnd.Month;
            return Math.Abs(monthsApart);
        }
        
        /// <summary> [Hien.Nguyen] Hàm lấy mã chuỗi phòng ban cha </summary>
        /// <param name="ListOrg">List Phòng Ban</param>
        /// <param name="CurrentID">ID Phòng ban hiện tại</param>
        /// <returns></returns>
        public string GetCodeOrgStructure(List<Cat_OrgStructureEntity> ListOrg, Guid CurrentID)
        {

            string result = string.Empty;
            var model = ListOrg.Where(m => m.ID == CurrentID).FirstOrDefault();
            if (model != null)
            {
                var parent = ListOrg.FirstOrDefault(m => m.ID == model.ParentID);
                if (parent != null)
                {
                    result = parent.OrgStructureName;
                }
            }
            return result;
        }
    }
}
