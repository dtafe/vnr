using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
  

    public class Sys_BookmarkRepository : CustomBaseRepository<Sys_Bookmark>
    {

        public Sys_BookmarkRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        //public IQueryable<Sys_Bookmark> Get()
        //{
        //    IQueryable<Sys_Bookmark> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        //public IQueryable<Sys_BookmarkEntity> GetBookmark(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_sys_sp_get_Bookmark);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Sys_BookmarkEntity>(param.SqlQuery, param.Params).ToList<Sys_BookmarkEntity>().AsQueryable();
        //}

        //public Sys_BookmarkEntity GetBookmarkByUserID(int UserID)
        //{
        //    return this.UnitOfWork.Context.Database.SqlQuery<Sys_BookmarkEntity>(ConstantSql.hrm_sys_sp_get_BookmarkByUserID, new SqlParameter
        //    {
        //        ParameterName = "UserID",
        //        Value = UserID
        //    }).ToList<Sys_BookmarkEntity>().FirstOrDefault<Sys_BookmarkEntity>();
        //}

    }
}
