using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_ResignReasonRepository : CustomBaseRepository<Cat_ResignReason>
    {
        public Cat_ResignReasonRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        #region HRM8 - 20140720
        //public IQueryable<Cat_ResignReason> Get()
        //{
        //    IQueryable<Cat_ResignReason> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}
        //public IQueryable<Cat_ResignReasonEntity> GetResignReason(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_cat_sp_get_ResignReason);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Cat_ResignReasonEntity>(param.SqlQuery, param.Params).ToList<Cat_ResignReasonEntity>().AsQueryable();
        //    //IQueryable<Cat_LateEarlyRule> result = this.DbSet.ToList().AsQueryable();
        //    //return result;
        //}
        //public IQueryable<Cat_ResignReasonEntity> GetResignReason()
        //{
        //    return this.UnitOfWork.Context.Database
        //       .SqlQuery<Cat_ResignReasonEntity>(ConstantSql.hrm_cat_sp_get_ResignReason).ToList<Cat_ResignReasonEntity>().AsQueryable();
        //    //IQueryable<Cat_LateEarlyRuleEntity> result = this.DbSet.ToList().AsQueryable();
        //    //return result;
        //} 
        #endregion
    }
}
