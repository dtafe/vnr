using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_ReligionServices_NUnitTest
    {
        string status = string.Empty;
        [Test]
        public void NUnit_Domain_GetCatReligion(ListQueryModel model)
        {
            Cat_ReligionServices service = new Cat_ReligionServices();
            //int count = service.GetCatReligion().Count();
            int count = service.GetAllUseEntity<Cat_Religion>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatReligion()
        {
            Cat_ReligionServices service = new Cat_ReligionServices();
            var Religion = new Cat_Religion { Id = 2 };
            //var result = service.GetByIdCatReligion(Religion.Id) as Cat_Religion;
            var result = service.GetById<Cat_Religion>(Religion.Id, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.ReligionName + " | " + result.ReligionCode );
        }

        [Test]
        public void NUnit_Domain_AddCatReligion()
        {
            Cat_ReligionServices service = new Cat_ReligionServices();
            bool result = true;
            int rs = 0;
            for (int i = 0; i < 10; i++)
            {
                var Religion = new Cat_Religion
                {
                    ReligionName = "Religion Name Test" + i,
                    ReligionCode = "tt" + i
                };
                //result = service.AddCatReligion(Religion);
                //if (result == true)
                //{
                //    rs += 1;
                //    Console.WriteLine("Process Success >>> Create >>> " + Religion.Id + " | " + Religion.ReligionName + " | " + Religion.Code );
                //}
                if (service.Add(Religion) == ActionStatus.AddSucceed.ToString())
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + Religion.Id + " | " + Religion.ReligionName + " | " + Religion.ReligionCode);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatReligion()
        {
            Cat_ReligionServices service = new Cat_ReligionServices();
            var Religion = new Cat_Religion
            {
                Id = 3,
                ReligionName = "Update Religion Name Test",
                ReligionCode = "Update tt"
            };
            //bool result = service.UpdateCatReligion(Religion);
            //NUnit.Framework.Assert.IsTrue(result);
            //if (result == true)
            //{
            //    Console.WriteLine("==>>> Result After Update:  " + Religion.Id + " | " + Religion.ReligionName + " | " + Religion.Code );
            //}
            if (service.Edit(Religion) == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("==>>> Result After Update:  " + Religion.Id + " | " + Religion.ReligionName + " | " + Religion.ReligionCode);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatReligion()
        {
            Cat_ReligionServices service = new Cat_ReligionServices();
            bool result = true;
            int rs = 0;
            for (int i = 3; i <= 5; i++)
            {
                var Religion = new Cat_Religion { Id = i };
                //result = service.DeleteCatReligion(Religion.Id);
                if (service.Delete<Cat_Religion>(Religion.Id) == ActionStatus.DeleteSucceed.ToString())
                {
                    rs += 1;
                    Console.Write(" Process Success >>> DeleteEternity >>> " + i);
                }
                if (result == true)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        
}
}
