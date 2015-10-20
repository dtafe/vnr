using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_EthnicGroupServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatEthnicGroup()
        {
            string status = string.Empty;
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            int count = service.GetAllUseEntity<Cat_EthnicGroupEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_AddCatEthnicGroup()
        {
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            int rs=0;
            for (int i = 0; i < 10; i++)
            {
                var EthnicGroup = new Cat_EthnicGroup
                {
                    EthnicGroupName = "NUnit Domain EthnicGroupName " + i,
                    EthnicGroupCode = "NUnit-EthnicGroupCode-" + i,
                };
                if (service.Add(EthnicGroup) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + EthnicGroup.Id + " | " + EthnicGroup.EthnicGroupName + " | " + EthnicGroup.EthnicGroupCode);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatEthnicGroup()
        {
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            var EthnicGroup = new Cat_EthnicGroup
            {
                Id = 12,
                EthnicGroupName = "ABCDomain12",
                EthnicGroupCode = "BC-12",
            };
            //NUnit.Framework.Assert.IsTrue(result);
            if (service.Edit(EthnicGroup) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + EthnicGroup.Id + " | " + EthnicGroup.EthnicGroupName + " | " + EthnicGroup.EthnicGroupCode);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatEthnicGroup()
        {
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            int rs = 0;
            for (int i = 1; i <= 10; i++)
            {
                var EthnicGroup = new Cat_EthnicGroup { Id = i };
                if (service.Delete<Cat_EthnicGroup>(EthnicGroup.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        [Test]
        public void NUnit_Domain_DeleteEternityCatEthnicGroup()
        {
            Cat_EthnicGroupServices service = new Cat_EthnicGroupServices();
            int rs = 0;
            for (int i = 1; i <= 10; i++)
            {
                var EthnicGroup = new Cat_EthnicGroup { Id = i };
                if (service.Delete<Cat_EthnicGroup>(EthnicGroup.Id) == ActionStatus.DeleteSucceed.ToString())
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
