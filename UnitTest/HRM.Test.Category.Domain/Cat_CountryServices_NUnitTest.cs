using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_CountryServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatCountry()
        {
            string status = string.Empty;
            Cat_CountryServices service = new Cat_CountryServices();
            int count = service.GetAllUseEntity<Cat_CountryMultiEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatCountry()
        {
            Cat_CountryServices service = new Cat_CountryServices();
            var Country = new Cat_Country { Id = 2 };
            string status = string.Empty;
            var result = service.GetById<Cat_Country>(Country.Id,ref status) ;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.CountryCode + " | " + result.CountryName + " | " + result.CountryNameES + " | " + result.Notes);
        }

        [Test]
        public void NUnit_Domain_AddCatCountry()
        {
            Cat_CountryServices service = new Cat_CountryServices();
            int rs = 0;
            for (int i = 0; i < 10; i++)
            {
                var Country = new Cat_Country
                {
                    CountryCode = "tt" + i,
                    CountryName = "Country Name Test" + i,
                    CountryNameES = "Name ES Test" + i,
                    Notes = "Notes Test" + i
                };
                if (service.Add<Cat_Country>(Country) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + Country.Id + " | " + Country.CountryCode + " | " + Country.CountryName + " | " + Country.CountryNameES + " | " + Country.Notes);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatCountry()
        {
            Cat_CountryServices service = new Cat_CountryServices();
            var Country = new Cat_Country
            {
                Id = 3,
                CountryCode = "UD tt",
                CountryName = "UD Name Test",
                CountryNameES = "UD Name ES Test",
                Notes = "UD Notes Test"
            };
            //NUnit.Framework.Assert.IsTrue(result);
            if ( service.Edit<Cat_Country>(Country) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + Country.Id + " | " + Country.CountryCode + " | " + Country.CountryName + " | " + Country.CountryNameES + " | " + Country.Notes);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatCountry()
        {
            Cat_CountryServices service = new Cat_CountryServices();
            int rs = 0;
            for (int i = 3; i <= 5; i++)
            {
                var Country = new Cat_Country { Id = i };
                if (service.Delete<Cat_Country>(Country.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        //[Test]
        //public void NUnit_Domain_DeleteEternityCatCountry()
        //{
        //    Cat_CountryServices service = new Cat_CountryServices();
        //    bool result = true;
        //    int rs = 0;
        //    for (int i = 4; i <= 6; i++)
        //    {
        //        var Country = new Cat_Country { Id = i };
        //        result = service.DeleteEternity(Country.Id);
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
