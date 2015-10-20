using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Business.Category.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_DataPermissionServices : BaseService
    {
        public Sys_DataPermissionEntity  CheckDuplicateDataPermission(Guid userId, Guid groupId)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_DataPermissionRepository(unitOfWork);
                return repo.CheckDuplicateDataPermission(userId, groupId);
            }
        }

        public List<Sys_DataPermissionEntity> GetByUserId(Guid id, string userLogin)
        {
            var getOrgName = new List<Cat_OrgStructureEntity>();
            var listEntity = new List<Sys_DataPermissionEntity>();
            string status = string.Empty;

            List<object> objOrgstructureParams = new List<object>();
            if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
            {
                objOrgstructureParams.Add(null);
                objOrgstructureParams.Add(null);
                objOrgstructureParams.Add(null);
                objOrgstructureParams.Add(null);
                objOrgstructureParams.Add(null);
            }
            else
            {
                objOrgstructureParams.Add(null);
                objOrgstructureParams.Add(null);
                objOrgstructureParams.Add(null);
                objOrgstructureParams.Add(1);
                objOrgstructureParams.Add(100);
                objOrgstructureParams.Add(null);
            }

            getOrgName = GetData<Cat_OrgStructureEntity>(objOrgstructureParams, ConstantSql.hrm_cat_sp_get_OrgStructure, userLogin, ref status);

            listEntity = GetData<Sys_DataPermissionEntity>(id, ConstantSql.hrm_sys_sp_get_DataPermissionByUserId, userLogin, ref status);

            foreach (var item in listEntity)
            {
                var orgName = item.Branches.ToNumbers()
                    .Select(orgId => getOrgName.Where(d => d.OrderNumber == orgId).Select(d => d.OrgStructureName ?? string.Empty).FirstOrDefault())
                    .Where(p=>p != null)
                    .ToList();
                item.BranchesName = orgName.ToSplitString(',');
            }
            return listEntity;

        }

        public List<Guid> GetUserByGroupId(Guid groupId)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_DataPermissionRepository(unitOfWork);
                return repo.FindBy(p => p.IsDelete == null && p.GroupID == groupId).Select(p => p.UserID).ToList();
            }
        }

        public List<Guid> GetUsers()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork) (new UnitOfWork(context));
                var repo = new Sys_DataPermissionRepository(unitOfWork);
                return repo.FindBy(p => p.IsDelete == null).Select(p => p.UserID).ToList();
            }
        }
    }
}
