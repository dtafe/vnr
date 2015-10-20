using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Data.BaseRepository;
using System;
using HRM.Data.Entity;
using HRM.Business.HrmSystem.Models;
using VnResource.Tasks;
using VnResource.Helper.Utility;
using HRM.Business.Hr.Models;
using System.IO;
using HRM.Business.Category.Models;
using HRM.Business.Evaluation.Models;
using System.Collections;

namespace HRM.Business.Main.Domain
{
    public static class LibraryService
    {       

        #region Xử Lý tự động send mail Eva chưa có đánh giá

        public static void SendMailEvalution()
        {
            BaseService _base = new BaseService();
            string status = string.Empty;
            List<object> lstParam = new List<object>();
            lstParam.AddRange(new object[3]);
            var dataProfile = _base.GetData<Hre_ProfileEntity>(lstParam, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, string.Empty, ref status);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                try
                {
                    #region get config cycle Evalution

                    var dataSys_AllSetting = unitOfWork.CreateQueryable<Sys_AllSetting>(Guid.Empty).Select(s => new { s.ID, s.Name, s.Value1 }).ToList();

                    var empEva = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_ISEMPLOYEEEVALUATION.ToString()).FirstOrDefault();
                    var userEva1 = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_ISUSERAPPROVEVALUATION1.ToString()).FirstOrDefault();
                    var userEva2 = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_ISUSERAPPROVEVALUATION2.ToString()).FirstOrDefault();

