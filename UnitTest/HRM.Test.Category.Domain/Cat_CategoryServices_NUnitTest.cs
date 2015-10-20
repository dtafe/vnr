using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_CategoryServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatCategory()
        {
            Cat_CategoryServices service = new Cat_CategoryServices();
            string status = string.Empty;
            var repo = service.GetAllUseEntity<Cat_Category>(ref status);
            //int count = service.GetCatCategory().Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + repo.Count());
        }

        [Test]
        public void NUnit_Domain_GetByIdCatCategory()
        {
            Cat_CategoryServices service = new Cat_CategoryServices();
            var category = new Cat_Category { Id = 22 };
            string status = string.Empty;
            var result = service.GetById<Cat_Category>(category.Id, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.CategoryName + " | " + result.CategoryCode);
        }

        [Test]
        public void NUnit_Domain_AddCatCategory()
        {
            Cat_CategoryServices service = new Cat_CategoryServices();
            int rs=0;
            for (int i = 0; i < 10; i++)
            {
                var Category = new Cat_Category
                {
                    CategoryName = "NUnit Domain CategoryName " + i,
                    CategoryCode =  "NUnit-CategoryCode-" + i,
                    Description = "NUnit-Description-" + i,
                };
                if (service.Add<Cat_Category>(Category) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + Category.Id + " | " + Category.CategoryName + " | " + Category.Description + " | " + Category.CategoryCode);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatCategory()
        {
            Cat_CategoryServices service = new Cat_CategoryServices();
            var Category = new Cat_Category
            {
                Id = 12,
                CategoryName = "ABCDomain12",
                Description = "BC-12",
                CategoryCode = "123",
            };
            //NUnit.Framework.Assert.IsTrue(result);
            if (service.Edit<Cat_Category>(Category) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + Category.Id + " | " + Category.CategoryName + " | " + Category.Description + " | " + Category.CategoryCode);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatCategory()
        {
            Cat_CategoryServices service = new Cat_CategoryServices();
            int rs = 0;
            for (int i = 1; i <= 10; i++)
            {
                var Category = new Cat_Category { Id = i };
                if (service.Delete<Cat_Category>(Category.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        //[Test]
        //public void NUnit_Domain_DeleteEternityCatCategory()
        //{
        //    Cat_CategoryServices service = new Cat_CategoryServices();
        //    bool result = true;
        //    int rs = 0;
        //    for (int i = 1; i <= 10; i++)
        //    {
        //        var Category = new Cat_Category { Id = i };
        //        result = service.DeleteEternity(Category.Id);
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
