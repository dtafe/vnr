using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Infrastructure.Utilities.Helper;
using System.Threading;
using HRM.Business.Insurance.Models;

namespace HRM.Business.Payroll.Domain
{
    public class Sal_RevenueRecordService : BaseService
    {
        public void ComputeComissionService(Guid? ShopID, DateTime Month, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoShop = new CustomBaseRepository<Cat_Shop>(unitOfWork);
                var repoRevenueForShop = new CustomBaseRepository<Sal_RevenueForShopEntity>(unitOfWork);
                var repoItemForShopEntity = new CustomBaseRepository<Sal_ItemForShopEntity>(unitOfWork);
                var repoLineItemForShopEntity = new CustomBaseRepository<Sal_LineItemForShopEntity>(unitOfWork);
                var repoRevenueRecord = new CustomBaseRepository<Sal_RevenueRecord>(unitOfWork);

                #region Get Data


                List<object> listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                List<Cat_Shop> listShop = GetData<Cat_Shop>(listModel, ConstantSql.hrm_cat_sp_get_Shop, UserLogin, ref status).ToList();

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                List<Cat_ShopGroupEntity> listGroupShop = GetData<Cat_ShopGroupEntity>(listModel, ConstantSql.hrm_cat_sp_get_ShopGroup, UserLogin, ref status).ToList();

                listModel = new List<object>();
                listModel.AddRange(new object[6]);
                listModel[4] = 1;
                listModel[5] = Int32.MaxValue - 1;
                List<Sal_RevenueForShopEntity> listRevenueForShop = GetData<Sal_RevenueForShopEntity>(listModel, ConstantSql.hrm_sal_sp_get_RevenueForShop, UserLogin, ref status).ToList();

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                List<Sal_ItemForShopEntity> listItemForShop = GetData<Sal_ItemForShopEntity>(listModel, ConstantSql.hrm_sal_sp_get_ItemForShop, UserLogin, ref status).ToList();

                listModel = new List<object>();
                listModel.AddRange(new object[5]);
                listModel[3] = 1;
                listModel[4] = Int32.MaxValue - 1;
                List<Sal_LineItemForShopEntity> listLineItemForShop = GetData<Sal_LineItemForShopEntity>(listModel, ConstantSql.hrm_sal_sp_get_LineItemForShop, UserLogin, ref status).ToList();

                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[0] = Month;
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                List<Sal_RevenueRecord> listRevenueRecord = GetData<Sal_RevenueRecord>(listModel, ConstantSql.hrm_sal_sp_get_RevenueRecord, UserLogin, ref status).ToList();

                listModel = new List<object>();
                listModel.AddRange(new object[18]);
                listModel[16] = 1;
                listModel[17] = int.MaxValue - 1;
                List<Hre_ProfileEntity> listProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_Profile, UserLogin, ref status).ToList();

                List<Cat_KPIBonusEntity> listKPIBonus = new List<Cat_KPIBonusEntity>();
                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                listKPIBonus = GetData<Cat_KPIBonusEntity>(listModel, ConstantSql.hrm_cat_sp_get_KPIBonus, UserLogin, ref status).ToList();

                List<Cat_KPIBonusItemEntity> listKPIBonusItem = new List<Cat_KPIBonusItemEntity>();
                listModel = new List<object>();
                listModel.AddRange(new object[4]);
                listModel[2] = 1;
                listModel[3] = Int32.MaxValue - 1;
                listKPIBonusItem = GetData<Cat_KPIBonusItemEntity>(listModel, ConstantSql.hrm_cat_sp_get_KPIBonusItem, UserLogin, ref status).ToList();


                #endregion

                List<Guid> ListShopID = new List<Guid>();
                if (ShopID == null)
                {
                    ListShopID = listShop.Select(m => m.ID).ToList();
                }
                else
                {
                    if (!listShop.Any(m => m.ID == ShopID))
                    {
                        if (!listGroupShop.Any(m => m.ID == ShopID))
                        {
                            ListShopID = listShop.Select(m => m.ID).ToList();
                        }
                        else
                        {
                            ListShopID = listShop.Where(m => m.ShopGroupID == ShopID).Select(m => m.ID).ToList();
                        }
                    }
                    else
                    {
                        ListShopID.Add((Guid)ShopID);
                    }
                }

                List<Sal_RevenueRecord> _tmpRevenueRecord;
                List<ElementFormula> listElementFormula = new List<ElementFormula>();

