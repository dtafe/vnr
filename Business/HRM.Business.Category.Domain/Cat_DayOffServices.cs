using System.Collections.Generic;
using HRM.Business.Category.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Main.Data.Sql;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using VnResource.Helper.Data;

namespace HRM.Business.Category.Domain
{
    public class Cat_DayOffServices: BaseService
    {
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public List<Cat_DayOffEntity> GetAll()
        {
            using (var context = new VnrHrmDataContext())
            {
                List<Cat_DayOffEntity> lstSource = new List<Cat_DayOffEntity>();
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_DayOffRepository repo = new Cat_DayOffRepository(unitOfWork);
                var data = repo.FindBy(s => s.IsDelete == null).ToList();
                if (data.Count > 0)
                    lstSource = data.Translate<Cat_DayOffEntity>();
                return lstSource;
            }
        }
    }
}
