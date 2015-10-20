using System;
using System.Collections.Generic;
using HRM.Business.HrmSystem.Models;
using HRM.Data.BaseRepository;
using System.Linq;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Data.Entity.Repositories
{
    public class Sys_GroupPermissionRepository : CustomBaseRepository<Sys_GroupPermission2>
    {
        public Sys_GroupPermissionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        //public IQueryable<Sys_GroupPermission2> Get()
        //{
        //    IQueryable<Sys_GroupPermission2> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        public Sys_GroupPermission2 Get(Guid groupId, Guid resourceId)
        {
            var data = this.DbSet.FirstOrDefault(g => g.GroupID == groupId && g.ResourceID == resourceId);
            return data;
        }

        ///// <summary>
        ///// Lấy tất cả dữ liệu Sys_GroupPermission2 [Tung.Ly - 2014/05/13]
        ///// </summary>
        //public List<Sys_GroupPermission2Entity> GetGroupPermission()
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupPermission2Entity>(ConstantSql.hrm_sys_sp_get_GroupPermission,
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Sys_GroupPermission2Entity>();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupPermission2Entity>(ConstantSql.hrm_sys_sp_get_GroupPermission)
        //      .ToList<Sys_GroupPermission2Entity>();
        //    }
        //}

        /// <summary>
        /// Lấy đối tượng Sys_GroupPermission2 bởi Id [Tung.Ly - 2014/05/13]
        /// </summary>
        //public Sys_GroupPermission2Entity GetGroupPermissionById(int groupPermissionId)
        //{
        //    return this.UnitOfWork.Context.Database
        //        .SqlQuery<Sys_GroupPermission2Entity>(ConstantSql.hrm_sys_sp_get_GroupPermissionById, new SqlParameter
        //    {
        //        ParameterName = "grouppermissionId",
        //        Value = groupPermissionId
        //    }).ToList<Sys_GroupPermission2Entity>().FirstOrDefault<Sys_GroupPermission2Entity>();
        //} 
        
        //public List<Sys_GroupPermission2Entity> GetGroupPermissionByGroupId(int groupId)
        //{
        //    var data = this.UnitOfWork.Context.Database 
        //        .SqlQuery<Sys_GroupPermission2Entity>(ConstantSql.hrm_sys_sp_get_GroupPermissionByGroupId, new SqlParameter
        //    {
        //        ParameterName = "groupId",
        //        Value = groupId
        //    }).ToList<Sys_GroupPermission2Entity>();
        //    return data;
        //}

        public List<Sys_GroupPermission2Entity> GetAllGroupPermissionByGroupId(Guid groupId)
        {
            var result = this.DbSet.Where(p => p.GroupID == groupId).ToList().Translate<Sys_GroupPermission2Entity>();
            return result;
        }

        ///// <summary>
        ///// Lấy tất cả danh sách dữ liệu 
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Sys_TablesMultiEntity> GetSysTables(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_cat_sp_get_tables);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Sys_TablesMultiEntity>(param.SqlQuery, param.Params).ToList<Sys_TablesMultiEntity>().AsQueryable();
        //}


        public List<Sys_GroupPermission2> GetGroupPermissionByGroupAndResoure(Guid groupId, Guid resourceId)
        { 
            var status = string.Empty;
            List<object> obj = new List<object>();
            obj.Add(groupId);
            obj.Add(resourceId);
            return ExecuteQuery(obj, ConstantSql.hrm_sys_sp_get_GroupPermissionByGroupAndResource,true, string.Empty, ref status).ToList();

            //return this.UnitOfWork.Context.Database
            //    .SqlQuery<Sys_GroupPermission2>(ConstantSql.hrm_sys_sp_get_GroupPermissionByGroupAndResource, new SqlParameter
            //    {
            //        ParameterName = "GroupId",
            //        Value = groupId
            //    }, new SqlParameter
            //    {
            //        ParameterName = "ResourceId",
            //        Value = resourceId
            //    }).ToList<Sys_GroupPermission2>();
        }

        public List<Sys_GroupPermission2> GetGroupPermissionByGroup(Guid groupId)
        {
            var status = string.Empty;
            List<object> obj = new List<object>();
            obj.Add(groupId);
            obj.Add(null);
            return ExecuteQuery(obj, ConstantSql.hrm_sys_sp_get_GroupPermissionByGroupAndResource, true, string.Empty, ref status).ToList();
        }


    }
}
