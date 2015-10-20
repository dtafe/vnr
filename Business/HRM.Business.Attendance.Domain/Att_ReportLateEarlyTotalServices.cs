using HRM.Data.BaseRepository;
using System.Linq;
using HRM.Business.Attendance.Models;
using HRM.Data.Entity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Main.Domain;
using HRM.Business.Category.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Data.Entity;
using System.Threading;
using System.Text.RegularExpressions;
using System.Data;
using HRM.Business.Hr.Domain;

namespace HRM.Business.Attendance.Domain
{
    public class Att_ReportLateEarlyTotalServices : BaseService
    {
        //public List<Att_ReportLateEarlyTotalEntity> GetReportLateEarlyTotal(DateTime From, DateTime To, List<Guid> lstProfileIDs)
        //{
        //    var result = new List<Att_ReportLateEarlyTotalEntity>();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repoProfile = new Hre_ProfileRepository(unitOfWork);
        //        var repoTAMScanReasonMiss=new Cat_TAMScanReasonMissRepository(unitOfWork);
        //        var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
        //        Guid ForgetTams = repoTAMScanReasonMiss.FindBy(x => x.Code.Equals("FC")).Select(x=>x.ID).FirstOrDefault();
        //        var lstWorkdayQuery = repoAtt_Workday.FindBy(m => m.WorkDate >= From && m.WorkDate <= To && (m.LateEarlyDuration > 0 || m.Type == WorkdayType.E_LATE_EARLY.ToString()) && !(m.Status == WorkdayStatus.E_APPROVED.ToString() && m.LateEarlyDuration == 0)).ToList();
        //        var lstWorkdayFogetTamsQuery = repoAtt_Workday.FindBy(m => m.WorkDate >= From && m.WorkDate <= To && m.MissInOutReason == ForgetTams).ToList();
        //       // var listProfile = repoProfile.FindBy(x => lstProfileIDs.Contains(x.ID)).ToList();
        //        if (lstProfileIDs != null && lstProfileIDs.Count != 0)
        //        {
        //            lstWorkdayQuery = lstWorkdayQuery.Where(x => lstProfileIDs.Contains(x.ProfileID)).ToList();
        //            lstWorkdayFogetTamsQuery = lstWorkdayFogetTamsQuery.Where(x => lstProfileIDs.Contains(x.ProfileID)).ToList();
        //        }
        //        var lstWorkday = lstWorkdayQuery.Select(m => new { m.ID, m.ProfileID, m.WorkDate, m.LateEarlyDuration }).ToList();
        //        var lstWorkdayFogetTams = lstWorkdayFogetTamsQuery.Select(m => new { m.ID, m.ProfileID, m.WorkDate }).ToList();
        //    }
        //    return result;
        //}
        
        public DataTable getSchema()
        {
            DataTable tblData = new DataTable("Att_ReportLateEarlyTotalEntity");
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.CodeEmp, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.ProfileName, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.OrgStructureName, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.DepartmentCode, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.SectionCode, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.BranchCode, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.TeamCode, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.DepartmentName, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.SectionName, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.BranchName, typeof(string));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.TeamName, typeof(string));
  
