using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_ProvinceServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatProvinces()
        {
            string status = string.Empty;
            Cat_ProvinceServices service = new Cat_ProvinceServices();
            int count = service.GetAllUseEntity<Cat_ProvinceEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatProvinces()
        {
            Cat_ProvinceServices service = new Cat_ProvinceServices();
            var Province = new Cat_Province { Id = 22 };
            string status = string.Empty;
            var result = service.GetById<Cat_Province>(Province.Id, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.ProvinceName + " | " + result.ProvinceCode + " | " + result.Notes);
        }

        [Test]
        public void NUnit_Domain_AddCatProvince()
        {
            Cat_ProvinceServices service = new Cat_ProvinceServices();
            bool result =true;
            int rs=0;
            for (int i = 0; i < 1000000; i++)
            {
                var Province = new Cat_Province
                {
                    ProvinceName = "NUnit Domain ProvinceName " + i,
                    ProvinceCode = "NUnit-ProvinceCode-" + i,
                    Notes = "NUnit-Notes-"+i
                };
                if (service.Add<Cat_Province>(Province) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + Province.Id + " | " + Province.ProvinceName + " | " + Province.ProvinceCode + " | " + Province.Notes);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatProvince()
        {
            Cat_ProvinceServices service = new Cat_ProvinceServices();
            var Province = new Cat_Province
            {
                Id = 12,
                ProvinceName = "ABCDomain12",
                ProvinceCode = "BC-12",
                Notes = "NUnit Test12"
            };
          //  NUnit.Framework.Assert.IsTrue(result);
            if (service.Edit<Cat_Province>(Province) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + Province.Id + " | " + Province.ProvinceName + " | " + Province.ProvinceCode + " | " + Province.Notes);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatProvince()
        {
            Cat_ProvinceServices service = new Cat_ProvinceServices();
            int rs = 0;
            for (int i = 1; i <= 1000000; i++)
            {
                var Province = new Cat_Province { Id = i };
                if (service.Delete<Cat_Province>(Province.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        //[Test]
        //public void NUnit_Domain_DeleteEternityCatProvince()
        //{
        //    Cat_ProvinceServices service = new Cat_ProvinceServices();
        //    bool result = true;
        //    int rs = 0;
        //    for (int i = 1; i <= 1000000; i++)
        //    {
        //        var Province = new Cat_Province { Id = i };
        //        result = service.DeleteEternity(Province.Id);
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
