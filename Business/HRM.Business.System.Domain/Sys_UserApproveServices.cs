using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using System.Linq;
using System;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using HRM.Business.Hr.Models;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Category.Models;
using HRM.Business.Finance.Models;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_UserApproveServices : BaseService
    {
        public List<Sys_UserApproveEntity> GetUserApprovedForMulti_E_FIN_PurchaseRequest(Guid recordID)
        {
            List<Sys_UserApproveEntity> lstReturn = new List<Sys_UserApproveEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var type = ApproveType.E_FIN_PurchaseRequest.ToString();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = string.Empty;
                var UserLogin = string.Empty;
                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var lstUserInfo = repoSys_UserInfo.FindBy(s => s.IsDelete == null).ToList();
                var record = GetData<FIN_PurchaseRequestEntity>(recordID, ConstantSql.hrm_hr_sp_get_PurchaseRequestById, UserLogin, ref status).FirstOrDefault();
                string currentStatus = record.Status;
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == currentStatus).FirstOrDefault();

                List<ElementFormula> listFomula = new List<ElementFormula>();
                ElementFormula FomulaItem = new ElementFormula("Total", record.Total, 0);
                ElementFormula statusItem = new ElementFormula(configProcess.NextStatusFormular, configProcess.NextStatusFormular, 1);
                listFomula.Add(FomulaItem);
                listFomula.Add(statusItem);
                string nextStatus = GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular).ToString();

                var lstUserApprove = repoSys_UserApprove.FindBy(s => s.IsDelete == null && s.Type == type && s.CurrentStatus == nextStatus).ToList();
                Sys_UserApproveEntity model = new Sys_UserApproveEntity();
                foreach (var item in lstUserApprove)
                {
                    if (item.UserRequestID != null && item.UserRequestID == record.UserCreateID)
                    {
                        model = new Sys_UserApproveEntity();
                        var temp = lstUserInfo.Where(s => s.ID == item.UserApproveID).FirstOrDefault();
                        model.ID = item.ID;
                        model.UserApproveName = temp.UserInfoName;
                        model.UserApproveID = item.UserApproveID;
                        lstReturn.Add(model);
                    }
                }
            }
            return lstReturn;
        }

        public List<Sys_UserApproveEntity> GetUserApprovedForMulti_E_FIN_ClaimRequest(Guid recordID, string userLogin)
        {
            List<Sys_UserApproveEntity> lstReturn = new List<Sys_UserApproveEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var type = ApproveType.E_FIN_ClaimRequest.ToString();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = string.Empty;

                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var lstUserInfo = repoSys_UserInfo.FindBy(s => s.IsDelete == null).ToList();
                var record = GetData<FIN_ClaimEntity>(recordID, ConstantSql.hrm_hr_sp_get_ClaimById, userLogin, ref status).FirstOrDefault();
                string currentStatus = record.Status;
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == currentStatus).FirstOrDefault();

                List<ElementFormula> listFomula = new List<ElementFormula>();
                ElementFormula FomulaItem = new ElementFormula("Total", record.Total, 0);
                ElementFormula statusItem = new ElementFormula(configProcess.NextStatusFormular, configProcess.NextStatusFormular, 1);
                listFomula.Add(FomulaItem);
                listFomula.Add(statusItem);
                string nextStatus = GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular).ToString();

                var lstUserApprove = repoSys_UserApprove.FindBy(s => s.IsDelete == null && s.Type == type && s.CurrentStatus == nextStatus).ToList();
                Sys_UserApproveEntity model = new Sys_UserApproveEntity();
                foreach (var item in lstUserApprove)
                {
                    if (item.UserRequestID != null && item.UserRequestID == record.UserCreateID)
                    {
                        model = new Sys_UserApproveEntity();
                        var temp = lstUserInfo.Where(s => s.ID == item.UserApproveID).FirstOrDefault();
                        model.ID = item.ID;
                        model.UserApproveName = temp.UserInfoName;
                        model.UserApproveID = item.UserApproveID;
                        lstReturn.Add(model);
                    }
                }
            }
            return lstReturn;
        }

        public List<Sys_UserApproveEntity> GetUserApprovedForMulti_E_FIN_TravelRequest(Guid recordID, string userLogin)
        {
            List<Sys_UserApproveEntity> lstReturn = new List<Sys_UserApproveEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var type = ApproveType.E_FIN_TravelRequest.ToString();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = string.Empty;

                var repoSys_ConfigProcessApprove = new CustomBaseRepository<Sys_ConfigProcessApprove>(unitOfWork);
                var repoSys_UserApprove = new CustomBaseRepository<Sys_UserApprove>(unitOfWork);
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                var lstUserInfo = repoSys_UserInfo.FindBy(s => s.IsDelete == null).ToList();
                var record = GetData<FIN_TravelRequestEntity>(recordID, ConstantSql.hrm_hr_sp_get_TravelRequestById, userLogin, ref status).FirstOrDefault();
                string currentStatus = record.Status;
                var configProcess = repoSys_ConfigProcessApprove.FindBy(s => s.IsDelete == null && s.Function == type && s.CurrentStatus == currentStatus).FirstOrDefault();

                List<ElementFormula> listFomula = new List<ElementFormula>();
                ElementFormula FomulaItem = new ElementFormula("Total", record.Total, 0);
                ElementFormula statusItem = new ElementFormula(configProcess.NextStatusFormular, configProcess.NextStatusFormular, 1);
                listFomula.Add(FomulaItem);
                listFomula.Add(statusItem);
                var nextStatus = (HRM.Infrastructure.Utilities.Helper.FormulaHelper.FormulaHelperModel)GetObjectValue(new List<Cat_ElementEntity>(), listFomula, configProcess.NextStatusFormular);

                var lstUserApprove = repoSys_UserApprove.FindBy(s => s.IsDelete == null && s.Type == type && s.CurrentStatus == nextStatus.Value.ToString()).ToList();
                Sys_UserApproveEntity model = new Sys_UserApproveEntity();
                foreach (var item in lstUserApprove)
                {
                    if (item.UserRequestID != null && item.UserRequestID == record.UserCreateID)
                    {
                        model = new Sys_UserApproveEntity();
                        var temp = lstUserInfo.Where(s => s.ID == item.UserApproveID).FirstOrDefault();
                        model.ID = item.ID;
                        model.UserApproveName = temp.UserInfoName;
                        model.UserApproveID = item.UserApproveID;
                        lstReturn.Add(model);
                    }
                }
            }
            return lstReturn;
        }
        /// <summary>
        /// [Hien.Nguyen] Hàm tính value cho công tức
        /// </summary>
        /// <param name="ListElement">List các Element đã tính được trước đó</param>
        /// <param name="formula">Công thức</param>
        /// <returns>Giá trị tính được từ công thức</returns>
        public object GetObjectValue(List<Cat_ElementEntity> listElement, List<ElementFormula> ListElementFormula, string formula)
        {
            try
            {
                formula = formula.Replace("\n", "").Replace("\t", "").Trim();
                //Các phần tử tính lương tách ra từ 1 chuỗi công thức
                var ListFormula = ParseFormulaToList(formula).Where(m => m.IndexOf('[') != -1 && m.IndexOf(']') != -1).ToList();

                //Các phần tử tính lương chưa có kết quả
                var ListFormulaNotValue = ListFormula.Where(m => !ListElementFormula.Any(t => t.VariableName == m.Replace("[", "").Replace("]", ""))).ToList();

                //Nếu có tồn tại phần tử chưa có Value, thì đi tính value cho phần tử đó
                if (ListFormulaNotValue != null && ListFormulaNotValue.Count > 0)
                {
                    foreach (var i in ListFormulaNotValue)
                    {
                        if (!ListElementFormula.Any(m => m.VariableName == i.Replace("[", "").Replace("]", "")))
                        {
                            string ttt = "";
                            foreach (var j in listElement)
                            {
                                if (j.ElementCode == i.Replace("[", "").Replace("]", ""))
                                    ttt = j.Formula;
                            }
                            ElementFormula item = new ElementFormula(i.Replace("[", "").Replace("]", ""), GetObjectValue(listElement, ListElementFormula, listElement.Where(m => m.ElementCode == i.Replace("[", "").Replace("]", "")).FirstOrDefault().Formula), 0);
                            ListElementFormula.Add(item);
                        }
                    }
                }
                //Do mệnh đề if luôn trả về false nên xử lý riêng cho mệnh đề if ở đây
                //nguyên ngân là do dll CalcEngine, nhưng chưa tìm ra cách giải quyết
                if (formula.ToUpper().Contains("IF("))
                {
                    foreach (var i in ListElementFormula.Distinct().ToList())
                    {
                        if (formula.Contains("[" + i.VariableName + "]"))
                        {
                            formula = formula.Replace("[" + i.VariableName + "]", i.Value.ToString());
                        }
                    }
                    return FormulaHelper.ParseFormula(formula.Replace("[", "").Replace("]", ""), ListElementFormula.Distinct().ToList());
                }
                else
                {
                    return FormulaHelper.ParseFormula(formula.Replace("[", "").Replace("]", ""), ListElementFormula.Distinct().ToList());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Tách công thức thành List các phần tử để kiểm tra
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public List<string> ParseFormulaToList(string formula)
        {
            formula = formula.Replace("[", " [").Replace("]", "] ");
            return formula.Split(' ').Where(m => m.StartsWith("[") && m.EndsWith("]")).Distinct().ToList();


            //formula = formula.Replace(" ", "").Replace("+", " ").Replace("-", " ").Replace("*", " ").Replace("/", " ").Replace("(", " ").Replace(")", "");
            //return formula.Split(' ').ToList();
        }
    }
}
