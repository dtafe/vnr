using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_GradeCfgRepository : CustomBaseRepository<Cat_GradeCfg>
    {
        public Cat_GradeCfgRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
