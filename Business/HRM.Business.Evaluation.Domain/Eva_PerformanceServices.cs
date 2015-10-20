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
using HRM.Business.Hr.Models;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Payroll.Models;
using HRM.Business.Payroll.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Infrastructure.Utilities.Helper;
using VnResource.Helper.Data;

namespace HRM.Business.Evaluation.Domain
{
    public class Eva_PerformanceServices : BaseService
    {
        #region AttachFile Ultilities

        /// <summary> Chuyển chuổi sang XmlDocument </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public XmlDocument StringToXML(string xmlString)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.LoadXml(xmlString);
            }
            catch (Exception)
            { }
            return xml;
        }

        /// <summary> Chuyển XmlDocument sang chuổi </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string XmlToString(XmlDocument doc)
        {
            return doc.OuterXml;
        }

        /// <summary> Ghi xml  </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public XmlDocument WriteXml(List<AttachFileEntity> files)
        {
            //A new XML Document
            var doc1 = new XmlDocument();

            //Xml Declaration
            var declaration = doc1.CreateXmlDeclaration("1.0", "utf-8", "yes");
            //Attach declaration to the document
            doc1.AppendChild(declaration);

            //Create root element
            XmlElement root1 = doc1.CreateElement("files");
            //Attach the root node to the document
            doc1.AppendChild(root1);

            foreach (var attachFile in files)
            {
                //Create a file child element
                XmlElement file = doc1.CreateElement("file");
                //Add an attribute FullName 
                file.SetAttribute(AttachFileEntity.FieldNames.FullName, attachFile.FullName);
                file.SetAttribute(AttachFileEntity.FieldNames.FileName, attachFile.FileName);
                file.SetAttribute(AttachFileEntity.FieldNames.FileSize, attachFile.FileSize.ToString());
                file.SetAttribute(AttachFileEntity.FieldNames.FileExtension, attachFile.FileExtension);

                //Attach Person child element to the root Persons element
                if (doc1.DocumentElement != null)
                {
                    doc1.DocumentElement.AppendChild(file);
                }
            }
            return doc1;
        }

        /// <summary> Đọc data của XML </summary>
        /// <param name="doc"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public List<string> ReadXml(XmlDocument doc, string attributeName)
        {
            var lstFileNames = new List<string>();
            if (doc.DocumentElement != null)
            {
                foreach (XmlNode xmlNode in doc.DocumentElement.ChildNodes)
                {
                    if (xmlNode.Attributes != null && xmlNode.Attributes[attributeName] != null)
                    {
                        var value = xmlNode.Attributes[attributeName].Value;
                        lstFileNames.Add(value);
                    }
                }
            }
            return lstFileNames;
        }

        public List<string> ReadXml(string xmlDocument, string attributeName)
        {
            var lstFileNames = new List<string>();
            var doc = StringToXML(xmlDocument);
            if (doc.DocumentElement != null)
            {
                foreach (XmlNode xmlNode in doc.DocumentElement.ChildNodes)
                {
                    if (xmlNode.Attributes != null && xmlNode.Attributes[attributeName] != null)
                    {
                        var value = xmlNode.Attributes[attributeName].Value;
                        lstFileNames.Add(value);
                    }
                }
            }
            return lstFileNames;
        }

        /// <summary> Chuyen Xml sang FileEntity object </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public List<AttachFileEntity> XmlToAttachFileEntity(XmlDocument doc)
        {
            var lstFileNames = new List<AttachFileEntity>();
            if (doc.DocumentElement != null)
            {
                foreach (XmlNode xmlNode in doc.DocumentElement.ChildNodes)
                {
                    string fullName = string.Empty;
                    int _fileSize = 0;
                    if (xmlNode.Attributes != null)
                    {
                        var atts = xmlNode.Attributes;
                        //duyet attributes
                        for (int i = 0; i < atts.Count; i++)
                        {
                            var name = atts[i].Name;
                            if (name == AttachFileEntity.FieldNames.FullName)
                            {
                                fullName = atts[i].Value;
                            }
                            if (name == AttachFileEntity.FieldNames.FileSize)
                            {
                                int.TryParse(atts[i].Value, out _fileSize);
                            }
                        }
                        var attachFile = new AttachFileEntity(fullName, _fileSize);
                        lstFileNames.Add(attachFile);
                    }
                }
            }
            return lstFileNames;
        }

        #endregion

        /// <summary> Lấy file đính kèm theo cấp bậc đánh giá </summary>
        /// <param name="PerformanceID"></param>
        /// <returns></returns>
        public string GetAttachFileByOrderNo(Guid PerformanceID, int orderNo)
        {
            string attachFile = "";
            //lay thu tu đánh giá của người trước
            orderNo = orderNo - 1;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoP = new Eva_PerformanceRepository(unitOfWork);
                var repoPE = new Eva_PerformanceEvaRepository(unitOfWork);
                Eva_Performance performance = repoP.GetById(PerformanceID);
                if (performance != null && !string.IsNullOrEmpty(performance.AttachFile) && performance.IsDelete == null)
                {
                    attachFile = performance.AttachFile;
                }
                var ilEva_PerformanceEva = repoPE.FindBy(x => x.PerformanceID == PerformanceID
                    && x.IsDelete == null && x.OrderEva == orderNo).Select(p => p.AttachFile).FirstOrDefault();
                if (ilEva_PerformanceEva != null)
                {
                    attachFile = ilEva_PerformanceEva;
                }
            }

            #region Đọc Xml từ db và chuyển sang string

            if (!string.IsNullOrEmpty(attachFile))
            {
                var performanceService = new Eva_PerformanceServices();
                var doc = performanceService.ReadXml(attachFile, AttachFileEntity.FieldNames.FullName);
                attachFile = string.Join(",", doc.ToArray());
            }
            #endregion

            return attachFile;
        }


        private List<Eva_KPIEntity> GetKPIByTemplateID(Guid? templateId,string userLogin)
        {
            var templateID = string.Empty;
            if (templateId != null)
            {
                templateID = templateId == null ? Guid.Empty.ToString() : templateId.ToString();
                templateID = Common.DotNetToOracle(templateID);
            }

            string status = string.Empty;
            List<object> lstObj = new List<object>();
            lstObj.Add(templateID);
            lstObj.Add(1);
            lstObj.Add(50);
            var listKPIs = GetData<Eva_KPIEntity>(lstObj, ConstantSql.hrm_eva_sp_get_KpiByTemplateID, userLogin, ref status);
            return listKPIs;
        }

        public bool DeletePerformance(Guid performanceId)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPerformance = new Eva_PerformanceRepository(unitOfWork);
                var repoEvaluator = new Eva_EvaluatorRepository(unitOfWork);
                var repoPerformanceForDetail = new Eva_PerformanceForDetailRepository(unitOfWork);
                var repoPerformanceEva = new Eva_PerformanceEvaRepository(unitOfWork);
                var repoPerformanceEvaDetail = new Eva_PerformanceEvaDetailRepository(unitOfWork);
                //   var evaluatorObjs = repoEvaluator.FindBy(p => evaluators.Contains(p.EvaluatorID ?? Guid.Empty) && p.ProfileID == model.ProfileID && p.PerformanceTypeID == model.PerformanceTypeID).ToList();

                //get performance
                var performanceDelete = repoPerformance.FindBy(p => p.ID == performanceId).FirstOrDefault();
                if (performanceDelete != null)
                {
                    performanceDelete.IsDelete = true;
                    repoPerformance.SaveChanges();

                    #region xoa performanceForDetail
                    var performanceForDetailDelete =
                        repoPerformanceForDetail.FindBy(p => p.PerformanceID == performanceDelete.ID).ToList();
                    foreach (var performanceForDetail in performanceForDetailDelete)
                    {
                        performanceForDetail.IsDelete = true;
                    }
                    if (performanceForDetailDelete.Any())
                    {
                        repoPerformanceForDetail.SaveChanges();
                    }
                    #endregion

                    #region xoa eva_performanceEva

                    var performanceEvaDelete =
                        repoPerformanceEva.FindBy(p => p.PerformanceID == performanceDelete.ID).ToList();

                    foreach (var evaPerformanceEva in performanceEvaDelete)
                    {
                        evaPerformanceEva.IsDelete = true;
                    }
                    if (performanceEvaDelete.Any())
                    {
                        repoPerformanceEva.SaveChanges();
                    }

                    #endregion

                    #region xoa eva_performanceEvaDetail

                    if (performanceEvaDelete.Any())
                    {
                        foreach (var evaPerformanceEva in performanceEvaDelete)
                        {
                            var performanceEvaDetailDelete =
                                repoPerformanceEvaDetail.FindBy(p => p.PerfomanceEvaID == evaPerformanceEva.ID)
                                    .FirstOrDefault();
                            if (performanceEvaDetailDelete != null)
                            {
                                performanceEvaDetailDelete.IsDelete = true;
                            }
                        }
                        repoPerformanceEvaDetail.SaveChanges();
                    }

                    #endregion
                }
            }
            return true;
        }

        /// <summary> Kiểm tra Performance có tồn tại không  </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckPerformanceExist(Eva_PerformanceEntity model)
        {
            bool isExist = false;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPerformance = new Eva_PerformanceRepository(unitOfWork);
                if (model != null)
                {
                    var performance = repoPerformance.FindBy(p => p.IsDelete == null && p.ProfileID.Value == model.ProfileID.Value
                                                                  && p.PerformanceTemplateID == model.PerformanceTemplateID
                                                                  && p.PerformancePlanID == model.PerformancePlanID
                                                                  && p.PeriodFromDate == model.PeriodFromDate
                                                                  && p.PeriodToDate == model.PeriodToDate).FirstOrDefault();
                    if (performance != null)
                    {
                        isExist = true;
                    }
                }
            }
            return isExist;
        }

        public Eva_PerformanceEntity AddPerfomance(Eva_PerformanceEntity model, bool isSelft)
        {
            /*
            *  Goal(Luu đánh giá nhân viên)
            *  Steps :
            *      Step1  :  Insert Eva_Performance
            *      Step2  :  Tự động insert xuống table Eva_PerformanceForDetail
            *      Step3  :  Tự động insert xuống table Eva_PerformanceEva &Eva_PerformanceEvaDetail 
            *                  - Tự Đánh Giá : orderNo =0 (Ngoai ra orderNo >0)
             
            */


            var evaluationStatus = PerformaceEvaStatus.E_WAITING.ToString();
            var result = new Eva_PerformanceEntity();
            var performanceForDetail = new Eva_PerformanceForDetail();
            var performanceEva = new Eva_PerformanceEva();
            var performanceEvaDetail = new Eva_PerformanceEvaDetail();

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPerformance = new Eva_PerformanceRepository(unitOfWork);
                var repoEvaluator = new Eva_EvaluatorRepository(unitOfWork);
                var repoPerformanceForDetail = new Eva_PerformanceForDetailRepository(unitOfWork);
                var repoPerformanceEva = new Eva_PerformanceEvaRepository(unitOfWork);
                var repoPerformanceEvaDetail = new Eva_PerformanceEvaDetailRepository(unitOfWork);

                #region step1 : thêm Eva_Performance
                result = model;
                if (result != null && result.ID == Guid.Empty)
                {
                    result.Status = evaluationStatus;
                }
                Add(result);
                #endregion

                #region Step2  :  Tự động insert xuống table Eva_PerformanceForDetail dựa vào các KPI tương ứng

                //lay ds KPIs (kèm theo Marks và Evaluation) - lay dữ liệu từ edit trên lưới
                var kpis = model.KPIs;//cac tieu chi danh gia
                var kpiByTempalateIds = kpis;

                var lstPerformanceForDetails = new List<Eva_PerformanceForDetail>();
                foreach (var kpiByTempalateId in kpiByTempalateIds)
                {
                    performanceForDetail = new Eva_PerformanceForDetail();
                    performanceForDetail.ID = Guid.NewGuid();
                    if (result != null)
                    {
                        performanceForDetail.PerformanceID = result.ID;
                    }
                    performanceForDetail.KPIID = kpiByTempalateId.ID;
                    performanceForDetail.KPIName = kpiByTempalateId.KPIName;
                    performanceForDetail.OrderKPI = kpiByTempalateId.OrderNumber;
                    performanceForDetail.OrderNumber = kpiByTempalateId.OrderNumber;
                    performanceForDetail.Rate = kpiByTempalateId.Rate;
                    lstPerformanceForDetails.Add(performanceForDetail);
                    repoPerformanceForDetail.Add(performanceForDetail);//add vao repo
                }
                repoPerformanceForDetail.SaveChanges();
                #endregion

                #region Step3  :  Tự động insert xuống table Eva_PerformanceEva &Eva_PerformanceEvaDetail

                #region Luu bảng Eva_PerfromanceEva (cho từng người đánh giá)

                var lstPerformanceEvaIDs = new List<Guid>();
                int orderNo = 1;
                var performanceEvaSeftId = Guid.Empty;//ID cua bang Eva_PerformanceEva

                #region Tự đánh giá : tự động add 1 người đánh giá là chính bảng thân người tạo bảng đánh giá
                if (isSelft && result != null && model != null && model.ProfileID != Guid.Empty)
                {
                    //tu tao nguoi danh gia la ban than
                    performanceEva = new Eva_PerformanceEva();
                    performanceEva.ID = Guid.NewGuid();
                    performanceEvaSeftId = performanceEva.ID;
                    lstPerformanceEvaIDs.Add(performanceEva.ID);
                    performanceEva.PerformanceID = result.ID;
                    performanceEva.EvaluatorID = model.ProfileID;
                    performanceEva.Status = PerformaceEvaStatus.E_SEFT.ToString();//evaluationStatus;
                    performanceEva.OrderEva = 0;
                    repoPerformanceEva.Add(performanceEva);

                    //cap nhat lai status cua Eva_Performance
                    var performanceEntity = repoPerformance.GetById(result.ID);
                    if (performanceEntity != null)
                    {
                        performanceEntity.Status = PerformaceEvaStatus.E_SEFT.ToString();
                        repoPerformance.SaveChanges();
                    }
                }

                #endregion

                //Lấy ds người đánh giá theo NV và loại đánh giá
                var evaluatorIds = model.EvaluatorIDs;
                var evaluatorObjs = repoEvaluator.FindBy(p => p.IsDelete == null && evaluatorIds.Contains(p.EvaluatorID ?? Guid.Empty)
                    && p.ProfileID == model.ProfileID
                    && p.PerformanceTypeID == model.PerformanceTypeID).ToList();
                evaluatorObjs = evaluatorObjs.OrderBy(p => p.OrderNo).ToList();

                foreach (var evaluator in evaluatorObjs)
                {
                    performanceEva = new Eva_PerformanceEva();
                    performanceEva.ID = Guid.NewGuid();
                    lstPerformanceEvaIDs.Add(performanceEva.ID);
                    if (result != null)
                    {
                        performanceEva.PerformanceID = result.ID;
                    }
                    performanceEva.EvaluatorID = evaluator.EvaluatorID;
                    performanceEva.Status = evaluationStatus;
                    performanceEva.OrderEva = orderNo++;
                    repoPerformanceEva.Add(performanceEva);
                }
                repoPerformanceEva.SaveChanges();
                #endregion

                #region Luu Eva_PerfromanceEvaDetail (lưu chi tiết cho bảng đánh giá)

                var isSeft = false;
                //khởi tạo chi tiet đánh giá cho từng người đánh giá
                foreach (var performanceEvaID in lstPerformanceEvaIDs)
                {
                    //ds cac tieu chí
                    foreach (var performanceForDetailItem in lstPerformanceForDetails)
                    {
                        performanceEvaDetail = new Eva_PerformanceEvaDetail
                        {
                            ID = Guid.NewGuid(),
                            PerfomanceEvaID = performanceEvaID,
                            PerfomanceDetailID = performanceForDetailItem.ID,
                            KPIName = performanceForDetailItem.KPIName
                        };

                        #region Dành Cho Tự Đánh Giá

                        if (performanceEvaSeftId != Guid.Empty && performanceEvaSeftId == performanceEvaID)
                        {
                            /*Add Mark va Evaluation cho nguoi danh gia la ban than*/
                            //lay kpi theo name tu model de set data cho Mark va Evaluation

                            var kpi = kpiByTempalateIds.FirstOrDefault(p => p.KPIName == performanceEvaDetail.KPIName);
                            if (kpi != null)
                            {
                                performanceEvaDetail.Mark = kpi.Mark;
                                performanceEvaDetail.Evaluation = kpi.Evaluation;
                            }
                            //đặt flag là tự đánh giá
                            isSeft = true;
                        }
                        repoPerformanceEvaDetail.Add(performanceEvaDetail);
                        #endregion
                    }

                    if (isSeft)
                    {
                        performanceEvaSeftId = Guid.Empty;
                    }

                }
                repoPerformanceEvaDetail.SaveChanges();

                #endregion

                #endregion

            }
            return result;
        }
        public void UpdatePerformanceExtendIDOfPerformance(Guid PerformanceExtendID, Guid PerformanceID)
        {
            var result = new Eva_EvaluatorEntity();
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Eva_PerformanceRepository(unitOfWork);
                var Performance = repo.GetById(PerformanceID);
                if (Performance != null)
                    Performance.PerformanceExtendID = PerformanceExtendID;
                repo.SaveChanges();



            }
        }
        public string SubmitKPIforPerformance(Guid PerformanceID, List<Eva_KPIEntity> list)
        {
            string rs = null;
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPerformance = new Eva_PerformanceRepository(unitOfWork);
                var repoPerformanceForDetail = new Eva_PerformanceForDetailRepository(unitOfWork);
                var repoEva_KPI = new Eva_KPIRepository(unitOfWork);
                var lstPerformanceForDetail = repoPerformanceForDetail.FindBy(x => x.PerformanceID == PerformanceID).ToList();

                var kpiIds = list.Select(p => p.ID).ToList();
                var kpiSource = repoEva_KPI.FindBy(p => kpiIds.Contains(p.ID)).ToList();
                foreach (var evaKpi in list)
                {
                    if (evaKpi.IsKPIFix != null && evaKpi.IsKPIFix.Value)
                    {
                        var kpiSourceItem = kpiSource.FirstOrDefault(x => x.ID == evaKpi.ID);
                        if (kpiSourceItem != null)
                        {
                            // evaKpi.DescriptionKPIFix = kpiSourceItem.DescriptionKPIFix;
                        }
                    }
                }


                if (lstPerformanceForDetail == null && lstPerformanceForDetail.Count == 0)
                    return null;
                foreach (var item in lstPerformanceForDetail)
                {
                    var obj = list.FirstOrDefault(x => x.PerformanceForDetailID == item.ID);
                    if (obj != null)
                    {
                        item.DescriptionKP = obj.DescriptionKP;
                        item.DescriptionKPIFix = obj.DescriptionKPIFix;
                        item.Rate = obj.Rate;
                        //todo: kt neu isFixKPI = false => cho edit DescriptionKPIFix
                        //if (obj.IsKPIFix.HasValue && !obj.IsKPIFix.Value)
                        //{
                        //    item.DescriptionKPIFix = obj.DescriptionKPIFix;
                        //}    


                    }
                }
                repoPerformanceForDetail.SaveChanges();
                var Performance = repoPerformance.GetById(PerformanceID);
                Performance.Status = "Submit";
                repoPerformance.SaveChanges();
                rs = Performance.Status;
            }
            return rs;
        }
        public string ApproveKPIforPerformance(Guid PerformanceID, List<Eva_KPIEntity> list)
        {
            string rs = null;
            using (var context = new VnrHrmDataContext())
            {
                var status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPerformance = new Eva_PerformanceRepository(unitOfWork);
                var repoPerformanceForDetail = new Eva_PerformanceForDetailRepository(unitOfWork);
                var repoPerformanceEvaDetail = new Eva_PerformanceEvaDetailRepository(unitOfWork);
                var repoPerformanceEva = new Eva_PerformanceEvaRepository(unitOfWork);
                var repoEvaluator = new Eva_EvaluatorRepository(unitOfWork);
                var lstPerformanceForDetail = repoPerformanceForDetail.FindBy(x => x.PerformanceID == PerformanceID).ToList();
                var Performance = repoPerformance.GetById(PerformanceID);
                if (lstPerformanceForDetail == null && lstPerformanceForDetail.Count == 0)
                    return null;
                foreach (var item in lstPerformanceForDetail)
                {
                    var obj = list.FirstOrDefault(x => x.PerformanceForDetailID.HasValue && x.PerformanceForDetailID == item.ID);
                    if (obj != null)
                    {
                        item.DescriptionKP = obj.DescriptionKP;
                    }
                }
                repoPerformanceForDetail.SaveChanges();
                Performance.Status = "Approve";
                repoPerformance.SaveChanges();
                rs = Performance.Status;

            }
            return rs;
        }

        public string ApplyPerformance(Guid? profileID, Guid? rankdetailID, DateTime? dateEffect,string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string result = ConstantMessages.Succeed;
                var profileservices = new Hre_ProfileServices();
                BaseService service = new BaseService();
                string message = string.Empty;
                string status = string.Empty;
                var contractservices = new Hre_ContractServices();
                var basicSalaryServices = new Sal_BasicSalaryServices();
                var InsuranceSalaryServices = new Sal_InsuranceSalaryServices();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCandidate = new Rec_CandidateRepository(unitOfWork);
                var repoHistory = new Rec_RecruitmentHistoryRepository(unitOfWork);
                var profile = service.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
                var salaryRank = service.GetData<Cat_SalaryRankEntity>(Common.DotNetToOracle(rankdetailID.ToString()), ConstantSql.hrm_cat_sp_get_SalaryRankById, userLogin, ref status).FirstOrDefault();

                Guid? _AbilitiTitleID = null;
                if (salaryRank != null && salaryRank.SalaryClassID != null)
                {
                    var abilityTitleBySalaryClass = service.GetData<Cat_AbilityTileEntity>(Common.DotNetToOracle(salaryRank.SalaryClassID.ToString()), ConstantSql.hrm_cat_sp_get_AbilityTileBySalaryClassId, userLogin, ref status).FirstOrDefault();
                    if (abilityTitleBySalaryClass != null)
                    {
                        _AbilitiTitleID = abilityTitleBySalaryClass.ID;
                    }
                }

                // theo task 0049950 - không làm gì liên quan đến hđ hết.
                //var contractybyProfile = service.GetData<Hre_ContractEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_ContractsByProfileId, userLogin, ref status)
                //    .OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                ////if (contractybyProfile == null)
                ////{
                ////    result = ConstantDisplay.HRM_Common_ProfileNotHaveContract.ToString();
                ////    return result;
                ////}
                ////if (contractybyProfile != null && contractybyProfile.ContractNextID == null)
                ////{
                ////    result = ConstantDisplay.HRM_Common_ProfileNotHaveNextContract.ToString();
                ////    return result;
                ////}
                //if (contractybyProfile != null && contractybyProfile.ContractNextID != null)
                //{
                    //var contractype = service.GetData<Cat_ContractTypeEntity>(Guid.Parse(Common.DotNetToOracle(contractybyProfile.ContractNextID.ToString())), ConstantSql.hrm_cat_sp_get_ContractTypeById, userLogin, ref status).FirstOrDefault();
                    //if (contractype == null)
                    //{
                    //    result = ConstantDisplay.HRM_Common_ProfileNotHaveNextContract.ToString();
                    //    return result;
                    //}
                    //Hre_ContractEntity contract = new Hre_ContractEntity();
                    //contract.ProfileID = profile.ID;
                    //contract.DateStart = dateEffect.Value;
                    //contract.DateSigned = dateEffect;
                    //contract.RankRateID = rankdetailID;
                    //contract.ClassRateID = salaryRank != null ? salaryRank.SalaryClassID : null;
                    //contract.Salary = salaryRank != null ? salaryRank.SalaryStandard : null;
                    //contract.ContractTypeID = contractype.ID;
                    //if (!string.IsNullOrEmpty(contractype.Formula))
                    //{
                    //    contract = SetNewDateEndContract(contract,userLogin);
                    //}

                    //if (contractybyProfile.DateEndNextContract != null)
                    //{
                    //    contractybyProfile.DateEnd = contractybyProfile.DateEndNextContract.Value;
                    //}
                    //contractservices.Add(contract);

                    // Thêm mới lương cơ bản + lương BHXH
                    string defaultcur = null;
                    string ConfigHre = AppConfig.HRM_HRE_CONFIG.ToString();
                    List<object> lstOb = new List<object>();
                    lstOb.Add(ConfigHre);
                    lstOb.Add(null);
                    lstOb.Add(null);
                    var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting, userLogin, ref status);
                    string valueConfig = AppConfig.HRM_HRE_CONFIG_DEFAULTCURRENCY.ToString();
                    if (config != null)
                    {
                        defaultcur = config.Where(s => s.Name == valueConfig).Select(s => s.Value1).FirstOrDefault().ToString();
                        List<object> listObjCurrency = new List<object>();
                        listObjCurrency.Add(null);
                        listObjCurrency.Add(defaultcur);
                        listObjCurrency.Add(1);
                        listObjCurrency.Add(Int32.MaxValue - 1);
                        var lstCurrency = service.GetData<Cat_CurrencyEntity>(listObjCurrency, ConstantSql.hrm_cat_sp_get_Currency, userLogin, ref status).FirstOrDefault();

                        var basicsalarybyprofile = service.GetData<Sal_BasicSalaryEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_sal_sp_get_BasicSalaryByProfileIds, userLogin, ref status)
                            .OrderByDescending(s => s.DateOfEffect).FirstOrDefault();
                        var insuransalarybyprofile = service.GetData<Sal_InsuranceSalaryEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_sal_sp_get_InsuranceSalaryByProfileIds, userLogin, ref status)
                     .OrderByDescending(s => s.DateEffect).FirstOrDefault();

                        if (basicsalarybyprofile != null && salaryRank != null && basicsalarybyprofile.RankRateID != rankdetailID && salaryRank.SalaryClassID == basicsalarybyprofile.ClassRateID)
                        {
                            Sal_BasicSalaryEntity basicSalary = new Sal_BasicSalaryEntity();
                            basicSalary.ProfileID = profile.ID;
                            basicSalary.GrossAmount = (salaryRank != null && salaryRank.SalaryStandard != null) ? salaryRank.SalaryStandard.Value.ToString() : null;
                            basicSalary.Amount = basicSalary.GrossAmount != null ? basicSalary.GrossAmount.Encrypt() : "";
                            basicSalary.InsuranceAmount = (salaryRank != null && salaryRank.SalaryStandard != null) ? salaryRank.SalaryStandard.Value : 0;
                            basicSalary.DateOfEffect = dateEffect.Value;
                            basicSalary.CurrencyID = lstCurrency.ID;
                            basicSalary.Status = EnumDropDown.Sal_BasicSalaryStatus.E_WATTING_APPROVED.ToString();
                            basicSalary.AmountTotal = salaryRank != null ? salaryRank.SalaryStandard : null;
                            basicSalary.ClassRateID = salaryRank.SalaryClassID;
                            basicSalary.RankRateID = salaryRank.ID;
                            basicSalaryServices.Add(basicSalary);

                            if(insuransalarybyprofile != null)
                            {
                                Sal_InsuranceSalaryEntity insuranceSalary = new Sal_InsuranceSalaryEntity();
                                insuranceSalary.ProfileID = profile.ID;
                                insuranceSalary.InsuranceAmount = salaryRank != null ? salaryRank.SalaryStandard : null;
                                insuranceSalary.DateEffect = dateEffect;
                                insuranceSalary.CurrencyID = lstCurrency.ID;
                                insuranceSalary.IsSocialIns = insuransalarybyprofile.IsSocialIns;
                                insuranceSalary.IsMedicalIns = insuransalarybyprofile.IsMedicalIns;
                                insuranceSalary.IsUnimploymentIns = insuransalarybyprofile.IsUnimploymentIns;
                                InsuranceSalaryServices.Add(insuranceSalary);
                            }
                          
                        }
                        else
                        {
                            profile.SalaryClassID = salaryRank == null ? Guid.Empty : salaryRank.SalaryClassID;
                            profile.AbilityTileID = _AbilitiTitleID;
                            profile.DateOfEffect = dateEffect;
                            service.Edit(profile);

                            var workHistoryEntity = service.GetData<Hre_WorkHistoryEntity>(Common.DotNetToOracle(profileID.ToString()), ConstantSql.hrm_hr_sp_get_WorkHistoryByProfileId, userLogin, ref status)
                                .OrderByDescending(s => s.DateEffective).FirstOrDefault();


                            if (workHistoryEntity == null || workHistoryEntity.DateEffective != dateEffect)
                            {
                                var workHistory = new Hre_WorkHistoryEntity();
                                workHistory.ProfileID = profile.ID;
                                workHistory.SalaryClassID = salaryRank != null ? salaryRank.SalaryClassID : null;
                                workHistory.DateEffective = dateEffect != null ? dateEffect.Value : DateTime.Now;
                                workHistory.AbilityTileID = _AbilitiTitleID;
                                service.Add(workHistory);
                            }
                            else
                            {
                                workHistoryEntity.SalaryClassID = salaryRank != null ? salaryRank.SalaryClassID : null;
                                workHistoryEntity.DateEffective = dateEffect != null ? dateEffect.Value : DateTime.Now;
                                workHistoryEntity.AbilityTileID = _AbilitiTitleID;
                                service.Edit(workHistoryEntity);
                            }

                            Sal_BasicSalaryEntity basicSalary = new Sal_BasicSalaryEntity();
                            basicSalary.ProfileID = profile.ID;
                            basicSalary.GrossAmount = (salaryRank != null && salaryRank.SalaryStandard != null) ? salaryRank.SalaryStandard.Value.ToString() : null;
                            basicSalary.Amount = basicSalary.GrossAmount != null ? basicSalary.GrossAmount.Encrypt() : "";
                            basicSalary.InsuranceAmount = (salaryRank != null && salaryRank.SalaryStandard != null) ? salaryRank.SalaryStandard.Value : 0;
                            basicSalary.AmountTotal = salaryRank != null ? salaryRank.SalaryStandard : null;
                            basicSalary.DateOfEffect = dateEffect.Value;
                            basicSalary.CurrencyID = lstCurrency.ID;
                            basicSalary.Status = EnumDropDown.Sal_BasicSalaryStatus.E_WATTING_APPROVED.ToString();
                            basicSalaryServices.Add(basicSalary);

                            if (insuransalarybyprofile != null)
                            {
                                Sal_InsuranceSalaryEntity insuranceSalary = new Sal_InsuranceSalaryEntity();
                                insuranceSalary.ProfileID = profile.ID;
                                insuranceSalary.InsuranceAmount = salaryRank != null ? salaryRank.SalaryStandard : null;
                                insuranceSalary.DateEffect = dateEffect;
                                insuranceSalary.CurrencyID = lstCurrency.ID;
                                insuranceSalary.IsSocialIns = insuransalarybyprofile.IsSocialIns;
                                insuranceSalary.IsMedicalIns = insuransalarybyprofile.IsMedicalIns;
                                insuranceSalary.IsUnimploymentIns = insuransalarybyprofile.IsUnimploymentIns;
                                InsuranceSalaryServices.Add(insuranceSalary);
                            }
                        }
                }
                return result;
            }
        }

        public Hre_ContractEntity SetNewDateEndContract(Hre_ContractEntity contractEntity,string userLogin)
        {
            if (contractEntity != null)
            {
                var service = new BaseService();
                var status = string.Empty;
                var objContractType = service.GetFirstData<Cat_ContractTypeEntity>(contractEntity.ContractTypeID, ConstantSql.hrm_cat_sp_get_ContractTypeById, userLogin, ref status);
                if (objContractType != null)
                {
                    var contractTypeEntity = (Cat_ContractTypeEntity)objContractType;
                    if (!string.IsNullOrEmpty(contractTypeEntity.Formula))
                    {
                        var formula = contractTypeEntity.Formula.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        ElementFormula elementContactType = new ElementFormula("ContractType", contractTypeEntity.Type, 0);
                        ElementFormula elementUnitType = new ElementFormula("UnitType", contractTypeEntity.UnitTime, 0);
                        ElementFormula elementValueTime = new ElementFormula("ValueTime", contractTypeEntity.ValueTime, 0);
                        ElementFormula elementDateStart = new ElementFormula("DateStart", contractEntity.DateStart, 0);

                        var result = FormulaHelper.ParseFormula(formula, new List<ElementFormula>() { elementContactType, elementUnitType, elementDateStart, elementValueTime });

                        if (result != null && string.IsNullOrEmpty(result.ErrorMessage))
                        {
                            if (result.Value.GetType().Name == "DateTime")
                            {
                                contractEntity.DateEnd = (DateTime)result.Value;
                            }
                            else
                            {
                                contractEntity.ErrorMessage += contractEntity.ProfileName + ",";
                                contractEntity.ActionStatus = ConstantMessages.WrongFormulaInContractType.ToString();
                            }

                        }
                    }
                }

            }
            return contractEntity;
        }

        /// <summary>
        /// Approve chốt đánh giá
        /// </summary>
        /// <param name="performanceId"></param>
        public void ApproveEvaPerformance(Guid performanceId)
        {
            /*
            *  Goal(Chốt Đánh Giá)
            *  Steps :
            *      Step1  :  tính Điểm
            *      Step2  :  Chốt Đánh Giá (set status la E_APPROVE_END)
            */


            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPerformance = new Eva_PerformanceRepository(unitOfWork);
                var performance = repoPerformance.GetById(performanceId);
                if (performance != null)
                {                   
                    var performanceEva = unitOfWork.CreateQueryable<Eva_PerformanceEva>(Guid.Empty, m => m.PerformanceID == performanceId).ToList();
                    var evaluatorIDs = performanceEva.Select(m => m.EvaluatorID).ToList();
                    var profileID = performance.ProfileID;
                    var performanceTypeId = performance.PerformanceTypeID;
                    var performanceTemplateId = performance.PerformanceTemplateID;
                    var evaluatorDic = new Dictionary<Guid,double>();
                    var evaluators = unitOfWork.CreateQueryable<Eva_Evaluator>(Guid.Empty, m => evaluatorIDs.Contains(m.EvaluatorID)
                        && m.ProfileID == profileID && m.PerformanceTypeID == performanceTypeId 
                       /* && m.PerformanceTemplateID == performanceTemplateId*/).ToList();
                    double totalMarkFinalEvaluators = 0;
                    double totalRate = 0;
                    foreach (var item in performanceEva)
                    {
                        double totalMarkPerEvaluator = 0;
                        var evaluatorRate = evaluators.Where(m => m.Rate != null && m.Rate.Value >0 &&  m.EvaluatorID == item.EvaluatorID).Select(m=>m.Rate.Value).FirstOrDefault();
                        if (evaluatorRate != 0 && item.TotalMark.HasValue && item.TotalMark.Value>0)
                        {
                            totalRate += evaluatorRate;
                            var totalMark = item.TotalMark??0;
                            totalMarkPerEvaluator = totalMark * evaluatorRate;
                            totalMarkFinalEvaluators += totalMarkPerEvaluator;
                        }
                    }
                    //Tung ly loi float va double
                    performance.TotalMark = (float) (totalMarkFinalEvaluators / totalRate);
                    //set status=E_APPROVE_END (chốt đánh giá)
                    performance.Status = PerformaceEvaStatus.E_APPROVE_END.ToString();
                    repoPerformance.SaveChanges();
                    
                }
            }
        }

        #region Performance360
        public Eva_PerformanceEntity AddPerfomance360(Eva_PerformanceEntity model, bool isSelft)
        {
            /*
            *  Goal(Luu đánh giá nhân viên)
            *  Steps :
            *      Step1  :  Insert Eva_Performance
            *      Step2  :  Tự động insert xuống table Eva_PerformanceForDetail
            *      Step3  :  Tự động insert xuống table Eva_PerformanceEva &Eva_PerformanceEvaDetail 
            *                  - Tự Đánh Giá : orderNo =0 (Ngoai ra orderNo >0)
             
            */


            var evaluationStatus = PerformaceEvaStatus.E_WAITING.ToString();
            var result = new Eva_PerformanceEntity();
            var performanceForDetail = new Eva_PerformanceForDetail();
            var performanceEva = new Eva_PerformanceEva();
            var performanceEvaDetail = new Eva_PerformanceEvaDetail();

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPerformance = new Eva_PerformanceRepository(unitOfWork);
                var repoEvaluator = new Eva_EvaluatorRepository(unitOfWork);
                var repoPerformanceForDetail = new Eva_PerformanceForDetailRepository(unitOfWork);
                var repoPerformanceEva = new Eva_PerformanceEvaRepository(unitOfWork);
                var repoPerformanceEvaDetail = new Eva_PerformanceEvaDetailRepository(unitOfWork);

                #region step1 : thêm Eva_Performance
                result = model;
                if (result != null && result.ID == Guid.Empty)
                {
                    result.Status = evaluationStatus;
                }
                Add(result);
                #endregion

                #region Step2  :  Tự động insert xuống table Eva_PerformanceForDetail dựa vào các KPI tương ứng

                //lay ds KPIs (kèm theo Marks và Evaluation) - lay dữ liệu từ edit trên lưới
                var kpis = model.KPIs;//cac tieu chi danh gia
                var kpiByTempalateIds = kpis;

                var lstPerformanceForDetails = new List<Eva_PerformanceForDetail>();
                foreach (var kpiByTempalateId in kpiByTempalateIds)
                {
                    performanceForDetail = new Eva_PerformanceForDetail();
                    performanceForDetail.ID = Guid.NewGuid();
                    if (result != null)
                    {
                        performanceForDetail.PerformanceID = result.ID;
                    }
                    performanceForDetail.KPIID = kpiByTempalateId.ID;
                    performanceForDetail.KPIName = kpiByTempalateId.KPIName;
                    performanceForDetail.OrderKPI = kpiByTempalateId.OrderNumber;
                    performanceForDetail.OrderNumber = kpiByTempalateId.OrderNumber;
                    performanceForDetail.Rate = kpiByTempalateId.Rate;
                    lstPerformanceForDetails.Add(performanceForDetail);
                    repoPerformanceForDetail.Add(performanceForDetail);//add vao repo
                }
                repoPerformanceForDetail.SaveChanges();
                #endregion

                #region Step3  :  Tự động insert xuống table Eva_PerformanceEva &Eva_PerformanceEvaDetail

                #region Luu bảng Eva_PerformanceEva (cho từng người đánh giá)

                var lstPerformanceEvaIDs = new List<Guid>();
                int orderNo = 1;
                var performanceEvaSeftId = Guid.Empty;//ID cua bang Eva_PerformanceEva

                #region Tự đánh giá : tự động add 1 người đánh giá là chính bảng thân người tạo bảng đánh giá
                if (isSelft && result != null && model != null && model.ProfileID != Guid.Empty)
                {
                    //tu tao nguoi danh gia la ban than
                    performanceEva = new Eva_PerformanceEva();
                    performanceEva.ID = Guid.NewGuid();
                    performanceEvaSeftId = performanceEva.ID;
                    lstPerformanceEvaIDs.Add(performanceEva.ID);
                    performanceEva.PerformanceID = result.ID;
                    performanceEva.EvaluatorID = model.ProfileID;
                    performanceEva.Status = PerformaceEvaStatus.E_SEFT.ToString();//evaluationStatus;
                    performanceEva.OrderEva = 0;
                    repoPerformanceEva.Add(performanceEva);

                    //cap nhat lai status cua Eva_Performance
                    var performanceEntity = repoPerformance.GetById(result.ID);
                    if (performanceEntity != null)
                    {
                        performanceEntity.Status = PerformaceEvaStatus.E_SEFT.ToString();
                        repoPerformance.SaveChanges();
                    }
                }

                #endregion

                //Lấy ds người đánh giá theo NV và loại đánh giá
                var evaluatorIds = model.EvaluatorIDs;
                var evaluatorObjs = repoEvaluator.FindBy(p => p.IsDelete == null && evaluatorIds.Contains(p.EvaluatorID ?? Guid.Empty)
                    && p.ProfileID == model.ProfileID
                    && p.PerformanceTypeID == model.PerformanceTypeID).ToList();
                evaluatorObjs = evaluatorObjs.OrderBy(p => p.OrderNo).ToList();

                foreach (var evaluator in evaluatorObjs)
                {
                    performanceEva = new Eva_PerformanceEva();
                    performanceEva.ID = Guid.NewGuid();
                    lstPerformanceEvaIDs.Add(performanceEva.ID);
                    if (result != null)
                    {
                        performanceEva.PerformanceID = result.ID;
                    }
                    performanceEva.EvaluatorID = evaluator.EvaluatorID;
                    performanceEva.Status = evaluationStatus;
                    performanceEva.OrderEva = orderNo++;
                    repoPerformanceEva.Add(performanceEva);
                }
                repoPerformanceEva.SaveChanges();
                #endregion
                              
                #region comment Luu Eva_PerformanceEvaDetail (lưu chi tiết cho bảng đánh giá)

                //var isSeft = false;
                ////khởi tạo chi tiet đánh giá cho từng người đánh giá
                //foreach (var performanceEvaID in lstPerformanceEvaIDs)
                //{
                //    //ds cac tieu chí
                //    foreach (var performanceForDetailItem in lstPerformanceForDetails)
                //    {
                //        performanceEvaDetail = new Eva_PerformanceEvaDetail
                //        {
                //            ID = Guid.NewGuid(),
                //            PerfomanceEvaID = performanceEvaID,
                //            PerfomanceDetailID = performanceForDetailItem.ID,
                //            KPIName = performanceForDetailItem.KPIName
                //        };

                //        #region Dành Cho Tự Đánh Giá

                //        if (performanceEvaSeftId != Guid.Empty && performanceEvaSeftId == performanceEvaID)
                //        {
                //            /*Add Mark va Evaluation cho nguoi danh gia la ban than*/
                //            //lay kpi theo name tu model de set data cho Mark va Evaluation

                //            var kpi = kpiByTempalateIds.FirstOrDefault(p => p.KPIName == performanceEvaDetail.KPIName);
                //            if (kpi != null)
                //            {
                //                performanceEvaDetail.Mark = kpi.Mark;
                //                performanceEvaDetail.Evaluation = kpi.Evaluation;
                //            }
                //            //đặt flag là tự đánh giá
                //            isSeft = true;
                //        }
                //        repoPerformanceEvaDetail.Add(performanceEvaDetail);
                //        #endregion
                //    }

                //    if (isSeft)
                //    {
                //        performanceEvaSeftId = Guid.Empty;
                //    }

                //}
                //repoPerformanceEvaDetail.SaveChanges();

                #endregion

                #endregion

            }
            return result;
        }

        /// <summary>
        /// truoc khi vào màn hình chờ đánh giá , sẽ thêm vào bảng eva_performanceEvaDetail nếu bảng này chưa có data ứng với performanceID
        /// </summary>
        /// <param name="performanceEvaID"></param>
        /// <returns></returns>
        public bool AddPerformanceEvaDetail360(Guid performanceEvaID) {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPerformance = new Eva_PerformanceRepository(unitOfWork);
                var repoEvaluator = new Eva_EvaluatorRepository(unitOfWork);
                var repoPerformanceForDetail = new Eva_PerformanceForDetailRepository(unitOfWork);
                var repoPerformanceEva = new Eva_PerformanceEvaRepository(unitOfWork);
                var repoPerformanceEvaDetail = new Eva_PerformanceEvaDetailRepository(unitOfWork);
                var performanceEvaDetail = new Eva_PerformanceEvaDetail();

                #region Lay DS EvaPerformanceEvaDetail
                //neu co du lieu => khong tao moi 
                var lstPerformanceEvaDetail = unitOfWork.CreateQueryable<Eva_PerformanceEvaDetail>(Guid.Empty, m => m.PerfomanceEvaID == performanceEvaID).ToList();
                if (lstPerformanceEvaDetail.Any())
                {
                    return false;
                }
                #endregion
                   
                //lay performanceID
                var performanceEva = unitOfWork.CreateQueryable<Eva_PerformanceEva>(Guid.Empty, m => m.ID == performanceEvaID).FirstOrDefault();
                var performanceID = Guid.Empty;
                if (performanceEva != null && performanceEva.PerformanceID.HasValue)
                {
                    performanceID = performanceEva.PerformanceID.Value;
                }
                else
                {
                    return false;
                }

                #region lay ds eva_performancefordetail theo performanceID
                var lstPerformanceForDetails = unitOfWork.CreateQueryable<Eva_PerformanceForDetail>(Guid.Empty, m => m.PerformanceID == performanceID).ToList();
                #endregion
                                
                #region comment Luu Eva_PerformanceEvaDetail (lưu chi tiết cho bảng đánh giá)

                //khởi tạo chi tiet đánh giá cho người đánh giá
                if (performanceEvaID != Guid.Empty)
                {
                    //ds cac tieu chí
                    foreach (var performanceForDetailItem in lstPerformanceForDetails)
                    {
                        performanceEvaDetail = new Eva_PerformanceEvaDetail
                        {
                            ID = Guid.NewGuid(),
                            PerfomanceEvaID = performanceEvaID,
                            PerfomanceDetailID = performanceForDetailItem.ID,
                            KPIName = performanceForDetailItem.KPIName
                        };
                        repoPerformanceEvaDetail.Add(performanceEvaDetail);                       
                    }
                }
                repoPerformanceEvaDetail.SaveChanges();
                #endregion
            }
            return false;
        }
        #endregion

    }
}
