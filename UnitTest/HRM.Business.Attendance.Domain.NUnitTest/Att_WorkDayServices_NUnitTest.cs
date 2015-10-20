using System;
using System.Linq;
using NUnit.Framework;
using HRM.Business.Attendance.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRM.Business.Hr.Domain;
using HRM.Data.Attendance.Model;
using System.Collections.Generic;
using HRM.Business.Hr.Models;
using HRM.Business.Attendance.Models;
using HRM.Data.Category.Model;
using HRM.Business.Category.Models;
using HRM.Data.Attendance.Data;
//using HRM.Data.Category.Data;
//using HRM.Data.Sql;
//using HRM.Data.Attendance.Sql;
using HRM.Data.Main.Sql;
using HRM.Data.Hr.Model;
using HRM.Infrastructure.Utilities;

namespace HRM.Business.Attendance.Domain.NUnitTest
{
    //[TestFixture]
    [TestClass]
    public class Att_WorkDayServices_NUnitTest
    {
        /// <summary>
        /// Lấy tổng số bản ghi có trong table
        /// </summary>
        [Test]
        public void NUnit_WorkDay_Domain_Get()
        {
            Att_WorkDayServices service = new Att_WorkDayServices();
            string status = string.Empty;
            var repo = service.GetAllUseEntity<Att_WorkDayEntity>(ref status);
            Console.Write("Total Record: " + repo.Count());
        }

        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_WorkDay_Domain_GetById()
        {
            string status = string.Empty;
            Att_WorkDayServices service = new Att_WorkDayServices();
            var model = new Att_WorkDay { Id = 3 };
            var result = service.GetById<Att_WorkDayEntity>(model.Id, ref status) as Att_WorkDayEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.ProfileID
                + " | " + result.WorkDate
                + " | " + result.FirstInTime
                //+ " | " + result.FirstOutTime
                //+ " | " + result.LastInTime
                + " | " + result.LastOutTime
                + " | " + result.WorkDuration
                + " | " + result.EarlyDuration1
                + " | " + result.LateDuration1
                + " | " + result.LateEarlyDuration
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_WorkDay_Domain_Add()
        {
            Att_WorkDayServices service = new Att_WorkDayServices();
            string result = string.Empty;
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_WorkDay
                {
                    ProfileID = 1,
                    WorkDate = DateTime.Parse("10-05-14"),
                    FirstInTime = DateTime.Parse("10-05-14 08:00:00"),
                    LastOutTime = DateTime.Parse("10-05-14 23:00:00"),
                    WorkDuration = 15
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
        public void NUnit_WorkDay_Domain_Edit()
        {
            Att_WorkDayServices service = new Att_WorkDayServices();
            var model = new Att_WorkDay
            {
                Id = 5,
                ProfileID = 1
            };
            string result = service.Edit(model);
            if (result != string.Empty)
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.WorkDate
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_WorkDay_Domain_Remove()
        {
            Att_WorkDayServices service = new Att_WorkDayServices();
            int rs = 0;
            var model = new Att_WorkDay { Id = 3 };
            string result = service.Remove<Att_WorkDayEntity>(model.Id);
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
        public void NUnit_WorkDay_Domain_Delete()
        {
            Att_WorkDayServices service = new Att_WorkDayServices();
            int rs = 0;
            var model = new Att_WorkDay { Id = 3 };
            string result = service.Delete<Att_WorkDayEntity>(model.Id);
            if (result != string.Empty)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }

        //[Test]
        [TestMethod]
        public void NUnit_WorkDay_Domain_SummaryInOut()
        {
            #region listShift
            using (var contextCategory = new VnrHrmDataContext())
            {
                List<Cat_Shift> listShift = new List<Cat_Shift>();
                Cat_Shift _shift = new Cat_Shift()
                {
                    Id = 1,
                    ShiftName = "Ca Test",
                    InTime = DateTime.Parse("01-09-14 08:00:00"),
                    CoOut = 9,
                    CoBreakIn = 4,
                    CoBreakOut = 5,
                };
                listShift.Add(_shift);
                contextCategory.Cat_Shift.Add(_shift);
                contextCategory.SaveChanges();
            }
            #endregion
            #region listRoster
            using (var contextAtt = new VnrHrmDataContext())
            {
                List<Att_Roster> listRoster = new List<Att_Roster>();
                Att_Roster _roster = new Att_Roster()
                {
                    ProfileID = 1,
                    RosterGroupName = "Nhom 1",
                    Type = "E_DEFAULT",
                    Status = "",
                    DateEnd = DateTime.Parse("01-03-2014"),
                    DateStart = DateTime.Parse("01-02-2014"),
                    MonShiftID = 1,
                    TueShiftID = 1,
                    WedShiftID = 1,
                    ThuShiftID = 1,
                    FriShiftID = 1,
                    SatShiftID = 1,
                    SunShiftID = 1
                };
                listRoster.Add(_roster);
                contextAtt.Att_Roster.Add(_roster);
                contextAtt.SaveChanges();
                #endregion
                #region listTamScanLog
                List<Att_TAMScanLog> listTamScanLog = new List<Att_TAMScanLog>();
                Att_TAMScanLog _tamScanLog1 = new Att_TAMScanLog()
                {
                    CardCode = "123",
                    TimeLog = DateTime.Parse("01-02-2014 08:00:00"),
                    SrcType = "E_IN",
                    Type = ""
                };
                Att_TAMScanLog _tamScanLog2 = new Att_TAMScanLog()
                {
                    CardCode = "123",
                    TimeLog = DateTime.Parse("01-02-2014 17:00:00"),
                    SrcType = "E_OUT",
                    Type = ""
                };
                Att_TAMScanLog _tamScanLog3 = new Att_TAMScanLog()
                {
                    CardCode = "123",
                    TimeLog = DateTime.Parse("02-02-2014 08:00:00"),
                    SrcType = "E_IN",
                    Type = ""
                };
                Att_TAMScanLog _tamScanLog4 = new Att_TAMScanLog()
                {
                    CardCode = "123",
                    TimeLog = DateTime.Parse("02-02-2014 17:00:00"),
                    SrcType = "E_OUT",
                    Type = ""
                };
                listTamScanLog.Add(_tamScanLog1);
                listTamScanLog.Add(_tamScanLog2);
                listTamScanLog.Add(_tamScanLog3);
                listTamScanLog.Add(_tamScanLog4);
                contextAtt.Att_TAMScanLog.Add(_tamScanLog1);
                contextAtt.Att_TAMScanLog.Add(_tamScanLog2);
                contextAtt.Att_TAMScanLog.Add(_tamScanLog3);
                contextAtt.Att_TAMScanLog.Add(_tamScanLog4);
                contextAtt.SaveChanges();
            }
            #endregion
            # region CardHistory
            using (var contextHr = new VnrHrmDataContext())
            {
                List<Hre_CardHistory> listcardhistory = new List<Hre_CardHistory>();
                Hre_CardHistory cardhistory = new Hre_CardHistory()
                {
                    ProfileID = 1,
                    CardCode = "123",
                    DateEffect = DateTime.Parse("01-01-2014")
                };
                listcardhistory.Add(cardhistory);
                contextHr.Hre_CardHistory.Add(cardhistory);
                contextHr.SaveChanges();
            }
            #endregion
            DateTime dateFrom = DateTime.Parse("01-02-2014");
            DateTime dateTo = DateTime.Parse("01-03-2014");

            List<int> listSelectedProfileID = new List<int>(){1,2};

            Att_WorkDayServices service = new Att_WorkDayServices();
            //List<Att_WorkDayEntity> ListInOut = service.SummaryInOut(dateFrom, dateTo, listSelectedProfileID);
        }
    }
}
