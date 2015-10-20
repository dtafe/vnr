using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using HRM.Data.Main.Data.Sql;
using System.Collections.Generic;
using HRM.Data.Entity;


namespace HRM.Data.Attendance.Data.Sql.Repositories
{

    public class Att_OvertimeRepository : CustomBaseRepository<Att_Overtime>
    {
        
        public Att_OvertimeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        

        #region Store Procedure

        /// <summary>
        /// Gọi store add column hrm_sys_sp_altertable_AddColumn [Tam.Le - 2014/05/15]
        /// </summary>
        /// <returns></returns>
        public void SaveOvertimeConfirm(Guid OvertimeId, double? ConfirmHours)
        {
            if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
            {
                this.UnitOfWork.Context.Database.SqlQuery<Att_OvertimeEntity>(ConstantSql.hrm_att_sp_save_OvertimeConfirm,
                    new OracleParameter()
                    {
                        ParameterName = "OvertimeId",
                        Value = OvertimeId,
                        OracleDbType = OracleDbType.NVarChar,
                        DbType = System.Data.DbType.Guid
                    },
                     new OracleParameter()
                     {
                         ParameterName = "ConfirmHours",
                         Value = ConfirmHours,
                         OracleDbType = OracleDbType.Number,
                         DbType = System.Data.DbType.Double
                     }
                    ).ToList();
            }
            else
            {
                this.UnitOfWork.Context.Database.SqlQuery<Att_OvertimeEntity>(ConstantSql.hrm_att_sp_save_OvertimeConfirm, new SqlParameter
                {
                    ParameterName = "OvertimeId",
                    Value = OvertimeId
                }, new SqlParameter
                {
                    ParameterName = "ConfirmHours",
                    Value = ConfirmHours
                }).ToList();
            }
        }
        /// <summary>
        /// Update lai trang thai cua overtime
        /// </summary>
        /// <returns></returns>
        public void SetOvertimeStatus(Guid Id, string Status)
        {
            if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
            {
                this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_Set_Overtime_Status,
                    new OracleParameter()
                    {
                        ParameterName = "Id",
                        Value = Id,
                        OracleDbType = OracleDbType.NVarChar,
                        DbType = System.Data.DbType.Guid
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Status",
                        Value = Status,
                        OracleDbType = OracleDbType.NVarChar,
                        DbType = System.Data.DbType.String
                    }
                    ).ToList();
            }
            else
            {
                this.UnitOfWork.Context.Database.SqlQuery<Att_RosterEntity>(ConstantSql.hrm_att_sp_Set_Overtime_Status,
                               new SqlParameter
                               {
                                   ParameterName = "Id",
                                   Value = Id
                               },
                               new SqlParameter
                               {
                                   ParameterName = "Status",
                                   Value = Status
                               }).ToList();
            }
        }
        #endregion

    }
}
