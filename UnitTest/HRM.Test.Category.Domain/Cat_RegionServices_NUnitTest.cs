using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_RegionServices_NUnitTest
    {
        string status = string.Empty;
        [Test]
        public void NUnit_Domain_GetCatRegions(ListQueryModel model)
        {
            string status = string.Empty;
            Cat_RegionServices service = new Cat_RegionServices();
            //int count = service.GetCatRegion().Count();
            int count = service.GetAllUseEntity<Cat_Region>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatRegions()
        {
            Cat_RegionServices service = new Cat_RegionServices();
            var Region = new Cat_Region { Id = 22 };
            //var result = service.GetByIdCatRegion(Region.Id) as Cat_Region;
            var result = service.GetById<Cat_Region>(Region.Id, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.RegionName + " | " + result.RegionCode + " | " + result.Notes);
        }

        [Test]
        public void NUnit_Domain_AddCatRegion()
        {
            Cat_RegionServices service = new Cat_RegionServices();
            bool result =true;
            int rs=0;
            for (int i = 0; i < 1000000; i++)
            {
                var Region = new Cat_Region
                {
                    RegionName = "NUnit Domain RegionName " + i,
                    RegionCode = "NUnit-RegionCode-" + i,
                    Notes = "NUnit-Notes-"+i
                };
                //result = service.AddCatRegion(Region);
                //if (result == true)
                //{
                //    rs += 1;
                //    Console.WriteLine("Process Success >>> Create >>> " + Region.Id + " | " + Region.RegionName + " | " + Region.Code + " | " + Region.Notes);
                //}
                if (service.Add(Region) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + Region.Id + " | " + Region.RegionName + " | " + Region.RegionCode + " | " + Region.Notes);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatRegion()
        {
            Cat_RegionServices service = new Cat_RegionServices();
            var Region = new Cat_Region
            {
                Id = 12,
                RegionName = "ABCDomain12",
                RegionCode = "BC-12",
                Notes = "NUnit Test12"
            };
            //bool result = service.UpdateCatRegion(Region);
            //NUnit.Framework.Assert.IsTrue(result);
            //if (result == true)
            //{
            //    Console.WriteLine("==>>> Result After Update:  " + Region.Id + " | " + Region.RegionName + " | " + Region.Code + " | " + Region.Notes);
            //}
            if (service.Edit(Region) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + Region.Id + " | " + Region.RegionName + " | " + Region.RegionCode + " | " + Region.Notes);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatRegion()
        {
            Cat_RegionServices service = new Cat_RegionServices();
            bool result = true;
            int rs = 0;
            for (int i = 1; i <= 1000000; i++)
            {
                var Region = new Cat_Region { Id = i };
                //result = service.DeleteCatRegion(Region.Id);
                //if (result == true)
                //{
                //    rs += 1;
                //    Console.WriteLine("Process Success >>> Delete >>> " + i);
                //}
                if (service.Delete<Cat_Region>(Region.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.Write(" Process Success >>> DeleteEternity >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

      

        
    }
}