            for (int i = 1; i <= 12; i++)
            {
                tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.Less2Hour+ i, typeof(double));
                tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.Over2Hour + i, typeof(double));
                tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.TotalLateEarly+ i, typeof(double));
                tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.ForgetTamscan + i, typeof(double));
            }
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.DateFrom, typeof(DateTime));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.DateTo, typeof(DateTime));
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.UserExport);
            tblData.Columns.Add(Att_ReportLateEarlyTotalEntity.FieldNames.DateExport, typeof(DateTime));
            return tblData;
        }
        public DataTable GetReportLateEarlyTotal(Att_ReportLateEarlyTotalEntity model, string userLogin)
        {
            DataTable tblData = getSchema();
            var service = new Att_ReportLateEarlyTotalServices();
            var hrService = new Hre_ProfileServices();
            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;
            List<Guid?> OrgIds = new List<Guid?>();
            if (model.SDateFrom != null)
            {
                DateFrom = model.SDateFrom.Value;
            }
            if (model.SDateTo != null)
            {
                DateTo = model.SDateTo.Value;
            }
            var From = DateFrom;
            var To = DateTo;

            string strOrgIDs = null;
            if (!string.IsNullOrEmpty(model.OrgStructureID))
            {
                strOrgIDs = model.OrgStructureID;
            }
            List<object> listObj = new List<object>();
            listObj.Add(strOrgIDs);
            listObj.Add(string.Empty);
            listObj.Add(string.Empty);


            string status = string.Empty;
           
            var result = new DataTable();
            var lstprofile = hrService.GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgStructure, userLogin, ref status).ToList();
            if (lstprofile == null || lstprofile.Count == 0)
            {
            return null;
            }
            List<Guid> lstProfileIDs = lstprofile.Select(x => x.ID).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var repoOrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoOrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoTAMScanReasonMiss = new Cat_TAMScanReasonMissRepository(unitOfWork);
                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                //var lstprofile=repoProfile.FindBy(x=>lstProfileIDs.Contains(x.ID)).ToList();
                var orgs = repoOrgStructure.FindBy(s => s.Code != null).ToList();
                var orgTypes = repoOrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                Guid ForgetTams = repoTAMScanReasonMiss.FindBy(x => x.Code.Equals("FC")).Select(x => x.ID).FirstOrDefault();
                var lstWorkdayQuery = repoAtt_Workday.FindBy(m => m.WorkDate >= From && m.WorkDate <= To && (m.LateEarlyDuration > 0 || m.Type == WorkdayType.E_LATE_EARLY.ToString()) && !(m.Status == WorkdayStatus.E_APPROVED.ToString() && m.LateEarlyDuration == 0)).ToList();
                var lstWorkdayFogetTamsQuery = repoAtt_Workday.FindBy(m => m.WorkDate >= From && m.WorkDate <= To && m.MissInOutReason == ForgetTams).ToList();
                // var listProfile = repoProfile.FindBy(x => lstProfileIDs.Contains(x.ID)).ToList();
                if (lstProfileIDs != null && lstProfileIDs.Count != 0)
                {
                    lstWorkdayQuery = lstWorkdayQuery.Where(x => lstProfileIDs.Contains(x.ProfileID)).ToList();
                    lstWorkdayFogetTamsQuery = lstWorkdayFogetTamsQuery.Where(x => lstProfileIDs.Contains(x.ProfileID)).ToList();
                }
                var lstWorkday = lstWorkdayQuery.Select(m => new { m.ID, m.ProfileID, m.WorkDate, m.LateEarlyDuration }).ToList();
                var lstWorkdayFogetTams = lstWorkdayFogetTamsQuery.Select(m => new { m.ID, m.ProfileID, m.WorkDate }).ToList();
                foreach (var item in lstprofile)
                {
                    DataRow row = tblData.NewRow();
                    row[Att_ReportLateEarlyTotalEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    row[Att_ReportLateEarlyTotalEntity.FieldNames.ProfileName] = item.ProfileName;
                    row[Att_ReportLateEarlyTotalEntity.FieldNames.DateFrom] = From;
                    row[Att_ReportLateEarlyTotalEntity.FieldNames.DateTo] = To;
                    row[Att_ReportLateEarlyTotalEntity.FieldNames.UserExport] = model.UserExport;
                    row[Att_ReportLateEarlyTotalEntity.FieldNames.DateExport] = DateTime.Today;
                    if (item.OrgStructureID.HasValue)
                    {
                        Cat_OrgStructure objOrgStructure = orgs.Where(x => x.ID == item.OrgStructureID).FirstOrDefault();
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.OrgStructureName] = objOrgStructure.OrgStructureName;
                        Guid? orgId = item.OrgStructureID;
                        var orgBrand = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                        var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                        var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                        var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.BranchCode] = orgBrand != null ? orgBrand.Code : string.Empty;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.BranchName] = orgBrand != null ? orgBrand.OrgStructureName : string.Empty;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    }
                    for (DateTime MonthCheck = From; MonthCheck <= To; MonthCheck = MonthCheck.AddMonths(1))
                    {
                        DateTime BeginMonth = new DateTime(MonthCheck.Year, MonthCheck.Month, 1);
                        DateTime EndMonth = BeginMonth.AddMonths(1).AddMinutes(-1);
                        double Less2Hour = lstWorkday.Where(m => m.ProfileID == item.ID && m.WorkDate >= BeginMonth && m.WorkDate <= EndMonth && m.LateEarlyDuration < 2).ToList().Count;
                        double Over2Hour = lstWorkday.Where(m => m.ProfileID == item.ID && m.WorkDate >= BeginMonth && m.WorkDate <= EndMonth && m.LateEarlyDuration >= 2).ToList().Count;
                        double TotalLateEarly = Less2Hour + Over2Hour;
                        double ForgetTamscan = lstWorkdayFogetTams.Where(m => m.ProfileID == item.ID && m.WorkDate >= BeginMonth && m.WorkDate <= EndMonth).ToList().Count;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.Less2Hour + BeginMonth.Month] = Less2Hour;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.Over2Hour + BeginMonth.Month] = Over2Hour;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.TotalLateEarly + BeginMonth.Month] = TotalLateEarly;
                        row[Att_ReportLateEarlyTotalEntity.FieldNames.ForgetTamscan + BeginMonth.Month] = ForgetTamscan;
                    }
                    tblData.Rows.Add(row);
                }

            }
            var configs = new Dictionary<string, Dictionary<string, object>>();
            var confighidden = new Dictionary<string, object>();
            var configCodeEmp = new Dictionary<string, object>();
            configCodeEmp.Add("width", 90);
            var configProfileName = new Dictionary<string, object>();
            configProfileName.Add("width", 125);
            var configOrgStructureName = new Dictionary<string, object>();
            configOrgStructureName.Add("width", 80);
            var configDepartmentCode = new Dictionary<string, object>();
            configDepartmentCode.Add("width", 98);
            var configSectionCode = new Dictionary<string, object>();
            configSectionCode.Add("width", 50);
            var configBranchCode = new Dictionary<string, object>();
            configBranchCode.Add("width", 90);
            var configBranchName = new Dictionary<string, object>();
            configBranchName.Add("width", 77);
            confighidden.Add("hidden", true);
            configs.Add("CodeEmp", configCodeEmp);
            configs.Add("ProfileName", configProfileName);
            configs.Add("OrgStructureName", configOrgStructureName);
            configs.Add("DepartmentCode", configDepartmentCode);
            configs.Add("SectionCode", configSectionCode);
            configs.Add("BranchCode", configBranchCode);
            configs.Add("DepartmentName", configOrgStructureName);
            configs.Add("SectionName", configSectionCode);
            configs.Add("BranchName", configBranchName);
            configs.Add("TeamName", configSectionCode);
            configs.Add("DateFrom", confighidden);
            configs.Add("DateTo", confighidden);
            configs.Add("UserExport", confighidden);
            configs.Add("DateExport", confighidden);
            return tblData.ConfigTable(configs);
        }

    }
}