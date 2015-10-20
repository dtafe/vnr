using System;
using HRM.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Linq;
using HRM.Data.HrmSystem.Model;

namespace HRM.Business.HrmSystem.Domain.NUnitTest
{
    [TestFixture]
    public class Sys_DynamicColumnServices_NUnitTest
    {
        string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_DynamicColumn_Domain_Get()
        {
            Sys_DynamicColumnServices service = new Sys_DynamicColumnServices();
            var repo = service.GetAllUseEntity<Sys_DynamicColumn>(ref status); ;
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_DynamicColumn_Domain_GetById()
        {
            Sys_DynamicColumnServices service = new Sys_DynamicColumnServices();
            var model = new Sys_DynamicColumn { Id = 75 };
            var result = service.GetById<Sys_DynamicColumn>(model.Id,ref status) as Sys_DynamicColumn;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id 
                + " | " + result.Code
                + " | " + result.ColumnName 
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        //[Test]
        //public void NUnit_DynamicColumn_Domain_Add()
        //{
        //    Sys_DynamicColumnServices service = new Sys_DynamicColumnServices();
           
        //    int countSC = 0;
        //    for (int i = 1; i <= 10; i++)
        //    {
        //        var model = new Sys_DynamicColumn
        //        {
        //            Code = "1",
        //            ColumnName = "ColumnName " + i,
        //            TableName = "Att_AttendanceTableItem",
        //            DataType = "Int"
        //        };
        //        var result = false;
        //        if (service.Add(model) == "0")
        //        {

        //            result = true;
        //        }
        //        result = service.Add(model);
        //        NUnit.Framework.Assert.IsTrue(result);
        //        Console.WriteLine("Total success record: " + result);
              
        //    }
        //    Console.WriteLine("Total success record: " + countSC);
        //}

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_DynamicColumn_Domain_Edit()
        {
            Sys_DynamicColumnServices service = new Sys_DynamicColumnServices();
            var model = new Sys_DynamicColumn
            {
                Id = 67,
                Code = "1",
                ColumnName = "test ",
            };
            var result = service.Edit<Sys_DynamicColumn>(model);
         //   NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.Code
                        + " | " + model.ColumnName
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_DynamicColumn_Domain_Remove()
        {
            Sys_DynamicColumnServices service = new Sys_DynamicColumnServices();
            int rs = 0;
            var model = new Sys_DynamicColumn { Id = 78 };
            var result = service.Remove<Sys_DynamicColumn>(model.Id);
         //   NUnit.Framework.Assert.IsTrue(result);
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
        public void NUnit_DynamicColumn_Domain_Delete()
        {
            Sys_DynamicColumnServices service = new Sys_DynamicColumnServices();
            int rs = 0;
            var model = new Sys_DynamicColumn { Id = 79 };
            var result = service.Delete<Sys_DynamicColumn>(model.Id);
          //  NUnit.Framework.Assert.IsTrue(result);
            if (result == ActionStatus.EditSucceed.ToString())
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
