using System;
using System.Linq;
using HRM.Business.Category.Domain;
using HRM.Business.Hr.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using HRM.Data.Attendance.Model;
using HRM.Infrastructure.Utilities;
using HRM.Business.Attendance.Models;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Domain.NUnitTest
{
    //[TestClass] // Để debug 
    [TestFixture]
    public class Att_CutOffDurationServices_NUnitTest
    {
        public string status = string.Empty;
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_CutOffDuration_Domain_Get(ListQueryModel model)
        {
            Att_CutOffDurationServices service = new Att_CutOffDurationServices();
            var repo = service.GetDataCondition<Att_CutOffDurationEntity>(model, ConstantSql.hrm_att_sp_get_CutOffDurations, ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_CutOffDuration_Domain_GetById()
        {
            Att_CutOffDurationServices service = new Att_CutOffDurationServices();
            var model = new Att_CutOffDuration { Id = 1 };
            var result = service.GetById<Att_CutOffDuration>(model.Id,ref status) as Att_CutOffDuration;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.CutOffDurationName

                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_CutOffDuration_Domain_Add()
        {
            Att_CutOffDurationServices service = new Att_CutOffDurationServices();
            var result = "";
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_CutOffDuration
                {
                    
                    CutOffDurationName = "CutOffDurationName : " + 1,
                    MonthYear = DateTime.Now,
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    IsInsuranceSocial = true
                };
                result = service.Add<Att_CutOffDuration>(model);
                NUnit.Framework.Assert.IsNull(result);
                if (result != "")
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        //[TestMethod]
        [Test]
        public void NUnit_CutOffDuration_Domain_Edit()
        {
            Att_CutOffDurationServices service = new Att_CutOffDurationServices();
            var model = new Att_CutOffDuration
            {
                Id = 5,
                MonthYear = DateTime.Now,
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now
            };
            var result = service.Edit<Att_CutOffDuration>(model);
            NUnit.Framework.Assert.IsNull(result);
            if (result != null)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id);
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_CutOffDuration_Domain_Remove()
        {
            Att_CutOffDurationServices service = new Att_CutOffDurationServices();
            int rs = 0;
            var model = new Att_CutOffDuration { Id = 1 };
            var result = service.Remove<Att_CutOffDuration>(model.Id);
            NUnit.Framework.Assert.IsNull(result);
            if (result != null)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo Id có trong table
        /// </summary>
        [Test]
        public void NUnit_CutOffDuration_Domain_Delete()
        {
            Att_CutOffDurationServices service = new Att_CutOffDurationServices();
            int rs = 0;
            var model = new Att_CutOffDuration { Id = 2 };
            var result = service.Delete<Att_CutOffDuration>(model.Id);
            NUnit.Framework.Assert.IsNull(result);
            if (result != null)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
