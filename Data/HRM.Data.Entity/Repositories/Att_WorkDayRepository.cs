using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Att_WorkDayRepository : CustomBaseRepository<Att_Workday>
    {
        public Att_WorkDayRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        #region HRM8 - 20140720
        
        /// <summary>
        /// Lấy tất cả danh sách overtime bởi store hrm_att_sp_get_WorkDay
        /// </summary>
        /// <returns></returns> 
        public IQueryable<Att_Workday> GetWorkDays(ListQueryModel model)
        {
            ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_att_sp_get_WorkDay);
            var data = this.UnitOfWork.Context.Database
                .SqlQuery<Att_Workday>(param.SqlQuery, param.Params)
                .ToList<Att_Workday>().AsQueryable();
            return data;
        }


        public List<Att_Workday> GetWorkDaysByInOut(DateTime inTime, DateTime outTime)
        {
            if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
            {
                return
                 this.UnitOfWork.Context.Database.SqlQuery<Att_Workday>(ConstantSql.hrm_att_sp_get_WorkDayByInOut,
                    new OracleParameter()
                    {
                        ParameterName = "inTime",
                        Value = inTime,
                        OracleDbType = OracleDbType.Date,
                        DbType = System.Data.DbType.DateTime
                    },
                     new OracleParameter()
                     {
                         ParameterName = "outTime",
                         Value = outTime,
                         OracleDbType = OracleDbType.Date,
                         DbType = System.Data.DbType.DateTime
                     },
                    new OracleParameter()
                    {
                        ParameterName = "R_Output",
                        OracleDbType = OracleDbType.Cursor,
                        Direction = System.Data.ParameterDirection.Output
                    }
                    ).ToList<Att_Workday>();
            }
            else
            {
                return this.UnitOfWork.Context.Database.SqlQuery<Att_Workday>(ConstantSql.hrm_att_sp_get_WorkDayByInOut, new SqlParameter
                {
                    ParameterName = "inTime",
                    Value = inTime
                }, new SqlParameter
                {
                    ParameterName = "outTime",
                    Value = outTime
                }).ToList<Att_Workday>();
            }
        }

        
        #endregion


    }
}
