using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_RelativeTypeRepository : CustomBaseRepository<Cat_RelativeType>
    {
        public Cat_RelativeTypeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #region HRM 8 - 2014-07-20
        //public IQueryable<Cat_RelativeType> GetAll()
        //{
        //    IQueryable<Cat_RelativeType> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách dữ liệu show lên Multi
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_RelativeTypeMultiEntity> GetMulti(object keyword)
        //{
        //    var dataSearch = keyword == null ? System.DBNull.Value : keyword;
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //        this.UnitOfWork.Context.Database.SqlQuery<Cat_RelativeTypeMultiEntity>(ConstantSql.hrm_cat_sp_get_RelativeType_multi,
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
        //            .ToList<Cat_RelativeTypeMultiEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database
        //      .SqlQuery<Cat_RelativeTypeMultiEntity>(ConstantSql.hrm_cat_sp_get_RelativeType_multi
        //      , new SqlParameter { ParameterName = "Keyword", Value = dataSearch, Size = 100 })
        //      .ToList<Cat_RelativeTypeMultiEntity>()
        //      .AsQueryable();
        //    }

        //}

        ///// <summary>
        ///// Lấy tất cả danh sách Loại Hợp đồng store hrm_hr_sp_get_RelativeType
        ///// </summary>
        ///// <returns></returns>
        ///// 
        //public IQueryable<Cat_RelativeType> GetRelativeTypeType(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_cat_sp_get_RelativesType);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_RelativeType>(param.SqlQuery, param.Params).ToList<Cat_RelativeType>().AsQueryable();
        //}

        //public List<Cat_RelativeTypeMultiEntity> GetRelativeTypeByIds(string selectedIds)
        //{
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_RelativeTypeMultiEntity>(ConstantSql.hrm_cat_sp_get_RelativesTypeIds, new SqlParameter
        //    {
        //        ParameterName = "selectedIds",
        //        Value = selectedIds
        //    }).ToList<Cat_RelativeTypeMultiEntity>();
        //}

        ///// <summary>
        ///// Lấy Thông Tin Đối Tương Loại Hợp đồng Bởi ID
        ///// </summary>
        ///// <param name="profileID"></param>
        ///// <returns>Đối tượng Contract của tần businness</returns>
        //public Cat_RelativeTypeMultiEntity GetCatRelativeTypeByID(int RelativeTypeID)
        //{
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_RelativeTypeMultiEntity>(ConstantSql.hrm_cat_sp_get_RelativesTypeById, new SqlParameter
        //    {
        //        ParameterName = "RelativeTypeID",
        //        Value = RelativeTypeID
        //    }).ToList<Cat_RelativeTypeMultiEntity>().FirstOrDefault<Cat_RelativeTypeMultiEntity>();
        //}

        //public IQueryable<Cat_RelativeType> GetRelative()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public IQueryable<Cat_RelativeType> GetAllRelative()
        //{
        //    throw new System.NotImplementedException();
        //} 
        #endregion
    }
}
