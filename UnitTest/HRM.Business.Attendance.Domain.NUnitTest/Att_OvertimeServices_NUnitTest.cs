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
    //[TestFixture]
    public class Att_OvertimeServices_NUnitTest
    {                
        /// <summary>
        /// Tìm kiếm dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Overtime_Domain_GetById()
        {
            Att_OvertimeServices service = new Att_OvertimeServices();
            var status = string.Empty;
            var model = new Att_Overtime { Id = 5 };
            var result = service.GetById<Att_OvertimeEntity>(model.Id, ref status);
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id
                + " | " + result.ProfileID
                + " | " + result.MethodPayment
                );
        }

        /// <summary>
        /// Thêm mới 10 bản ghi, in ra các bản ghi và tổng số bản ghi insert thành công
        /// </summary>
        [Test]
        public void NUnit_Overtime_Domain_Add()
        {
            Att_OvertimeServices service = new Att_OvertimeServices();
            var result = "";
            int countSC = 0;
            for (int i = 1; i <= 10; i++)
            {
                var model = new Att_Overtime
                {
                    ProfileID = 1,
                    MethodPayment = "MethodPayment " + i,
                };
                result = service.Add<Att_Overtime>(model);
                NUnit.Framework.Assert.IsNull(result);
                if (result != "")
                {
                    countSC += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id
                        + " | " + model.MethodPayment
                        );
                }
            }
            Console.WriteLine("Total success record: " + countSC);
        }

        /// <summary>
        /// Update dữ liệu theo Id
        /// </summary>
        [Test]
        public void NUnit_Overtime_Domain_Edit()
        {
            Att_OvertimeServices service = new Att_OvertimeServices();
            var model = new Att_Overtime
            {
                Id = 5,
                ProfileID = 1,
                MethodPayment = "Update Name ",
            };
            var result = service.Edit<Att_Overtime>(model);
            NUnit.Framework.Assert.IsNull(result);
            if (result != "")
            {
                Console.WriteLine("Process Success >>> Update >>> " + model.Id
                        + " | " + model.ProfileID
                        + " | " + model.MethodPayment
                        );
            }
        }

        /// <summary>
        /// Chuyển đổi trạng thái IsDelete của bản ghi thành true, 
        /// </summary>
        [Test]
        public void NUnit_Overtime_Domain_Remove()
        {
            Att_OvertimeServices service = new Att_OvertimeServices();
            int rs = 0;
            var model = new Att_Overtime { Id = 5 };
            var result = service.Remove<Att_Overtime>(model.Id);
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
        public void NUnit_Overtime_Domain_Delete()
        {
            Att_OvertimeServices service = new Att_OvertimeServices();
            int rs = 0;
            var model = new Att_Overtime { Id = 4 };
            var result = service.Delete<Att_Overtime>(model.Id);
            NUnit.Framework.Assert.IsNull(result);
            if (result != "")
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Delete >>> " + model.Id);
            }
        }

        [TestMethod]
        //[Test]
        public void NUnit_Overtime_Domain_AnalyzeOvertime()
        {
            #region listWorkDay
            List<Att_WorkDayEntity> listWorkDay = new List<Att_WorkDayEntity>();
            Att_WorkDayEntity _workDayEntity = new Att_WorkDayEntity()
            {
                ProfileID = 1,
                WorkDate = DateTime.Parse("01-09-14"),
                FirstInTime = DateTime.Parse("01-09-14 07:00:00"),
                LastOutTime = DateTime.Parse("02-09-14 07:00:00"),
                InTime1 = DateTime.Parse("01-09-14 07:00:00"),
                OutTime1 = DateTime.Parse("02-09-14 07:00:00"),
                ShiftID = 1,
                WorkDuration = (float)14.5
            };
            listWorkDay.Add(_workDayEntity); 
            #endregion

            #region listShift
            List<Cat_ShiftEntity> listShift = new List<Cat_ShiftEntity>();
            Cat_ShiftEntity _shiftEntity = new Cat_ShiftEntity()
            {
                Id = 1,
                ShiftName = "Ca Test",
                InTime = DateTime.Parse("01-09-14 07:00:00"),
                CoOut = 9.5,
                CoBreakIn = 4,
                CoBreakOut = 5.5,
                NightTimeStart = DateTime.Parse("01-09-14 21:00:00"),
                NightTimeEnd = DateTime.Parse("02-09-14 05:00:00")
            };
            listShift.Add(_shiftEntity); 
            #endregion

            #region listShiftItem
            List<Cat_ShiftItemEntity> listShiftItem = new List<Cat_ShiftItemEntity>();
            Cat_ShiftItemEntity _shiftItemEntity1 = new Cat_ShiftItemEntity()
            {
                ShiftItemName = "Giao Ca Sang",
                ShiftID = 1,
                CoFrom = 4,
                CoTo = 5.5
            };
            Cat_ShiftItemEntity _shiftItemEntity2 = new Cat_ShiftItemEntity()
            {
                ShiftItemName = "Giao Ca Toi",
                ShiftID = 1,
                CoFrom = 9.5,
                CoTo = 10
            };
            listShiftItem.Add(_shiftItemEntity1);
            listShiftItem.Add(_shiftItemEntity2);
            #endregion

            #region listDayOff
            List<Cat_DayOffEntity> listDayOff = new List<Cat_DayOffEntity>();
            Cat_DayOffEntity _DayOffEntity1 = new Cat_DayOffEntity()
            {
                DateOff = DateTime.Parse("02-09-14"),
            };
            listDayOff.Add(_DayOffEntity1);
            #endregion

            #region listOvertimeType
            List<Cat_OvertimeTypeEntity> listOvertimeType = new List<Cat_OvertimeTypeEntity>();
            Cat_OvertimeTypeEntity _OvertimeTypeEntity1 = new Cat_OvertimeTypeEntity()
            {
                OvertimeTypeName = "OT - 3.0",
                OvertimeTypeCode = "E_HOLIDAY",
                Rate = 3,
                TaxRate = 1,
            };
            Cat_OvertimeTypeEntity _OvertimeTypeEntity5 = new Cat_OvertimeTypeEntity()
            {
                OvertimeTypeName = "OT - 3.5",
                OvertimeTypeCode = "E_HOLIDAY_NIGHTSHIFT",
                Rate = 3.5,
                TaxRate = 1,
            };
            Cat_OvertimeTypeEntity _OvertimeTypeEntity2 = new Cat_OvertimeTypeEntity()
            {
                OvertimeTypeName = "OT - 2.0",
                OvertimeTypeCode = "E_WEEKEND",
                Rate = 1,
                TaxRate = 1,
            };

            Cat_OvertimeTypeEntity _OvertimeTypeEntity3 = new Cat_OvertimeTypeEntity()
            {
                OvertimeTypeName = "OT - 1.5",
                OvertimeTypeCode = "E_WORKDAY",
                Rate = 1.5,
                TaxRate = 1,
            };

            Cat_OvertimeTypeEntity _OvertimeTypeEntity4 = new Cat_OvertimeTypeEntity()
            {
                OvertimeTypeName = "OT - 2.0",
                OvertimeTypeCode = "E_WORKDAY_NIGHTSHIFT",
                Rate = 2,
                TaxRate = 1,
            };

            listOvertimeType.Add(_OvertimeTypeEntity1);
            listOvertimeType.Add(_OvertimeTypeEntity2);
            listOvertimeType.Add(_OvertimeTypeEntity3);
            listOvertimeType.Add(_OvertimeTypeEntity4);
            #endregion

            #region listOvertimeInfoFillterAnalyze
            Att_OvertimeInfoFillterAnalyze _OvertimeInfoFillterAnalyzeEntity = new Att_OvertimeInfoFillterAnalyze()
            {
                isAllowGetOTOutterShift               = true,
                isAllowGetBeforeShift                 = false,
                isAllowGetAfterShift                  = true,
                isAllowGetInShift                     = false,
                isAllowGetTypeBaseOnActualDate        = true,
                isAllowGetTypeBaseOnBeginShift        = true,
                isAllowGetTypeBaseOnEndShift          = false,
                isAllowGetNightShift = true,
                // cắt h qua đêm 12h khuya
                isBreakMiddleNight = true,
                MininumOvertimeHour                   = 0.5
            };
            #endregion

            Att_OvertimeServices service = new Att_OvertimeServices();
            List<Att_OvertimeEntity> listResult = service.AnalyzeOvertime(listWorkDay, listShift, listShiftItem, listDayOff, listOvertimeType, _OvertimeInfoFillterAnalyzeEntity);

        }
    }
}
