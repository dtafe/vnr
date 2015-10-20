using HRM.Business.Main.Domain;
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
    public class Sal_PayrollLib : BaseService
    {
        /// <summary>
        /// Lay gia tri trong PayrollTableItem voi elementType 
        /// </summary>
        public static object GetItemValue(List<Sal_PayrollTableItem> lstItem, String elementType)
        {
            Sal_PayrollTableItem item = lstItem.Where(it => it.Code == elementType).FirstOrDefault();
            if (item != null)
            {
                return item.udValue;
            }
            return DBNull.Value;
        }

        public static Double ConvertExtractRateToVND(Double moneyDest, Cat_Currency curDest, List<Cat_ExchangeRate> lstExchangeRate)
        {
            Double value = 0;
            String curVND = CurrencyCode.VND.ToString();
            List<Cat_ExchangeRate> lstExchange = new List<Cat_ExchangeRate>();
            if (curVND == curDest.Code)
                return moneyDest;
            //Lay Nguyen Te : VND | Ngoai Te : USD
            lstExchange = lstExchangeRate.Where(cur => cur.CurrencyDestID == curDest.ID && cur.Cat_Currency.Code == curVND)
                                            .OrderByDescending(cu => cu.MonthOfEffect).ToList();
            if (lstExchange.Count <= 0)
            {
                //Neu lstExchange khong co thi lay nguoc lai Nguyen Te : USD | Ngoai te : VND
                lstExchange = lstExchangeRate.Where(cur => cur.CurrencyBaseID == curDest.ID && cur.Cat_Currency1.Code == curVND)
                                             .OrderByDescending(cu => cu.MonthOfEffect).ToList();

                if (lstExchange.Count <= 0)
                    throw new Exception("Currency " + curVND + " / " + curDest.Code + "not found ");

                //Xu ly voi truong hop Nguyen Te : USD | Ngoai te : VND
                Cat_ExchangeRate exRate = lstExchange[0];
                //Vi doi tu USD sang VND nen phai lay gia mua
                if (exRate.BuyingRate == 0)
                    throw new Exception("Exchange buying rate" + curDest.Code + " / " + curVND + "not found ");

                if (exRate.BuyingRate.HasValue)
                    value = moneyDest / exRate.BuyingRate.Value;
                return value;
            }

            //Xu ly voi truong hop Nguyen Te : VND | Ngoai te : USD
            Cat_ExchangeRate exRate1 = lstExchange[0];
            //Vi doi tu USD sang VND nen phai lay gia ban
            if (exRate1.SellingRate == 0)
                throw new Exception("Exchange selling rate" + curVND + " / " + curDest.Code + "not found ");


            value = moneyDest * exRate1.SellingRate;
            return value;
        }

    }
}
