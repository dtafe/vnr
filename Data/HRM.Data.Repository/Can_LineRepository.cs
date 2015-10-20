using System;
using System.Linq;
using System.Linq.Expressions;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using Devart.Data.Oracle;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Can_LineRepository : CustomBaseRepository<Can_Line>
    {
        public Can_LineRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
