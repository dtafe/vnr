using System;
using System.Linq;
using System.Linq.Expressions;
using HRM.Data.BaseRepository;

using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;

using Devart.Data.Oracle;
using System.Collections.Generic;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Can_TamScanLogRepository : CustomBaseRepository<Can_TamScanLogCMS>
    {
        public Can_TamScanLogRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        /// <summary>
        /// Xóa dữ liệu quẹt thẻ bởi store hrm_att_sp_delete_TAMScanLog
        /// </summary>
        /// <returns></returns>
        public void DeleteTAMScanLog(string CardCode, DateTime DateFrom, DateTime DateTo)
        {
            if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
            {
                
              this.UnitOfWork.Context.Database.SqlQuery<Can_TamScanLogCMS>(ConstantSql.hrm_can_sp_delete_TAMScanLog,
                    new OracleParameter()
                    {
                        ParameterName = "CardCode",
                        Value = CardCode,
                        OracleDbType = OracleDbType.NVarChar,
                        DbType = System.Data.DbType.String
                    },
                      new OracleParameter()
                      {
                          ParameterName = "DateFrom",
                          Value = DateFrom,
                          OracleDbType = OracleDbType.Date,
                          DbType = System.Data.DbType.Date
                      },
                    new OracleParameter()
                    {
                        ParameterName = "DateTo",
                        Value = DateTo,
                        OracleDbType = OracleDbType.Date,
                        DbType = System.Data.DbType.Date
                    }
                    ).ToList<Can_TamScanLogCMS>();
            }
            else
            {
                this.UnitOfWork.Context.Database.SqlQuery<Can_TamScanLogCMS>(ConstantSql.hrm_can_sp_delete_TAMScanLog, new SqlParameter
                {
                    ParameterName = "CardCode",
                    Value = CardCode
                }, new SqlParameter
                {
                    ParameterName = "DateFrom",
                    Value = DateFrom
                }, new SqlParameter
                {
                    ParameterName = "DateTo",
                    Value = DateTo
                }).ToList();
            }
        }

    }
}
