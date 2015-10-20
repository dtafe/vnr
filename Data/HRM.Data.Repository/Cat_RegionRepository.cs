using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_RegionRepository : CustomBaseRepository<Cat_Region>
    {
        public Cat_RegionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        #region HRM 8 - 2014-07-20
        //public IQueryable<Cat_Region> Get()
        //{
        //    var rs = this.DbSet.ToList().AsQueryable();
        //    return rs;
        //}

        //public IQueryable<Cat_Region> GetRegion(string name, string code)
        //{
        //    var rs = this.DbSet.ToList().AsQueryable();
        //    if(!string.IsNullOrWhiteSpace(name)){
        //        return rs = this.DbSet.Where(i=> i.RegionName.Contains(name)).ToList().AsQueryable();
        //    }
        //    if (!string.IsNullOrWhiteSpace(code))
        //    {
        //        return rs = this.DbSet.Where(i => i.Code.Contains(code)).ToList().AsQueryable();
        //    }
        //    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(code))
        //    {
        //        return rs = this.DbSet.Where(i => i.RegionName.Contains(name) || i.Code.Contains(code)).ToList().AsQueryable();
        //    }
        //    return rs;
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách Loại Hợp đồng show lên Multi
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_RegionMultiEntity> GetMulti(object keyword)
        //{
        //    var dataSearch = keyword == null ? System.DBNull.Value : keyword;
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //        this.UnitOfWork.Context.Database.SqlQuery<Cat_RegionMultiEntity>(ConstantSql.hrm_cat_sp_get_Region_multi,
        //            new OracleParameter()
        //            {
        //                ParameterName = "Keyword",
        //                Value = dataSearch,
        //                OracleDbType = OracleDbType.NVarChar,
        //                DbType = System.Data.DbType.String
        //            },
        //            new OracleParameter()
        //            {
        //                ParameterName = "R_Output",
        //                OracleDbType = OracleDbType.Cursor,
        //                Direction = System.Data.ParameterDirection.Output
        //            }
        //            )
        //            .ToList<Cat_RegionMultiEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database
        //       .SqlQuery<Cat_RegionMultiEntity>(ConstantSql.hrm_cat_sp_get_Region_multi
        //       , new SqlParameter { ParameterName = "Keyword", Value = dataSearch, Size = 100 })
        //       .ToList<Cat_RegionMultiEntity>()
        //       .AsQueryable();
        //    }

        //}
        #endregion
    }
}
