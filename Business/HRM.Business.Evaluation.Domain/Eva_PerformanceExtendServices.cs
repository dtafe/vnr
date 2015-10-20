using System;
using System.Linq.Dynamic;
using HRM.Business.Evaluation.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using HRM.Business.Hr.Models;


namespace HRM.Business.Evaluation.Domain
{
    public class Eva_PerformanceExtendServices : BaseService
    {

        public Eva_PerformanceExtendEntity GetPerformanceExtendByPerID(string ID,string userLogin)
        {
            Guid perID = Guid.Empty;
            Guid.TryParse( ID, out perID);
            BaseService service = new BaseService();
            string status = string.Empty;

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoEva_Performance = new CustomBaseRepository<Eva_Performance>(unitOfWork);
                var repoEva_PerformanceExtend = new CustomBaseRepository<Eva_PerformanceExtend>(unitOfWork);

                var performance = repoEva_Performance.GetAll().Where(s => s.ID == perID).FirstOrDefault();
                var extend = repoEva_PerformanceExtend.GetAll().Where(s => s.ID == performance.PerformanceExtendID).FirstOrDefault();

                var result = extend.Copy<Eva_PerformanceExtendEntity>();
                string proID = Common.DotNetToOracle(performance.ProfileID.Value.ToString());
                var profile = service.GetData<Hre_ProfileEntity>(proID, ConstantSql.hrm_hr_sp_get_ProfileById,userLogin, ref status).FirstOrDefault();

                if (profile != null && result == null)
                {
                    result = new Eva_PerformanceExtendEntity();
                }
                
                result.ProfileName = profile.ProfileName ?? string.Empty;
                result.CodeEmp = profile.CodeEmp ?? string.Empty;
                result.PositionName = profile.PositionName ?? string.Empty;
                result.JobTitleName = profile.JobTitleName ?? string.Empty;
                result.DateOfBirth = profile.DateOfBirth;
                result.DateHire = profile.DateHire;
                result.PayrollGroupName = profile.PayrollGroupName ?? string.Empty;
                result.SupervisorName = profile.SupervisorName ?? string.Empty;
                result.HighSupervisorName = profile.HighSupervisorName ?? string.Empty;
                result.WorkPlaceName = profile.WorkPlaceName ?? string.Empty;
                result.TCountryName = profile.TCountryName ?? string.Empty;
                result.TProvinceName = profile.TProvinceName ?? string.Empty;
                result.Channel = profile.Channel ?? string.Empty;
                result.Region = profile.Region ?? string.Empty;
                result.Area = profile.Area ?? string.Empty;
                result.DateOfEffect = profile.DateOfEffect;

                return result;

            }
        }
    }
}
        