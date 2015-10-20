using HRM.Data.BaseRepository;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Data.Attendance.Model;
using System.Linq;
using HRM.Data.Attendance.Sql;

namespace HRM.Business.Hr.Domain
{
    public class Cat_ShiftServices
    {
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        public IQueryable<Cat_Shift> GetAll()
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
                return repo.GetAllCatShift().Where(i => i.IsDelete == null);

            }
        }

        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cat_Shift GetById(int id)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
                var catshift= repo.GetById(id);
                if (catshift.IsDelete == true) catshift = null;
                return catshift;
            }
        }

        /// <summary>
        /// Thêm mới một record
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public bool Add(Cat_Shift cat)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
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
        public bool Edit(Cat_Shift cat)
        {
            using (var context = new VnrAttendanceDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
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
                Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
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
                Cat_ShiftRepository repo = new Cat_ShiftRepository(unitOfWork);
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
