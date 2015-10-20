using HRM.Data.BaseRepository;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;

namespace HRM.Business.Category.Domain
{
    public class Cat_TAMScanReasonMissServices : BaseService
    {
        

        public bool IsDuplication(string code, int id)
        {
            return true;

            // phải dùng store để kiểm tra trùng
            //using (var context = new VnrHrmDataContext())
            //{
            //    var isExisted = false;
            //    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            //    var repo = new Cat_TAMScanReasonMissRepository(unitOfWork);
            //    //var existedData = repo.FindBy(p => p.TAMScanReasonMissCode == code && (id == 0 || p.Id != id)).FirstOrDefault();
            //    //if (existedData != null)
            //    //{
            //    //    isExisted = true;
            //    //}
            //    //return isExisted;
            //}
        }
    }
}
