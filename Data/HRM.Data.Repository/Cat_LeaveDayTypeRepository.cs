using System.Linq;
using HRM.Data.BaseRepository;
//using HRM.Data.Category.Data;
//using HRM.Data.Category.Model;
//using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using Devart.Data.Oracle;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_LeaveDayTypeRepository : CustomBaseRepository<Cat_LeaveDayType>
    {
        public Cat_LeaveDayTypeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
