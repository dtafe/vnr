using HRM.Business.Main.Domain;

namespace HRM.Business.Category.Domain
{
    public class Cat_ProvinceServices:BaseService
    {

        #region HRM8.20/7/2014
        ///// <summary>
        ///// [hien.pham] - 2014/05/07
        ///// Lấy danh sách tất cả tỉnh thành
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_ProvinceEntity> GetProvinces()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ProvinceRepository(unitOfWork);
        //        return repo.GetProvinces();
        //    }
        //}

        ///// <summary>
        ///// [hien.pham] - 2014/05/07
        ///// Lấy danh sách tất cả tỉnh thành
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_ProvinceEntity> GetProvinces(ListQueryModel model)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ProvinceRepository(unitOfWork);
        //        return repo.GetProvinces(model);
        //    }
        //}

        ///// <summary>
        ///// [hien.pham] - 2014/05/07
        ///// Lấy tất cả các bản ghi 
        ///// Gọi GetAllCatProvince() trong Cat_ProvinceRepository
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_Province> GetCatProvince()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ProvinceRepository repo = new Cat_ProvinceRepository(unitOfWork);
        //        return repo.GetAllCatProvince().Where(i => i.IsDelete == null);
        //    }
        //}

        ///// <summary>
        ///// [hien.pham] - 2014/05/07
        ///// Lấy bản ghi có id
        ///// </summary>
        ///// <returns></returns>
        //public Cat_Province GetByIdCatProvince(int id)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ProvinceRepository repo = new Cat_ProvinceRepository(unitOfWork);
        //        Cat_Province reli = new Cat_Province();
        //        reli = repo.GetById(id);
        //        return reli;
        //    }
        //}

        ///// <summary>
        ///// Thêm bản ghi mới
        ///// </summary>
        ///// <param name="catProvince"></param>
        ///// <returns></returns>
        //public bool AddCatProvince(Cat_Province catProvince)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ProvinceRepository repo = new Cat_ProvinceRepository(unitOfWork);
        //        try
        //        {
        //            repo.Add(catProvince);
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
        ///// <param name="catProvince"></param>
        ///// <returns></returns>
        //public bool UpdateCatProvince(Cat_Province catProvince)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ProvinceRepository repo = new Cat_ProvinceRepository(unitOfWork);
        //        try
        //        {
        //            repo.Edit(catProvince);
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
        ///// Delete 
        ///// </summary>
        ///// <param name="catProvinceId"></param>
        ///// <returns></returns>
        //public bool DeleteCatProvince(int catProvinceId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ProvinceRepository repo = new Cat_ProvinceRepository(unitOfWork);
        //        Cat_Province catProvince = new Cat_Province();
        //        catProvince = repo.GetById(catProvinceId);
        //        try
        //        {
        //            repo.Remove(catProvince);
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
        ///// Delete 
        ///// </summary>
        ///// <param name="catProvinceEternityId"></param>
        ///// <returns></returns>
        //public bool DeleteEternity(int catProvinceId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        Cat_ProvinceRepository repo = new Cat_ProvinceRepository(unitOfWork);
        //        Cat_Province catProvince = new Cat_Province();
        //        catProvince = repo.GetById(catProvinceId);
        //        try
        //        {
        //            repo.Delete(catProvince);
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
        ///// Lấy danh sách tất cả tỉnh thành bởi countryId
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_Province> GetProvinceByCountryId(int countryId)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ProvinceRepository(unitOfWork);
        //        return repo.GetProvinceByCountryId(countryId);
        //    }
        //}

        ///// <summary>
        ///// Lấy toàn bộ data dùng store chỉ có ID, Name
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Cat_ProvinceMultiEntity> GetMulti(string text)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ProvinceRepository(unitOfWork);
        //        return repo.GetMulti(text);
        //    }
        //}
        #endregion


    }
}
