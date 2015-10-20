using HRM.Data.BaseRepository;
using HRM.Data.Entity;
namespace HRM.Data.Entity.Repositories
{

    public class Lau_LockerRepository : CustomBaseRepository<LMS_LockerLMS>
    {
        public Lau_LockerRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        //public IQueryable<Lau_LockerEntity> GetLocker(ListQueryModel model)
        //{
        //    ParamaterModle param = ListUtility.ParseParam(model, ConstantSql.hrm_lau_sp_get_Locker);
        //    return this.UnitOfWork.Context.Database.SqlQuery<Lau_LockerEntity>(param.SqlQuery, param.Params).ToList<Lau_LockerEntity>().AsQueryable();
        //}
    }
}
