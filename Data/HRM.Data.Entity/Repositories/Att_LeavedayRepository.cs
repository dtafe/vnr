using System;
using System.Linq;
using System.Linq.Expressions;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Att_LeavedayRepository : CustomBaseRepository<Att_LeaveDay>
    {
        public Att_LeavedayRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #region MyRegion

        //public IQueryable<Att_Leaveday> Get()
        //{
        //    IQueryable<Att_Leaveday> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách Leaveday bởi store hrm_hr_sp_get_Leaveday
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_LeavedayEntity> GetLeavedays(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_att_sp_get_Leaveday);
        //    var rs = this.UnitOfWork.Context.Database.SqlQuery<Att_LeavedayEntity>(param.SqlQuery, param.Params).ToList<Att_LeavedayEntity>().AsQueryable();
        //    return rs;
        //}



        

        ///// <summary>
        ///// Lấy danh sách Xuất Báo Cáo
        ///// </summary>
        ///// <returns></returns>
        //public List<Att_LeavedayEntity> GetByIds(string selectedIds)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //        this.UnitOfWork.Context.Database.SqlQuery<Att_LeavedayEntity>(ConstantSql.hrm_att_sp_get_LeavedayByIds,
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
        //            ).ToList<Att_LeavedayEntity>();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_LeavedayEntity>(ConstantSql.hrm_att_sp_get_LeavedayByIds, new SqlParameter
        //        {
        //            ParameterName = "selectedIds",
        //            Value = selectedIds
        //        }).ToList<Att_LeavedayEntity>();
        //    }
        //}

        
        #endregion

        public bool UpdateStatusLeaveday(string selectedIds, string type)
        {
            try
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    var x = this.UnitOfWork.Context.Database.SqlQuery<Att_LeaveDayEntity>(ConstantSql.hrm_att_sp_get_Leaveday_UpdateStatus,
                        new OracleParameter()
                        {
                            ParameterName = "selectedIds",
                            Value = selectedIds,
                            OracleDbType = OracleDbType.NVarChar,
                            DbType = System.Data.DbType.String
                        },
                         new OracleParameter()
                         {
                             ParameterName = "status",
                             Value = type,
                             OracleDbType = OracleDbType.NVarChar,
                             DbType = System.Data.DbType.String
                         },
                        new OracleParameter()
                        {
                            ParameterName = "R_Output",
                            OracleDbType = OracleDbType.Cursor,
                            Direction = System.Data.ParameterDirection.Output
                        }
                        ).ToList<Att_LeaveDayEntity>();
                }
                else
                {
                    var x = this.UnitOfWork.Context.Database.SqlQuery<Att_LeaveDayEntity>(ConstantSql.hrm_att_sp_get_Leaveday_UpdateStatus, new SqlParameter
                    {
                        ParameterName = "selectedIds",
                        Value = selectedIds
                    }, new SqlParameter
                    {
                        ParameterName = "status",
                        Value = type
                    }).ToList<Att_LeaveDayEntity>();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///// <summary>
        ///// Lấy đối tượng ngày nghỉ theo Profile và Cutoff
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Att_LeavedayEntity> GetByProIDandCutOID(int ProfileID, int CutOffID)
        //{
        //    //return this.UnitOfWork.Context.Database.SqlQuery<Att_LeavedayEntity>(ConstantSql.hrm_att_sp_get_LeavedayByProIDandCutOID,
        //    //    new SqlParameter
        //    //    {
        //    //        ParameterName = "ProfileID",
        //    //        Value = ProfileID
        //    //    },
        //    //    new SqlParameter
        //    //    {
        //    //        ParameterName = "CutOffID",
        //    //        Value = CutOffID
        //    //    }).ToList<Att_LeavedayEntity>().AsQueryable();


        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //       this.UnitOfWork.Context.Database.SqlQuery<Att_LeavedayEntity>(ConstantSql.hrm_att_sp_get_LeavedayByProIDandCutOID,
        //            new OracleParameter()
        //            {
        //                ParameterName = "ProfileID",
        //                Value = ProfileID,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "CutOffID",
        //                Value = CutOffID,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Att_LeavedayEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_LeavedayEntity>(ConstantSql.hrm_att_sp_get_LeavedayByProIDandCutOID,
        //       new SqlParameter
        //       {
        //           ParameterName = "ProfileID",
        //           Value = ProfileID
        //       },
        //       new SqlParameter
        //       {
        //           ParameterName = "CutOffID",
        //           Value = CutOffID
        //       }).ToList<Att_LeavedayEntity>().AsQueryable();
        //    }


        //}
        //public IQueryable<Att_LeavedayEntity> GetByProIDandCutOID(int ProfileID, int CutOffID)
        //{
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //       this.UnitOfWork.Context.Database.SqlQuery<Att_LeavedayEntity>(ConstantSql.hrm_att_sp_get_OvertimeByProIDandCutOID,
        //            new OracleParameter()
        //            {
        //                ParameterName = "ProfileID",
        //                Value = ProfileID,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "CutOffID",
        //                Value = CutOffID,
        //                OracleDbType = OracleDbType.Number,
        //                DbType = System.Data.DbType.Int32
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            ).ToList<Att_LeavedayEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database.SqlQuery<Att_LeavedayEntity>(ConstantSql.hrm_att_sp_get_OvertimeByProIDandCutOID,
        //   new SqlParameter
        //   {
        //       ParameterName = "ProfileID",
        //       Value = ProfileID
        //   },
        //   new SqlParameter
        //   {
        //       ParameterName = "CutOffID",
        //       Value = CutOffID
        //   }).ToList<Att_LeavedayEntity>().AsQueryable();
        //    }
        //}
    }
}
