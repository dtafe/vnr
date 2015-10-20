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
using HRM.Infrastructure.Utilities;


namespace HRM.Business.Attendance.Domain.NUnitTest
{
    [TestClass]
    ////[TestFixture]
    public class Att_AnnualLeaveServices_NUnitTest
    {
        public string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_AnnualLeave_Domain_Get()
        {
            Att_AnnualLeaveServices service = new Att_AnnualLeaveServices();
            var repo = service.GetAllUseEntity<Att_AnnualLeaveEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_AnnualLeave_Domain_GetById()
        {
            Att_AnnualLeaveServices service = new Att_AnnualLeaveServices();
            var model = new Att_AnnualLeave { Id = 5 };
            var result = service.GetByIdUseStore<Att_AnnualLeaveEntity>(model.Id, ConstantSql.hrm_att_sp_get_AnnualLeaveById, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.ProfileID
                + " | " + result.Year
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_AnnualLeave_Domain_Add()
        {
            Att_AnnualLeaveServices service = new Att_AnnualLeaveServices();
            var result = "";
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_AnnualLeave
                {
                    ProfileID = 3,
                    Year =  i,
                    MonthStart=i,
                    InitAnlValue=i,
                    InitSickValue =i,
                    InitSaveSickValue=i,
                };
                result = service.Add<Att_AnnualLeave>(model);
                NUnit.Framework.Assert.IsNull(result);
                if (result != "")
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.Year
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_AnnualLeave_Domain_Edit()
        {
            Att_AnnualLeaveServices service = new Att_AnnualLeaveServices();
            var model = new Att_AnnualLeave
            {
                Id = 2,
                ProfileID = 3,
                Year = 2015,
                InitAnlValue = 1,
                InitSickValue = 1,
                InitSaveSickValue = 1,
            };
            var result = service.Edit<Att_AnnualLeave>(model);
            NUnit.Framework.Assert.IsNull(result);
            if (result != "")
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.Year
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_AnnualLeave_Domain_Remove()
        {
            Att_AnnualLeaveServices service = new Att_AnnualLeaveServices();
            int rs = 0;
            var model = new Att_AnnualLeave { Id = 5 };
            var result = service.Remove<Att_AnnualLeave>(model.Id);
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
        public void NUnit_AnnualLeave_Domain_Delete()
        {
            Att_AnnualLeaveServices service = new Att_AnnualLeaveServices();
            int rs = 0;
            var model = new Att_AnnualLeave { Id = 4 };
            var result = service.Delete<Att_AnnualLeave>(model.Id);
            NUnit.Framework.Assert.IsNull(result);
            if (result != "")
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
