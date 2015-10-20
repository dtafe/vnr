using System;
using System.Linq;
using HRM.Business.Category.Domain;
using HRM.Business.Hr.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using HRM.Data.Attendance.Model;
using HRM.Business.Attendance.Models;

namespace HRM.Business.Attendance.Domain.NUnitTest
{
    [TestFixture]
    public class Att_TAMScanLogServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_TAMScanLog_Domain_Get()
        {
            //Att_TAMScanLogServices service = new Att_TAMScanLogServices();
            //IQueryable<Att_TAMScanLog> repo = service.Get();
            //Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_TAMScanLog_Domain_GetById()
        {
            string status = string.Empty;
            Att_TAMScanLogServices service = new Att_TAMScanLogServices();
            var model = new Att_TAMScanLog { Id = 1 };
            var result = service.GetById<Att_TAMScanLogEntity>(model.Id, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.CardCode

                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_TAMScanLog_Domain_Add()
        {
            Att_TAMScanLogServices service = new Att_TAMScanLogServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_TAMScanLog
                {
                    Comment = "Comment : " + 1
                };
                result = service.Add(model);
                if (result != string.Empty)
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
        [Test]
        public void NUnit_TAMScanLog_Domain_Edit()
        {
            Att_TAMScanLogServices service = new Att_TAMScanLogServices();
            var model = new Att_TAMScanLog
            {
                Id = 1
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.CardCode
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_TAMScanLog_Domain_Remove()
        {
            Att_TAMScanLogServices service = new Att_TAMScanLogServices();
            int rs = 0;
            var model = new Att_TAMScanLog { Id = 1 };
            string result = service.Remove<Att_TAMScanLogEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Remove >>> " + model.Id);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo Id có trong table
        /// </summary>
        [Test]
        public void NUnit_TAMScanLog_Domain_Delete()
        {
            Att_TAMScanLogServices service = new Att_TAMScanLogServices();
            int rs = 0;
            var model = new Att_TAMScanLog { Id = 3 };
            string result = service.Delete<Att_TAMScanLogEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }
    }
}
