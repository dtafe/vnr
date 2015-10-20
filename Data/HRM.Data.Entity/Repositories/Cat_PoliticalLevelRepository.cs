using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Cat_PoliticalLevelRepository : CustomBaseRepository<Cat_PoliticalLevel>
    {
        public Cat_PoliticalLevelRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
