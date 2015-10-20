using HRM.Business.Hr.Models;
using HRM.Data.BaseRepository;
using System.Linq;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using VnResource.Helper.Data;
using HRM.Business.HrmSystem.Models;
using System.Data;
using System.Collections;
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;
using HRM.Business.HrmSystem.Domain;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Attendance.Models;
using HRM.Business.Payroll.Models;

namespace HRM.Business.Hr.Domain
{
    public class Hre_ProfileServices : BaseService
    {
        public bool CheckLeader(Guid LeaderID, Guid ProfileID)
        {
            bool rs = false;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var Profile = repoProfile.GetById(ProfileID);
                if (Profile.SupervisorID == LeaderID || Profile.HighSupervisorID == LeaderID)
                    rs = true;

            }
            return rs;
        }

        #region hàm lấy code của phòng ban
        /// <summary>
        /// hàm lấy code của phòng ban (thuộc cấp độ theo Level) đồng thời là cha của phòng ban con đc truyền vào
        /// </summary>
        /// <param name="OrgID">ID Phòng ban con được truyền vào </param>
        /// <param name="lstOrgAll">Tất cả DS phòng ban</param>
        /// <param name="levelOrg">Cấp độ của phòng ban 0: root; 1: cấp lớn sau root; 2 : là cấp con của cấp 1 </param>
        /// <returns></returns>
        public static string GetCodeInListOrgParent(Guid OrgID, List<Cat_OrgStructure> lstOrgAll, int levelOrg)
        {

            List<Guid> lstOrgIds = new List<Guid>(); //DS Org với cấp độ truyền vào
            List<Guid> orgRootIDs = lstOrgAll.Where(m => m.IsRoot == true && m.ParentID == null).Select(m => m.ID).ToList();
            lstOrgIds = orgRootIDs;
            for (int num = 0; num < levelOrg; num++)
            {
                lstOrgIds = getChildListOrgID(lstOrgIds, lstOrgAll);
            }
            //Qua bươc trên sẽ lấy đc DS orgID theo cấp độ 0 là root 1 là con của root

            Guid OrgIDResult = OrgID;

            for (int i = 0; i < lstOrgAll.Count; i++)
            {
                if (lstOrgAll.Any(m => m.ID == OrgIDResult && m.IsRoot == true && m.ParentID == null))
                    break;
                if (lstOrgIds.Contains(OrgIDResult))
                    break;
                if (i == lstOrgAll.Count)
                {
                    OrgIDResult = Guid.Empty;
                }

                OrgIDResult = getOrgParentID(OrgIDResult, lstOrgAll);

                if (OrgIDResult == null || OrgIDResult == Guid.Empty)
                {
                    break;
                }
            }

            string result = string.Empty;
            Cat_OrgStructure orgFinal = lstOrgAll.Where(m => m.ID == OrgIDResult).FirstOrDefault();
            if (orgFinal != null)
            {
                result = orgFinal.Code;
            }

            return result;
        }
        private static List<Guid> getChildListOrgID(List<Guid> orgIDs, List<Cat_OrgStructure> lstOrgAll)
        {
            List<Guid> result = lstOrgAll.Where(m => orgIDs.Contains(m.ParentID ?? Guid.Empty)).Select(m => m.ID).ToList();
            return result;
        }
        private static Guid getOrgParentID(Guid orgID, List<Cat_OrgStructure> lstOrgAll)
        {
            Guid result = lstOrgAll.Where(m => m.ID == orgID).Select(m => m.ParentID ?? Guid.Empty).FirstOrDefault();
            return result;
        }
        #endregion

