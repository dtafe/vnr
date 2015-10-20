using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_DayOffServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatDayOff()
        {
            string status = string.Empty;
            Cat_DayOffServices service = new Cat_DayOffServices();
            int count = service.GetAllUseEntity<Cat_DayOffEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatDayOff()
        {
            Cat_DayOffServices service = new Cat_DayOffServices();
            var DayOff = new Cat_DayOff { Id = 2 };
            string status = string.Empty;
            var result = service.GetById<Cat_DayOff>(DayOff.Id, ref status) ;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.DateOff + " | " + result.Type + " | " + result.Status + " | " + result.Comments + " | " + result.OrgStructureID + " | " + result.IsLeaveDay);
        }

        [Test]
        public void NUnit_Domain_AddCatDayOff()
        {
            Cat_DayOffServices service = new Cat_DayOffServices();
            int rs = 0;
            for (int i = 0; i < 10; i++)
            {
                var DayOff = new Cat_DayOff
                {
                    Type = "NUnit Domain DayOffName " + i,
                    Status = "NUnit-DayOffCode-" + i,
                };
                if (service.Add<Cat_DayOff>(DayOff) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + DayOff.Id + " | " + DayOff.DateOff + " | " + DayOff.Type + " | " + DayOff.Status + " | " + DayOff.Comments + " | " + DayOff.OrgStructureID + " | " + DayOff.IsLeaveDay);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatDayOff()
        {
            Cat_DayOffServices service = new Cat_DayOffServices();
            var DayOff = new Cat_DayOff
            {
                Id = 12,
                Type = "ABCDomain12",
                Status = "BC-12",
            };
         //   NUnit.Framework.Assert.IsTrue(result);
            if (service.Edit<Cat_DayOff>(DayOff) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + DayOff.Id + " | " + DayOff.DateOff + " | " + DayOff.Type + " | " + DayOff.Status + " | " + DayOff.Comments + " | " + DayOff.OrgStructureID + " | " + DayOff.IsLeaveDay);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatDayOff()
        {
            Cat_DayOffServices service = new Cat_DayOffServices();
            int rs = 0;
            for (int i = 1; i <= 10; i++)
            {
                var DayOff = new Cat_DayOff { Id = i };
                if (service.Delete<Cat_DayOff>(DayOff.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        //[Test]
        //public void NUnit_Domain_DeleteEternityCatDayOff()
        //{
        //    Cat_DayOffServices service = new Cat_DayOffServices();
        //    bool result = true;
        //    int rs = 0;
        //    for (int i = 1; i <= 10; i++)
        //    {
        //        var DayOff = new Cat_DayOff { Id = i };
        //        result = service.Delete(DayOff.Id);
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
