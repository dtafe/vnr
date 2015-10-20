using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Sys_GroupRepository : CustomBaseRepository<Sys_Group>
    {
        public Sys_GroupRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        //public IQueryable<Sys_Group> GetAll()
        //{
        //    IQueryable<Sys_Group> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        //public IQueryable<Sys_GroupEntity> GetGroups(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_sys_sp_get_groups);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Sys_GroupEntity>(param.SqlQuery, param.Params).ToList<Sys_GroupEntity>().AsQueryable();
        //}
      
    }
}
