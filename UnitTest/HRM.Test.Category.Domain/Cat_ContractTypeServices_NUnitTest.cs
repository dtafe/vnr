using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Category.Domain.Test
{
    [TestClass]
    //[TestFixture]
    public class Cat_ContractTypeServices_NUnitTest
    {

      //  [Test]
        public void NUnit_Domain_GetCatContractType()
        {
            string status = string.Empty;
            Cat_ContractTypeServices service = new Cat_ContractTypeServices();
            int count = service.GetAllUseEntity<Cat_ContractTypeEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

      //  [Test]
        public void NUnit_Domain_GetByIdCatContractType()
        {
            string status = string.Empty;
            Cat_ContractTypeServices service = new Cat_ContractTypeServices();
            var ContractType = new Cat_ContractTypeEntity { Id = 2 };
            var result = service.GetById<Cat_ContractTypeEntity>(ContractType.Id, ref status) as Cat_ContractTypeEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.ContractTypeCode + " | " + result.ContractTypeName + " | " + result.Description);
        }

        [TestMethod]
        //[Test]
        public void NUnit_Domain_AddCatContractType()
        {
            Cat_ContractTypeServices service = new Cat_ContractTypeServices();
            string result = string.Empty;
            int rs = 0;
            for (int i = 0; i < 10; i++)
            {
                var ContractType = new Cat_ContractType
                {
                    ContractTypeCode = "tt" + i,
                    ContractTypeName = "Contract Type Name Test" + i,
                    Description = "Description Test" + i
                    
                };
                result = service.Add(ContractType);
                if (result != string.Empty)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + ContractType.Id + " | " + ContractType.ContractTypeCode + " | " + ContractType.ContractTypeName
                        + " | " + ContractType.Description);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        //[Test]
        public void NUnit_Domain_UpdateCatContractType()
        {
            Cat_ContractTypeServices service = new Cat_ContractTypeServices();
            var ContractType = new Cat_ContractType
            {
                Id = 3,
                ContractTypeCode = "Contract Type Name tt",
                ContractTypeName = "Contract Type Name Test",
               Description = "Desctiption test"
            };
            string result = service.Edit(ContractType);
            if (result!= null)
            {
                Console.WriteLine("==>>> Result After Update:  " + ContractType.Id + " | " + ContractType.ContractTypeCode + " | " + ContractType.ContractTypeName
                    + " | " + ContractType.Description);
            }
        }

      //  [Test]
        public void NUnit_Domain_DeleteCatContractType()
        {
            Cat_ContractTypeServices service = new Cat_ContractTypeServices();
            string result = string.Empty;
            int rs = 0;
            for (int i = 3; i <= 5; i++)
            {
                var ContractType = new Cat_ContractType { Id = i };
                result = service.Delete<Cat_ContractTypeEntity>(ContractType.Id);
                if (result != null)
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
