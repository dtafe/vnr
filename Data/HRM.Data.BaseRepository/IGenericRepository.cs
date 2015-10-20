using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HRM.Data.BaseRepository
{


    public interface IGenericRepository<T> : IDisposable where T : class
    {
        /// <summary> Lấy tất cả dữ liệu </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary> Tìm kiếm một đối tượng theo Id </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(Guid id);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        void Delete(T entity);

        void Edit(T entity);

        void Save();

        DataErrorCode SaveChanges();

        void Remove(T entity);
    }
}
