using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_EmployeeTypeServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatEmployeeTypes()
        {
            string status = string.Empty;
            Cat_EmployeeTypeServices service = new Cat_EmployeeTypeServices();
            int count = service.GetAllUseEntity<Cat_EmployeeTypeMultiEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatEmployeeTypes()
        {
            string status = string.Empty;
            Cat_EmployeeTypeServices service = new Cat_EmployeeTypeServices();
            var employeeType = new Cat_EmployeeTypeMultiEntity { Id = 2 };
            var result = service.GetById<Cat_EmployeeTypeMultiEntity>(employeeType.Id, ref status) as Cat_EmployeeTypeMultiEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.EmployeeTypeName + " | " + result.CodeID + " | " + result.Notes);
        }

        [Test]
        public void NUnit_Domain_AddCatEmployeeType()
        {
            Cat_EmployeeTypeServices service = new Cat_EmployeeTypeServices();
            string result = string.Empty;
            int rs = 0;
           
            var employeeType = new Cat_EmployeeType
            {
                EmployeeTypeName = "NUnit Domain EmployeeTypeName " ,
                CodeID = 7,
                Notes = "NUnit-Notes-" 
            };
            result = service.Add(employeeType);
            if (result!= string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Create >>> " + employeeType.Id + " | " + employeeType.EmployeeTypeName + " | " + employeeType.CodeID + " | " + employeeType.Notes);
            }
            
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatEmployeeType()
        {
            Cat_EmployeeTypeServices service = new Cat_EmployeeTypeServices();
            var employeeType = new Cat_EmployeeType
            {
                Id = 2,
                EmployeeTypeName = "ABCDomain12",
                CodeID = 12,
                Notes = "NUnit Test12"
            };
            string result = service.Edit(employeeType);
            
            if (result != string.Empty)
            {
                Console.WriteLine("==>>> Result After Update:  " + employeeType.Id + " | " + employeeType.EmployeeTypeName + " | " + employeeType.CodeID + " | " + employeeType.Notes);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatEmployeeType()
        {
            Cat_EmployeeTypeServices service = new Cat_EmployeeTypeServices();
            string result = string.Empty;
            int rs = 0;
           
                var employeeType = new Cat_EmployeeType { Id =  1};
                result = service.Delete<Cat_EmployeeTypeMultiEntity>(employeeType.Id);
                if (result != string.Empty)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + 1);
                }
            
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        [Test]
        public void NUnit_Domain_DeleteEternityCatEmployeeType()
        {
            Cat_EmployeeTypeServices service = new Cat_EmployeeTypeServices();
            string result = string.Empty;
            int rs = 0;
           
                var employeeType = new Cat_EmployeeType { Id = 1 };
                result = service.Delete<Cat_EmployeeTypeMultiEntity>(employeeType.Id);
                if (result != string.Empty)
                {
                    rs += 1;
                    Console.Write(" Process Success >>> DeleteEternity >>> " + 1);
                }
          
            Console.WriteLine("Total success record: " + rs);
            //NUnit.Framework.Assert.IsTrue(result);
        }

        
    }
}
