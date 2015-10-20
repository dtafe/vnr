using System.Linq;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_DynamicColumnServices : BaseService
    {
        /// <summary>
        /// Lấy toàn bộ data theo TableName
        /// </summary>
        /// <returns></returns>
        public IQueryable<Sys_DynamicColumnEntity> GetByTableName(string tableName)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_DynamicColumnRepository(unitOfWork);
                var dynamicColumns = GetAllUseEntity<Sys_DynamicColumnEntity>(ref status);
                return dynamicColumns.Where(i => i.TableName == tableName).AsQueryable<Sys_DynamicColumnEntity>();
            }
        }

     
    }
}
