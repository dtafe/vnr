using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_CurrencyServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatCurrency()
        {
            string status = string.Empty;
            Cat_CurrencyServices service = new Cat_CurrencyServices();
            int count = service.GetAllUseEntity<Cat_Currency>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatCurrency()
        {
            Cat_CurrencyServices service = new Cat_CurrencyServices();
            var Currency = new Cat_Currency { Id = 2 };
            string status = string.Empty;
            var result = service.GetById<Cat_Currency>(Currency.Id, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.CurrencyName + " | " + result.CurrencyCode + " | " + result.Description + " | " + result.DateCreate);
        }

        [Test]
        public void NUnit_Domain_AddCatCurrency()
        {
            Cat_CurrencyServices service = new Cat_CurrencyServices();
 
            int rs = 0;
            for (int i = 0; i < 10; i++)
            {
                var Currency = new Cat_Currency
                {
                    CurrencyName = "Currency Name Test" + i,
                    CurrencyCode = "tt" + i,
                    Description = "TestDES" + i,
                    DateCreate = DateTime.Now
                };

                if (service.Add<Cat_Currency>(Currency) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + Currency.Id + " | " + Currency.CurrencyName + " | " + Currency.CurrencyCode + " | " + Currency.Description + " | " + Currency.DateCreate);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatCurrency()
        {
            Cat_CurrencyServices service = new Cat_CurrencyServices();
            var Currency = new Cat_Currency
            {
                Id = 12,
                CurrencyName = "Update Currency Name Test",
                CurrencyCode = "Update tt",
                Description = "Update TestDES",
                DateCreate = DateTime.Now
            };
          //  NUnit.Framework.Assert.IsTrue(result);
            if (service.Edit<Cat_Currency>(Currency) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + Currency.Id + " | " + Currency.CurrencyName + " | " + Currency.CurrencyCode + " | " + Currency.Description + " | " + Currency.DateCreate);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatCurrency()
        {
            Cat_CurrencyServices service = new Cat_CurrencyServices();
         
            int rs = 0;
            for (int i = 1; i <= 4; i++)
            {
                var Currency = new Cat_Currency { Id = i };

                if (service.Delete<Cat_Currency>(Currency.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        //[Test]
        //public void NUnit_Domain_DeleteEternityCatCurrency()
        //{
        //    Cat_CurrencyServices service = new Cat_CurrencyServices();
        //    bool result = true;
        //    int rs = 0;
        //    for (int i = 1; i <= 3; i++)
        //    {
        //        var Currency = new Cat_Currency { Id = i };
        //        result = service.DeleteEternity(Currency.Id);
        //        if (result == true)
        //        {
        //            rs += 1;
        //            Console.Write(" Process Success >>> DeleteEternity >>> " + i);
        //        }
        //    }
        //    Console.WriteLine("Total success record: " + rs);
        //    //NUnit.Framework.Assert.IsTrue(result);
        //}
    }
}
