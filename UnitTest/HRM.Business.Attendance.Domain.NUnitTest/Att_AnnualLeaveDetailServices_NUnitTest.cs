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
    public class Att_AnnualLeaveDetailServices_NUnitTest
    {
        public string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_Get()
        {
            Att_AnnualLeaveDetailServices service = new Att_AnnualLeaveDetailServices();
            var repo = service.GetAllUseEntity<Att_AnnualLeaveDetail>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_GetById()
        {
            Att_AnnualLeaveDetailServices service = new Att_AnnualLeaveDetailServices();
            var model = new Att_AnnualLeaveDetail { Id = 5 };
            var result = service.GetById<Att_AnnualLeaveDetail>(model.Id, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.ProfileID
                + " | " + result.LeaveType
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_Add()
        {
            Att_AnnualLeaveDetailServices service = new Att_AnnualLeaveDetailServices();
            var result = "";
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_AnnualLeaveDetail
                {
                    ProfileID = 3,
                    LeaveType = "a"
                };
                result = service.Add<Att_AnnualLeaveDetail>(model);
                NUnit.Framework.Assert.IsNull(result);
                if (result != "")
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.LeaveType
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
            Att_AnnualLeaveDetailServices service = new Att_AnnualLeaveDetailServices();
            var model = new Att_AnnualLeaveDetail
            {
                Id = 2,
                ProfileID = 3,
                LeaveType = "a"
            };
            var result = service.Edit<Att_AnnualLeaveDetail>(model);
            NUnit.Framework.Assert.IsNull(result);
            if (result != "")
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.LeaveType
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_AllowLimitOvertime_Domain_Remove()
        {
            Att_AnnualLeaveDetailServices service = new Att_AnnualLeaveDetailServices();
            int rs = 0;
            var model = new Att_AnnualLeaveDetail { Id = 5 };
            var result = service.Remove<Att_AnnualLeaveDetail>(model.Id);
            NUnit.Framework.Assert.IsNull(result);
            if (result != "")
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
            Att_AnnualLeaveDetailServices service = new Att_AnnualLeaveDetailServices();
            int rs = 0;
            var model = new Att_AnnualLeaveDetail { Id = 4 };
            var result = service.Delete<Att_AnnualLeaveDetail>(model.Id);
            NUnit.Framework.Assert.IsNull(result);
            if (result != "")
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
