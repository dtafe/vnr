using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Devart.Data.Oracle;
using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data;
using HRM.Business.Attendance.Models;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using System.Data.Entity;


namespace HRM.Data.Entity.Repositories
{

    public class Att_AnnualDetailRepository : CustomBaseRepository<Att_AnnualDetail>
    {
        public Att_AnnualDetailRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
