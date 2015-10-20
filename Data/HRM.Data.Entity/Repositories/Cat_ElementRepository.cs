using System.Linq;
using HRM.Data.BaseRepository;

using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_ElementRepository : CustomBaseRepository<Cat_Element>
    {
        public Cat_ElementRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        
    }
}
