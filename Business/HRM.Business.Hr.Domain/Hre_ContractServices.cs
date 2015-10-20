using HRM.Data.BaseRepository;
using System.Linq;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using System;
using HRM.Business.Category.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Data.Entity.Repositories;
using System.Data;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using System;
using Microsoft.Ajax.Utilities;

namespace HRM.Business.Hr.Domain
{
    public class Hre_ContractServices : BaseService
    {
        public Hre_ContractEntity SetNewCodeContract(Hre_ContractEntity contractEntity, string strIdConstract, string userLogin)
        {
            if (contractEntity != null)
            {
                var service = new BaseService();
                var status = string.Empty;
                var objContractType1 = service.GetFirstData<Cat_ContractTypeEntity>(contractEntity.ContractTypeID, ConstantSql.hrm_cat_sp_get_ContractTypeById, userLogin, ref status);
                var objProfile1 = service.GetFirstData<Hre_ProfileEntity>(contractEntity.ProfileID, ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status);
                if (objContractType1 != null)
                {
                    var contractType = (Cat_ContractTypeEntity)objContractType1;
                    var profileEntity = (Hre_ProfileEntity)objProfile1;
                    if (!string.IsNullOrEmpty(contractType.Type))
                    {
                        var objSysConfig = service.GetFirstData<Sys_AllSettingEntity>(AppConfig.HRM_HRE_GENERATE_CODE_CONTRACT.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey, userLogin, ref status);
                        if (objSysConfig != null)
                        {
                            var sysConfig = (Sys_AllSettingEntity)objSysConfig;
                            if (sysConfig != null && !string.IsNullOrEmpty(sysConfig.Value1))
                            {
                                ElementFormula elementContactType = new ElementFormula("ContactType", contractType.Type, 0);
                                ElementFormula elementCodeEmp = new ElementFormula("CodeEmp", profileEntity.CodeEmp, 0);
                                ElementFormula elementOrdinal = new ElementFormula("Ordinal", 1, 0);

                                var result = FormulaHelper.ParseFormula(sysConfig.Value1, new List<ElementFormula>() { elementContactType, elementCodeEmp });
                                if (result != null && string.IsNullOrEmpty(result.ErrorMessage))
                                {
                                    var value = result.Value;
                                    if (value != null)
                                    {
                                        string newCode = value.ToString();
                                        if (newCode.EndsWith("Ordinal"))
                                        {
                                            var strNewCode = newCode.Substring(0, newCode.Length - 7);
                                            if (!string.IsNullOrEmpty(strIdConstract))
                                            {
                                                var listContractType = service.GetData<Cat_ContractTypeEntity>(Common.DotNetToOracle(strIdConstract), ConstantSql.hrm_cat_sp_get_ContractTypeByIds, userLogin, ref status);
                                                if (listContractType != null)
                                                {
                                                    var listContractTypeById = listContractType.Where(d => d.Type == contractType.Type).FirstOrDefault();
                                                    if (listContractTypeById != null)
                                                    {
                                                        var listId = strIdConstract.Split(',').ToList();
                                                        var count = listId.Where(s => listContractTypeById.ID.ToString() == s).Select(d => d == listContractTypeById.ToString()).Count();

                                                        //var count = listContractTypeById.Count();
                                                        newCode = strNewCode + "-" + (count + 1);
                                                    }
                                                    else
                                                    {
                                                        
                                                        newCode = strNewCode + "-" + "1";
                                                    }
                                                }
                                                else
                                                {
                                                    newCode = strNewCode + "-" + 1;
                                                }
                                            }
                                            else
                                            {
                                                newCode = strNewCode + "-" + 1;
                                            }

                                        }
                                        contractEntity.ContractNo = newCode;
                                    }
                                }
                            }
                        }
                    }
                }
                contractEntity.ActionStatus = status;
            }
            return contractEntity;
        }

        public void SubmitStatus(string selectedIds, string status)
        {
            using (var context = new VnrHrmDataContext())
            {
                string statuss = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Hre_ContractRepository(unitOfWork);
                List<Guid> lstContractIds = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                var lstContracts = repo.FindBy(m => m.ID != null && lstContractIds.Contains(m.ID)).ToList();
                foreach (var Contract in lstContracts)
                {
                    Contract.Status = status;
                }
                repo.SaveChanges();
            }
        }

