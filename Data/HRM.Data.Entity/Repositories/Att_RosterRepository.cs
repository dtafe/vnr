using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Att_RosterRepository : CustomBaseRepository<Att_Roster>
    {
        public Att_RosterRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #region HRM8 - 20140720
        //public IQueryable<Att_Roster> Get()
        //{
        //    IQueryable<Att_Roster> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}


        ///// <summary>
        ///// [Hieu.Van] 09/07/2014
        ///// Lấy danh sách nhân viên không có ca làm việc
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public List<Att_ProfileEntity> GetProfileNotRoster(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_att_sp_get_Roster_ProfileNotRoster);
        //    var data = this.UnitOfWork.Context.Database.SqlQuery<Att_ProfileEntity>(param.SqlQuery, param.Params).ToList<Att_ProfileEntity>();
        //    return data;
        //}

        ///// <summary>
        ///// Lấy danh sách Xuất Báo Cáo Chưa Có Ca Làm Viêc
        ///// </summary>
        ///// <returns></returns>
        //public List<Att_ProfileEntity> GetProfileNotRosterByIds(string selectedIds)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //        this.UnitOfWork.Context.Database.SqlQuery<Att_ProfileEntity>(ConstantSql.hrm_att_sp_get_Roster_ProfileNotRosterByIds,
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
        //            ).ToList<Att_ProfileEntity>();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_ProfileEntity>(ConstantSql.hrm_att_sp_get_Roster_ProfileNotRosterByIds, new SqlParameter
        //        {
        //            ParameterName = "selectedIds",
        //            Value = selectedIds
        //        }).ToList<Att_ProfileEntity>();
        //    }
        //}

        ///// <summary>
        ///// Lấy danh sách Xuất Báo Cáo
        ///// </summary>
        ///// <returns></returns>
        //public List<Att_RosterEntity> GetByIds(string selectedIds)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //        this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_get_RosterByIds,
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
        //            ).ToList<Att_RosterEntity>();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_get_RosterByIds, new SqlParameter
        //        {
        //            ParameterName = "selectedIds",
        //            Value = selectedIds
        //        }).ToList<Att_RosterEntity>();
        //    }
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách ca làm việc bởi store hrm_att_sp_get_Roster
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_RosterEntity> GetRosters()
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //       this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_get_Roster,
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Att_RosterEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_get_Roster).ToList<Att_RosterEntity>().AsQueryable();
        //    }
        //}
        ///// <summary>
        ///// Lấy tất cả danh sách ca làm việc bởi store hrm_att_sp_get_Roster
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_RosterEntity> GetRosters(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_att_sp_get_Roster);
        //    var rs = this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(param.SqlQuery, param.Params).ToList<Att_RosterEntity>().AsQueryable();
        //    return rs;
        //}
        ///// <summary>
        ///// Lấy đối tượng roster bởi store hrm_att_sp_get_RosterById [Tung.Ly - 2014/05/08]
        ///// </summary>
        ///// <returns></returns>
        //public Att_Roster GetRosterById(int rosterId)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //       this.UnitOfWork.Context.Database.SqlQuery<Att_Roster>(ConstantSql.hrm_att_sp_get_RosterById,
        //            new OracleParameter()
        //            {
        //                ParameterName = "RosterId",
        //                Value = rosterId,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Att_Roster>().FirstOrDefault<Att_Roster>();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_Roster>(ConstantSql.hrm_att_sp_get_RosterById, new SqlParameter
        //        {
        //            ParameterName = "RosterId",
        //            Value = rosterId
        //        }).ToList<Att_Roster>().FirstOrDefault<Att_Roster>();
        //    }
        //}

        //public IQueryable<Att_RosterEntity> GetRosterByProfileId(int profileid)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //      this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_get_RosterByProfileId,
        //            new OracleParameter()
        //            {
        //                ParameterName = "ProfileId",
        //                Value = profileid,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Att_RosterEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_get_RosterByProfileId, new SqlParameter
        //        {
        //            ParameterName = "profileid",
        //            Value = profileid
        //        }).ToList<Att_RosterEntity>().AsQueryable();
        //    }
        //}

        ///// <summary>
        ///// Lấy đối tượng lịch làm việc
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_RosterEntity> GetByProIDandCutOID(int ProfileID, int CutOffID)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //       this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_get_RosterByProIDandCutOID,
        //            new OracleParameter()
        //            {
        //                ParameterName = "ProfileID",
        //                Value = ProfileID,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //              new OracleParameter()
        //              {
        //                  ParameterName = "CutOffID",
        //                  Value = CutOffID,
        //                  OracleDbType = OracleDbType.Number,
        //                  DbType = System.Data.DbType.Int32
        //              },
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Att_RosterEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_get_RosterByProIDandCutOID,
        //                      new SqlParameter
        //                      {
        //                          ParameterName = "ProfileID",
        //                          Value = ProfileID
        //                      },
        //                      new SqlParameter
        //                      {
        //                          ParameterName = "CutOffID",
        //                          Value = CutOffID
        //                      }).ToList<Att_RosterEntity>().AsQueryable();
        //    }
        //}
        ///// <summary>
        ///// Update lai trang thai cua roster
        ///// </summary>
        ///// <returns></returns>
        //public void SetRosterStatus(int Id, string Status)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_Set_Roster_Status,
        //            new OracleParameter()
        //            {
        //                ParameterName = "Id",
        //                Value = Id,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "Status",
        //                Value = Status,
        //                OracleDbType = OracleDbType.NVarChar,
        //                DbType = System.Data.DbType.String
        //            }
        //            ).ToList();
        //    }
        //    else
        //    {
        //        this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_Set_Roster_Status,
        //        new SqlParameter
        //        {
        //            ParameterName = "Id",
        //            Value = Id
        //        },
        //        new SqlParameter
        //        {
        //            ParameterName = "Status",
        //            Value = Status
        //        }).ToList();
        //    }
        //} 
        #endregion
    }
}
