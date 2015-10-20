using HRM.Data.BaseRepository;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Eva_LevelRepository : CustomBaseRepository<Eva_Level>
    {
        public Eva_LevelRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