        public DataTable GetDataContractSchema() 
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = new DataTable("Hre_ContractEntity");
                tb.Columns.Add(Hre_ContractEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.ProfileName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.E_DEPARTMENT);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.E_DIVISION);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.E_SECTION);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.E_TEAM);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.E_UNIT);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.EvaluationResult);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.Salary);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.CurrencyName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.InsuranceAmount);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.CurenncyInsName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.SalaryClassTypeName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.RankRateName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateEnd);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateStart);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateSigned);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateOfContractEva);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.ContractEvaType);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.ContractNo);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.ContractResult);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.ContractTypeName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.Gender);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.JobTitleName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.PositionName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.ProfileSingName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.WorkPlaceName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.StatusView);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.StatusEvaluation);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.Allowance);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.Allowance1);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.Allowance2);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.Allowance3);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.Allowance4);

                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateStartOld);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateStartOld_Day);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateStartOld_Month);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateStartOld_Year);

                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateEndOld);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateEndOld_Day);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateEndOld_Month);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateEndOld_Year);

                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateSignedOld);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateSignedOld_Day);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateSignedOld_Month);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateSignedOld_Year);

                tb.Columns.Add(Hre_ContractEntity.FieldNames.PAddress);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.PDistrictName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.PProvinceName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.PCountryName);

                tb.Columns.Add(Hre_ContractEntity.FieldNames.TenDem);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.FristName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.LastName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.OrgStructureName);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.OrgStructureID);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.TypeOfPassView);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.DateEndNextContract);
                tb.Columns.Add(Hre_ContractEntity.FieldNames.NextContractTypeName);
                return tb;
            }
        }

        public DataTable GetDataContract(List<Hre_ContractEntity> lstContract, string UserLogin) 
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = GetDataContractSchema();
                var status = string.Empty;
                var objContractExtend = new List<object>();
                objContractExtend.AddRange(new object[10]);
                objContractExtend[8] = 1;
                objContractExtend[9] = int.MaxValue -1;
                var lstContractExtend = GetData<Hre_ContractExtendEntity>(objContractExtend, ConstantSql.hrm_hr_sp_get_ContractExtendList, UserLogin, ref status).ToList();

                if (lstContract.Count > 0)
                {
                    foreach (var item in lstContract)
                    {
                        DataRow dr = tb.NewRow();

                        var lstContractExtendByContractID = lstContractExtend.Where(s => s.ContractID != null && s.ContractID.Value == item.ID).ToList();

                        dr[Hre_ContractEntity.FieldNames.CodeEmp] = item.CodeEmp;
                        dr[Hre_ContractEntity.FieldNames.ProfileName] = item.ProfileName;
                        dr[Hre_ContractEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                        dr[Hre_ContractEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                        dr[Hre_ContractEntity.FieldNames.E_SECTION] = item.E_SECTION;
                        dr[Hre_ContractEntity.FieldNames.E_TEAM] = item.E_TEAM;
                        dr[Hre_ContractEntity.FieldNames.E_UNIT] = item.E_UNIT;
                        dr[Hre_ContractEntity.FieldNames.EvaluationResult] = item.EvaluationResult;
                        dr[Hre_ContractEntity.FieldNames.Salary] = item.Salary;
                        dr[Hre_ContractEntity.FieldNames.CurrencyName] = item.CurrencySalName;
                        dr[Hre_ContractEntity.FieldNames.InsuranceAmount] = item.InsuranceAmount;
                        dr[Hre_ContractEntity.FieldNames.CurenncyInsName] = item.InsuranceAmount;
                        dr[Hre_ContractEntity.FieldNames.SalaryClassTypeName] = item.SalaryClassTypeName;
                        dr[Hre_ContractEntity.FieldNames.RankRateName] = item.RankRateName;
                        dr[Hre_ContractEntity.FieldNames.DateEnd] = item.DateEnd;
                        dr[Hre_ContractEntity.FieldNames.DateStart] = item.DateStart.ToShortDateString();
                        dr[Hre_ContractEntity.FieldNames.DateSigned] = item.DateSigned;
                        dr[Hre_ContractEntity.FieldNames.DateOfContractEva] = item.DateOfContractEva;
                        dr[Hre_ContractEntity.FieldNames.ContractEvaType] = item.ContractEvaType;
                        dr[Hre_ContractEntity.FieldNames.ContractNo] = item.ContractNo;
                        dr[Hre_ContractEntity.FieldNames.ContractResult] = item.ContractResult;
                        dr[Hre_ContractEntity.FieldNames.ContractTypeName] = item.ContractTypeName;
                        dr[Hre_ContractEntity.FieldNames.Gender] = item.Gender;
                        dr[Hre_ContractEntity.FieldNames.JobTitleName] = item.JobTitleName;
                        dr[Hre_ContractEntity.FieldNames.PositionName] = item.PositionName;
                        dr[Hre_ContractEntity.FieldNames.ProfileSingName] = item.ProfileSingName;
                        dr[Hre_ContractEntity.FieldNames.StatusView] = item.StatusView;
                        dr[Hre_ContractEntity.FieldNames.StatusEvaluation] = item.StatusEvaluation;
                        dr[Hre_ContractEntity.FieldNames.Allowance] = item.Allowance;
                        dr[Hre_ContractEntity.FieldNames.Allowance1] = item.Allowance1;
                        dr[Hre_ContractEntity.FieldNames.Allowance2] = item.Allowance2;
                        dr[Hre_ContractEntity.FieldNames.Allowance3] = item.Allowance3;
                        dr[Hre_ContractEntity.FieldNames.Allowance4] = item.Allowance4;

                        dr[Hre_ContractEntity.FieldNames.DateStartOld] = item.DateStartOld;
                        dr[Hre_ContractEntity.FieldNames.DateStartOld_Day] = item.DateStartOld_Day;
                        dr[Hre_ContractEntity.FieldNames.DateStartOld_Month] = item.DateStartOld_Month;
                        dr[Hre_ContractEntity.FieldNames.DateStartOld_Year] = item.DateStartOld_Year;

                        dr[Hre_ContractEntity.FieldNames.DateEndOld] = item.DateEndOld;
                        dr[Hre_ContractEntity.FieldNames.DateEndOld_Day] = item.DateEndOld_Day;
                        dr[Hre_ContractEntity.FieldNames.DateEndOld_Month] = item.DateEndOld_Month;
                        dr[Hre_ContractEntity.FieldNames.DateEndOld_Year] = item.DateEndOld_Year;

                        dr[Hre_ContractEntity.FieldNames.DateSignedOld] = item.DateSignedOld;
                        dr[Hre_ContractEntity.FieldNames.DateSignedOld_Day] = item.DateSignedOld_Day;
                        dr[Hre_ContractEntity.FieldNames.DateSignedOld_Month] = item.DateSignedOld_Month;
                        dr[Hre_ContractEntity.FieldNames.DateSignedOld_Year] = item.DateSignedOld_Year;

                        dr[Hre_ContractEntity.FieldNames.TenDem] = item.TenDem;
                        dr[Hre_ContractEntity.FieldNames.FristName] = item.FristName;
                        dr[Hre_ContractEntity.FieldNames.LastName] = item.LastName;
                        dr[Hre_ContractEntity.FieldNames.OrgStructureName] = item.OrgStructureName;
                        dr[Hre_ContractEntity.FieldNames.NextContractTypeName] = item.NextContractTypeName;
                        dr[Hre_ContractEntity.FieldNames.TypeOfPassView] = item.TypeOfPassView;
                        dr[Hre_ContractEntity.FieldNames.DateEndNextContract] = item.DateEndNextContract;


                        if (lstContractExtendByContractID.Count > 0)
                        {
                            var count = 0;
                            foreach (var contractExtend in lstContractExtendByContractID)
                            {
                                count++;
                                var annexCodeTitle = "AnnexCode" + count;
                                var dateStartTitle = "DateStart" + count;
                                var dateEndTitle = "DateEnd" + count;
                              
                                if (!tb.Columns.Contains(annexCodeTitle))
                                {
                                    tb.Columns.Add(annexCodeTitle);
                                }

                                if (!tb.Columns.Contains(dateStartTitle))
                                {
                                    tb.Columns.Add(dateStartTitle, typeof(DateTime));
                                }

                                if (!tb.Columns.Contains(dateEndTitle))
                                {
                                    tb.Columns.Add(dateEndTitle,typeof(DateTime));
                                }

                                if (tb.Columns.Contains(annexCodeTitle))
                                {
                                    dr[annexCodeTitle] = contractExtend.AnnexCode;
                                }
                                if (tb.Columns.Contains(dateStartTitle))
                                {
                                    if (contractExtend.DateStart != null)
                                    {
                                        dr[dateStartTitle] = contractExtend.DateStart;
                                    }
                                }
                                if (tb.Columns.Contains(dateEndTitle))
                                {
                                    if (contractExtend.DateEnd != null)
                                    {
                                        dr[dateEndTitle] = contractExtend.DateEnd;
                                    }
                                }
                            }
                        }
                        tb.Rows.Add(dr);
                    }
                }
                return tb;
            }
        }


        public DataTable GetDataContractByProfileIDSchema() 
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = new DataTable("Hre_ReportProfileComeBackEntity");
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.ProfileName);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.E_UNIT);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.E_DIVISION);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.E_DEPARTMENT);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.E_TEAM);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.E_SECTION);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.PositionName);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.DateOfBirth,typeof(DateTime));
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.Gender);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.PlaceOfBirth);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.IDPlaceOfIssue);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.PAddress);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.HomePhone);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.Cellphone);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.IDNo);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.DateComeBack, typeof(DateTime));
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.SalaryRankName);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.DateHire, typeof(DateTime));
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.DateQuit, typeof(DateTime));
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.TypeOfStop);
                tb.Columns.Add(Hre_ReportProfileComeBackEntity.FieldNames.ResignReasonName);

                return tb;

            
            }
        }

        public DataTable GetDataContractByProfileID(List<Hre_ReportProfileComeBackEntity> lstProfile, string UserLogin) 
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = GetDataContractByProfileIDSchema();
                var status = string.Empty;
                var objContract = new List<object>();
                objContract.AddRange(new object[21]);
                objContract[19] = 1;
                objContract[20] = int.MaxValue - 1;
                var lstContract = GetData<Hre_ContractEntity>(objContract, ConstantSql.hrm_hr_sp_get_Contract, UserLogin, ref status).ToList();

                var objContractExtend = new List<object>();
                objContractExtend.AddRange(new object[10]);
                objContractExtend[8] = 1;
                objContractExtend[9] = int.MaxValue - 1;
                var lstContractExtend = GetData<Hre_ContractExtendEntity>(objContractExtend, ConstantSql.hrm_hr_sp_get_ContractExtendList, UserLogin, ref status).ToList();

                foreach (var item in lstProfile)
                {
                    DataRow dr = tb.NewRow();
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.ProfileName] = item.ProfileName;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.E_UNIT] = item.E_UNIT;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.E_TEAM] = item.E_TEAM;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.E_SECTION] = item.E_SECTION;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.PositionName] = item.PositionName;
                    if (item.DateOfBirth != null)
                    {
                        dr[Hre_ReportProfileComeBackEntity.FieldNames.DateOfBirth] = item.DateOfBirth;
                    }
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.Gender] = item.Gender;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.PlaceOfBirth] = item.PlaceOfBirth;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.IDPlaceOfIssue] = item.IDPlaceOfIssue;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.PAddress] = item.PAddress;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.Cellphone] = item.Cellphone;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.IDNo] = item.IDNo;
                    if (item.DateComeBack != null)
                    {
                        dr[Hre_ReportProfileComeBackEntity.FieldNames.DateComeBack] = item.DateComeBack;
                    }
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.SalaryRankName] = item.SalaryRankName;
                    if (item.DateHire != null)
                    {
                        dr[Hre_ReportProfileComeBackEntity.FieldNames.DateHire] = item.DateHire;
                    }
                    if(item.DateQuit != null)
                    {
                        dr[Hre_ReportProfileComeBackEntity.FieldNames.DateQuit] = item.DateQuit;
                    }
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.TypeOfStop] = item.TypeOfStop;
                    dr[Hre_ReportProfileComeBackEntity.FieldNames.ResignReasonName] = item.ResignReasonName;

                    var contractEntity = lstContract.Where(s => item.ProfileID != null && s.ProfileID == item.ProfileID.Value).FirstOrDefault();

                    if (contractEntity != null)
                    {
                        var lstContractExtendByContractID = lstContractExtend.Where(s => s.ContractID != null && s.ContractID.Value == contractEntity.ID).ToList();
                        if (lstContractExtendByContractID.Count > 0)
                        {
                            var count = 0;
                            foreach (var contractExtend in lstContractExtendByContractID)
                            {
                                count++;
                                var annexCodeTitle = "AnnexCode" + count;
                                var dateStartTitle = "DateStart" + count;
                                var dateEndTitle = "DateEnd" + count;

                                if (!tb.Columns.Contains(annexCodeTitle))
                                {
                                    tb.Columns.Add(annexCodeTitle);
                                }

                                if (!tb.Columns.Contains(dateStartTitle))
                                {
                                    tb.Columns.Add(dateStartTitle, typeof(DateTime));
                                }

                                if (!tb.Columns.Contains(dateEndTitle))
                                {
                                    tb.Columns.Add(dateEndTitle, typeof(DateTime));
                                }

                                if (tb.Columns.Contains(annexCodeTitle))
                                {
                                    dr[annexCodeTitle] = contractExtend.AnnexCode;
                                }
                                if (tb.Columns.Contains(dateStartTitle))
                                {
                                    if (contractExtend.DateStart != null)
                                    {
                                        dr[dateStartTitle] = contractExtend.DateStart;
                                    }
                                }
                                if (tb.Columns.Contains(dateEndTitle))
                                {
                                    if (contractExtend.DateEnd != null)
                                    {
                                        dr[dateEndTitle] = contractExtend.DateEnd;
                                    }
                                }
                            }
                        }

                    }
                    tb.Rows.Add(dr);
                }
                return tb;
            }
        }



    }
}
