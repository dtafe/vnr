using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_CategoryRepository : CustomBaseRepository<Cat_Category>
    {
        public Cat_CategoryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
