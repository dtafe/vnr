using System;
using HRM.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.HrmSystem.Model;

namespace HRM.Business.HrmSystem.Domain.NUnitTest
{
    [TestFixture]
    public class Sys_GroupServices_NUnitTest
    {
        string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_Get()
        {
            Sys_GroupServices service = new Sys_GroupServices();
            var repo = service.GetAllUseEntity<Sys_Group>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_GetById()
        {
            Sys_GroupServices service = new Sys_GroupServices();
            var model = new Sys_Group { Id = 2 };
            var result = service.GetById<Sys_Group>(model.Id, ref status) as Sys_Group;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.Code
                + " | " + result.GroupName 
                );
        }

        ///// <summary>
        ///// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        ///// </summary>
        //[Test]
        //public void NUnit_Group_Domain_Add()
        //{
        //    Sys_GroupServices service = new Sys_GroupServices();
            
        //    int countSC = 0;
        //    for (int i = 1; i <= 10; i++)
        //    {
        //        var model = new Sys_Group
        //        {
        //            Code = "1",
        //            GroupName = "GroupName " + i
        //        };
        //        var result = false;
        //        if(service.Add(model)){
        //            result = true;
        //        }
        //        NUnit.Framework.Assert.IsTrue(result);
          
        //        Console.WriteLine("Total success record: " + result);
              
        //    }
         
        //}

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_Edit()
        {
            Sys_GroupServices service = new Sys_GroupServices();
            var model = new Sys_Group
            {
                Id = 1,
                Code = "1",
                GroupName = "Update GroupName ",
            };
            var result = service.Edit<Sys_Group>(model);
            Console.WriteLine("Result"  + result);
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Group_Domain_Remove()
        {
            Sys_GroupServices service = new Sys_GroupServices();
            int rs = 0;
            var model = new Sys_Group { Id = 1 };
            var result = service.Remove<Sys_Group>(model.Id);
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
        public void NUnit_Group_Domain_Delete()
        {
            Sys_GroupServices service = new Sys_GroupServices();
            int rs = 0;
            var model = new Sys_Group { Id = 1 };
            var result = service.Delete<Sys_Group>(model.Id);
          //  NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
