using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_BankServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatBanks(ListQueryModel model)
        {
            Cat_BankServices service = new Cat_BankServices();
            string status = string.Empty;
            int count = service.GetAllUseEntity<Cat_Bank>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatBanks()
        {
            Cat_BankServices service = new Cat_BankServices();
            var bank = new Cat_Bank { Id = 22 };
            var status = string.Empty;
            var result = service.GetById<Cat_Bank>(bank.Id,ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.BankName + " | " + result.BankCode + " | " + result.Notes);
        }

        [Test]
        public void NUnit_Domain_AddCatBank()
        {
            Cat_BankServices service = new Cat_BankServices();
            int rs=0;
            for (int i = 0; i < 1000000; i++)
            {
                var bank = new Cat_Bank
                {
                    BankName = "NUnit Domain BankName " + i,
                    BankCode = "NUnit-BankCode-" + i,
                    Notes = "NUnit-Notes-"+i
                };
                if (service.Add(bank) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + bank.Id + " | " + bank.BankName + " | " + bank.BankCode + " | " + bank.Notes);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatBank()
        {
            Cat_BankServices service = new Cat_BankServices();
            var bank = new Cat_Bank
            {
                Id = 12,
                BankName = "ABCDomain12",
                BankCode = "BC-12",
                Notes = "NUnit Test12"
            };
            //NUnit.Framework.Assert.IsTrue(result);
            if (service.Edit(bank) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + bank.Id + " | " + bank.BankName + " | " + bank.BankCode + " | " + bank.Notes);
            }
        }

        [Test]
        public void NUnit_Domain_RemoveCatBank()
        {
            Cat_BankServices service = new Cat_BankServices();
            int rs = 0;
            for (int i = 1; i <= 1000000; i++)
            {
                var bank = new Cat_Bank { Id = i };
                if (service.Remove<Cat_Bank>(bank.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        [Test]
        public void NUnit_Domain_DeleteCatBank()
        {
            Cat_BankServices service = new Cat_BankServices();
            int rs = 0;
            for (int i = 1; i <= 1000000; i++)
            {
                var bank = new Cat_Bank { Id = i };
                if (service.Delete<Cat_Bank>(bank.Id) == ActionStatus.DeleteSucceed.ToString())
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
