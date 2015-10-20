namespace HRM.Data.Entity.Repositories
{
    //Todo: TungLy_Problem Sys_GroupUser (chua biet co su dung ko ?)

    //public class Sys_GroupUserRepository : CustomBaseRepository<Sys_GroupUser>
    //{
    //    public Sys_GroupUserRepository(IUnitOfWork unitOfWork)
    //        : base(unitOfWork)
    //    {
    //    }
      

    //    ///// <summary>
    //    ///// Lấy danh sách Group users (sử dụng store hrm_sys_sp_get_GroupUser)
    //    ///// </summary>
    //    ///// <returns></returns>
    //    //public IQueryable<Sys_GroupUserEntity> GetGroupUsers()
    //    //{
    //    //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
    //    //    {
    //    //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUser,
    //    //            new OracleParameter()
    //    //            {
    //    //                ParameterName = "R_Output",
    //    //                OracleDbType = OracleDbType.Cursor,
    //    //                Direction = System.Data.ParameterDirection.Output
    //    //            }
    //    //            ).ToList<Sys_GroupUserEntity>().AsQueryable(); 
    //    //    }
    //    //    else
    //    //    {
    //    //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUser)
    //    //       .ToList<Sys_GroupUserEntity>().AsQueryable(); 
    //    //    }
    //    //}

    //    ///// <summary>
    //    ///// Lấy đối tượng Group User theo Id (sử dụng store hrm_sys_sp_get_GroupUserById)
    //    ///// </summary>
    //    ///// <param name="groupUserId"></param>
    //    ///// <returns></returns>
    //    //public Sys_GroupUserEntity GetGroupUserByGroupUserId(int groupUserId)
    //    //{
    //    //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
    //    //    {
    //    //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUserById,
    //    //            new OracleParameter()
    //    //            {
    //    //                ParameterName = "groupuserId",
    //    //                Value = groupUserId,
    //    //                OracleDbType = OracleDbType.Number,
    //    //                DbType = System.Data.DbType.Int32
    //    //            },
    //    //            new OracleParameter()
    //    //            {
    //    //                ParameterName = "R_Output",
    //    //                OracleDbType = OracleDbType.Cursor,
    //    //                Direction = System.Data.ParameterDirection.Output
    //    //            }
    //    //            ).ToList<Sys_GroupUserEntity>().FirstOrDefault<Sys_GroupUserEntity>();
    //    //    }
    //    //    else
    //    //    {
    //    //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUserById, new SqlParameter
    //    //        {
    //    //            ParameterName = "groupuserId",
    //    //            Value = groupUserId
    //    //        }).ToList<Sys_GroupUserEntity>().FirstOrDefault<Sys_GroupUserEntity>();
    //    //    }
    //    //}
        
    //    //public List<Sys_GroupUserEntity> GetGroupUserByGroupId(int groupId)
    //    //{
    //    //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
    //    //    {
    //    //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUserByGroupId,
    //    //            new OracleParameter()
    //    //            {
    //    //                ParameterName = "groupId",
    //    //                Value = groupId,
    //    //                OracleDbType = OracleDbType.Number,
    //    //                DbType = System.Data.DbType.Int32
    //    //            },
    //    //            new OracleParameter()
    //    //            {
    //    //                ParameterName = "R_Output",
    //    //                OracleDbType = OracleDbType.Cursor,
    //    //                Direction = System.Data.ParameterDirection.Output
    //    //            }
    //    //            ).ToList<Sys_GroupUserEntity>();
    //    //    }
    //    //    else
    //    //    {
    //    //        return this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUserByGroupId, new SqlParameter
    //    //        {
    //    //            ParameterName = "groupId",
    //    //            Value = groupId
    //    //        }).ToList<Sys_GroupUserEntity>();
    //    //    }
    //    //}
        
    //    public List<Sys_GroupUserEntity> GetGroupUserByUserId(int userId)
    //    {
    //        var listEntity = new List<Sys_GroupUserEntity>();
    //        var getOrgName = new List<Cat_OrgStructureEntity>();
    //        if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
    //        {
    //            getOrgName = this.UnitOfWork.Context.Database.SqlQuery<Cat_OrgStructureEntity>(ConstantSql.hrm_cat_sp_get_OrgStructure,
    //               new OracleParameter()
    //               {
    //                   ParameterName = "R_Output",
    //                   OracleDbType = OracleDbType.Cursor,
    //                   Direction = System.Data.ParameterDirection.Output
    //               }
    //               ).ToList<Cat_OrgStructureEntity>();

    //            listEntity = this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUserByUserId,
    //                new OracleParameter()
    //                {
    //                    ParameterName = "userId",
    //                    Value = userId,
    //                    OracleDbType = OracleDbType.Number,
    //                    DbType = System.Data.DbType.Int32
    //                },
    //                new OracleParameter()
    //                {
    //                    ParameterName = "R_Output",
    //                    OracleDbType = OracleDbType.Cursor,
    //                    Direction = System.Data.ParameterDirection.Output
    //                }
    //                ).ToList<Sys_GroupUserEntity>();
    //        }
    //        else
    //        {
    //            getOrgName = this.UnitOfWork.Context.Database.SqlQuery<Cat_OrgStructureEntity>(ConstantSql.hrm_cat_sp_get_OrgStructure).ToList<Cat_OrgStructureEntity>();
    //            listEntity = this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUserByUserId, new SqlParameter
    //            {
    //                ParameterName = "userId",
    //                Value = userId
    //            }).ToList<Sys_GroupUserEntity>();
    //        }

    //        foreach (var item in listEntity)
    //        {
    //            var orgName = item.OrgStructures.ToNumbers().Select(orgId => getOrgName.Where(d => d.Id == orgId).Select(d => d.OrgStructureName).FirstOrDefault()).ToList();
    //            item.OrgStructuresName = orgName.ToSplitString('|');
    //        }
    //        return listEntity;
    //    }


    //}
}
