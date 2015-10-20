using HRM.Data.BaseRepository;

using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Data.BaseRepository.Caching;
using System.Collections.Generic;
using HRM.Data.Main.Data.Sql;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
namespace HRM.Business.Category.Domain
{
    public class Cat_CountryServices : BaseService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string COUNTRIES_ALL_KEY = "COUNTRIES_ALL_KEY";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string COUNTRIES_PATTERN_KEY = "Nop.country.";

        #endregion
        #region Fields

        private readonly ICacheManager _cacheManager;

        #endregion

        #region HRM8.20/7/2014

        ///// <summary>
        ///// Gets all countries
        ///// </summary>
        ///// <param name="showHidden">A value indicating whether to show hidden records</param>
        ///// <returns>Country collection</returns>
        //public virtual IList<Cat_Country> GetAllCountries(bool showHidden = false)
        //{
        //    var countries = new List<Cat_Country>();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_CountryRepository repo = new Cat_CountryRepository(unitOfWork);
        //        countries = repo.GetAllCatCountry().Where(i => i.IsDelete == null).ToList();
        //    }
        //    return countries;
        //    string key = string.Format(COUNTRIES_ALL_KEY, showHidden);

        //    return _cacheManager.Get(key, () =>
        //    {

        //        return countries;
        //    });
        //}

        ///// <summary>
        ///// Lấy tất cả các bản ghi có thuộc tính IsDelete là null
        ///// Gọi GetAllCatCountry() trong Cat_CountryRepository
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_Country> GetCatCountry()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_CountryRepository repo = new Cat_CountryRepository(unitOfWork);
        //        return repo.GetAllCatCountry().Where(i => i.IsDelete == null);
        //    }

        //}

        ///// <summary>
        ///// Lấy tất cả các bản ghi có thuộc tính IsDelete là null
        ///// Gọi GetAllCatCountry() trong Cat_CountryRepository
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_Country> GetCatCountry(string name, string ESname, string code)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_CountryRepository repo = new Cat_CountryRepository(unitOfWork);
        //        return repo.GetAllCatCountry(name,ESname,code);
        //    }

        //}

        ///// <summary>
        ///// Tìm kiếm bản ghi có theo tham số id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Cat_Country GetByIdCatCountry(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_CountryRepository repo = new Cat_CountryRepository(unitOfWork);
        //        Cat_Country catCountry = new Cat_Country();
        //        catCountry = repo.GetById(id);
        //        if (catCountry.IsDelete == true) catCountry = null;
        //        return catCountry;
        //    }
        //}

        ///// <summary>
        ///// Thêm bản ghi mới
        ///// </summary>
        ///// <param name="catCountry"></param>
        ///// <returns></returns>
        //public bool AddCatCountry(Cat_Country catCountry)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_CountryRepository repo = new Cat_CountryRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(catCountry);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Update bản ghi
        ///// </summary>
        ///// <param name="catCountry"></param>
        ///// <returns></returns>
        //public bool UpdateCatCountry(Cat_Country catCountry)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_CountryRepository repo = new Cat_CountryRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(catCountry);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Update thuộc tính IsDelete là true
        ///// </summary>
        ///// <param name="catCountryId"></param>
        ///// <returns></returns>
        //public bool DeleteCatCountry(int catCountryId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_CountryRepository repo = new Cat_CountryRepository(unitOfWork);
        //        Cat_Country catCountry = new Cat_Country();
        //        catCountry = repo.GetById(catCountryId);
        //        try
        //        {
        //            repo.Remove(catCountry);
        //            repo.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }
        //}

        ///// <summary>
        ///// Lấy toàn bộ data dùng store chỉ có ID, Name
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_CountryMultiEntity> GetMulti(string text)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_CountryRepository(unitOfWork);
        //        return repo.GetMulti(text);
        //    }
        //}
        #endregion


    }
}
