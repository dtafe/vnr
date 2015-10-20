using HRM.Business.Main.Domain;

namespace HRM.Business.Category.Domain
{
    public class Cat_RegionServices :BaseService
    {

        #region  HRM 8 - 2014-07-20
        //    /// <summary>
        //    /// Lấy tất cả các bản ghi 
        //    /// Gọi GetAllCatRegion() trong Cat_RegionRepository
        //    /// </summary>
        //    /// <returns></returns>
        //    public IQueryable<Cat_Region> GetCatRegion()
        //    {
        //        using (var context = new VnrHrmDataContext())
        //        {
        //            IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //            Cat_RegionRepository repo = new Cat_RegionRepository(unitOfWork);
        //            return repo.Get().Where(i => i.IsDelete == null);
        //        }
        //    }

        //    // <summary>
        //    /// Lấy tất cả các bản ghi 
        //    /// Gọi GetAllCatRegion() trong Cat_RegionRepository
        //    /// </summary>
        //    /// <returns></returns>
        //    public IQueryable<Cat_Region> GetCatRegion(string name, string code)
        //    {
        //        using (var context = new VnrHrmDataContext())
        //        {
        //            IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //            Cat_RegionRepository repo = new Cat_RegionRepository(unitOfWork);
        //            var rs =  repo.GetRegion(name,code);
        //            return rs;
        //        }
        //    }

        //    /// <summary>
        //    /// Lấy bản ghi có id
        //    /// </summary>
        //    /// <returns></returns>
        //    public Cat_Region GetByIdCatRegion(int id)
        //    {
        //        using (var context = new VnrHrmDataContext())
        //        {
        //            IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //            Cat_RegionRepository repo = new Cat_RegionRepository(unitOfWork);
        //            Cat_Region reli = new Cat_Region();
        //            reli = repo.GetById(id);
        //            return reli;
        //        }
        //    }

        //    /// <summary>
        //    /// Thêm bản ghi mới
        //    /// </summary>
        //    /// <param name="catRegion"></param>
        //    /// <returns></returns>
        //    public bool AddCatRegion(Cat_Region catRegion)
        //    {
        //        using (var context = new VnrHrmDataContext())
        //        {
        //            IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //            Cat_RegionRepository repo = new Cat_RegionRepository(unitOfWork);
        //            try
        //            {
        //                repo.Add(catRegion);
        //                repo.SaveChanges();
        //                return true;
        //            }
        //            catch
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    /// <summary>
        //    /// Update bản ghi
        //    /// </summary>
        //    /// <param name="catRegion"></param>
        //    /// <returns></returns>
        //    public bool UpdateCatRegion(Cat_Region catRegion)
        //    {
        //        using (var context = new VnrHrmDataContext())
        //        {
        //            IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //            Cat_RegionRepository repo = new Cat_RegionRepository(unitOfWork);
        //            try
        //            {
        //                repo.Edit(catRegion);
        //                repo.SaveChanges();
        //                return true;
        //            }
        //            catch
        //            {
        //                return false;
        //            }

        //        }
        //    }

        //    /// <summary>
        //    /// Delete 
        //    /// </summary>
        //    /// <param name="catRegionId"></param>
        //    /// <returns></returns>
        //    public bool DeleteCatRegion(int catRegionId)
        //    {
        //        using (var context = new VnrHrmDataContext())
        //        {
        //            IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //            Cat_RegionRepository repo = new Cat_RegionRepository(unitOfWork);
        //            Cat_Region catRegion = new Cat_Region();
        //            catRegion = repo.GetById(catRegionId);
        //            try
        //            {
        //                repo.Remove(catRegion);
        //                repo.SaveChanges();
        //                return true;
        //            }
        //            catch
        //            {
        //                return false;
        //            }

        //        }
        //    }

        //    /// <summary>
        //    /// Delete 
        //    /// </summary>
        //    /// <param name="catRegionEternityId"></param>
        //    /// <returns></returns>
        //    public bool DeleteEternity(int catRegionId)
        //    {
        //        using (var context = new VnrHrmDataContext())
        //        {
        //            IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //            Cat_RegionRepository repo = new Cat_RegionRepository(unitOfWork);
        //            Cat_Region catRegion = new Cat_Region();
        //            catRegion = repo.GetById(catRegionId);
        //            try
        //            {
        //                repo.Delete(catRegion);
        //                repo.SaveChanges();
        //                return true;
        //            }
        //            catch
        //            {
        //                return false;
        //            }

        //        }
        //    }

        //    /// <summary>
        //    /// Lấy toàn bộ data dùng store chỉ có ID, Name
        //    /// </summary>
        //    /// <returns></returns>
        //    public IQueryable<Cat_RegionMultiEntity> GetMulti(string text)
        //    {
        //        using (var context = new VnrHrmDataContext())
        //        {
        //            var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //            var repo = new Cat_RegionRepository(unitOfWork);
        //            return repo.GetMulti(text);
        //        }
        //    } 
        #endregion
    }
}
