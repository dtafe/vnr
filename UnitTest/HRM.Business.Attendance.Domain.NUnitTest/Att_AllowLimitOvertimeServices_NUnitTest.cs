using System;
using System.Linq;
using NUnit.Framework;
using HRM.Business.Attendance.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRM.Business.Hr.Domain;
using HRM.Data.Attendance.Model;
using HRM.Business.Attendance.Models;
using System.Collections.Generic;
using HRM.Business.Category.Models;


namespace HRM.Business.Attendance.Domain.NUnitTest
{
    [TestClass]
    ////[TestFixture]
    public class Att_AllowLimitOvertimeServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_Get()
        {
            Att_AllowLimitOvertimeServices service = new Att_AllowLimitOvertimeServices();
            IQueryable<Att_AllowLimitOvertime> repo = service.Get();
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_GetById()
        {
            Att_AllowLimitOvertimeServices service = new Att_AllowLimitOvertimeServices();
            var model = new Att_AllowLimitOvertime { Id = 5 };
            var result = service.Get(model.Id) as Att_AllowLimitOvertime;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.ProfileID
                + " | " + result.LevelAllowLimit
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_Add()
        {
            Att_AllowLimitOvertimeServices service = new Att_AllowLimitOvertimeServices();
            bool result = true;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_AllowLimitOvertime
                {
                    ProfileID = 3,
                    LevelAllowLimit = 4
                };
                result = service.Add(model);
                NUnit.Framework.Assert.IsTrue(result);
                if (result == true)
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.LevelAllowLimit
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_Edit()
        {
            Att_AllowLimitOvertimeServices service = new Att_AllowLimitOvertimeServices();
            var model = new Att_AllowLimitOvertime
            {
                Id = 2,
                ProfileID = 3,
                LevelAllowLimit = 4
            };
            bool result = service.Edit(model);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.LevelAllowLimit
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_Remove()
        {
            Att_AllowLimitOvertimeServices service = new Att_AllowLimitOvertimeServices();
            int rs = 0;
            var model = new Att_AllowLimitOvertime { Id = 5 };
            bool result = service.Remove(model.Id);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo Id có trong table
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_Delete()
        {
            Att_AllowLimitOvertimeServices service = new Att_AllowLimitOvertimeServices();
            int rs = 0;
            var model = new Att_AllowLimitOvertime { Id = 4 };
            bool result = service.Delete(model.Id);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
