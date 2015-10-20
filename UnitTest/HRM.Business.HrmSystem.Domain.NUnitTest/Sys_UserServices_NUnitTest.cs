using System;
using HRM.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.HrmSystem.Model;

namespace HRM.Business.HrmSystem.Domain.NUnitTest
{
    [TestFixture]
    public class Sys_UserServices_NUnitTest
    {
        string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_User_Domain_Get()
        {
            Sys_UserServices service = new Sys_UserServices();
           var repo = service.GetAllUseEntity<Sys_User>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_User_Domain_GetById()
        {
            Sys_UserServices service = new Sys_UserServices();
            var model = new Sys_User { Id = 4 };
            var result = service.GetById<Sys_User>(model.Id,ref status) as Sys_User;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.Code
                + " | " + result.UserName 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_User_Domain_Add()
        {
            Sys_UserServices service = new Sys_UserServices();

            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Sys_User
                {
                    Code = "1",
                    LDAPDatasource = "LDAPDatasource",
                    UserName = "UserName " + i
                };
                var result = false;
                if (result = service.Add(model) == "0")
                {
                    result = true;
                }

                NUnit.Framework.Assert.IsTrue(result);

                Console.WriteLine("Total success record: " + result);
            }
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        //[Test]
        //public void NUnit_User_Domain_Edit()
        //{
        //    Sys_UserServices service = new Sys_UserServices();
        //    var model = new Sys_User
        //    {
        //        Id = 3,
        //        LDAPDatasource = "LDAPDatasource",
        //        Code = "1",
        //        UserName = "Update UserName ",
        //    };
        //    var result = service.Edit(model);
        //    NUnit.Framework.Assert.IsTrue(result);
        //    if (result == true)
        //    {
        //        Console.WriteLine("Process Success >>> Update >>> " + model.Id
        //                + " | " + model.Code
        //                + " | " + model.UserName
        //                );
        //    }
        //}

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_User_Domain_Remove()
        {
            Sys_UserServices service = new Sys_UserServices();
            int rs = 0;
            var model = new Sys_User { Id = 1 };
            var result = service.Remove<Sys_User>(model.Id);
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
        public void NUnit_User_Domain_Delete()
        {
            Sys_UserServices service = new Sys_UserServices();
            int rs = 0;
            var model = new Sys_User { Id = 3 };
            var result = service.Delete<Sys_User>(model.Id);
            //NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
