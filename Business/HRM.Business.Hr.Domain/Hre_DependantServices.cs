using HRM.Data.BaseRepository;
using System.Linq;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System;

namespace HRM.Business.Hr.Domain
{
    public class Hre_DependantServices : BaseService
    {
        public void AddOrUpdateDependant(Hre_RelativesEntity model)
        {
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_DependantRepository(unitOfWork);
                Hre_Dependant Dependant = null;
                // kiểm tra theo profileID và Tên người thân để tránh trùng
                var lstDependant = repo.FindBy(x => x.ProfileID == model.ProfileID && x.DependantName.Equals(model.RelativeName)).ToList();
                if (lstDependant != null && lstDependant.Count != 0)
                {
                    Dependant = lstDependant.FirstOrDefault();
                }
                if (Dependant == null)
                {
                    Dependant = new Hre_Dependant();
                    Dependant.ProfileID = model.ProfileID;
                    Dependant.DependantName = model.RelativeName;
                    Dependant.IDNo = model.IDNo;

                    if(model.YearOfBirth != null)
                    {
                        try
                        {
                            string yearparse = model.YearOfBirth.Substring(model.YearOfBirth.Length - 4, 4);
                            int Year = int.Parse(yearparse);
                            Dependant.DateOfBirth = new DateTime(Year, 1, 1);
                        }
                        catch
                        {

                        }
                    }
                   
                    Dependant.Gender = model.Gender;
                    Dependant.RelationID = model.RelativeTypeID;
                    Dependant.MonthOfEffect = model.MonthOfEffect;
                    Dependant.MonthOfExpiry = model.MonthOfExpiry;

                    Dependant.DeclareFile = model.DeclareFile;
                    Dependant.BirthCertificate = model.BirthCertificate;
                    Dependant.HouseHold = model.HouseHold;
                    Dependant.MarriageLicenses = model.MarriageLicenses;
                    Dependant.StudyingSchools = model.StudyingSchools;
                    Dependant.LaborDisabled = model.LaborDisabled;
                    Dependant.NurturingObligations = model.NurturingObligations;
                    Dependant.IDCardNo = model.IDCardNo;


                    repo.Add(Dependant);
                    repo.SaveChanges();

                }
                else
                {
                    Dependant.IDNo = model.IDNo;
                    try
                    {
                        int Year = int.Parse(model.YearOfBirth);
                        Dependant.DateOfBirth = new DateTime(Year, 1, 1);
                    }
                    catch
                    {

                    }
                    Dependant.Gender = model.Gender;
                    Dependant.RelationID = model.RelativeTypeID;
                    Dependant.MonthOfEffect = model.MonthOfEffect;
                    Dependant.MonthOfExpiry = model.MonthOfExpiry;
                    repo.SaveChanges();
                }



            }
        }
    }
}
