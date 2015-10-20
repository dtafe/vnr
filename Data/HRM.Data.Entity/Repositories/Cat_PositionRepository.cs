using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_PositionRepository : CustomBaseRepository<Cat_Position>
    {
        public Cat_PositionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
#region HRM8- 20140720
		
        //#region HRM8.20/7/2014
        //public IQueryable<Cat_Position> GetAllCatPositions()
        //{
        //    IQueryable<Cat_Position> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}
        //// <summary>
        //// Lấy tất cả danh sách chức vụ bởi store hrm_cat_sp_get_Position
        //// </summary>
        //// <returns></returns>
        //public IQueryable<Cat_Position> GetPositions()
        //{
        //    IQueryable<Cat_Position> result = this.DbSet.ToList().AsQueryable();
        //    var data = this.UnitOfWork.Context.Database.SqlQuery<Cat_PositionEntity>(ConstantSql.hrm_cat_sp_get_Position).ToList<Cat_PositionEntity>();
        //    return result;
        //}
        //// <summary>
        //// Lấy tất cả danh sách chức vụ bởi store hrm_cat_sp_get_Position
        //// </summary>
        //// <returns></returns>
        //public IQueryable<Cat_PositionEntity> GetPositions(ListQueryModel model)
        //{
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_PositionEntity>(ConstantSql.hrm_cat_sp_get_Position).ToList<Cat_PositionEntity>().AsQueryable();
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_cat_sp_get_Position);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_PositionEntity>(param.SqlQuery, param.Params).ToList<Cat_PositionEntity>().AsQueryable();
        //}
        //// <summary>
        //// Lấy tất cả danh sách chức vụ bởi store [hrm_cat_sp_get_Position_Multi]
        //// </summary>
        //// <returns></returns>       
        //public IQueryable<Cat_PositionMultiEntity> GetMulti(object keyword)
        //{
        //    var dataSearch = keyword == null ? System.DBNull.Value : keyword;
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //        this.UnitOfWork.Context.Database.SqlQuery<Cat_PositionMultiEntity>(ConstantSql.hrm_cat_sp_get_Position_Multi,
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
        //            .ToList<Cat_PositionMultiEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database
        //        .SqlQuery<Cat_PositionMultiEntity>(ConstantSql.hrm_cat_sp_get_Position_Multi
        //        , new SqlParameter { ParameterName = "Keyword", Value = dataSearch, Size = 100 })
        //        .ToList<Cat_PositionMultiEntity>().AsQueryable();
        //    }

        //}   
	#endregion
    }
}
