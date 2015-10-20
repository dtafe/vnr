using System;
using HRM.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.HrmSystem.Model;

namespace HRM.Business.HrmSystem.Domain.NUnitTest
{
    [TestFixture]
    public class Sys_ResourceServices_NUnitTest
    {
        string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_Get()
        {
            Sys_ResourceServices service = new Sys_ResourceServices();
            var repo = service.GetAllUseEntity<Sys_Resource>(ref status);

            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_GetById()
        {
            Sys_ResourceServices service = new Sys_ResourceServices();
            var model = new Sys_Resource { Id = 3 };
            var result = service.GetById<Sys_Resource>(model.Id,ref status) as Sys_Resource;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.Code
                + " | " + result.ResourceName 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_Add()
        {
            Sys_ResourceServices service = new Sys_ResourceServices();
            bool result = true;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                
                var model = new Sys_Resource
                {
                    Id = i,
                    Code = i.ToString(),
                    ResourceName = "ResourceName " + i
                };
              var  result1 = service.Add<Sys_Resource>(model);
                //NUnit.Framework.Assert.IsTrue(result);
              if (result1 == ActionStatus.EditSucceed.ToString())
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.Code
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_Edit()
        {
            Sys_ResourceServices service = new Sys_ResourceServices();
            var model = new Sys_Resource
            {
                Id = 3,
                Code = "3",
                ResourceName = "Update ResourceName ",
            };
            var result = service.Edit<Sys_Resource>(model);
            //NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.Code
                        + " | " + model.ResourceName
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_Remove()
        {
            Sys_ResourceServices service = new Sys_ResourceServices();
            int rs = 0;
            var model = new Sys_Resource { Id = 4 };
            var result = service.Delete<Sys_Resource>(model.Id);
            //NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo Id có trong table
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_Delete()
        {
            Sys_ResourceServices service = new Sys_ResourceServices();
            int rs = 0;
            var model = new Sys_Resource { Id = 5 };
            var result = service.Delete<Sys_Resource>(model.Id);
            //NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
