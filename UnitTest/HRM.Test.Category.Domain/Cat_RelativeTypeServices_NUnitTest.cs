using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_RelativeTypeServices_NUnitTest
    {
        string status = string.Empty;
        [Test]
        public void NUnit_Domain_GetCatRelativeType(ListQueryModel model)
        {
            Cat_RelativeTypeServices service = new Cat_RelativeTypeServices();
            //int count = service.Get().Count();
            int count = service.GetAllUseEntity<Cat_RelativeType>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        //[Test]
        //public void NUnit_Domain_GetByIdCatRelativeType()
        //{
        //    Cat_RelativeTypeServices service = new Cat_RelativeTypeServices();
        //    var RelativeType = new Cat_RelativeType { Id = 2 };
        //    var result = service.GetById(RelativeType.Id) as Cat_RelativeType;
        //    NUnit.Framework.Assert.IsNotNull(result);
        //    Console.Write("SearchResult: " + result.Id + " | " + result.RelativeTypeCode + " | " + result.RelativeTypeName);
        //}

        [Test]
        public void NUnit_Domain_AddCatRelativeType()
        {
            Cat_RelativeTypeServices service = new Cat_RelativeTypeServices();
            bool result = true;
            int rs = 0;
            for (int i = 0; i < 10; i++)
            {
                var RelativeType = new Cat_RelativeType
                {
                    RelativeTypeCode = "tt" + i,
                    RelativeTypeName = "RelativeType Name Test" + i,
                };
                //result = service.Add(RelativeType);
                //if (result == true)
                //{
                //    rs += 1;
                //    Console.WriteLine("Process Success >>> Create >>> " + RelativeType.Id + " | " + RelativeType.RelativeTypeCode + " | " + RelativeType.RelativeTypeName);
                //}
                if (service.Add(RelativeType) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + RelativeType.Id + " | " + RelativeType.RelativeTypeName + " | " + RelativeType.RelativeTypeCode);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatRelativeType()
        {
            Cat_RelativeTypeServices service = new Cat_RelativeTypeServices();
            var RelativeType = new Cat_RelativeType
            {
                Id = 3,
                RelativeTypeCode = "UD tt",
                RelativeTypeName = "UD Name Test",
            };
            //bool result = service.Edit(RelativeType);
            //NUnit.Framework.Assert.IsTrue(result);
            //if (result == true)
            //{
            //    Console.WriteLine("==>>> Result After Update:  " + RelativeType.Id + " | " + RelativeType.RelativeTypeCode + " | " + RelativeType.RelativeTypeName);
            //}
            if (service.Edit(RelativeType) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + RelativeType.Id + " | " + RelativeType.RelativeTypeName + " | " + RelativeType.RelativeTypeCode);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatRelativeType()
        {
            Cat_RelativeTypeServices service = new Cat_RelativeTypeServices();
            bool result = true;
            int rs = 0;
            for (int i = 3; i <= 5; i++)
            {
                var RelativeType = new Cat_RelativeType { Id = i };
                //result = service.Delete(RelativeType.Id);
                //if (result == true)
                //{
                //    rs += 1;
                //    Console.WriteLine("Process Success >>> Delete >>> " + i);
                //}
                if (service.Remove<Cat_RelativeType>(RelativeType.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs); 
        }

        //[Test]
        //public void NUnit_Domain_DeleteEternityCatRelativeType()
        //{
        //    Cat_RelativeTypeServices service = new Cat_RelativeTypeServices();
        //    bool result = true;
        //    int rs = 0;
        //    for (int i = 4; i <= 6; i++)
        //    {
        //        var RelativeType = new Cat_RelativeType { Id = i };
        //        result = service.DeleteEternity(RelativeType.Id);
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
