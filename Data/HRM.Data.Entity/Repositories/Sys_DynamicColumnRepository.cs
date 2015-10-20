using System.Data.SqlClient;
using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;

namespace HRM.Data.Entity.Repositories
{
    public class Sys_DynamicColumnRepository : CustomBaseRepository<Sys_DynamicColumn>
    {
        public Sys_DynamicColumnRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        ///// <summary>
        ///// Lấy tất cả danh sách DynamicColumn bởi store [hrm_sys_sp_get_DynamicColumn] [Tam.Le - 2014/06/12]
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Sys_DynamicColumnEntity> GetDynamicColumns(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_sys_sp_get_DynamicColumn);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Sys_DynamicColumnEntity>(param.SqlQuery, param.Params).ToList<Sys_DynamicColumnEntity>().AsQueryable();
        //}
        //public IQueryable<Sys_DynamicColumn> Get()
        //{
        //    IQueryable<Sys_DynamicColumn> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}
        /// <summary>
        /// Gọi store add column hrm_sys_sp_altertable_AddColumn [Tam.Le - 2014/05/15]
        /// </summary>
        /// <returns></returns>
        public void AddColumn(string tablename, string columnname, string datatype)
        {
            this.UnitOfWork.Context.Database.SqlQuery<Sys_DynamicColumn>(ConstantSql.hrm_sys_sp_altertable_AddColumn, new SqlParameter
            {
                ParameterName = "tablename",
                Value = tablename
            }, new SqlParameter
            {
                ParameterName = "columnname",
                Value = columnname
            }, new SqlParameter
            {
                ParameterName = "datatype",
                Value = datatype
            }).ToList();
        }
        /// <summary>
        /// Gọi store remove column [hrm_sys_sp_altertable_DeleteColumn] [Tam.Le - 2014/05/15]
        /// </summary>
        /// <returns></returns>
        public void RemoveColumn(string tablename, string columnname)
        {
            this.UnitOfWork.Context.Database.SqlQuery<Sys_DynamicColumn>(ConstantSql.hrm_sys_sp_altertable_DeleteColumn, new SqlParameter
            {
                ParameterName = "tablename",
                Value = tablename
            }, new SqlParameter
            {
                ParameterName = "columnname",
                Value = columnname
            }).ToList();
        }

    }
}
