using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_ShiftServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatShift()
        {
            string status = string.Empty;
            Cat_ShiftServices service = new Cat_ShiftServices();
            var count = service.GetAllUseEntity<Cat_ShiftEntity>(ref status);
            int count1 = count.Count();
            Console.Write("Total Record: " + count1);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatShift()
        {
            string status = string.Empty;
            Cat_ShiftServices service = new Cat_ShiftServices();
            var Shift = new Cat_Shift { Id = 2 };
            var result = service.GetById<Cat_ShiftEntity>(Shift.Id, ref status) as Cat_ShiftEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.ShiftName + " | " + result.ShiftCode + " | " + result.InTime + " | " + result.CoOut + " | " + result.CoBreakIn + " | " + result.CoBreakOut);
        }

        [Test]
        public void NUnit_Domain_AddCatShift()
        {
            Cat_ShiftServices service = new Cat_ShiftServices();
            string result = string.Empty;
            int rs = 0;
            for (int i = 0; i < 10; i++)
            {
                var Shift = new Cat_Shift
                {
                    ShiftName = "NUnit Domain ShiftName " + i,
                    ShiftCode = "NUnit-Code-" + i,
                };
                result = service.Add(Shift);
                if (result != string.Empty)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + Shift.Id + " | " + Shift.ShiftName + " | " + Shift.ShiftCode + " | " + Shift.InTime + " | " + Shift.CoOut + " | " + Shift.CoBreakIn + " | " + Shift.CoBreakOut);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatShift()
        {
            Cat_ShiftServices service = new Cat_ShiftServices();
            var Shift = new Cat_Shift
            {
                Id = 12,
                ShiftName = "ABCDomain12",
                ShiftCode = "BC-12",
            };
            string result = service.Edit(Shift);
            if (result != string.Empty)
            {
                Console.WriteLine("==>>> Result After Update:  " + Shift.Id + " | " + Shift.ShiftName + " | " + Shift.ShiftCode + " | " + Shift.InTime + " | " + Shift.CoOut + " | " + Shift.CoBreakIn + " | " + Shift.CoBreakOut);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatShift()
        {
            Cat_ShiftServices service = new Cat_ShiftServices();
            string result = string.Empty;
            int rs = 0;
            for (int i = 1; i <= 10; i++)
            {
                var Shift = new Cat_Shift { Id = i };
                result = service.Delete<Cat_ShiftEntity>(Shift.Id);
                if (result != string.Empty)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        [Test]
        public void NUnit_Domain_DeleteEternityCatShift()
        {
            Cat_ShiftServices service = new Cat_ShiftServices();
            string result = string.Empty;
            int rs = 0;
            for (int i = 1; i <= 10; i++)
            {
                var Shift = new Cat_Shift { Id = i };
                result = service.Delete<Cat_ShiftEntity>(Shift.Id);
                if (result != string.Empty)
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
