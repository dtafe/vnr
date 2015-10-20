using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Data.Attendance.Model;
using System.Linq;
using HRM.Data.Attendance.Sql;

namespace HRM.Business.Hr.Domain
{
    public class Cat_DayOffServices
    {
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public IQueryable<Cat_DayOff> GetAll()
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_DayOffRepository repo = new Cat_DayOffRepository(unitOfWork);
                return repo.GetAllCatDayOff().Where(i => i.IsDelete == null);

            }
        }

        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_DayOff GetById(int id)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_DayOffRepository repo = new Cat_DayOffRepository(unitOfWork);
                var catdayoff= repo.GetById(id);
                if (catdayoff.IsDelete == true) catdayoff = null;
                return catdayoff;
            }
        }

        /// <summary>
        /// Thêm mới một record
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public bool Add(Cat_DayOff cat)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_DayOffRepository repo = new Cat_DayOffRepository(unitOfWork);
                try
                {
                    repo.Add(cat);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Edit một record
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public bool Edit(Cat_DayOff cat)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_DayOffRepository repo = new Cat_DayOffRepository(unitOfWork);
                try
                {
                    repo.Edit(cat);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// Remove 1 record là chuyển trạng thái IsDelete=true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Remove(int id)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_DayOffRepository repo = new Cat_DayOffRepository(unitOfWork);
                var cat = repo.GetById(id);
                try
                {
                    repo.Remove(cat);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// Delete 1 record là xóa luôn record khỏi database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_DayOffRepository repo = new Cat_DayOffRepository(unitOfWork);
                var cat = repo.GetById(id);
                try
                {
                    repo.Delete(cat);
                    repo.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
    }
}
