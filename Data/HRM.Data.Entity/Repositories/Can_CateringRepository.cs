﻿using System;
using System.Linq;
using System.Linq.Expressions;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using HRM.Data.Entity;

namespace HRM.Data.Entity.Repositories
{
    public class Can_CateringRepository : CustomBaseRepository<Can_Catering>
    {
        public Can_CateringRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
