using System.Linq;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;
using Devart.Data.Oracle;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_ContractTypeRepository : CustomBaseRepository<Cat_ContractType>
    {
        public Cat_ContractTypeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
