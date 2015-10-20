using System;
using System.Collections.Generic;
using System.Xml;
using HRM.Business.Evaluation.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System.Linq;
using HRM.Presentation.Evaluation.Models;
using System.Data;
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Attendance;
using HRM.Business.Attendance.Domain;
using HRM.Business.Attendance.Models;
using HRM.Business.Payroll.Domain;
using HRM.Business.Payroll.Models;
using VnResource.Helper.Data;
namespace HRM.Business.Evaluation.Domain
{
    public class Eva_EvalutionDataServices : BaseService
    {

        public string SaveEvalutionData(int year, Guid? TimesGetDataID, string orgStructureID, DateTime? _dateStart, DateTime? _dateEnd,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var evaServices = new Eva_ReportServices();
                List<Eva_EvalutionDataEntity> lstEvalutionDataEntity = new List<Eva_EvalutionDataEntity>();
                lstEvalutionDataEntity = evaServices.SummaryEvalutionData(year, TimesGetDataID, orgStructureID, _dateStart, _dateEnd,userLogin);
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoEva_EvalutionData = new CustomBaseRepository<Eva_EvalutionData>(unitOfWork);
                List<Eva_EvalutionData> lstEvalutionData = new List<Eva_EvalutionData>();
                lstEvalutionData = lstEvalutionDataEntity.Translate<Eva_EvalutionData>();
                if (lstEvalutionData != null)
                {
                    int _total = lstEvalutionData.Count;
                    int _totalPage = _total / 200 + 1;
                    int _pageSize = 200;
                    for (int _page = 1; _page <= _totalPage; _page++)
                    {
                        int _skip = _pageSize * (_page - 1);
                        var _listCurrenPage = lstEvalutionData.Skip(_skip).Take(_pageSize).ToList();
                        foreach (var item in _listCurrenPage)
                        {
                            repoEva_EvalutionData.Add(item);
                        }
                        unitOfWork.SaveChanges();
                    }
                }
                return "SaveSuccess";
                //    #endregion
            }
        }  

    }
}
