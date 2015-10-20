using HRM.Data.BaseRepository;
using HRM.Data.Entity;


namespace HRM.Data.Entity.Repositories
{
    public class Cat_ReligionRepository : CustomBaseRepository<Cat_Religion>
    {
        public Cat_ReligionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #region MyRegion
        //public IQueryable<Cat_Religion> GetAllCatReligion()
        //{
        //    var result = this.DbSet.ToList().AsQueryable();

        //    return result;
        //}
        //public IQueryable<Cat_Religion> GetAllCatReligion(string name, string code)
        //{
        //    var result = this.DbSet.ToList().AsQueryable();
        //    if (!string.IsNullOrWhiteSpace(name))
        //    {
        //        return result = this.DbSet.Where(i => i.ReligionName.Contains(name)).ToList().AsQueryable();
        //    }
        //    if (!string.IsNullOrWhiteSpace(code))
        //    {
        //        return result = this.DbSet.Where(i => i.Code.Contains(code)).ToList().AsQueryable();
        //    }
        //    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(code))
        //    {
        //        return result = this.DbSet.Where(i => i.ReligionName.Contains(name) && i.Code.Contains(code)).ToList().AsQueryable();
        //    }
        //    return result;
        //} 
        #endregion
    }
}
