using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_ProvinceRepository : CustomBaseRepository<Cat_Province>
    {
        public Cat_ProvinceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #region HRM8.20/7.2014
        ///// <summary>
        ///// Lấy tất cả danh sách ca làm việc bởi store hrm_cat_sp_get_Province
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_ProvinceEntity> GetProvinces()
        //{

        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_ProvinceEntity>(ConstantSql.hrm_cat_sp_get_Province).ToList<Cat_ProvinceEntity>().AsQueryable();
        //}

        //public IQueryable<Cat_ProvinceEntity> GetProvinces(ListQueryModel model)
        //{

        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_cat_sp_get_Province);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_ProvinceEntity>(param.SqlQuery, param.Params).ToList<Cat_ProvinceEntity>().AsQueryable();
        //  //  return this.UnitOfWork.Context.Database.SqlQuery<Cat_ProvinceEntity>(ConstantSql.hrm_cat_sp_get_Province).ToList<Cat_ProvinceEntity>().AsQueryable();
        //}

        //public IQueryable<Cat_Province> GetAllCatProvince()
        //{
        //    IQueryable<Cat_Province> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        ///// <summary>
        ///// Lấy danh sách tỉnh thành bởi countryId [Tung.Ly - 2014/05/08]
        ///// </summary>
        ///// <param name="countryId"></param>
        ///// <returns></returns>
        //public IQueryable<Cat_Province> GetProvinceByCountryId(int countryId)
        //{
        //    IQueryable<Cat_Province> result = this.DbSet.Where(p=>p.CountryID == countryId && p.IsDelete == null).ToList().AsQueryable();
        //    return result;
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách Loại Hợp đồng show lên Multi
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_ProvinceMultiEntity> GetMulti(object keyword)
        //{
        //    var dataSearch = keyword == null ? System.DBNull.Value : keyword;
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //        this.UnitOfWork.Context.Database.SqlQuery<Cat_ProvinceMultiEntity>(ConstantSql.hrm_cat_sp_get_Province_multi,
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
        //            .ToList<Cat_ProvinceMultiEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database
        //       .SqlQuery<Cat_ProvinceMultiEntity>(ConstantSql.hrm_cat_sp_get_Province_multi
        //       , new SqlParameter { ParameterName = "Keyword", Value = dataSearch, Size = 100 })
        //       .ToList<Cat_ProvinceMultiEntity>()
        //       .AsQueryable();
        //    }

        //}
        #endregion
    }
}
