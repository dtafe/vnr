using System;
using HRM.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.HrmSystem.Model;

namespace HRM.Business.HrmSystem.Domain.NUnitTest
{
    [TestFixture]
    public class Sys_GroupUserServices_NUnitTest
    {
        string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_GroupUser_Domain_Get()
        {
            Sys_GroupUserServices service = new Sys_GroupUserServices();
            var repo = service.GetAllUseEntity<Sys_GroupUser>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_GroupUser_Domain_GetById()
        {
            Sys_GroupUserServices service = new Sys_GroupUserServices();
            var model = new Sys_GroupUser { Id = 1 };
            var result = service.GetById<Sys_GroupUser>(model.Id, ref status) as Sys_GroupUser;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.Code
                + " | " + result.GroupUserName 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_GroupUser_Domain_Add()
        {
            Sys_GroupUserServices service = new Sys_GroupUserServices();
            bool result = true;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Sys_GroupUser
                {
                    Code = "1",
                    GroupID = 1,
                    UserID = 2,
                    GroupUserName = "GroupUserName " + i
                };
              var  result1 = service.Add<Sys_GroupUser>(model);
                NUnit.Framework.Assert.IsTrue(result);
                if (result == true)
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
        public void NUnit_GroupUser_Domain_Edit()
        {
            Sys_GroupUserServices service = new Sys_GroupUserServices();
            var model = new Sys_GroupUser
            {
                Id = 2,
                Code = "1",
                GroupUserName = "Update GroupUserName ",
            };
            var result = service.Edit< Sys_GroupUser >(model);
         //   NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.Code
                        + " | " + model.GroupUserName
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_GroupUser_Domain_Remove()
        {
            Sys_GroupUserServices service = new Sys_GroupUserServices();
            int rs = 0;
            var model = new Sys_GroupUser { Id = 3};
            var result = service.Remove<Sys_GroupUser>(model.Id);
           // NUnit.Framework.Assert.IsTrue(result);
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
        public void NUnit_GroupUser_Domain_Delete()
        {
            Sys_GroupUserServices service = new Sys_GroupUserServices();
            int rs = 0;
            var model = new Sys_GroupUser { Id = 4};
            var result = service.Delete<Sys_GroupUser>(model.Id);
          //  NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