                    var isCycle = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_ISCYCLE.ToString()).FirstOrDefault();
                    var cycleDay1 = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_CYCLE_DAY1.ToString()).FirstOrDefault();
                    var cycleDay2 = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_CYCLE_DAY2.ToString()).FirstOrDefault();
                    var cycleDay3 = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_BYCYCLE_DAY3.ToString()).FirstOrDefault();

                    var isTime = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_ISTIME.ToString()).FirstOrDefault();
                    var from = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_TIME_DATEFROM.ToString()).FirstOrDefault();
                    var to = dataSys_AllSetting.Where(s => s.Name == AppConfig.HRM_EVA_CONFIG_EMAIL_TIME_DATETO.ToString()).FirstOrDefault();

                    if ((isCycle.Value1 == null || isCycle.Value1 == false.ToString()) && (isTime.Value1 == null || isTime.Value1 == false.ToString()))
                    {
                        return;
                    }
                    else if (isCycle.Value1 == true.ToString())
                    {
                        if (!(isCycle.Value1 == true.ToString() && (
                            (cycleDay1.Value1 != null && int.Parse(cycleDay1.Value1) == DateTime.Today.Day)
                            || (cycleDay2.Value1 != null && int.Parse(cycleDay2.Value1) == DateTime.Today.Day)
                            || (cycleDay3.Value1 != null && int.Parse(cycleDay3.Value1) == DateTime.Today.Day))))
                        {
                            return;
                        }
                    }
                    else if (isTime.Value1 == true.ToString())
                    {
                        if (!(isTime.Value1 == true.ToString() && ((from.Value1 != null && int.Parse(from.Value1) <= DateTime.Today.Day) && (to.Value1 != null && int.Parse(to.Value1) >= DateTime.Today.Day))))
                        {
                            return;
                        }
                    }



                    #endregion

                    #region getdata
                    List<Eva_PerformanceEvaSendMailEntity> lstEvaLevel = new List<Eva_PerformanceEvaSendMailEntity>();
                    List<Eva_PerformanceEvaSendMailEntity> lstEvaLevel1 = new List<Eva_PerformanceEvaSendMailEntity>();
                    List<Eva_PerformanceEvaSendMailEntity> lstEvaLevel2 = new List<Eva_PerformanceEvaSendMailEntity>();

                    var dataEva_Performance = unitOfWork.CreateQueryable<Eva_Performance>(Guid.Empty, d => d.Status != PerformaceEvaStatus.E_APPROVE_END.ToString())
                        .Select(s => new { s.ID, s.ProfileID, s.Status, s.PerformanceTemplateID, s.PeriodFromDate, s.PeriodToDate }).ToList();

                    var dataEva_PerformanceEva = unitOfWork.CreateQueryable<Eva_PerformanceEva>(Guid.Empty, d => d.Status != PerformaceEvaStatus.E_APPROVE_END.ToString())
                        .Select(s => new Eva_PerformanceEvaSendMailEntity() { ID = s.ID, PerformanceID = s.PerformanceID, OrderEva = s.OrderEva, EvaluatorID = s.EvaluatorID, Status = s.Status }).ToList();

                    var dataEva_Evaluator = unitOfWork.CreateQueryable<Eva_Evaluator>(Guid.Empty).Select(s => new { s.ID, s.ProfileID, s.EvaluatorID, s.OrderNo }).ToList();
                    var dataEva_PerformanceTemplate = unitOfWork.CreateQueryable<Eva_PerformanceTemplate>(Guid.Empty).Select(s => new { s.ID, s.TemplateName }).ToList();

                    var profileOfPerformance = dataEva_Performance.Select(s => s.ProfileID).Distinct().ToList();

                    List<Hre_ProfileEntity> lstProEvalution = dataProfile.Where(s => profileOfPerformance.Contains(s.ID)).ToList();
                    #endregion

                    #region process Data Evalution

                    foreach (var profile in lstProEvalution)
                    {
                        var performByProfile = dataEva_Performance.Where(s => s.ProfileID == profile.ID).ToList();
                        if (performByProfile.Count == 0)
                            continue;
                        var performByProfileIDs = performByProfile.Select(s => s.ID).ToList();
                        var performEvaByProfile = dataEva_PerformanceEva.Where(s => s.PerformanceID != null && performByProfileIDs.Contains(s.PerformanceID.Value)).ToList();

                        foreach (var perform in performByProfile)
                        {
                            var performEva = performEvaByProfile.Where(s => s.PerformanceID == perform.ID).OrderBy(s => s.OrderEva).ToList();

                            foreach (var eva in performEva)
                            {
                                if (eva.Status == null || eva.Status == PerformaceEvaStatus.E_SEFT.ToString() || eva.Status == PerformaceEvaStatus.E_WAITING.ToString())
                                {
                                    //var evaluator = dataEva_Evaluator.Where(s => s.ProfileID == perform.ProfileID && s.EvaluatorID == eva.EvaluatorID).FirstOrDefault();
                                    eva.ProfileIDofEvaluator = eva.EvaluatorID;

                                    if (performEva.IndexOf(eva) == 0)
                                    {
                                        lstEvaLevel.Add(eva);
                                        break;
                                    }
                                    else if (performEva.IndexOf(eva) == 1)
                                    {
                                        lstEvaLevel1.Add(eva);
                                        break;
                                    }
                                    else if (performEva.IndexOf(eva) == 2)
                                    {
                                        lstEvaLevel2.Add(eva);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region process Send Mail

                    #region get Template merger data

                    string _typeTemplate = EnumDropDown.EmailType.E_REMIND_EVALUTION.ToString();
                    var template = unitOfWork.CreateQueryable<Sys_TemplateSendMail>(Guid.Empty, s => s.Type == _typeTemplate).FirstOrDefault();
                    if (template == null)
                        return;

                    string[] strsParaKey = null;
                    string[] strsParaValues = null;
                    strsParaKey = new string[] 
                    { 
                        EnumDropDown.EmailTypeDetails.E_UserName.ToString(), 
                        EnumDropDown.EmailTypeDetails.E_TableContent.ToString(), 
                    };
                    string table = string.Empty;

                    #endregion

                    if (lstEvaLevel.Count > 0)
                    {
                        var lstEvaluatorDistinct = lstEvaLevel.Select(s => s.ProfileIDofEvaluator).Distinct().ToList();

                        foreach (var evaluatorID in lstEvaluatorDistinct)
                        {
                            var evaInfo = dataProfile.Where(s => s.ID == evaluatorID).FirstOrDefault();
                            var lstSendwithEvalutor = lstEvaLevel.Where(s => s.ProfileIDofEvaluator == evaluatorID).ToList();

                            int i = 0;
                            table = string.Empty;
                            foreach (var item in lstSendwithEvalutor)
                            {
                                var _Performance = dataEva_Performance.Where(s => s.ID == item.PerformanceID).FirstOrDefault();
                                var _PerformanceTemplate = dataEva_PerformanceTemplate.Where(s => s.ID == _Performance.PerformanceTemplateID).FirstOrDefault();
                                var info = lstProEvalution.Where(s => s.ID == _Performance.ProfileID).FirstOrDefault();

                                i++;
                                if (i > 50)
                                {
                                    table += RowLast();
                                    break;
                                }
                                if (i == 1)
                                {
                                    table += RowFirst();
                                }

                                table += "<tr><td style=\"text-align: left\">";
                                table += i;
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                table += info.ProfileName.Trim();
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                table += info.CodeEmp.Trim();
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                table += info.OrgStructureName.Trim();
                                table += "</td>";

                                table += "<td style=\"text-align: left\">";
                                table += _PerformanceTemplate.TemplateName;
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                if (_Performance.PeriodFromDate != null)
                                {
                                    table += _Performance.PeriodFromDate.Value.ToString("dd/MM/yyyy");
                                }
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                if (_Performance.PeriodToDate != null)
                                {
                                    table += _Performance.PeriodToDate.Value.ToString("dd/MM/yyyy");
                                }
                                table += "</td>";
                                table += "</tr>";
                            }

                            table += "</tbody></table>";
                            strsParaValues = new string[] 
                            { 
                                evaInfo.ProfileName.ToString(), 
                                table                     
                            };
                            string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                            _base.SendMail(template.Subject, evaInfo.Email, body, null);
                        }
                    }
                    if (lstEvaLevel1.Count > 0)
                    {
                        var lstEvaluatorDistinct = lstEvaLevel1.Select(s => s.ProfileIDofEvaluator).Distinct().ToList();

                        foreach (var evaluatorID in lstEvaluatorDistinct)
                        {
                            var evaInfo = dataProfile.Where(s => s.ID == evaluatorID).FirstOrDefault();
                            var lstSendwithEvalutor = lstEvaLevel1.Where(s => s.ProfileIDofEvaluator == evaluatorID).ToList();

                            int i = 0;
                            table = string.Empty;
                            foreach (var item in lstSendwithEvalutor)
                            {
                                var _Performance = dataEva_Performance.Where(s => s.ID == item.PerformanceID).FirstOrDefault();
                                var _PerformanceTemplate = dataEva_PerformanceTemplate.Where(s => s.ID == _Performance.PerformanceTemplateID).FirstOrDefault();
                                var info = lstProEvalution.Where(s => s.ID == _Performance.ProfileID).FirstOrDefault();
                                i++;
                                if (i > 50)
                                {
                                    table += RowLast();
                                    break;
                                }
                                if (i == 1)
                                {
                                    table += RowFirst();
                                }

                                table += "<tr><td style=\"text-align: left\">";
                                table += i;
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                table += info.ProfileName.Trim();
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                table += info.CodeEmp.Trim();
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                table += info.OrgStructureName.Trim();
                                table += "</td>";

                                table += "<td style=\"text-align: left\">";
                                table += _PerformanceTemplate.TemplateName;
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                if (_Performance.PeriodFromDate != null)
                                {
                                    table += _Performance.PeriodFromDate.Value.ToString("dd/MM/yyyy");
                                }
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                if (_Performance.PeriodToDate != null)
                                {
                                    table += _Performance.PeriodToDate.Value.ToString("dd/MM/yyyy");
                                }
                                table += "</td>";
                                table += "</tr>";
                            }

                            table += "</tbody></table>";
                            strsParaValues = new string[] 
                            { 
                                evaInfo.ProfileName.ToString(), 
                                table                     
                            };
                            string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                            _base.SendMail(template.Subject, evaInfo.Email, body, null);
                        }
                    }
                    if (lstEvaLevel2.Count > 0)
                    {
                        var lstEvaluatorDistinct = lstEvaLevel2.Select(s => s.ProfileIDofEvaluator).Distinct().ToList();

                        foreach (var evaluatorID in lstEvaluatorDistinct)
                        {
                            var evaInfo = dataProfile.Where(s => s.ID == evaluatorID).FirstOrDefault();
                            var lstSendwithEvalutor = lstEvaLevel2.Where(s => s.ProfileIDofEvaluator == evaluatorID).ToList();

                            int i = 0;
                            table = string.Empty;
                            foreach (var item in lstSendwithEvalutor)
                            {
                                var _Performance = dataEva_Performance.Where(s => s.ID == item.PerformanceID).FirstOrDefault();
                                var _PerformanceTemplate = dataEva_PerformanceTemplate.Where(s => s.ID == _Performance.PerformanceTemplateID).FirstOrDefault();
                                var info = lstProEvalution.Where(s => s.ID == _Performance.ProfileID).FirstOrDefault();

                                i++;
                                if (i > 50)
                                {
                                    table += RowLast();
                                    break;
                                }
                                if (i == 1)
                                {
                                    table += RowFirst();
                                }

                                table += "<tr><td style=\"text-align: left\">";
                                table += i;
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                table += info.ProfileName.Trim();
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                table += info.CodeEmp.Trim();
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                table += info.OrgStructureName.Trim();
                                table += "</td>";

                                table += "<td style=\"text-align: left\">";
                                table += _PerformanceTemplate.TemplateName;
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                if (_Performance.PeriodFromDate != null)
                                {
                                    table += _Performance.PeriodFromDate.Value.ToString("dd/MM/yyyy");
                                }
                                table += "</td>";
                                table += "<td style=\"text-align: left\">";
                                if (_Performance.PeriodToDate != null)
                                {
                                    table += _Performance.PeriodToDate.Value.ToString("dd/MM/yyyy");
                                }
                                table += "</td>";
                                table += "</tr>";
                            }

                            table += "</tbody></table>";
                            strsParaValues = new string[] 
                            { 
                                evaInfo.ProfileName, 
                                table                     
                            };
                            string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);
                            _base.SendMail(template.Subject, evaInfo.Email, body, null);
                        }
                    }

                    #endregion

                }
                catch (Exception)
                {
                    throw;
                }

            }

        }

        #endregion

        #region Hàm xử lý tự động send mail hợp đồng đến hạn

        /// <summary>
        /// HieuVan
        /// Hàm xử lý tự động send mail hợp đồng đến hạn
        /// </summary>
        public static void SendMailContract()
        {
            BaseService _base = new BaseService();
            string status = string.Empty;
            string fullPath = string.Empty;
            List<Hre_ContractSendMailEntity> lstProbation = new List<Hre_ContractSendMailEntity>();
            List<Hre_ContractEntity> lstAttach = new List<Hre_ContractEntity>();
            Hre_ContractEntity attach = new Hre_ContractEntity();
            DateTime DS = DateTime.Now;
            DateTime DE = DateTime.Now;

            List<object> lstParam = new List<object>();
            lstParam.AddRange(new object[3]);
            var dataProfile = _base.GetData<Hre_ProfileEntity>(lstParam, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, string.Empty, ref status);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_Contract = new CustomBaseRepository<Hre_Contract>(unitOfWork);
                var repoCat_ContractType = new CustomBaseRepository<Cat_ContractType>(unitOfWork);
                var repoCat_Export = new CustomBaseRepository<Cat_Export>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);
                var repoSys_TemplateSendMail = new CustomBaseRepository<Sys_TemplateSendMail>(unitOfWork);

                string _typeTemplate = EnumDropDown.EmailType.E_WARNINGCONTRACT.ToString();
                var template = repoSys_TemplateSendMail.FindBy(s => s.Type == _typeTemplate).FirstOrDefault();
                if (template == null)
                    return;

                var dataContractType = repoCat_ContractType.FindBy(s => s.ID != null).ToList();
                var dataContract = repoHre_Contract.FindBy(s => s.IsDelete == null).ToList();
                var dataOrgStructure = repoCat_OrgStructure.FindBy(s => s.IsDelete == null).ToList();
                var dataOrgStructureType = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                // lọc từng hợp đồng, kiểm tra những hợp đồng nào đến hạn
                Hre_ContractSendMailEntity entity = new Hre_ContractSendMailEntity();
                List<string> lstTemp = new List<string>();
                foreach (var item in dataContract)
                {
                    lstTemp = new List<string>();
                    entity = new Hre_ContractSendMailEntity();
                    if (item.DateEnd == null)
                        continue;
                    var contractType = dataContractType.Where(s => s.ID == item.ContractTypeID).FirstOrDefault();
                    int loop = contractType.ExpiryContractLoop != null ? contractType.ExpiryContractLoop.Value : 0;
                    int dbd = contractType.ExpiryContractDayByDay != null ? contractType.ExpiryContractDayByDay.Value : 0;

                    DateTime loopStart = DateTime.Now.AddDays(loop);
                    DateTime dbdStart = DateTime.Now.AddDays(dbd);

                    if ((dbdStart >= item.DateEnd.Value || item.DateEnd.Value <= DateTime.Now) && item.ContractResult == null)
                    {
                        attach = item.Copy<Hre_ContractEntity>();
                        var profile = dataProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                        entity.ContractID = item.ID;
                        entity.DayDue = item.DateEnd;
                        entity.ProfileName = profile.ProfileName;
                        attach.ProfileName = profile.ProfileName;
                        attach.CodeEmp = profile.CodeEmp;
                        entity.Type = EnumDropDown.ExpiredContractType.E_DayByDay.ToString();
                        if (contractType.EmailToList != null)
                        {
                            var lstCodeOrg = contractType.EmailToList.Split(',').ToList();
                            foreach (var itemCO in lstCodeOrg)
                            {
                                var temp = LibraryService.GetNearestParent(profile.OrgStructureID, (OrgUnit)Enum.Parse(typeof(OrgUnit), itemCO, true), dataOrgStructure, dataOrgStructureType);
                                if (temp == null || temp.Email == null)
                                    continue;
                                var mailOfOrg = temp.Email.Split(',').ToList();
                                lstTemp.AddRange(mailOfOrg);
                            }
                        }
                        if (!string.IsNullOrEmpty(contractType.EmailOther))
                        {
                            var lstEmailOther = contractType.EmailOther.Split(',').ToList();
                            lstTemp.AddRange(lstEmailOther);
                        }
                        if (lstTemp != null)
                            entity.lstEmail = lstTemp;
                        lstAttach.Add(attach);
                        lstProbation.Add(entity);
                    }
                    else if ((loopStart >= item.DateEnd.Value || item.DateEnd.Value <= DateTime.Now) && item.ContractResult == null)
                    {
                        attach = item.Copy<Hre_ContractEntity>();
                        if (DateTime.Now.Day != contractType.DayInMonthLoop1 && DateTime.Now.Day != contractType.DayInMonthLoop2 && DateTime.Now.Day != contractType.DayInMonthLoop3)
                            continue;
                        var profile = dataProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                        entity.ContractID = item.ID;
                        entity.DayDue = item.DateEnd;
                        entity.ProfileName = profile.ProfileName;
                        attach.ProfileName = profile.ProfileName;
                        attach.CodeEmp = profile.CodeEmp;
                        entity.Type = EnumDropDown.ExpiredContractType.E_Loop.ToString();
                        if (contractType.EmailToList != null)
                        {
                            var lstCodeOrg = contractType.EmailToList.Split(',').ToList();
                            foreach (var itemCO in lstCodeOrg)
                            {
                                var temp = LibraryService.GetNearestParent(profile.OrgStructureID, (OrgUnit)Enum.Parse(typeof(OrgUnit), itemCO, true), dataOrgStructure, dataOrgStructureType);
                                if (temp == null || temp.Email == null)
                                    continue;
                                var mailOfOrg = temp.Email.Split(',').ToList();
                                lstTemp.AddRange(mailOfOrg);
                            }
                        }
                        if (!string.IsNullOrEmpty(contractType.EmailOther))
                        {
                            var lstEmailOther = contractType.EmailOther.Split(',').ToList();
                            lstTemp.AddRange(lstEmailOther);
                        }
                        if (lstTemp != null)
                            entity.lstEmail = lstTemp;
                        lstAttach.Add(attach);
                        lstProbation.Add(entity);
                    }
                }

                if (lstAttach.Count > 0)
                {
                    var lstExport = repoCat_Export.FindBy(s => s.IsDelete == null && s.ScreenName == EnumDropDown.EmailType.E_WARNINGCONTRACT.ToString()).ToList();
                    if (lstExport.Count > 0)
                    {
                        var export = lstExport.FirstOrDefault();
                        fullPath = ExportService.Export(export.ID, lstAttach, null, ExportFileType.Excel);
                    }
                }

                List<Hre_ContractBodyEntity> lstSM = new List<Hre_ContractBodyEntity>();
                Hre_ContractBodyEntity SM = new Hre_ContractBodyEntity();
                List<Guid> lstGC = new List<Guid>();
                foreach (var item in lstProbation)
                {
                    string _type = "Theo Chu Kỳ";
                    if (item.Type == EnumDropDown.ExpiredContractType.E_DayByDay.ToString())
                        _type = "Hàng Ngày";
                    if (item.lstEmail == null)
                        continue;
                    string value = "<tr><td> " + item.ProfileName + " </td><td> " + item.DayDue + "</td><td> " + _type + "</td></tr>";

                    foreach (var email in item.lstEmail)
                    {
                        lstGC = new List<Guid>();
                        SM = new Hre_ContractBodyEntity();
                        var exits = lstSM.Where(m => m.Email == email).FirstOrDefault();
                        if (exits != null && !exits.lstGContract.Contains(item.ContractID))
                        {
                            if (SM.lstGContract != null)
                                lstGC = SM.lstGContract;

                            lstGC.Add(item.ContractID);
                            exits.lstGContract = lstGC;
                            exits.Body += value;
                        }
                        else if (exits == null || !exits.lstGContract.Contains(item.ContractID))
                        {
                            if (SM.lstGContract != null)
                                lstGC = SM.lstGContract;

                            lstGC.Add(item.ContractID);
                            SM.lstGContract = lstGC;
                            SM.Email = email;
                            SM.Body = value;
                            lstSM.Add(SM);
                        }
                    }
                }

                foreach (var item in lstSM)
                {
                    string title = template.Subject;
                    var name = item.Email.Split('@');


                    #region magreData

                    string[] strsParaKey = null;
                    string[] strsParaValues = null;
                    strsParaKey = new string[] 
                    { 
                        EnumDropDown.EmailType_WARNINGCONTRACT.E_PROFILENAME.ToString(), 
                        EnumDropDown.EmailType_WARNINGCONTRACT.E_TABLECONTENT.ToString(), 
                    };
                    strsParaValues = new string[] 
                    { 
                        name[0].ToString(), 
                        item.Body                        
                    };
                    string body = LibraryService.ReplaceContentFile(template.Content, strsParaKey, strsParaValues);

                    #endregion

                    //string body = "<html><head><title></title></head><body class='scayt-enabled'>Dear " + name[0].ToString() + " ,<br /><br />"
                    //    + "This is email warning about contract probation:<br /> <br /><table border='1' cellpadding='1' cellspacing='1' style='width: 500px;'>"
                    //    + "<tr><td> Nhân Viên </td><td> Ngày Hết Hạn </td><td> Loại Hết Hạn </td></tr>"
                    //    + item.Body
                    //    + "</tbody></table><tbody>"
                    //    + "</body></html>";

                    _base.SendMail(title, item.Email, body, fullPath);
                }

            }
        }

        #endregion

        #region ReplaceContentFile

        public static string ReplaceContentFile(string contentFile, string[] strsParaKey, string[] strsParaValues)
        {
            if (!string.IsNullOrEmpty(contentFile))
            {
                for (int i = 0; i < strsParaKey.Length; i++)
                {
                    contentFile = contentFile.Replace(strsParaKey[i], strsParaValues[i]);
                }
            }

            return contentFile;
        }

        #endregion

        #region GetNearestParentEntity
        public static Cat_OrgStructureEntity GetNearestParentEntity(Guid? orgId
                                             , OrgUnit unit
                                             , List<Cat_OrgStructureEntity> lstAllStructure, List<Cat_OrgStructureTypeEntity> lstAllOrgType)
        {
            Cat_OrgStructureEntity structure = lstAllStructure.Where(m => m.ID == orgId).FirstOrDefault();
            Cat_OrgStructureTypeEntity orgType = lstAllOrgType.Where(m => m.OrgStructureTypeCode == unit.ToString()).FirstOrDefault();
            if (structure == null)
                return null;
            if (structure.OrgStructureTypeID == null)
                return null;
            if (orgType == null)
                return null;
            if (structure.OrgStructureTypeID.Value == orgType.ID || structure.ParentID == null)
                return structure;
            return GetNearestParentEntity(structure.ParentID, unit, lstAllStructure, lstAllOrgType);
        }

        public static Cat_OrgStructure GetNearestParent(Guid? orgId
                                             , OrgUnit unit
                                             , List<Cat_OrgStructure> lstAllStructure, List<Cat_OrgStructureType> lstAllOrgType)
        {
            Cat_OrgStructure structure = lstAllStructure.Where(m => m.ID == orgId).FirstOrDefault();
            Cat_OrgStructureType orgType = lstAllOrgType.Where(m => m.OrgStructureTypeCode == unit.ToString()).FirstOrDefault();
            if (structure == null)
                return null;
            if (structure.OrgStructureTypeID == null)
                return null;
            if (orgType == null)
                return null;
            if (structure.OrgStructureTypeID.Value == orgType.ID || structure.ParentID == null)
                return structure;
            return GetNearestParent(structure.ParentID, unit, lstAllStructure, lstAllOrgType);
        }

        public static Sys_AllSettingEntity GetSys_AllSettingByKey(string key)
        {
            var service = new BaseService();
            var sys_AllSetting = new Sys_AllSettingEntity();
            string status = string.Empty;
            var lstEntity = service.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, string.Empty, ref status);
            if (lstEntity != null)
                sys_AllSetting = lstEntity.FirstOrDefault();
            return sys_AllSetting;
        }

        public static Type GetEntityType(string entityName)
        {
            return typeof(HRM.Data.Entity.VnrHrmDataContext).Assembly.GetTypes().Where(d => d.Name == entityName).FirstOrDefault();
        }

        #endregion

        

        #region FirstLast Row Table

        public static string RowLast()
        {
            string body = "<tr><td style=\"text-align: left\">";
            body += 51;
            body += "</td>";
            body += "<td style=\"text-align: left\"> More...";
            body += "</td>";
            body += "<td style=\"text-align: left\">";
            body += "</td>";
            body += "<td style=\"text-align: left\">";
            body += "</td>";
            body += "<td style=\"text-align: left\">";
            body += "</td>";
            body += "<td style=\"text-align: left\">";
            body += "</td>";
            body += "<td style=\"text-align: left\">";
            body += "</td>";
            body += "</tr>";
            return body;
        }
        public static string RowFirst()
        {
            string body = "<table style=\"border-bottom: 1px solid #dedede;width:970px; border-right: 1px solid #dedede; border-left: 1px solid #dedede;\"><tbody><tr>";
            body += "<td style=\"font-weight: bold; text-align: center; color: #000000; background-color: #CCCCCC; width: 30px;\">#</td>";
            body += "<td style=\"font-weight: bold; text-align: center; color: #000000; background-color: #CCCCCC; width: 250px;\">Tên/Name</td>";
            body += "<td style=\"font-weight: bold; text-align: center; color: #000000; background-color: #CCCCCC; width: 150px;\">Mã/CodeEmp</td>";
            body += "<td style=\"font-weight: bold; text-align: center; color: #000000; background-color: #CCCCCC; width: 150px;\">Phòng Ban/Orgstructure</td>";
            body += "<td style=\"font-weight: bold; text-align: center; color: #000000; background-color: #CCCCCC; width: 150px;\">Bảng Đánh Giá/Table Eva</td>";
            body += "<td style=\"font-weight: bold; text-align: center; color: #000000; background-color: #CCCCCC; width: 120px;\">Từ/From</td>";
            body += "<td style=\"font-weight: bold; text-align: center; color: #000000; background-color: #CCCCCC; width: 120px;\">Đến/To</td>";
            body += "</tr>";
            return body;
        }

        #endregion

    }

}
