using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Data.Entity.Repositories;
using HRM.Business.Attendance.Models;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Category.Domain;
using HRM.Business.Hr.Domain;
using VnResource.Helper.Data;
using HRM.Business.Attendance.Domain;
using HRM.Infrastructure.Utilities.Helper;

namespace HRM.Business.Payroll.Domain
{
    public class Kai_KaiZenDataServices : BaseService
    {
        public bool SaveAmountKaiZen(List<Guid> SelectIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    string status = string.Empty;
                    var repoKai_KaizenData = new CustomBaseRepository<Kai_KaizenData>(unitOfWork);
                    List<Kai_KaizenData> listKai_KaizenData = repoKai_KaizenData.FindBy(m => m.IsDelete != true && SelectIds.Any(t => t == m.ID)).ToList();
                    listKai_KaizenData.ForEach(m => m.AmountTransfered = m.SumAmount);
                    unitOfWork.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public int[] SetPayrmentOut(List<Guid> SelectIds, DateTime MonthYear)
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    string status = string.Empty;
                    var repoKai_KaizenData = new CustomBaseRepository<Kai_KaizenData>(unitOfWork);
                    List<Kai_KaizenData> ListKaiZendData = repoKai_KaizenData.FindBy(m => m.IsDelete != true && m.Status == EnumDropDown.Kai_KaizenDataStatus.E_APPROVED.ToString()).ToList();
                    ListKaiZendData = ListKaiZendData.Where(m => SelectIds.Any(t => t == m.ID)).ToList();

                    int Pass = 0;
                    int Fail = 0;

                    foreach (var i in ListKaiZendData)
                    {
                        if (i.IsPaymentOut == null && i.DateTransferPayment == null)
                        {
                            i.IsPaymentOut = true;
                            i.DateTransferPayment = MonthYear;
                            Pass++;
                        }
                        else
                        {
                            Fail++;
                        }
                    }
                    unitOfWork.SaveChanges();
                    return new int[] { Pass, Fail };
                }
                catch
                {
                    return new int[] { 0, 0 };
                }
            }
        }

        public bool SaveCustomAccumulate(Guid ID, double AccumulateRevice)
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    string status = string.Empty;
                    var repoKai_KaizenData = new CustomBaseRepository<Kai_KaizenData>(unitOfWork);
                    Kai_KaizenData KaiZendData = repoKai_KaizenData.FindBy(m => m.ID == ID).FirstOrDefault();
                    KaiZendData.AccumulateRevice = AccumulateRevice;
                    unitOfWork.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool SetApproveKaiZenData(List<Guid> SelectIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    string status = string.Empty;
                    var repoKai_KaizenData = new CustomBaseRepository<Kai_KaizenData>(unitOfWork);
                    List<Kai_KaizenData> ListKaiZendData = repoKai_KaizenData.FindBy(m => m.IsDelete != true).ToList();
                    foreach (var i in ListKaiZendData)
                    {
                        if (SelectIds.Any(m => m == i.ID))
                        {
                            i.Status = EnumDropDown.Kai_KaizenDataStatus.E_APPROVED.ToString();
                        }
                    }
                    unitOfWork.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public string[] SetWaitApproveKaiZenData(List<Guid> SelectIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                try
                {
                    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                    string status = string.Empty;
                    var repoKai_KaizenData = new CustomBaseRepository<Kai_KaizenData>(unitOfWork);
                    var repoKai_RankMark = new CustomBaseRepository<Kai_RankMark>(unitOfWork);
                    List<Kai_KaizenData> ListKaiZendData = repoKai_KaizenData.FindBy(m => m.IsDelete != true).ToList();
                    List<Kai_RankMark> ListKaiRank = repoKai_RankMark.FindBy(m => m.IsDelete != true).ToList();

                    int Pass = 0;
                    int Fail = 0;
                    string FailCode = string.Empty;

                    foreach (var i in ListKaiZendData)
                    {
                        if (SelectIds.Any(m => m == i.ID))
                        {
                            #region Kiểm tra hợp lệ vùng điểm
                            List<Kai_RankMark> ListKaiRankByRankMark = ListKaiRank.Where(m => m.MarkIdea == i.MarkIdea).ToList();
                            if (ListKaiRankByRankMark.Any(m => m.MarkPerform == i.MarkPerform))
                            {
                                i.Status = EnumDropDown.Kai_KaizenDataStatus.E_WATTING_APPROVED.ToString();
                                Pass++;
                            }
                            else
                            {
                                Fail++;
                                FailCode += i.Hre_Profile.CodeEmp + ",";
                            }
                            #endregion
                        }
                    }
                    unitOfWork.SaveChanges();

                    return new string[] { Pass.ToString(), Fail.ToString(), FailCode };
                }
                catch
                {
                    return new string[] { "0", "0", string.Empty };
                }
            }
        }

        public int[] ApprevedKaiZenData(List<Guid> selectedIds, DateTime MonthYear, Guid UnusualEDTypeID, Guid Currency)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoKai_KaizenData = new CustomBaseRepository<Kai_KaizenData>(unitOfWork);
                var repoSal_UnusualAllowance = new CustomBaseRepository<Sal_UnusualAllowance>(unitOfWork);
                List<Sal_UnusualAllowance> listUnusualAllowance = new List<Sal_UnusualAllowance>();

                //lọc ra các kaizen được chọn
                List<Kai_KaizenData> listKaiZendata = repoKai_KaizenData.FindBy(m => m.IsDelete != true).ToList();
                listKaiZendata = listKaiZendata.Where(m => m.Status == EnumDropDown.Kai_KaizenDataStatus.E_APPROVED.ToString() && selectedIds.Any(t => t == m.ID)).ToList();

                int Pass = 0;
                int Fail = 0;

                Sal_UnusualAllowance item = new Sal_UnusualAllowance();
                foreach (var KaiZenItem in listKaiZendata)
                {
                    if (KaiZenItem.DateTransferPayment == null && (KaiZenItem.IsPaymentOut == null || KaiZenItem.IsPaymentOut == false))
                    {
                        item = new Sal_UnusualAllowance();
                        item.ProfileID = KaiZenItem.ProfileID != null ? (Guid)KaiZenItem.ProfileID : Guid.NewGuid();
                        item.MonthStart = new DateTime(MonthYear.Year, MonthYear.Month, 1);
                        item.MonthEnd = new DateTime(MonthYear.Year, MonthYear.Month, 1).AddMonths(1).AddDays(-1);
                        item.Type = EnumDropDown.EDType.E_EARNING.ToString();
                        item.UnusualEDTypeID = UnusualEDTypeID;
                        item.CurrencyID = Currency;
                        item.Amount = KaiZenItem.SumAmount != null ? KaiZenItem.SumAmount : 0;
                        listUnusualAllowance.Add(item);

                        KaiZenItem.DateTransferPayment = MonthYear;
                        Pass++;
                    }
                    else
                    {
                        Fail++;
                    }
                }
                repoSal_UnusualAllowance.Add(listUnusualAllowance);
                unitOfWork.SaveChanges();
                return new int[] { Pass, Fail };
            }
        }
    }
}
