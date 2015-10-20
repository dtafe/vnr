using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using Devart.Data.Oracle;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_GradeRepository : CustomBaseRepository<Cat_GradeCfg>
    {
        public Cat_GradeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        #region HRM8.20/7/2014

        //public IQueryable<Cat_Grade> GetAllCat_Grade()
        //{
        //    IQueryable<Cat_Grade> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}
        ///// <summary>
        ///// Lấy tất cả danh sách Grade store hrm_hr_sp_get_Cat_Grade
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_GradeEntity> GetCat_Grade(ListQueryModel model)

        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_cat_sp_get_Cat_Grade);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_GradeEntity>(param.SqlQuery, param.Params).ToList<Cat_GradeEntity>().AsQueryable();
        //  //  return this.UnitOfWork.Context.Database.SqlQuery<Cat_GradeEntity>(ConstantSql.hrm_cat_sp_get_Cat_Grade).ToList<Cat_GradeEntity>().AsQueryable();
        //}

        //public IQueryable<Cat_GradeEntity> GetCat_Grade()
        //{
        //      return this.UnitOfWork.Context.Database.SqlQuery<Cat_GradeEntity>(ConstantSql.hrm_cat_sp_get_Cat_Grade).ToList<Cat_GradeEntity>().AsQueryable();
        //}

        ///// <summary>
        ///// Lấy Thông Tin Đối Tương Grade Bởi ID
        ///// </summary>
        ///// <param name="profileID"></param>
        ///// <returns>Đối tượng Contract của tần businness</returns>
        //public Cat_GradeEntity GetCat_GradeByID(int GradeID)
        //{
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_GradeEntity>(ConstantSql.hrm_cat_sp_get_Cat_GradeById, new SqlParameter
        //    {
        //        ParameterName = "GradeID",
        //        Value = GradeID
        //    }).ToList<Cat_GradeEntity>().FirstOrDefault<Cat_GradeEntity>();
        //}

        ///// <summary>
        ///// Lấy tất cả danh sách dữ liệu show lên Multi
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_GradeMultiEntity> GetMulti(object keyword)
        //{
        //    var dataSearch = keyword == null ? System.DBNull.Value : keyword;
        //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //    {
        //        return
        //        this.UnitOfWork.Context.Database.SqlQuery<Cat_GradeMultiEntity>(ConstantSql.hrm_cat_sp_get_Grade_multi,
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
        //            .ToList<Cat_GradeMultiEntity>().AsQueryable();
        //    }
        //    else
        //    {
        //        return this.UnitOfWork.Context.Database
        //         .SqlQuery<Cat_GradeMultiEntity>(ConstantSql.hrm_cat_sp_get_Grade_multi
        //         , new SqlParameter { ParameterName = "Keyword", Value = dataSearch, Size = 100 })
        //         .ToList<Cat_GradeMultiEntity>()
        //         .AsQueryable();
        //    }

        //}
        #endregion
    }
}
