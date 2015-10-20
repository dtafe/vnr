using System;
using System.Collections.Generic;
using HRM.Business.Evaluation.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Infrastructure.Utilities;
using System.Linq;
using VnResource.Helper.Data;
using System.Data;
using HRM.Infrastructure.Utilities.Helper;

namespace HRM.Business.Evaluation.Domain
{
    public class Eva_PerformanceEvaServices : BaseService
    {
        public Eva_PerformanceEvaEntity EditPerformanceEva(Eva_PerformanceEvaEntity model)
        {
            var Performance = new Eva_Performance();
            var PerformanceEva = new Eva_PerformanceEva();
            List<Guid> ilID = model.PerformanceEvaDetails.Select(x => x.ID).ToList();



            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoP = new Eva_PerformanceRepository(unitOfWork);
                var repoPE = new Eva_PerformanceEvaRepository(unitOfWork);
                var repoPED = new Eva_PerformanceEvaDetailRepository(unitOfWork);
                PerformanceEva = repoPE.GetById(model.ID);
                //cập nhật đánh giá của người đánh giá Eva_PerformanceEva
                if (PerformanceEva != null)
                {
                    //PerformanceEva.AttachFile = model.AttachFile.Substring(0, model.AttachFile.LastIndexOf("_"));
                    PerformanceEva.AttachFile = model.AttachFile;
                    if (CheckOrderEnd(PerformanceEva) == true)
                        PerformanceEva.Status = PerformaceEvaStatus.E_APPROVE_END.ToString();
                    else
                        PerformanceEva.Status = "E_APPROVE_" + PerformanceEva.OrderEva.ToString();
                    PerformanceEva.Strengths = model.Strengths;
                    PerformanceEva.Weaknesses = model.Weaknesses;
                    PerformanceEva.LevelID = model.LevelID;
                    PerformanceEva.ResultNote = model.ResultNote;
                    PerformanceEva.Proportion = model.Proportion;
                    PerformanceEva.TotalMark = model.TotalMark;
                }
                repoPE.SaveChanges();
                //Cập nhật ngược status và Level, Strengths, Weakness, Resultnote cho Eva_Performance theo status Eva_PerformanceEva
                Performance = repoP.GetById(PerformanceEva.PerformanceID ?? Guid.Empty);
                if (Performance != null)
                {
                    Performance.Status = PerformanceEva.Status;
                    if (PerformanceEva.LevelID.HasValue)
                        Performance.LevelID = PerformanceEva.LevelID;
                    if (!string.IsNullOrEmpty(PerformanceEva.Strengths))
                        Performance.Strengths = PerformanceEva.Strengths;
                    if (!string.IsNullOrEmpty(PerformanceEva.Weaknesses))
                        Performance.Weaknesses = PerformanceEva.Weaknesses;
                    if (!string.IsNullOrEmpty(PerformanceEva.ResultNote))
                        Performance.ResultNote = PerformanceEva.ResultNote;
                    repoP.SaveChanges();

                }
                //cập nhật average cho Eva_Performance khi có người đánh giá

                var ilPerformanceEva = repoPE.FindBy(x => x.IsDelete == null && x.OrderEva <= PerformanceEva.OrderEva && x.OrderEva > 0 && x.TotalMark.HasValue).ToList();
                double? totalmark = ilPerformanceEva.Sum(x => x.TotalMark) / ilPerformanceEva.Count();

                float _totalMark = 0;
                float.TryParse(Math.Round(totalmark ?? 0, 2).ToString(), out _totalMark);
                Performance.TotalMark = _totalMark;  //float.Parse(Math.Round(totalmark??0,2).ToString());
                repoP.SaveChanges();




                //cập nhật chi tiết người đánh giá Eva_PerformanceEvaDetail


                Eva_PerformanceEvaDetail objPerformanceEvaDetail = null;
                List<Eva_PerformanceEvaDetail> ilPerformanceEvaDetail = repoPED.FindBy(x => ilID.Contains(x.ID)).ToList();
                var ilobj = model.PerformanceEvaDetails.ToList();
                foreach (var obj in ilobj)
                {
                    objPerformanceEvaDetail = repoPED.GetById(obj.ID);
                    objPerformanceEvaDetail.KPIName = obj.KPIName;
                    objPerformanceEvaDetail.Evaluation = obj.Evaluation;
                    if (obj.Mark.HasValue)
                        objPerformanceEvaDetail.Mark = obj.Mark;
                }
                repoPED.SaveChanges();





            }

