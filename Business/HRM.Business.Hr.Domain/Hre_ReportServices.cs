using System.Collections.Generic;
using HRM.Business.Hr.Models;
using System;
using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using System.Data;
using VnResource.Helper.Data;
using HRM.Business.Attendance.Models;
using HRM.Business.Insurance.Models;
using VnResource.Helper.Linq;
using HRM.Business.Payroll.Models;
using HRM.Business.Canteen.Models;
namespace HRM.Business.Hr.Domain
{
    public class Hre_ReportServices : BaseService
    {
        #region BC CV nặng nhọc

        public List<Hre_ReportHDTJobEntity> GetReportHDTJob(DateTime? DateFrom, DateTime? DateTo, string lstOrgOrderNumber, string userLogin)
        {
            string status = string.Empty;
            List<Hre_ReportHDTJobEntity> lstReportHDTJobEntity = new List<Hre_ReportHDTJobEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var hdtJobServices = new Hre_HDTJobServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(lstOrgOrderNumber);
                listObjHDTJob.Add(DateFrom);
                listObjHDTJob.Add(DateTo);
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_RptHDTJob, userLogin, ref status).ToList();

                if (lstHDTJob == null)
                {
                    return lstReportHDTJobEntity;
                }
                var lstProfileByHDTJob = lstHDTJob.Select(s => s.ProfileID).ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var lstProfile = repoProfile.FindBy(s => lstProfileByHDTJob.Contains(s.ID)).ToList();
                List<Guid> profileid = lstProfile.Select(m => m.ID).ToList();
                var repoHDTJobType = new Cat_HDTJobTypeRepository(unitOfWork);
                var lstHDTJobType = repoHDTJobType.GetAll().ToList();
                foreach (var ProfileID in profileid)
                {
                    if (ProfileID == null)
                        continue;
                    var lstHDTJobbyProfile = lstHDTJob.Where(m => m.ProfileID == ProfileID).ToList();
                    foreach (var HDTJobbyProfile in lstHDTJobbyProfile)
                    {
                        Hre_ReportHDTJobEntity reportHDTJobEntity = new Hre_ReportHDTJobEntity();
                        var profile = lstProfile.Where(m => m.ID == ProfileID).Select(m => new { m.ProfileName, m.CodeEmp, m.PositionID, m.JobTitleID }).FirstOrDefault();
                        var HDTJobType = lstHDTJobType.Where(s => HDTJobbyProfile.HDTJobTypeID == s.ID).FirstOrDefault();
                        reportHDTJobEntity.ProfileName = profile.ProfileName;
                        reportHDTJobEntity.CodeEmp = profile.CodeEmp;
                        reportHDTJobEntity.HDTType = HDTJobType != null ? HDTJobType.HDTJobTypeName : null;
                        reportHDTJobEntity.DateFrom = HDTJobbyProfile.DateFrom;
                        reportHDTJobEntity.DateTo = HDTJobbyProfile.DateTo;
                        lstReportHDTJobEntity.Add(reportHDTJobEntity);
                    }
                }
            }
            return lstReportHDTJobEntity;
        }
        #endregion

        #region BC Mã thẻ chưa có trong hệ thống
        public List<Hre_ReportCodeNotInSystemEntity> GetReportCodeNotInSystem(DateTime? DateFrom, DateTime? DateTo)
        {
            List<Hre_ReportCodeNotInSystemEntity> lstReportCodeNotInSystem = new List<Hre_ReportCodeNotInSystemEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_ProfileRepository(unitOfWork);
                var codeAttendance = repo.FindBy(s => s.IsDelete == null).Select(m => m.CodeAttendance).Distinct().ToList();
                var repoTAMScanLog = new Att_TAMScanLogRepository(unitOfWork);
                var lstCardHistory = repoTAMScanLog.FindBy(m => m.TimeLog >= DateFrom && m.TimeLog <= DateTo && !codeAttendance.Contains(m.CardCode))
                .Select(m => new { m.CardCode, m.TimeLog, m.MachineNo, m.Type, m.SrcType, m.DateCreate, m.UserCreate }).ToList();

                foreach (var CardHistory in lstCardHistory)
                {
                    Hre_ReportCodeNotInSystemEntity codeNotInSystemEntity = new Hre_ReportCodeNotInSystemEntity();
                    codeNotInSystemEntity.Code = CardHistory.CardCode;
                    codeNotInSystemEntity.Type = CardHistory.Type;
                    codeNotInSystemEntity.Time = CardHistory.TimeLog;
                    codeNotInSystemEntity.Machine = CardHistory.MachineNo;
                    codeNotInSystemEntity.UserCreate = CardHistory.UserCreate;
                    codeNotInSystemEntity.DateCreate = CardHistory.DateCreate;
                    lstReportCodeNotInSystem.Add(codeNotInSystemEntity);
                }
                return lstReportCodeNotInSystem;
            }
        }
        #endregion

        #region DS HD đến hạn
        public List<Hre_ReportExpiryContractEntity> GetReportExpiryContract(DateTime? DateFrom, DateTime? DateTo, List<Guid> lstProfileIDs)
        {
            List<Hre_ReportExpiryContractEntity> lstReportExpiryContract = new List<Hre_ReportExpiryContractEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoContract = new Hre_ContractRepository(unitOfWork);
                var lstContract = repoContract.FindBy(s => s.DateEnd != null && s.DateEnd >= DateFrom && s.DateEnd <= DateTo && lstProfileIDs.Contains(s.ProfileID)).ToList();

                if (lstContract == null)
                {
                    return lstReportExpiryContract;
                }

                List<Guid> lstProfileIDsbycontract = lstContract.Select(s => s.ProfileID).ToList();
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var lstAllprofile = repoProfile.FindBy(s => lstProfileIDsbycontract.Contains(s.ID)).Select(s => new { s.ID, s.ProfileName, s.CodeEmp, s.OrgStructureID, s.PositionID, s.JobTitleID }).ToList();

                var repoOrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var lstOrgStructure = repoOrgStructure.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.OrgStructureName }).ToList();

                var repoPosition = new Cat_PositionRepository(unitOfWork);
                var lstPosition = repoPosition.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.PositionName }).ToList();

                var repoJobtitle = new Cat_JobTitleRepository(unitOfWork);
                var lstJobtitle = repoJobtitle.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.JobTitleName }).ToList();

                var repoContractType = new Cat_ContractTypeRepository(unitOfWork);
                var lstContractType = repoContractType.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.ContractTypeName, s.ContractNextID }).ToList();

                foreach (var contract in lstContract)
                {
                    Hre_ReportExpiryContractEntity ReportExpiryContract = new Hre_ReportExpiryContractEntity();
                    var profileByContract = lstAllprofile.Where(s => contract.ProfileID == s.ID).FirstOrDefault();
                    if (profileByContract == null)
                        continue;

                    var orgByProfile = lstOrgStructure.Where(s => profileByContract.OrgStructureID == s.ID).FirstOrDefault();
                    var positionByProfile = lstPosition.Where(s => contract.PositionID == s.ID).FirstOrDefault();
                    var jobtitleByProfile = lstJobtitle.Where(s => contract.JobTitleID == s.ID).FirstOrDefault();
                    var contractTypeByContract = lstContractType.Where(s => s.ID == contract.ContractTypeID).FirstOrDefault();

                    ReportExpiryContract.CodeEmp = profileByContract.CodeEmp;
                    ReportExpiryContract.ProfileName = profileByContract.ProfileName;
                    ReportExpiryContract.OrgStructureName = orgByProfile != null ? orgByProfile.OrgStructureName : "";
                    ReportExpiryContract.JobTitleName = jobtitleByProfile != null ? jobtitleByProfile.JobTitleName : "";
                    ReportExpiryContract.PositionName = positionByProfile != null ? positionByProfile.PositionName : "";
                    ReportExpiryContract.ContractTypeName = contractTypeByContract != null ? contractTypeByContract.ContractTypeName : "";
                    ReportExpiryContract.DateSigned = contract.DateSigned;
                    ReportExpiryContract.DateStart = contract.DateStart;
                    ReportExpiryContract.DateEnd = contract.DateEnd;
                    lstReportExpiryContract.Add(ReportExpiryContract);
                }
                return lstReportExpiryContract;
            }
        }
        #endregion

        #region Báo cáo Sinh nhật
        public List<Hre_ReportBirthdayEntity> GetReportBirthday(DateTime? DateFrom, DateTime? DateTo, List<Guid> lstProfileIDs)
        {
            List<Hre_ReportBirthdayEntity> lstReportBirthday = new List<Hre_ReportBirthdayEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var lstProfile = repoProfile.FindBy(s => s.DateOfBirth != null && s.IsDelete == null && s.DateOfBirth >= DateFrom && s.DateOfBirth <= DateTo && lstProfileIDs.Contains(s.ID)).ToList();

                if (lstProfile == null)
                {
                    return lstReportBirthday;
                }

                List<Guid> lstProfileIDsbyBirthday = lstProfile.Select(s => s.ID).ToList();
                //var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var lstAllprofile = repoProfile.FindBy(s => lstProfileIDsbyBirthday.Contains(s.ID)).Select(s => new { s.ID, s.ProfileName, s.CodeEmp, s.OrgStructureID, s.PositionID, s.JobTitleID, s.DateHire, s.DateOfBirth, s.PlaceOfBirth }).ToList();

                var repoOrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var lstOrgStructure = repoOrgStructure.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.OrgStructureName }).ToList();

                var repoPosition = new Cat_PositionRepository(unitOfWork);
                var lstPosition = repoPosition.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.PositionName }).ToList();

                var repoJobtitle = new Cat_JobTitleRepository(unitOfWork);
                var lstJobtitle = repoJobtitle.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.JobTitleName }).ToList();


                foreach (var ProfileBirthday in lstProfile)
                {
                    Hre_ReportBirthdayEntity ReportBirthday = new Hre_ReportBirthdayEntity();
                    //var profileByContract = lstAllprofile.Where(s => contract.ProfileID == s.ID).FirstOrDefault();
                    //if (profileByContract == null)
                    //    continue;

                    var orgByProfile = lstOrgStructure.Where(s => ProfileBirthday.OrgStructureID == s.ID).FirstOrDefault();
                    var positionByProfile = lstPosition.Where(s => ProfileBirthday.PositionID == s.ID).FirstOrDefault();
                    var jobtitleByProfile = lstJobtitle.Where(s => ProfileBirthday.JobTitleID == s.ID).FirstOrDefault();

                    ReportBirthday.CodeEmp = ProfileBirthday.CodeEmp;
                    ReportBirthday.ProfileName = ProfileBirthday.ProfileName;
                    ReportBirthday.ProfileID = ProfileBirthday.ID;
                    ReportBirthday.OrgStructureName = orgByProfile != null ? orgByProfile.OrgStructureName : "";
                    ReportBirthday.JobTitleName = jobtitleByProfile != null ? jobtitleByProfile.JobTitleName : "";
                    ReportBirthday.PositionName = positionByProfile != null ? positionByProfile.PositionName : "";
                    ReportBirthday.DateTo = ProfileBirthday.DateTo;
                    ReportBirthday.DateFrom = ProfileBirthday.DateFrom;
                    ReportBirthday.DateHire = ProfileBirthday.DateHire;
                    ReportBirthday.DateOfBirth = ProfileBirthday.DateOfBirth;
                    ReportBirthday.PlaceOfBirth = ProfileBirthday.PlaceOfBirth;
                    lstReportBirthday.Add(ReportBirthday);
                }
                return lstReportBirthday;
            }
        }
        #endregion

        #region Báo cáo Thâm niên
        public List<Hre_ReportSeniorityEntity> GetReportSeniority(DateTime? DateSeniority, List<Hre_ProfileEntity> lstProfiles)
        {
            List<Hre_ReportSeniorityEntity> lstReportSeniority = new List<Hre_ReportSeniorityEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                lstProfiles = lstProfiles.Where(s => s.DateHire != null && s.IsDelete == null && s.DateHire <= DateSeniority).ToList();

                if (lstProfiles.Count == 0)
                {
                    return lstReportSeniority;
                }

                var lstprofileids = lstProfiles.Select(s => s.ID).Distinct().ToList();

                #region Lấy Sal_BasicSalary
                var repoBasicSalary = new Sal_BasicSalaryRepository(unitOfWork);
                var lstBasicSalary = new List<Sal_BasicSalary>().Select(s => new
                {
                    s.ProfileID,
                    s.ID,
                    s.GrossAmount,
                    s.DateOfEffect,
                    s.CurrencyID1,
                    s.CurrencyID2,
                    s.CurrencyID3,
                    s.CurrencyID4,
                    s.CurrencyID5,
                    s.CurrencyID6
                }).ToList();

                foreach (var item in lstprofileids.Chunk(1000))
                {
                    lstBasicSalary.AddRange(repoBasicSalary.FindBy(s => s.IsDelete == null && item.Contains(s.ProfileID)).Select(s => new
                        {
                            s.ProfileID,
                            s.ID,
                            s.GrossAmount,
                            s.DateOfEffect,
                            s.CurrencyID1,
                            s.CurrencyID2,
                            s.CurrencyID3,
                            s.CurrencyID4,
                            s.CurrencyID5,
                            s.CurrencyID6
                        }).ToList());
                }
                #endregion

                #region Lấy Sal_BasicSalary
                var ContractRepository = new Hre_ContractRepository(unitOfWork);
                var lstContract = new List<Hre_Contract>().Select(s => new
                {
                    s.ID,
                    s.ContractTypeID,
                    ContractTypeName = s.Cat_ContractType.ContractTypeName,
                    s.ProfileID,
                    s.DateStart,
                    s.DateEnd
                }).ToList();

                foreach (var item in lstprofileids.Chunk(1000))
                {
                    lstContract.AddRange(ContractRepository.FindBy(s => s.IsDelete == null && item.Contains(s.ProfileID)).Select(s => new
                    {
                        s.ID,
                        s.ContractTypeID,
                        ContractTypeName = s.Cat_ContractType.ContractTypeName,
                        s.ProfileID,
                        s.DateStart,
                        s.DateEnd
                    }).ToList());
                }
                #endregion

                #region Lấy Att_AnnualDetail
                var AnnualLeaveDetailRepository = new Att_AnnualDetailRepository(unitOfWork);
                var listAnnualDetail = new List<Att_AnnualDetail>().ToList();

                foreach (var item in lstprofileids.Chunk(1000))
                {
                    listAnnualDetail.AddRange(AnnualLeaveDetailRepository.FindBy(s => s.IsDelete == null && item.Contains(s.ProfileID.Value)).ToList());
                }
                #endregion

                foreach (var profileid in lstprofileids)
                {
                    var Profile = lstProfiles.Where(s => s.ID == profileid).FirstOrDefault();
                    if (Profile != null)
                    {
                        Hre_ReportSeniorityEntity ReportSeniority = new Hre_ReportSeniorityEntity();
                        var contractByProfile = lstContract.Where(s => s.ProfileID != null && Profile.ID == s.ProfileID).FirstOrDefault();
                        var basicsalaryByProfile = lstBasicSalary.Where(x => x.ProfileID != null && x.ProfileID == Profile.ID).FirstOrDefault();
                        ReportSeniority.CodeEmp = Profile.CodeEmp;
                        ReportSeniority.ProfileName = Profile.ProfileName;
                        ReportSeniority.DateHire = Profile.DateHire;
                        ReportSeniority.E_DEPARTMENT = Profile.E_DEPARTMENT;
                        ReportSeniority.E_SECTION = Profile.E_SECTION;
                        int Year = 0;
                        int Month = 0;
                        if (ReportSeniority.DateHire.HasValue)
                        {
                            if (ReportSeniority.DateHire.Value.Month < DateSeniority.Value.Month)
                            {
                                Month = DateSeniority.Value.Month - 1 - ReportSeniority.DateHire.Value.Month;
                                if (DateSeniority.Value.Year > ReportSeniority.DateHire.Value.Year)
                                    Year = DateSeniority.Value.Year - ReportSeniority.DateHire.Value.Year;
                            }
                            else
                            {
                                Month = (DateSeniority.Value.Month - 1 - ReportSeniority.DateHire.Value.Month) + 12;
                                if (DateSeniority.Value.Year > ReportSeniority.DateHire.Value.Year)
                                    Year = DateSeniority.Value.Year - 1 - ReportSeniority.DateHire.Value.Year;
                            }
                        }
                        ReportSeniority.YearSeniority = Year;
                        ReportSeniority.MonthSeniority = Month;
                        ReportSeniority.JobTitleName = Profile.JobTitleName;
                        ReportSeniority.PositionName = Profile.PositionName;
                        if (contractByProfile != null)
                        {
                            ReportSeniority.ContractTypeName = contractByProfile.ContractTypeName;
                            ReportSeniority.DateStart = contractByProfile.DateStart;
                            ReportSeniority.DateStart = contractByProfile.DateEnd;
                        }
                        if (basicsalaryByProfile != null)
                        {
                            ReportSeniority.GrossAmount = !string.IsNullOrEmpty(basicsalaryByProfile.GrossAmount) ? basicsalaryByProfile.GrossAmount : "0.00";
                        }
                        ReportSeniority.DateOfEffect = Profile.DateOfEffect;
                        ReportSeniority.Notes = Profile.Notes;
                        Att_AnnualDetail AnnualDetailByProfile = listAnnualDetail.Where(m => m.ProfileID == Profile.ID && m.Type == AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString()).FirstOrDefault();
                        if (AnnualDetailByProfile != null)
                        {
                            ReportSeniority.AnnualYearRest = AnnualDetailByProfile.Remain;
                        }
                        lstReportSeniority.Add(ReportSeniority);
                    }
                }
                return lstReportSeniority;
            }
        }

        #endregion

        #region BC NV đang làm việc

        public List<Hre_ReportProfileWorkingEntity> GetReportProfileWorking(DateTime? DateFrom, DateTime? DateTo, string lstOrgOrderNumber, string codeEmp, string userLogin)
        {
            string status = string.Empty;
            List<Hre_ReportProfileWorkingEntity> lstReportProfileWorkingEntity = new List<Hre_ReportProfileWorkingEntity>();

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var orgsService = new Cat_OrgStructureServices();
                var orgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();

                var empoyeeTypesServices = new Cat_EmployeeTypeServices();
                var empoyeeTypes = empoyeeTypesServices.GetDataNotParam<Cat_EmployeeTypeEntity>(ConstantSql.hrm_cat_sp_get_AllEmpType, userLogin, ref status).ToList();

                var postionsServices = new Cat_PositionServices();
                var postions = postionsServices.GetDataNotParam<Cat_PositionEntity>(ConstantSql.hrm_cat_sp_get_AllPosition, userLogin, ref status).ToList();

                var orgTypesServices = new Cat_OrgStructureTypeServices();
                var orgTypes = orgTypesServices.GetDataNotParam<Cat_OrgStructureType>(ConstantSql.hrm_cat_sp_get_AllOrgType, userLogin, ref status).ToList();

                var hrService = new Hre_ProfileServices();
                List<object> listObjHr = new List<object>();
                listObjHr.Add(lstOrgOrderNumber);
                listObjHr.Add(DateFrom);
                listObjHr.Add(DateTo);
                listObjHr.Add(codeEmp);
                var profiles = hrService.GetData<Hre_ProfileEntity>(listObjHr, ConstantSql.hrm_hr_sp_get_RptWorkingProfile, userLogin,ref status).ToList();

                foreach (var profile in profiles)
                {
                    Hre_ReportProfileWorkingEntity ReportProfileWorkingEntity = new Hre_ReportProfileWorkingEntity();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    ReportProfileWorkingEntity.CodeBranch = orgBranch != null ? orgBranch.Code : string.Empty;
                    ReportProfileWorkingEntity.CodeOrg = orgOrg != null ? orgOrg.Code : string.Empty;
                    ReportProfileWorkingEntity.CodeTeam = orgTeam != null ? orgTeam.Code : string.Empty;
                    ReportProfileWorkingEntity.CodeSection = orgSection != null ? orgSection.Code : string.Empty;
                    ReportProfileWorkingEntity.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    ReportProfileWorkingEntity.OrgName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    ReportProfileWorkingEntity.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    ReportProfileWorkingEntity.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    ReportProfileWorkingEntity.CodeEmp = profile.CodeEmp;
                    ReportProfileWorkingEntity.ProfileName = profile.ProfileName;
                    ReportProfileWorkingEntity.IDNo = profile.IDNo;
                    ReportProfileWorkingEntity.Gender = profile.Gender;
                    ReportProfileWorkingEntity.CodeAttendance = profile.CodeAttendance;
                    ReportProfileWorkingEntity.PAStreet = profile.PAddress;
                    ReportProfileWorkingEntity.DateHire = profile.DateHire;
                    var employeeType = empoyeeTypes.Where(s => profile.EmpTypeID == s.ID).FirstOrDefault();
                    if (employeeType != null)
                    {
                        ReportProfileWorkingEntity.EmployeeTypeName = employeeType.EmployeeTypeName;
                    }
                    var position = postions.Where(s => profile.PositionID == s.ID).FirstOrDefault();
                    if (position != null)
                    {
                        ReportProfileWorkingEntity.PositionName = position.PositionName;
                    }
                    lstReportProfileWorkingEntity.Add(ReportProfileWorkingEntity);
                }
                return lstReportProfileWorkingEntity;
            }
        }
        #endregion

        #region BC NV Thử Việc - Tìm Theo Ngày

        public List<Hre_ReportProfileProbationEntity> GetReportProfileProbation(DateTime? DateFrom, DateTime? DateToSearch, List<Guid> lstProfileIDs)
        {
            List<Hre_ReportProfileProbationEntity> lstReportProfileProbation = new List<Hre_ReportProfileProbationEntity>();
            DateTime DateTo = DateToSearch.Value.AddDays(1).AddMinutes(-1);
            if (lstProfileIDs == null)
            {
                return lstReportProfileProbation;
            }
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoorgs = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoorgs.FindBy(s => s.IsDelete == null && s.Code != null).ToList();


                var repopostions = new Cat_PositionRepository(unitOfWork);
                var postions = repopostions.FindBy(s => s.IsDelete == null && s.PositionName != null).ToList();

                var repojobtitles = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repojobtitles.FindBy(s => s.IsDelete == null && s.JobTitleName != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var repoWorkPlace = new Cat_WorkPlaceRepository(unitOfWork);
                var workPlace = repoWorkPlace.FindBy(s => s.IsDelete == null && s.WorkPlaceName != null).ToList();


                var repoprofiles = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoprofiles.FindBy(s => s.IsDelete == null && s.DateEndProbation >= DateFrom && s.DateEndProbation <= DateTo && lstProfileIDs.Contains(s.ID))
             .Select(s => new
             {
                 s.ID,
                 s.DateQuit,
                 s.OrgStructureID,
                 s.ProfileName,
                 s.CodeEmp,
                 s.PositionID,
                 s.JobTitleID,
                 s.IDNo,
                 s.CodeAttendance,
                 s.DateEndProbation,
                 s.ProbExtendDate,
                 s.IDDateOfIssue,
                 s.IDPlaceOfIssue,
                 s.DateHire,
                 s.WorkPlaceID,
                 s.PAStreet,
                 s.EmpTypeID,
                 s.Gender

             }).ToList();
                foreach (var profile in profiles)
                {
                    Hre_ReportProfileProbationEntity ReportProfileProbation = new Hre_ReportProfileProbationEntity();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var jobtitlebypro = jobtitles.Where(s => s.ID == profile.JobTitleID).FirstOrDefault();
                    var positiobbypro = postions.Where(s => s.ID == profile.PositionID).FirstOrDefault();
                    var workplacebypro = workPlace.Where(s => s.ID == profile.WorkPlaceID).FirstOrDefault();
                    ReportProfileProbation.CodeEmp = profile.CodeEmp;
                    ReportProfileProbation.ProfileName = profile.ProfileName;
                    ReportProfileProbation.CodeOrg = org != null ? org.Code : null;
                    ReportProfileProbation.OrgStructureName = org != null ? org.OrgStructureName : null;
                    ReportProfileProbation.JobTitleName = jobtitlebypro != null ? jobtitlebypro.JobTitleName : null;
                    ReportProfileProbation.PositionName = positiobbypro != null ? positiobbypro.PositionName : null;
                    ReportProfileProbation.DateHire = profile.DateHire;
                    ReportProfileProbation.DateEndProbation = profile.DateEndProbation;
                    ReportProfileProbation.ProbExtendDate = profile.ProbExtendDate;
                    ReportProfileProbation.IDNo = profile.IDNo;
                    ReportProfileProbation.IDDateOfIssue = profile.IDDateOfIssue;
                    ReportProfileProbation.IDPlaceOfIssue = profile.IDPlaceOfIssue;
                    ReportProfileProbation.WorkPlace = workplacebypro != null ? workplacebypro.WorkPlaceName : null;
                    lstReportProfileProbation.Add(ReportProfileProbation);
                }
                return lstReportProfileProbation;
            }
        }
        #endregion

        #region BC NV Nghỉ Việc - Tìm Theo Ngày

        public List<Hre_ReportProfileQuitEntity> GetReportProfileQuit(DateTime? DateFrom, DateTime? DateToSearch, List<Guid> lstProfileIDs)
        {
            List<Hre_ReportProfileQuitEntity> lstReportProfileQuit = new List<Hre_ReportProfileQuitEntity>();
            DateTime DateTo = DateToSearch.Value.AddDays(1).AddMinutes(-1);
            if (lstProfileIDs == null)
            {
                return lstReportProfileQuit;
            }
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoorgs = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoorgs.FindBy(s => s.IsDelete == null && s.Code != null).ToList();


                var repopostions = new Cat_PositionRepository(unitOfWork);
                var postions = repopostions.FindBy(s => s.IsDelete == null && s.PositionName != null).ToList();

                var repojobtitles = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repojobtitles.FindBy(s => s.IsDelete == null && s.JobTitleName != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var reporesreason = new Cat_ResignReasonRepository(unitOfWork);
                var lstResReason = reporesreason.FindBy(s => s.IsDelete == null && s.ResignReasonName != null).ToList();


                var repoprofiles = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoprofiles.FindBy(s => s.IsDelete == null && s.DateQuit >= DateFrom && s.DateQuit <= DateTo && lstProfileIDs.Contains(s.ID))
             .Select(s => new
             {
                 s.ID,
                 s.DateQuit,
                 s.OrgStructureID,
                 s.ProfileName,
                 s.CodeEmp,
                 s.PositionID,
                 s.JobTitleID,
                 s.IDNo,
                 s.CodeAttendance,
                 s.DateEndProbation,
                 s.ProbExtendDate,
                 s.IDDateOfIssue,
                 s.IDPlaceOfIssue,
                 s.DateHire,
                 s.WorkPlaceID,
                 s.PAStreet,
                 s.EmpTypeID,
                 s.RequestDate,
                 s.ResReasonID,
                 s.Gender

             }).ToList();
                foreach (var profile in profiles)
                {
                    Hre_ReportProfileQuitEntity ReportProfileQuit = new Hre_ReportProfileQuitEntity();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var jobtitlebypro = jobtitles.Where(s => s.ID == profile.JobTitleID).FirstOrDefault();
                    var positiobbypro = postions.Where(s => s.ID == profile.PositionID).FirstOrDefault();
                    var resReasonbypro = lstResReason.Where(s => s.ID == profile.ResReasonID).FirstOrDefault();

                    ReportProfileQuit.CodeEmp = profile.CodeEmp;
                    ReportProfileQuit.ProfileName = profile.ProfileName;
                    ReportProfileQuit.CodeOrg = org != null ? org.Code : null;
                    ReportProfileQuit.OrgStructureName = org != null ? org.OrgStructureName : null;
                    ReportProfileQuit.JobTitleName = jobtitlebypro != null ? jobtitlebypro.JobTitleName : null;
                    ReportProfileQuit.PositionName = positiobbypro != null ? positiobbypro.PositionName : null;
                    ReportProfileQuit.DateHire = profile.DateHire;
                    ReportProfileQuit.RequestDate = profile.RequestDate;
                    ReportProfileQuit.DateQuit = profile.DateQuit;
                    ReportProfileQuit.ResignReasonName = resReasonbypro != null ? resReasonbypro.ResignReasonName : null;
                    lstReportProfileQuit.Add(ReportProfileQuit);
                }
                return lstReportProfileQuit;
            }
        }
        #endregion

        #region BC TK Trình Độ Học Vấn
        public List<Hre_ReportEducationChartListEntity> GetReportEducationCharList(DateTime? DateFrom, DateTime? DateTo, List<Guid> lstProfileIDs, bool AppliedForThisPeriod)
        {
            List<Hre_ReportEducationChartListEntity> lstEducationChar = new List<Hre_ReportEducationChartListEntity>();
            if (lstProfileIDs == null)
            {
                return lstEducationChar;
            }
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoOrgs = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoOrgs.FindBy(s => s.IsDelete == null && s.Code != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var type = Education.E_EDUCATION_LEVEL.ToString();
                var repoCat_NameEntity = new Cat_NameEntityRepository(unitOfWork);
                var lstNameEntity = repoCat_NameEntity.FindBy(s => s.NameEntityType == type && s.IsDelete == null).ToList();

                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var lstProfile = repoProfile.FindBy(s => s.IsDelete == null).ToList();

                if (AppliedForThisPeriod)
                {
                    lstProfile = lstProfile.Where(s => s.DateHire <= DateTo && (s.DateQuit == null || (s.DateQuit >= DateFrom && s.DateQuit <= DateTo))).ToList();
                }

                lstProfile = lstProfile.Where(s => s.DateHire <= DateTo && (s.DateQuit == null || s.DateQuit >= DateFrom) && lstProfileIDs.Contains(s.ID)).ToList();

                foreach (var nameEntity in lstNameEntity)
                {
                    Hre_ReportEducationChartListEntity EducationCharList = new Hre_ReportEducationChartListEntity();
                    int countprofile = lstProfile.Count(p => p.EducationLevelID == nameEntity.ID);
                    if (countprofile == 0) continue;
                    EducationCharList.EducationLevel = nameEntity.NameEntityName;
                    EducationCharList.ProfileCount = countprofile;
                    lstEducationChar.Add(EducationCharList);
                }
                return lstEducationChar;
            }

        }
        #endregion

        #region BC Headcount thâm niên


        DataTable CreateReportHCSenioritySchema(Guid orgID, Guid? orgTypeID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable datatable = new DataTable("Hre_ReportHCSeniorityEntity");
                DataRow dr = datatable.NewRow();
                datatable.Columns.Add(Hre_ReportHCSeniorityEntity.FieldNames.OrgStructureName);
                datatable.Columns.Add(Hre_ReportHCSeniorityEntity.FieldNames.Type);
                datatable.Columns.Add(Hre_ReportHCSeniorityEntity.FieldNames.HeadCount);
                var orgsService = new Cat_OrgStructureServices();
                var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();
                var lstorgs = lstallorgs.Where(s => s.ParentID == orgID).ToList();

                //if (orgTypeID != null && orgTypeID != Guid.Empty)
                //{
                //    lstorgs = lstorgs.Where(s => s.OrgStructureTypeID != null && s.OrgStructureTypeID == orgTypeID).ToList();
                //}

                int count = 0;
                foreach (var item in lstorgs)
                {
                    orderNumber = string.Empty;
                    orderNumber += item.OrderNumber.ToString() + ",";
                    getChildOrgStructure(lstallorgs, item.ID);

                    if (orderNumber.IndexOf(',') > 0)
                        orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                    List<Cat_OrgStructure> lstOrgByOrderNumberCount = new List<Cat_OrgStructure>();
                    var lstObjOrgByOrderNumberCount = new List<object>();
                    lstObjOrgByOrderNumberCount.Add(orderNumber);
                    if (orderNumber != string.Empty)
                    {
                        lstOrgByOrderNumberCount = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumberCount, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin,ref status).OrderBy(s => s.OrgStructureName).ToList();

                    }
                    if (orgTypeID != null)
                    {
                        lstOrgByOrderNumberCount = lstOrgByOrderNumberCount.Where(s => s.OrgStructureTypeID == orgTypeID).ToList();
                    }
                    foreach (var org in lstOrgByOrderNumberCount)
                    {
                        if (!string.IsNullOrEmpty(org.OrgStructureName))
                        {
                            //xử lý tạo thêm 1 dòng để chứa những tên phòng ban trùng tên
                            if (datatable.Columns.Contains(org.OrgStructureName))
                            {
                                dr[0] += org.OrgStructureName + ",";
                                count++;
                                datatable.Columns.Add(org.OrgStructureName, typeof(int));
                            }
                            else
                            {
                                //datatable.Columns.Add(org.OrgStructureName);
                                datatable.Columns.Add(org.OrgStructureName);
                            }
                        }
                    }

                }
                //datatable.Columns.Add(Hre_ReportHCSeniorityEntity.FieldNames.DateExport);
                //datatable.Columns.Add(Hre_ReportHCSeniorityEntity.FieldNames.UserExport);
                datatable.Columns.Add(Hre_ReportHCSeniorityEntity.FieldNames.Total);
                dr[1] = count;
                datatable.Rows.Add(dr);
                return datatable;
            }
        }

        public DataTable GetReportHCSeniority(DateTime dateSearch, List<Guid> lstjobTitles, Guid orgID, Guid? orgTypeId, bool isIncludeQuitEmp, bool isCreateTemplate, string userLogin)
        {
            List<string> lstSeniority = new List<string> { ">12 Tháng", "9-12 Tháng", "6-9 Tháng", "3-6 Tháng", "0-2 Tháng" };
            DataTable table = CreateReportHCSenioritySchema(orgID, orgTypeId, userLogin);
            if (isCreateTemplate)
            {
                return table.ConfigTable();
            }
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var serviceProfile = new Hre_ProfileServices();
                var baseService = new BaseService();
                var lstObjProfileIDs = new List<object>();
                DataRow row1 = table.NewRow();

                var jobtitleServices = new Cat_JobTitleServices();
                var lstJobtitle = jobtitleServices.GetData<Cat_JobTitleEntity>(lstjobTitles[0], ConstantSql.hrm_cat_sp_get_JobTitleById, userLogin,ref status).FirstOrDefault();

                var orgsService = new Cat_OrgStructureServices();
                var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();

                var lstorgs = lstallorgs.Where(s => s.ParentID == orgID).ToList();
                var lstOrgName = lstallorgs.Where(s => s.ID == orgID).FirstOrDefault();

                var listorgid = lstorgs.Select(s => new { s.ID, s.OrderNumber, s.Code, s.OrgStructureName }).ToList();

                //Xử Lý lấy tất cả nhân viên trong phòng ban đã chọn và group 1 cấp
                var orgIDs = string.Empty;
                orderNumber = string.Empty;
                foreach (var item in listorgid)
                {
                    orderNumber += item.OrderNumber.ToString() + ",";
                    getChildOrgStructure(lstallorgs, item.ID);
                }
                if (orderNumber.IndexOf(',') > 0)
                    orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                var lstObjOrgByOrderNumber = new List<object>();
                lstObjOrgByOrderNumber.Add(orderNumber);
                var lstOrgByOrderNumber = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin,ref status).Select(s => s.ID).ToList();

                List<object> listObj = new List<object>();
                listObj.Add(orderNumber);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);

                var lstprofile = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin,ref status).ToList();

                if (!isIncludeQuitEmp)
                {
                    lstprofile = lstprofile.Where(s => s.DateQuit == null).ToList();
                }
                if (lstjobTitles != null && lstjobTitles[0] != null && lstjobTitles[0] != Guid.Empty)
                {
                    lstprofile = lstprofile.Where(s => s.JobTitleID != null && lstjobTitles.Contains(s.JobTitleID.Value)).ToList();
                }
                if (lstprofile == null || lstorgs == null)
                {
                    return table;
                }
                if (lstJobtitle != null)
                {
                    row1[Hre_ReportHCSeniorityEntity.FieldNames.OrgStructureName] = lstOrgName == null ? string.Empty : lstOrgName.OrgStructureName + " - " + lstJobtitle.JobTitleName;
                }
                else
                {
                    row1[Hre_ReportHCSeniorityEntity.FieldNames.OrgStructureName] = lstOrgName == null ? string.Empty : lstOrgName.OrgStructureName;

                }

                table.Rows.Add(row1);

                foreach (var item in lstSeniority)
                {
                    DataRow row = table.NewRow();
                    row[Hre_ReportHCSeniorityEntity.FieldNames.Type] = item;
                    var IDsCount = string.Empty;
                    var count = 0;
                    int totalcount = 0;
                    foreach (var org in lstorgs)
                    {

                        //xử lý đến nhân viên của phòng ban con
                        orderNumber = string.Empty;
                        orderNumber += org.OrderNumber.ToString() + ",";
                        getChildOrgStructure(lstallorgs, org.ID);

                        if (orderNumber.IndexOf(',') > 0)
                            orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                        var lstObjOrgByOrderNumberCount = new List<object>();
                        lstObjOrgByOrderNumberCount.Add(orderNumber);
                        var lstOrgByOrderNumberCount = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumberCount, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin,ref status).ToList();
                        if (orgTypeId != null)
                        {
                            lstOrgByOrderNumberCount = lstOrgByOrderNumberCount.Where(s => s.OrgStructureTypeID == orgTypeId).ToList();
                        }
                        foreach (var orgName in lstOrgByOrderNumberCount)
                        {
                            var lstprofilebyOrg = lstprofile.Where(s => s.OrgStructureID != null && orgName.ID == s.OrgStructureID.Value).ToList();
                            if (lstprofilebyOrg == null)
                            {
                                continue;
                            }
                            // > 12 tháng
                            DateTime dateHire = DateTime.MinValue;
                            if (item == ">12 Tháng")
                            {
                                dateHire = dateSearch.AddMonths(-12);
                                count = lstprofilebyOrg.Where(m => m.DateHire < dateHire).Count();
                            }
                            // 9 -12 tháng
                            if (item == "9-12 Tháng")
                            {
                                dateHire = dateSearch.AddMonths(-9);
                                DateTime datehireTo = dateSearch.AddMonths(-12);
                                count = lstprofilebyOrg.Where(m => m.DateHire < dateHire && m.DateHire >= datehireTo).Count();
                            }
                            // 6-9 tháng
                            if (item == "6-9 Tháng")
                            {
                                dateHire = dateSearch.AddMonths(-6);
                                DateTime datehireTo = dateSearch.AddMonths(-9);
                                count = lstprofilebyOrg.Where(m => m.DateHire < dateHire && m.DateHire >= datehireTo).Count();
                            }
                            // 3-6 tháng
                            if (item == "3-6 Tháng")
                            {
                                dateHire = dateSearch.AddMonths(-3);
                                DateTime datehireTo = dateSearch.AddMonths(-6);
                                count = lstprofilebyOrg.Where(m => m.DateHire < dateHire && m.DateHire >= datehireTo).Count();
                            }
                            // 0-2 tháng
                            if (item == "0-2 Tháng")
                            {
                                dateHire = dateSearch.AddMonths(-2);
                                count = lstprofilebyOrg.Where(m => m.DateHire >= dateHire).Count();
                            }

                            totalcount += count;

                            if (count > 0)
                            {
                                var orgStructureName = orgName.OrgStructureName + "_" + orgName.Code;
                                if (orgName != null && !string.IsNullOrEmpty(orgName.OrgStructureName) && table.Columns.Contains(orgName.OrgStructureName))
                                {
                                    row[Hre_ReportHCGenderEntity.FieldNames.HeadCount] = totalcount;
                                    row[orgName.OrgStructureName] = count;
                                    row[Hre_ReportHCGenderEntity.FieldNames.Total] = totalcount;

                                }
                            }
                        }
                    }
                    //if (totalcount == 0)
                    //{
                    //    continue;
                    //}
                    table.Rows.Add(row);


                }
                DataRow datarow = table.NewRow();
                datarow[1] = "Total GT";
                for (int i = 2; i < table.Columns.Count; i++)
                {
                    int gt = 0;
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        var valueRow = table.Rows[j][i].ToString();
                        if (!string.IsNullOrEmpty(valueRow) && !string.IsNullOrWhiteSpace(valueRow))
                        {
                            var value = int.Parse(valueRow);
                            if (value >= 0)
                            {
                                gt += value;
                            }
                        }
                    }

                    datarow[i] = gt;
                }

                table.Rows.Add(datarow);
                return table.ConfigTable(true);
            }
        }
        #endregion

        #region BC Head Count Giới Tính

        DataTable CreateReportHCGenderSchema(Guid orgID, Guid? orgTypeID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable datatable = new DataTable("Hre_ReportHCGenderEntity");
                DataRow dr = datatable.NewRow();
                datatable.Columns.Add(Hre_ReportHCGenderEntity.FieldNames.OrgStructureName);
                datatable.Columns.Add(Hre_ReportHCGenderEntity.FieldNames.JobTitleName);
                datatable.Columns.Add(Hre_ReportHCGenderEntity.FieldNames.HeadCount);

                var orgsService = new Cat_OrgStructureServices();
                var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();
                var lstorgs = lstallorgs.Where(s => s.ParentID == orgID).ToList();
                //  var lstOrgName = lstallorgs.Where(s => s.ID == orgID).FirstOrDefault();
                //dr[Hre_ReportHCGenderEntity.FieldNames.OrgStructureName] = lstOrgName == null ? string.Empty : lstOrgName.OrgStructureName + " - " + Gender;
                int count = 0;
                foreach (var item in lstorgs)
                {
                    orderNumber = string.Empty;
                    orderNumber += item.OrderNumber.ToString() + ",";
                    getChildOrgStructure(lstallorgs, item.ID);

                    if (orderNumber.IndexOf(',') > 0)
                        orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                    List<Cat_OrgStructure> lstOrgByOrderNumberCount = new List<Cat_OrgStructure>();
                    var lstObjOrgByOrderNumberCount = new List<object>();
                    lstObjOrgByOrderNumberCount.Add(orderNumber);
                    if (orderNumber != string.Empty)
                    {
                        lstOrgByOrderNumberCount = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumberCount, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin,ref status).OrderBy(s => s.OrgStructureName).ToList();

                    }
                    if (orgTypeID != null)
                    {
                        lstOrgByOrderNumberCount = lstOrgByOrderNumberCount.Where(s => s.OrgStructureTypeID == orgTypeID).ToList();
                    }

                    foreach (var org in lstOrgByOrderNumberCount)
                    {
                        if (!string.IsNullOrEmpty(org.OrgStructureName))
                        {
                            //xử lý tạo thêm 1 dòng để chứa những tên phòng ban trùng tên
                            if (datatable.Columns.Contains(org.OrgStructureName))
                            {
                                dr[0] += org.OrgStructureName + ",";
                                count++;
                                datatable.Columns.Add(org.OrgStructureName, typeof(int));
                            }
                            else
                            {
                                //datatable.Columns.Add(org.OrgStructureName);
                                datatable.Columns.Add(org.OrgStructureName);
                            }

                        }
                    }

                }
                datatable.Columns.Add(Hre_ReportHCGenderEntity.FieldNames.Total);

                //    dr[1] = count;
                datatable.Rows.Add(dr);
                return datatable;
            }
        }

        public DataTable GetReportHCGender(DateTime dateSearch, List<Guid?> lstjobTitles, Guid orgID, Guid? orgTypeID, string Gender, bool _isIncludeQuitEmp, bool isCreateTemplate, string userLogin)
        {

            DataTable table = CreateReportHCGenderSchema(orgID, orgTypeID,userLogin);
            if (isCreateTemplate)
            {
                return table.ConfigTable();
            }
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {

                DataRow row1 = table.NewRow();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var serviceProfile = new Hre_ProfileServices();
                var baseService = new BaseService();
                var lstObjProfileIDs = new List<object>();

                var orgsService = new Cat_OrgStructureServices();
                var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();

                var lstorgs = lstallorgs.Where(s => s.ParentID == orgID).ToList();
                var lstOrgName = lstallorgs.Where(s => s.ID == orgID).FirstOrDefault();

                string male = EnumDropDown.GenderType.E_MALE.ToString();
                string female = EnumDropDown.GenderType.E_FEMALE.ToString();


                var listorgid = lstorgs.Select(s => new { s.ID, s.OrderNumber, s.Code, s.OrgStructureName }).ToList();

                //Xử Lý lấy tất cả nhân viên trong phòng ban đã chọn và group 1 cấp
                var orgIDs = string.Empty;
                orderNumber = string.Empty;

                foreach (var item in listorgid)
                {
                    orderNumber += item.OrderNumber.ToString() + ",";
                    getChildOrgStructure(lstallorgs, item.ID);
                }
                if (orderNumber.IndexOf(',') > 0)
                    orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                var lstObjOrgByOrderNumber = new List<object>();
                lstObjOrgByOrderNumber.Add(orderNumber);
                var lstOrgByOrderNumber = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin ,ref status).Select(s => s.ID).ToList();

                List<object> listObj = new List<object>();
                listObj.Add(orderNumber);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);

                var lstprofile = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin ,ref status).ToList();
                if (!_isIncludeQuitEmp)
                {
                    lstprofile = lstprofile.Where(s => s.DateQuit == null).ToList();
                }

                var jobtitleService = new Cat_JobTitleServices();
                var lstObjJobtitle = new List<object>();
                lstObjJobtitle.Add(null);
                lstObjJobtitle.Add(null);
                lstObjJobtitle.Add(null);
                lstObjJobtitle.Add(1);
                lstObjJobtitle.Add(100000);

                var lstJobtitle = GetData<Cat_JobTitleEntity>(lstObjJobtitle, ConstantSql.hrm_cat_sp_get_JobTitle, userLogin ,ref status).ToList();

                if (lstjobTitles != null && lstjobTitles[0] != null && lstjobTitles[0] != Guid.Empty)
                {
                    lstprofile = lstprofile.Where(s => s.JobTitleID != null && lstjobTitles.Contains(s.JobTitleID.Value)).ToList();
                    lstJobtitle = lstJobtitle.Where(s => lstjobTitles.Contains(s.ID)).ToList();
                }
                if (lstprofile == null || lstorgs == null)
                {
                    return table;
                }



                string gender = string.Empty;
                if (Gender == male)
                {
                    gender = "Male";

                }
                else
                {
                    gender = "Female";
                }

                row1[Hre_ReportHCGenderEntity.FieldNames.OrgStructureName] = lstOrgName == null ? string.Empty : lstOrgName.OrgStructureName + " - " + gender;
                table.Rows.Add(row1);

                foreach (var item in lstJobtitle)
                {
                    bool addTitle = false;
                    int totalcount = 0;
                    var count = 0;

                    DataRow row = table.NewRow();
                    var lstJobtitleById = lstJobtitle.Where(s => item.ID == s.ID).FirstOrDefault();
                    row[Hre_ReportHCGenderEntity.FieldNames.JobTitleName] = lstJobtitleById == null ? string.Empty : lstJobtitleById.JobTitleName;

                    foreach (var org in listorgid)
                    {

                        //xử lý đếm nhân viên của phòng ban con
                        orderNumber = string.Empty;
                        orderNumber += org.OrderNumber.ToString() + ",";
                        getChildOrgStructure(lstallorgs, org.ID);

                        if (orderNumber.IndexOf(',') > 0)
                            orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                        var lstObjOrgByOrderNumberCount = new List<object>();
                        lstObjOrgByOrderNumberCount.Add(orderNumber);
                        var lstOrgByOrderNumberCount = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumberCount, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin,ref status).ToList();
                        if (orgTypeID != null)
                        {
                            lstOrgByOrderNumberCount = lstOrgByOrderNumberCount.Where(s => s.OrgStructureTypeID == orgTypeID).ToList();
                        }

                        foreach (var orgName in lstOrgByOrderNumberCount)
                        {

                            var lstprofilebyOrg = lstprofile.Where(s => s.OrgStructureID != null && orgName.ID == s.OrgStructureID.Value && s.JobTitleID == lstJobtitleById.ID && s.Gender == Gender && s.DateHire != null && s.DateHire.Value <= dateSearch).ToList();

                            if (lstprofilebyOrg == null)
                            {
                                continue;
                            }
                            count = lstprofilebyOrg.Count();
                            totalcount += count;
                            if (count > 0)
                            {
                                var orgStructureName = orgName.OrgStructureName;
                                if (orgName != null && !string.IsNullOrEmpty(orgName.OrgStructureName) && table.Columns.Contains(orgStructureName))
                                {
                                    row[orgName.OrgStructureName] = count;
                                }
                                row[Hre_ReportHCGenderEntity.FieldNames.HeadCount] = totalcount;
                                row[Hre_ReportHCGenderEntity.FieldNames.Total] = totalcount;

                            }
                        }
                    }
                    if (totalcount == 0)
                    {
                        continue;
                    }
                    table.Rows.Add(row);

                }
                DataRow datarow = table.NewRow();
                datarow[1] = "Total GT";
                for (int i = 2; i < table.Columns.Count; i++)
                {
                    int gt = 0;
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        var valueRow = table.Rows[j][i].ToString();
                        if (!string.IsNullOrEmpty(valueRow) && !string.IsNullOrWhiteSpace(valueRow))
                        {
                            var value = int.Parse(valueRow);
                            if (value >= 0)
                            {
                                gt += value;
                            }
                        }
                    }

                    datarow[i] = gt;
                }

                table.Rows.Add(datarow);
                return table.ConfigTable(true);
            }
        }

        #endregion

        #region BC HeadCount Hàng Tháng
        DataTable CreateReportMonthlyHCSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable datatable = new DataTable("Hre_ReportMonthlyHCEntity");
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.OrgStructureParentName);
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.OrgStructureName);
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.JobTitleName);

                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppJan, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppFeb, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppMar, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppApr, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppMay, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppJun, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppJul, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppAug, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppSep, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppOct, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppNov, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppDec, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.AppYear, typeof(int));

                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActJan, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActFeb, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActMar, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActApr, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActMay, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActJun, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActJul, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActAug, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActSep, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActOct, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActNov, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActDec, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.ActYear, typeof(int));

                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaJan, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaFeb, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaMar, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaApr, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaMay, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaJun, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaJul, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaAug, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaSep, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaOct, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaNov, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaDec, typeof(int));
                datatable.Columns.Add(Hre_ReportMonthlyHCEntity.FieldNames.LeaYear, typeof(int));

                return datatable;
            }
        }



        public DataTable GetReportMonthlyHC(DateTime DateSearch, List<Guid?> lstjobTitles, Guid orgID, Guid? orgTypeId, bool isCreateTemplate, string userLogin)
        {

            DataTable table = CreateReportMonthlyHCSchema();
            if (isCreateTemplate)
            {
                return table;
            }
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var serviceProfile = new Hre_ProfileServices();
                var baseService = new BaseService();
                var lstObjProfileIDs = new List<object>();

                var orgsService = new Cat_OrgStructureServices();
                var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();

                var lstorgs = lstallorgs.Where(s => s.ParentID == orgID).ToList();

                var listorgid = lstorgs.Select(s => new { s.ID, s.OrderNumber, s.OrgStructureTypeID }).ToList();


                //Xử Lý lấy tất cả nhân viên trong phòng ban đã chọn và group 1 cấp
                var orgIDs = string.Empty;
                orderNumber = string.Empty;
                foreach (var item in listorgid)
                {
                    orderNumber += item.OrderNumber.ToString() + ",";
                    getChildOrgStructure(lstallorgs, item.ID);
                }
                if (orderNumber.IndexOf(',') > 0)
                    orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);
                if (orderNumber == string.Empty)
                    return table;

                var lstObjOrgByOrderNumber = new List<object>();
                lstObjOrgByOrderNumber.Add(orderNumber);
                var lstOrgByOrderNumber = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin ,ref status).Select(s => new { s.ID, s.OrderNumber, s.OrgStructureName, s.OrgStructureTypeID, s.ParentID }).ToList();
                if (orgTypeId != null)
                {
                    lstOrgByOrderNumber = lstOrgByOrderNumber.Where(s => s.OrgStructureTypeID == orgTypeId).ToList();
                }

                List<object> listObj = new List<object>();
                listObj.Add(orderNumber);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);


                var headCountService = new Hre_PlanHeadCountServices();
                var lstObjPlanHeadCount = new List<object>();
                lstObjPlanHeadCount.Add(null);
                lstObjPlanHeadCount.Add(1);
                lstObjPlanHeadCount.Add(10000000);
                var lstPlanHeadCount = GetData<Hre_PlanHeadCountEntity>(lstObjPlanHeadCount, ConstantSql.hrm_hr_sp_get_PlanHeadCount, userLogin, ref status).ToList();

                var lstProfile = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).Where(s => s.DateHire != null && s.DateQuit == null && s.DateHire <= DateSearch).ToList();
                var lstprofileWorking = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).Where(s => s.DateHire != null && s.StatusSyn == null && s.DateHire.Value.Year == DateSearch.Year && s.DateQuit == null).ToList();
                var lstprofileQuit = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).Where(s => s.DateQuit != null && s.DateQuit.Value.Year == DateSearch.Year).ToList();

                var jobtitleService = new Cat_JobTitleServices();
                var lstObjJobtitle = new List<object>();
                lstObjJobtitle.Add(null);
                lstObjJobtitle.Add(null);
                lstObjJobtitle.Add(null);
                lstObjJobtitle.Add(1);
                lstObjJobtitle.Add(100000);

                var lstJobtitle = GetData<Cat_JobTitleEntity>(lstObjJobtitle, ConstantSql.hrm_cat_sp_get_JobTitle, userLogin, ref status).ToList();

                if (lstjobTitles != null && lstjobTitles[0] != null && lstjobTitles[0] != Guid.Empty)
                {
                    lstprofileWorking = lstprofileWorking.Where(s => s.JobTitleID != null && lstjobTitles.Contains(s.JobTitleID.Value)).ToList();
                    lstprofileQuit = lstprofileQuit.Where(s => s.JobTitleID != null && lstjobTitles.Contains(s.JobTitleID.Value)).ToList();
                    lstJobtitle = lstJobtitle.Where(s => lstjobTitles.Contains(s.ID)).ToList();
                }
                if (lstprofileWorking == null || lstorgs == null || lstprofileQuit == null)
                {
                    return table;
                }

                foreach (var org in lstOrgByOrderNumber)
                {
                    DataRow row = table.NewRow();
                    var parentOrg = lstallorgs.Where(s => s.ID == org.ParentID).FirstOrDefault();
                    row[Hre_ReportMonthlyHCEntity.FieldNames.OrgStructureParentName] = parentOrg != null ? parentOrg.OrgStructureName : string.Empty;
                    row[Hre_ReportMonthlyHCEntity.FieldNames.OrgStructureName] = org != null ? org.OrgStructureName : string.Empty;

                    //xử lý đếm nhân viên của phòng ban con
                    var IDsCount = string.Empty;
                    orderNumber = string.Empty;
                    orderNumber += org.OrderNumber.ToString() + ",";
                    getChildOrgStructure(lstallorgs, org.ID);

                    if (orderNumber.IndexOf(',') > 0)
                        orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                    var lstObjOrgByOrderNumberCount = new List<object>();
                    lstObjOrgByOrderNumberCount.Add(orderNumber);
                    var lstOrgByOrderNumberCount = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumberCount, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin, ref status).Select(s => new { s.ID, s.OrgStructureTypeID }).ToList();
                    if (orgTypeId != null)
                    {
                        lstOrgByOrderNumberCount = lstOrgByOrderNumberCount.Where(s => s.OrgStructureTypeID == orgTypeId).ToList();
                    }
                    var lstOrgGroupByType = lstOrgByOrderNumberCount.Select(s => s.ID).ToList();


                    bool addTitle = false;


                    foreach (var jobtitle in lstJobtitle)
                    {
                        DataRow row1 = table.NewRow();
                        var countingPlanHeadCount = 0;
                        var counttingProfileWorking = 0;
                        var countingProfileQuit = 0;


                        if (jobtitle.OrgStructureID != null)
                        {
                            if (jobtitle.OrgStructureID == orgID)
                            {
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.JobTitleName] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;

                                #region Approved
                                var countProfile = 0;
                                var totalProfile = 0;
                                var lstProfileWorkingByJobTitleID = lstprofileWorking.Where(s => jobtitle.ID == s.JobTitleID && lstOrgGroupByType.Contains(s.OrgStructureID.Value)).Select(p => new { p.ID, p.DateHire, p.StatusSyn }).ToList();
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 01).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 01).ToList().Count();
                                    //   countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    //row1[Hre_ReportMonthlyHCEntity.FieldNames.AppJan] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 02).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 02).ToList().Count();
                                    //    countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    //row1[Hre_ReportMonthlyHCEntity.FieldNames.AppFeb] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 03).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 03).ToList().Count();
                                    //    countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    // row1[Hre_ReportMonthlyHCEntity.FieldNames.AppMar] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 04).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 04).ToList().Count();
                                    //    countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    // row1[Hre_ReportMonthlyHCEntity.FieldNames.AppApr] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 05).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 05).ToList().Count();
                                    //  countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    // row1[Hre_ReportMonthlyHCEntity.FieldNames.AppMay] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 06).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 06).ToList().Count();
                                    //   countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    //row1[Hre_ReportMonthlyHCEntity.FieldNames.AppJun] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 07).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 07).ToList().Count();
                                    //   countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    //row1[Hre_ReportMonthlyHCEntity.FieldNames.AppJul] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 08).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 08).ToList().Count();
                                    // countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    //row1[Hre_ReportMonthlyHCEntity.FieldNames.AppAug] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 09).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 09).ToList().Count();
                                    //  countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.AppSep] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 10).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 10).ToList().Count();
                                    //  countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    // row1[Hre_ReportMonthlyHCEntity.FieldNames.AppOct] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 11).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 11).ToList().Count();
                                    //countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.AppNov] = totalProfile;
                                }
                                if (lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 12).ToList() != null)
                                {
                                    countProfile = lstProfileWorkingByJobTitleID.Where(s => s.DateHire.Value.Month == 12).ToList().Count();
                                    // countingProfileQuit += countProfileQuit;
                                    totalProfile += countProfile;
                                    //row1[Hre_ReportMonthlyHCEntity.FieldNames.AppDec] = totalProfile;
                                }


                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppJan] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppFeb] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppMar] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppApr] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppMay] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppJun] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppJul] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppAug] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppSep] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppOct] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppNov] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppDec] = totalProfile;
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.AppYear] = totalProfile;


                                #endregion

                                #region  Actual HC
                                double countProfileWorking = 0;
                                double totalProfileWorking = 0;

                                var lstProfileByJobTitleID = lstProfile.Where(s => jobtitle.ID == s.JobTitleID && lstOrgGroupByType.Contains(s.OrgStructureID.Value)).Select(p => new { p.ID, p.DateHire, p.StatusSyn, p.DateQuit }).ToList();


                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 01).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 01).ToList().Count();
                                    //   counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActJan] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 02).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 02).ToList().Count();
                                    //  counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActFeb] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 03).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 03).ToList().Count();
                                    //   counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActMar] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 04).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 04).ToList().Count();
                                    //  counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActApr] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 05).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 05).ToList().Count();
                                    //  counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActMay] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 06).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 06).ToList().Count();
                                    // counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActJun] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 07).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 07).ToList().Count();
                                    //  counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActJul] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 08).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 08).ToList().Count();
                                    // counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActAug] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 09).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 09).ToList().Count();
                                    // counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActSep] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 10).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 10).ToList().Count();
                                    // counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActOct] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 11).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 11).ToList().Count();
                                    // counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActNov] = countProfileWorking;
                                }
                                if (lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 12).ToList() != null)
                                {
                                    countProfileWorking = lstProfileByJobTitleID.Where(s => s.DateHire.Value.Month == 12).ToList().Count();
                                    // counttingProfileWorking += countProfileWorking;
                                    totalProfileWorking += countProfileWorking;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.ActDec] = countProfileWorking;
                                }
                                double total = 0;
                                if (totalProfileWorking / 12 < 1 && totalProfileWorking / 12 > 0)
                                {
                                    total = 1;
                                }
                                else
                                {
                                    total = Math.Round(totalProfileWorking / 12);
                                }
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.ActYear] = totalProfileWorking;
                                #endregion

                                #region Leaver HC
                                var countProfileQuit = 0;
                                var totalQuit = 0;
                                var lstProfileQuitByJobTitleID = lstprofileQuit.Where(s => jobtitle.ID == s.JobTitleID && lstOrgGroupByType.Contains(s.OrgStructureID.Value)).Select(p => new { p.ID, p.DateHire, p.StatusSyn, p.DateQuit }).ToList();

                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 01).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 01).ToList().Count();
                                    //   countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaJan] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 02).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 02).ToList().Count();
                                    //    countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaFeb] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 03).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 03).ToList().Count();
                                    //    countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaMar] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 04).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 04).ToList().Count();
                                    //    countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaApr] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 05).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 05).ToList().Count();
                                    //  countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaMay] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 06).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 06).ToList().Count();
                                    //   countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaJun] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 07).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 07).ToList().Count();
                                    //   countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaJul] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 08).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 08).ToList().Count();
                                    // countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaAug] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 09).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 09).ToList().Count();
                                    //  countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaSep] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 10).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 10).ToList().Count();
                                    //  countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaOct] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 11).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 11).ToList().Count();
                                    //  countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaNov] = countProfileQuit;
                                }
                                if (lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 12).ToList() != null)
                                {
                                    countProfileQuit = lstProfileQuitByJobTitleID.Where(s => s.DateQuit.Value.Month == 12).ToList().Count();
                                    // countingProfileQuit += countProfileQuit;
                                    totalQuit += countProfileQuit;
                                    row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaDec] = countProfileQuit;
                                }
                                row1[Hre_ReportMonthlyHCEntity.FieldNames.LeaYear] = totalQuit;
                                #endregion
                                //if (totalQuit == 0 && totalProfileWorking == 0)
                                //{
                                //    continue;
                                //}
                                //if (lstProfileQuitByJobTitleID.Count == 0 && lstProfileWorkingByJobTitleID.Count == 0)
                                //{
                                //    continue;
                                //}
                                if (!addTitle)
                                {
                                    table.Rows.Add(row);
                                    addTitle = true;
                                }
                                table.Rows.Add(row1);
                            }

                        }
                    }

                }

                DataRow datarow = table.NewRow();
                datarow[1] = "Total GT";

                for (int i = 3; i < table.Columns.Count; i++)
                {
                    int gt = 0;
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        var valueRow = table.Rows[j][i].ToString();
                        if (!string.IsNullOrEmpty(valueRow) && !string.IsNullOrWhiteSpace(valueRow))
                        {
                            var value = int.Parse(valueRow);
                            if (value >= 0)
                            {
                                gt += value;

                            }
                        }
                    }
                    datarow[i] = gt;
                }

                table.Rows.Add(datarow);
                return table;
            }
        }

        #endregion

        #region BC NV quay lại
        public List<Hre_ReportProfileComeBackEntity> GetReportProfileComeBack(DateTime? DateFrom, DateTime? DateTo, string lstOrgStructureIDs, string userLogin)
        {
            string status = string.Empty;
            List<Hre_ReportProfileComeBackEntity> lstReportProfileComeBackEntity = new List<Hre_ReportProfileComeBackEntity>();
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                List<object> lstpara = new List<object>();
                lstpara.Add(DateFrom);
                lstpara.Add(DateTo);
                lstpara.Add(Common.DotNetToOracle(lstOrgStructureIDs));

                var lstProfileComeBack = service.GetData<Hre_ReportProfileComeBackEntity>(lstpara, ConstantSql.hrm_hr_sp_get_RptProfileComBack, userLogin, ref status);
                if (lstProfileComeBack == null)
                {
                    return lstReportProfileComeBackEntity;
                }
                var lstProfileids = lstProfileComeBack.Select(s => s.ProfileID).ToList();

                var lstobjectProfileids = new List<object>();
                string strIDs = string.Empty;
                foreach (var item in lstProfileids)
                {
                    strIDs += Common.DotNetToOracle(item.ToString()) + ",";
                }
                if (strIDs.IndexOf(",") > 0)
                    strIDs = strIDs.Substring(0, strIDs.Length - 1);
                lstobjectProfileids.Add(strIDs);
                var lstprofile = service.GetData<Hre_ProfileEntity>(lstobjectProfileids, ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                foreach (var ProfileComeBack in lstProfileComeBack)
                {
                    var profileByProfileComeBack = lstprofile.Where(s => s.ID == ProfileComeBack.ProfileID).FirstOrDefault();
                    Hre_ReportProfileComeBackEntity EntityReport = new Hre_ReportProfileComeBackEntity();
                    Guid? orgId = profileByProfileComeBack.OrgStructureID;
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    EntityReport.CodeEmp = profileByProfileComeBack.CodeEmp;
                    EntityReport.ProfileName = profileByProfileComeBack.ProfileName;
                    EntityReport.BranchName = orgBranch != null ? orgBranch.OrgStructureName : null;
                    EntityReport.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : null;
                    EntityReport.TeamName = orgTeam != null ? orgTeam.OrgStructureName : null;
                    EntityReport.SectionName = orgSection != null ? orgSection.OrgStructureName : null;
                    EntityReport.PositionName = ProfileComeBack.PositionName;
                    EntityReport.DateOfBirth = ProfileComeBack.DateOfBirth;
                    EntityReport.Gender = ProfileComeBack.Gender;
                    EntityReport.PlaceOfBirth = ProfileComeBack.PlaceOfBirth;
                    EntityReport.IDPlaceOfIssue = ProfileComeBack.IDPlaceOfIssue;
                    EntityReport.PAddress = ProfileComeBack.PAddress;
                    EntityReport.HomePhone = ProfileComeBack.HomePhone;
                    EntityReport.Cellphone = ProfileComeBack.Cellphone;
                    EntityReport.IDNo = ProfileComeBack.IDNo;
                    EntityReport.DateComeBack = ProfileComeBack.DateComeBack;
                    EntityReport.DateHire = ProfileComeBack.DateHire;
                    EntityReport.DateQuit = ProfileComeBack.DateQuit;
                    EntityReport.TypeOfStop = ProfileComeBack.TypeOfStop;
                    EntityReport.ResignReasonName = ProfileComeBack.ResignReasonName;
                    EntityReport.SalaryRankName = ProfileComeBack.SalaryRankName;
                    lstReportProfileComeBackEntity.Add(EntityReport);
                }
            }
            return lstReportProfileComeBackEntity;
        }
        #endregion

        #region Hàm Lấy Phòng Ban theo đệ quy
        //Biến để get orderNumber của phòng ban
        string orderNumber = string.Empty;
        //Hàm đệ quy để lấy phòng ban
        public void getChildOrgStructure(List<Cat_OrgStructure> source, Guid idParent, Guid? orgTypeID)
        {
            var child = source.Where(m => m.ParentID == idParent && m.OrgStructureTypeID == orgTypeID).ToList();
            if (child.Count <= 0)
                return;
            else
            {
                for (int i = 0; i < child.Count; i++)
                {
                    orderNumber += child[i].OrderNumber.ToString() + ",";
                    getChildOrgStructure(source, child[i].ID, orgTypeID);
                }
            }
        }

        public void getChildOrgStructure(List<Cat_OrgStructure> source, Guid idParent)
        {
            var child = source.Where(m => m.ParentID == idParent).ToList();
            if (child.Count <= 0)
                return;
            else
            {
                for (int i = 0; i < child.Count; i++)
                {
                    orderNumber += child[i].OrderNumber.ToString() + ",";
                    getChildOrgStructure(source, child[i].ID);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOrg"></param>
        /// <param name="listOrgType"></param>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public List<string> GetParentOrg(List<Cat_OrgStructure> listOrg, List<Cat_OrgStructureType> listOrgType, Guid? CurrentID)
        {
            List<string> result = new List<string>();
            while (true)
            {
                var item = listOrg.Single(m => m.ID == CurrentID);
                if (item != null)
                {
                    result.Add(listOrg.Single(m => m.ID == CurrentID).OrgStructureName);
                    if (item.OrgStructureTypeID != null && listOrgType.Where(m => m.ID == (Guid)item.OrgStructureTypeID).FirstOrDefault().OrgStructureTypeCode != "DEPARTMENT")
                    {
                        if (item.ParentID == null)
                        {
                            return result;
                        }
                        CurrentID = (Guid)item.ParentID;
                    }
                    else
                    {
                        return result;
                    }
                }

            }
        }
        public List<string> GetParentOrgName(List<Cat_OrgStructureEntity> listOrg, List<Cat_OrgStructureTypeEntity> listOrgType, Guid? CurrentID)
        {
            List<string> result = new List<string>();


            while (true)
            {
                var item = listOrg.Where(m => m.ID == CurrentID).FirstOrDefault();
                if (item != null)
                {
                    result.Add(listOrg.Single(m => m.ID == CurrentID).OrgStructureName);
                    if (item.OrgStructureTypeID != null && listOrgType.Where(m => m.ID == (Guid)item.OrgStructureTypeID).FirstOrDefault().OrgStructureTypeCode != "DEPARTMENT")
                    {
                        if (item.ParentID == null)
                        {
                            return result;
                        }
                        CurrentID = (Guid)item.ParentID;
                    }
                    else
                    {
                        return result;
                    }
                }

            }
        }
        #endregion

        #region Báo cáo TỔNG HỢP danh sách kỷ luật

        DataTable CreateSchema_SummaryDiscipline()
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable datatable = new DataTable("Hre_ReportSummaryDisciplineModel");
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                datatable.Columns.Add("DisciplineTypeName");
                for (int i = 1; i <= 12; i++)
                {
                    datatable.Columns.Add(i.ToString(), typeof(int));
                }
                return datatable;
            }
        }

        public DataTable GetReportSummaryDiscipline(DateTime DateFrom, DateTime DateTo, string strOrgIDs, bool isCreateTemplate, string userLogin)
        {
            DataTable tblData = CreateSchema_SummaryDiscipline();
            if (isCreateTemplate)
            {
                return tblData;
            }

            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_DisciplineTypes = new CustomBaseRepository<Cat_DisciplinedTypes>(unitOfWork);
                var lstDisciplineTypes = repoCat_DisciplineTypes.FindBy(s => s.IsDelete == null).ToList();
                // Son.Vo - 20150405 - fix cứng theo yêu cầu - task 0044222
                if (lstDisciplineTypes != null)
                {
                    lstDisciplineTypes = lstDisciplineTypes.Where(s => s.Code == "DisciplineWithWritten" || s.Code == "DelayOfSalaryReview" || s.Code == "Dismissal").ToList();
                }
                List<object> listObj = new List<object>();
                listObj.Add(DateFrom);
                listObj.Add(DateTo);
                listObj.Add(strOrgIDs);

                var lstDiscipline = GetData<Hre_ReportProfileDisciplineEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptDiscripline, userLogin, ref status).ToList();

                foreach (var type in lstDisciplineTypes)
                {
                    var lstDisciplineOfType = lstDiscipline.Where(s => s.DisciplineTypeID == type.ID).ToList();
                    if (lstDisciplineOfType.Count <= 0)
                        continue;
                    DataRow dr = tblData.NewRow();
                    dr["DisciplineTypeName"] = type.DisciplinedTypesName;

                    for (int i = 1; i <= 12; i++)
                    {
                        var lstDisciplineOfTypeInMonth = lstDisciplineOfType.Where(s => s.DateOfViolation != null && s.DateOfViolation.Value.Month == i).ToList();
                        if (lstDisciplineOfTypeInMonth != null)
                            dr[i.ToString()] = lstDisciplineOfTypeInMonth.Count;
                        else
                            dr[i.ToString()] = 0;
                    }
                    tblData.Rows.Add(dr);
                }

            }

            tblData.Columns["1"].ColumnName = "Jan";
            tblData.Columns["2"].ColumnName = "Feb";
            tblData.Columns["3"].ColumnName = "Mar";
            tblData.Columns["4"].ColumnName = "Apr";
            tblData.Columns["5"].ColumnName = "May";
            tblData.Columns["6"].ColumnName = "Jun";
            tblData.Columns["7"].ColumnName = "Jul";
            tblData.Columns["8"].ColumnName = "Aug";
            tblData.Columns["9"].ColumnName = "Sep";
            tblData.Columns["10"].ColumnName = "Oct";
            tblData.Columns["11"].ColumnName = "Nov";
            tblData.Columns["12"].ColumnName = "Dec";

            return tblData;
        }

        #endregion

        #region BC Nhân Viên Mới Vào Làm Việc
        public DataTable CreateProfileNewSchema()
        {
            DataTable tb = new DataTable("Hre_ReportProfileNewModel");
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.ProfileName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.DepartmentNameOrg);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.JobTitleName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.PositionName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.DateHire, typeof(DateTime));
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.IDNo);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.IDPlaceOfIssue);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.IDDateOfIssue, typeof(DateTime));
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.SalaryClassName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.Gender);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.PAStreet);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.EducationLevelName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.GraduatedLevelName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.UnitNameOrg);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.DivisionNameOrg);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.SectionNameOrg);
            //tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.DateOfBirth);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.PlaceOfBirth);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.PAddress);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.StatusSyn);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.EmployeeTypeName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.DateFrom);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.DateTo);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_SECTION);


            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.datequit);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.EthnicGroupName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.ContractNo);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.DateStart);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.Code);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.InsuranceAmount);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_MaleBirth);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_FeMaleBirth);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_ProfileCount);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_ProfileIsWorking);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_FEMALE);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_MALE);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.ProfileNew);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_Profile_FEMALE);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_Profile_MALE);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.PProvinceName);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.E_GrossAmount);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.GrossAmount_Old);
            tb.Columns.Add(Hre_ReportProfileNewEntity.FieldNames.GrossAmount_Now);

            return tb;

        }
        public DataTable GetReportProfileNew(DateTime from, DateTime to, string orderNumber, 
            bool IsCreateTemplate, string codeemp, string profilename, Guid? Salaryclassid, 
            string codeCandidate, Guid? workPlaceID, Guid? emptypeID, string userLogin)
        {
            DataTable table = CreateProfileNewSchema();
            if (IsCreateTemplate)
            {
                return table;
            }
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                //var orgTypeServices = new Cat_OrgStructureTypeServices();
                //var objOrgType = new List<object>();
                //objOrgType.Add(null);
                //objOrgType.Add(null);
                //objOrgType.Add(1);
                //objOrgType.Add(int.MaxValue - 1);
                //var lstOrgType = orgTypeServices.GetData<Cat_OrgStructureTypeEntity>(objOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status).ToList().Translate<Cat_OrgStructureType>();

                //var orgServices = new Cat_OrgStructureServices();
                //var objOrg = new List<object>();
                //objOrg.Add(null);
                //objOrg.Add(null);
                //objOrg.Add(null);
                //objOrg.Add(1);
                //objOrg.Add(int.MaxValue - 1);
                //var lstOrg = orgServices.GetData<Cat_OrgStructureEntity>(objOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList().Translate<Cat_OrgStructure>();

                var hrService = new Hre_ProfileServices();
                List<object> listObj = new List<object>();
                listObj.Add(orderNumber);
                listObj.Add(from);
                listObj.Add(to);
                listObj.Add(codeemp);
                listObj.Add(profilename);
                listObj.Add(Salaryclassid);
                listObj.Add(codeCandidate);
                listObj.Add(workPlaceID);
                listObj.Add(emptypeID);
                var lstProfile = hrService.GetData<Hre_ReportProfileNewEntity>(listObj, ConstantSql.hrm_hr_sp_get_RptProfileNew, userLogin, ref status).ToList();

                foreach (var item in lstProfile)
                {
                    DataRow dr = table.NewRow();
                    //Guid? orgId = item.OrgStructureID;
                    //var org = lstOrg.FirstOrDefault(s => s.ID == item.OrgStructureID);
                    //var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, lstOrg, lstOrgType);
                    //var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, lstOrg, lstOrgType);
                    //var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, lstOrg, lstOrgType);
                    //var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, lstOrg, lstOrgType);
                    dr[Hre_ReportProfileNewEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    dr[Hre_ReportProfileNewEntity.FieldNames.ProfileName] = item.ProfileName;
                    dr[Hre_ReportProfileNewEntity.FieldNames.PositionName] = item.PositionName;
                    dr[Hre_ReportProfileNewEntity.FieldNames.JobTitleName] = item.PositionName;
                    //dr[Hre_ReportProfileNewEntity.FieldNames.OrgStructureName] = item.OrgStructureName;
                    dr[Hre_ReportProfileNewEntity.FieldNames.DateHire] = item.DateHire;
                    dr[Hre_ReportProfileNewEntity.FieldNames.IDNo] = item.IDNo;
                    dr[Hre_ReportProfileNewEntity.FieldNames.IDPlaceOfIssue] = item.IDPlaceOfIssue;
                    if (item.IDDateOfIssue != null)
                    {
                        dr[Hre_ReportProfileNewEntity.FieldNames.IDDateOfIssue] = item.IDDateOfIssue;
                    }

                    dr[Hre_ReportProfileNewEntity.FieldNames.Gender] = item.Gender;
                    dr[Hre_ReportProfileNewEntity.FieldNames.PAStreet] = item.PAStreet;
                    dr[Hre_ReportProfileNewEntity.FieldNames.SalaryClassName] = item.SalaryClassName;
                    dr[Hre_ReportProfileNewEntity.FieldNames.EducationLevelName] = item.EducationLevelName;
                    dr[Hre_ReportProfileNewEntity.FieldNames.GraduatedLevelName] = item.GraduatedLevelName;
                    if (item.DepartmentNameOrg != null)
                        dr[Hre_ReportProfileNewEntity.FieldNames.DepartmentNameOrg] = item.DepartmentNameOrg;
                    if (item.UnitNameOrg != null)
                        dr[Hre_ReportProfileNewEntity.FieldNames.UnitNameOrg] = item.UnitNameOrg;
                    if (item.DivisionNameOrg != null)
                        dr[Hre_ReportProfileNewEntity.FieldNames.DivisionNameOrg] = item.DivisionNameOrg;
                    if (item.SectionNameOrg != null)
                        dr[Hre_ReportProfileNewEntity.FieldNames.SectionNameOrg] = item.SectionNameOrg;
                    dr[Hre_ReportProfileNewEntity.FieldNames.DateOfBirth] = item.DateOfBirth;
                    dr[Hre_ReportProfileNewEntity.FieldNames.PlaceOfBirth] = item.PlaceOfBirth;
                    dr[Hre_ReportProfileNewEntity.FieldNames.PAddress] = item.PAddress;
                    dr[Hre_ReportProfileNewEntity.FieldNames.StatusSyn] = item.StatusSyn;
                    dr[Hre_ReportProfileNewEntity.FieldNames.EmployeeTypeName] = item.EmployeeTypeName;
                    dr[Hre_ReportProfileNewEntity.FieldNames.DateFrom] = from;
                    dr[Hre_ReportProfileNewEntity.FieldNames.DateTo] = to;

                    dr[Hre_ReportProfileNewEntity.FieldNames.E_UNIT] = item.E_UNIT;

                    dr[Hre_ReportProfileNewEntity.FieldNames.E_DIVISION] = item.E_DIVISION;

                    dr[Hre_ReportProfileNewEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;


                    dr[Hre_ReportProfileNewEntity.FieldNames.E_TEAM] = item.E_TEAM;


                    dr[Hre_ReportProfileNewEntity.FieldNames.E_SECTION] = item.E_SECTION;

                    dr[Hre_ReportProfileNewEntity.FieldNames.datequit] = item.datequit;
                    dr[Hre_ReportProfileNewEntity.FieldNames.EthnicGroupName] = item.EthnicGroupName;
                    dr[Hre_ReportProfileNewEntity.FieldNames.ContractNo] = item.ContractNo;
                    dr[Hre_ReportProfileNewEntity.FieldNames.DateStart] = item.DateStart;
                    dr[Hre_ReportProfileNewEntity.FieldNames.Code] = item.Code;
                    dr[Hre_ReportProfileNewEntity.FieldNames.InsuranceAmount] = item.InsuranceAmount;
                    dr[Hre_ReportProfileNewEntity.FieldNames.E_MaleBirth] = item.E_MaleBirth;
                    dr[Hre_ReportProfileNewEntity.FieldNames.E_FeMaleBirth] = item.E_FeMaleBirth;
                    dr[Hre_ReportProfileNewEntity.FieldNames.E_ProfileCount] = item.E_ProfileCount;
                    dr[Hre_ReportProfileNewEntity.FieldNames.E_ProfileIsWorking] = item.E_ProfileIsWorking;
                    dr[Hre_ReportProfileNewEntity.FieldNames.E_FEMALE] = item.E_FEMALE;
                    dr[Hre_ReportProfileNewEntity.FieldNames.E_MALE] = item.E_MALE;
                    dr[Hre_ReportProfileNewEntity.FieldNames.ProfileNew] = item.ProfileNew;
                    dr[Hre_ReportProfileNewEntity.FieldNames.E_Profile_FEMALE] = item.E_Profile_FEMALE;
                    dr[Hre_ReportProfileNewEntity.FieldNames.E_Profile_MALE] = item.E_Profile_MALE;
                    dr[Hre_ReportProfileNewEntity.FieldNames.PProvinceName] = item.PProvinceName;
                    dr[Hre_ReportProfileNewEntity.FieldNames.E_GrossAmount] = item.E_GrossAmount;
                    dr[Hre_ReportProfileNewEntity.FieldNames.GrossAmount_Old] = item.GrossAmount_Old;
                    dr[Hre_ReportProfileNewEntity.FieldNames.GrossAmount_Now] = item.GrossAmount_Now;

                    table.Rows.Add(dr);
                }

                return table;
            }
        }
        #endregion

        #region BC tình hình trả lương HDT
        public List<Hre_ReportPayHDTJobEntity> GetPayHDTJob(DateTime DateFrom, DateTime DateTo, string lstOrgOrderNumber, string userLogin)
        {
            string status = string.Empty;
            List<Hre_ReportPayHDTJobEntity> lstReportHDTJobEntity = new List<Hre_ReportPayHDTJobEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(lstOrgOrderNumber);
                listObjHDTJob.Add(DateFrom);
                listObjHDTJob.Add(DateTo);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_HDTJob, userLogin, ref status).ToList();

                if (lstHDTJob.Count == 0)
                {
                    return lstReportHDTJobEntity;
                }

                var lstprofile = lstHDTJob.Select(s => s.ProfileID).Distinct().ToList();
                var repoHDTJobType = new Cat_HDTJobTypeRepository(unitOfWork);
                var lstHDTJobType = repoHDTJobType.GetAll().ToList();
                List<object> lstParam = new List<object>();
                lstParam.Add(null);
                lstParam.Add(DateFrom);
                lstParam.Add(DateTo);
                var workDays = GetData<Att_WorkdayEntity>(lstParam, ConstantSql.hrm_att_getdata_Workday, userLogin, ref status).ToList();
                if (workDays.Count == 0)
                {
                    return lstReportHDTJobEntity;
                }
                foreach (var Profileid in lstprofile)
                {
                    Hre_ReportPayHDTJobEntity entity = new Hre_ReportPayHDTJobEntity();
                    var Profile = lstHDTJob.Where(s => s.ProfileID == Profileid).FirstOrDefault();
                    if (Profile != null)
                    {
                        entity.ProfileName = Profile.ProfileName;
                        entity.CodeEmp = Profile.CodeEmp;
                        entity.E_UNIT = Profile.E_UNIT;
                        entity.E_DIVISION = Profile.E_DIVISION;
                        entity.E_DEPARTMENT = Profile.E_DEPARTMENT;
                        entity.E_TEAM = Profile.E_TEAM;
                        entity.E_SECTION = Profile.E_SECTION;
                        var hdtbyprofile = lstHDTJob.Where(s => s.ProfileID == Profile.ID).FirstOrDefault();
                        entity.OrgStructureName = hdtbyprofile != null ? hdtbyprofile.Dept : string.Empty;
                        int counttype4 = 0;
                        int counttype5 = 0;
                        var lstWorkdayByProfile = workDays.Where(s => s.ProfileID == Profileid).ToList();
                        for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1))
                        {
                            var workdaybyprofile = lstWorkdayByProfile.Where(s => s.FirstInTime != null && s.LastOutTime != null &&
                                s.WorkDate != null && s.WorkDate.Date == date.Date).FirstOrDefault();
                            var countype4 = lstHDTJob.Where(s => s.ProfileID == Profileid && s.Type == "E_TYPE4"
                                  && s.DateFrom <= date && s.DateTo >= date).ToList();
                            if (countype4 != null && countype4.Count > 0 && workdaybyprofile != null)
                            {
                                counttype4++;
                            }

                            var countype5 = lstHDTJob.Where(s => s.ProfileID == Profileid && s.Type == "E_TYPE5"
                                  && s.DateFrom <= date && s.DateTo >= date).ToList();
                            if (countype5 != null && countype5.Count > 0 && workdaybyprofile != null)
                            {
                                counttype5++;
                            }

                        }
                        entity.CountType4 = counttype4;
                        entity.CountType5 = counttype5;
                        lstReportHDTJobEntity.Add(entity);
                    }

                }
            }
            return lstReportHDTJobEntity;
        }
        #endregion

        #region BC NV đk hdt nhưng không đi làm
        public DataTable GetSchema_ReportProfileHDTNotWork(String nameReport)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add("ProfileName");
            tb.Columns.Add("CodeEmp");
            tb.Columns.Add("E_UNIT");
            tb.Columns.Add("E_DIVISION");
            tb.Columns.Add("E_DEPARTMENT");
            tb.Columns.Add("E_TEAM");
            tb.Columns.Add("E_SECTION");
            tb.Columns.Add("HDTJobGroupCode");
            tb.Columns.Add("HDTJobTypeCode");
            tb.Columns.Add("Price", typeof(double));
            tb.Columns.Add("LeaveDayTypeCode");
            tb.Columns.Add("HDTDateFrom", typeof(DateTime));
            tb.Columns.Add("HDTDateTo", typeof(DateTime));

            return tb;
        }


        public DataTable GetReportProfileHDTNotWork(DateTime DateFrom, DateTime DateTo, string lstOrgOrderNumber, string nameReport, bool isCreateTemplate,string userLogin)
        {
            string status = string.Empty;
            //List<Hre_ReportProfileHDTNotWorkEntity> lstReportProfileHDTNotWork = new List<Hre_ReportProfileHDTNotWorkEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();


                DataTable tb = GetSchema_ReportProfileHDTNotWork(nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }


                var hdtJobServices = new Hre_HDTJobServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.AddRange(new object[14]);
                listObjHDTJob[5] = lstOrgOrderNumber;
                listObjHDTJob[6] = DateFrom;
                listObjHDTJob[7] = DateTo;
                listObjHDTJob[12] = 1;
                listObjHDTJob[13] = Int32.MaxValue - 1;
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_HDTJob, userLogin, ref status).ToList();

                if (lstHDTJob == null || lstHDTJob.Count == 0)
                {
                    return tb.ConfigTable();
                }

                var lstProfileIDsByHDTJob = lstHDTJob.Select(s => s.ProfileID).Distinct().ToList();

                //var lstobjectProfileids = new List<object>();
                //string strIDs = string.Empty;
                //foreach (var item in lstProfileIDsByHDTJob)
                //{
                //    strIDs += Common.DotNetToOracle(item.ToString()) + ",";
                //}
                //if (strIDs.IndexOf(",") > 0)
                //    strIDs = strIDs.Substring(0, strIDs.Length - 1);
                //lstobjectProfileids.Add(strIDs);
                //var lstLeaveDay = basevices.GetData<Att_LeaveDayEntity>(strIDs, ConstantSql.hrm_att_sp_get_LeavedayByIds, ref status);

                List<object> para = new List<object>();
                para.AddRange(new object[3]);
                para[0] = (object)lstOrgOrderNumber;
                para[1] = DateFrom;
                para[2] = DateTo;
                var lstLeaveDay = GetData<Att_LeaveDayEntity>(para, ConstantSql.hrm_att_getdata_LeaveDay, userLogin, ref status);
                if (lstLeaveDay.Count > 0)
                {
                    lstLeaveDay = lstLeaveDay.Where(s => lstProfileIDsByHDTJob.Contains(s.ProfileID)).ToList();
                }

                if (lstLeaveDay != null && lstLeaveDay.Count > 0)
                {
                    lstLeaveDay = lstLeaveDay.Where(s => s.DateStart != null && s.DateEnd != null && s.DateStart <= DateTo && s.DateEnd >= DateFrom).ToList();
                }

                if (lstLeaveDay == null)
                {
                    return tb.ConfigTable();
                }

                List<object> listObjLeaveDayType = new List<object>();
                listObjLeaveDayType.Add(null);
                listObjLeaveDayType.Add(null);
                listObjLeaveDayType.Add(1);
                listObjLeaveDayType.Add(Int32.MaxValue - 1);
                var lstLeaveDayType = basevices.GetData<Cat_LeaveDayTypeEntity>(listObjLeaveDayType, ConstantSql.hrm_cat_sp_get_LeaveDayType, userLogin, ref status).ToList();

                Hre_ProfileServices hreService = new Hre_ProfileServices();
                lstHDTJob = hreService.getHDTJobByPrice(lstHDTJob, DateFrom, DateTo);

                foreach (var HDTJob in lstHDTJob)
                {
                    DataRow row = tb.NewRow();

                    row["ProfileName"] = HDTJob.ProfileName;
                    row["CodeEmp"] = HDTJob.CodeEmp;
                    row["E_DEPARTMENT"] = HDTJob.E_DEPARTMENT;
                    row["E_DIVISION"] = HDTJob.E_DIVISION;
                    row["E_SECTION"] = HDTJob.E_SECTION;
                    row["E_TEAM"] = HDTJob.E_TEAM;
                    row["E_UNIT"] = HDTJob.E_UNIT;
                    row["HDTJobGroupCode"] = HDTJob.HDTJobGroupCode;
                    row["HDTJobTypeCode"] = HDTJob.HDTJobTypeCode;

                    if (HDTJob.DateFrom != null)
                        row["HDTDateFrom"] = HDTJob.DateFrom;

                    if (HDTJob.DateTo != null)
                        row["HDTDateTo"] = HDTJob.DateTo;

                    if (HDTJob.Price != null)
                        row["Price"] = HDTJob.Price;
                    if (HDTJob.DateTo != null && HDTJob.DateFrom != null)
                    {
                        var leavedaybyhdt = lstLeaveDay.Where(s => s.ProfileID == HDTJob.ProfileID
                       && (s.DateStart != null && s.DateEnd != null && s.DateStart.Date <= HDTJob.DateTo.Value.Date && s.DateEnd.Date >= HDTJob.DateFrom.Value.Date)).FirstOrDefault();
                        if (lstLeaveDayType != null && leavedaybyhdt != null)
                        {
                            var leavedaytype = lstLeaveDayType.Where(s => s.ID == leavedaybyhdt.LeaveDayTypeID).FirstOrDefault();
                            row["LeaveDayTypeCode"] = leavedaytype != null ? leavedaytype.Code : null;
                        }
                    }
                    else
                    {
                        var leavedaybyhdt = lstLeaveDay.Where(s => s.ProfileID == HDTJob.ProfileID
                       && (s.DateStart != null && s.DateEnd != null && s.DateStart.Date <= HDTJob.DateFrom.Value.Date && s.DateEnd.Date >= HDTJob.DateFrom.Value.Date)).FirstOrDefault();
                        if (lstLeaveDayType != null && leavedaybyhdt != null)
                        {
                            var leavedaytype = lstLeaveDayType.Where(s => s.ID == leavedaybyhdt.LeaveDayTypeID).FirstOrDefault();
                            row["LeaveDayTypeCode"] = leavedaytype != null ? leavedaytype.Code : null;
                        }
                    }

                    tb.Rows.Add(row);
                }

                return tb.ConfigTable();
            }
        }
        #endregion


        #region BC Quản lý thực tế nhân viên làm NNĐHNH trong tháng

        DataTable CreateReportProfileHDTInMonthSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Hre_ReportProfileHDTInMonth");

                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.ProfileName);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.E_UNIT);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.E_DIVISION);

                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.E_DEPARTMENT);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.E_TEAM);

                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.E_SECTION);

                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.Unit);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.Dept);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.Part);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.Line);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.Price, typeof(double));
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.Session);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.HDTJobTypeName);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.HDTJobTypeNameHVN);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.StandardElement);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.EncryptJob);
                tb.Columns.Add(Hre_ReportProfileHDTInMonthEntity.FieldNames.HDTJobGroupName);

                for (int i = 1; i <= 31; i++)
                {
                    DataColumn column = new DataColumn(Hre_ReportProfileHDTInMonthEntity.FieldNames.Data + i);
                    column.Caption = i.ToString();
                    tb.Columns.Add(column);
                }

                return tb;
            }
        }

        public DataTable GetReportProfileHDTInMonth(DateTime Month, string lstOrgOrderNumber, List<string> lstUnit, List<string> lstDept, List<string> lstPart, bool IsCreateTemplate, string userLogin)
        {
            DataTable table = CreateReportProfileHDTInMonthSchema();
            if (IsCreateTemplate)
            {
                return table.ConfigTable();
            }
            DateTime monthStart = new DateTime(Month.Year, Month.Month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddMilliseconds(-1);
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.AddRange(new object[14]);
                listObjHDTJob[5] = lstOrgOrderNumber;
                listObjHDTJob[12] = 1;
                listObjHDTJob[13] = Int32.MaxValue - 1;
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_HDTJob, userLogin, ref status).ToList();
                //Hieu.Van: HungLe yêu cầu thêm nghiệp vụ này idTask: 19234
                //Chỉ xuất những người còn làm HDTJob trong tháng

                lstHDTJob = lstHDTJob.Where(s => s.DateFrom != null && s.DateFrom.Value.Month == Month.Month).ToList();
               
                if (lstUnit.Count() > 0)
                {
                    lstHDTJob = lstHDTJob.Where(s => lstUnit.Contains(s.Unit)).ToList();
                }
                if (lstDept.Count() > 0)
                {
                    lstHDTJob = lstHDTJob.Where(s => lstDept.Contains(s.Dept)).ToList();
                }
                if (lstPart.Count() > 0)
                {
                    lstHDTJob = lstHDTJob.Where(s => lstPart.Contains(s.Part)).ToList();
                }
                if (lstHDTJob.Count == 0)
                {
                    return table;
                }
                var lstProfileIDsByHDTJob = lstHDTJob.Select(s => s.ProfileID).ToList();
                #region DS ngày công Theo ProfileIds
                var workDayRepository = new Att_WorkDayRepository(unitOfWork);
                var lstWorkDay = new List<Att_Workday>().Select(s => new
                {
                    s.ID,
                    s.ProfileID,
                    s.WorkDate,
                    s.ShiftActual
                }).ToList();

                lstWorkDay.AddRange(workDayRepository.FindBy(s => s.IsDelete == null && s.FirstInTime != null && s.LastOutTime != null && s.WorkDate.Month == Month.Month 
                    && lstProfileIDsByHDTJob.Contains(s.ProfileID)).Select(s => new
                {
                    s.ID,
                    s.ProfileID,
                    s.WorkDate,
                    s.ShiftActual
                }).ToList());
                #endregion
                #region DS ngày nghỉ Theo ProfileIds
                var leavedayRepository = new Att_LeavedayRepository(unitOfWork);
                var lstleaveday = new List<Att_LeaveDay>().Select(s => new
                {
                    s.ID,
                    s.ProfileID,
                    s.DateStart,
                    s.DateEnd
                }).ToList();

                lstleaveday.AddRange(leavedayRepository.FindBy(s => s.IsDelete == null
                    && s.DateStart.Month <= Month.Month && s.DateEnd.Month >= Month.Month
                    && lstProfileIDsByHDTJob.Contains(s.ProfileID) && s.Status == LeaveDayStatus.E_APPROVED.ToString()).Select(s => new
                    {
                        s.ID,
                        s.ProfileID,
                        s.DateStart,
                        s.DateEnd
                    }).ToList());
                #endregion
                #region DS ca làm việc
                var shiftRepository = new Cat_ShiftRepository(unitOfWork);
                var lstShift = new List<Cat_Shift>().Select(s => new
                {
                    s.ID,
                    s.Code
                }).ToList();

                lstShift.AddRange(shiftRepository.FindBy(s => s.IsDelete == null ).Select(s => new
                    {
                        s.ID,
                        s.Code
                    }).ToList());
                #endregion
                Hre_ProfileServices hreService = new Hre_ProfileServices();
                lstHDTJob = hreService.getHDTJobByPrice(lstHDTJob, monthStart, monthEnd);
                foreach (var HDTJob in lstHDTJob)
                {
                    DataRow row = table.NewRow();
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.ProfileName] = HDTJob.ProfileName;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.CodeEmp] = HDTJob.CodeEmp;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.E_DEPARTMENT] = HDTJob.E_DEPARTMENT;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.E_DIVISION] = HDTJob.E_DIVISION;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.E_SECTION] = HDTJob.E_SECTION;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.E_TEAM] = HDTJob.E_TEAM;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.E_UNIT] = HDTJob.E_UNIT;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.Unit] = HDTJob.Unit;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.Dept] = HDTJob.Dept;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.Part] = HDTJob.Part;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.Line] = HDTJob.Line;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.Price] = HDTJob.Price != null ? HDTJob.Price : 0;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.Session] = HDTJob.Session;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.HDTJobTypeName] = HDTJob.HDTJobTypeName;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.HDTJobTypeNameHVN] = HDTJob.HDTJobTypeNameHVN;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.StandardElement] = HDTJob.StandardElement;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.EncryptJob] = HDTJob.EncryptJob;
                    row[Hre_ReportProfileHDTInMonthEntity.FieldNames.HDTJobGroupName] = HDTJob.HDTJobGroupName;
                    var workdaybypro = lstWorkDay.Where(s => s.ProfileID == HDTJob.ProfileID).ToList();
                    if (workdaybypro.Count  == 0)
                    {
                        continue;
                    }
                    for (int i = 1; i <= 31; i++)
                    {
                        var workdaybydate = workdaybypro.Where(s => s.WorkDate != null && s.WorkDate.Day == i).FirstOrDefault();
                        if (workdaybydate == null)
                        {
                            continue;
                        }
                        else
                        {
                            var leavedaybydate = lstleaveday.Where(s => s.DateStart.Day <= i && s.DateEnd.Day >= i).FirstOrDefault();
                            if (leavedaybydate != null)
                            {
                                continue;
                            }
                            else
                            {
                                var shiftbydate = lstShift.Where(s => s.ID == workdaybydate.ShiftActual).FirstOrDefault();
                                row[Hre_ReportProfileHDTInMonthEntity.FieldNames.Data + i] = shiftbydate != null ? shiftbydate.Code : null;
                            }
                        }
                    }
                    table.Rows.Add(row);
                }
            }
            return table.ConfigTable();
        }
        #endregion

        #region BC NV Cho Nghi Viec
        public DataTable CreateReportProfileWaitingStopWorkingScheme()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Hre_ReportProfileWaitingStopWorkingEntity");
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.CodeEmp, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.ProfileName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.UnitName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.UnitCode, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DepartmentName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DepartmentCode, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.SectionName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.SectionCode, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.TeamName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.TeamCode, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.PositionName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DateOfBirth, typeof(DateTime));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.Gender, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.PlaceOfBirth, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.IDPlaceOfIssue, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.PAddress, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.HomePhone, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.Cellphone, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.IDNo, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.IDDateOfIssue, typeof(DateTime));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DateHire, typeof(DateTime));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.SalaryClassName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.GraduatedLevelName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.EducationLevelName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DateQuit, typeof(DateTime));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.TypeOfStop, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.ResignReasonName, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_DEPARTMENT, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_DIVISION, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_SECTION, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_TEAM, typeof(string));
                tb.Columns.Add(Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_UNIT, typeof(string));
                return tb;
            }
        }
        public DataTable GetReportProfileWaitingStopWorking(List<Hre_ProfileEntity> lstprofile)
        {


            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (UnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportProfileWaitingStopWorkingScheme();
                if (lstprofile == null)
                    return table;
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                foreach (var profile in lstprofile)
                {
                    if (profile != null)
                    {
                        DataRow row = table.NewRow();
                        Guid? orgId = profile.OrgStructureID;
                        var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                        var orgDepartment = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                        var orgUnit = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                        var orgDivision = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                        var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                        var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                        if (profile.CodeEmp != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        if (profile.ProfileName != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.ProfileName] = profile.ProfileName;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.UnitName] = orgUnit != null ? orgUnit.OrgStructureName : string.Empty;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.UnitCode] = orgUnit != null ? orgUnit.Code : string.Empty;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DepartmentName] = orgDepartment != null ? orgDepartment.OrgStructureName : string.Empty;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DepartmentCode] = orgDepartment != null ? orgDepartment.Code : string.Empty;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_SECTION] = profile.E_SECTION;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                        row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                        if (profile.DateOfBirth != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DateOfBirth] = profile.DateOfBirth;
                        if (profile.Gender != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.Gender] = profile.Gender;
                        if (profile.PlaceOfBirth != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.PlaceOfBirth] = profile.PlaceOfBirth;
                        if (profile.IDPlaceOfIssue != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.IDPlaceOfIssue] = profile.IDPlaceOfIssue;
                        if (profile.PAddress != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.PAddress] = profile.PAddress;
                        if (profile.HomePhone != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.HomePhone] = profile.HomePhone;
                        if (profile.Cellphone != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.Cellphone] = profile.Cellphone;
                        if (profile.IDNo != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.IDNo] = profile.IDNo;
                        if (profile.IDDateOfIssue != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.IDDateOfIssue] = profile.IDDateOfIssue;
                        if (profile.DateHire != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DateHire] = profile.DateHire;
                        if (profile.SalaryClassName != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.SalaryClassName] = profile.SalaryClassName;
                        if (profile.GraduatedLevelName != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.GraduatedLevelName] = profile.GraduatedLevelName;
                        if (profile.EducationLevelName != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.EducationLevelName] = profile.EducationLevelName;
                        if (profile.DateQuit != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.DateQuit] = profile.DateQuit;
                        if (profile.TypeOfStop != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.TypeOfStop] = profile.TypeOfStop;
                        if (profile.ResignReasonName != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.ResignReasonName] = profile.ResignReasonName;
                        if (profile.PositionName != null)
                            row[Hre_ReportProfileWaitingStopWorkingEntity.FieldNames.PositionName] = profile.PositionName;
                        table.Rows.Add(row);
                    }
                }
                return table.ConfigTable(true);
            }
        }
        #endregion


        #region Báo cáo nhân viên nghỉ việc

        public DataTable GetSchema_ProfileQuitV2(String nameReport, List<Cat_ContractType> lstContractType)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add("Stt");
            tb.Columns.Add("CodeEmp");
            tb.Columns.Add("ProfileName");
            tb.Columns.Add("E_UNIT");
            tb.Columns.Add("E_DIVISION");

            tb.Columns.Add("E_DEPARTMENT");
            tb.Columns.Add("E_TEAM");
            tb.Columns.Add("E_SECTION");

            tb.Columns.Add("Position");
            tb.Columns.Add("DateOfBirth", typeof(DateTime));
            tb.Columns.Add("Gender");
            tb.Columns.Add("PlaceOfBirth");
            tb.Columns.Add("WorkPlaceName", typeof(string));
            tb.Columns.Add("PAddress");
            tb.Columns.Add("IDNo");
            tb.Columns.Add("IDDateOfIssue", typeof(DateTime));
            tb.Columns.Add("IDPlaceOfIssue");
            tb.Columns.Add("DateHire", typeof(DateTime));
            tb.Columns.Add("DateWorkFinal", typeof(DateTime));
            tb.Columns.Add("ResignNo", typeof(string));
            tb.Columns.Add("DateQuit", typeof(DateTime));
            tb.Columns.Add("ResignReasonName", typeof(string));
            tb.Columns.Add("SalaryClassName", typeof(string));
            tb.Columns.Add("E_EDUCATIONLEVEL", typeof(string));
            tb.Columns.Add("QualificationName", typeof(string));

            foreach (var item in lstContractType)
            {
                tb.Columns.Add(item.Code + "_" + "ContractNo");
                tb.Columns.Add(item.Code + "_" + "DateStart", typeof(DateTime));
                tb.Columns.Add(item.Code + "_" + "DateEnd", typeof(DateTime));
                for (int i = 1; i <= 5; i++)
                {
                    tb.Columns.Add(item.Code + "_" + i + "_" + "CodeExtend");
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateStartExtend", typeof(DateTime));
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateEndExtend", typeof(DateTime));
                }
            }
            return tb;
        }

        public DataTable GetReportProfileQuitV2(List<Guid> listOrgID, DateTime monthStart, DateTime monthEnd,
            string codeEmp, string profileName, Guid? resignReasonID, Guid? typeOfStopID, Guid? jobTitleID, Guid? positionID, Guid? workPlaceID, bool isCreateTemplate,
            string nameReport, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                var repoCat_ContractType = new CustomBaseRepository<Cat_ContractType>(unitOfWork);
                var repoHre_Contract = new CustomBaseRepository<Hre_Contract>(unitOfWork);
                var repoHre_AppendixContract = new CustomBaseRepository<Hre_AppendixContract>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);
                var lstContracType = repoCat_ContractType.FindBy(s => s.IsDelete == null).ToList();
                DataTable tb = GetSchema_ProfileQuitV2(nameReport, lstContracType);
                string status = string.Empty;
                List<object> listModel = new List<object>();
                listModel = new List<object>();
                listModel.AddRange(new object[19]);
                listModel[3] = positionID;
                listModel[6] = jobTitleID;
                listModel[8] = monthStart;
                listModel[9] = monthEnd;
                listModel[10] = resignReasonID;
                listModel[11] = typeOfStopID;
                listModel[12] = workPlaceID;
                listModel[17] = 1;
                listModel[18] = Int32.MaxValue - 1;
                List<Hre_ProfileEntity> lstProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_ProfileQuit, userLogin,ref status);

                if (!string.IsNullOrWhiteSpace(codeEmp))
                {
                    lstProfile = lstProfile.Where(d => d.CodeEmp != null && d.CodeEmp.Contains(codeEmp)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(profileName))
                {
                    lstProfile = lstProfile.Where(d => d.ProfileName.Contains(profileName)).ToList();
                }

                if (listOrgID != null && listOrgID.Count() > 0)
                {
                    lstProfile = lstProfile.Where(d => d.OrgStructureID.HasValue && listOrgID.Contains(d.OrgStructureID.Value)).ToList();
                }

                #region code BC

                string E_APPROVED = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_APPROVED.ToString();
                List<Guid> lstProfileIDs = lstProfile.Select(m => m.ID).Distinct().ToList();

                var lstContractAll = new List<Hre_Contract>().Select(m => new
                {
                    m.ID,
                    m.ContractTypeID,
                    m.ProfileID,
                    m.DateStart,
                    m.DateEnd,
                    m.ContractNo,
                    m.DateUpdate
                }).ToList();

                foreach (var lstProfileID in lstProfileIDs.Chunk(1000))
                {
                    lstContractAll.AddRange(unitOfWork.CreateQueryable<Hre_Contract>(Guid.Empty, m => m.Status == E_APPROVED
                        && lstProfileID.Contains(m.ProfileID) && m.DateStart <= monthEnd && m.DateEnd >= monthStart).Select(m => new
                        {
                            m.ID,
                            m.ContractTypeID,
                            m.ProfileID,
                            m.DateStart,
                            m.DateEnd,
                            m.ContractNo,
                            m.DateUpdate
                        }).ToList());
                }

                List<Guid> lstContractIDs = lstContractAll.Select(m => m.ID).ToList();
                var lstAppendixContract = new List<Hre_AppendixContract>().Select(m => new
                {
                    m.ID,
                    m.ContractID,
                    m.Code,
                    m.DateofEffect,
                    m.DateEndAppendixContract
                }).ToList();

                foreach (var lstContractID in lstContractIDs.Chunk(1000))
                {
                    lstAppendixContract.AddRange(unitOfWork.CreateQueryable<Hre_AppendixContract>(Guid.Empty,
                        m => lstContractID.Contains(m.ContractID)).Select(m => new
                        {
                            m.ID,
                            m.ContractID,
                            m.Code,
                            m.DateofEffect,
                            m.DateEndAppendixContract
                        }).ToList());
                }

                var lstOrg = repoCat_OrgStructure.GetAll().ToList();
                var orgTypes = repoCat_OrgStructureType.GetAll().ToList();
                int stt = 0;
                var lstContractExtend = new List<Hre_ContractExtend>().Select(s => new
                {
                    s.ContractID,
                    s.AnnexCode,
                    s.DateStart,
                    s.DateEnd
                }).ToList();

                foreach (var lstContractID in lstContractIDs.Chunk(1000))
                {
                    lstContractExtend.AddRange(unitOfWork.CreateQueryable<Hre_ContractExtend>(Guid.Empty, s => lstContractID.Contains(s.ContractID.Value)).Select(s => new
                    {
                        s.ContractID,
                        s.AnnexCode,
                        s.DateStart,
                        s.DateEnd
                    }).ToList());
                }
                foreach (var profile in lstProfile)
                {
                    stt++;
                    DataRow dr = tb.NewRow();
                    dr["Stt"] = stt;
                    dr["CodeEmp"] = profile.CodeEmp;
                    dr["ProfileName"] = profile.ProfileName;
                    dr["E_DIVISION"] = profile.E_DIVISION;
                    dr["E_UNIT"] = profile.E_UNIT;
                    dr["E_DEPARTMENT"] = profile.E_DEPARTMENT;
                    dr["E_TEAM"] = profile.E_TEAM;
                    dr["E_SECTION"] = profile.E_SECTION;
                    if (!string.IsNullOrWhiteSpace(profile.PositionName))
                    {
                        dr["Position"] = profile.PositionName;
                    }
                    if (profile.DateOfBirth != null)
                    {
                        dr["DateOfBirth"] = profile.DateOfBirth;
                    }
                    if (profile.Gender != null)
                    {
                        dr["Gender"] = profile.Gender.TranslateString();
                    }
                    if (profile.PlaceOfBirth != null)
                    {
                        dr["PlaceOfBirth"] = profile.PlaceOfBirth;
                    }
                    if (profile.WorkPlaceName != null)
                        dr["WorkPlaceName"] = profile.WorkPlaceName;
                    if (profile.PAddress != null)
                    {
                        dr["PAddress"] = profile.PAddress;
                    }
                    if (profile.IDNo != null)
                    {
                        dr["IDNo"] = profile.IDNo;
                    }
                    if (profile.IDDateOfIssue != null)
                    {
                        dr["IDDateOfIssue"] = profile.IDDateOfIssue;
                    }
                    if (profile.IDPlaceOfIssue != null)
                    {
                        dr["IDPlaceOfIssue"] = profile.IDPlaceOfIssue;
                    }
                    if (profile.DateHire != null)
                    {
                        dr["DateHire"] = profile.DateHire;
                    }
                    if (profile.DateQuit != null)
                    {
                        dr["DateWorkFinal"] = profile.DateQuit.Value.AddDays(-1);
                        dr["ResignNo"] = profile.ResignNo.ToString();
                        dr["DateQuit"] = profile.DateQuit;
                    }
                    if (!string.IsNullOrWhiteSpace(profile.SalaryClassName))
                    {
                        dr["SalaryClassName"] = profile.SalaryClassName;
                    }
                    if (!string.IsNullOrWhiteSpace(profile.ResignReasonName))
                    {
                        dr["ResignReasonName"] = profile.ResignReasonName;
                    }
                    if (!string.IsNullOrWhiteSpace(profile.EducationLevelName))
                    {
                        dr["E_EDUCATIONLEVEL"] = profile.EducationLevelName;
                    }
                    if (!string.IsNullOrWhiteSpace(profile.GraduatedLevelName))
                    {
                        dr["QualificationName"] = profile.GraduatedLevelName;
                    }
                    var lstContractByProfile = lstContractAll.Where(m => m.ProfileID == profile.ID).OrderBy(m => m.DateStart).ToList();
                    foreach (var ContractType in lstContracType)
                    {
                        var contractByContractType = lstContractByProfile.Where(m => m.ContractTypeID == ContractType.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                        if (contractByContractType != null)
                        {
                            string ContractCode = ContractType.Code;
                            if (string.IsNullOrEmpty(ContractCode))
                            {
                                continue;
                            }

                            // Son.Vo - 20150610 - sửa lại lấy thông tin gia hạn hđ. Max là 5 dòng gia hạn. - theo task 0049132
                            //if (ContractType.Type == EnumDropDown.TypeContract.E_DURATION.ToString())
                            //{
                            var lstContractByType = lstContractByProfile.Where(s => s.ContractTypeID == ContractType.ID).ToList();
                            foreach (var item in lstContractByType)
                            {
                                dr[ContractCode + "_" + "ContractNo"] = item.ContractNo;
                                if (item.DateStart != null)
                                {
                                    dr[ContractCode + "_" + "DateStart"] = item.DateStart;
                                }
                                if (contractByContractType.DateEnd != null)
                                {
                                    dr[ContractCode + "_" + "DateEnd"] = item.DateEnd;
                                }
                                //var contractByProfile_ByType = lstContractByProfile.Where(m => m.ContractTypeID == ContractType.ID).FirstOrDefault();
                                var lstContractExtendByContract = lstContractExtend.Where(s => s.ContractID == item.ID).OrderBy(s => s.DateStart).ToList();
                                int numContractExtend = 0;
                                foreach (var objContractExtend in lstContractExtendByContract)
                                {
                                    numContractExtend++;
                                    if (numContractExtend > 5)
                                        continue;
                                    if (objContractExtend != null)
                                    {
                                        if (objContractExtend.AnnexCode != null)
                                            dr[ContractCode + "_" + numContractExtend + "_" + "CodeExtend"] = objContractExtend.AnnexCode;
                                        if (objContractExtend.DateStart != null)
                                            dr[ContractCode + "_" + numContractExtend + "_" + "DateStartExtend"] = objContractExtend.DateStart;
                                        if (objContractExtend.DateEnd != null)
                                            dr[ContractCode + "_" + numContractExtend + "_" + "DateEndExtend"] = objContractExtend.DateEnd;

                                    }
                                }
                            }
                        }
                    }
                    tb.Rows.Add(dr);
                }
                #endregion

                var configs = new Dictionary<string, Dictionary<string, object>>();
                return tb.ConfigTable(configs);
            }
        }

        #endregion

        #region Hỗ trợ báo cáo thông tin nhân viên tại 1 thời điểm

        public DataTable GetSchema_ReportProfileInformationMoment(String nameReport)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add("Stt");
            tb.Columns.Add("CodeEmp");
            tb.Columns.Add("ProfileName");

            tb.Columns.Add("E_UNIT");
            tb.Columns.Add("E_DIVISION");

            tb.Columns.Add("E_DEPARTMENT");
            tb.Columns.Add("E_TEAM");
            tb.Columns.Add("E_SECTION");

            tb.Columns.Add("Position");
            tb.Columns.Add("DateOfBirth", typeof(DateTime));
            tb.Columns.Add("Gender");
            tb.Columns.Add("PlaceOfBirth");
            tb.Columns.Add("PAddress");
            tb.Columns.Add("IDNo");
            tb.Columns.Add("IDDateOfIssue", typeof(DateTime));
            tb.Columns.Add("IDPlaceOfIssue");
            tb.Columns.Add("DateHire", typeof(DateTime));
            tb.Columns.Add("CellPhone", typeof(string));
            tb.Columns.Add("HomePhone", typeof(string));


            tb.Columns.Add("ContractType", typeof(string));
            tb.Columns.Add("ContractNo", typeof(string));
            tb.Columns.Add("DateStart", typeof(DateTime));
            tb.Columns.Add("DateEnd", typeof(DateTime));
            tb.Columns.Add("SalaryClass", typeof(string));

            for (int i = 1; i <= 3; i++)
            {
                tb.Columns.Add("Appendix" + i + "_" + "Code");
                tb.Columns.Add("Appendix" + i + "_" + "DateofEffect", typeof(DateTime));
                tb.Columns.Add("Appendix" + i + "_" + "DateEndAppendixContract", typeof(DateTime));
            }
            return tb;
        }
        public DataTable GetReportProfileInformationMoment(List<Hre_ProfileEntity> lstProfile, DateTime DateCheck, bool isCreateTemplate, String nameReport, Guid? workPlaceID, Guid? salaryClassID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoCat_ContractType = new CustomBaseRepository<Cat_ContractType>(unitOfWork);
                var repoHre_Contract = new CustomBaseRepository<Hre_Contract>(unitOfWork);
                var repoHre_AppendixContract = new CustomBaseRepository<Hre_AppendixContract>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoCat_SalaryClass = new CustomBaseRepository<Cat_SalaryClass>(unitOfWork);

                var lstContracType = repoCat_ContractType.FindBy(s => s.IsDelete == null).ToList();

                DataTable tb = GetSchema_ReportProfileInformationMoment(nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }


                #region code BC
                List<Guid> lstProfileIDs = lstProfile.Select(m => m.ID).Distinct().ToList();
                string E_APPROVED = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_APPROVED.ToString();
                var lstContractAll = repoHre_Contract.GetAll().Where(m => m.Status == E_APPROVED && m.DateStart <= DateCheck && m.DateEnd >= DateCheck)
                    .Select(m => new { m.ID, m.Code, m.ProfileID, m.DateStart, m.DateEnd, m.ContractNo, m.ContractTypeID }).ToList();

                var lstProfileIDs_ByContract = lstContractAll.Select(m => m.ProfileID).Distinct().ToList();
                lstProfile = lstProfile.Where(m => lstProfileIDs_ByContract.Contains(m.ID)).ToList();

                List<Guid> lstContractIDs = lstContractAll.Select(m => m.ID).ToList();
                var bienthai = repoHre_AppendixContract.GetAll().ToList();
                var lstAppendixContract = repoHre_AppendixContract.GetAll().Where(m => lstContractIDs.Contains(m.ContractID))
                    .Select(m => new { m.ID, m.ContractID, m.Code, m.DateofEffect, m.DateEndAppendixContract }).ToList();

                var lstOrg = repoCat_OrgStructure.GetAll().ToList();
                var orgTypes = repoCat_OrgStructureType.GetAll().ToList();
                var lstPosition = repoCat_Position.GetAll().ToList();
                var lstSalaryClass = repoCat_SalaryClass.GetAll().ToList();
                if (workPlaceID != null)
                {
                    lstProfile = lstProfile.Where(s => s.WorkPlaceID == workPlaceID).ToList();
                }
                if (salaryClassID != null)
                {
                    lstProfile = lstProfile.Where(s => s.SalaryClassID == salaryClassID).ToList();
                }
                int stt = 0;
                foreach (var profile in lstProfile)
                {
                    stt++;
                    DataRow dr = tb.NewRow();
                    dr["Stt"] = stt;
                    dr["CodeEmp"] = profile.CodeEmp;
                    dr["ProfileName"] = profile.ProfileName;


                    //var orgId = profile.OrgStructureID;
                    //var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, lstOrg, orgTypes);
                    //var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, lstOrg, orgTypes);
                    //var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, lstOrg, orgTypes);
                    //var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, lstOrg, orgTypes);
                    //var orgDivision = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, lstOrg, orgTypes);
                    //var orgUnit = LibraryService.GetNearestParent(orgId, OrgUnit.E_UNIT, lstOrg, orgTypes);



                    dr["E_DIVISION"] = profile.E_DIVISION;

                    dr["E_UNIT"] = profile.E_UNIT;

                    dr["E_DEPARTMENT"] = profile.E_DEPARTMENT;

                    dr["E_TEAM"] = profile.E_TEAM;

                    dr["E_SECTION"] = profile.E_SECTION;

                    if (profile.PositionID != null)
                    {
                        var Position = lstPosition.Where(m => m.ID == profile.PositionID).FirstOrDefault();
                        if (Position != null)
                        {
                            dr["Position"] = Position.PositionName;
                        }
                    }
                    if (profile.DateOfBirth != null)
                    {
                        dr["DateOfBirth"] = profile.DateOfBirth;
                    }
                    if (profile.Cellphone != null)
                    {
                        dr["CellPhone"] = profile.Cellphone;
                    }
                    if (profile.HomePhone != null)
                    {
                        dr["HomePhone"] = profile.HomePhone;
                    }
                    if (profile.Gender != null)
                    {
                        if (profile.Gender == EnumDropDown.Gender.E_FEMALE.ToString() && profile.Gender == EnumDropDown.Gender.E_MALE.ToString() && profile.Gender == EnumDropDown.Gender.E_OTHER.ToString())
                        {
                            dr["Gender"] = EnumDropDown.GetEnumDescription<EnumDropDown.Gender>((EnumDropDown.Gender)Enum.Parse(typeof(EnumDropDown.Gender), profile.Gender.TranslateString(), true));
                        }
                        else
                        {
                            dr["Gender"] = profile.Gender;
                        }
                    }

                    if (profile.PlaceOfBirth != null)
                    {
                        dr["PlaceOfBirth"] = profile.PlaceOfBirth;
                    }
                    if (profile.PAddress != null)
                    {
                        dr["PAddress"] = profile.PAddress;
                    }
                    if (profile.IDNo != null)
                    {
                        dr["IDNo"] = profile.IDNo;
                    }
                    if (profile.IDDateOfIssue != null)
                    {
                        dr["IDDateOfIssue"] = profile.IDDateOfIssue;
                    }
                    if (profile.IDPlaceOfIssue != null)
                    {
                        dr["IDPlaceOfIssue"] = profile.IDPlaceOfIssue;
                    }
                    if (profile.DateHire != null)
                    {
                        dr["DateHire"] = profile.DateHire;
                    }

                    if (profile.DateHire != null)
                    {
                        dr["DateHire"] = profile.DateHire;
                    }

                    var SalaryClass = lstSalaryClass.Where(m => m.ID == profile.SalaryClassID).FirstOrDefault();
                    if (SalaryClass != null)
                    {
                        dr["SalaryClass"] = SalaryClass.SalaryClassName;
                    }

                    var ContractByProfile = lstContractAll.Where(m => m.ProfileID == profile.ID).FirstOrDefault();
                    if (ContractByProfile == null)
                        continue;
                    var lstAppendixContractByProfile = lstAppendixContract.Where(m => m.ContractID == ContractByProfile.ID).ToList();


                    var ContractType = lstContracType.Where(m => m.ID == ContractByProfile.ContractTypeID).FirstOrDefault();
                    if (ContractType != null)
                    {
                        dr["ContractType"] = ContractType.ContractTypeName;
                    }
                    dr["ContractNo"] = ContractByProfile.ContractNo;
                    dr["DateStart"] = ContractByProfile.DateStart;
                    dr["DateEnd"] = ContractByProfile.DateEnd;

                    var lstAppendixContractByContract = lstAppendixContractByProfile.Where(m => m.ContractID == ContractByProfile.ID).OrderBy(m => m.DateofEffect).ToList();
                    int numAppendix = 0;
                    foreach (var AppendixContract in lstAppendixContractByContract)
                    {
                        numAppendix++;
                        if (numAppendix > 3)
                            continue;
                        dr["Appendix" + numAppendix + "_" + "Code"] = AppendixContract.Code;
                        if (AppendixContract.DateofEffect != null)
                        {
                            dr["Appendix" + numAppendix + "_" + "DateofEffect"] = AppendixContract.DateofEffect;
                        }
                        if (AppendixContract.DateEndAppendixContract != null)
                        {
                            dr["Appendix" + numAppendix + "_" + "DateEndAppendixContract"] = AppendixContract.DateEndAppendixContract;
                        }
                    }
                    tb.Rows.Add(dr);
                    //HRM.Infrastructure.Utilities.EnumDropDown.TypeContract
                }
                #endregion
                var configs = new Dictionary<string, Dictionary<string, object>>();
                return tb.ConfigTable(configs);
            }
        }

        #endregion

        #region Báo cáo hợp đồng hiện tại

        public DataTable GetSchema_ReportContractCurrent(String nameReport)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add("CodeEmp");
            tb.Columns.Add("ProfileName");
            tb.Columns.Add("DateHire", typeof(DateTime));
            tb.Columns.Add("OrgStructureCode");
            tb.Columns.Add("OrgStructureName");

            return tb;
        }
        public DataTable GetReportContractCurrent(List<Hre_ProfileEntity> lstProfile, DateTime monthStart, DateTime monthEnd, bool isCreateTemplate, String nameReport)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = GetSchema_ReportContractCurrent(nameReport);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                return tb.ConfigTable(configs);
            }
        }

        #endregion

        #region Báo cáo hợp đồng hết hạn

        public DataTable GetSchema_ContractExpired(String nameReport, List<Cat_ContractType> lstContractType)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add("Stt");
            tb.Columns.Add("CodeEmp");
            tb.Columns.Add("ProfileName");

            tb.Columns.Add("E_UNIT");
            tb.Columns.Add("E_DIVISION");

            tb.Columns.Add("E_DEPARTMENT");
            tb.Columns.Add("E_TEAM");

            tb.Columns.Add("E_SECTION");

            tb.Columns.Add("Position");
            tb.Columns.Add("DateOfBirth", typeof(DateTime));
            tb.Columns.Add("Gender");
            tb.Columns.Add("PlaceOfBirth");
            tb.Columns.Add("PAddress");
            tb.Columns.Add("IDNo");
            tb.Columns.Add("IDDateOfIssue", typeof(DateTime));
            tb.Columns.Add("IDPlaceOfIssue");
            tb.Columns.Add("DateHire", typeof(DateTime));
            tb.Columns.Add("SalaryClass", typeof(string));
            tb.Columns.Add("EducationLevelName", typeof(string));
            tb.Columns.Add("StatusSynView", typeof(string));

            foreach (var item in lstContractType)
            {
                tb.Columns.Add(item.Code + "_" + "ContractNo");
                tb.Columns.Add(item.Code + "_" + "DateStart", typeof(DateTime));
                tb.Columns.Add(item.Code + "_" + "DateEnd", typeof(DateTime));
                for (int i = 1; i <= 3; i++)
                {
                    tb.Columns.Add(item.Code + "_" + i + "_" + "Code");
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateofEffect", typeof(DateTime));
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateEndAppendixContract", typeof(DateTime));
                }
                for (int i = 1; i <= 5; i++)
                {
                    tb.Columns.Add(item.Code + "_" + i + "_" + "CodeExtend");
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateStartExtend", typeof(DateTime));
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateEndExtend", typeof(DateTime));
                }
            }

            return tb;
        }
        public DataTable GetReportContractExpired(List<Hre_ProfileEntity> lstProfile, DateTime? _DateSignedStart, DateTime? _DateSignedEnd, string _ContractNo, Guid? _ContractTypeID, bool isCreateTemplate, String nameReport)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_ContractType = new CustomBaseRepository<Cat_ContractType>(unitOfWork);
                var repoHre_Contract = new CustomBaseRepository<Hre_Contract>(unitOfWork);
                var repoHre_AppendixContract = new CustomBaseRepository<Hre_AppendixContract>(unitOfWork);
                var lstContracType = repoCat_ContractType.FindBy(s => s.IsDelete == null).ToList();
                DataTable tb = GetSchema_ContractExpired(nameReport, lstContracType);
                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }
                #region code BC
                string E_APPROVED = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_APPROVED.ToString();
                var lstContractAll = repoHre_Contract.FindBy(m => m.IsDelete == null && m.Status == E_APPROVED)
                    .Select(m => new { m.ID, m.ContractTypeID, m.ProfileID, m.DateStart, m.DateEnd, m.ContractNo, m.DateSigned }).ToList();
                if (_DateSignedStart != null)
                    lstContractAll = lstContractAll.Where(s => s.DateSigned != null && s.DateSigned >= _DateSignedStart).ToList();
                if (_DateSignedEnd != null)
                    lstContractAll = lstContractAll.Where(s => s.DateSigned != null && s.DateSigned <= _DateSignedEnd).ToList();
                if (_ContractNo != string.Empty && _ContractNo != null)
                    lstContractAll = lstContractAll.Where(s => s.ContractNo != null && s.ContractNo == _ContractNo).ToList();
                if (_ContractTypeID != null && _ContractTypeID != Guid.Empty)
                    lstContractAll = lstContractAll.Where(s => s.ContractTypeID == _ContractTypeID).ToList();
                List<Guid> lstProfileIDs = lstContractAll.Select(m => m.ProfileID).Distinct().ToList();
                lstProfile = lstProfile.Where(m => lstProfileIDs.Contains(m.ID)).ToList();
                List<Guid> lstContractIDs = lstContractAll.Select(m => m.ID).ToList();
                var lstAppendixContract = repoHre_AppendixContract.GetAll().Where(m => lstContractIDs.Contains(m.ContractID))
                    .Select(m => new { m.ID, m.ContractID, m.Code, m.DateofEffect, m.DateEndAppendixContract }).ToList();
                #region Phu luc hd
                var lstContractExtend = new List<Hre_ContractExtend>().Select(s => new
                {
                    s.ContractID,
                    s.AnnexCode,
                    s.DateStart,
                    s.DateEnd
                }).ToList();
                foreach (var lstContractID in lstContractIDs.Chunk(1000))
                {
                    lstContractExtend.AddRange(unitOfWork.CreateQueryable<Hre_ContractExtend>(Guid.Empty, s => lstContractID.Contains(s.ContractID.Value)).Select(s => new
                    {
                        s.ContractID,
                        s.AnnexCode,
                        s.DateStart,
                        s.DateEnd
                    }).ToList());
                }
                #endregion
                int stt = 0;
                foreach (var profile in lstProfile)
                {
                    stt++;
                    DataRow dr = tb.NewRow();
                    dr["Stt"] = stt;
                    dr["CodeEmp"] = profile.CodeEmp;
                    dr["ProfileName"] = profile.ProfileName;
                    dr["E_DIVISION"] = profile.E_DIVISION;
                    dr["E_UNIT"] = profile.E_UNIT;
                    dr["E_DEPARTMENT"] = profile.E_DEPARTMENT;
                    dr["E_TEAM"] = profile.E_TEAM;
                    dr["E_SECTION"] = profile.E_SECTION;
                    if (profile.PositionName != null)
                    {
                        dr["Position"] = profile.PositionName;
                    }
                    if (profile.DateOfBirth != null)
                    {
                        dr["DateOfBirth"] = profile.DateOfBirth;
                    }
                    if (profile.Gender != null)
                    {
                        dr["Gender"] = profile.Gender.TranslateString();
                    }

                    if (profile.PlaceOfBirth != null)
                    {
                        dr["PlaceOfBirth"] = profile.PlaceOfBirth;
                    }
                    if (profile.PAddress != null)
                    {
                        dr["PAddress"] = profile.PAddress;
                    }
                    if (profile.IDNo != null)
                    {
                        dr["IDNo"] = profile.IDNo;
                    }
                    if (profile.IDDateOfIssue != null)
                    {
                        dr["IDDateOfIssue"] = profile.IDDateOfIssue;
                    }
                    if (profile.IDPlaceOfIssue != null)
                    {
                        dr["IDPlaceOfIssue"] = profile.IDPlaceOfIssue;
                    }
                    if (profile.DateHire != null)
                    {
                        dr["DateHire"] = profile.DateHire;
                    }
                    if (profile.DateHire != null)
                    {
                        dr["DateHire"] = profile.DateHire;
                    }
                    if (profile.SalaryClassName != null)
                    {
                        dr["SalaryClass"] = profile.SalaryClassName;
                    }
                    dr["EducationLevelName"] = profile.EducationLevelName;
                    dr["StatusSynView"] = profile.StatusSynView;

                    var lstContractByProfile = lstContractAll.Where(m => m.ProfileID == profile.ID).OrderByDescending(m => m.DateStart).ToList();
                    var lstContractByProfileID = lstContractByProfile.Select(m => m.ID).ToList();
                    var lstAppendixContractByProfile = lstAppendixContract.Where(m => lstContractByProfileID.Contains(m.ContractID)).ToList();
                    foreach (var ContractType in lstContracType)
                    {
                        string ContractCode = ContractType.Code;
                        if (string.IsNullOrEmpty(ContractCode))
                            continue;
                        var contractByProfile_ByType = lstContractByProfile.Where(m => m.ContractTypeID == ContractType.ID).FirstOrDefault();
                        if (contractByProfile_ByType != null)
                        {
                            if (contractByProfile_ByType.ContractNo != null)
                                dr[ContractCode + "_" + "ContractNo"] = contractByProfile_ByType.ContractNo;
                            if (contractByProfile_ByType.DateStart != null)
                                dr[ContractCode + "_" + "DateStart"] = contractByProfile_ByType.DateStart;
                            if (contractByProfile_ByType.DateEnd != null)
                                dr[ContractCode + "_" + "DateEnd"] = contractByProfile_ByType.DateEnd;

                            var lstAppendixContractByContract = lstAppendixContractByProfile.Where(m => m.ContractID == contractByProfile_ByType.ID).OrderBy(m => m.DateofEffect).ToList();
                            int numAppendix = 0;
                            foreach (var AppendixContract in lstAppendixContractByContract)
                            {
                                numAppendix++;
                                if (numAppendix > 3)
                                    continue;
                                if (string.IsNullOrEmpty(AppendixContract.Code))
                                    continue;
                                if (AppendixContract.Code != null)
                                    dr[ContractCode + "_" + numAppendix + "_" + "Code"] = AppendixContract.Code;
                                if (AppendixContract.DateofEffect != null)
                                    dr[ContractCode + "_" + numAppendix + "_" + "DateofEffect"] = AppendixContract.DateofEffect;
                                if (AppendixContract.DateEndAppendixContract != null)
                                    dr[ContractCode + "_" + numAppendix + "_" + "DateEndAppendixContract"] = AppendixContract.DateEndAppendixContract;
                            }
                            var lstContractExtendByContract = lstContractExtend.Where(s => s.ContractID == contractByProfile_ByType.ID).OrderBy(s => s.DateStart).ToList();
                            int numContractExtend = 0;
                            foreach (var objContractExtend in lstContractExtendByContract)
                            {
                                numContractExtend++;
                                if (numContractExtend > 5)
                                    continue;
                                if (objContractExtend.AnnexCode != null)
                                    dr[ContractCode + "_" + numContractExtend + "_" + "CodeExtend"] = objContractExtend.AnnexCode;
                                if (objContractExtend.DateStart != null)
                                    dr[ContractCode + "_" + numContractExtend + "_" + "DateStartExtend"] = objContractExtend.DateStart;
                                if (objContractExtend.DateEnd != null)
                                    dr[ContractCode + "_" + numContractExtend + "_" + "DateEndExtend"] = objContractExtend.DateEnd;
                            }
                        }
                    }
                    tb.Rows.Add(dr);
                }
                #endregion
                var configs = new Dictionary<string, Dictionary<string, object>>();
                return tb.ConfigTable(configs);
            }
        }

        #endregion

        #region Báo cáo chi tiết hợp đồng lao động

        public DataTable GetSchema_ContractDetail(String nameReport, List<Cat_ContractType> lstContractType)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add("Stt");
            tb.Columns.Add("CodeEmp");
            tb.Columns.Add("ProfileName");
            //  tb.Columns.Add("E_BRANCH");
            tb.Columns.Add("E_UNIT");
            tb.Columns.Add("E_DIVISION");

            tb.Columns.Add("E_DEPARTMENT");
            tb.Columns.Add("E_TEAM");
            tb.Columns.Add("E_SECTION");

            tb.Columns.Add("Position");
            tb.Columns.Add("DateOfBirth", typeof(DateTime));
            tb.Columns.Add("Gender");
            tb.Columns.Add("PlaceOfBirth");
            tb.Columns.Add("PAddress");
            tb.Columns.Add("IDNo");
            tb.Columns.Add("IDDateOfIssue", typeof(DateTime));
            tb.Columns.Add("IDPlaceOfIssue");
            tb.Columns.Add("DateHire", typeof(DateTime));
            tb.Columns.Add("SalaryClass", typeof(string));

            foreach (var item in lstContractType)
            {
                tb.Columns.Add(item.Code + "_" + "ContractNo");
                tb.Columns.Add(item.Code + "_" + "DateStart", typeof(DateTime));
                tb.Columns.Add(item.Code + "_" + "DateEnd", typeof(DateTime));
                for (int i = 1; i <= 3; i++)
                {
                    tb.Columns.Add(item.Code + "_" + i + "_" + "Code");
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateofEffect", typeof(DateTime));
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateEndAppendixContract", typeof(DateTime));
                }
                for (int i = 1; i <= 5; i++)
                {
                    tb.Columns.Add(item.Code + "_" + i + "_" + "CodeExtend");
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateStartExtend", typeof(DateTime));
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateEndExtend", typeof(DateTime));
                }
            }

            return tb;
        }

        public DataTable GetReportContractDetail(List<Hre_ProfileEntity> lstProfile, DateTime? _DateSignedStart, DateTime? _DateSignedEnd, string _ContractNo, Guid? _ContractTypeID, bool isCreateTemplate, String nameReport)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_ContractType = new CustomBaseRepository<Cat_ContractType>(unitOfWork);
                var repoHre_Contract = new CustomBaseRepository<Hre_Contract>(unitOfWork);
                var repoHre_AppendixContract = new CustomBaseRepository<Hre_AppendixContract>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);

                var lstContracType = repoCat_ContractType.FindBy(s => s.IsDelete == null).ToList();
                DataTable tb = GetSchema_ContractDetail(nameReport, lstContracType);

                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }

                #region code BC

                //var profileQueryable = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty,
                //    d => d.DateHire >= monthStart && d.DateHire <= monthEnd);

                //if (!string.IsNullOrWhiteSpace(codeEmp))
                //{
                //    profileQueryable = profileQueryable.Where(d => d.CodeEmp.Contains(codeEmp));
                //}

                //if (!string.IsNullOrWhiteSpace(profileName))
                //{
                //    profileQueryable = profileQueryable.Where(d => d.ProfileName.Contains(profileName));
                //}

                //if (listOrgID != null && listOrgID.Count() > 0)
                //{
                //    profileQueryable = profileQueryable.Where(d => d.OrgStructureID.HasValue && listOrgID.Contains(d.OrgStructureID.Value));
                //}

                string status = string.Empty;
                //List<object> listModel = new List<object>();

                //listModel = new List<object>();
                //listModel.AddRange(new object[16]);
                //listModel[2] = null;
                //listModel[8] = monthStart;
                //listModel[9] = monthEnd;
                //listModel[14] = null;
                //listModel[15] = Int32.MaxValue - 1;
                //List<Hre_ProfileEntity> lstProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, ref status);

                //var profileQueryable = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty,
                //    d => d.DateHire >= monthStart && d.DateHire <= monthEnd);

                //if (!string.IsNullOrWhiteSpace(codeEmp))
                //{
                //    lstProfile = lstProfile.Where(d => d.CodeEmp.Contains(codeEmp)).ToList();
                //}

                //if (!string.IsNullOrWhiteSpace(profileName))
                //{
                //    lstProfile = lstProfile.Where(d => d.ProfileName.Contains(profileName)).ToList();
                //}

                //if (listOrgID != null && listOrgID.Count() > 0)
                //{
                //    lstProfile = lstProfile.Where(d => d.OrgStructureID.HasValue && listOrgID.Contains(d.OrgStructureID.Value)).ToList();
                //}

                //List<Hre_ProfileEntity> lstProfile = profileQueryable.Select(d => new Hre_ProfileEntity
                //{
                //    ID = d.ID,
                //    CodeEmp = d.CodeEmp,
                //    CodeAttendance = d.CodeAttendance,
                //    ProfileName = d.ProfileName,
                //    DateOfBirth = d.DateOfBirth,
                //    Gender = d.Gender,
                //    PlaceOfBirth = d.PlaceOfBirth,
                //    PAddress = d.PAddress,
                //    IDNo = d.IDNo,
                //    IDDateOfIssue = d.IDDateOfIssue,
                //    IDPlaceOfIssue = d.IDPlaceOfIssue,
                //    DateHire = d.DateHire,
                //    DateQuit = d.DateQuit,
                //    StatusSyn = d.StatusSyn,
                //    PositionName = d.Cat_Position.PositionName,
                //    SalaryClassName = d.Cat_SalaryClass.SalaryClassName,
                //    OrgStructureID=d.OrgStructureID
                //}).ToList();

                List<Guid> lstProfileIDs = lstProfile.Select(m => m.ID).Distinct().ToList();
                string E_APPROVED = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_APPROVED.ToString();

                #region getdata COntract Appendix

                // string status = string.Empty;
                var hreServiceContract = new Hre_ContractServices();
                List<object> paraContract = new List<object>();
                paraContract.AddRange(new object[16]);
                paraContract[13] = E_APPROVED;
                paraContract[14] = 1;
                paraContract[15] = Int32.MaxValue - 1;

                var AppendixServices = new Hre_AppendixContractServices();
                List<object> paraAppendix = new List<object>();
                paraAppendix.AddRange(new object[7]);
                paraAppendix[5] = 1;
                paraAppendix[6] = Int32.MaxValue - 1;

                #endregion

                var lstContractAll = new List<Hre_Contract>().Select(m => new
                {
                    m.ID,
                    m.Status,
                    m.ContractTypeID,
                    m.ProfileID,
                    m.DateStart,
                    m.DateEnd,
                    m.ContractNo,
                    m.DateSigned
                }).ToList();

                foreach (var lstProfileID in lstProfileIDs.Chunk(1000))
                {
                    lstContractAll.AddRange(unitOfWork.CreateQueryable<Hre_Contract>(Guid.Empty, m => m.Status == E_APPROVED
                        && lstProfileID.Contains(m.ProfileID)).Select(m => new
                        {
                            m.ID,
                            m.Status,
                            m.ContractTypeID,
                            m.ProfileID,
                            m.DateStart,
                            m.DateEnd,
                            m.ContractNo,
                            m.DateSigned
                        }).ToList());
                }
                if (_DateSignedStart != null)
                    lstContractAll = lstContractAll.Where(s => s.DateSigned != null && s.DateSigned >= _DateSignedStart).ToList();
                if (_DateSignedEnd != null)
                    lstContractAll = lstContractAll.Where(s => s.DateSigned != null && s.DateSigned <= _DateSignedEnd).ToList();
                if (_ContractNo != string.Empty && _ContractNo != null)
                    lstContractAll = lstContractAll.Where(s => s.ContractNo != null && s.ContractNo == _ContractNo).ToList();
                if (_ContractTypeID != null && _ContractTypeID != Guid.Empty)
                    lstContractAll = lstContractAll.Where(s => s.ContractTypeID == _ContractTypeID).ToList();

                List<Guid> lstContractIDs = lstContractAll.Select(m => m.ID).ToList();

                var lstAppendixContract = new List<Hre_AppendixContractEntity>().Select(m => new
                {
                    m.ID,
                    m.ContractID,
                    m.Code,
                    m.DateofEffect,
                    m.DateEndAppendixContract
                }).ToList();

                foreach (var lstContractID in lstContractIDs.Chunk(1000))
                {
                    lstAppendixContract.AddRange(unitOfWork.CreateQueryable<Hre_AppendixContract>(Guid.Empty,
                        m => lstContractID.Contains(m.ContractID)).Select(m => new
                        {
                            m.ID,
                            m.ContractID,
                            m.Code,
                            m.DateofEffect,
                            m.DateEndAppendixContract
                        }).ToList());
                }

                var lstOrg = repoCat_OrgStructure.GetAll().ToList();
                var orgTypes = repoCat_OrgStructureType.GetAll().ToList();

                #region Phu luc hd
                var lstContractExtend = new List<Hre_ContractExtend>().Select(s => new
                {
                    s.ContractID,
                    s.AnnexCode,
                    s.DateStart,
                    s.DateEnd
                }).ToList();

                foreach (var lstContractID in lstContractIDs.Chunk(1000))
                {
                    lstContractExtend.AddRange(unitOfWork.CreateQueryable<Hre_ContractExtend>(Guid.Empty, s => lstContractID.Contains(s.ContractID.Value)).Select(s => new
                    {
                        s.ContractID,
                        s.AnnexCode,
                        s.DateStart,
                        s.DateEnd
                    }).ToList());
                }

                #endregion

                int stt = 0;
                foreach (var profile in lstProfile)
                {
                    stt++;
                    DataRow dr = tb.NewRow();
                    dr["Stt"] = stt;
                    dr["CodeEmp"] = profile.CodeEmp;
                    dr["ProfileName"] = profile.ProfileName;

                    //var orgId = profile.OrgStructureID;
                    //var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, lstOrg, orgTypes);
                    //var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, lstOrg, orgTypes);
                    //var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, lstOrg, orgTypes);
                    //var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, lstOrg, orgTypes);
                    //var orgDivision = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, lstOrg, orgTypes);
                    //var orgUnit = LibraryService.GetNearestParent(orgId, OrgUnit.E_UNIT, lstOrg, orgTypes);


                    dr["E_DIVISION"] = profile.E_DIVISION;

                    dr["E_UNIT"] = profile.E_UNIT;

                    dr["E_DEPARTMENT"] = profile.E_DEPARTMENT;

                    dr["E_TEAM"] = profile.E_TEAM;

                    dr["E_SECTION"] = profile.E_SECTION;

                    //if (orgDivision != null)
                    //{
                    //    dr["E_DIVISION"] = orgDivision.OrgStructureName;
                    //}
                    //if (orgUnit != null)
                    //{
                    //    dr["E_UNIT"] = orgUnit.OrgStructureName;
                    //}
                    //if (orgOrg != null)
                    //{
                    //    dr["E_DEPARTMENT"] = orgOrg.OrgStructureName;
                    //}
                    //if (orgTeam != null)
                    //{
                    //    dr["E_TEAM"] = orgTeam.OrgStructureName;
                    //}
                    //if (orgSection != null)
                    //{
                    //    dr["E_SECTION"] = orgSection.OrgStructureName;
                    //}
                    if (!string.IsNullOrWhiteSpace(profile.PositionName))
                    {
                        dr["Position"] = profile.PositionName;
                    }
                    if (profile.DateOfBirth != null)
                    {
                        dr["DateOfBirth"] = profile.DateOfBirth;
                    }
                    if (profile.Gender != null)
                    {
                        dr["Gender"] = profile.Gender.TranslateString();
                    }

                    if (profile.PlaceOfBirth != null)
                    {
                        dr["PlaceOfBirth"] = profile.PlaceOfBirth;
                    }
                    if (profile.PAddress != null)
                    {
                        dr["PAddress"] = profile.PAddress;
                    }
                    if (profile.IDNo != null)
                    {
                        dr["IDNo"] = profile.IDNo;
                    }
                    if (profile.IDDateOfIssue != null)
                    {
                        dr["IDDateOfIssue"] = profile.IDDateOfIssue;
                    }
                    if (profile.IDPlaceOfIssue != null)
                    {
                        dr["IDPlaceOfIssue"] = profile.IDPlaceOfIssue;
                    }
                    if (profile.DateHire != null)
                    {
                        dr["DateHire"] = profile.DateHire;
                    }

                    if (!string.IsNullOrWhiteSpace(profile.SalaryClassName))
                    {
                        dr["SalaryClass"] = profile.SalaryClassName;
                    }
                    var lstContractByProfile = lstContractAll.Where(m => m.ProfileID == profile.ID).OrderBy(m => m.DateStart).ToList();
                    var lstContractByProfileID = lstContractByProfile.Select(m => m.ID).ToList();
                    var lstAppendixContractByProfile = lstAppendixContract.Where(m => lstContractByProfileID.Contains(m.ContractID)).ToList();

                    foreach (var ContractType in lstContracType)
                    {
                        string ContractCode = ContractType.Code;
                        if (string.IsNullOrEmpty(ContractCode))
                            continue;

                        var ContractOfProfileByType = lstContractByProfile.Where(m => m.ContractTypeID == ContractType.ID).FirstOrDefault();
                        if (ContractOfProfileByType != null)
                        {
                            if (ContractOfProfileByType.ContractNo != null)
                                dr[ContractCode + "_" + "ContractNo"] = ContractOfProfileByType.ContractNo;
                            if (ContractOfProfileByType.DateStart != null)
                                dr[ContractCode + "_" + "DateStart"] = ContractOfProfileByType.DateStart;
                            if (ContractOfProfileByType.DateEnd != null)
                                dr[ContractCode + "_" + "DateEnd"] = ContractOfProfileByType.DateEnd;

                            var lstAppendixContractByContract = lstAppendixContractByProfile.Where(m => m.ContractID == ContractOfProfileByType.ID).OrderBy(m => m.DateofEffect).ToList();
                            int numAppendix = 0;
                            foreach (var AppendixContract in lstAppendixContractByContract)
                            {
                                numAppendix++;
                                if (numAppendix > 3)
                                    continue;
                                if (string.IsNullOrEmpty(AppendixContract.Code))
                                    continue;
                                dr[ContractCode + "_" + numAppendix + "_" + "Code"] = AppendixContract.Code;
                                if (AppendixContract.DateofEffect != null)
                                {
                                    dr[ContractCode + "_" + numAppendix + "_" + "DateofEffect"] = AppendixContract.DateofEffect;
                                }
                                if (AppendixContract.DateEndAppendixContract != null)
                                {
                                    dr[ContractCode + "_" + numAppendix + "_" + "DateEndAppendixContract"] = AppendixContract.DateEndAppendixContract;
                                }
                            }
                            var lstContractExtendByContract = lstContractExtend.Where(s => s.ContractID == ContractOfProfileByType.ID).OrderBy(s => s.DateStart).ToList();
                            int numContractExtend = 0;
                            foreach (var objContractExtend in lstContractExtendByContract)
                            {
                                numContractExtend++;
                                if (numContractExtend > 5)
                                    continue;
                                if (objContractExtend.AnnexCode != null)
                                    dr[ContractCode + "_" + numContractExtend + "_" + "CodeExtend"] = objContractExtend.AnnexCode;
                                if (objContractExtend.DateStart != null)
                                    dr[ContractCode + "_" + numContractExtend + "_" + "DateStartExtend"] = objContractExtend.DateStart;
                                if (objContractExtend.DateEnd != null)
                                    dr[ContractCode + "_" + numContractExtend + "_" + "DateEndExtend"] = objContractExtend.DateEnd;
                            }
                        }
                    }
                    tb.Rows.Add(dr);
                }
                #endregion
                var configs = new Dictionary<string, Dictionary<string, object>>();
                return tb.ConfigTable(configs);
            }
        }


        #endregion

        #region Báo cáo thông tin nhân viên

        public DataTable GetSchema_ProfileInformation(String nameReport, List<Cat_ContractType> lstContractType)
        {
            DataTable tb = new DataTable(nameReport);
            tb.Columns.Add("Stt");
            tb.Columns.Add("CodeEmp");
            tb.Columns.Add("ProfileName");

            tb.Columns.Add("NameFamily");
            tb.Columns.Add("FirstName");
            tb.Columns.Add("CodeAttendance");
            tb.Columns.Add("DateApplyAttendanceCode");
            tb.Columns.Add("DateHire");
            tb.Columns.Add("DateEndProbation");
            tb.Columns.Add("CodeTax");
            tb.Columns.Add("DateOfIssuedTaxCode");
            tb.Columns.Add("ContractTypeID");
            tb.Columns.Add("IDNo");
            tb.Columns.Add("IDDateOfIssue");
            tb.Columns.Add("IDPlaceOfIssue");
            tb.Columns.Add("Origin");
            tb.Columns.Add("Gender");
            tb.Columns.Add("BirthDay");
            tb.Columns.Add("DayOfBirth");
            tb.Columns.Add("MonthOfBirth");
            tb.Columns.Add("YearOfBirth");
            tb.Columns.Add("PlaceOfBirth");
            tb.Columns.Add("CountryName");
            tb.Columns.Add("NameEnglish");
            tb.Columns.Add("Cat_ReqDocument");
            tb.Columns.Add("MarriageStatus");
            tb.Columns.Add("Cat_NameEntity-Name");
            tb.Columns.Add("Cat_NameEntity1-Name");
            tb.Columns.Add("Cat_Region-Name");
            tb.Columns.Add("Cat_EthnicGroup-Name");
            tb.Columns.Add("Cat_Religion-Name");
            tb.Columns.Add("Height(Cm)");
            tb.Columns.Add("Weight(Kg)");
            tb.Columns.Add("BloodType");
            tb.Columns.Add("SocialInsIssuePlace_WorkPlace");
            tb.Columns.Add("Notes");
            tb.Columns.Add("Email");
            tb.Columns.Add("Email2");
            tb.Columns.Add("Cellphone");

            tb.Columns.Add("ContactPhone1");
            tb.Columns.Add("ContactPhone2");
            tb.Columns.Add("Cat_Country1-Name");
            tb.Columns.Add("Cat_Province8-Name");
            tb.Columns.Add("Cat_Distric3-Name");
            tb.Columns.Add("Cat_Village-Name");
            tb.Columns.Add("Taddress");
            tb.Columns.Add("Cat_Contry2-Name");
            tb.Columns.Add("Cat_Province7-Name");
            tb.Columns.Add("Cat_District1-Name");
            tb.Columns.Add("Cat_Village1-Name");
            tb.Columns.Add("Paddress");
            tb.Columns.Add("AddressEmergency");
            tb.Columns.Add("DateOfEffect");


            tb.Columns.Add("E_UNIT");
            tb.Columns.Add("E_DIVISION");

            tb.Columns.Add("Cat_OrgStructure-Name");
            tb.Columns.Add("E_TEAM");
            tb.Columns.Add("E_SECTION");

            tb.Columns.Add("Cat_Shop-Name");
            tb.Columns.Add("Cat_JobTitle-Name");
            tb.Columns.Add("Cat_Position-Name");
            tb.Columns.Add("Hre_Profile2-CodeEmp");
            tb.Columns.Add("Hre_Profile3-CodeEmp");
            tb.Columns.Add("Cat_EmployeeType-Name");
            tb.Columns.Add("Cat_PayrollGroup-Name");
            tb.Columns.Add("Cat_SalaryClassType-Name");
            tb.Columns.Add("Cat_CostCentre-Name");
            tb.Columns.Add("Cat_WorkPlace-WorkPlaceName");
            tb.Columns.Add("PassportNo");
            tb.Columns.Add("PassportPlaceOfIssue");
            tb.Columns.Add("PassportDateOfIssue");
            tb.Columns.Add("PassportDateOfExpiry");
            tb.Columns.Add("WorkPermitStatus");
            tb.Columns.Add("WorkPermitNo");
            tb.Columns.Add("WorkPermitInsDate");
            tb.Columns.Add("WorkPermitExpiredDate");
            tb.Columns.Add("IsRegisterSocialIns");
            tb.Columns.Add("SocialInsNo");
            tb.Columns.Add("SocialInsOldNo");
            tb.Columns.Add("SocialInsIssueDate");
            tb.Columns.Add("SocialInsDateReg");
            tb.Columns.Add("SocialInsIssuePlace");
            tb.Columns.Add("ReceiveSocialIns");
            tb.Columns.Add("ReceiveSocialInsDate");
            tb.Columns.Add("SocialInsSubmitBookStatus");
            tb.Columns.Add("SocialInsSubmitBookDate");
            tb.Columns.Add("IsRegisterHealthIns");
            tb.Columns.Add("HealthInsNo");
            tb.Columns.Add("HealthInsIssueDate");
            tb.Columns.Add("HealthInsExpiredDate");
            tb.Columns.Add("HealthTreatmentPlace");
            tb.Columns.Add("HealthTreatmentPlaceCode");
            tb.Columns.Add("IsRegisterUnEmploymentIns");
            tb.Columns.Add("UnEmpInsDateReg");
            tb.Columns.Add("UnEmpInsCountMonthOld");
            tb.Columns.Add("CommentIns");
            tb.Columns.Add("StatusSyn");
            tb.Columns.Add("TypeOfStop");
            //   tb.Columns.Add("TypeOfStop").TypeOfStop;
            tb.Columns.Add("TypeSuspense");
            //   tb.Columns.Add("TypeSuspense").TypeSuspense;
            tb.Columns.Add("Cat_ResignReason-Name");
            tb.Columns.Add("RequestDate");
            tb.Columns.Add("DateQuitRequest");
            tb.Columns.Add("ProfileSign");
            tb.Columns.Add("DateQuit");
            tb.Columns.Add("ResignNo");
            tb.Columns.Add("DateQuitSign");
            tb.Columns.Add("ReceiveHealthIns");
            tb.Columns.Add("ReceiveHealthInsDate");
            tb.Columns.Add("IsBlackList");
            tb.Columns.Add("ResonBackList");

            //  tb.Columns.Add("Position");
            tb.Columns.Add("DateOfBirth", typeof(DateTime));
            //tb.Columns.Add("Gender");
            //tb.Columns.Add("PlaceOfBirth");
            //tb.Columns.Add("PAddress");
            //tb.Columns.Add("IDNo");
            //tb.Columns.Add("IDDateOfIssue", typeof(DateTime));
            //tb.Columns.Add("IDPlaceOfIssue");
            //tb.Columns.Add("DateHire", typeof(DateTime));
            //tb.Columns.Add("SalaryClass", typeof(string));
            //tb.Columns.Add("Education", typeof(string));
            //tb.Columns.Add("StatusSyn", typeof(string));
            //tb.Columns.Add("CostCentreName");
            //tb.Columns.Add("EthnicGroupName");
            //tb.Columns.Add("QualificationName");

            foreach (var item in lstContractType)
            {
                tb.Columns.Add(item.Code + "_" + "ContractNo");
                tb.Columns.Add(item.Code + "_" + "DateStart", typeof(String));
                tb.Columns.Add(item.Code + "_" + "DateEnd", typeof(String));
                for (int i = 1; i <= 3; i++)
                {
                    tb.Columns.Add(item.Code + "_" + i + "_" + "Code");
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateofEffect", typeof(String));
                    tb.Columns.Add(item.Code + "_" + i + "_" + "DateEndAppendixContract", typeof(String));
                }
            }

            return tb;
        }
        public DataTable GetReportProfileInformation(List<Guid> listOrgID, DateTime monthStart, DateTime monthEnd,
            string codeEmp, string profileName, bool isCreateTemplate, string nameReport, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var objContractExtend = new List<object>();
                objContractExtend.AddRange(new object[10]);
                objContractExtend[8] = 1;
                objContractExtend[9] = int.MaxValue - 1;
                var lstContractExtend = GetData<Hre_ContractExtendEntity>(objContractExtend, ConstantSql.hrm_hr_sp_get_ContractExtendList, userLogin, ref status).ToList();
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                var repoCat_ContractType = new CustomBaseRepository<Cat_ContractType>(unitOfWork);
                var repoHre_Contract = new CustomBaseRepository<Hre_Contract>(unitOfWork);
                var repoHre_AppendixContract = new CustomBaseRepository<Hre_AppendixContract>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);

                var lstContracType = repoCat_ContractType.FindBy(s => s.IsDelete == null).ToList();
                DataTable tb = GetSchema_ProfileInformation(nameReport, lstContracType);

                if (isCreateTemplate)
                {
                    return tb.ConfigTable();
                }
              
                List<object> listModel = new List<object>();

                listModel = new List<object>();
                listModel.AddRange(new object[18]);
                listModel[16] = 1;
                listModel[17] = Int32.MaxValue - 1;
                List<Hre_ProfileEntity> lstProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, userLogin, ref status);

                //var profileQueryable = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty,
                //    d => d.DateHire >= monthStart && d.DateHire <= monthEnd);

                if (!string.IsNullOrWhiteSpace(codeEmp))
                {
                    lstProfile = lstProfile.Where(d => d.CodeEmp.Contains(codeEmp)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(profileName))
                {
                    lstProfile = lstProfile.Where(d => d.ProfileName.Contains(profileName)).ToList();
                }

                if (listOrgID != null && listOrgID.Count() > 0)
                {
                    lstProfile = lstProfile.Where(d => d.OrgStructureID.HasValue && listOrgID.Contains(d.OrgStructureID.Value)).ToList();
                }

                lstProfile = lstProfile.Where(d => (DateTime?)d.DateHire >= monthStart && (DateTime?)d.DateHire <= monthEnd).ToList();





                //var profileQueryable = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty,
                //    d => d.DateHire >= monthStart && d.DateHire <= monthEnd);

                //if (!string.IsNullOrWhiteSpace(codeEmp))
                //{
                //    profileQueryable = profileQueryable.Where(d => d.CodeEmp.Contains(codeEmp));
                //}

                //if (!string.IsNullOrWhiteSpace(profileName))
                //{
                //    profileQueryable = profileQueryable.Where(d => d.ProfileName.Contains(profileName));
                //}

                //if (listOrgID != null && listOrgID.Count() > 0)
                //{
                //    profileQueryable = profileQueryable.Where(d => d.OrgStructureID.HasValue && listOrgID.Contains(d.OrgStructureID.Value));
                //}

                //List<Hre_ProfileEntity> lstProfile = profileQueryable.Select(d => new Hre_ProfileEntity
                //{
                //    ID = d.ID,
                //    CodeEmp = d.CodeEmp,
                //    CodeAttendance = d.CodeAttendance,
                //    ProfileName = d.ProfileName,
                //    DateOfBirth = d.DateOfBirth,
                //    Gender = d.Gender,
                //    PlaceOfBirth = d.PlaceOfBirth,
                //    PAddress = d.PAddress,
                //    IDNo = d.IDNo,
                //    IDDateOfIssue = d.IDDateOfIssue,
                //    IDPlaceOfIssue = d.IDPlaceOfIssue,
                //    DateHire = d.DateHire,
                //    DateQuit = d.DateQuit,
                //    StatusSyn = d.StatusSyn,
                //    PositionName = d.Cat_Position.PositionName,
                //    SalaryClassName = d.Cat_SalaryClass.SalaryClassName,
                //    EducationLevelName = d.Cat_NameEntity.NameEntityName,
                //    GraduatedLevelName = d.Cat_NameEntity1.NameEntityName,
                //    CostCentreName = d.Cat_CostCentre.CostCentreName,
                //    EthnicGroupName = d.Cat_EthnicGroup.EthnicGroupName,
                //    OrgStructureID=d.OrgStructureID


                //}).ToList();

                #region code BC
                List<Guid> lstProfileIDs = lstProfile.Select(m => m.ID).Distinct().ToList();
                string E_APPROVED = HRM.Infrastructure.Utilities.EnumDropDown.Status.E_APPROVED.ToString();

                var lstContractAll = new List<Hre_Contract>().Select(m => new
                {
                    m.ID,
                    m.ContractTypeID,
                    m.ProfileID,
                    m.DateStart,
                    m.DateEnd,
                    m.ContractNo
                }).ToList();

                foreach (var lstProfileID in lstProfileIDs.Chunk(1000))
                {
                    lstContractAll.AddRange(unitOfWork.CreateQueryable<Hre_Contract>(Guid.Empty, m => m.Status == E_APPROVED
                        && lstProfileID.Contains(m.ProfileID) && m.DateStart <= monthEnd && m.DateEnd >= monthStart).Select(m => new
                        {
                            m.ID,
                            m.ContractTypeID,
                            m.ProfileID,
                            m.DateStart,
                            m.DateEnd,
                            m.ContractNo
                        }).ToList());
                }

                List<Guid> lstContractIDs = lstContractAll.Select(m => m.ID).ToList();
                var lstAppendixContract = new List<Hre_AppendixContract>().Select(m => new
                {
                    m.ID,
                    m.ContractID,
                    m.Code,
                    m.DateofEffect,
                    m.DateEndAppendixContract
                }).ToList();

                foreach (var lstContractID in lstContractIDs.Chunk(1000))
                {
                    lstAppendixContract.AddRange(unitOfWork.CreateQueryable<Hre_AppendixContract>(Guid.Empty,
                        m => lstContractID.Contains(m.ContractID)).Select(m => new
                        {
                            m.ID,
                            m.ContractID,
                            m.Code,
                            m.DateofEffect,
                            m.DateEndAppendixContract
                        }).ToList());
                }


                var lstOrg = repoCat_OrgStructure.GetAll().ToList();
                var orgTypes = repoCat_OrgStructureType.GetAll().ToList();


                int stt = 0;
                foreach (var profile in lstProfile)
                {
                    stt++;
                    
                    DataRow dr = tb.NewRow();
                    dr["Stt"] = stt;
                    dr["CodeEmp"] = profile.CodeEmp;
                    dr["ProfileName"] = profile.ProfileName;

                    dr["NameFamily"] = profile.NameFamily;
                    dr["FirstName"] = profile.FirstName;
                    dr["CodeAttendance"] = profile.CodeAttendance;
                    dr["DateApplyAttendanceCode"] = profile.DateApplyAttendanceCode != null ? profile.DateApplyAttendanceCode.Value.ToString("dd/MM/yyyy") : null;
                    dr["DateHire"] = profile.DateHire != null ? profile.DateHire.Value.ToString("dd/MM/yyyy") : null;
                    dr["DateEndProbation"] = profile.DateEndProbation != null ? profile.DateEndProbation.Value.ToString("dd/MM/yyyy") : null;
                    dr["CodeTax"] = profile.CodeTax;
                    dr["DateOfIssuedTaxCode"] = profile.DateOfIssuedTaxCode != null ? profile.DateOfIssuedTaxCode.Value.ToString("dd/MM/yyyy") : null;
                    dr["ContractTypeID"] = profile.ContractTypeID;
                    dr["IDNo"] = profile.IDNo;
                    dr["IDDateOfIssue"] = profile.IDDateOfIssue != null ? profile.IDDateOfIssue.Value.ToString("dd/MM/yyyy") : null;
                    dr["IDPlaceOfIssue"] = profile.IDPlaceOfIssue;
                    dr["Origin"] = profile.Origin;
                    dr["Gender"] = profile.Gender;
                    dr["BirthDay"] = profile.Birthday;
                    dr["DayOfBirth"] = profile.DayOfBirth;
                    dr["MonthOfBirth"] = profile.MonthOfBirth;
                    dr["YearOfBirth"] = profile.YearOfBirth;
                    dr["PlaceOfBirth"] = profile.PlaceOfBirth;
                    dr["CountryName"] = profile.NationalityName;
                    dr["NameEnglish"] = profile.NameEnglish;
                    dr["Cat_ReqDocument"] = profile.StoredDocuments;
                    dr["MarriageStatus"] = profile.MarriageStatus;
                    dr["Cat_NameEntity-Name"] = profile.GraduatedLevelName;
                    dr["Cat_NameEntity1-Name"] = profile.EducationLevelName;
                    dr["Cat_Region-Name"] = profile.RegionName;
                    dr["Cat_EthnicGroup-Name"] = profile.EthnicGroupName;
                    dr["Cat_Religion-Name"] = profile.ReligionName;
                    dr["Height(Cm)"] = profile.Height;
                    dr["Weight(Kg)"] = profile.Weight;
                    dr["BloodType"] = profile.BloodType;
                    dr["SocialInsIssuePlace_WorkPlace"] = profile.WorkPlaceName;

                    dr["Notes"] = profile.Notes;
                    dr["Email"] = profile.Email;
                    dr["Email2"] = profile.Email2;
                    dr["Cellphone"] = profile.Cellphone;
                    dr["ContactPhone1"] = profile.HomePhone;
                    dr["ContactPhone2"] = profile.BusinessPhone;
                    dr["Cat_Country1-Name"] = profile.TCountryName;
                    dr["Cat_Province8-Name"] = profile.TProvinceName;
                    dr["Cat_Distric3-Name"] = profile.TDistrictName;
                    dr["Cat_Village-Name"] = profile.TVillageName;
                    dr["Taddress"] = profile.TAddress;
                    dr["Cat_Contry2-Name"] = profile.PCountryName;
                    dr["Cat_Province7-Name"] = profile.PProvinceName;
                    dr["Cat_District1-Name"] = profile.PDistrictName;
                    dr["Cat_Village1-Name"] = profile.PVillageName;
                    dr["Paddress"] = profile.PAddress;
                    dr["AddressEmergency"] = profile.AddressEmergency;
                    dr["DateOfEffect"] = profile.DateOfEffect != null ? profile.DateOfEffect.Value.ToString("dd/MM/yyyy") : null;






                    dr["Cat_OrgStructure-Name"] = profile.E_DEPARTMENT;

                    dr["E_UNIT"] = profile.E_UNIT;

                    dr["E_TEAM"] = profile.E_TEAM;

                    dr["E_DIVISION"] = profile.E_DIVISION;

                    dr["E_SECTION"] = profile.E_SECTION;


                    dr["Cat_Shop-Name"] = profile.ShopName;
                    dr["Cat_JobTitle-Name"] = profile.JobTitleName;
                    dr["Cat_Position-Name"] = profile.PositionName;
                    dr["Hre_Profile2-CodeEmp"] = profile.SupervisorName;
                    dr["Hre_Profile3-CodeEmp"] = profile.HighSupervisorName;
                    dr["Cat_EmployeeType-Name"] = profile.EmployeeTypeName;
                    dr["Cat_PayrollGroup-Name"] = profile.PayrollGroupName;
                    dr["Cat_SalaryClassType-Name"] = profile.SalaryClassCode;
                    dr["Cat_CostCentre-Name"] = profile.CostCentreCode;
                    dr["Cat_WorkPlace-WorkPlaceName"] = profile.WorkPlaceName;
                    dr["PassportNo"] = profile.PassportNo;
                    dr["PassportPlaceOfIssue"] = profile.PassportPlaceOfIssue;
                    dr["PassportDateOfIssue"] = profile.PassportDateOfIssue != null ? profile.PassportDateOfIssue.Value.ToString("dd/MM/yyyy") : null;
                    dr["PassportDateOfExpiry"] = profile.PassportDateOfExpiry;
                    dr["WorkPermitStatus"] = profile.WorkPermitStatus;
                    dr["WorkPermitNo"] = profile.WorkPermitNo;
                    dr["WorkPermitInsDate"] = profile.WorkPermitInsDate != null ? profile.WorkPermitInsDate.Value.ToString("dd/MM/yyyy") : null;
                    dr["WorkPermitExpiredDate"] = profile.WorkPermitExpiredDate != null ? profile.WorkPermitExpiredDate.Value.ToString("dd/MM/yyyy") : null;
                    dr["IsRegisterSocialIns"] = profile.IsRegisterSocialIns;
                    dr["SocialInsNo"] = profile.SocialInsNo;
                    dr["SocialInsOldNo"] = profile.SocialInsOldNo;
                    dr["SocialInsIssueDate"] = profile.SocialInsIssueDate != null ? profile.SocialInsIssueDate.Value.ToString("dd/MM/yyyy") : null;
                    dr["SocialInsDateReg"] = profile.SocialInsDateReg != null ? profile.SocialInsDateReg.Value.ToString("dd/MM/yyyy") : null;
                    dr["SocialInsIssuePlace"] = profile.SocialInsIssuePlace;
                    dr["ReceiveSocialIns"] = profile.ReceiveSocialIns;
                    dr["ReceiveSocialInsDate"] = profile.ReceiveSocialInsDate != null ? profile.ReceiveSocialInsDate.Value.ToString("dd/MM/yyyy") : null;
                    dr["SocialInsSubmitBookStatus"] = profile.SocialInsSubmitBookStatus;
                    dr["SocialInsSubmitBookDate"] = profile.SocialInsSubmitBookDate != null ? profile.SocialInsSubmitBookDate.Value.ToString("dd/MM/yyyy") : null;
                    dr["IsRegisterHealthIns"] = profile.IsRegisterHealthIns;
                    dr["HealthInsNo"] = profile.HealthInsNo;
                    dr["HealthInsIssueDate"] = profile.HealthInsIssueDate != null ? profile.HealthInsIssueDate.Value.ToString("dd/MM/yyyy") : null;
                    dr["HealthInsExpiredDate"] = profile.HealthInsExpiredDate != null ? profile.HealthInsExpiredDate.Value.ToString("dd/MM/yyyy") : null;
                    dr["HealthTreatmentPlace"] = profile.HealthTreatmentPlace;
                    dr["HealthTreatmentPlaceCode"] = profile.HealthTreatmentPlaceCode;
                    dr["IsRegisterUnEmploymentIns"] = profile.IsRegisterUnEmploymentIns;
                    dr["UnEmpInsDateReg"] = profile.UnEmpInsDateReg != null ? profile.UnEmpInsDateReg.Value.ToString("dd/MM/yyyy") : null;
                    dr["UnEmpInsCountMonthOld"] = profile.UnEmpInsCountMonthOld;
                    dr["CommentIns"] = profile.CommentIns;
                    dr["StatusSyn"] = profile.StatusSyn;
                    dr["TypeOfStop"] = profile.TypeOfStop;
                    //  dr["TypeOfStop"] = profile.TypeOfStop;
                    dr["TypeSuspense"] = profile.TypeSuspense;
                    //  dr["TypeSuspense"] = profile.TypeSuspense;
                    dr["Cat_ResignReason-Name"] = profile.ResignReasonName;
                    dr["RequestDate"] = profile.RequestDate != null ? profile.RequestDate.Value.ToString("dd/MM/yyyy") : null;
                    dr["DateQuitRequest"] = profile.DateQuitRequest != null ? profile.DateQuitRequest.Value.ToString("dd/MM/yyyy") : null;
                    dr["ProfileSign"] = profile.ProfileSign;
                    dr["DateQuit"] = profile.DateQuit != null ? profile.DateQuit.Value.ToString("dd/MM/yyyy") : null;
                    dr["ResignNo"] = profile.ResignNo;
                    dr["DateQuitSign"] = profile.DateQuitSign != null ? profile.DateQuitSign.Value.ToString("dd/MM/yyyy") : null;
                    dr["ReceiveHealthIns"] = profile.ReceiveHealthIns;
                    dr["ReceiveHealthInsDate"] = profile.ReceiveHealthInsDate;
                    dr["IsBlackList"] = profile.IsBlackList;
                    dr["ResonBackList"] = profile.ResonBackList;

                    if (profile.DateOfBirth != null)
                    {
                        dr["DateOfBirth"] = profile.DateOfBirth.Value;
                    }

                    var lstContractByProfile = lstContractAll.Where(m => m.ProfileID == profile.ID).OrderBy(m => m.DateStart).ToList();
                    var lstContractByProfileID = lstContractByProfile.Select(m => m.ID).ToList();
                    var lstAppendixContractByProfile = lstAppendixContract.Where(m => lstContractByProfileID.Contains(m.ContractID)).ToList();
                    var lstContractExtendByContractID = lstContractExtend.Where(s => s.ContractID != null && lstContractByProfileID.Contains((Guid)s.ContractID)).ToList();
                    foreach (var ContractType in lstContracType)
                    {
                        string ContractCode = ContractType.Code;
                        if (string.IsNullOrEmpty(ContractCode))
                            continue;

                        var ContractOfProfileByType = lstContractByProfile.Where(m =>
                            m.ContractTypeID == ContractType.ID).FirstOrDefault();

                        if (ContractOfProfileByType != null)
                        {
                            dr[ContractCode + "_" + "ContractNo"] = ContractOfProfileByType.ContractNo;
                            dr[ContractCode + "_" + "DateStart"] = ContractOfProfileByType.DateStart != null ? ContractOfProfileByType.DateStart.ToString("dd/MM/yyyy") : null;
                            dr[ContractCode + "_" + "DateEnd"] = ContractOfProfileByType.DateEnd != null ? ContractOfProfileByType.DateEnd.Value.ToString("dd/MM/yyyy") : null;

                            var lstAppendixContractByContract = lstAppendixContractByProfile.Where(m =>
                                m.ContractID == ContractOfProfileByType.ID).OrderBy(m => m.DateofEffect).ToList();

                            int numAppendix = 0;
                            foreach (var AppendixContract in lstAppendixContractByContract)
                            {
                                numAppendix++;
                                if (numAppendix > 3)
                                    continue;
                                if (string.IsNullOrEmpty(AppendixContract.Code))
                                    continue;
                                dr[ContractCode + "_" + numAppendix + "_" + "Code"] = AppendixContract.Code;
                                if (AppendixContract.DateofEffect != null)
                                {
                                    dr[ContractCode + "_" + numAppendix + "_" + "DateofEffect"] = AppendixContract.DateofEffect;
                                }
                                if (AppendixContract.DateEndAppendixContract != null)
                                {
                                    dr[ContractCode + "_" + numAppendix + "_" + "DateEndAppendixContract"] = AppendixContract.DateEndAppendixContract;
                                }
                            }
                        }
                    }
                    if (lstContractExtendByContractID.Count > 0)
                    {
                        var count = 0;
                        foreach (var contractExtend in lstContractExtendByContractID)
                        {
                            count++;
                            var annexCodeTitle = "AnnexCode" + count;
                            var dateStartTitle = "DateStart" + count;
                            var dateEndTitle = "DateEnd" + count;

                            if (!tb.Columns.Contains(annexCodeTitle))
                            {
                                tb.Columns.Add(annexCodeTitle);
                            }

                            if (!tb.Columns.Contains(dateStartTitle))
                            {
                                tb.Columns.Add(dateStartTitle, typeof(DateTime));
                            }

                            if (!tb.Columns.Contains(dateEndTitle))
                            {
                                tb.Columns.Add(dateEndTitle, typeof(DateTime));
                            }

                            if (tb.Columns.Contains(annexCodeTitle))
                            {
                                dr[annexCodeTitle] = contractExtend.AnnexCode;
                            }
                            if (tb.Columns.Contains(dateStartTitle))
                            {
                                if (contractExtend.DateStart != null)
                                {
                                    dr[dateStartTitle] = contractExtend.DateStart;
                                }
                            }
                            if (tb.Columns.Contains(dateEndTitle))
                            {
                                if (contractExtend.DateEnd != null)
                                {
                                    dr[dateEndTitle] = contractExtend.DateEnd;
                                }
                            }
                        }
                    }
                    tb.Rows.Add(dr);
                }
                #endregion
                var configs = new Dictionary<string, Dictionary<string, object>>();
                return tb.ConfigTable(configs);
            }
        }

        #endregion

        #region BC tình hình nhận hiện vật theo thời gian
        public DataTable GetSchemaReportRecieveObjectByTime(string userLogin)
        {
            string status = string.Empty;
            var basevices = new BaseService();
            DataTable tb = new DataTable("Hre_ReportRecieveObjectByTime");
            tb.Columns.Add("ProfileName");
            tb.Columns.Add("CodeEmp");
            tb.Columns.Add("E_UNIT");
            tb.Columns.Add("E_DIVISION");
            tb.Columns.Add("E_DEPARTMENT");
            tb.Columns.Add("E_TEAM");
            tb.Columns.Add("E_SECTION");
            tb.Columns.Add("Unit");
            tb.Columns.Add("Dept");
            tb.Columns.Add("Part");
            tb.Columns.Add("Line");
            tb.Columns.Add("Session");

            List<object> listObjPrice = new List<object>();
            listObjPrice.Add(null);
            listObjPrice.Add(null);
            listObjPrice.Add(null);

            listObjPrice.Add(1);
            listObjPrice.Add(Int32.MaxValue - 1);
            var lstHDTJobTypePrice = basevices.GetData<Cat_HDTJobTypePriceEntity>(listObjPrice, ConstantSql.hrm_cat_sp_get_HDTJobTypePrice, userLogin, ref status).Where(s => s.Price != null).Select(s => s.Price).Distinct().ToList();
            foreach (var item in lstHDTJobTypePrice)
            {
                tb.Columns.Add(item.ToString());
            }
            return tb;
        }

        public DataTable GetReportRecieveObjectByTime(string orgStructureIDs, DateTime dateFrom, DateTime dateTo, bool IsCreateTemplate, string userLogin)
        {
            DataTable table = GetSchemaReportRecieveObjectByTime(userLogin);
            if (IsCreateTemplate)
            {
                return table.ConfigTable();
            }
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.AddRange(new object[14]);
                listObjHDTJob[5] = orgStructureIDs;
                listObjHDTJob[6] = dateFrom;
                listObjHDTJob[7] = dateTo;
                listObjHDTJob[12] = 1;
                listObjHDTJob[13] = Int32.MaxValue - 1;
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_RptRecieveObjectByTime, userLogin, ref status).ToList();
                if (lstHDTJob.Count == 0)
                {
                    return table;
                }

                var lstProfileIDByHDTJob = lstHDTJob.Select(s => s.ProfileID).Distinct().ToList();

                #region Lấy WorkDay
                var workDayRepository = new Att_WorkDayRepository(unitOfWork);
                var lstworkDays = new List<Att_Workday>().Select(d => new
                {
                    d.ProfileID,
                    d.WorkDate,
                    d.FirstInTime,
                    d.LastOutTime
                }).ToList();

                foreach (var item in lstProfileIDByHDTJob.Chunk(1000))
                {
                    lstworkDays.AddRange(workDayRepository.FindBy(s => s.IsDelete == null && s.WorkDate >= dateFrom
                        && s.WorkDate <= dateTo && item.Contains(s.ProfileID)).Select(d => new
                        {
                            d.ProfileID,
                            d.WorkDate,
                            d.FirstInTime,
                            d.LastOutTime
                        }).ToList());
                }

                if (lstworkDays.Count == 0)
                {
                    return table;
                }
                #endregion

                #region Lấy MealRecord
                var mealRecordRepository = new Can_MealRecordRepository(unitOfWork);
                var lstmeadrecored = new List<Can_MealRecord>().Select(d => new
                {
                    d.ProfileID,
                    d.WorkDay,
                    d.Amount
                }).ToList();

                foreach (var item in lstProfileIDByHDTJob.Chunk(1000))
                {
                    lstmeadrecored.AddRange(mealRecordRepository.FindBy(s => s.IsDelete == null && s.WorkDay >= dateFrom
                        && s.WorkDay <= dateTo && item.Contains(s.ProfileID)).Select(d => new
                        {
                            d.ProfileID,
                            d.WorkDay,
                            d.Amount
                        }).ToList());
                }
                #endregion

                List<object> listObjPrice = new List<object>();
                listObjPrice.Add(null);
                listObjPrice.Add(null);
                listObjPrice.Add(null);
                listObjPrice.Add(1);
                listObjPrice.Add(Int32.MaxValue - 1);
                var lstHDTJobTypePrice = basevices.GetData<Cat_HDTJobTypePriceEntity>(listObjPrice, ConstantSql.hrm_cat_sp_get_HDTJobTypePrice, userLogin, ref status).Where(s => s.Price != null).Distinct().ToList();
                var profileServices = new Hre_ProfileServices();
                var listResult = profileServices.getHDTJobByPrice(lstHDTJob, dateFrom, dateTo);

                var ListProfileID = listResult.Select(m => m.ProfileID).Distinct().ToList();

                foreach (var profileID in ListProfileID)
                {
                    var firstProfile = listResult.Where(m => m.ProfileID == profileID).FirstOrDefault();
                    var lstWorkDaysByProfile = lstworkDays.Where(m => m.ProfileID == profileID && (m.FirstInTime != null || m.LastOutTime != null)).ToList();
                    var lstmeadlbypro = lstmeadrecored.Where(s => s.ProfileID == profileID).ToList();
                    if (firstProfile != null && lstWorkDaysByProfile.Count != 0)
                    {
                        bool isAdd = false;
                        DataRow row = table.NewRow();
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.ProfileName] = firstProfile.ProfileName;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.CodeEmp] = firstProfile.CodeEmp;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.E_DEPARTMENT] = firstProfile.E_DEPARTMENT;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.E_DIVISION] = firstProfile.E_DIVISION;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.E_SECTION] = firstProfile.E_SECTION;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.E_TEAM] = firstProfile.E_TEAM;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.E_UNIT] = firstProfile.E_UNIT;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.Dept] = firstProfile.Dept;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.Unit] = firstProfile.Unit;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.Part] = firstProfile.Part;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.Line] = firstProfile.Line;
                        row[Hre_ReportRecieveObjectByTimeEntity.FieldNames.Session] = firstProfile.Session;
                        var hdtjobbyprofile = lstHDTJob.Where(s => s.ProfileID == profileID).ToList();
                        var workdaybyProfile = lstworkDays.Where(s => s.ProfileID == profileID).ToList();
                        foreach (var item in lstHDTJobTypePrice)
                        {
                            var resultByPrice = listResult.Where(m => m.ProfileID == profileID && m.Price == item.Price).ToList();

                            if (resultByPrice.Count == 0)
                            {
                                continue;
                            }
                            int? count = 0;

                            foreach (var Price in resultByPrice)
                            {
                                if (Price.DateTo == null)
                                {
                                    Price.DateTo = dateTo;
                                }
                                for (DateTime date = Price.DateFrom.Value; date <= Price.DateTo; date = date.AddDays(1))
                                {
                                    var workdaybyprice = workdaybyProfile.Where(s => s.WorkDate != null && s.WorkDate.Date == date.Date && s.FirstInTime != null && s.LastOutTime != null).FirstOrDefault();
                                    var mealbyprice = lstmeadlbypro.Where(s => s.Amount != null && s.WorkDay != null && s.WorkDay.Value.Date == date.Date && (double)s.Amount == Price.Price).FirstOrDefault();

                                    if (workdaybyprice != null && mealbyprice != null)
                                    {
                                        count++;
                                    }
                                    row[item.Price.ToString()] = count != 0 ? count : null;
                                    if (count != null && count != 0)
                                    {
                                        isAdd = true;
                                    }
                                }

                            }
                        }
                        if (isAdd == true)
                        {
                            table.Rows.Add(row);
                        }
                    }
                }
            }
            return table.ConfigTable();
        }
        #endregion

        #region BC tông hợp nv làm hdt
        public List<Hre_ReportSumaryHDTProfileEntity> GetReportSumaryHDTProfile(DateTime? DateFrom, DateTime? DateTo, Guid? HDTJobGroupID, string CodeEmp, string userLogin)
        {
            string status = string.Empty;
            List<Hre_ReportSumaryHDTProfileEntity> lstReportSumaryHDTProfile = new List<Hre_ReportSumaryHDTProfileEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                var ProfileServices = new Hre_ProfileServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(CodeEmp);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(DateFrom);
                listObjHDTJob.Add(DateTo);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_ReportSumaryHDTProfile, userLogin, ref status).ToList();
                if (lstHDTJob.Count == 0)
                {
                    return lstReportSumaryHDTProfile;
                }
                if (HDTJobGroupID != null)
                {
                    lstHDTJob = lstHDTJob.Where(s => s.HDTJobGroupID == HDTJobGroupID).ToList();
                }
                var lstprofileids = lstHDTJob.Select(s => s.ProfileID).Distinct().ToList();

                string selectedIds = Common.DotNetToOracle(String.Join(",", lstHDTJob.Select(s => s.ProfileID.ToString()).ToList<string>()));

                var lstInsurance = new List<Ins_ProfileInsuranceMonthlyEntity>();
                int _total = lstprofileids.Count;
                int _totalPage = _total / 100 + 1;
                int _pageSize = 100;
                for (int _page = 1; _page <= _totalPage; _page++)
                {
                    int _skip = _pageSize * (_page - 1);
                    var _listCurrenPage = lstprofileids.Skip(_skip).Take(_pageSize).ToList();
                    string _strselectedIDs = Common.DotNetToOracle(string.Join(",", _listCurrenPage));
                    var lstresultInsurance = basevices.GetData<Ins_ProfileInsuranceMonthlyEntity>(_strselectedIDs, ConstantSql.hrm_ins_sp_get_ProfileInsMonthlyByProfileIds, userLogin, ref status).ToList();
                    if (lstresultInsurance != null && lstresultInsurance.Count > 0)
                    {
                        lstInsurance.AddRange(lstresultInsurance);
                    }
                }

                var lstHDTJobCut = ProfileServices.getHDTJobByPrice(lstHDTJob, DateFrom, DateTo);

                foreach (var HDTJob in lstHDTJobCut)
                {
                    Hre_ReportSumaryHDTProfileEntity entity = new Hre_ReportSumaryHDTProfileEntity();
                    entity.ProfileName = HDTJob.ProfileName;
                    entity.CodeEmp = HDTJob.CodeEmp;
                    entity.E_UNIT = HDTJob.E_UNIT;
                    entity.E_DIVISION = HDTJob.E_DIVISION;
                    entity.E_DEPARTMENT = HDTJob.E_DEPARTMENT;
                    entity.E_TEAM = HDTJob.E_TEAM;
                    entity.E_SECTION = HDTJob.E_SECTION;
                    entity.Dept = HDTJob.Dept;
                    entity.Type = HDTJob.Type != null ? HDTJob.Type.TranslateString() : null;
                    entity.HDTJobGroupCode = HDTJob.HDTJobGroupCode;
                    entity.HDTJobGroupName = HDTJob.HDTJobGroupName;
                    entity.DateFrom = HDTJob.DateFrom;
                    entity.DateTo = HDTJob.DateTo;
                    var insuracebyHDT = lstInsurance.Where(s => s.HDTJobGroupCode == HDTJob.HDTJobGroupCode && s.ProfileID == HDTJob.ProfileID && s.AmountHDTIns > 0).ToList();
                    entity.MonthInsurance = insuracebyHDT != null ? insuracebyHDT.Count : 0;
                    lstReportSumaryHDTProfile.Add(entity);
                }
            }

            return lstReportSumaryHDTProfile;
        }
        #endregion

        #region Hre_ReportSummaryDependant
        public DataTable CreateReportSummaryDependantScheme()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Hre_ReportSummaryDependantEntity");
                tb.Columns.Add(Hre_ReportSummaryDependantEntity.FieldNames.CodeEmp, typeof(string));
                tb.Columns.Add(Hre_ReportSummaryDependantEntity.FieldNames.ProfileName, typeof(string));
                tb.Columns.Add(Hre_ReportSummaryDependantEntity.FieldNames.CodeTax, typeof(string));
                tb.Columns.Add(Hre_ReportSummaryDependantEntity.FieldNames.DependantName, typeof(string));
                tb.Columns.Add(Hre_ReportSummaryDependantEntity.FieldNames.CompleteDate, typeof(DateTime));
                tb.Columns.Add(Hre_ReportSummaryDependantEntity.FieldNames.MonthOfEffect, typeof(DateTime));
                tb.Columns.Add(Hre_ReportSummaryDependantEntity.FieldNames.MonthOfExpiry, typeof(DateTime));
                tb.Columns.Add(Hre_ReportSummaryDependantEntity.FieldNames.TotalDependant, typeof(int));
                return tb;
            }
        }

        public DataTable GetReportSummaryDependant(List<Hre_DependantEntity> lstDependant, bool _isCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (UnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportSummaryDependantScheme();
                if (lstDependant == null)
                {
                    return table;
                }
                foreach (var item in lstDependant)
                {
                    DataRow row = table.NewRow();
                    row[Hre_ReportSummaryDependantEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    row[Hre_ReportSummaryDependantEntity.FieldNames.ProfileName] = item.ProfileName;
                    row[Hre_ReportSummaryDependantEntity.FieldNames.CodeTax] = item.CodeTax;
                    row[Hre_ReportSummaryDependantEntity.FieldNames.DependantName] = item.DependantName;
                    if (item.CompleteDate != null)
                        row[Hre_ReportSummaryDependantEntity.FieldNames.CompleteDate] = item.CompleteDate;
                    if (item.MonthOfEffect != null)
                        row[Hre_ReportSummaryDependantEntity.FieldNames.MonthOfEffect] = item.MonthOfEffect;
                    if (item.MonthOfExpiry != null)
                        row[Hre_ReportSummaryDependantEntity.FieldNames.MonthOfExpiry] = item.MonthOfExpiry;
                    var lstDependantbyprofile = lstDependant.Where(s => s.ProfileID == item.ProfileID).ToList();
                    int _TotalDependant = lstDependantbyprofile.Count;
                    if (_TotalDependant > 0)
                        row[Hre_ReportSummaryDependantEntity.FieldNames.TotalDependant] = _TotalDependant;
                    table.Rows.Add(row);
                }
                return table.ConfigTable(true);
            }
        }
        #endregion

        #region BC nv làm hdt chưa có ngày ra
        public List<Hre_ReportHDTJobNotDateEndEntity> GetReportHDTJobNotDateEnd(DateTime? DateFrom, DateTime? DateTo, string lstOrgOrderNumber, string userLogin)
        {
            string status = string.Empty;
            List<Hre_ReportHDTJobNotDateEndEntity> lstReportHDTJobNotDateEndEntity = new List<Hre_ReportHDTJobNotDateEndEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                var ProfileServices = new Hre_ProfileServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(lstOrgOrderNumber);
                listObjHDTJob.Add(DateFrom);
                listObjHDTJob.Add(DateTo);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_HDTJob, userLogin, ref status).ToList();

                if (lstHDTJob == null)
                {
                    return lstReportHDTJobNotDateEndEntity;
                }

                var lstResultHDTJob = ProfileServices.getHDTJobByPrice(lstHDTJob, DateFrom, DateTo);

                var profileinList = lstResultHDTJob.Select(s => s.ProfileID).Distinct().ToList();


                foreach (var ids in profileinList)
                {
                    bool isNull = false;
                    var hdtbypro = lstResultHDTJob.Where(s => s.ProfileID == ids).OrderBy(s => s.DateFrom).ToList();
                    var hdt = lstResultHDTJob.Where(s => s.ProfileID == ids).FirstOrDefault();

                    foreach (var item in hdtbypro)
                    {
                        if (hdtbypro.Count == 1 && item.DateTo != null)
                        {
                            continue;
                        }

                        if (item.DateTo == null)
                        {
                            isNull = true;
                        }

                        if (isNull)
                        {
                            Hre_ReportHDTJobNotDateEndEntity entity = new Hre_ReportHDTJobNotDateEndEntity();
                            entity.CodeEmp = item.CodeEmp;
                            entity.ProfileName = item.ProfileName;
                            entity.E_UNIT = item.E_UNIT;
                            entity.E_DIVISION = item.E_DIVISION;
                            entity.E_DEPARTMENT = item.E_DEPARTMENT;
                            entity.E_TEAM = item.E_TEAM;
                            entity.E_SECTION = item.E_SECTION;
                            entity.Dept = item.Dept;
                            entity.HDTJobTypeCode = item.HDTJobTypeCode;
                            entity.HDTJobTypeName = item.HDTJobTypeName;
                            entity.HDTJobGroupName = item.HDTJobGroupName;
                            entity.DateFrom = item.DateFrom;
                            entity.DateTo = item.DateTo;
                            entity.StatusView = item.StatusView;
                            lstReportHDTJobNotDateEndEntity.Add(entity);
                        }

                        if (item.DateTo != null)
                        {
                            isNull = false;
                        }
                    }
                }
            }
            lstReportHDTJobNotDateEndEntity = lstReportHDTJobNotDateEndEntity.OrderBy(s => s.CodeEmp).ToList();
            return lstReportHDTJobNotDateEndEntity;
        }
        #endregion

        #region BC tông hợp thâm niên nv làm HDT
        public List<Hre_ReportSumarySeniorHDTProfileEntity> GetReportSumarySeniorHDTProfile(DateTime? DateFrom, DateTime? DateTo, string profileName,
            string codeEmp, string OrgStructureID, string userLogin)
        {
            string status = string.Empty;
            List<Hre_ReportSumarySeniorHDTProfileEntity> lstReportSumarySeniorHDTProfile = new List<Hre_ReportSumarySeniorHDTProfileEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                var ProfileServices = new Hre_ProfileServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(profileName);
                listObjHDTJob.Add(codeEmp);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(OrgStructureID);
                listObjHDTJob.Add(DateFrom);
                listObjHDTJob.Add(DateTo);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_RptRecieveObjectByTime, userLogin, ref status).ToList();

                if (lstHDTJob.Count == 0)
                {
                    return lstReportSumarySeniorHDTProfile;
                }

                var lstProfileIds = lstHDTJob.Select(s => s.ProfileID).Distinct().ToList();
                string selectedIds = Common.DotNetToOracle(String.Join(",", lstHDTJob.Select(s => s.ProfileID.ToString()).ToList<string>()));

                var lstInsurance = new List<Ins_ProfileInsuranceMonthlyEntity>();
                int _total = lstProfileIds.Count;
                int _totalPage = _total / 100 + 1;
                int _pageSize = 100;
                for (int _page = 1; _page <= _totalPage; _page++)
                {
                    int _skip = _pageSize * (_page - 1);
                    var _listCurrenPage = lstProfileIds.Skip(_skip).Take(_pageSize).ToList();
                    string _strselectedIDs = Common.DotNetToOracle(string.Join(",", _listCurrenPage));
                    var lstresultInsurance = basevices.GetData<Ins_ProfileInsuranceMonthlyEntity>(_strselectedIDs, ConstantSql.hrm_ins_sp_get_ProfileInsMonthlyByProfileIds, userLogin, ref status).ToList();
                    if (lstresultInsurance != null && lstresultInsurance.Count > 0)
                    {
                        lstInsurance.AddRange(lstresultInsurance);
                    }
                }

                var lstHDTJobCut = ProfileServices.getHDTJobByPrice(lstHDTJob, DateFrom, DateTo);

                var lstprofileNameDistince = lstHDTJob.Select(s => new { s.ProfileID, s.ProfileName, s.CodeEmp, s.E_DEPARTMENT }).Distinct().ToList();
                foreach (var profile in lstprofileNameDistince)
                {
                    Hre_ReportSumarySeniorHDTProfileEntity entity = new Hre_ReportSumarySeniorHDTProfileEntity();
                    entity.CodeEmp = profile.CodeEmp;
                    entity.ProfileName = profile.ProfileName;
                    entity.E_DEPARTMENT = profile.E_DEPARTMENT;
                    var hdtbyProfile = lstHDTJobCut.Where(s => s.ProfileID == profile.ProfileID).ToList();
                    int counttype4 = 0;
                    int counttype5 = 0;
                    foreach (var item in hdtbyProfile)
                    {
                        var insuracebyHDT4 = lstInsurance.Where(s => s.HDTJobGroupCode == item.HDTJobGroupCode && s.ProfileID == item.ProfileID && s.AmountHDTIns > 0 && item.Type == "E_TYPE4").ToList();
                        if (insuracebyHDT4.Count > 0)
                        {
                            counttype4 += insuracebyHDT4.Count;
                        }
                        var insuracebyHDT5 = lstInsurance.Where(s => s.HDTJobGroupCode == item.HDTJobGroupCode && s.ProfileID == item.ProfileID && s.AmountHDTIns > 0 && item.Type == "E_TYPE5").ToList();
                        if (insuracebyHDT4.Count > 0)
                        {
                            counttype5 += insuracebyHDT5.Count;
                        }
                    }

                    entity.MonthInsuranceType4 = counttype4;
                    entity.MonthInsuranceType5 = counttype5;
                    lstReportSumarySeniorHDTProfile.Add(entity);
                }
            }
            return lstReportSumarySeniorHDTProfile;
        }
        #endregion

        #region ReportUnusualHDT
        public DataTable CreateReportUnusualHDTScheme()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Hre_ReportUnusualHDTEntity");
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.ProfileName);
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.E_UNIT);
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.E_DIVISION);

                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.E_DEPARTMENT);
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.E_TEAM);
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.E_SECTION);

                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.HDTJobTypeCode);
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.HDTJobTypeName);
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.Price, typeof(double));
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.TimeScan);
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.PriceRecieve, typeof(double));
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.NotRegister, typeof(bool));
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.HaveRegister, typeof(bool));
                tb.Columns.Add(Hre_ReportUnusualHDTEntity.FieldNames.RevieveWrong, typeof(bool));
                return tb;
            }
        }

        public DataTable GetReportUnusualHDT(DateTime? DateFrom, DateTime? DateTo, string lstOrgOrderNumber, bool _isCreateTemplate, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (UnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportUnusualHDTScheme();
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                var ProfileServices = new Hre_ProfileServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(lstOrgOrderNumber);
                listObjHDTJob.Add(DateFrom);
                listObjHDTJob.Add(DateTo);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                var lstReportUnusualHDT = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_HDTJob, userLogin, ref status).ToList();
                if (lstReportUnusualHDT.Count == 0)
                {
                    return table;
                }
                lstReportUnusualHDT = ProfileServices.getHDTJobByPrice(lstReportUnusualHDT, DateFrom, DateTo);

                var lstprofileids = lstReportUnusualHDT.Select(s => s.ProfileID).Distinct().ToList();
                var repoCan_MealRecord = new CustomBaseRepository<Can_MealRecord>(unitOfWork);
                var lstmealrecord = new List<Can_MealRecord>().Select(d => new
                {
                    d.ProfileID,
                    d.TimeLog,
                    d.LineID,
                    d.Amount
                }).ToList();
                foreach (var item in lstprofileids.Chunk(1000))
                {
                    lstmealrecord.AddRange(repoCan_MealRecord.FindBy(s => s.IsDelete == null && lstprofileids.Contains(s.ProfileID) && s.TimeLog >= DateFrom && s.TimeLog <= DateTo).Select(d => new
                    {
                        d.ProfileID,
                        d.TimeLog,
                        d.LineID,
                        d.Amount
                    }).ToList());
                }
                var lstlineids = lstmealrecord.Select(s => s.LineID).Distinct().ToList();

                var repoCan_Line = new CustomBaseRepository<Can_Line>(unitOfWork);
                var lstline = repoCan_Line.FindBy(s => s.IsDelete == null && lstlineids.Contains(s.ID) && s.IsHDTJOB == true).ToList();
                foreach (var item in lstReportUnusualHDT)
                {
                    DataRow row = table.NewRow();
                    row[Hre_ReportUnusualHDTEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    row[Hre_ReportUnusualHDTEntity.FieldNames.ProfileName] = item.ProfileName;
                    row[Hre_ReportUnusualHDTEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                    row[Hre_ReportUnusualHDTEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                    row[Hre_ReportUnusualHDTEntity.FieldNames.E_SECTION] = item.E_SECTION;
                    row[Hre_ReportUnusualHDTEntity.FieldNames.E_TEAM] = item.E_TEAM;
                    row[Hre_ReportUnusualHDTEntity.FieldNames.E_UNIT] = item.E_UNIT;
                    row[Hre_ReportUnusualHDTEntity.FieldNames.HDTJobTypeCode] = item.HDTJobTypeCode;
                    row[Hre_ReportUnusualHDTEntity.FieldNames.HDTJobTypeName] = item.HDTJobTypeName;
                    if (item.Price != null)
                    {
                        row[Hre_ReportUnusualHDTEntity.FieldNames.Price] = item.Price;
                    }
                    var mealbypro = lstmealrecord.Where(s => s.ProfileID == item.ProfileID).FirstOrDefault();
                    if (mealbypro != null)
                    {
                        if (mealbypro.TimeLog != null)
                        {
                            row[Hre_ReportUnusualHDTEntity.FieldNames.TimeScan] = mealbypro.TimeLog.Value.ToString("dd/MM/yyyy hh:ss");
                        }
                        var linebyhdt = lstline.Where(s => s.ID == mealbypro.LineID).FirstOrDefault();
                        if (linebyhdt != null && linebyhdt.Amount != null)
                        {
                            row[Hre_ReportUnusualHDTEntity.FieldNames.PriceRecieve] = linebyhdt.Amount;
                        }
                        if (linebyhdt != null && linebyhdt.Amount != null)
                        {
                            if (item.Price != (double)linebyhdt.Amount)
                            {
                                row[Hre_ReportUnusualHDTEntity.FieldNames.RevieveWrong] = true;
                            }
                        }
                        if (item.Price != null && linebyhdt == null)
                        {
                            row[Hre_ReportUnusualHDTEntity.FieldNames.HaveRegister] = true;
                        }
                        if (item.Price == null && linebyhdt != null)
                        {
                            row[Hre_ReportUnusualHDTEntity.FieldNames.NotRegister] = true;
                        }
                    }
                    table.Rows.Add(row);
                }
                return table.ConfigTable(true);
            }
        }
        #endregion

        #region ReportSummaryDependantDeduction
        public DataTable CreateReportSummaryDependantDeductionSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Hre_ReportSummaryDependantDeductionModel");
                tb.Columns.Add(Hre_ReportSummaryDependantDeductionEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Hre_ReportSummaryDependantDeductionEntity.FieldNames.ProfileName);
                tb.Columns.Add(Hre_ReportSummaryDependantDeductionEntity.FieldNames.CodeTax);
                tb.Columns.Add(Hre_ReportSummaryDependantDeductionEntity.FieldNames.DependantName);
                tb.Columns.Add(Hre_ReportSummaryDependantDeductionEntity.FieldNames.CompleteDate, typeof(DateTime));
                tb.Columns.Add(Hre_ReportSummaryDependantDeductionEntity.FieldNames.MonthOfEffect, typeof(DateTime));
                tb.Columns.Add(Hre_ReportSummaryDependantDeductionEntity.FieldNames.MonthOfExpiry, typeof(DateTime));
                return tb;
            }
        }

        public DataTable GetReportSummaryDependantDeduction(List<Hre_DependantEntity> lstDependant, bool _isCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (UnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportSummaryDependantDeductionSchema();
                foreach (var item in lstDependant)
                {
                    DataRow row = table.NewRow();
                    row[Hre_ReportSummaryDependantDeductionEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    row[Hre_ReportSummaryDependantDeductionEntity.FieldNames.ProfileName] = item.ProfileName;
                    row[Hre_ReportSummaryDependantDeductionEntity.FieldNames.CodeTax] = item.CodeTax;
                    row[Hre_ReportSummaryDependantDeductionEntity.FieldNames.DependantName] = item.DependantName;
                    if (item.CompleteDate != null)
                        row[Hre_ReportSummaryDependantDeductionEntity.FieldNames.CompleteDate] = item.CompleteDate;
                    if (item.MonthOfEffect != null)
                        row[Hre_ReportSummaryDependantDeductionEntity.FieldNames.MonthOfEffect] = item.MonthOfEffect;
                    if (item.MonthOfExpiry != null)
                        row[Hre_ReportSummaryDependantDeductionEntity.FieldNames.MonthOfExpiry] = item.MonthOfExpiry;
                    table.Rows.Add(row);
                }
                return table.ConfigTable(true);
            }
        }
        #endregion

        #region BC Tổng Hợp Điểm Tích Lũy
        DataTable CreateReportKaizenAccumulateSchema(DateTime? dateFrom, DateTime? dateTo)
        {
            DataTable tb = new DataTable("Kai_ReportKaizenDetailEntity");
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.CodeEmp, typeof(string));
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.ProfileName, typeof(string));
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.E_UNIT, typeof(string));
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.E_DIVISION, typeof(string));
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.E_DEPARTMENT, typeof(string));
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.E_TEAM, typeof(string));
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.E_SECTION, typeof(string));
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.StatusSynView, typeof(string));
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.KaizenName, typeof(string));
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.AccumulateTotal, typeof(double));
            DateTime? tempdateFrom = dateFrom.Value;
            DateTime? tempdateTo = dateTo.Value;
            int _month = 1;
            while (tempdateFrom <= tempdateTo)
            {
                if (!tb.Columns.Contains("MarkPerform" + _month))
                {
                    tb.Columns.Add("MarkPerform" + _month);
                }

                tempdateFrom = tempdateFrom.Value.AddMonths(1);
                _month += 1;
            }
            _month = 1;
            while (dateFrom <= dateTo)
            {
                if (!tb.Columns.Contains("AccumulateRevice" + _month))
                {
                    tb.Columns.Add("AccumulateRevice" + _month);
                }
                dateFrom = dateFrom.Value.AddMonths(1);
                _month += 1;
            }
            tb.Columns.Add(Kai_ReportKaizenDetailEntity.FieldNames.AccumulateReviceTotal, typeof(double));
            return tb;
        }
        public DataTable GetReportKaizenAccumulate(List<Kai_ReportKaizenDetailEntity> lstKaizenDetailEntity, DateTime? dateFrom, DateTime? dateTo, int? _RateDetail, bool _IsCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable table = CreateReportKaizenAccumulateSchema(dateFrom, dateTo);
                if (_IsCreateTemplate)
                    return table;
                if (lstKaizenDetailEntity == null)
                    return table;
                List<Guid> lstProfileId = new List<Guid>();
                foreach (var item in lstKaizenDetailEntity)
                {
                    DateTime? tempdateFrom = dateFrom.Value;
                    DateTime? tempdateTo = dateTo.Value;
                    if (!lstProfileId.Contains(item.ProfileID.Value))
                    {
                        DataRow row = table.NewRow();
                        row[Kai_ReportKaizenDetailEntity.FieldNames.CodeEmp] = item.CodeEmp;
                        row[Kai_ReportKaizenDetailEntity.FieldNames.ProfileName] = item.ProfileName;
                        row[Kai_ReportKaizenDetailEntity.FieldNames.E_UNIT] = item.E_UNIT;
                        row[Kai_ReportKaizenDetailEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                        row[Kai_ReportKaizenDetailEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                        row[Kai_ReportKaizenDetailEntity.FieldNames.E_TEAM] = item.E_TEAM;
                        row[Kai_ReportKaizenDetailEntity.FieldNames.E_SECTION] = item.E_SECTION;
                        row[Kai_ReportKaizenDetailEntity.FieldNames.KaizenName] = item.KaizenName;
                        row[Kai_ReportKaizenDetailEntity.FieldNames.StatusSynView] = item.StatusSynView;

                        double? _AccumulateTotal = 0;
                        double? _AccumulateReviceTotal = 0;
                        int _month = 1;
                        while (tempdateFrom <= tempdateTo)
                        {
                            var lstKaizenDetailEntityByMonth = lstKaizenDetailEntity.Where(s => s.ProfileID == item.ProfileID.Value && s.Month != null && s.Month.Value.Month == tempdateFrom.Value.Month && s.Month.Value.Year == tempdateFrom.Value.Year).ToList();
                            if (lstKaizenDetailEntityByMonth.Count > 0)
                            {
                                //row["Accumulate_" + dateFrom.Value.Month + "_" + dateFrom.Value.Year] = lstKaizenDetailEntityByMonth.Sum(s => s.Accumulate);
                                _AccumulateTotal += lstKaizenDetailEntityByMonth.Sum(s => s.Accumulate);

                            }
                            lstKaizenDetailEntityByMonth = lstKaizenDetailEntityByMonth.Where(s => s.MarkPerform > 0).ToList();
                            int countMarkPerformByMonth = lstKaizenDetailEntityByMonth.Count;
                            if (countMarkPerformByMonth > 0)
                            {
                                row["MarkPerform" + _month] = countMarkPerformByMonth;
                            }
                            double? _AccumulateRevice = 0;
                            if (lstKaizenDetailEntityByMonth.Count > 0)
                            {
                                _AccumulateRevice = lstKaizenDetailEntityByMonth.Sum(s => s.AccumulateRevice);
                                _AccumulateReviceTotal += lstKaizenDetailEntityByMonth.Sum(s => s.AccumulateRevice);
                                //row["AccumulateRevice" + _month] = lstKaizenDetailEntityByMonth.Sum(s => s.AccumulateRevice);
                            }
                            if (_AccumulateRevice > 0)
                                row["AccumulateRevice" + _month] = _AccumulateRevice;
                            tempdateFrom = tempdateFrom.Value.AddMonths(1);
                            _month += 1;
                        }
                        row[Kai_ReportKaizenDetailEntity.FieldNames.AccumulateReviceTotal] = _AccumulateReviceTotal;
                        if (_AccumulateTotal > 0)
                            row[Kai_ReportKaizenDetailEntity.FieldNames.AccumulateTotal] = _AccumulateTotal;
                        lstProfileId.Add(item.ProfileID.Value);
                        table.Rows.Add(row);
                    }
                }
                if (_RateDetail != null && table.Rows.Count > _RateDetail)
                {
                    table.DefaultView.Sort = "AccumulateReviceTotal asc";
                    DataTable tableSortByAccumulateReviceTotal = table.Clone();
                    for (int i = 0; i < _RateDetail; i++)
                    {
                        tableSortByAccumulateReviceTotal.ImportRow(table.Rows[i]);
                    }
                    return tableSortByAccumulateReviceTotal.ConfigTable();
                }
                return table.ConfigTable();
            }
        }
        #endregion

        #region BC HeadCount theo tháng
        DataTable CreateReportHCByMonthSchema(Guid? orgID)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable datatable = new DataTable("Hre_ReportHCByMonthEntity");
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.Division);
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.Department);
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.Section);
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.Position);
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.FirstOfMonth, typeof(int));
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.NewHiring, typeof(int));
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.Resign, typeof(int));
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.Transfer, typeof(int));
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.Join, typeof(int));
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.HeadcountBudget);
                datatable.Columns.Add(Hre_ReportHCByMonthEntity.FieldNames.Vacancy);
                return datatable;
            }
        }

        public DataTable GetReportHCByMonth(DateTime? DateSearch, Guid? orgID, bool isCreateTemplate, string userLogin)
        {

            DataTable table = CreateReportHCByMonthSchema(orgID);
            if (isCreateTemplate)
            {
                return table.ConfigTable();
            }

            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPosition = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var orgStructureServices = new Cat_OrgStructureServices();

                var orgsService = new Cat_OrgStructureServices();
                var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, userLogin, ref status).ToList();

                var lstorgs = lstallorgs.Where(s => s.ParentID == orgID).ToList();
                var lstOrgName = lstallorgs.Where(s => s.ID == orgID).FirstOrDefault();
                var listorgid = lstorgs.Select(s => new { s.ID, s.OrderNumber, s.Code, s.OrgStructureName }).ToList();
                var orgIDs = string.Empty;
                orderNumber = string.Empty;

                foreach (var item in listorgid)
                {
                    orderNumber += item.OrderNumber.ToString() + ",";
                    getChildOrgStructure(lstallorgs, item.ID);
                }
                if (orderNumber.IndexOf(',') > 0)
                    orderNumber = orderNumber.Substring(0, orderNumber.Length - 1);

                var lstObjOrgByOrderNumber = new List<object>();
                lstObjOrgByOrderNumber.Add(orderNumber);
                var lstOrgByOrderNumber = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumber, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, userLogin, ref status).Select(s => s.ID).ToList();

                List<object> listObj = new List<object>();
                listObj.Add(orderNumber);
                listObj.Add(string.Empty);
                listObj.Add(string.Empty);
                var lstprofile = GetData<Hre_ProfileEntity>(listObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).ToList();

                var PositionServices = new Cat_PositionServices();
                var lstObjPosition = new List<object>();
                lstObjPosition.Add(null);
                lstObjPosition.Add(null);
                lstObjPosition.Add(1);
                lstObjPosition.Add(int.MaxValue - 1);
                var lstPosition = GetData<Cat_PositionEntity>(lstObjPosition, ConstantSql.hrm_cat_sp_get_Position, userLogin, ref status).ToList();

                var orgTypeServices = new Cat_OrgStructureTypeServices();
                var objOrgType = new List<object>();
                objOrgType.Add(null);
                objOrgType.Add(null);
                objOrgType.Add(1);
                objOrgType.Add(int.MaxValue - 1);
                var lstOrgType = orgTypeServices.GetData<Cat_OrgStructureTypeEntity>(objOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, userLogin, ref status).ToList().Translate<Cat_OrgStructureType>();

                var PlanHeadCountServices = new Hre_PlanHeadCountServices();
                var objPlanHeadCount = new List<object>();
                objPlanHeadCount.Add(null);
                objPlanHeadCount.Add(1);
                objPlanHeadCount.Add(int.MaxValue - 1);
                var lstobjPlanHeadCount = orgTypeServices.GetData<Hre_PlanHeadCountEntity>(objPlanHeadCount, ConstantSql.hrm_hr_sp_get_PlanHeadCount, userLogin, ref status).ToList();

                foreach (var item in lstOrgByOrderNumber)
                {
                    DataRow row = table.NewRow();
                    var lstpositionbyOrg = lstPosition.Where(s => s.OrgStructureID == item).ToList();
                    if (lstpositionbyOrg.Count == 0)
                    {
                        continue;
                    }

                    foreach (var position in lstpositionbyOrg)
                    {
                        Guid? orgId = item;
                        var org = lstallorgs.FirstOrDefault(s => s.ID == item);
                        var E_DIVISION = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, lstallorgs, lstOrgType);
                        var E_DEPARTMENT = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, lstallorgs, lstOrgType);
                        var E_SECTION = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, lstallorgs, lstOrgType);
                        var countFirstOfMonth = lstprofile.Where(s => s.DateOfEffect != null && s.PositionID == position.ID && s.DateOfEffect.Value.Month <= DateSearch.Value.Month).Count();
                        var newHiring = lstprofile.Where(s => s.DateHire != null && s.PositionID == position.ID && s.DateHire.Value.Month == DateSearch.Value.Month).Count();
                        var resignInMonth = lstprofile.Where(s => s.DateQuit != null && s.PositionID == position.ID && s.DateQuit.Value.Month == DateSearch.Value.Month).Count();
                        var tranferInMonth = lstprofile.Where(s => s.DateOfEffect != null &&
                            (s.PositionID == position.ID && s.DateOfEffect.Value.Month < DateSearch.Value.Month)
                            && (s.PositionID != position.ID && s.DateOfEffect.Value.Month >= DateSearch.Value.Month)).Count();
                        var joinInMonth = lstprofile.Where(s => s.DateOfEffect != null &&
                         (s.PositionID == position.ID && s.DateOfEffect.Value.Month >= DateSearch.Value.Month)
                         && (s.PositionID != position.ID && s.DateOfEffect.Value.Month < DateSearch.Value.Month)).Count();
                        var headcount = lstobjPlanHeadCount.Where(s => s.OrgStructureID == item && s.PostionID == position.ID).FirstOrDefault();
                        row[Hre_ReportHCByMonthEntity.FieldNames.Division] = E_DIVISION != null ? E_DIVISION.OrgStructureName : null;
                        row[Hre_ReportHCByMonthEntity.FieldNames.Department] = E_DEPARTMENT != null ? E_DEPARTMENT.OrgStructureName : null;
                        row[Hre_ReportHCByMonthEntity.FieldNames.Section] = E_SECTION != null ? E_SECTION.OrgStructureName : null;
                        row[Hre_ReportHCByMonthEntity.FieldNames.Position] = position.PositionName;
                        row[Hre_ReportHCByMonthEntity.FieldNames.FirstOfMonth] = countFirstOfMonth;
                        row[Hre_ReportHCByMonthEntity.FieldNames.NewHiring] = newHiring;
                        row[Hre_ReportHCByMonthEntity.FieldNames.Resign] = resignInMonth;
                        row[Hre_ReportHCByMonthEntity.FieldNames.Transfer] = tranferInMonth;
                        row[Hre_ReportHCByMonthEntity.FieldNames.Join] = joinInMonth;
                        row[Hre_ReportHCByMonthEntity.FieldNames.HeadcountBudget] = headcount != null ? headcount.HrPlanHC : null;
                        table.Rows.Add(row);
                    }
                }
                return table;
            }
        }

        #endregion

        #region BC NV nghỉ việc có thay đổi người phụ thuộc
        public List<Hre_ReportDependantProfileQuitEntity> GetReportDependantProfileQuit(DateTime? dateFrom, DateTime? dateTo, Guid? workPlaceID, string orgStructureID, string userLogin)
        {
            string status = string.Empty;
            List<Hre_ReportDependantProfileQuitEntity> lstReportDependantProfileQuit = new List<Hre_ReportDependantProfileQuitEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                var ProfileServices = new Hre_ProfileServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(orgStructureID);
                listObjHDTJob.Add(workPlaceID);
                listObjHDTJob.Add(dateFrom);
                listObjHDTJob.Add(dateTo);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                var lstDependant = hdtJobServices.GetData<Hre_DependantEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_ReportDependantProfileQuit, userLogin, ref status).ToList();

                if (lstDependant.Count == 0)
                {
                    return lstReportDependantProfileQuit;
                }
                Guid[] lstprofileIds = lstDependant.Select(x => Guid.Parse(x.ProfileID.ToString())).ToArray();

                var repostopwoking = new Hre_StopWorkingRepository(unitOfWork);
                var profileids = repostopwoking.FindBy(s => s.IsDelete == null && s.ProfileID != null && lstprofileIds.Contains(s.ProfileID.Value)
                    && (s.StopWorkType == HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_SUSPENSE.ToString() ||
                    s.StopWorkType == HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_STOP.ToString()
                    )).Select(s => s.ProfileID).ToList();
                if (profileids != null && profileids.Count > 0)
                {
                    lstDependant = lstDependant.Where(s => profileids.Contains(s.ProfileID)).ToList();
                }

                foreach (var Dependant in lstDependant)
                {
                    Hre_ReportDependantProfileQuitEntity entity = new Hre_ReportDependantProfileQuitEntity();
                    entity.CodeEmp = Dependant.CodeEmp;
                    entity.ProfileName = Dependant.ProfileName;
                    entity.DependantName = Dependant.DependantName;
                    entity.DateOfBirth = Dependant.DateOfBirth;
                    entity.DateQuit = Dependant.DateQuit;
                    entity.MonthOfEffect = Dependant.MonthOfEffect;
                    lstReportDependantProfileQuit.Add(entity);
                }
            }
            return lstReportDependantProfileQuit;
        }
        #endregion

        public List<Hre_ReportDependantProfileQuitEntity> GetReportDependantProfileQuits(DateTime? dateFrom, DateTime? dateTo, Guid? workPlaceID, string orgStructureID, string userLogin)
        {
            string status = string.Empty;
            List<Hre_ReportDependantProfileQuitEntity> lstReportDependantProfileQuit = new List<Hre_ReportDependantProfileQuitEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                var ProfileServices = new Hre_ProfileServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(orgStructureID);
                listObjHDTJob.Add(workPlaceID);
                listObjHDTJob.Add(dateFrom);
                listObjHDTJob.Add(dateTo);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                var lstDependant = hdtJobServices.GetData<Hre_DependantEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_ReportDependantProfileQuit, userLogin, ref status).ToList();

                if (lstDependant.Count == 0)
                {
                    return lstReportDependantProfileQuit;
                }
                Guid[] lstprofileIds = lstDependant.Select(x => Guid.Parse(x.ProfileID.ToString())).ToArray();

                var repostopwoking = new Hre_StopWorkingRepository(unitOfWork);
                var profileids = repostopwoking.FindBy(s => s.IsDelete == null && s.ProfileID != null && lstprofileIds.Contains(s.ProfileID.Value)
                    && (s.StopWorkType == HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_SUSPENSE.ToString() ||
                    s.StopWorkType == HRM.Infrastructure.Utilities.EnumDropDown.StopWorkType.E_STOP.ToString()
                    )).Select(s => s.ProfileID).ToList();
                if (profileids != null && profileids.Count > 0)
                {
                    lstDependant = lstDependant.Where(s => profileids.Contains(s.ProfileID)).ToList();
                }

                foreach (var Dependant in lstDependant)
                {
                    Hre_ReportDependantProfileQuitEntity entity = new Hre_ReportDependantProfileQuitEntity();
                    entity.CodeEmp = Dependant.CodeEmp;
                    entity.ProfileName = Dependant.ProfileName;
                    entity.DependantName = Dependant.DependantName;
                    entity.DateOfBirth = Dependant.DateOfBirth;
                    entity.DateQuit = Dependant.DateQuit;
                    entity.MonthOfEffect = Dependant.MonthOfEffect;
                    lstReportDependantProfileQuit.Add(entity);
                }
            }
            return lstReportDependantProfileQuit;
        }
        public List<Hre_HDTJobEntity> GetReportHDTJobOut(DateTime? DateTo, string lstOrgOrderNumber, Guid? workPlaceID, string profileName, string CodeEmp, string userLogin)
        {
            string status = string.Empty;
            List<Hre_HDTJobEntity> lstReportHDTJobEntity = new List<Hre_HDTJobEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                var profileServices = new Hre_ProfileServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(profileName);
                listObjHDTJob.Add(CodeEmp);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(lstOrgOrderNumber);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_HDTJob, userLogin, ref status).ToList();

                if (lstHDTJob.Count == 0)
                {
                    return lstReportHDTJobEntity;
                }
                if (DateTo != null)
                {
                    lstHDTJob = lstHDTJob.Where(s => s.DateTo != null && s.DateTo.Value.Date == DateTo.Value.Date).ToList();
                }
                lstReportHDTJobEntity = profileServices.getHDTJobByPrice(lstHDTJob, null, DateTo);
            }
            return lstReportHDTJobEntity;
        }
        public List<Hre_HDTJobEntity> GetReportHDTJobIn(DateTime? DateFrom, string lstOrgOrderNumber, Guid? workPlaceID, string profileName, string CodeEmp, string userLogin)
        {
            string status = string.Empty;
            List<Hre_HDTJobEntity> lstReportHDTJobEntity = new List<Hre_HDTJobEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                var profileServices = new Hre_ProfileServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(profileName);
                listObjHDTJob.Add(CodeEmp);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(lstOrgOrderNumber);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_HDTJob, userLogin, ref status).ToList();

                if (lstHDTJob.Count == 0)
                {
                    return lstReportHDTJobEntity;
                }
                if (DateFrom != null)
                {
                    lstHDTJob = lstHDTJob.Where(s => s.DateTo != null && s.DateFrom.Value.Date == DateFrom.Value.Date).ToList();
                }
                lstReportHDTJobEntity = profileServices.getHDTJobByPrice(lstHDTJob, DateFrom, null);
            }
            return lstReportHDTJobEntity;
        }


        public List<Hre_HDTJobEntity> GetReportHDTJobDecisionAssignWork(DateTime? Datesearch, string lstOrgOrderNumber, Guid? positionID, Guid? jobTitleID, string profileName, string CodeEmp, string userLogin)
        {
            string status = string.Empty;
            List<Hre_HDTJobEntity> lstReportHDTJobEntity = new List<Hre_HDTJobEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var basevices = new BaseService();
                var hdtJobServices = new Hre_HDTJobServices();
                var profileServices = new Hre_ProfileServices();
                List<object> listObjHDTJob = new List<object>();
                listObjHDTJob.Add(profileName);
                listObjHDTJob.Add(CodeEmp);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(lstOrgOrderNumber);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(null);
                listObjHDTJob.Add(1);
                listObjHDTJob.Add(Int32.MaxValue - 1);
                
                var lstHDTJob = hdtJobServices.GetData<Hre_HDTJobEntity>(listObjHDTJob, ConstantSql.hrm_hr_sp_get_HDTJob, userLogin, ref status).ToList();              
                if (lstHDTJob.Count == 0)
                {
                    return lstReportHDTJobEntity;
                }
                if (Datesearch != null )
                {
                    lstHDTJob = lstHDTJob.Where(s => s.DateFrom != null &&  s.DateTo != null && (s.DateFrom.Value.Date == Datesearch.Value.Date || s.DateTo.Value.Date == Datesearch.Value.Date)).ToList();
                }
                
                lstReportHDTJobEntity = profileServices.getHDTJobByPrice(lstHDTJob, null, null);
                lstReportHDTJobEntity = lstReportHDTJobEntity.OrderByDescending(s => s.CodeEmp).ThenBy(s => s.ProfileName).ThenBy(s => s.DateFrom).ToList();
            }
            return lstReportHDTJobEntity;
        }
    }
}