using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Data.Main.Data.Sql;
using VnResource.Helper.Data;
namespace HRM.Business.HrmSystem.Domain
{
    public class EmployeeServices : BaseService
    {
        //public IQueryable<Sys_User> GetEmployees()
        //{
        //    var unitOfWork = (IUnitOfWork)(new UnitOfWork(new VnrHrmDataContext()));
        //    var repo = new Sys_UserRepository(unitOfWork);
        //    return repo.GetAllUsers();

        //}

        public bool EditEmployees(Sys_UserInfoEntity userInfo)
        {
            var unitOfWork = (IUnitOfWork)(new UnitOfWork(new VnrHrmDataContext()));
            var repo = new Sys_UserRepository(unitOfWork);
            try
            {
                Sys_UserInfo userRoot = new Sys_UserInfo();
                userRoot = userInfo.CopyData<Sys_UserInfo>();
                repo.Edit(userRoot);
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
