using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_OvertimeTypeServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatOvertimeType()
        {
            string status = string.Empty;
            Cat_OvertimeTypeServices service = new Cat_OvertimeTypeServices();
            int count = service.GetAllUseEntity<Cat_OvertimeTypeEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatOvertimeType()
        {
            string status = string.Empty;
            Cat_OvertimeTypeServices service = new Cat_OvertimeTypeServices();
            var OvertimeType = new Cat_OvertimeType { Id = 2 };
            var result = service.GetById<Cat_OvertimeTypeEntity>(OvertimeType.Id, ref status) as Cat_OvertimeTypeEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.OvertimeTypeCode + " | " + result.OvertimeTypeName + " | " +
                result.Rate + " | " + result.TaxRate + " | " + result.TimeOffInLieuRate + " | " + result.IsNightShift);
        }

        [Test]
        public void NUnit_Domain_AddCatOvertimeType()
        {
            Cat_OvertimeTypeServices service = new Cat_OvertimeTypeServices();
            bool result = false;
            int rs = 0;
            for (int i = 0; i < 10; i++)
            {
                var OvertimeType = new Cat_OvertimeType
                {
                    OvertimeTypeCode = "tt" + i,
                    OvertimeTypeName = "Overtime Type Name Test" + i,
                    Rate = i,
                    TaxRate = i,
                    TimeOffInLieuRate = i
                };
                if (service.Add(OvertimeType) == "0")
                {
                    result = true;
                }
                if (result == true)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + OvertimeType.Id + " | " + OvertimeType.OvertimeTypeCode + " | " + OvertimeType.OvertimeTypeName
                        + " | " + OvertimeType.Rate + " | " + OvertimeType.TaxRate + " | " + OvertimeType.TimeOffInLieuRate);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatOvertimeType()
        {
            Cat_OvertimeTypeServices service = new Cat_OvertimeTypeServices();
            var OvertimeType = new Cat_OvertimeType
            {
                Id = 3,
                OvertimeTypeCode = "Overtime Type Name tt",
                OvertimeTypeName = "Overtime Type Name Test",
                Rate = 1,
                TaxRate = 1,
                TimeOffInLieuRate = 1
            };
            var result = false;
             if (service.Edit(OvertimeType) == "0")
            {
                result = true;
            }
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                Console.WriteLine("==>>> Result After Update:  " + OvertimeType.Id + " | " + OvertimeType.OvertimeTypeCode + " | " + OvertimeType.OvertimeTypeName
                    + " | " + OvertimeType.Rate + " | " + OvertimeType.TaxRate + " | " + OvertimeType.TimeOffInLieuRate);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatOvertimeType()
        {
            Cat_OvertimeTypeServices service = new Cat_OvertimeTypeServices();
            string  result = string.Empty;
            int rs = 0;
            for (int i = 3; i <= 5; i++)
            {
                var OvertimeType = new Cat_OvertimeType { Id = i };
                result = service.Delete<Cat_OvertimeTypeEntity>(OvertimeType.Id);
                if (result != string.Empty)
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
