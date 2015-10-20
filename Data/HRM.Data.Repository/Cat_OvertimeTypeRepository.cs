using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_OvertimeTypeRepository : CustomBaseRepository<Cat_OvertimeType>
    {
        public Cat_OvertimeTypeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        #region HRM82.20/7/2014
        //public IQueryable<Cat_OvertimeType> GetAllCatOvertimeType()
        //{
        //    IQueryable<Cat_OvertimeType> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách OvertimeType theo Store
        ///// </summary>
        ///// <returns></returns>
        //public List<Cat_OvertimeTypeEntity> GetOvertimeType()
        //{
        //    return this.UnitOfWork.Context.Database
        //        .SqlQuery<Cat_OvertimeTypeEntity>(ConstantSql.hrm_cat_sp_get_OvertimeType).ToList<Cat_OvertimeTypeEntity>();
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách dữ liệu show lên Multi
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_OvertimeTypeMultiEntity> GetMulti(object keyword)
        //{
        //    var dataSearch = keyword == null ? System.DBNull.Value : keyword;
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //        this.UnitOfWork.Context.Database.SqlQuery<Cat_OvertimeTypeMultiEntity>(ConstantSql.hrm_cat_sp_get_OvertimeType_multi,
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
        //            .ToList<Cat_OvertimeTypeMultiEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database
        //        .SqlQuery<Cat_OvertimeTypeMultiEntity>(ConstantSql.hrm_cat_sp_get_OvertimeType_multi
        //        , new SqlParameter { ParameterName = "Keyword", Value = dataSearch, Size = 100 })
        //        .ToList<Cat_OvertimeTypeMultiEntity>()
        //        .AsQueryable();
        //    }

        //}
        ///// <summary>
        ///// <summary> Lấy tất cả danh sách OvertimeType bởi store hrm_cat_sp_get_OvertimeType [Tam.Le - 2014/05/14] </summary>
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_OvertimeTypeEntity> GetOvertimeTypes(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_cat_sp_get_OvertimeType);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_OvertimeTypeEntity>(param.SqlQuery, param.Params).ToList<Cat_OvertimeTypeEntity>().AsQueryable();
        //    //ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_cat_sp_get_OvertimeType);
        //    //return this.UnitOfWork.Context.Database.SqlQuery<Cat_OvertimeTypeEntity>(param.SqlQuery, param.Params).ToList<Cat_OvertimeTypeEntity>().AsQueryable();
        //}
        #endregion
    }
}
