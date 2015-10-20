using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_DistrictServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatDistricts()
        {
            string status = string.Empty;
            Cat_DistrictServices service = new Cat_DistrictServices();
            int count = service.GetAllUseEntity<Cat_DistrictEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatDistricts()
        {
            Cat_DistrictServices service = new Cat_DistrictServices();
            var District = new Cat_District { Id = 22 };
            string status = string.Empty;
            var result = service.GetById<Cat_District>(District.Id,ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.DistrictName );
        }

        [Test]
        public void NUnit_Domain_AddCatDistrict()
        {
            Cat_DistrictServices service = new Cat_DistrictServices();
            int rs=0;
            for (int i = 0; i < 1000000; i++)
            {
                var District = new Cat_District
                {
                    DistrictCode = "NUnit-DistrictCode-" + i,
                    DistrictName = "NUnit Domain DistrictName " + i,
                    Notes = "NUnit-Notes-"+i
                };
              
                if (service.Add<Cat_District>(District) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + District.Id + " | " + District.DistrictName + " | " + District.DistrictCode + " | " + District.Notes);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatDistrict()
        {
            Cat_DistrictServices service = new Cat_DistrictServices();
            var District = new Cat_District
            {
                Id = 12,
                DistrictCode = "BC-12",
                DistrictName = "ABCDomain12",
                Notes = "NUnit Test12"
            };
           // NUnit.Framework.Assert.IsTrue(result);
            if (service.Edit<Cat_District>(District) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + District.Id + " | " + District.DistrictName + " | " + District.DistrictCode + " | " + District.Notes);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatDistrict()
        {
            Cat_DistrictServices service = new Cat_DistrictServices();
            int rs = 0;
            for (int i = 1; i <= 1000000; i++)
            {
                var District = new Cat_District { Id = i };
                if (service.Delete<Cat_District>(District.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        //[Test]
        //public void NUnit_Domain_DeleteEternityCatDistrict()
        //{
        //    Cat_DistrictServices service = new Cat_DistrictServices();
        //    bool result = true;
        //    int rs = 0;
        //    for (int i = 1; i <= 1000000; i++)
        //    {
        //        var District = new Cat_District { Id = i };
        //        result = service.DeleteEternity(District.Id);
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
