using System.Linq;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace HRM.Data.Entity.Repositories
{

    public class Att_AllowLimitOvertimeRepository : GenericRepository<Att_AllowLimitOvertime>
    {
        public Att_AllowLimitOvertimeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IQueryable<Att_AllowLimitOvertime> Get()
        {
            IQueryable<Att_AllowLimitOvertime> result = this.DbSet.ToList().AsQueryable();
            return result;
        }
        /// <summary>
        /// Lấy tất cả danh sách overtime bởi store hrm_att_sp_get_AllowLimitOvertime
        /// </summary>
        /// <returns></returns>
        public IQueryable<Att_AllowLimitOvertimeEntity> GetAllowLimitOvertimes(ListQueryModel model)
        {
            ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_att_sp_get_AllowLimitOvertime);
            return this.UnitOfWork.Context.Database.SqlQuery<Att_AllowLimitOvertimeEntity>(param.SqlQuery, param.Params).ToList<Att_AllowLimitOvertimeEntity>().AsQueryable();
        }
        /// <summary>
        /// Lấy tất cả danh sách overtime bởi store hrm_att_sp_get_AllowLimitOvertimebyid
        /// </summary>
        /// <returns></returns>
        public Att_AllowLimitOvertimeEntity GetAllowLimitOvertimesbyid(int allowLimitOvertimebyid)
        {
            if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
            {
                return this.UnitOfWork.Context.Database.SqlQuery<Att_AllowLimitOvertimeEntity>(ConstantSql.hrm_att_sp_get_AllowLimitOvertimebyid,
                    new OracleParameter()
                    {
                        ParameterName = "allowLimitOvertimebyid",
                        Value = allowLimitOvertimebyid,
                        OracleDbType = OracleDbType.Number,
                        DbType = System.Data.DbType.Int32
                    },
                    new OracleParameter()
                    {
                        ParameterName = "R_Output",
                        OracleDbType = OracleDbType.Cursor,
                        Direction = System.Data.ParameterDirection.Output
                    }
                    ).ToList<Att_AllowLimitOvertimeEntity>().FirstOrDefault<Att_AllowLimitOvertimeEntity>();
            }
            else
            {
                return this.UnitOfWork.Context.Database.SqlQuery<Att_AllowLimitOvertimeEntity>(ConstantSql.hrm_att_sp_get_AllowLimitOvertimebyid, new SqlParameter
                {
                    ParameterName = "allowLimitOvertimebyid",
                    Value = allowLimitOvertimebyid
                }).ToList<Att_AllowLimitOvertimeEntity>().FirstOrDefault<Att_AllowLimitOvertimeEntity>();
            }
        }

        public List<Att_AllowLimitOvertimeEntity> GetAllowLimitOvertimeByIds(string selectedIds)
        {
            return this.UnitOfWork.Context.Database.SqlQuery<Att_AllowLimitOvertimeEntity>(ConstantSql.hrm_att_sp_get_AllowLimitOvertimeByIds, new SqlParameter
            {
                ParameterName = "selectedIds",
                Value = selectedIds
            }).ToList<Att_AllowLimitOvertimeEntity>();
        }
    }
}
