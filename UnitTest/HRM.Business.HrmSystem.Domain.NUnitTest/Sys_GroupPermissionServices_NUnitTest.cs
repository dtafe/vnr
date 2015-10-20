using System;
using HRM.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.HrmSystem.Model;

namespace HRM.Business.HrmSystem.Domain.NUnitTest
{
    [TestFixture]
    public class Sys_GroupPermissionServices_NUnitTest
    {
        string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_GroupPermission_Domain_Get()
        {
            Sys_GroupPermissionServices service = new Sys_GroupPermissionServices();
           var repo = service.GetAllUseEntity<Sys_GroupPermission>(ref status);

            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_GroupPermission_Domain_GetById()
        {
            Sys_GroupPermissionServices service = new Sys_GroupPermissionServices();
            var model = new Sys_GroupPermission { Id = 2 };
            var result = service.GetById<Sys_GroupPermission>(model.Id,ref status) as Sys_GroupPermission;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.Code
                + " | " + result.PrivilegeNumber 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_GroupPermission_Domain_Add()
        {
            Sys_GroupPermissionServices service = new Sys_GroupPermissionServices();
            bool result = true;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Sys_GroupPermission
                {
                    Code = "1",
                    PrivilegeNumber =  i,
                    GroupID = 1,
                    ResourceID = 1,
                };
               var result1 = service.Add<Sys_GroupPermission>(model);
               // NUnit.Framework.Assert.IsTrue(result1);
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
        public void NUnit_Sys_GroupPermission_Domain_Edit()
        {
            Sys_GroupPermissionServices service = new Sys_GroupPermissionServices();
            var model = new Sys_GroupPermission
            {
                Id = 1,
                Code = "1",
                PrivilegeNumber = 2,
            };
            var result = service.Edit<Sys_GroupPermission>(model);
        //    NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.Code
                        + " | " + model.PrivilegeNumber
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_GroupPermission_Domain_Remove()
        {
            Sys_GroupPermissionServices service = new Sys_GroupPermissionServices();
            int rs = 0;
            var model = new Sys_GroupPermission { Id = 1 };
            var result = service.Remove<Sys_GroupPermission>(model.Id);
          //  NUnit.Framework.Assert.IsTrue(result);
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
        public void NUnit_GroupPermission_Domain_Delete()
        {
            Sys_GroupPermissionServices service = new Sys_GroupPermissionServices();
            int rs = 0;
            var model = new Sys_GroupPermission { Id = 1 };
            var result = service.Delete<Sys_GroupPermission>(model.Id);
            //NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