        public void UpdateStatusApprovedProfile(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoWorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);
                var repoContract = new CustomBaseRepository<Hre_Contract>(unitOfWork);
                var repoBasicSalary = new CustomBaseRepository<Sal_BasicSalary>(unitOfWork);
                List<Guid> lstProfileIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                List<Hre_Profile> lstProfile = repoProfile.FindBy(x => lstProfileIds.Contains(x.ID)).ToList();
                var lstWorkHistoty = repoWorkHistory.FindBy(x => lstProfileIds.Contains(x.ProfileID) && x.IsDelete == null).ToList();
                var lstContract = repoContract.FindBy(x => lstProfileIds.Contains(x.ProfileID) && x.IsDelete == null).ToList();
                var lstBasicSalary = repoBasicSalary.FindBy(x => lstProfileIds.Contains(x.ProfileID) && x.IsDelete == null).ToList();
                // cập nhật StatusSyn="E_HIRE"
                foreach (var item in lstProfile)
                {
                    item.StatusSyn = ProfileStatusSyn.E_HIRE.ToString();
                }
                repoProfile.SaveChanges();
                if (lstWorkHistoty != null && lstWorkHistoty.Count != 0)
                {
                    // lấy ngày mới nhất để cập nhật Status là E_Approved (Hre_WorkHistory) dửa vào DateEffective
                    var WorkHistoty = lstWorkHistoty.OrderByDescending(x => x.DateEffective).FirstOrDefault();
                    WorkHistoty.Status = WorkHistoryStatus.E_APPROVED.ToString();
                    repoWorkHistory.SaveChanges();

                }
                if (lstContract != null && lstContract.Count != 0)
                {
                    // lấy ngày mới nhất để cập nhật Status là E_Approved (Hre_Contract) dựa vào DateSigned
                    var Contract = lstContract.OrderByDescending(x => x.DateSigned).FirstOrDefault();
                    Contract.Status = "E_APPROVED";
                    repoContract.SaveChanges();

                }
                if (lstBasicSalary != null && lstBasicSalary.Count != 0)
                {
                    // lấy ngày mới nhất để cập nhật Status là E_Approved (Sal_BasicSalary) dựa vào DateOfEffect
                    var BasicSalary = lstBasicSalary.OrderByDescending(x => x.DateOfEffect).FirstOrDefault();
                    BasicSalary.Status = "E_APPROVED";
                    repoBasicSalary.SaveChanges();

                }

            }

        }

        DataTable getSchema_PersionalInfo()
        {
            DataTable tblData = new DataTable("Hre_PersionalInformationEntity");
            tblData.Columns.Add("ProfileName");
            tblData.Columns.Add("CodeEmp");
            tblData.Columns.Add("DateHire", typeof(DateTime));
            tblData.Columns.Add("DayOfBirth", typeof(DateTime));
            tblData.Columns.Add("PlaceOfBirth");
            tblData.Columns.Add("Gender");
            tblData.Columns.Add("IDNo");
            tblData.Columns.Add("IDDateOfIssue");
            tblData.Columns.Add("IDPlaceOfIssue");
            tblData.Columns.Add("PassportNo");
            tblData.Columns.Add("PassportDateOfExpiry", typeof(DateTime));
            tblData.Columns.Add("PassportDateOfIssue", typeof(DateTime));
            tblData.Columns.Add("PassportPlaceOfIssue", typeof(DateTime));
            tblData.Columns.Add("PAddressID");
            tblData.Columns.Add("TAddressID");
            tblData.Columns.Add("JobTitleName");
            tblData.Columns.Add("PositionName");
            tblData.Columns.Add("DateQuit", typeof(DateTime));
            tblData.Columns.Add("ImagePath");

            tblData.Columns.Add("DateEffective", typeof(DateTime));
            tblData.Columns.Add("OrgStructureNew");
            tblData.Columns.Add("OrgStructureOld");
            tblData.Columns.Add("PositionNew");
            tblData.Columns.Add("PositionOld");
            tblData.Columns.Add("JobTitleNew");
            tblData.Columns.Add("JobTitleOld");
            tblData.Columns.Add("Note");

            //tblData.Columns.Add("DateEffective");
            //tblData.Columns.Add("DateEffective");
            //tblData.Columns.Add("DateEffective");
            //tblData.Columns.Add("DateEffective");

            tblData.Columns.Add("DateStart", typeof(DateTime));
            tblData.Columns.Add("DateEnd", typeof(DateTime));
            tblData.Columns.Add("ContractTypeID");
            tblData.Columns.Add("Salary");

            tblData.Columns.Add("DateOfViolation", typeof(DateTime));
            tblData.Columns.Add("ContentOfDiscipline");
            tblData.Columns.Add("ViolationRule");
            tblData.Columns.Add("DateOfEffective", typeof(DateTime));


            return tblData;
        }

        public DataTable GetPersionalInfo(string id, string userLogin, bool isCreateTemplate)
        {
            DataTable table = getSchema_PersionalInfo();
            if (isCreateTemplate)
            {
                return table.ConfigDatatable();
            }
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);

                var lstCat_JobTitle = repoCat_JobTitle.FindBy(s => s.Code != null).ToList();
                var lstCat_Position = repoCat_Position.FindBy(s => s.Code != null).ToList();
                var lstCat_OrgStructure = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                var objs = new List<object>();
                string status = string.Empty;
                var profile = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                objs.Add(profile.ID);
                var workHistory = GetData<Hre_WorkHistoryEntity>(objs, ConstantSql.hrm_hr_sp_get_WorkHistoryByProfileId, userLogin, ref status);
                var contract = GetData<Hre_ContractEntity>(objs, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, userLogin, ref status);
                var discipline = GetData<Hre_DisciplineEntity>(objs, ConstantSql.hrm_hr_sp_get_DisciplineprofileId, userLogin, ref status);

                int countMax = 0;
                if (profile != null)
                    countMax = 1;
                if (workHistory != null && workHistory.Count > countMax)
                    countMax = workHistory.Count;
                if (contract != null && contract.Count > countMax)
                    countMax = contract.Count;
                if (discipline != null && discipline.Count > countMax)
                    countMax = discipline.Count;

                for (int i = 0; i < countMax; i++)
                {
                    DataRow row = table.NewRow();
                    row["ProfileName"] = profile.ProfileName;
                    row["CodeEmp"] = profile.CodeEmp;
                    if (profile.DateHire != null)
                        row["DateHire"] = profile.DateHire;
                    if (profile.YearOfBirth != null && profile.MonthOfBirth != null && profile.DayOfBirth != null)
                        row["DayOfBirth"] = new DateTime(profile.YearOfBirth.Value, profile.MonthOfBirth.Value, profile.DayOfBirth.Value);
                    row["PlaceOfBirth"] = profile.PlaceOfBirth;
                    row["Gender"] = profile.Gender;
                    row["IDNo"] = profile.IDNo;
                    row["IDDateOfIssue"] = profile.IDDateOfIssue;
                    row["IDPlaceOfIssue"] = profile.IDPlaceOfIssue;
                    row["PassportNo"] = profile.PassportNo;
                    if (profile.PassportDateOfExpiry != null)
                        row["PassportDateOfExpiry"] = profile.PassportDateOfExpiry;
                    if (profile.PassportDateOfIssue != null)
                        row["PassportDateOfIssue"] = profile.PassportDateOfIssue;
                    if (profile.PassportPlaceOfIssue != null)
                        row["PassportPlaceOfIssue"] = profile.PassportPlaceOfIssue;
                    row["PAddressID"] = profile.PAddressID;
                    row["TAddressID"] = profile.TAddressID;
                    if (profile.PositionID != null)
                    {
                        var pos = lstCat_Position.Where(s => s.ID == profile.PositionID).FirstOrDefault();
                        row["PositionName"] = pos.Code;
                    }
                    if (profile.JobTitleID != null)
                    {
                        var job = lstCat_JobTitle.Where(s => s.ID == profile.JobTitleID).FirstOrDefault();
                        row["JobTitleName"] = job.Code;
                    }

                    if (profile.DateQuit != null)
                        row["DateQuit"] = profile.DateQuit;
                    row["ImagePath"] = profile.ImagePath;

                    if (workHistory != null && workHistory.Count > 0)
                    {
                        if (workHistory[i].DateEffective != null)
                            row["DateEffective"] = workHistory[i].DateEffective;
                        if (workHistory[i].OrganizationStructureID != null)
                        {
                            var orgN = lstCat_OrgStructure.Where(s => s.ID == workHistory[i].OrganizationStructureID).FirstOrDefault();
                            row["OrgStructureNew"] = orgN.Code;
                        }
                        row["OrgStructureOld"] = workHistory[i].OrgOld;
                        if (workHistory[i].PositionID != null)
                        {
                            var posN = lstCat_Position.Where(s => s.ID == workHistory[i].PositionID).FirstOrDefault();
                            row["PositionNew"] = posN.Code;
                        }
                        row["PositionOld"] = workHistory[i].PositionOld;
                        if (workHistory[i].JobTitleID != null)
                        {
                            var jobN = lstCat_JobTitle.Where(s => s.ID == workHistory[i].JobTitleID).FirstOrDefault();
                            row["JobTitleNew"] = jobN.Code;
                        }
                        row["JobTitleOld"] = workHistory[i].JobTitleOld;
                        row["Note"] = workHistory[i].Note;
                    }

                    if (contract != null && contract.Count > 0)
                    {
                        if (contract[i].DateStart != null)
                            row["DateStart"] = contract[i].DateStart;
                        if (contract[i].DateEnd != null)
                            row["DateEnd"] = contract[i].DateEnd;
                        row["ContractTypeID"] = contract[i].ContractTypeID;
                        row["Salary"] = contract[i].Salary;
                    }

                    if (discipline != null && discipline.Count > 0)
                    {
                        if (discipline[i].DateOfViolation != null)
                            row["DateOfViolation"] = discipline[i].DateOfViolation;
                        row["ContentOfDiscipline"] = discipline[i].ContentOfDiscipline;
                        row["ViolationRule"] = discipline[i].ViolationRule;
                        if (discipline[i].DateOfEffective != null)
                            row["DateOfEffective"] = discipline[i].DateOfEffective;
                    }
                    table.Rows.Add(row);
                }

                return table.ConfigTable();
            }
        }

        public List<DataTable> GetPersionalInformation(string id, bool isCreateTemplate, string userLogin)
        {
            List<DataTable> lstTable = new List<DataTable>();
            DataTable table = getSchema_PersionalInfo();
            lstTable.Add(table);
            if (isCreateTemplate)
            {
                return lstTable;
            }
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);

                var lstCat_JobTitle = repoCat_JobTitle.FindBy(s => s.Code != null).ToList();
                var lstCat_Position = repoCat_Position.FindBy(s => s.Code != null).ToList();
                var lstCat_OrgStructure = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                var objs = new List<object>();
                string status = string.Empty;
                var profile = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                objs.Add(profile.ID);
                var workHistory = GetData<Hre_WorkHistoryEntity>(objs, ConstantSql.hrm_hr_sp_get_WorkHistoryByProfileId, userLogin, ref status);
                var contract = GetData<Hre_ContractEntity>(objs, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, userLogin, ref status);
                var discipline = GetData<Hre_DisciplineEntity>(objs, ConstantSql.hrm_hr_sp_get_DisciplineprofileId, userLogin, ref status);

                int countMax = 0;
                if (profile != null)
                    countMax = 1;
                if (workHistory != null && workHistory.Count > countMax)
                    countMax = workHistory.Count;
                if (contract != null && contract.Count > countMax)
                    countMax = contract.Count;
                if (discipline != null && discipline.Count > countMax)
                    countMax = discipline.Count;

                for (int i = 0; i < countMax; i++)
                {
                    DataRow row = table.NewRow();
                    row["ProfileName"] = profile.ProfileName;
                    row["CodeEmp"] = profile.CodeEmp;
                    if (profile.DateHire != null)
                        row["DateHire"] = profile.DateHire;
                    if (profile.YearOfBirth != null && profile.MonthOfBirth != null && profile.DayOfBirth != null)
                        row["DayOfBirth"] = new DateTime(profile.YearOfBirth.Value, profile.MonthOfBirth.Value, profile.DayOfBirth.Value);
                    row["PlaceOfBirth"] = profile.PlaceOfBirth;
                    row["Gender"] = profile.Gender;
                    row["IDNo"] = profile.IDNo;
                    row["IDDateOfIssue"] = profile.IDDateOfIssue;
                    row["IDPlaceOfIssue"] = profile.IDPlaceOfIssue;
                    row["PassportNo"] = profile.PassportNo;
                    if (profile.PassportDateOfExpiry != null)
                        row["PassportDateOfExpiry"] = profile.PassportDateOfExpiry;
                    if (profile.PassportDateOfIssue != null)
                        row["PassportDateOfIssue"] = profile.PassportDateOfIssue;
                    if (profile.PassportPlaceOfIssue != null)
                        row["PassportPlaceOfIssue"] = profile.PassportPlaceOfIssue;
                    row["PAddressID"] = profile.PAddressID;
                    row["TAddressID"] = profile.TAddressID;
                    if (profile.PositionID != null)
                    {
                        var pos = lstCat_Position.Where(s => s.ID == profile.PositionID).FirstOrDefault();
                        row["PositionName"] = pos.Code;
                    }
                    if (profile.JobTitleID != null)
                    {
                        var job = lstCat_JobTitle.Where(s => s.ID == profile.JobTitleID).FirstOrDefault();
                        row["JobTitleName"] = job.Code;
                    }

                    if (profile.DateQuit != null)
                        row["DateQuit"] = profile.DateQuit;
                    row["ImagePath"] = profile.ImagePath;

                    if (workHistory != null && workHistory.Count > 0)
                    {
                        if (workHistory[i].DateEffective != null)
                            row["DateEffective"] = workHistory[i].DateEffective;
                        if (workHistory[i].OrganizationStructureID != null)
                        {
                            var orgN = lstCat_OrgStructure.Where(s => s.ID == workHistory[i].OrganizationStructureID).FirstOrDefault();
                            row["OrgStructureNew"] = orgN.Code;
                        }
                        row["OrgStructureOld"] = workHistory[i].OrgOld;
                        if (workHistory[i].PositionID != null)
                        {
                            var posN = lstCat_Position.Where(s => s.ID == workHistory[i].PositionID).FirstOrDefault();
                            row["PositionNew"] = posN.Code;
                        }
                        row["PositionOld"] = workHistory[i].PositionOld;
                        if (workHistory[i].JobTitleID != null)
                        {
                            var jobN = lstCat_JobTitle.Where(s => s.ID == workHistory[i].JobTitleID).FirstOrDefault();
                            row["JobTitleNew"] = jobN.Code;
                        }
                        row["JobTitleOld"] = workHistory[i].JobTitleOld;
                        row["Note"] = workHistory[i].Note;
                    }

                    if (contract != null && contract.Count > 0)
                    {
                        if (contract[i].DateStart != null)
                            row["DateStart"] = contract[i].DateStart;
                        if (contract[i].DateEnd != null)
                            row["DateEnd"] = contract[i].DateEnd;
                        row["ContractTypeID"] = contract[i].ContractTypeID;
                        row["Salary"] = contract[i].Salary;
                    }

                    if (discipline != null && discipline.Count > 0)
                    {
                        if (discipline[i].DateOfViolation != null)
                            row["DateOfViolation"] = discipline[i].DateOfViolation;
                        row["ContentOfDiscipline"] = discipline[i].ContentOfDiscipline;
                        row["ViolationRule"] = discipline[i].ViolationRule;
                        if (discipline[i].DateOfEffective != null)
                            row["DateOfEffective"] = discipline[i].DateOfEffective;
                    }
                    table.Rows.Add(row);
                }

                return lstTable;
            }
        }

        public Hre_ContractEntity SetNewDateEndContract(Hre_ContractEntity contractEntity, string userLogin)
        {
            if (contractEntity != null)
            {
                var service = new BaseService();
                var status = string.Empty;
                var objContractType = service.GetFirstData<Cat_ContractTypeEntity>(contractEntity.ContractTypeID, ConstantSql.hrm_cat_sp_get_ContractTypeById, userLogin, ref status);
                if (objContractType != null)
                {
                    var contractTypeEntity = (Cat_ContractTypeEntity)objContractType;
                    if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                    {
                        var formula = contractTypeEntity.Formula.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        ElementFormula elementContactType = new ElementFormula("ContractType", contractTypeEntity.Type, 0);
                        ElementFormula elementUnitType = new ElementFormula("UnitType", contractTypeEntity.UnitTime, 0);
                        ElementFormula elementValueTime = new ElementFormula("ValueTime", contractTypeEntity.ValueTime, 0);
                        ElementFormula elementDateStart = new ElementFormula("DateStart", contractEntity.DateStart, 0);

                        var result = FormulaHelper.ParseFormula(formula, new List<ElementFormula>() { elementContactType, elementUnitType, elementDateStart, elementValueTime });

                        if (result != null && string.IsNullOrEmpty(result.ErrorMessage))
                        {
                            contractEntity.DateEnd = (DateTime)result.Value;
                        }
                    }
                }

            }
            return contractEntity;
        }

        public void AddDataForContract(string BasicSalary, string ProfileIDs, Guid ContractTypeID, DateTime DateHire, Guid SalaryRankID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var repoContract = new Hre_ContractRepository(unitOfWork);
                List<Hre_ContractEntity> lstContract = new List<Hre_ContractEntity>();
                var contractServices = new Hre_ContractServices();
                List<Guid> lstProfileIDs = ProfileIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                var repoSalaryRank = new CustomBaseRepository<Cat_SalaryRank>(unitOfWork);
                var repoSalaryClass = new CustomBaseRepository<Cat_SalaryClass>(unitOfWork);
                // var salaryRank = repoSalaryRank.FindBy(s => s.IsDelete == null && s.ID == SalaryRankID).FirstOrDefault();
                //  var salaryClass = repoSalaryClass.FindBy(s => s.IsDelete == null && s.ID == salaryRank.SalaryClassID).FirstOrDefault();
                var lstObjContractType = new List<object>();
                lstObjContractType.AddRange(new object[6]);
                lstObjContractType[4] = 1;
                lstObjContractType[5] = int.MaxValue - 1;
                var lstContractType = GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, userLogin, ref status).ToList();

                var salaryRankServices = new Cat_SalaryRankServices();
                var lstObjRank = new List<object>();
                lstObjRank.Add(null); lstObjRank.Add(null);
                lstObjRank.Add(1);
                lstObjRank.Add(int.MaxValue - 1);
                var lstRank = salaryRankServices.GetData<Cat_SalaryRankEntity>(lstObjRank, ConstantSql.hrm_cat_sp_get_SalaryRank, userLogin, ref status).ToList().Translate<Cat_SalaryRank>();
                var rankEntity = lstRank.Where(s => s.ID == SalaryRankID).FirstOrDefault();

                var salaryClassServices = new Cat_SalaryClassServices();
                var lstObjClass = new List<object>();
                lstObjClass.Add(null);
                lstObjClass.Add(1);
                lstObjClass.Add(int.MaxValue - 1);
                var salaryClass = salaryClassServices.GetData<Cat_SalaryClassEntity>(lstObjClass, ConstantSql.hrm_cat_sp_get_SalaryClass, userLogin, ref status).ToList().Translate<Cat_SalaryClass>();
                var salaryClassEntity = salaryClass.Where(s => rankEntity.SalaryClassID == s.ID).FirstOrDefault();
                //  var contractType = lstContractType.Where(s => s.ID == ContractTypeID).FirstOrDefault();
                foreach (var item in lstProfileIDs)
                {
                    //model.ProfileID = profile.ID;
                    var objContract = new List<object>();
                    objContract.Add(item);
                    var lstContractByProfileID = contractServices.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, userLogin, ref status);
                    var listIdContract = string.Empty;
                    if (lstContractByProfileID != null)
                    {
                        listIdContract = string.Join(",", lstContractByProfileID.Select(d => d.ContractTypeID));
                    }
                    var contractType = lstContractType.Where(s => s.ID == ContractTypeID).FirstOrDefault();
                    DateTime dateEnd = DateHire;
                    if (contractType != null)
                    {
                        if (contractType.ValueTime != null)
                        {
                            //month = (int)contractType.ValueTime.Value;
                            //if (contractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                            //{
                            //    month = month * 12;
                            //}
                            if (contractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_MONTH.ToString())
                            {
                                dateEnd = DateHire.AddMonths(int.Parse(contractType.ValueTime.Value.ToString()));
                            }
                            else if (contractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                            {
                                dateEnd = DateHire.AddYears(int.Parse(contractType.ValueTime.Value.ToString()));
                            }
                        }
                    }
                    double Salary = 0;
                    if (!string.IsNullOrEmpty(BasicSalary))
                        Salary = double.Parse(BasicSalary);
                    Hre_ContractEntity Contract = new Hre_ContractEntity();
                    Contract.ProfileID = item;
                    Contract.Salary = Salary;
                    Contract.ContractTypeID = ContractTypeID;
                    Contract.DateStart = DateHire;
                    Contract.DateSigned = DateHire;
                    Contract.DateEnd = dateEnd;


                    Contract.RankRateID = SalaryRankID;
                    Contract.ClassRateID = salaryClassEntity.ID;
                    if (!string.IsNullOrEmpty(contractType.Formula))
                    {
                        Contract = SetNewDateEndContract(Contract, userLogin);
                    }
                    Contract = SetNewCodeContract(Contract, listIdContract, userLogin);
                    lstContract.Add(Contract);
                }

                contractServices.Add(lstContract);
                repoContract.SaveChanges();
            }
        }
        public void UpdateSalaryClassNameForProfile(string SalaryClassName, string ProfileIDs, DateTime DateEndProbation, DateTime DateHire, Guid OrgStructureID, Guid SalaryRankID, Guid WorkPlaceID, Guid ContractTypeID, string BasicSalary, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = string.Empty;
                var repo = new Hre_ProfileRepository(unitOfWork);
                var profileSevices = new Hre_ProfileServices();
                var repoWorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);
                var workingHistoryServices = new Hre_WorkHistoryServices();
                // var insuranceServices = new Sal_InsuranceSalaryServices();
                var ObjProfile = new List<object>();
                ObjProfile.AddRange(new object[17]);
                ObjProfile[15] = 1;
                ObjProfile[16] = int.MaxValue - 1;


                List<Guid> lstProfileIDs = ProfileIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstProfiles = profileSevices.GetData<Hre_ProfileEntity>(ObjProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, userLogin, ref status).Where(s => lstProfileIDs.Contains(s.ID)).ToList();
                var contractServices = new Hre_ContractServices();
                var repoContract = new Hre_ContractRepository(unitOfWork);
                List<Hre_ContractEntity> lstContract = new List<Hre_ContractEntity>();

                var salaryRankServices = new Cat_SalaryRankServices();
                var lstObjRank = new List<object>();
                lstObjRank.Add(null);
                lstObjRank.Add(null);
                lstObjRank.Add(1);
                lstObjRank.Add(int.MaxValue - 1);
                var lstRank = salaryRankServices.GetData<Cat_SalaryRankEntity>(lstObjRank, ConstantSql.hrm_cat_sp_get_SalaryRank, userLogin, ref status).ToList().Translate<Cat_SalaryRank>();
                var rankEntity = lstRank.Where(s => s.ID == SalaryRankID).FirstOrDefault();

                var salaryClassServices = new Cat_SalaryClassServices();
                var lstObjClass = new List<object>();
                lstObjClass.Add(null);
                lstObjClass.Add(1);
                lstObjClass.Add(int.MaxValue - 1);
                var salaryClass = salaryClassServices.GetData<Cat_SalaryClassEntity>(lstObjClass, ConstantSql.hrm_cat_sp_get_SalaryClass, userLogin, ref status).ToList().Translate<Cat_SalaryClass>();
                var salaryClassEntity = salaryClass.Where(s => rankEntity.SalaryClassID == s.ID).FirstOrDefault();

                var lstObjContractType = new List<object>();
                lstObjContractType.AddRange(new object[6]);
                lstObjContractType[4] = 1;
                lstObjContractType[5] = int.MaxValue - 1;
                var lstContractType = GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, userLogin, ref status).ToList();

                var candidateServices = new Hre_CandidateGeneralServices();
                var objCan = new List<object>();
                objCan.AddRange(new object[14]);
                objCan[12] = 1;
                objCan[13] = int.MaxValue - 1;
                var lstCan = candidateServices.GetData<Hre_CandidateGeneralEntity>(objCan, ConstantSql.hrm_hr_sp_get_CandidateGeneral, userLogin, ref status).ToList();

                var lstProfile = new List<Hre_ProfileEntity>();
                var workplaceServices = new Cat_WorkPlaceServices();
                var workplace = workplaceServices.GetData<Cat_WorkPlaceEntity>(Common.DotNetToOracle(WorkPlaceID.ToString()), ConstantSql.hrm_cat_sp_get_WorkPlaceById, userLogin, ref status).FirstOrDefault();

                foreach (var item in lstProfiles)
                {
                    var canEntity = lstCan.Where(s => s.ProfileID.Value == item.ID).FirstOrDefault();
                    if (canEntity == null)
                    {
                        //Add new contract
                        var objContract = new List<object>();
                        objContract.Add(item.ID);
                        var lstContractByProfileID = contractServices.GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_ContractsByProfileId, userLogin, ref status);
                        var listIdContract = string.Empty;
                        if (lstContractByProfileID != null)
                        {
                            listIdContract = string.Join(",", lstContractByProfileID.Select(d => d.ContractTypeID));
                        }
                        var contractType = lstContractType.Where(s => s.ID == ContractTypeID).FirstOrDefault();
                        DateTime dateEnd = DateHire;
                        if (contractType != null)
                        {
                            if (contractType.ValueTime != null)
                            {
                                //month = (int)contractType.ValueTime.Value;
                                //if (contractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                                //{
                                //    month = month * 12;
                                //}
                                if (contractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_MONTH.ToString())
                                {
                                    dateEnd = DateHire.AddMonths(int.Parse(contractType.ValueTime.Value.ToString()));
                                }
                                else if (contractType.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                                {
                                    dateEnd = DateHire.AddYears(int.Parse(contractType.ValueTime.Value.ToString()));
                                }
                            }
                        }
                        double Salary = 0;
                        if (!string.IsNullOrEmpty(BasicSalary))
                            Salary = double.Parse(BasicSalary);
                        Hre_ContractEntity Contract = new Hre_ContractEntity();
                        Contract.ProfileID = item.ID;
                        Contract.Salary = Salary;
                        Contract.ContractTypeID = ContractTypeID;
                        Contract.DateStart = DateHire;
                        Contract.DateSigned = DateHire;
                        Contract.DateEnd = dateEnd;

                        Contract.RankRateID = SalaryRankID;
                        Contract.ClassRateID = salaryClassEntity.ID;
                        if (!string.IsNullOrEmpty(contractType.Formula))
                        {
                            Contract = SetNewDateEndContract(Contract, userLogin);
                        }
                        Contract = SetNewCodeContract(Contract, listIdContract, userLogin);
                        contractServices.Add(Contract);


                        //Edit Profile
                        item.OrgStructureID = OrgStructureID;
                        item.SalaryClassID = salaryClassEntity == null ? Guid.Empty : salaryClassEntity.ID;
                        item.DateOfEffect = DateHire;
                        item.DateHire = DateHire;
                        item.DateEndProbation = Contract.DateEnd.Value;
                        item.WorkPlaceID = WorkPlaceID;
                        item.ContractTypeID = ContractTypeID;

                        profileSevices.Edit(item);

                        //// Add Insurance
                        //if (contractType != null && contractType.NoneTypeInsuarance == true)
                        //{
                        //    var insuranceEntity = new Sal_InsuranceSalaryEntity
                        //    {
                        //        ProfileID = item.ID,
                        //        InsuranceAmount = model.InsuranceAmount,
                        //        DateEffect = DateHire,
                        //        IsSocialIns = contractType.IsSocialInsurance == null ? null : contractType.IsSocialInsurance,
                        //        IsUnimploymentIns = contractType.IsUnEmployInsurance == null ? null : contractType.IsUnEmployInsurance,
                        //        IsMedicalIns = contractType.IsHealthInsurance == null ? null : contractType.IsHealthInsurance,
                        //        CurrencyID = model.CurenncyID1
                        //    };
                        //    insuranceServices.Add(insuranceEntity);
                        //}

                        Hre_WorkHistoryEntity workHistory = new Hre_WorkHistoryEntity();
                        workHistory.ProfileID = item.ID;
                        workHistory.DateEffective = DateHire;
                        workHistory.SalaryClassID = salaryClassEntity == null ? Guid.Empty : salaryClassEntity.ID;
                        workHistory.OrganizationStructureID = OrgStructureID;
                        workHistory.WorkLocation = workplace != null ? workplace.WorkPlaceName : null;

                        workingHistoryServices.Add(workHistory);
                    }


                }

            }
        }
        #region Cat_Org
        public Dictionary<Guid, OrgNameClass> GetOrgFullLink(List<Guid> lstOrgIDs, List<Cat_OrgStructure> lstOrgStructure)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_NameEntity = new CustomBaseRepository<Cat_NameEntity>(unitOfWork);

                Dictionary<Guid, OrgNameClass> OrgNameAllLevel = new Dictionary<Guid, OrgNameClass>();
                string E_BRANCH = OrgUnit.E_BRANCH.ToString();
                string E_DEPARTMENT = OrgUnit.E_DEPARTMENT.ToString();
                string E_SECTION = OrgUnit.E_SECTION.ToString();
                string E_TEAM = OrgUnit.E_TEAM.ToString();
                var lstAllTypeStructure = repoCat_NameEntity.FindBy(m => m.EnumType == E_BRANCH
                    || m.EnumType == E_DEPARTMENT
                    || m.EnumType == E_SECTION
                    || m.EnumType == E_TEAM).Select(m => new { m.ID, m.EnumType }).ToList();
                Guid Type_BRANCH_ID = lstAllTypeStructure.Where(m => m.EnumType == E_BRANCH).Select(m => m.ID).FirstOrDefault();
                Guid Type_DEPARTMENT_ID = lstAllTypeStructure.Where(m => m.EnumType == E_DEPARTMENT).Select(m => m.ID).FirstOrDefault();
                Guid Type_SECTION_ID = lstAllTypeStructure.Where(m => m.EnumType == E_SECTION).Select(m => m.ID).FirstOrDefault();
                Guid Type_TEAM_ID = lstAllTypeStructure.Where(m => m.EnumType == E_TEAM).Select(m => m.ID).FirstOrDefault();


                foreach (var item in lstOrgIDs)
                {
                    OrgNameClass OrgNameLevel = new OrgNameClass();
                    RecursionOrgGetName(lstOrgStructure, item, Type_DEPARTMENT_ID, Type_SECTION_ID, Type_BRANCH_ID, Type_TEAM_ID, OrgNameLevel);
                    if (!OrgNameAllLevel.ContainsKey(item))
                        OrgNameAllLevel.Add(item, OrgNameLevel);
                }
                return OrgNameAllLevel;
            }
        }
        private void RecursionOrgGetName(List<Cat_OrgStructure> lstOrgAll, Guid OrgCurrentID, Guid TypeDeparmentID, Guid TypeSectionID, Guid TypeBrandID, Guid TypeTeamID, OrgNameClass result)
        {

            var currentOrg = lstOrgAll.Where(m => m.ID == OrgCurrentID).FirstOrDefault();
            if (currentOrg == null)
                return;


            if (TypeDeparmentID != null && TypeDeparmentID != Guid.Empty && currentOrg.TypeID == TypeDeparmentID && result.DepartmentName == null)
            {
                result.DepartmentCode = currentOrg.Code;
                result.DepartmentName = currentOrg.OrgStructureName;
            }
            if (TypeSectionID != null && TypeSectionID != Guid.Empty && currentOrg.TypeID == TypeSectionID && result.SectionName == null)
            {
                result.SectionCode = currentOrg.Code;
                result.SectionName = currentOrg.OrgStructureName;
            }
            if (TypeBrandID != null && TypeBrandID != Guid.Empty && currentOrg.TypeID == TypeBrandID && result.BrandName == null)
            {
                result.BrandCode = currentOrg.Code;
                result.BrandName = currentOrg.OrgStructureName;
            }
            if (TypeTeamID != null && TypeTeamID != Guid.Empty && currentOrg.TypeID == TypeTeamID && result.TeamName == null)
            {
                result.TeamCode = currentOrg.Code;
                result.TeamName = currentOrg.OrgStructureName;
            }
            if (currentOrg.ParentID == null || currentOrg.ParentID == Guid.Empty)
            {
                return;
            }
            else
            {
                Guid ParentID = currentOrg.ParentID ?? Guid.Empty;
                RecursionOrgGetName(lstOrgAll, ParentID, TypeDeparmentID, TypeSectionID, TypeBrandID, TypeTeamID, result);
            }
        }
        #endregion

        public void SubmitStatus(List<Guid> selectedIds, string status, string userID, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var baseServices = new BaseService();
                string statuss = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_ProfileRepository(unitOfWork);
                var lstProfiles = repo.FindBy(m => m.ID != null && selectedIds.Contains(m.ID)).ToList();

                Guid guidUserID = Guid.Empty;
                if (!string.IsNullOrEmpty(userID))
                {
                    try
                    {
                        Guid.TryParse(userID, out guidUserID);
                    }
                    catch (Exception)
                    {
                    }
                }

                foreach (var Profile in lstProfiles)
                {
                    if (Profile.ContractTypeID != null)
                    {
                        var contractypebyProfile = baseServices.GetData<Cat_ContractTypeEntity>(Common.DotNetToOracle(Profile.ContractTypeID.ToString()), ConstantSql.hrm_cat_sp_get_ContractTypeById, userLogin, ref status).FirstOrDefault();
                        if (contractypebyProfile != null &&
                            (contractypebyProfile.Type == HRM.Infrastructure.Utilities.EnumDropDown.TypeContract.E_PROBATION.ToString()
                            || contractypebyProfile.Type == HRM.Infrastructure.Utilities.EnumDropDown.TypeContract.E_APPRENTICESHIP.ToString())
                          )
                        {
                            Profile.StatusSyn = ProfileStatusSyn.E_WAITING_APPROVE.ToString();
                        }
                        else
                        {
                            Profile.StatusSyn = ProfileStatusSyn.E_HIRE.ToString();
                        }
                    }
                    else
                    {
                        Profile.StatusSyn = ProfileStatusSyn.E_HIRE.ToString();
                    }
                }

                unitOfWork.GenerateCode(guidUserID, "CodeEmp", lstProfiles.ToArray());

                repo.SaveChanges();
            }
        }

        public static List<Guid> GetProfileIdList(List<Hre_Profile> lstProfile)
        {
            List<Guid> res = new List<Guid>();
            if (lstProfile == null)
                throw new Exception("List Profile Null");
            res = lstProfile.Select(p => p.ID).ToList();
            //

            return res;
        }

        public List<Hre_Profile> GetWorkingProfile(List<Hre_ProfileEntity> listPro, DateTime from, DateTime to)
        {
            List<Hre_Profile> listProfile = GetWorkingProfile(listPro, from, to, false);

            return listProfile;
        }

        public List<Hre_Profile> GetWorkingProfile(List<Hre_ProfileEntity> listPro, DateTime from, DateTime to, bool isIncludeQuitEmp)
        {
            string[] strInclude = new string[] { };
            List<Hre_Profile> listProfile = GetWorkingProfile(listPro, from, to, strInclude, isIncludeQuitEmp);
            return listProfile;
        }

        public List<Hre_Profile> GetWorkingProfile(List<Hre_ProfileEntity> listPro, DateTime from, DateTime to, string[] strInclude, bool isIncludeQuitEmp)
        {
            string waitStatus = ProfileStatusSyn.E_WAITING.ToString();
            List<Hre_Profile> listProfile = listPro.Where(pro => pro.StatusSyn != waitStatus).ToList().Translate<Hre_Profile>();

            if (!isIncludeQuitEmp)
            {
                listProfile = listProfile.Where(pro => (pro.DateQuit == null || pro.DateQuit.Value > from) && pro.DateHire <= to).ToList();
            }

            return listProfile;
        }

        /// <summary>
        /// Hàm lấy ra danh sách profile dựa theo danh sách Phòng Ban có sẵn và có kiểm tra timeLine history 
        /// !! Hàm có thể thay đổi profile với Phòng ban nên phải cẩn thận khi Lưu dữ liệu
        /// </summary>
        /// <param name="guiContext"></param>
        /// <param name="userID"></param>
        /// <param name="lstProfile"></param>
        /// <param name="lstOrgAll"></param>
        /// <param name="date">Tháng kiểm tra</param>
        /// <param name="isSave">!IMPORTANCE là True khi có thưc hiện thao tác save sau khi sử dụng hàm nay</param>
        /// <returns>trả về ds profile có sẵn với cái payroll dúng với tháng đang search</returns>
        public List<Hre_Profile> GetProfileWithOrgByHistory(List<Hre_Profile> lstProfile, List<Cat_OrgStructure> lstOrgAll, List<Hre_WorkHistory> lstWorkHistory, DateTime date, bool isSave)
        {
            List<Hre_Profile> lstProfileResult = new List<Hre_Profile>();
            lstWorkHistory = lstWorkHistory.OrderByDescending(m => m.DateEffective).ToList();
            foreach (Hre_Profile profile in lstProfile)
            {
                var historyByProfile = lstWorkHistory.Where(m => m.ProfileID == profile.ID).FirstOrDefault();
                if (historyByProfile != null && historyByProfile.OrganizationStructureID != null) // co histoty
                {
                    if (historyByProfile.OrganizationStructureID != profile.OrgStructureID)// co thay doi phong ban
                    {
                        Cat_OrgStructure org = lstOrgAll.Where(m => m.ID == historyByProfile.OrganizationStructureID).FirstOrDefault();
                        if (org != null)
                        {
                            if (!isSave)
                            {
                                profile.Cat_OrgStructure = org;
                            }
                            lstProfileResult.Add(profile);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else // không thay đổi phòng ban
                    {
                        Cat_OrgStructure org = lstOrgAll.Where(m => m.ID == historyByProfile.OrganizationStructureID).FirstOrDefault();
                        if (org != null) //Thoa theo nhom luong dang Search
                        {
                            lstProfileResult.Add(profile);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    Cat_OrgStructure org = lstOrgAll.Where(m => m.ID == profile.OrgStructureID).FirstOrDefault();
                    if (org != null) //Thoa theo nhom luong dang Search
                    {
                        lstProfileResult.Add(profile);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return lstProfileResult;
        }

        /// <summary>
        /// Hàm lấy ra danh sách profile dựa theo danh sách nhóm lương có sẵn và có kiểm tra timeLine history 
        /// !! Hàm có thể thay đổi profile với payrollGroup nên phải cẩn thận khi Lưu dữ liệu
        /// </summary>
        /// <param name="guiContext"></param>
        /// <param name="userID"></param>
        /// <param name="lstProfile"></param>
        /// <param name="lstPayrollGroup"></param>
        /// <param name="date">Tháng kiểm tra</param>
        /// <param name="isSave">!IMPORTANCE là True khi có thưc hiện thao tác save sau khi sử dụng hàm nay</param>
        /// <returns>trả về ds profile có sẵn với cái payroll dúng với tháng đang search</returns>
        public List<Hre_Profile> GetProfileWithPayrollGroupByHistory(List<Hre_Profile> lstProfile, List<Cat_PayrollGroup> lstPayrollGroup, List<Hre_WorkHistory> lstWorkHistory, DateTime date, bool isSave)
        {

            List<Hre_Profile> lstProfileResult = new List<Hre_Profile>();
            //List<Hre_WorkHistory> lstWorkHistoryCheck = lstWorkHistory.Where(m => m.DateEffective <= date).ToList();
            //var histories = EntityService.CreateQueryable<Hre_WorkHistory>(guiContext, userID, s => s.PayrollGroupID != null
            //    && (s.DateEffective < date || s.DateEffective.Year == date.Year && s.DateEffective.Month == date.Month))
            //    .Select(s => new { s.ProfileID, s.DateEffective, s.PayrollGroupID }).OrderByDescending(s => s.DateEffective).Excute();
            lstWorkHistory = lstWorkHistory.OrderByDescending(m => m.DateEffective).ToList();
            foreach (Hre_Profile profile in lstProfile)
            {
                var historyByProfile = lstWorkHistory.Where(m => m.ProfileID == profile.ID).FirstOrDefault();
                if (historyByProfile != null && historyByProfile.PayrollGroupID != null) // co histoty
                {
                    if (historyByProfile.PayrollGroupID != profile.PayrollGroupID)// co thay doi nhom luong
                    {
                        Cat_PayrollGroup payroll = lstPayrollGroup.Where(m => m.ID == historyByProfile.PayrollGroupID).FirstOrDefault();
                        if (payroll != null)
                        {
                            if (!isSave)
                            {
                                profile.Cat_PayrollGroup = payroll;
                            }
                            lstProfileResult.Add(profile);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else // Neu khong thay doi nhom luong
                    {
                        Cat_PayrollGroup payroll = lstPayrollGroup.Where(m => m.ID == historyByProfile.PayrollGroupID).FirstOrDefault();
                        if (payroll != null) //Thoa theo nhom luong dang Search
                        {
                            lstProfileResult.Add(profile);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else// neu khong co history
                {
                    Cat_PayrollGroup payroll = lstPayrollGroup.Where(m => m.ID == profile.PayrollGroupID).FirstOrDefault();
                    if (payroll != null) //Thoa theo nhom luong dang Search
                    {
                        lstProfileResult.Add(profile);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return lstProfileResult;
        }

        // Son.Vo - Kiểm tra có bắt trùng tên nv hay không - dùng cho BDF
        public bool IsCheckDuplidateProfileName(string userLogin)
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);

            if (config == null)
            {
                return false;
            }
            string valueConfig = AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATEPROFILENAME.ToString();

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        // Son.Vo - Kiểm tra có bắt trùng mã nv hay không - dùng cho BDF
        public bool IsCheckDuplidateCodeEmp()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);

            string valueConfig = AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATECODEEMP.ToString();
            if (config == null)
            {
                return false;
            }
            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        // Son.Vo - Kiểm tra có bắt trùng mã chấm công hay không - dùng cho BDF
        public bool IsCheckDuplidateCodeAtt()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            if (config == null)
            {
                return false;
            }
            string valueConfig = AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATECODEATT.ToString();

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        // Son.Vo - Kiểm tra có bắt trùng tên người thân hay không - dùng cho BDF
        public bool IsCheckDuplidateRelatives()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            if (config == null)
            {
                return false;
            }
            string valueConfig = AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATERELATIVES.ToString();

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        // Son.Vo - Kiểm tra có bắt trùng tên NV + Ngày Sinh
        public bool IsCheckDuplidateProfileNameAndBirthDay()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            if (config == null)
            {
                return false;
            }
            string valueConfig = AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATEPROFILENAMEANDBIRTHDAY.ToString();

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        // Son.Vo - Kiểm tra có bắt trùng CMND
        public bool IsCheckDuplidateIDNo()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            if (config == null)
            {
                return false;
            }
            string valueConfig = AppConfig.HRM_HRE_CONFIG_CHECKDUPLICATEIDNO.ToString();

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        // Son.Vo - Kiểm tra có group theo phòng ban ở  ds nv nghỉ việc hay không - dùng cho BDF
        public bool IsGroupByOrgProfileQuit()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);

            if (config == null)
            {
                return false;
            }
            string valueConfig = AppConfig.HRM_HRE_CONFIG_GROUPBYORGPROFILEQUIT.ToString();

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        public Hre_ContractEntity SetNewCodeContract(Hre_ContractEntity contractEntity, string strIdConstract, string userLogin)
        {
            if (contractEntity != null)
            {
                var service = new BaseService();
                var status = string.Empty;
                var objContractType1 = service.GetFirstData<Cat_ContractTypeEntity>(contractEntity.ContractTypeID, ConstantSql.hrm_cat_sp_get_ContractTypeById, userLogin, ref status);
                var objProfile1 = service.GetFirstData<Hre_ProfileEntity>(contractEntity.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status);
                if (objContractType1 != null)
                {
                    var contractType = (Cat_ContractTypeEntity)objContractType1;
                    var profileEntity = (Hre_ProfileEntity)objProfile1;
                    if (!string.IsNullOrEmpty(contractType.Type))
                    {
                        var objSysConfig = service.GetFirstData<Sys_AllSettingEntity>(AppConfig.HRM_HRE_GENERATE_CODE_CONTRACT.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey, userLogin, ref status);
                        if (objSysConfig != null)
                        {
                            var sysConfig = (Sys_AllSettingEntity)objSysConfig;
                            if (sysConfig != null && !string.IsNullOrEmpty(sysConfig.Value1))
                            {
                                ElementFormula elementContactType = new ElementFormula("ContactType", contractType.Type, 0);
                                ElementFormula elementCodeEmp = new ElementFormula("CodeEmp", profileEntity.CodeEmp, 0);
                                ElementFormula elementOrdinal = new ElementFormula("Ordinal", 1, 0);

                                var result = FormulaHelper.ParseFormula(sysConfig.Value1, new List<ElementFormula>() { elementContactType, elementCodeEmp });
                                if (result != null && string.IsNullOrEmpty(result.ErrorMessage))
                                {
                                    var value = result.Value;
                                    if (value != null)
                                    {
                                        string newCode = value.ToString();
                                        if (newCode.EndsWith("Ordinal"))
                                        {
                                            var strNewCode = newCode.Substring(0, newCode.Length - 7);
                                            if (!string.IsNullOrEmpty(strIdConstract))
                                            {
                                                var listContractType = service.GetData<Cat_ContractTypeEntity>(Common.DotNetToOracle(strIdConstract), ConstantSql.hrm_cat_sp_get_ContractTypeByIds, userLogin, ref status);
                                                if (listContractType != null)
                                                {
                                                    var listContractTypeById = listContractType.Where(d => d.Type == contractType.Type).FirstOrDefault();
                                                    if (listContractTypeById != null)
                                                    {
                                                        var listId = strIdConstract.Split(',').ToList();
                                                        var count = listId.Select(d => d == listContractTypeById.ID.ToString()).Count();
                                                        newCode = strNewCode + "-" + (count + 1);
                                                    }
                                                }
                                                else
                                                {
                                                    newCode = strNewCode + "-" + 1;
                                                }
                                            }
                                            else
                                            {
                                                newCode = strNewCode + "-" + 1;
                                            }

                                        }
                                        contractEntity.ContractNo = newCode;
                                    }
                                }
                            }
                        }
                    }
                }
                contractEntity.ActionStatus = status;
            }
            return contractEntity;
        }

        #region Profile Info

        public Hre_ProfileEntity GetProfileInfo(Guid id, string userLogin)
        {
            string status = string.Empty;
            var entity = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
            return entity;
        }

        #endregion

        // Son.Vo - Kiểm tra có show thông báo hđ đến hạn theo kết quá đánh giá hđ hay không - Honda
        public bool IsShowExConByConResult()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);

            if (config == null)
            {
                return false;
            }
            string valueConfig = AppConfig.HRM_HRE_CONFIG_ISNOTUSEEXPIRYCONTRACTLOOP.ToString();

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        public List<Hre_HDTJobEntity> getHDTJobByPrice(List<Hre_HDTJobEntity> lstHDTJobInput, DateTime? DateFrom, DateTime? DateTo)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DateTime? dateMin = lstHDTJobInput.Select(m => m.DateFrom).OrderBy(m => m).FirstOrDefault();

                DateTime? dateMax = lstHDTJobInput.Select(m => m.DateTo).OrderByDescending(m => m).FirstOrDefault();

                List<Hre_HDTJobEntity> lstTmp = new List<Hre_HDTJobEntity>();
                //GetData lấy toàn bộ dữ liệu bảng Cat_HDTJobTypePrice
                var lsthdtjobtypeid = lstHDTJobInput.Select(s => s.HDTJobTypeID).ToList();
                var service = new BaseService();
                string status = string.Empty;
                var repoHDTJobTypePrice = new CustomBaseRepository<Cat_HDTJobTypePrice>(unitOfWork);
                var lstHDTJobTypePrice = repoHDTJobTypePrice.FindBy(s => s.HDTJobTypeID != null && s.IsDelete == null && lsthdtjobtypeid.Contains(s.HDTJobTypeID)).ToList();

                var repoCat_HDTJobType = new CustomBaseRepository<Cat_HDTJobType>(unitOfWork);
                var lstHDTJobType = repoCat_HDTJobType.FindBy(s => s.IsDelete == null && lsthdtjobtypeid.Contains(s.ID)).ToList();
                var lstHDTJobGroupIDs = lstHDTJobType.Select(m => m.HDTJobGroupID).Distinct().ToList();

                var repoCat_HDTJobGroup = new CustomBaseRepository<Cat_HDTJobGroup>(unitOfWork);
                var lstHDTJobGroup = repoCat_HDTJobGroup.FindBy(s => s.IsDelete == null && lstHDTJobGroupIDs.Contains(s.ID)).ToList();

                List<Cat_HDTJobTypePrice> lstHDTJobTypePriceInRange = lstHDTJobTypePrice.Where(m => m.DateOfEffect >= dateMin && m.DateOfEffect <= dateMax).ToList();

                foreach (var JobTypePriceInRange in lstHDTJobTypePriceInRange)
                {
                    if (JobTypePriceInRange.DateOfEffect == null)
                    {
                        continue;
                    }
                    Guid HDTJobTypeID = JobTypePriceInRange.HDTJobTypeID ?? Guid.Empty;
                    var lstHDTJobByType = lstHDTJobInput.Where(m => m.HDTJobTypeID == HDTJobTypeID
                        && m.DateFrom <= JobTypePriceInRange.DateOfEffect
                        && m.DateTo >= JobTypePriceInRange.DateOfEffect).ToList();
                    lstHDTJobInput.RemoveRange(lstHDTJobByType);
                    foreach (var HDTJob in lstHDTJobByType)
                    {
                        var lstHDTJobTypePriceOld = lstHDTJobTypePrice.Where(m => m.HDTJobTypeID == HDTJobTypeID && m.DateOfEffect < JobTypePriceInRange.DateOfEffect).OrderByDescending(m => m.DateOfEffect).FirstOrDefault();
                        Hre_HDTJobEntity HDTJobNew = HDTJob.Copy<Hre_HDTJobEntity>();
                        HDTJob.DateTo = JobTypePriceInRange.DateOfEffect.Value.AddDays(-1);
                        HDTJob.Price = lstHDTJobTypePriceOld != null ? lstHDTJobTypePriceOld.Price : null;
                        HDTJobNew.DateFrom = JobTypePriceInRange.DateOfEffect.Value;
                        HDTJobNew.Price = JobTypePriceInRange.Price;
                        if (HDTJob.DateFrom < DateFrom)
                        {
                            HDTJob.DateFrom = DateFrom;
                        }
                        if (HDTJob.DateTo > DateTo)
                        {
                            HDTJob.DateTo = DateTo;
                        }
                        if (HDTJobNew.DateFrom < DateFrom)
                        {
                            HDTJobNew.DateFrom = DateFrom;
                        }
                        if (HDTJobNew.DateTo > DateTo)
                        {
                            HDTJobNew.DateTo = DateTo;
                        }
                        lstTmp.Add(HDTJob);
                        lstTmp.Add(HDTJobNew);
                    }
                    lstHDTJobInput.AddRange(lstTmp);
                    lstTmp = new List<Hre_HDTJobEntity>();
                }

                var lstHDTJobInputNonPrice = lstHDTJobInput.Where(m => m.Price == null).OrderBy(m => m.ProfileID).ThenBy(m => m.DateFrom).ToList();


                string HDTJobGroupCodeOld = string.Empty;
                string HDTJobGroupNameOld = string.Empty;
                string HDTJobTypeCodeOld = string.Empty;
                string HDTJobTypeNameOld = string.Empty;
                double? PriceOld = null;
                Guid ProfileLastID = Guid.Empty;

                foreach (var HDTJob in lstHDTJobInputNonPrice)
                {
                    Guid HDTJobTypeID = HDTJob.HDTJobTypeID ?? Guid.Empty;
                    DateTime DateEnd = HDTJob.DateTo ?? DateTime.MaxValue;
                    Cat_HDTJobTypePrice HDTJobTypePrice = lstHDTJobTypePrice.Where(m => m.HDTJobTypeID == HDTJobTypeID && m.DateOfEffect <= DateEnd).OrderByDescending(m => m.DateOfEffect).FirstOrDefault();



                    if (HDTJobTypePrice == null)
                        continue;
                    HDTJob.Price = HDTJobTypePrice.Price;


                    if (ProfileLastID != HDTJob.ProfileID)
                    {
                        ProfileLastID = HDTJob.ProfileID ?? Guid.Empty;
                        HDTJobGroupCodeOld = string.Empty;
                        HDTJobGroupNameOld = string.Empty;
                        HDTJobTypeCodeOld = string.Empty;
                        HDTJobTypeNameOld = string.Empty;
                        PriceOld = null;
                    }
                    HDTJob.HDTJobGroupCodeOld = HDTJobGroupCodeOld;
                    HDTJob.HDTJobGroupNameOld = HDTJobGroupNameOld;
                    HDTJob.HDTJobTypeCodeOld = HDTJobTypeCodeOld;
                    HDTJob.HDTJobTypeNameOld = HDTJobTypeNameOld;
                    HDTJob.PriceOld = PriceOld;

                    var jobtype = lstHDTJobType.Where(m => m.ID == HDTJobTypeID).FirstOrDefault();
                    if (jobtype != null)
                    {
                        HDTJobTypeCodeOld = jobtype.Code;
                        HDTJobTypeNameOld = jobtype.HDTJobTypeName;
                        var JobtypeGroup = lstHDTJobGroup.Where(m => m.ID == jobtype.HDTJobGroupID).FirstOrDefault();
                        if (JobtypeGroup != null)
                        {
                            HDTJobGroupCodeOld = JobtypeGroup.Code;
                            HDTJobGroupNameOld = JobtypeGroup.HDTJobGroupName;
                        }
                        PriceOld = HDTJob.Price ?? null;
                    }

                }

                return lstHDTJobInput;
            }
        }

        public bool UpdateSettlementProfiles(List<Guid> ProfileIds, int Settlement, DateTime? DateSettlement)
        {
            try
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);

                    List<Hre_Profile> listProfileQuit = repoHre_Profile.FindBy(m => m.IsDelete != true && m.DateQuit != null).ToList();

                    //lọc ra profile được chọn
                    listProfileQuit = listProfileQuit.Where(m => ProfileIds.Any(t => t == m.ID)).ToList();

                    foreach (var i in listProfileQuit)
                    {
                        i.Settlement = Settlement;
                        i.MonnthSettlement = DateSettlement;
                    }
                    //repoHre_Profile.Edit(listProfileQuit);
                    unitOfWork.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        // Trở lại ds chờ
        public string BackToWaiting(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                string message = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                List<Guid> lstprofileIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstprofiles = repoProfile.FindBy(m => m.ID != null && lstprofileIds.Contains(m.ID)).ToList();
                foreach (var profile in lstprofiles)
                {
                    profile.StatusSyn = ProfileStatusSyn.E_WAITING.ToString();
                    profile.StatusHire = ProfileStatusSyn.E_WAITING.ToString();
                    profile.IsBlackList = null;
                }
                repoProfile.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }
        public List<Hre_ProfileEntity> GetProfileNameAll()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                //var lstProfiles = repoProfile.FindBy(m => m.IsDelete == null).Select(m => new Hre_ProfileEntity {ProfileName= m.ProfileName,CodeEmp= m.CodeEmp }).ToList();
                var lstProfiles = unitOfWork.CreateQueryable<Hre_Profile>(Guid.Empty).Select(p => new Hre_ProfileEntity { ProfileName = p.ProfileName, CodeEmp = p.CodeEmp }).ToList();

                return lstProfiles;
                //return lstProfiles;
            }
        }
        public List<Hre_ContractEntity> GetContractByProfileID(List<Guid> lstProfileIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var lstContract = unitOfWork.CreateQueryable<Hre_Contract>(Guid.Empty, s => lstProfileIDs.Contains(s.ProfileID)).Select(p => new Hre_ContractEntity
                {
                    ProfileID = p.ProfileID,
                    DateSigned = p.DateSigned
                }).ToList();
                return lstContract;
            }
        }

        // Son.Vo - Màn hình cập nhật hồ sơ ứng viên có nhập theo Rank detail hay nhập bằng tay.
        public bool IsInputGeneralCandidateManual()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);

            string valueConfig = AppConfig.HRM_HRE_CONFIG_INPUTGENERALCANDIDATEMANUAL.ToString();

            if (config == null)
            {
                return false;
            }

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }


        // Son.Vo - Màn hình tạo mới ứng viên có hiển thị field mã NV KH hay không.
        public bool IsUseCodeEmpOfCustomer()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);

            string valueConfig = AppConfig.HRM_HRE_CONFIG_ISUSECODEEMPOFCUSTOMER.ToString();

            if (config == null)
            {
                return false;
            }

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        // Son.Vo - Có cảnh báo vượt quá số lượng định biên - màn hình tạo mới NV.
        public bool IsAlertIfNumberOfEmpExceedPlan()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);
            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            string valueConfig = AppConfig.HRM_HRE_CONFIG_ISALERTIFNUMBEROFEMPEXCEEDPLAN.ToString();

            if (config == null)
            {
                return false;
            }

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        public string SetBlackListProfile(string selectedIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                BaseService service = new BaseService();
                string message = string.Empty;
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                List<Guid> ProfileIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstProfiles = repoProfile.FindBy(m => m.ID != null && ProfileIds.Contains(m.ID)).ToList();
                foreach (var profile in lstProfiles)
                {
                    profile.IsBlackList = true;
                }
                repoProfile.SaveChanges();
                message = NotificationType.Success.ToString();
                return message;
            }
        }

        public string StrValidateActionHire()
        {
            string returnValue = null;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);

            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            if (config == null)
            {
                return null;
            }
            string valueConfig = AppConfig.HRM_HRE_CONFIG_FIELDVALIDATEHIREPROFILE.ToString();

            var valueReturn = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (valueReturn != null && valueReturn.Value1 != null)
            {
                returnValue = valueReturn.Value1;
            }

            return returnValue;
        }

        public bool IsNotUseExpiryContractLoop()
        {
            bool returnValue = false;
            string status = string.Empty;
            List<object> lstOb = new List<object>();
            string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
            lstOb.Add(ConfigHre);
            lstOb.Add(null);
            lstOb.Add(null);
            var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, string.Empty, ref status);
            string valueConfig = AppConfig.HRM_HRE_CONFIG_ISNOTUSEEXPIRYCONTRACTLOOP.ToString();

            if (config == null)
            {
                return false;
            }

            var isCheck = config.Where(s => s.Name == valueConfig).FirstOrDefault();

            if (isCheck != null && isCheck.Value1 != null && isCheck.Value1 == bool.TrueString)
            {
                returnValue = true;
            }

            return returnValue;
        }

        public string SaveContractAndNextSalaryApprovedEvaluation(Hre_ContractEntity contract, string userLogin)
        {

            using (var context = new VnrHrmDataContext())
            {
                var contractServices = new Hre_ContractServices();
                string message = string.Empty;
                var baseServices = new BaseService();
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                if (contract.DateEndNextContract == null)
                {
                    return string.Empty;
                }

                #region Lấy WorkHistory
                var repoWorkhistory = new Hre_WorkHistoryRepository(unitOfWork);
                var lstWorkhistory = new List<Hre_WorkHistory>().Select(d => new
                {
                    d.ProfileID,
                    d.SalaryClassID
                }).ToList();

                lstWorkhistory.AddRange(repoWorkhistory.FindBy(s => s.IsDelete == null).Select(d => new
                        {
                            d.ProfileID,
                            d.SalaryClassID
                        }).ToList());
                #endregion

                // Lấy biến Dateend này gắn cho quá trình công tác khi cập nhật quá trinh công tác ở dưới
                DateTime? dateEnd = contract.DateEnd;

                var profile = baseServices.GetData<Hre_ProfileEntity>(contract.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();

                var lstObjSalaryRank = new List<object>();
                lstObjSalaryRank.Add(null);
                lstObjSalaryRank.Add(null);
                lstObjSalaryRank.Add(1);
                lstObjSalaryRank.Add(int.MaxValue - 1);
                var lstSalaryRank = baseServices.GetData<Cat_SalaryRankEntity>(lstObjSalaryRank, ConstantSql.hrm_cat_sp_get_SalaryRank, userLogin, ref status).ToList();

                #region Lấy Att_Grade
                var att_GradeRepository = new Att_GradeRepository(unitOfWork);
                var lstAttGrade = new List<Att_Grade>().Select(d => new
                {
                    d.ProfileID,
                    d.MonthEnd
                }).ToList();

                lstAttGrade.AddRange(att_GradeRepository.FindBy(s => s.IsDelete == null).Select(d => new
                {
                    d.ProfileID,
                    d.MonthEnd
                }).ToList());
                #endregion

                #region Lấy Sal_Grade
                var salGradeRepository = new Sal_GradeRepository(unitOfWork);
                var lstSalGrade = new List<Sal_Grade>().Select(d => new
                {
                    d.ProfileID,
                    d.MonthEnd
                }).ToList();

                lstSalGrade.AddRange(salGradeRepository.FindBy(s => s.IsDelete == null).Select(d => new
                {
                    d.ProfileID,
                    d.MonthEnd
                }).ToList());

                #endregion

                var lstObjGradePayroll = new List<object>();
                lstObjGradePayroll.Add(null);
                lstObjGradePayroll.Add(null);
                lstObjGradePayroll.Add(1);
                lstObjGradePayroll.Add(int.MaxValue - 1);
                var lstGradePayroll = baseServices.GetData<Cat_GradePayrollEntity>(lstObjGradePayroll, ConstantSql.hrm_cat_sp_get_GradePayroll, userLogin, ref status).ToList();

                var lstObjGradeAtt = new List<object>();
                lstObjGradeAtt.AddRange(new object[10]);
                lstObjGradeAtt[8] = 1;
                lstObjGradeAtt[9] = int.MaxValue - 1;
                var lstGradeAtt = baseServices.GetData<Cat_GradeAttendanceEntity>(lstObjGradeAtt, ConstantSql.hrm_cat_sp_get_Cat_GradeAttendance, userLogin, ref status).ToList();

                var lstObjCurrency = new List<object>();
                lstObjCurrency.Add(null);
                lstObjCurrency.Add(null);
                lstObjCurrency.Add(1);
                lstObjCurrency.Add(int.MaxValue - 1);
                var lstCurrency = baseServices.GetData<Cat_CurrencyEntity>(lstObjCurrency, ConstantSql.hrm_cat_sp_get_Currency, userLogin, ref status).ToList();
                var lstCurrencyNew = lstCurrency.Where(s => s.CurrencyName == "VND").FirstOrDefault();

                var lstObjContractType = new List<object>();
                lstObjContractType.Add(null);
                lstObjContractType.Add(null);
                lstObjContractType.Add(null);
                lstObjContractType.Add(null);
                lstObjContractType.Add(1);
                lstObjContractType.Add(int.MaxValue - 1);
                var lstContractType = baseServices.GetData<Cat_ContractTypeEntity>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, userLogin, ref status).ToList();

                var objInsuranceConfig = new List<object>();
                objInsuranceConfig.Add(1);
                objInsuranceConfig.Add(int.MaxValue - 1);
                var lstInsuranceConfig = baseServices.GetData<Cat_InsuranceConfigEntity>(objInsuranceConfig, ConstantSql.hrm_cat_sp_get_InsuranceConfig, userLogin, ref status).ToList();


                #region Lấy Sal_InsuranceSalary
                var objInsurance = new List<object>();
                objInsurance.AddRange(new object[9]);
                objInsurance[7] = 1;
                objInsurance[8] = int.MaxValue - 1;
                var lstInsurance = baseServices.GetData<Sal_InsuranceSalaryEntity>(objInsurance, ConstantSql.hrm_sal_sp_get_InsuranceSalary, userLogin, ref status).ToList();

                #endregion

                #region Lấy Hre_Contract
                var hreContractRepository = new Hre_ContractRepository(unitOfWork);
                var lstContractIdByProfileID = new List<Hre_Contract>().Select(d => new
                {
                    d.ProfileID,
                    d.ContractTypeID
                }).ToList();

                lstContractIdByProfileID.AddRange(hreContractRepository.FindBy(s => s.IsDelete == null && s.ProfileID == contract.ProfileID).Select(d => new
                {
                    d.ProfileID,
                    d.ContractTypeID
                }).ToList());

                #endregion

                var listIdContract = string.Empty;

                if (lstContractIdByProfileID != null)
                {
                    listIdContract = string.Join(",", lstContractIdByProfileID.Select(d => d.ContractTypeID));
                }
                var contractTypeEntity = new Cat_ContractTypeEntity();
                if (contract.NextContractTypeID != null)
                {
                    contractTypeEntity = lstContractType.Where(s => contract.NextContractTypeID.Value == s.ID).FirstOrDefault();
                }
                else
                {
                    message = ConstantMessages.WarningContractHaveNotNextContract.ToString().TranslateString();
                    return message;
                }

                if (contractTypeEntity != null)
                {
                    if (contractTypeEntity.Type == EnumDropDown.TypeContract.E_NODURATION.ToString())
                    {
                        return string.Empty;
                    }
                }

                var workingHistoryEntity = lstWorkhistory.Where(s => s.ProfileID == contract.ProfileID).FirstOrDefault();


                #region Lấy Sal_Grade by profileid
                var objSalGrade = new List<object>();
                objSalGrade.Add(contract.ProfileID);
                objSalGrade.Add(null);
                objSalGrade.Add(1);
                objSalGrade.Add(int.MaxValue - 1);
                var salGradeByProfileIDEntity = baseServices.GetData<Sal_GradeEntity>(objSalGrade, ConstantSql.hrm_sal_sp_get_GradeAndAllownaceByProId, userLogin, ref status).FirstOrDefault();
                #endregion

                #region Lấy Att_Grade by profileid
                var objAttGrade = new List<object>();
                objAttGrade.Add(contract.ProfileID);
                objAttGrade.Add(null);
                objAttGrade.Add(1);
                objAttGrade.Add(int.MaxValue - 1);
                var attGradeByProfileIDEntity = baseServices.GetData<Att_GradeEntity>(objAttGrade, ConstantSql.hrm_att_sp_get_GradeAttendanceByProIdCutID, userLogin, ref status).FirstOrDefault();
                #endregion

                if (contractTypeEntity == null)
                {
                    return string.Empty;
                }

                // Theo task 0049932 - sinh ra lương với ngày hiệu lực = ngày bắt đầu hd.
                var dateEffect = DateTime.Now;

                var contracttypeByContract = lstContractType.Where(s => s.ID == contract.ContractTypeID).FirstOrDefault();
                if (contract.ContractResult == EnumDropDown.ResultContract.PASS.ToString())
                {
                    if (contract.ContractEvaType == EnumDropDown.ContractEvaType.E_EXPIRED_APPRENTICE.ToString())
                    {

                        //chưa tìm dc cách xử lý nên hard code 
                        var lstSalaryRankNew = new Cat_SalaryRankEntity();
                        if (contract.RankDetailForNextContract != null)
                        {
                            lstSalaryRankNew = lstSalaryRank.Where(s => contract.RankDetailForNextContract != null && s.ID == contract.RankDetailForNextContract.Value).FirstOrDefault();
                        }
                        else
                        {
                            lstSalaryRankNew = lstSalaryRank.Where(s => contract.RankRateID != null && s.ID == contract.RankRateID.Value).FirstOrDefault();
                        }

                        #region Xử lý Hre_Contract
                        if (contract.TypeOfPass == EnumDropDown.TypeOfPass.E_SIGNED_NEXTCONTRACT.ToString())
                        {
                            int month = 0;
                            if (contractTypeEntity != null && contractTypeEntity.ValueTime != null)
                            {
                                month = (int)contractTypeEntity.ValueTime.Value;
                                if (contractTypeEntity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                                {
                                    month = month * 12;
                                }
                                contractTypeEntity.DateStart = contract.DateEnd.Value.AddDays(1);

                                //chưa tìm dc cách xử lý nên hard code 
                                //  var nextContractTypeID = Common.ConvertToGuid(contractTypeEntity.ContractNextID).ToString();
                                var contractEntity = new Hre_ContractEntity
                                {
                                    ProfileID = contract.ProfileID,
                                    ProfileName = contract.ProfileName,
                                    DateStart = contract.DateEnd.Value.AddDays(1),
                                    DateSigned = contract.DateEnd.Value.AddDays(1),
                                    JobTitleID = contract.JobTitleID,
                                    PositionID = contract.PositionID,
                                    DateEnd = contractTypeEntity.DateStart.Value.AddMonths(month),
                                    Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                    RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                                    ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                    ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                    SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                    ContractTypeID = contractTypeEntity.ID,
                                    DateEndNextContract = contract.DateEndNextContract,
                                };
                                if (contract.DateEndNextContract != null)
                                {
                                    contractEntity.DateEnd = contract.DateEndNextContract.Value;
                                }

                                contractEntity = SetNewCodeContract(contractEntity, listIdContract, userLogin);

                                #region Nếu là loại hđ xác định thời hạn thì update lại cột TimesContract theo task 0049731
                                if (contracttypeByContract != null && contracttypeByContract.Type == HRM.Infrastructure.Utilities.EnumDropDown.TypeContract.E_DURATION.ToString())
                                {
                                    try
                                    {
                                        string times = contractEntity.ContractNo.Substring(contractEntity.ContractNo.Length - 1, 1);
                                        int Year = int.Parse(times);
                                        contractEntity.TimesContract = Year;
                                    }
                                    catch
                                    {
                                    }
                                }
                                #endregion
                                contractEntity.Status = "E_APPROVED";
                                contractEntity.StatusEvaluation = "E_APPROVED";
                                contractEntity.DateExtend = contractEntity.DateEnd;
                                if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                                {
                                    return string.Empty;
                                }
                                message = contractServices.Add(contractEntity);
                                dateEffect = contractEntity.DateStart;
                            }
                            else
                            {
                                contractTypeEntity.DateStart = contract.DateEnd.Value.AddDays(1);

                                //chưa tìm dc cách xử lý nên hard code 
                                var contractEntity = new Hre_ContractEntity
                                {
                                    ProfileID = contract.ProfileID,
                                    ProfileName = contract.ProfileName,
                                    DateStart = contract.DateEnd.Value.AddDays(1),
                                    DateSigned = contract.DateEnd.Value.AddDays(1),
                                    JobTitleID = contract.JobTitleID,
                                    PositionID = contract.PositionID,
                                    Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                    RankRateID = lstSalaryRankNew == null ? contract.RankRateID : lstSalaryRankNew.ID,
                                    ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                    ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                    SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                    ContractTypeID = contractTypeEntity.ID
                                };

                                if (contract.DateEndNextContract != null)
                                {
                                    contractEntity.DateEnd = contract.DateEndNextContract.Value;
                                }

                                contractEntity.DateExtend = contract.DateEnd;
                                contractEntity.StatusEvaluation = "E_APPROVED";

                                if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                                {
                                    return string.Empty;
                                }

                                message = contractServices.Add(contractEntity);
                                dateEffect = contractEntity.DateStart;
                            }
                        }

                        //Edit lai StatusEvaluation 
                        contract.StatusEvaluation = "E_APPROVED";
                        message = contractServices.Edit(contract);

                        #endregion

                        #region Xử Lý Sal_BasicSalary
                        var salaryEntity = new Sal_BasicSalaryEntity
                        {
                            ProfileID = contract.ProfileID,
                            ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                            RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                            GrossAmount = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryStandard.ToString(),
                            DateOfEffect = dateEffect,
                            CurrencyID = lstCurrencyNew.ID,
                            Note = contract.Remark,
                            Status = "E_APPROVED"
                        };
                        message = baseServices.Add(salaryEntity);

                        #endregion

                        #region Xử Lý Hre_Profile
                        var profileEntity = profile.CopyData<Hre_ProfileEntity>();
                        Guid? _AbilityTileID = null;
                        profileEntity.SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID;
                        if (profileEntity.SalaryClassID != null)
                        {
                            var abilityTitleBySalaryClass = baseServices.GetData<Cat_AbilityTileEntity>(Common.DotNetToOracle(profileEntity.SalaryClassID.ToString()), ConstantSql.hrm_cat_sp_get_AbilityTileBySalaryClassId, userLogin, ref status).FirstOrDefault();
                            if (abilityTitleBySalaryClass != null)
                            {
                                _AbilityTileID = abilityTitleBySalaryClass.ID;
                                profileEntity.AbilityTileID = _AbilityTileID;
                            }
                        }

                        baseServices.Edit(profileEntity);

                        if (workingHistoryEntity != null)
                        {
                            if (workingHistoryEntity.SalaryClassID != lstSalaryRankNew.SalaryClassID)
                            {
                                var workhistoryEntity = new Hre_WorkHistoryEntity
                                {
                                    ProfileID = contract.ProfileID,
                                    SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                    DateEffective = dateEnd != null ? dateEnd.Value.AddDays(1) : DateTime.Now,
                                    AbilityTileID = _AbilityTileID,
                                    Status = "E_APPROVED"

                                };
                                message = baseServices.Add(workhistoryEntity);
                            }
                        }
                        else
                        {
                            var workhistoryEntity = new Hre_WorkHistoryEntity
                            {
                                ProfileID = contract.ProfileID,
                                SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                DateEffective = dateEnd != null ? dateEnd.Value.AddDays(1) : DateTime.Now,
                                Status = "E_APPROVED",

                            };
                            message = baseServices.Add(workhistoryEntity);
                        }

                        #endregion

                        #region Sal_Grade
                        var lstGradeByProfileID = lstSalGrade.Where(s => contract.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                        var lstGradePayrollByProfileID = lstGradePayroll.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                        if (salGradeByProfileIDEntity != null)
                        {
                            if (salGradeByProfileIDEntity.GradePayrollID != lstGradePayrollByProfileID.ID)
                            {
                                var gradeEntity = new Sal_GradeEntity
                                {
                                    ProfileID = contract.ProfileID,
                                    GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                    MonthStart = contract.DateSigned != null ? contract.DateSigned.Value : DateTime.Now,
                                };
                                message = baseServices.Add(gradeEntity);
                            }
                        }
                        else
                        {
                            var gradeEntity = new Sal_GradeEntity
                            {
                                //   ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                                ProfileID = contract.ProfileID,
                                GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                MonthStart = contract.DateSigned != null ? contract.DateSigned.Value : DateTime.Now,
                            };
                            message = baseServices.Add(gradeEntity);
                        }
                        #endregion

                        #region Att_Grade
                        var lstAttGradeByProfileID = lstAttGrade.Where(s => contract.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                        var lstGradeAttByProfileID = lstGradeAtt.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                        if (attGradeByProfileIDEntity != null)
                        {
                            if (attGradeByProfileIDEntity.GradeAttendanceID != lstGradeAttByProfileID.ID)
                            {
                                var gradeAttEntity = new Att_GradeEntity
                                {
                                    ProfileID = contract.ProfileID,
                                    GradeAttendanceID = lstAttGradeByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                    MonthStart = contract.DateSigned != null ? contract.DateSigned.Value : DateTime.Now,
                                };
                                message = baseServices.Add(gradeAttEntity);
                            }
                        }
                        else
                        {
                            var gradeAttEntity = new Att_GradeEntity
                            {
                                // ID = lstGradeAttByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                                ProfileID = contract.ProfileID,
                                GradeAttendanceID = lstGradeAttByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                MonthStart = contract.DateSigned != null ? contract.DateSigned.Value : DateTime.Now,
                            };
                            message = baseServices.Add(gradeAttEntity);
                        }
                        #endregion

                        #region Xử Lý Lương BHXH
                        if (contractTypeEntity.NoneTypeInsuarance == true)
                        {
                            var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == contract.ProfileID && s.DateEffect == contract.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                            var insuranceEntity = new Sal_InsuranceSalaryEntity
                            {
                                ProfileID = contract.ProfileID,
                                InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                DateEffect = dateEffect,
                                IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance,
                                IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance,
                                IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance,
                                CurrencyID = lstCurrencyNew.ID
                            };
                            if (insuranceEntityByProfileID != null)
                            {
                                insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                insuranceEntityByProfileID.IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance;
                                insuranceEntityByProfileID.IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance;
                                insuranceEntityByProfileID.IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance;
                                message = baseServices.Edit(insuranceEntityByProfileID);
                            }
                            else
                            {
                                message = baseServices.Add(insuranceEntity);
                            }
                        }
                        if (contractTypeEntity.NoneTypeInsuarance == false)
                        {

                            var insuranceConfigEntity = lstInsuranceConfig.Where(s => s.ContractTypeID != null && s.ContractTypeID.Value == contractTypeEntity.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                            if (insuranceConfigEntity != null)
                            {
                                var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == contract.ProfileID && s.DateEffect == contract.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                                var insuranceEntity = new Sal_InsuranceSalaryEntity
                                {
                                    ProfileID = contract.ProfileID,
                                    InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                    DateEffect = dateEffect,
                                    IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial,
                                    IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy,
                                    IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth,
                                    CurrencyID = lstCurrencyNew.ID
                                };

                                if (insuranceEntityByProfileID != null)
                                {
                                    insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                    insuranceEntityByProfileID.IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial;
                                    insuranceEntityByProfileID.IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy;
                                    insuranceEntityByProfileID.IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth;
                                    message = baseServices.Edit(insuranceEntityByProfileID);
                                }
                                else
                                {
                                    message = baseServices.Add(insuranceEntity);
                                }

                            }
                        }
                        #endregion
                    }

                    if (contract.ContractEvaType == EnumDropDown.ContractEvaType.E_ANNUAL_EVALUATION.ToString() && contract.ContractResult == EnumDropDown.ResultContract.PASS.ToString())
                    {
                        var lstSalaryRankNew = new Cat_SalaryRankEntity();
                        if (contract.RankDetailForNextContract != null)
                        {
                            lstSalaryRankNew = lstSalaryRank.Where(s => contract.RankDetailForNextContract != null && s.ID == contract.RankDetailForNextContract.Value).FirstOrDefault();
                        }
                        else
                        {
                            lstSalaryRankNew = lstSalaryRank.Where(s => contract.RankRateID != null && s.ID == contract.RankRateID.Value).FirstOrDefault();
                        }

                        #region Xử lý Hre_Contract
                        int month = 0;
                        if (contractTypeEntity != null && contractTypeEntity.ValueTime != null)
                        {
                            month = (int)contractTypeEntity.ValueTime.Value;
                            if (contractTypeEntity.UnitTime == HRM.Infrastructure.Utilities.EnumDropDown.UnitType.E_YEAR.ToString())
                            {
                                month = month * 12;
                            }
                            contractTypeEntity.DateStart = contract.DateEnd.Value.AddDays(1);

                            //chưa tìm dc cách xử lý nên hard code 

                            var contractEntity = new Hre_ContractEntity
                            {
                                ProfileID = contract.ProfileID,
                                ProfileName = contract.ProfileName,
                                DateStart = new DateTime(DateTime.Now.Year, 6, 1),
                                DateSigned = new DateTime(DateTime.Now.Year, 6, 1),
                                JobTitleID = contract.JobTitleID,
                                PositionID = contract.PositionID,
                                DateEnd = contractTypeEntity.DateStart.Value.AddMonths(month),
                                Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                                ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                ContractTypeID = contractTypeEntity.ID
                            };

                            if (contract.DateEndNextContract != null)
                            {
                                contractEntity.DateEnd = contract.DateEndNextContract.Value;
                            }
                            contractEntity = SetNewCodeContract(contractEntity, listIdContract, userLogin);

                            #region Nếu là loại hđ xác định thời hạn thì update lại cột TimesContract theo task 0049731
                            if (contracttypeByContract != null && contracttypeByContract.Type == HRM.Infrastructure.Utilities.EnumDropDown.TypeContract.E_DURATION.ToString())
                            {
                                try
                                {
                                    string times = contractEntity.ContractNo.Substring(contractEntity.ContractNo.Length - 1, 1);
                                    int Year = int.Parse(times);
                                    contractEntity.TimesContract = Year;
                                }
                                catch
                                {
                                }
                            }
                            #endregion

                            contractEntity.Status = "E_APPROVED";
                            contractEntity.DateExtend = contractEntity.DateEnd;
                            contractEntity.StatusEvaluation = "E_APPROVED";

                            if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                            {
                                return string.Empty;
                            }
                            message = contractServices.Add(contractEntity);
                            dateEffect = contractEntity.DateStart;
                        }
                        else
                        {
                            contractTypeEntity.DateStart = contract.DateEnd.Value.AddDays(1);

                            //chưa tìm dc cách xử lý nên hard code 

                            var contractEntity = new Hre_ContractEntity
                            {
                                // ContractNo = getContractNo(item, item.DateSigned),
                                ProfileID = contract.ProfileID,
                                ProfileName = contract.ProfileName,
                                DateStart = new DateTime(DateTime.Now.Year, 6, 1),
                                DateSigned = new DateTime(DateTime.Now.Year, 6, 1),
                                JobTitleID = contract.JobTitleID,
                                PositionID = contract.PositionID,
                                // DateEnd = null,
                                Salary = lstSalaryRankNew == null ? 0 : lstSalaryRankNew.SalaryStandard,
                                RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                                ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                ClassRateName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryClassName,
                                SalaryRankName = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryRankName,
                                ContractTypeID = contractTypeEntity.ID
                            };

                            if (contract.DateEndNextContract != null)
                            {
                                contractEntity.DateEnd = contract.DateEndNextContract.Value;
                            }
                            contractEntity.DateExtend = contract.DateEnd;
                            contractEntity.StatusEvaluation = "E_APPROVED";
                            if (!string.IsNullOrEmpty(contractEntity.ErrorMessage))
                            {

                                return string.Empty;
                            }
                            message = contractServices.Add(contractEntity);
                            dateEffect = contractEntity.DateStart;
                        }
                        //Edit lai StatusEvaluation 
                        contract.StatusEvaluation = "E_APPROVED";
                        message = contractServices.Edit(contract);

                        #endregion

                        #region Xử Lý Sal_BasicSalary
                        var salaryEntity = new Sal_BasicSalaryEntity
                        {
                            ProfileID = contract.ProfileID,
                            ClassRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                            RankRateID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.ID,
                            GrossAmount = lstSalaryRankNew == null ? string.Empty : lstSalaryRankNew.SalaryStandard.ToString(),
                            DateOfEffect = new DateTime(DateTime.Now.Year, 6, 1),
                            CurrencyID = lstCurrencyNew.ID,
                            Note = contract.Remark,
                            Status = "E_APPROVED"

                        };
                        message = baseServices.Add(salaryEntity);

                        #endregion

                        #region Xử Lý Hre_Profile
                        var profileEntity = profile.CopyData<Hre_ProfileEntity>();
                        profileEntity.SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID;
                        Guid? _AbilityTileID = null;
                        if (profileEntity.SalaryClassID != null)
                        {
                            var abilityTitleBySalaryClass = baseServices.GetData<Cat_AbilityTileEntity>(Common.DotNetToOracle(profileEntity.SalaryClassID.ToString()), ConstantSql.hrm_cat_sp_get_AbilityTileBySalaryClassId, userLogin, ref status).FirstOrDefault();
                            if (abilityTitleBySalaryClass != null)
                            {
                                _AbilityTileID = abilityTitleBySalaryClass.ID;
                                profileEntity.AbilityTileID = _AbilityTileID;
                            }
                        }
                        message = baseServices.Edit(profileEntity);
                        if (workingHistoryEntity != null)
                        {
                            if (workingHistoryEntity.SalaryClassID != lstSalaryRankNew.SalaryClassID)
                            {
                                var workhistoryEntity = new Hre_WorkHistoryEntity
                                {
                                    ProfileID = contract.ProfileID,
                                    SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                    DateEffective = dateEnd != null ? dateEnd.Value.AddDays(1) : DateTime.Now,
                                    AbilityTileID = _AbilityTileID,
                                    Status = "E_APPROVED"
                                };
                                message = baseServices.Add(workhistoryEntity);
                            }
                        }
                        else
                        {
                            var workhistoryEntity = new Hre_WorkHistoryEntity
                            {
                                ProfileID = contract.ProfileID,
                                SalaryClassID = lstSalaryRankNew == null ? Guid.Empty : lstSalaryRankNew.SalaryClassID,
                                DateEffective = dateEnd != null ? dateEnd.Value.AddDays(1) : DateTime.Now,
                                Status = "E_APPROVED"

                            };
                            message = baseServices.Add(workhistoryEntity);
                        }
                        #endregion

                        #region Sal_Grade
                        var lstGradeByProfileID = lstSalGrade.Where(s => contract.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                        var lstGradePayrollByProfileID = lstGradePayroll.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                        if (salGradeByProfileIDEntity != null)
                        {
                            if (salGradeByProfileIDEntity.GradePayrollID != lstGradePayrollByProfileID.ID)
                            {
                                var gradeEntity = new Sal_GradeEntity
                                {
                                    //   ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                                    ProfileID = contract.ProfileID,
                                    GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                    MonthStart = new DateTime(DateTime.Now.Year, 6, 1),

                                };
                                message = baseServices.Add(gradeEntity);
                            }
                        }
                        else
                        {
                            var gradeEntity = new Sal_GradeEntity
                            {
                                //  ID = lstGradeByProfileID == null ? Guid.Empty : lstGradeByProfileID.ID,
                                ProfileID = contract.ProfileID,
                                GradePayrollID = lstGradePayrollByProfileID == null ? Guid.Empty : lstGradePayrollByProfileID.ID,
                                MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                            };
                            message = baseServices.Add(gradeEntity);
                        }


                        #endregion

                        #region Att_Grade
                        var lstAttGradeByProfileID = lstAttGrade.Where(s => contract.ProfileID == s.ProfileID).ToList().OrderByDescending(s => s.MonthEnd <= DateTime.Now).FirstOrDefault();
                        var lstGradeAttByProfileID = lstGradeAtt.Where(s => s.Code == lstSalaryRankNew.Code).FirstOrDefault();
                        if (attGradeByProfileIDEntity != null)
                        {
                            if (attGradeByProfileIDEntity.GradeAttendanceID != lstGradeAttByProfileID.ID)
                            {
                                var gradeAttEntity = new Att_GradeEntity
                                {
                                    //ID = lstAttGradeByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                                    ProfileID = contract.ProfileID,
                                    GradeAttendanceID = lstGradeAttByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                    MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                                };
                                message = baseServices.Add(gradeAttEntity);
                            }
                        }
                        else
                        {
                            var gradeAttEntity = new Att_GradeEntity
                            {
                                //ID = lstAttGradeByProfileID == null ? Guid.Empty: lstAttGradeByProfileID.ID,
                                ProfileID = contract.ProfileID,
                                GradeAttendanceID = lstGradeAttByProfileID == null ? Guid.Empty : lstGradeAttByProfileID.ID,
                                MonthStart = new DateTime(DateTime.Now.Year, 6, 1)
                            };
                            message = baseServices.Add(gradeAttEntity);
                        }


                        #endregion

                        #region Xử Lý Lương BHXH
                        if (contractTypeEntity.NoneTypeInsuarance == true)
                        {
                            var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == contract.ProfileID && s.DateEffect == contract.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                            var insuranceEntity = new Sal_InsuranceSalaryEntity
                            {
                                ProfileID = contract.ProfileID,
                                InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                DateEffect = dateEffect,
                                IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance,
                                IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance,
                                IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance,
                                CurrencyID = lstCurrencyNew.ID,

                            };

                            if (insuranceEntityByProfileID != null)
                            {
                                insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                insuranceEntityByProfileID.IsSocialIns = contractTypeEntity.IsSocialInsurance == null ? null : contractTypeEntity.IsSocialInsurance;
                                insuranceEntityByProfileID.IsUnimploymentIns = contractTypeEntity.IsUnEmployInsurance == null ? null : contractTypeEntity.IsUnEmployInsurance;
                                insuranceEntityByProfileID.IsMedicalIns = contractTypeEntity.IsHealthInsurance == null ? null : contractTypeEntity.IsHealthInsurance;
                                message = baseServices.Edit(insuranceEntityByProfileID);
                            }
                            else
                            {
                                message = baseServices.Add(insuranceEntity);
                            }
                        }
                        if (contractTypeEntity.NoneTypeInsuarance == false)
                        {
                            var insuranceConfigEntity = lstInsuranceConfig.Where(s => s.ContractTypeID != null && s.ContractTypeID.Value == contractTypeEntity.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                            if (insuranceConfigEntity != null)
                            {
                                var insuranceEntityByProfileID = lstInsurance.Where(s => s.ProfileID == contract.ProfileID && s.DateEffect == contract.DateEnd.Value.AddDays(1)).OrderByDescending(s => s.DateUpdate).FirstOrDefault();
                                var insuranceEntity = new Sal_InsuranceSalaryEntity
                                {
                                    ProfileID = contract.ProfileID,
                                    InsuranceAmount = lstSalaryRankNew.SalaryStandard,
                                    DateEffect = dateEffect,
                                    IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial,
                                    IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy,
                                    IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth,
                                    CurrencyID = lstCurrencyNew.ID
                                };
                                if (insuranceEntityByProfileID != null)
                                {
                                    insuranceEntityByProfileID.InsuranceAmount = lstSalaryRankNew.SalaryStandard;
                                    insuranceEntityByProfileID.IsSocialIns = insuranceConfigEntity.IsSocial == null ? null : insuranceConfigEntity.IsSocial;
                                    insuranceEntityByProfileID.IsUnimploymentIns = insuranceConfigEntity.IsUnEmploy == null ? null : insuranceConfigEntity.IsUnEmploy;
                                    insuranceEntityByProfileID.IsMedicalIns = insuranceConfigEntity.IsHealth == null ? null : insuranceConfigEntity.IsHealth;
                                    message = baseServices.Edit(insuranceEntityByProfileID);
                                }
                                else
                                {
                                    message = baseServices.Add(insuranceEntity);
                                }
                            }
                        }
                        #endregion
                    }
                }
                return message;
            }
        }
    }
}
