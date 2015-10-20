using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_PositionServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatPositions()
        {
            string status = string.Empty;
            Cat_PositionServices service = new Cat_PositionServices();
            int count = service.GetAllUseEntity<Cat_PositionEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatPositions()
        {
            string status = string.Empty;
            Cat_PositionServices service = new Cat_PositionServices();
            var position = new Cat_Position { Id = 1 };
            var result = service.GetById<Cat_PositionEntity>(position.Id, ref status) as Cat_PositionEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.PositionName + " | " + result.PositionCode + " | " + result.Description);
        }

        [Test]
        public void NUnit_Domain_AddCatPosition()
        {
            Cat_PositionServices service = new Cat_PositionServices();
            string result = string.Empty;
            int rs = 0;
           
            var position = new Cat_Position
            {
                PositionName = "NUnit Domain PositionName 2" ,
                PositionCode = "abc 2",
                Description = "NUnit-Description-2" 
            };
            result = service.Add(position);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Create >>> " + position.Id + " | " + position.PositionName + " | " + position.PositionCode + " | " + position.Description);
            }
            
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatPosition()
        {
            Cat_PositionServices service = new Cat_PositionServices();
            var position = new Cat_Position
            {
                Id = 1,
                PositionName = "ABCDomain1234",
                PositionCode = "abc1234",
                Description = "NUnit Test1234"
            };
            string result = service.Edit(position);
            if (result!= string.Empty)
            {
                Console.WriteLine("==>>> Result After Update:  " + position.Id + " | " + position.PositionName + " | " + position.PositionCode + " | " + position.Description);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatPosition()
        {
            Cat_PositionServices service = new Cat_PositionServices();
            string result = string.Empty;
            int rs = 0;
           
                var position = new Cat_Position { Id =  1};
                result = service.Delete<Cat_PositionEntity>(position.Id);
                if (result != string.Empty)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + 1);
                }
            
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        [Test]
        public void NUnit_Domain_DeleteEternityCatPosition()
        {
            Cat_PositionServices service = new Cat_PositionServices();
            string result = string.Empty;
            int rs = 0;
           
                var position = new Cat_Position { Id = 1 };
                result = service.Delete<Cat_PositionEntity>(position.Id);
                if (result != null)
                {
                    rs += 1;
                    Console.Write(" Process Success >>> DeleteEternity >>> " + 1);
                }
          
            Console.WriteLine("Total success record: " + rs);
            //NUnit.Framework.Assert.IsTrue(result);
        }

        
    }
}
