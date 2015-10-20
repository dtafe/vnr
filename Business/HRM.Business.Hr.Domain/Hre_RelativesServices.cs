using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Data.BaseRepository;
using System;
using System.Linq;
namespace HRM.Business.Hr.Domain
{
    public class Hre_RelativesServices : BaseService
    {
        public List<Hre_RelativesEntity> GetRelativeByProfileId(Guid ProfileId)
        {
            using(var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var lstRelativeByProfileid = unitOfWork.CreateQueryable<Hre_Relatives>(Guid.Empty, s => s.ProfileID == ProfileId).Select(p => new Hre_RelativesEntity 
                {
                    ProfileID=p.ProfileID,
                    RelativeName=p.RelativeName,
                    YearOfBirth=p.YearOfBirth
                }).ToList();
                return lstRelativeByProfileid;
            }
        }
    }

}
