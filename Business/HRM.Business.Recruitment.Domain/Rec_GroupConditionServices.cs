using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Domain
{
    public class Rec_GroupConditionServices : BaseService
    {
        public string AddConditionIntoGroupCondition(Guid GroupConditionID, string ConditionIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_GroupConditionRepository(unitOfWork);
                Rec_GroupCondition GroupCondition = repo.GetById(GroupConditionID);
                if (GroupCondition == null)
                    return null;
                List<Guid> lstCondition = new List<Guid>();
                List<Guid> lstAddCondition = new List<Guid>();
                lstAddCondition = ConditionIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                if (!string.IsNullOrEmpty(GroupCondition.JobConditionIDs))
                {
                    lstCondition = GroupCondition.JobConditionIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                }
                lstCondition.AddRange(lstAddCondition);
                lstCondition = lstCondition.Distinct().ToList();
                GroupCondition.JobConditionIDs = string.Join(",", lstCondition);
                repo.SaveChanges();
                return GroupCondition.JobConditionIDs;
            }

        }


        public string DeleteConditionInGroupCondition(Guid GroupConditionID, string ConditionIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Rec_GroupConditionRepository(unitOfWork);
                var ilConditionIDs = ConditionIDs.Split(',');
                List<Guid> lsConditionIDs = new List<Guid>();
                lsConditionIDs = ConditionIDs.Split(',').Select(x => Guid.Parse(x)).ToList();

                Rec_GroupCondition GroupCondition = repo.GetById(GroupConditionID);
                if (GroupCondition == null)
                    return null;
                List<Guid> lstCondition = new List<Guid>();
                lstCondition = GroupCondition.JobConditionIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                if (lstCondition != null && lstCondition.Count != 0)
                {
                    lstCondition = lstCondition.Where(x => !lsConditionIDs.Contains(x)).ToList();
                    GroupCondition.JobConditionIDs = string.Join(",", lstCondition);
                    repo.SaveChanges();
                    return GroupCondition.JobConditionIDs;
                }
            }
            return null;
        }
    }
}