                foreach (Guid Guid in ListShopID)
                {
                    #region Xóa tháng đã tính trước đó nếu có
                    _tmpRevenueRecord = new List<Sal_RevenueRecord>();
                    _tmpRevenueRecord = listRevenueRecord.Where(m => m.ShopID == Guid && m.Month.Value.Month == Month.Month && m.Month.Value.Year == Month.Year).ToList();
                    if (_tmpRevenueRecord.Count > 0)
                    {
                        foreach (var j in _tmpRevenueRecord)
                        {
                            repoRevenueRecord.Remove(j);
                        }
                    }
                    #endregion

                    #region Lấy các phần tử là Enum
                    Sal_RevenueForShopEntity RevenueForShopItem = new Sal_RevenueForShopEntity();
                    RevenueForShopItem = listRevenueForShop.Where(m => m.ShopID == Guid && m.KPIBonusID == listKPIBonus.Where(t => t.IsTotalRevenue == true).FirstOrDefault().ID && (m.DateFrom.Value.Month == Month.Month && m.DateFrom.Value.Year == Month.Year) && (m.DateTo.Value.Month == Month.Month && m.DateTo.Value.Year == Month.Year)).FirstOrDefault();

                    if (RevenueForShopItem != null)
                    {
                        listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_TAGET_SHOP.ToString(), RevenueForShopItem.Target, 0));
                        listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_ACTUAL_SHOP.ToString(), RevenueForShopItem.Actual, 0));
                        listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_PRECENT_REVENUE.ToString(), RevenueForShopItem.Actual / RevenueForShopItem.Target, 0));
                    }
                    else
                    {
                        continue;
                    }

                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_COUNT_SHOPMEMBER.ToString(), listProfile.Where(m => m.ShopID == Guid).Count(), 0));

                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_COUNT_SL.ToString(), listShop.Where(m => m.ID == Guid).FirstOrDefault().NoShiftLeader, 0));

                    listElementFormula.Add(new ElementFormula(PayrollElement.SAL_COM_RANK.ToString(), listShop.Where(m => m.ID == Guid).FirstOrDefault().Rank, 0));

                    #endregion


                    #region Lấy giá trị cho các loại phần tử là Hoa Hồng

                    double TotalMoney_RevenueForShop = 0;//biến lưu tổng doanh thu cửa hàng sau khi đã trừ tiền SL
                    if (listKPIBonus != null && listKPIBonus.Count > 0)
                    {
                        #region tính số tổng doanh thu sau khi trừ tiền SL

                        RevenueForShopItem = new Sal_RevenueForShopEntity();
                        RevenueForShopItem = listRevenueForShop.Where(m => m.ShopID == Guid && m.KPIBonusID == listKPIBonus.Where(t => t.IsTotalRevenue == true).FirstOrDefault().ID && (m.DateFrom.Value.Month == Month.Month && m.DateFrom.Value.Year == Month.Year) && (m.DateTo.Value.Month == Month.Month && m.DateTo.Value.Year == Month.Year)).FirstOrDefault();

                        double Money_SL = 0;
                        if (RevenueForShopItem != null)
                        {
                            Money_SL = (double)GetObjectValue(new List<Cat_ElementEntity>(), listElementFormula, listShop.Single(m => m.ID == Guid).Formular1).Value;
                            double Money_Revenue = (double)RevenueForShopItem.Actual * ((double)GetObjectValue(new List<Cat_ElementEntity>(), listElementFormula, listShop.Single(m => m.ID == Guid).Formular).Value / 100);
                            TotalMoney_RevenueForShop = Money_Revenue - Money_SL;
                        }

                        #endregion
                        Sal_RevenueRecord RecordItem = new Sal_RevenueRecord();
                        foreach (var j in listKPIBonus)
                        {
                            RevenueForShopItem = listRevenueForShop.Where(m => m.ShopID == Guid && m.KPIBonusID == j.ID && (m.DateFrom.Value.Month == Month.Month && m.DateFrom.Value.Year == Month.Year) && (m.DateTo.Value.Month == Month.Month && m.DateTo.Value.Year == Month.Year)).FirstOrDefault();

                            RecordItem = new Sal_RevenueRecord();
                            RecordItem.ShopID = Guid;
                            RecordItem.Month = Month;
                            RecordItem.KPIBonusID = j.ID;
                            RecordItem.Type = EnumDropDown.SalesType.E_SALE.ToString();
                            RecordItem.Note = string.Format("{0:n0}", Money_SL);

                            if ((RevenueForShopItem != null && RevenueForShopItem.Target <= RevenueForShopItem.Actual) || (RevenueForShopItem != null && RevenueForShopItem.IsPass != null && RevenueForShopItem.IsPass == true))
                            {
                                Cat_KPIBonusItemEntity bonusItem = listKPIBonusItem.Where(m => m.ShopID == Guid && m.KPIBonusID == j.ID).FirstOrDefault();
                                if (bonusItem != null)
                                {
                                    RecordItem.Amount = TotalMoney_RevenueForShop * (bonusItem.Value / 100);
                                }
                                else
                                {
                                    RecordItem.Amount = 0;
                                }
                            }
                            else
                            {
                                RecordItem.Amount = 0;
                            }
                            repoRevenueRecord.Add(RecordItem);
                        }

                        #region Lấy giá trị cho 2 enum là dòng sản phẩm và sản phẩm

                        //SAL_COM_PERCENT_SHOP_5
                        List<Sal_LineItemForShopEntity> listLineItem = listLineItemForShop.Where(m => m.ShopID == Guid && (m.DateFrom.Value.Month == Month.Month && m.DateFrom.Value.Year == Month.Year) && (m.DateTo.Value.Month == Month.Month && m.DateTo.Value.Year == Month.Year)).ToList();

                        RecordItem = new Sal_RevenueRecord();
                        RecordItem.ShopID = Guid;
                        RecordItem.Month = Month;
                        RecordItem.Type = EnumDropDown.SalesType.E_LINEITEM_MAJOR.ToString();

                        if (listLineItem != null && listLineItem.Count > 0 && listLineItem.Any(m => m.Actual < m.Target && m.IsPass != true))
                        {
                            int NumberPass = 0;
                            foreach (var lineItem in listLineItem)
                            {
                                if ((lineItem.IsPass != null && lineItem.IsPass == true) || lineItem.Actual >= lineItem.Target)
                                {
                                    NumberPass++;
                                }
                            }
                            if (NumberPass >= 3)
                            {
                                RecordItem.Amount = TotalMoney_RevenueForShop * (listShop.Single(m => m.ID == Guid).MainLineProduct / 100);
                            }
                            else
                            {
                                RecordItem.Amount = 0;
                            }
                        }
                        else
                        {
                            RecordItem.Amount = TotalMoney_RevenueForShop * (listShop.Single(m => m.ID == Guid).MainLineProduct / 100);
                        }
                        repoRevenueRecord.Add(RecordItem);

                        //SAL_COM_PERCENT_SHOP_6
                        List<Sal_ItemForShopEntity> listItem = listItemForShop.Where(m => m.ShopID == Guid && (m.DateFrom.Value.Month == Month.Month && m.DateFrom.Value.Year == Month.Year) && (m.DateTo.Value.Month == Month.Month && m.DateTo.Value.Year == Month.Year)).ToList();

                        RecordItem = new Sal_RevenueRecord();
                        RecordItem.ShopID = Guid;
                        RecordItem.Month = Month;
                        RecordItem.Type = EnumDropDown.SalesType.E_ITEM_MAJOR.ToString();

                        if (listItem != null && listItem.Count > 0 && listItem.Any(m => m.Actual < m.Target && m.IsPass != true))
                        {
                            RecordItem.Amount = 0;
                        }
                        else
                        {
                            RecordItem.Amount = TotalMoney_RevenueForShop * (listShop.Single(m => m.ID == Guid).PromoteProduct / 100);
                        }
                        repoRevenueRecord.Add(RecordItem);

                        #endregion
                    }
                    #endregion

                }
                repoRevenueRecord.SaveChanges();
            }
        }

        /// <summary>
        /// [Hien.Nguyen] Hàm tính value cho công tức
        /// </summary>
        /// <param name="ListElement">List các Element đã tính được trước đó</param>
        /// <param name="formula">Công thức</param>
        /// <returns>Giá trị tính được từ công thức</returns>
        public HRM.Infrastructure.Utilities.Helper.FormulaHelper.FormulaHelperModel GetObjectValue(List<Cat_ElementEntity> listElement, List<ElementFormula> ListElementFormula, string formula)
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
