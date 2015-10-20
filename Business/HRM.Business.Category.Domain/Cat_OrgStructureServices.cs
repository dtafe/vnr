using HRM.Data.BaseRepository;

using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Category.Models;
using HRM.Data.Main.Data.Sql;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using System;
namespace HRM.Business.Category.Domain
{
    public class Cat_OrgStructureServices : BaseService
    {

        public static Dictionary<Guid?, OrgLevelTypeName> GetFullLinkOrg(List<Guid?> lstOrgIDs, List<OrgTiny> lstOrgAll)
        {

            Dictionary<Guid?, OrgLevelTypeName> Result = new Dictionary<Guid?, OrgLevelTypeName>();

            string E_BRANCH = OrgUnit.E_BRANCH.ToString();
            string E_DEPARTMENT = OrgUnit.E_DEPARTMENT.ToString();
            string E_SECTION = OrgUnit.E_SECTION.ToString();
            string E_TEAM = OrgUnit.E_TEAM.ToString();
            var lstOrgStructureType = new List<Cat_OrgStructureType>().Select(m => new { m.ID, m.OrgStructureTypeCode }).ToList();
            List<OrgTiny> lstOrgByProfile = new List<OrgTiny>();


            lstOrgByProfile = lstOrgAll.Where(m => lstOrgIDs.Contains(m.ID)).ToList();

            Guid Type_BRANCH_ID = lstOrgStructureType.Where(m => m.OrgStructureTypeCode == E_BRANCH).Select(m => m.ID).FirstOrDefault();
            Guid Type_DEPARTMENT_ID = lstOrgStructureType.Where(m => m.OrgStructureTypeCode == E_DEPARTMENT).Select(m => m.ID).FirstOrDefault();
            Guid Type_SECTION_ID = lstOrgStructureType.Where(m => m.OrgStructureTypeCode == E_SECTION).Select(m => m.ID).FirstOrDefault();
            Guid Type_TEAM_ID = lstOrgStructureType.Where(m => m.OrgStructureTypeCode == E_TEAM).Select(m => m.ID).FirstOrDefault();


            foreach (var item in lstOrgByProfile)
            {
                OrgLevelTypeName OrgNameLevel = new OrgLevelTypeName();
                RecursionOrgGetName(lstOrgAll, item.ID, Type_DEPARTMENT_ID, Type_SECTION_ID, Type_BRANCH_ID, Type_TEAM_ID, OrgNameLevel);
                Result.Add(item.ID, OrgNameLevel);
            }
            return Result;

        }

        private static void RecursionOrgGetName(List<OrgTiny> lstOrgAll, Guid? OrgCurrentID, Guid? TypeDeparmentID, Guid? TypeSectionID, Guid? TypeBrandID, Guid? TypeTeamID, OrgLevelTypeName result)
        {

            var currentOrg = lstOrgAll.Where(m => m.ID == OrgCurrentID).FirstOrDefault();
            if (currentOrg == null)
                return;


            if (TypeDeparmentID != null && currentOrg.TypeID == TypeDeparmentID && result.DepartmentName == null)
            {
                result.DepartmentCode = currentOrg.OrgCode;
                result.DepartmentName = currentOrg.OrgName;
            }
            if (TypeSectionID != null && currentOrg.TypeID == TypeSectionID && result.SectionName == null)
            {
                result.SectionCode = currentOrg.OrgCode;
                result.SectionName = currentOrg.OrgName;
            }
            if (TypeBrandID != null && currentOrg.TypeID == TypeBrandID && result.BrandName == null)
            {
                result.BrandCode = currentOrg.OrgCode;
                result.BrandName = currentOrg.OrgName;
            }
            if (TypeTeamID != null && currentOrg.TypeID == TypeTeamID && result.TeamName == null)
            {
                result.TeamCode = currentOrg.OrgCode;
                result.TeamName = currentOrg.OrgName;
            }
            if (currentOrg.ParentID == null)
            {
                return;
            }
            else
            {
                Guid? ParentID = currentOrg.ParentID;
                RecursionOrgGetName(lstOrgAll, ParentID, TypeDeparmentID, TypeSectionID, TypeBrandID, TypeTeamID, result);
            }
        }

        public int GetMaxOrgstructureOrder() {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    var maxOrder = unitOfWork.CreateQueryable<Cat_OrgStructure>(Guid.Empty).Max(m => m.OrderNumber);
                    maxOrder++;
                    return maxOrder;
                }
                catch
                {
                    return 1;
                }

            }        
        }
    
    }
}
