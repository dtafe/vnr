using System;
using System.Linq;
using HRM.Business.HrmSystem.Models;
using HRM.Data.BaseRepository;
using VnResource.Helper.Data;

namespace HRM.Data.Entity.Repositories
{
    //sys_DataPermission
    public class Sys_DataPermissionRepository : CustomBaseRepository<Sys_DataPermission>
    {
        public Sys_DataPermissionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        //public IQueryable<Sys_DataPermission> Get()
        //{
        //    IQueryable<Sys_DataPermission> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        public Sys_DataPermissionEntity CheckDuplicateDataPermission(Guid userId, Guid groupId)
        {
            var result = this.DbSet.Where(p=>p.UserID == userId && p.GroupID == groupId && p.IsDelete == null).FirstOrDefault();
            if (result == null)
            {
                return new Sys_DataPermissionEntity
                {
                    GroupID = groupId,
                    UserID = userId,
                    ID = Guid.Empty
                };
            }
            return result.CopyData<Sys_DataPermissionEntity>();
        }

        ///// <summary>
        ///// Lấy tất cả danh sách overtime bởi store hrm_system_sp_get_DataPermission
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Sys_DataPermissionEntity> GetPermissions()
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_DataPermissionEntity>(ConstantSql.hrm_system_sp_get_DataPermission,
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Sys_DataPermissionEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_DataPermissionEntity>(ConstantSql.hrm_system_sp_get_DataPermission)
        //            .ToList<Sys_DataPermissionEntity>().AsQueryable();
        //    }
        //}

        //public List<Sys_DataPermissionEntity> GetDataPermissionByUserId(int userId)
        //{
        //    var getOrgName = new List<Cat_OrgStructureEntity>();
        //    var listEntity = new List<Sys_DataPermissionEntity>();
        //    string status = string.Empty;    
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //         string query = "begin CAT_SP_GET_ORGSTRUCTUREALL(:R_Output); end;";
        //         getOrgName = this.UnitOfWork.Context.Database.SqlQuery<Cat_OrgStructureEntity>(query,
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Cat_OrgStructureEntity>();

        //        listEntity = this.UnitOfWork.Context.Database.SqlQuery<Sys_DataPermissionEntity>(ConstantSql.hrm_sys_sp_get_DataPermissionByUserId,
        //            new OracleParameter()
        //            {
        //                ParameterName = "userId",
        //                Value = userId,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Sys_DataPermissionEntity>();
        //    }
        //    else
        //    {
        //         getOrgName = this.UnitOfWork.Context.Database.SqlQuery<Cat_OrgStructureEntity>(ConstantSql.hrm_cat_sp_get_OrgStructure).ToList<Cat_OrgStructureEntity>();
        //         listEntity = this.UnitOfWork.Context.Database.SqlQuery<Sys_DataPermissionEntity>(ConstantSql.hrm_sys_sp_get_DataPermissionByUserId, new SqlParameter
        //        {
        //            ParameterName = "userId",
        //            Value = userId
        //        }).ToList<Sys_DataPermissionEntity>();
        //    }

        //    foreach (var item in listEntity)
        //    {
        //        var orgName = item.Branches.ToNumbers().Select(orgId => getOrgName.Where(d => d.Id == orgId).Select(d => d.OrgStructureName).FirstOrDefault()).ToList();
        //        item.BranchesName = orgName.ToSplitString(',');
        //    }
        //    return listEntity;
        //}

    }
}
