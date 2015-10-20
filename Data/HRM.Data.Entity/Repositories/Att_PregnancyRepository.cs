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

    public class Att_PregnancyRepository : CustomBaseRepository<Att_Pregnancy>
    {
        public Att_PregnancyRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #region HRM8.20/7/2014
        //public IQueryable<Att_Pregnancy> Get()
        //{
        //    IQueryable<Att_Pregnancy> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách hợp đồng bởi store hrm_att_sp_get_Pregnancy
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_PregnancyEntity> GetPregnancy(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_att_sp_get_Pregnancy);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Att_PregnancyEntity>(param.SqlQuery, param.Params).ToList<Att_PregnancyEntity>().AsQueryable();
        //}

        ///// <summary>
        ///// Lấy danh sách Xuất Báo Cáo
        ///// </summary>
        ///// <returns></returns>
        //public List<Att_PregnancyEntity> GetByIds(string selectedIds)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //      this.UnitOfWork.Context.Database.SqlQuery<Att_PregnancyEntity>(ConstantSql.hrm_att_sp_get_PregnancyByIds, 
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
        //            ).ToList<Att_PregnancyEntity>();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_PregnancyEntity>(ConstantSql.hrm_att_sp_get_PregnancyByIds, new SqlParameter
        //        {
        //            ParameterName = "selectedIds",
        //            Value = selectedIds
        //        }).ToList<Att_PregnancyEntity>();
        //    }
        //} 
        #endregion
    }
}
