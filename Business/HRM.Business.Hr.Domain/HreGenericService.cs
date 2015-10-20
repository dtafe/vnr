using HRM.Business.Hr.Models;
using HRM.Data.BaseRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Hr.Domain
{
    public class HreGenericService<T> where T : class
    {
        protected IGenericRepository<T> _repository = null;

        public IQueryable<T> Get(ListQueryModel model)
        {
            return _repository.GetAll().FilterList<T>(model.Filters).SortList<T>(model.Sorts);
        }
    }
}