            return model;

        }
        //kiểm tra có phải là người cuối cùng đánh giá ko?
        public bool CheckOrderEnd(Eva_PerformanceEva obj)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPE = new Eva_PerformanceEvaRepository(unitOfWork);
                var objMax = repoPE.FindBy(x => x.PerformanceID == obj.PerformanceID).Where(x => x.IsDelete == null).Max(x => x.OrderEva);
                if (objMax == obj.OrderEva)
                    return true;
            }
            return false;
        }
        public string GetAttachFile(Guid PerformanceID)
        {
            string str = "";
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoP = new Eva_PerformanceRepository(unitOfWork);
                var repoPE = new Eva_PerformanceEvaRepository(unitOfWork);
                Eva_Performance Performance = repoP.GetById(PerformanceID);
                if (Performance != null && !string.IsNullOrEmpty(Performance.AttachFile) && Performance.IsDelete == null)
                {
                    str = Performance.AttachFile;
                }

                List<Eva_PerformanceEva> ilEva_PerformanceEva = repoPE.FindBy(x => x.PerformanceID == PerformanceID && x.IsDelete == null).ToList();
                List<string> lstAttachFiles = ilEva_PerformanceEva.OrderByDescending(p => p.AttachFile).Select(p => p.AttachFile).ToList();
                lstAttachFiles.Add(str);
                lstAttachFiles = lstAttachFiles.OrderByDescending(p => p).ToList();
                return lstAttachFiles.FirstOrDefault();
            }
        }
        // kiểm tra đánh giá vượt cấp
        public bool CheckOrder(Guid PerformanceID, Guid ID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoPE = new Eva_PerformanceEvaRepository(unitOfWork);
                var PerformanceEva = repoPE.GetById(ID);


                List<Eva_PerformanceEva> ilEva_PerformanceEva = repoPE.FindBy(x => x.PerformanceID == PerformanceID).Where(x => x.IsDelete == null).ToList();
                if (ilEva_PerformanceEva == null || ilEva_PerformanceEva.Count == 0)
                {
                    return false;
                }
                else
                {
                    ilEva_PerformanceEva = ilEva_PerformanceEva.Where(x => x.OrderEva > 0).OrderBy(x => x.OrderEva).ToList();
                    int Count = ilEva_PerformanceEva.Count();
                    //nếu đánh giá hiện tại là đầu tiên thì thỏa
                    if (PerformanceEva == ilEva_PerformanceEva[0])
                    {
                        return true;
                    }
                    else
                        // chạy từ 1 vì bỏ qua người đầu vì đã được kt ở trước
                        for (int i = 1; i < Count; i++)
                        {

                            if (ilEva_PerformanceEva[i] == PerformanceEva && PerformanceEva.OrderEva != 0)
                            {

                                if (ilEva_PerformanceEva[i - 1].Status != PerformaceEvaStatus.E_WAITING.ToString() || ilEva_PerformanceEva[i - 1].Status == PerformaceEvaStatus.E_SEFT.ToString())
                                    return true;
                            }
                        }
                }
            }


            return false;
        }

        /// <summary> Kiểm tra cấp sau đã đánh giá chưa? </summary>
        /// <param name="PerformanceID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckSuperiorHasPerformance(Guid PerformanceID, Guid ID)
        {
            /*
            *  Goal(Kiểm tra cấp sau đã đánh giá chưa [Bảng Eva_PerformanceEva])
            *  Input : PerformanceId , ID
            *  OutPut: true(cấp sau đã đánh giá) , false(cấp sau chưa đánh giá) 
            *  Steps :
            *      Step1    :  Nếu là cấp cuối => false(cấp sau chưa đánh giá)
            *      Step2    :  Lấy ds nguoi đánh giá (bảng Eva_PerformanceEva)
            *      Step3    :  Neu ds chỉ có 1 người đánh giá => false (cấp sau chưa đánh giá)
            *      Step4    :  Duyệt người đánh giá 
            *      Step4.1  :  kt từng người đánh giá có trạng thái "chờ" hoặc "Tự đánh giá" => false(cấp sau chưa đánh giá)    
            */
            
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoPE = new Eva_PerformanceEvaRepository(unitOfWork);
                var PerformanceEva = repoPE.GetById(ID);
                //nếu là người đánh giá cuối cùng thì cấp sau chưa đánh giá
                if (PerformanceEva.Status == PerformaceEvaStatus.E_APPROVE_END.ToString())
                {
                    //cấp sau chưa đánh giá
                    return false;
                }
                List<Eva_PerformanceEva> ilEva_PerformanceEva = repoPE.FindBy(x => x.PerformanceID == PerformanceID).Where(x => x.IsDelete == null).ToList();
                if (ilEva_PerformanceEva == null || ilEva_PerformanceEva.Count == 0)
                {
                    //true(cấp sau đã đánh giá)
                    return true;
                }
                else
                {
                    ilEva_PerformanceEva = ilEva_PerformanceEva.Where(x => x.OrderEva > 0).OrderBy(x => x.OrderEva).ToList();
                    int Count = ilEva_PerformanceEva.Count();
                    //nếu có 1 người đánh giá thì cấp sau chưa đánh giá
                    if (Count == 1)
                    {
                        //cấp sau chưa đánh giá
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < Count; i++)
                        {
                            if (i < Count - 1 && ilEva_PerformanceEva[i] == PerformanceEva)
                            {
                                //kiểm tra trạng thái người đánh giá sau là "Chờ" hoặc "Tự Đánh Giá"
                                if (ilEva_PerformanceEva[i + 1].Status == PerformaceEvaStatus.E_WAITING.ToString() 
                                    || ilEva_PerformanceEva[i + 1].Status == PerformaceEvaStatus.E_SEFT.ToString())
                                {
                                    //cấp sau chưa đánh giá
                                    return false;
                                }
                            }

                        }
                    }
                }
            }


            return true;
        }

        public List<Eva_PerformanceEva> GetlistPerformanceEva(Guid PerformanceID, Guid PerformanceEvaID)
        {
            int Rs = 0;
            List<Eva_PerformanceEva> ilEva_PerformanceEva;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPE = new Eva_PerformanceEvaRepository(unitOfWork);
                var PerformanceEva = repoPE.GetById(PerformanceEvaID);
                ilEva_PerformanceEva = repoPE.FindBy(x => x.PerformanceID == PerformanceID).Where(x => x.IsDelete == null).ToList();
                if (ilEva_PerformanceEva != null && ilEva_PerformanceEva.Count != 0)
                {
                    ilEva_PerformanceEva = ilEva_PerformanceEva.OrderByDescending(x => x.OrderEva).ToList();
                    ilEva_PerformanceEva = ilEva_PerformanceEva.Where(x => x.OrderEva < PerformanceEva.OrderEva && x.OrderEva > 0).ToList();

                }
            }
            return ilEva_PerformanceEva;


        }
        public List<Guid> CountKPI(Guid PerformanceID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoPE = new Eva_PerformanceForDetailRepository(unitOfWork);
                List<Guid> ilEva_PerformanceEva = repoPE.FindBy(x => x.IsDelete == null && x.PerformanceID == PerformanceID).OrderBy(p=>p.KPIName).Select(x => x.KPIID ?? Guid.Empty).ToList();
                if (ilEva_PerformanceEva != null && ilEva_PerformanceEva.Count != 0)
                {
                    return ilEva_PerformanceEva;

                }
                return null;

            }
        }

        public DataTable DtPerformEvaDetail;
        public DataTable GetPerformEvaDetail(Guid PerformanceID, Guid PerformanceEvaID, int OrderEva,string userLogin)
        {
            DtPerformEvaDetail = new DataTable();
            Eva_PerformanceEvaServices service1 = new Eva_PerformanceEvaServices();
            var Columns = service1.GetlistPerformanceEva(PerformanceID, PerformanceEvaID).ToList();
            if (Columns == null || Columns.Count == 0)
                return null;
            //cot nv
            //  DtPerformEvaDetail.Columns.Add(new DataColumn(ConstantDisplay.HRM_Evaluation_Evaluator.TranslateString()));
            //cot tieu chi
            DtPerformEvaDetail.Columns.Add(new DataColumn(ConstantDisplay.HRM_Eva_PerformanceEvaWaitingEva_KPIName.TranslateString()));
            //cot Mo ta
            DtPerformEvaDetail.Columns.Add(new DataColumn(ConstantDisplay.HRM_Eva_PerformanceEvaWaitingEva_DescriptionKP.TranslateString()));

            //cot Màu chữ KPI
            DtPerformEvaDetail.Columns.Add(new DataColumn("KPIColor"));

            foreach (var i in Columns)
            {
                DtPerformEvaDetail.Columns.Add(new DataColumn(ConstantDisplay.HRM_Eva_PerformanceEvaWaitingEva_Evaluation.TranslateString() + " " + i.OrderEva));
                DtPerformEvaDetail.Columns.Add(new DataColumn(ConstantDisplay.HRM_Eva_PerformanceEvaWaitingEva_Mark.TranslateString() + " " + i.OrderEva));
            }

            DataRow Row;
            try
            {
                string status = string.Empty;
                var baseService = new BaseService();
                var objs = new List<object>();
                objs.Add(PerformanceID);
                objs.Add(OrderEva);
                objs.Add(1);
                objs.Add(2000);

                var result = baseService.GetData<Eva_PerformanceEvaDetailEntity>(objs, ConstantSql.hrm_eva_sp_get_PerformanceEvaDetailAllSelfByPerformanceEvaID,userLogin, ref status);
                if (result == null || result.Count == 0)
                    return null;



                List<int> listOrderEva = result.OrderByDescending(x => x.OrderEva).Select(x => x.OrderEva.Value).Distinct().ToList();
                var result1 = new List<Eva_PerformanceEvaDetailEntity>();

                foreach (var y in CountKPI(PerformanceID))
                {
                    Row = DtPerformEvaDetail.NewRow();
                    result1 = result.Where(x => x.KPIID == y).ToList();
                    if (result1 != null && result1.Count != 0)
                    {

                        Row[ConstantDisplay.HRM_Eva_PerformanceEvaWaitingEva_KPIName.TranslateString()] = result1.FirstOrDefault().KPIName;
                        Row[ConstantDisplay.HRM_Eva_PerformanceEvaWaitingEva_DescriptionKP.TranslateString()] = result1.FirstOrDefault().DescriptionKP;
                        Row["KPIColor"] = result1.FirstOrDefault().KPIColor;

                        string Evaluator = "";
                        foreach (var x in result1)
                        {

                            foreach (var k in listOrderEva)
                            {
                                if (k == x.OrderEva)
                                {
                                    Evaluator += x.Evaluator + "; ";
                                    Row[ConstantDisplay.HRM_Eva_PerformanceEvaWaitingEva_Evaluation.TranslateString() + " " + k] = x.Evaluation;
                                    Row[ConstantDisplay.HRM_Eva_PerformanceEvaWaitingEva_Mark.TranslateString() + " " + k] = x.Mark;
                                }
                            }

                        }
                        //  Evaluator = Evaluator.Substring(0, Evaluator.Length - 2);
                        //   Row[ConstantDisplay.HRM_Evaluation_Evaluator.TranslateString()] = Evaluator;
                    }
                    DtPerformEvaDetail.Rows.Add(Row);

                }


            }
            catch
            {
                return null;
            }

            var configs = new Dictionary<string, Dictionary<string, object>>();
            var config = new Dictionary<string, object>();
            config.Add("hidden", true);
            if (!configs.ContainsKey("KPIColor"))
            {
                configs.Add("KPIColor", config);
            }
            return DtPerformEvaDetail.ConfigTable(configs);

        }
    }
}
