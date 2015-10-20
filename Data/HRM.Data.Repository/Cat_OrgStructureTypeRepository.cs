using Devart.Data.Oracle;

using HRM.Data.BaseRepository;

using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_OrgStructureTypeRepository : CustomBaseRepository<Cat_OrgStructureType>
    {
        public Cat_OrgStructureTypeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        #region HRM8 - 20140720
		                //public IQueryable<Cat_OrgStructureType> GetAllCatOrgStructureType(string name, string code)
                //{

                //    var rs = this.DbSet.ToList().Where(i=> i.IsDelete != true).AsQueryable();
                //    if (!string.IsNullOrWhiteSpace(name))
                //    {
                //        return rs = this.DbSet.Where(i => i.OrgStructureTypeName.Contains(name)).ToList().AsQueryable();
                //    }
                //    if (!string.IsNullOrWhiteSpace(code))
                //    {
                //        return rs = this.DbSet.Where(i => i.Code.Contains(code)).ToList().AsQueryable();
                //    }
                //    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(code))
                //    {
                //        return rs = this.DbSet.Where(i => i.OrgStructureTypeName.Contains(name) || i.Code.Contains(code)).ToList().AsQueryable();
                //    }
                //    return rs;
                //}

                ///// <summary>
                ///// Lấy tất cả danh sách dữ liệu show lên Multi
                ///// </summary>
                ///// <returns></returns>
                //public IQueryable<Cat_OrgStructureTypeMultiEntity> GetMulti(object keyword)
                //{
                //    var dataSearch = keyword == null ? System.DBNull.Value : keyword;
                //    if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                //    {
                //        return
                //        this.UnitOfWork.Context.Database.SqlQuery<Cat_OrgStructureTypeMultiEntity>(ConstantSql.hrm_cat_sp_get_OrgStructureType_multi,
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
                //            .ToList<Cat_OrgStructureTypeMultiEntity>().AsQueryable();
                //    }
                //    else
                //    {
                //        return this.UnitOfWork.Context.Database
                //        .SqlQuery<Cat_OrgStructureTypeMultiEntity>(ConstantSql.hrm_cat_sp_get_OrgStructureType_multi
                //        , new SqlParameter { ParameterName = "Keyword", Value = dataSearch, Size = 100 })
                //        .ToList<Cat_OrgStructureTypeMultiEntity>()
                //        .AsQueryable();
                //    }
             
	        #endregion
        
    }
}
