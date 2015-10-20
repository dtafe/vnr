using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;

namespace HRM.Business.Category.Domain.NUnitTest
{
    [TestFixture]
    public class Sys_SettingServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetSetting()
        {
            Sys_SettingServices service = new Sys_SettingServices();
            int count = service.GetSetting().Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdSetting()
        {
            Sys_SettingServices service = new Sys_SettingServices();
            var model = new Sys_Setting { Id = 2 };
            var result = service.GetByIdSetting(model.Id) as Sys_Setting;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.Name + " | " + result.Value + " | " + result.ModuleId);
        }

        [Test]
        public void NUnit_Domain_AddSetting()
        {
            Sys_SettingServices service = new Sys_SettingServices();
            bool result = true;
            int rs = 0;
            for (int i = 0; i < 10; i++)
            {
                var model = new Sys_Setting
                {
                    Name = "Name Test" + i,
                    Value = "value " + i,
                    ModuleId =  i
                };
                result = service.AddSetting(model);
                if (result == true)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Create >>> " + model.Id + " | " + model.Name + " | " + model.Value + " | " + model.ModuleId);
                }
            }
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateSetting()
        {
            Sys_SettingServices service = new Sys_SettingServices();
            var model = new Sys_Setting
            {
                Id = 3,
                Name = "Update Name Test",
                Value = "Update tt"
            };
            bool result = service.UpdateSetting(model);
            NUnit.Framework.Assert.IsTrue(result);
            if (result == true)
            {
                Console.WriteLine("==>>> Result After Update:  " + model.Id + " | " + model.Name + " | " + model.Value + " | " + model.ModuleId);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteSetting()
        {
            Sys_SettingServices service = new Sys_SettingServices();
            bool result = true;
            int rs = 0;
            for (int i = 3; i <= 5; i++)
            {
                var model = new Sys_Setting { Id = i };
                result = service.DeleteSetting(model.Id);
                if (result == true)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        [Test]
        public void NUnit_Domain_DeleteEternitySetting()
        {
            Sys_SettingServices service = new Sys_SettingServices();
            bool result = true;
            int rs = 0;
            for (int i = 4; i <= 6; i++)
            {
                var model = new Sys_Setting { Id = i };
                result = service.DeleteEternity(model.Id);
                if (result == true)
                {
                    rs += 1;
                    Console.Write(" Process Success >>> DeleteEternity >>> " + i);
                }
            }
            Console.WriteLine("Total success record: " + rs);
            //NUnit.Framework.Assert.IsTrue(result);
        }
    }
}
