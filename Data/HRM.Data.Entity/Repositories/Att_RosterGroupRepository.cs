using System;
using System.Linq;
using System.Linq.Expressions;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Att_RosterGroupRepository : CustomBaseRepository<Att_RosterGroup>
    {
        public Att_RosterGroupRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #region HRM8 - 20140720
        //public IQueryable<Att_RosterGroup> Get()
        //{
        //    IQueryable<Att_RosterGroup> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        ///// <summary>
        ///// Lấy danh sách Xuất Báo Cáo
        ///// </summary>
        ///// <returns></returns>
        //public List<Att_RosterGroupEntity> GetByIds(string selectedIds)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //      this.UnitOfWork.Context.Database.SqlQuery<Att_RosterGroupEntity>(ConstantSql.hrm_att_sp_get_RosterGroupByIds,
        //            new OracleParameter()
        //            {
        //                ParameterName = "selectedIds",
        //                Value = selectedIds,
        //                OracleDbType = OracleDbType.NVarChar,
        //                DbType = System.Data.DbType.String
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Att_RosterGroupEntity>();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_RosterGroupEntity>(ConstantSql.hrm_att_sp_get_RosterGroupByIds, new SqlParameter
        //        {
        //            ParameterName = "selectedIds",
        //            Value = selectedIds
        //        }).ToList<Att_RosterGroupEntity>();
        //    }
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách hợp đồng bởi store hrm_att_sp_get_RosterGroup
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_RosterGroupEntity> GetRosterGroup(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_att_sp_get_RosterGroup);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Att_RosterGroupEntity>(param.SqlQuery, param.Params).ToList<Att_RosterGroupEntity>().AsQueryable();
        //}
        ///// <summary>
        ///// Update lai trang thai cua roster group
        ///// </summary>
        ///// <returns></returns>
        //public void SetRosterGroupStatus(int Id, string Status)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //       this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_Set_RosterGroup_Status,
        //            new OracleParameter()
        //            {
        //                ParameterName = "Id",
        //                Value = Id,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //             new OracleParameter()
        //             {
        //                 ParameterName = "Status",
        //                 Value = Status,
        //                 OracleDbType = OracleDbType.NVarChar,
        //                 DbType = System.Data.DbType.String
        //             }
        //            ).ToList();
        //    }
        //    else
        //    {
        //        this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_Set_RosterGroup_Status,
        //                       new SqlParameter
        //                       {
        //                           ParameterName = "Id",
        //                           Value = Id
        //                       },
        //                       new SqlParameter
        //                       {
        //                           ParameterName = "Status",
        //                           Value = Status
        //                       }).ToList();
        //    }
        //} 
        #endregion
    }
}
