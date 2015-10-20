using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_CostCentreServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatCostCentres()
        {
            string status = string.Empty;
            Cat_CostCentreServices service = new Cat_CostCentreServices();
            int count = service.GetAllUseEntity<Cat_CostCentreMultiEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatCostCentres()
        {
            Cat_CostCentreServices service = new Cat_CostCentreServices();
            var costCentre = new Cat_CostCentre { Id = 2 };
            string status = string.Empty;
            var result = service.GetById<Cat_CostCentre>(costCentre.Id, ref status) ;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.CostCentreName + " | " + result.CostCentreCode + " | " + result.GroupCost);
        }

        [Test]
        public void NUnit_Domain_AddCatCostCentre()
        {
            Cat_CostCentreServices service = new Cat_CostCentreServices();
            int rs = 0;
           
            var costCentre = new Cat_CostCentre
            {
                CostCentreName = "NUnit Domain CostCentreName 2" ,
                CostCentreCode = "abc 2",
                GroupCost = "NUnit-GroupCost-2" 
            };
         
            if (service.Add<Cat_CostCentre>(costCentre) == ActionStatus.AddSucceed.ToString())
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Create >>> " + costCentre.Id + " | " + costCentre.CostCentreName + " | " + costCentre.CostCentreCode + " | " + costCentre.GroupCost);
            }
            
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatCostCentre()
        {
            Cat_CostCentreServices service = new Cat_CostCentreServices();
            var costCentre = new Cat_CostCentre
            {
                Id = 2,
                CostCentreName = "ABCDomain1234",
                CostCentreCode = "abc1234",
                GroupCost = "NUnit Test1234"
            };
           // NUnit.Framework.Assert.IsTrue(result);
            if (service.Edit<Cat_CostCentre>(costCentre) ==  ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + costCentre.Id + " | " + costCentre.CostCentreName + " | " + costCentre.CostCentreCode + " | " + costCentre.GroupCost);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatCostCentre()
        {
            Cat_CostCentreServices service = new Cat_CostCentreServices();
            int rs = 0;
           
                var costCentre = new Cat_CostCentre { Id =  1};
                if (service.Delete<Cat_CostCentre>(costCentre.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + 1);
                }
            
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        //[Test]
        //public void NUnit_Domain_DeleteEternityCatCostCentre()
        //{
        //    Cat_CostCentreServices service = new Cat_CostCentreServices();
        //    bool result = true;
        //    int rs = 0;
           
        //        var costCentre = new Cat_CostCentre { Id = 1 };
        //        result = service.Delete(costCentre.Id);
        //        if (result == true)
        //        {
        //            rs += 1;
        //            Console.Write(" Process Success >>> DeleteEternity >>> " + 1);
        //        }
          
        //    Console.WriteLine("Total success record: " + rs);
        //    //NUnit.Framework.Assert.IsTrue(result);
        //}

        
    }
}
